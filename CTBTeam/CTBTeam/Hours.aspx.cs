using System.Data.SqlClient;

using Date = System.DateTime;

using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.Collections;
using System.Collections.Generic;

namespace CTBTeam {
	public partial class Hours : SuperPage {
		private SqlConnection objConn;
		private DataTable projectData, projectHoursData, vehicleHoursData, employeesData, vehiclesData, datesData;
		private enum DATA_TYPE { VEHICLE, PROJECT };

		protected void Page_Load(object sender, EventArgs e) {
			Session["Alna_num"] = 173017;
			Session["Name"] = "Anthony Hewins";
			Session["Full_time"] = false;

			if (Session["Alna_num"] == null) {
				redirectSafely("~/Login");
				return;
			}

			objConn = openDBConnection();

			if (Session["Date"] == null)
				initDate(objConn);

			getDate();
			getData();

			if (!IsPostBack) {
				ddlInit();
			}

			populateTables();
			populateDataPercentage();
		}

		private void getDate() {
			Date date = (Date)Session["Date"];
			lblWeekOf.Text = "Week of " + date.Month + "/" + date.Day + "/" + date.Year;
		}

		private void getData() {
			objConn.Open();

			//TODO: make the employees queries only one query and seperate them by Full_time
			if (!chkInactive.Checked) {
				employeesData = getDataTable("select Alna_num, Name, Full_time from Employees where Active=@value1 order by Full_time;", true, objConn);
				projectData = getDataTable("select ID, Name, Category from Projects where Active=@value1", true, objConn);
				vehiclesData = getDataTable("select ID, Name from Vehicles where Active=@value1", true, objConn);
			}
			else {
				employeesData = getDataTable("select Alna_num, Name, Full_time from Employees order by Full_time", true, objConn);
				projectData = getDataTable("select ID, Name, Categories from Projects", null, objConn);
				vehiclesData = getDataTable("select ID, Name from Vehicles;", null, objConn);
			}

			//Everything else
			projectHoursData = getDataTable("select ID, Alna_num, Proj_ID, Hours_worked from ProjectHours where Date_ID=@value1", Session["Date_ID"], objConn);
			vehicleHoursData = getDataTable("select ID, Alna_num, Vehicle_ID, Hours_worked from VehicleHours where Date_ID=@value1", Session["Date_ID"], objConn);
			datesData = getDataTable("select Dates from Dates order by ID desc", null, objConn);
			objConn.Close();
		}

		private void ddlInit() {
			ddlselectWeek.Items.Clear();
			ddlProjects.Items.Clear();
			ddlVehicles.Items.Clear();

			foreach (DataRow d in datesData.Rows)
				ddlselectWeek.Items.Add(((Date)d[0]).ToShortDateString());

			Hashtable h = new Hashtable();

			int id; string name;
			foreach (DataRow r in projectData.Rows) {
				id = (int)r[0];
				name = r[1].ToString();
				ddlProjects.Items.Add(name);
				h.Add(id, name);
			}

			int alna = (int)Session["Alna_num"];
			foreach (DataRow d in projectHoursData.Rows) {
				if (alna == (int)d[1]) {
					ddlWorkedHours.Items.Add("P_ID#" + d[0].ToString() + ": worked " + d[3] + " hours on " + h[d[2]]);
				}
			}

			hoursUpdate();
			if ((bool)Session["Full_time"]) return;

			h = new Hashtable();
			foreach (DataRow r in vehiclesData.Rows) {
				id = (int)r[0];
				name = r[1].ToString();
				ddlVehicles.Items.Add(name);
				h.Add(id, name);
			}

			foreach (DataRow d in vehicleHoursData.Rows) {
				if (alna == (int)d[1]) {
					ddlWorkedHours.Items.Add("V_ID#" + d[0].ToString() + ": " + d[3] + " hrs on " + h[d[2]]);
				}
			}

			vehicleHoursTerns.Visible = true;
			ddlVehicles.Visible = true;
			ddlHoursVehicles.Visible = true;
			btnSubmitVehicles.Visible = true;

			objConn.Close();
		}

		protected void htmlEvent(object sender, EventArgs e) {
			if (sender.Equals(btnselectWeek)) {
				if (!Date.TryParse(ddlselectWeek.SelectedValue, out Date selection)) {
					throwJSAlert("Something went wrong. try again.");
					return;
				}
				Session["Date"] = selection;
				objConn.Open();
				SqlCommand cmd = new SqlCommand("select ID from Dates where Dates=@value1", objConn);
				cmd.Parameters.AddWithValue("@value1", selection);
				SqlDataReader reader = cmd.ExecuteReader();
				reader.Read();
				Session["Date_ID"] = (int)reader.GetValue(0);
				objConn.Close();
				redirectSafely("~/Hours");
			}
			else if (sender.Equals(btnSubmitPercent)) {
				if (insertRecord(ddlProjects.SelectedValue, ddlHours.SelectedValue, DATA_TYPE.PROJECT))
					redirectSafely("~/Hours");
			}
			else if (sender.Equals(btnSubmitVehicles)) {
				if (insertRecord(ddlVehicles.SelectedValue, ddlHoursVehicles.SelectedValue, DATA_TYPE.VEHICLE))
					redirectSafely("~/Hours");
			}
			else if (sender.Equals(btnDelete)) {
				if (!txtDelete.Text.Equals("YES")) {
					throwJSAlert("You must exactly type YES to delete all your records for the week. No extra whitespace, all caps.");
					return;
				}

				string selection = ddlWorkedHours.SelectedValue;
				if (string.IsNullOrEmpty(selection)) {
					throwJSAlert("Don't try and hack the system! What are you doing?");
					return;
				}

				string table = selection.Substring(0, 1).Equals("V") && !(bool)Session["Full_time"] ? "VehicleHours" : "ProjectHours";

				int startIndex = selection.IndexOf("#") + 1, endIndex = selection.IndexOf(":");
				selection = selection.Substring(startIndex, endIndex - startIndex);
				if (!int.TryParse(selection, out int id)) {
					throwJSAlert("Don't try and hack the system! What are you doing?");
					return;
				}

				objConn.Open();
				object[] o = { Session["Alna_num"], id };
				executeVoidSQLQuery("delete from " + table + " where Alna_num=@value1 and ID=@value2", o, objConn);
				objConn.Close();
				redirectSafely("~/Hours");
			}
			else {
				writeStackTrace("Btn not implemented", new ArgumentException("The button that called this function had no onclick implementation implemented"));
			}
		}

		//Listener for clicks on the arrow buttons below the tables
		/*
		protected void Arrow_Button_Clicked(object sender, EventArgs e) {
			string s;
			DATA_TYPE type;
			bool increment;
			//This entire if-else block figures out what arrow button was pressed
			//(referring to the arrow buttons below each table)
			if (sender.Equals(btnProjectPrevious)) {
				s = "ProjectCol";
				type = DATA_TYPE.PROJECT;
				increment = false;
			}
			else if (sender.Equals(btnProjectNext)) {
				s = "ProjectCol";
				type = DATA_TYPE.PROJECT;
				increment = true;
			}
			else if (sender.Equals(btnVehiclePrevious)) {
				s = "VehicleCol";
				type = DATA_TYPE.VEHICLE;
				increment = false;
			}
			else if (sender.Equals(btnVehicleNext)) {
				s = "VehicleCol";
				type = DATA_TYPE.VEHICLE;
				increment = true;
			}
			else {
				writeStackTrace("Sender object in Arrow_Button_Clicked had unexpected identity", new ArgumentException(sender.ToString()));
				return;
			}

			//Based on what button was pressed we perform this logic on the session.
			//The session holds the cookie for what table range of the tables the user
			//wants to view. The physical value is an int value that discriminates what rows to grab from
			//the table. We don't allow the users to have an offset above 18, because then they would view nothing.
			//TODO: make the hardcoded value the length of the table minus 5.
			if (increment) {
				Session[s] = (int)Session[s] + 6;
				if ((int)Session[s] > 18)
					Session[s] = 18;
			}
			else {
				Session[s] = (int)Session[s] - 6;
				if ((int)Session[s] < 1)
					Session[s] = 1;
			}
		}*/

		private bool insertRecord(string projectOrVehicle, string hoursSpent, DATA_TYPE type) {
			hoursSpent = hoursSpent.Substring(0, 4).Trim().Replace("%", "").Replace("-", "");
			if (!decimal.TryParse(hoursSpent, out decimal decHours)) {
				throwJSAlert("Error in hours selection. try again");
				return false;
			}

			int hours = (int)(40 * (decHours / 100));

			string table;
			string column;
			DataTable tableToUpdate;
			switch (type) {
				case DATA_TYPE.PROJECT:
					table = "ProjectHours";
					column = "Proj_ID";
					tableToUpdate = projectData;
					break;
				case DATA_TYPE.VEHICLE:
					table = "VehicleHours";
					column = "Vehicle_ID";
					tableToUpdate = vehiclesData;
					break;
				default:
					new NotImplementedException("Method has not been implemented");
					return false;
			}

			int projOrVehicleID = -1;
			foreach (DataRow d in tableToUpdate.Rows)
				if (d[1].Equals(projectOrVehicle)) {
					projOrVehicleID = (int)d[0];
					break;
				}

			if (projOrVehicleID == -1) {
				throwJSAlert("Project/vehicle does not exist");
				return false;
			}

			try {
				objConn.Open();
				object[] o = { Session["Alna_num"], projOrVehicleID, Session["Date_ID"] };
				SqlDataReader reader = getReader("select ID, Hours_worked from " + table + " where Alna_num=@value1 and " + column + "=@value2 and Date_ID=@value3", o, objConn);
				if (reader == null) return false;

				if (reader.HasRows) {
					reader.Read();
					int hoursWorked = reader.GetInt32(1);
					int otherRecordID = reader.GetInt32(0);
					reader.Close();
					executeVoidSQLQuery("delete from " + table + " where ID=@value1", otherRecordID, objConn);
					hours += hoursWorked;
				}
				else {
					reader.Close();
				}

				object[] o2 = { o[0], projOrVehicleID, hours, o[2] };
				executeVoidSQLQuery("insert into " + table + " values(@value1, @value2, @value3, @value4)", o2, objConn);
				objConn.Close();
			}
			catch (Exception ex) {
				throwJSAlert("Error connecting to database, check network connection");
				writeStackTrace("Hours Submit", ex);
				return false;
			}
			return true;
		}

		private void populateDataPercentage() {
			if (projectHoursData.Rows.Count == 0)
				return;
			int numEmployees = employeesData.Rows.Count;
			double[] projectHours = new double[4];
			int totalHours = 0;

			Dictionary<int, int> h = new Dictionary<int, int>();
			foreach (DataRow d in projectData.Rows) {
				switch (d[2]) {
					case "A":
						h.Add((int)d[0], 0);
						break;
					case "C":
						h.Add((int)d[0], 2);
						break;
					case "D":
						h.Add((int)d[0], 3);
						break;
					default:
						h.Add((int)d[0], 1);
						break;
				}
			}

			foreach (DataRow d in projectHoursData.Rows) {
				totalHours += (int)d[3];
				projectHours[h[(int)d[2]]] += (int)d[3];
			}

			string[] category = { "A", "B", "C", "D" };
			DataTable table = new DataTable();

			for (int i = 0; i < category.Length; i++) {
				table.Columns.Add(category[i], typeof(double));
				projectHours[i] /= totalHours;
			}

			chartPercent.Series[0].Points.DataBindXY(category, projectHours);
			chartPercent.Series[0].BorderWidth = 10;
			chartPercent.Series[0].ChartType = SeriesChartType.Pie;

			string text = "";
			foreach (Series charts in chartPercent.Series) {
				foreach (DataPoint point in charts.Points) {
					switch (point.AxisLabel) {
						case "A":
							point.Color = System.Drawing.Color.Aqua;
							text = "Advance Dev";
							break;
						case "B":
							point.Color = System.Drawing.Color.SpringGreen;
							text = "Time Off";
							break;
						case "C":
							point.Color = System.Drawing.Color.Salmon;
							text = "Production Dev (Auto)";
							break;
						case "D":
							point.Color = System.Drawing.Color.Violet;
							text = "Design in Market (Non-Auto)";
							break;
					}
					point.Label = string.Format("{0:P} - {1}", point.YValues[0], point.AxisLabel);
					point.LegendText = string.Format("{1} - " + text + "", point.YValues[0], point.AxisLabel);
				}
			}

			lblTotalHours.Text = "Hours: " + totalHours + " / " + (40 * numEmployees);
		}

		private void populateTables() {
			/*
			 * Need to put the partTimeEmployee/vehicle records into gridview.
			 * Due to really annoying limitations of SQL and C#, this is
			 * hard to do. Performance is also something I wanted to keep in mind because
			 * network latency and many SQL queries is an issue with this page more than any other.
			 * 
			 * We need this:
			 * +---------+---------+---------
			 * |theName  |project1 |moreProj  ...
			 * +---------+---------+---------
			 * |employee1|# of hrs | # of hrs ...
			 * +---------+---------+---------
			 * |employee2|# of hrs | # of hrs ...
			 * +---------+---------+---------
			 * |employee3|# of hrs | # of hrs ...
			 * +---------+---------+---------
			 * |employee4|# of hrs | # of hrs ...
			 * +---------+---------+---------
			 * You might think "We could do this in SQL with complicated joins", but you can't because it requires using rows in the Projects tables as the columns.
			 * Sql doesn't allow that, so we do it in C#, but it's a pain and potentially slow; BUT since the # of hours cells can be empty if the hours worked are 0,
			 * we can pull off ~Omega(2*#ofEmployees + #ofProjects + #ofVehicles), which is pretty damn good (still O(n^2), but this is 2D though, what can you expect)
			 * 
			 * 
			 * 
			 * Here's how it works:
			 * Step 1: take all the projects and put them in two things: hash table and the DataTable, as columns			 
			 * +------+------+-----+
			 * |name  |proj1 |proj2|	HashTable1(ProjID1 -> proj1'sIndexInTable (which is 1), ProjID2 -> proj2'sIndexInTable (which is 2), ...)
			 * +------+------+-----+
			 * 
			 * Step 2: This was the annoying part thanks to C#. The next intuitive step would be to start populating rows, but if we do that,
			 * then we cant edit them. So what I did instead was make a List<DataRow> so they can be edited. While I put them in there, I also put them in their own Hashtable.
			 * 
			 * The Datatable
			 * +------+------+-----+
			 * |name  | proj1|proj2|	HashTable1(ProjID1 -> proj1'sIndexInTable (which is 1), ProjID2 -> proj2'sIndexInTable (which is 2), ...)
			 * +------+------+-----+	HashTable2(
			 * 
			 * The List<DataRow>
			 * 
			 * List[0] = DataRow(Name: "Anthony Hewins", proj1: null, proj2: null, ...)
			 * List[1] = DataRow(Name: "Austin Danaj", proj1: null, proj2: null, ...)
			 * etc.
			 * 
			 * Step 3: using the hashtable, finally fill the DataTable:
			 * 
			 * Pretend Austin worked 3 hours for proj2. It would go like this
			 * 
			 * foreach DataRow in ProjectHours
			 *	 row# = Hashtable1.getWhatRowThisAlnaNumberIs(alna_num_supplied_from_ProjectHoursTable) //Remember: hash table takes the employee Alna and returns what row they are in the List
			 *	 col# = Hashtable2.getWhatColThisProjectIs(proj_ID_supplied_from_ProjectHoursTable)
			 *	 tempDatatable[row#][col#] = hours_spent_on_project_supplied_from_ProjectHoursTable
			 * 
			 *																  col#
			 *																	|
			 *																	V
			 * 
			 *			List[0] = DataRow(Name: "Anthony Hewins", proj1: null, proj2: null, ...)
			 *	row#->	List[1] = DataRow(Name: "Austin Danaj", proj1: null, proj2: 3, ...)
			 *	
			 *	
			 *	When done, the whole thing is filled.
			 *
			 *
			 * Step 4: Now the easy part, forall datarows in the List, add them to the DataTable
			 * Step 5: bind the data to the gridview at the very end
			 * Step 6: repeat for vehicles
			 */

			DataRow temp;
			DataTable tempProject = new DataTable();
			DataTable tempVehicle = new DataTable();
			Dictionary<int, int> employeeHashTable = new Dictionary<int, int>();
			Dictionary<int, int> projHashTable	   = new Dictionary<int, int>();		//I had to make 3 separate hash tables because there might be collisions
			Dictionary<int, int> vehicleHashTable  = new Dictionary<int, int>();		//with primary keys (all the PKs are autoincrements except alnas)

			tempProject.Columns.Add("Name");
			tempVehicle.Columns.Add("Name");

			int i = 0;
			foreach (DataRow d in projectData.Rows) {			
				projHashTable.Add((int)d[0], i);
				tempProject.Columns.Add((string)d[1]);
				i++;
			}
			i = 0;
			foreach (DataRow d in vehiclesData.Rows) {
				vehicleHashTable.Add((int)d[0], i);
				tempVehicle.Columns.Add((string)d[1]);
				i++;
			}
			i = 0;
			List<DataRow> projMatrix = new List<DataRow>();
			List<DataRow> vehicleMatrix = new List<DataRow>();
			foreach (DataRow d in employeesData.Rows) {
				if ((bool)d[2])                                 //<- Since we only include interns, we stop on full timers. We can do this because i used ORDER BY fulltime, so all the
					break;										//   part timers are first. Micro optimization for this part of code
				employeeHashTable.Add((int)d[0], i);
				temp = tempProject.NewRow();
				temp["Name"] = d[1];
				projMatrix.Add(temp);
				temp = tempVehicle.NewRow();
				temp["Name"] = d[1];
				vehicleMatrix.Add(temp);
				i++;
			}
			int whatCol, whatRow;

			foreach(DataRow d in projectHoursData.Rows) {
				int alna = (int) d[1];
				if (!employeeHashTable.ContainsKey(alna))	//We skip full time employees since they will not appear in the Hashtable
					continue;
				whatCol = projHashTable[(int)d[2]];
				whatRow = employeeHashTable[alna];
				projMatrix[whatRow][whatCol] = d[3];
			}

			foreach (DataRow d in vehicleHoursData.Rows) {
				whatCol = vehicleHashTable[(int)d[2]];
				whatRow = employeeHashTable[(int)d[1]];
				vehicleMatrix[whatRow][whatCol] = d[3];
			}

			foreach (DataRow d in projMatrix)
				tempProject.Rows.Add(d);

			foreach (DataRow d in vehicleMatrix)
				tempVehicle.Rows.Add(d);

			dgvProject.DataSource = tempProject;
			dgvProject.DataBind();
			dgvCars.DataSource = tempVehicle;
			dgvCars.DataBind();
		}

		private void hoursUpdate() {
			int hoursWorked = 0;

			Lambda howMuchHoursWorked = new Lambda(delegate (object o) {
				if (!(o is DataTable))
					return;
				DataTable temp = (DataTable)o;
				int session = (int)Session["Alna_num"];
				foreach (DataRow d in temp.Rows) {
					if ((int)d[1] == session)
						hoursWorked += (int)d[3];
				}
			});

			Lambda addDDLoptions = new Lambda(delegate (object o) {
				if (!(o is DropDownList))
					return;
				DropDownList temp = (DropDownList)o;
				float hoursSpent;
				for (int i = 1; i <= 40; i++) {
					if (i + hoursWorked > 40)
						break;
					hoursSpent = ((float)i / 40) * 100;
					temp.Items.Add("" + hoursSpent + "% -- " + i + " hours");
				}
			});

			ddlHours.Items.Add("--Select A Percent (Out of 40 hrs)--");
			howMuchHoursWorked(projectHoursData);
			addDDLoptions(ddlHours);

			if ((bool)Session["Full_time"]) return;

			hoursWorked = 0;
			ddlHoursVehicles.Items.Add("--Select A Percent (Out of 40 hrs)--");
			howMuchHoursWorked(vehicleHoursData);
			addDDLoptions(ddlHoursVehicles);

			lblUserHours.Text = "Logged " + hoursWorked + "/40";
		}
	}
}