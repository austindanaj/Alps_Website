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
		private DataTable projectData, projectHoursData, vehicleHoursData, partTimeEmployeeData, fullTimeEmployeeData, vehiclesData, datesData;
		private enum DATA_TYPE { VEHICLE, PROJECT };

		protected void Page_Load(object sender, EventArgs e) {
			objConn = openDBConnection();

			if (!IsPostBack) {
				sessionInits();
			}

			getDate();
			getData();

			if (!IsPostBack) {
				ddlInit();
			}
			
			populateTables((string) Session["Cols"]);
			populateDataPercentage();
		}

		private void sessionInits() {
			//SCAFFOLD for testing purposes
			if (Session["Alna_num"] == null) {
				Session["Alna_num"] = 173017;
				Session["Name"] = "Anthony Hewins";
				Session["Full_time"] = false;
			}

			if (Session["Alna_num"] == null)
				redirectSafely("~/Login");

			Session["Cols"] = ddlColNum.SelectedValue == null ? "25" : ddlColNum.SelectedValue;
				
			if (Session["Date"] == null) {
				initDate();
			}
		}

		private void getDate() {
			Date date = (Date)Session["Date"];
			lblWeekOf.Text = "Week of " + date.Month + "/" + date.Day + "/" + date.Year;
		}

		private void initDate() {
			objConn.Open();
			SqlDataReader reader = new SqlCommand("select top 1 Dates, ID from Dates order by ID DESC;", objConn).ExecuteReader();
			reader.Read();
			Date date = (Date)reader.GetValue(0);
			int id = (int)reader.GetValue(1);
			reader.Close();
			if (Date.Today > date.AddDays(6)) {
				date = date.AddDays(7);
				while (Date.Today > date.AddDays(6))
					date = date.AddDays(7);

				string sqlDateString = date.Year + "-" + date.Month + "-" + date.Day;
				executeVoidSQLQuery("insert into Dates (Dates.[Dates]) values (@value1)", sqlDateString, objConn);
				reader = getReader("select top 1 ID, Dates from Dates order by ID desc", null, objConn);

				reader.Read();
				Session["Date_ID"] = (int) reader.GetValue(0);
				Session["Date"] = (Date) reader.GetValue(1);
				reader.Close();
			} else {
				Session["Date"] = date;
				Session["Date_ID"] = id;
			}

			objConn.Close();
		}

		private void getData() {
			objConn.Open();

			//TODO: make the employees queries only one query and seperate them by Full_time
			if (!chkInactive.Checked) {
				object[] o = { true, false };
				partTimeEmployeeData = getDataTable("select Alna_num, Name from Employees where Active=@value1 and Full_time=@value2;", o, objConn);
				fullTimeEmployeeData = getDataTable("select Alna_num, Name from Employees where Active=@value1 and Full_time=@value1;", true, objConn);
				projectData = getDataTable("select ID, Name, Category from Projects where Active=@value1", true, objConn);
				vehiclesData = getDataTable("select ID, Name from Vehicles where Active=@value1", true, objConn);
			}
			else {
				partTimeEmployeeData = getDataTable("select Alna_num, Name from Employees where Full_time=@value1", false, objConn);
				fullTimeEmployeeData = getDataTable("select Alna_num, Name from Employees where Full_time=@value1", true, objConn);
				projectData = getDataTable("select ID, Name, Categories from Projects", null, objConn);
				vehiclesData = getDataTable("select ID, Name from Vehicles;", null, objConn);
			}

			//Everything else
			projectHoursData = getDataTable("select Alna_num, Proj_ID, Hours_worked from ProjectHours where Date_ID=@value1", Session["Date_ID"], objConn);
			vehicleHoursData = getDataTable("select Alna_num, Vehicle_ID, Hours_worked from VehicleHours where Date_ID=@value1", Session["Date_ID"], objConn);
			datesData = getDataTable("select Dates from Dates order by ID desc", null, objConn);
			objConn.Close();
		}

		private void ddlInit() {
			ddlselectWeek.Items.Clear();
			ddlProjects.Items.Clear();
			ddlVehicles.Items.Clear();
			ddlColNum.Items.Clear();

			foreach (DataRow d in datesData.Rows)
				ddlselectWeek.Items.Add(((Date)d[0]).ToShortDateString());

			foreach (DataRow r in projectData.Rows)
				ddlProjects.Items.Add(r[1].ToString());

			foreach (DataRow r in vehiclesData.Rows)
				ddlVehicles.Items.Add(r[1].ToString());

			int maxColValue = ddlProjects.Items.Count > ddlVehicles.Items.Count ? ddlProjects.Items.Count : ddlVehicles.Items.Count;

			for (int i = 4; i <= maxColValue; i++)
				ddlColNum.Items.Add(""+i);

			ddlColNum.SelectedIndex = 2;

			hoursUpdate();
			if ((bool) Session["Full_time"]) return;
			
			vehicleHoursTerns.Visible = true;
			ddlVehicles.Visible = true;
			ddlHoursVehicles.Visible = true;
			btnSubmitVehicles.Visible = true;

			objConn.Close();
		}

		protected void htmlEvent(object sender, EventArgs e) {
			if (sender.Equals(btnselectWeek)) {
				if(!Date.TryParse(ddlselectWeek.SelectedValue, out Date selection)) {
					throwJSAlert("Something went wrong. try again.");
					return;
				}
				Session["Date"] = selection;
				objConn.Open();
				SqlCommand cmd = new SqlCommand("select ID from Dates where Dates=@value1", objConn);
				cmd.Parameters.AddWithValue("@value1", selection);
				SqlDataReader reader = cmd.ExecuteReader();
				reader.Read();
				Session["Date_ID"] = (int) reader.GetValue(0);
				objConn.Close();
				redirectSafely("~/Hours");
			}
			else if (sender.Equals(btnSubmitPercent)) {
				if (insertRecord(ddlProjects.SelectedValue, ddlHours.SelectedValue, DATA_TYPE.PROJECT))
					redirectSafely("~/Hours");
			} else if (sender.Equals(btnSubmitVehicles)) {
				if(insertRecord(ddlVehicles.SelectedValue, ddlHoursVehicles.SelectedValue, DATA_TYPE.VEHICLE))
					redirectSafely("~/Hours");
			} else if(sender.Equals(btnDelete)) {
				if(!txtDelete.Text.Equals("YES")) {
					throwJSAlert("You must exactly type YES to delete all your records for the week. No extra whitespace, all caps.");
					return;
				}
				objConn.Open();
				object[] o = { Session["Alna_num"], Session["Date_ID"] };
				executeVoidSQLQuery("delete from ProjectHours where Alna_num=@value1 and Date_ID=@value2", o, objConn);
				executeVoidSQLQuery("delete from VehicleHours where Alna_num=@value1 and Date_ID=@value2", o, objConn);
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

			int hours = (int) (40 * (decHours / 100));

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
				SqlDataReader reader = getReader("select ID, Hours_worked from " + table + " where Alna_num=@value1 and " + column + "=@value2 and Date_ID=@value3", o,objConn);
				
				if (reader.HasRows) {
					reader.Read();
					int hoursWorked = reader.GetInt32(1);
					int otherRecordID = reader.GetInt32(0);
					reader.Close();
					executeVoidSQLQuery("delete from " + table + " where ID=@value1", otherRecordID, objConn);
					hours += hoursWorked;
				} else {
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
			int numEmployees = fullTimeEmployeeData.Rows.Count + partTimeEmployeeData.Rows.Count;
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

			foreach(DataRow d in projectHoursData.Rows) {
				totalHours += (int)d[2];
				projectHours[h[(int) d[1]]] += (int) d[2];
			}

			string[] category = { "A", "B", "C", "D" };
			DataTable table = new DataTable();

			for (int i=0;i<category.Length;i++) {
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

			/*try {
				objConn.Open();
				SqlCommand objCount = new SqlCommand("SELECT DISTINCT Emp_Name FROM PercentageLog WHERE Log_Date=@date ORDER BY Emp_Name", objConn);
				SqlDataReader readerCount = objCount.ExecuteReader();
				int empCount = 0;
				while (readerCount.Read()) {
					empCount++;
				}
				SqlCommand objCmdSelect = new SqlCommand("SELECT * FROM PercentageLog WHERE Log_Date=@date ORDER BY Emp_Name", objConn);
				SqlDataAdapter objAdapter = new SqlDataAdapter();
				objAdapter.SelectCommand = objCmdSelect;
				DataSet objDataSet = new DataSet();
				objAdapter.Fill(objDataSet);
				objDataSet.Tables[0].Columns.RemoveAt(0);

				DataTable chartData = objDataSet.Tables[0];
				string[] XPointMember = { "A", "B", "C", "D" };
				double[] YPointMember = { 0, 0, 0, 0 };
				int[] numberPeople = { 0, 0, 0, 0 };
				double value = 0;
				double runningSum = 0;
				for (int i = 0; i < chartData.Rows.Count; i++) {
					value = (Convert.ToDouble(chartData.Rows[i]["Percentage"])) / 100;
					value *= 40;
					runningSum += value;
					switch (chartData.Rows[i]["Category"].ToString()) {
						case "A":
							YPointMember[0] += value;
							break;
						case "B":
							YPointMember[1] += value;
							break;
						case "C":
							YPointMember[2] += value;
							break;
						case "D":
							YPointMember[3] += value;
							break;
					}
				}
				YPointMember[0] = (YPointMember[0] / runningSum);
				YPointMember[1] = (YPointMember[1] / runningSum);
				YPointMember[2] = (YPointMember[2] / runningSum);
				YPointMember[3] = (YPointMember[3] / runningSum);

				chartPercent.Series[0].Points.DataBindXY(XPointMember, YPointMember);
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
				lblTotalHours.Text = "Hours: " + runningSum + " / " + (20 * 40);
				objConn.Close();

				// dgvCars.HeaderRow.Cells[0].Visible = false;
			}
			catch (Exception ex) {
				writeStackTrace("Populate Percent", ex);
			}*/
		}

		private void populateTables(string colNum) {
			/*
			 * Need to put the partTimeEmployee/vehicle records into gridview.
			 * Due to really annoying limitations of SQL and C#, this is
			 * hard to do.
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
			 * You might think "We could do this in SQL", but you can't.
			 * You can do it in C#, but it's a pain, and it's slow; BUT since the # of hours cells can be
			 * empty if the hours worked are 0, the best case in under quadratic time (the worst case is quadratic, but this is 2D though, what can you expect)
			 * 
			 * 
			 * Essentially we do this:
			 * Step 1: take all the projects and put them in two things: hash table and the DataTable, as columns			 
			 * +------+------+-----+
			 * |just  |colum |ns   |	HashTable1(ProjID1 -> just, ProjID2 -> colum, ...)
			 * +------+------+-----+				^comes from the database
			 * 
			 * Step 2: This was the annoying part thanks to C#. I wanted to take all the employees and put them in the DataTable
			 * but after I do that I can't edit any rows. That wasn't going to work. So I put them in the hash table and a DataRow array first
			 * 
			 * The Datatable
			 * +------+------+-----+
			 * |name  | proj1|proj2|	HashTable1(ProjID1 -> just, ProjID2 -> colum, ...)
			 * +------+------+-----+
			 * 
			 * The DataRow[]
			 * 
			 * DataRow[0] = DataRow(Alna_num: 2332523, Name: "Anthony Hewins")
			 * DataRow[1] = DataRow(Alna_num: 283094, Name: "Austin Danaj")
			 * etc.
			 * 
			 * Step 3: using the hashtable, finally fill the DataTable:
			 * 
			 * foreach DataRow in ProjectHours
			 *	 row# = Hashtable1.getWhatRowThisAlnaNumberIs(alna_num_supplied_from_ProjectHoursTable)
			 *	 col# = Hashtable2.getWhatColThisProjectIs(proj_ID_supplied_from_ProjectHoursTable)
			 *	 tempDatatable[row#][col#] = hours_spent_on_project_supplied_from_ProjectHoursTable
			 * 
			 *						 col#
			 *						  |
			 *						  V
			 *				+------+------+-----+
			 *				|Name  | proj1|proj2|
			 *				+------+------+-----+
			 *				|Anthon|empty |empty|
			 *				+------+------+-----+
			 *	row# --->	|Austin|   2  |empty|
			 *				+------+------+-----+
			 *  
			 * 
			 * Step 4: fill the datatable into the actual DataTable class (do a foreach loop over it)
			 * Step 5: bind the data to the gridview at the very end
			 * Step 6: repeat for vehicles
			 */
			
			if (!int.TryParse(colNum, out int requestedColNum)) {
				requestedColNum = 6;
			}

			int smallestThreshold = requestedColNum >= projectData.Rows.Count ? projectData.Rows.Count : requestedColNum;

			DataTable tempDataTable = new DataTable();		//Data table to temporarily store the table before the gridview
			Hashtable h= new Hashtable();					//Temp Hash table for projects and vehicles
			Hashtable employeeHashTable = new Hashtable();  //Hash table for employees (there doesn't need to be 3 but it's simpler this way)

			tempDataTable.Columns.Add("Name", typeof(string));

			int i;    //Scrap int to use throughout the method: we have to reuse it a lot
			string s;

			Lambda addColumns = new Lambda(delegate(object o) {
				if (!(o is DataTable))
					return;
				DataTable d = (DataTable)o;
				for (i = 0; i < smallestThreshold; i++) {       //Forall rows in projectData: 
					s = d.Rows[i][1].ToString();				//Get the name of the project
					h.Add(projectData.Rows[i][0], s);           //Add it to the hash table with the Proj_Id as the key
					tempDataTable.Columns.Add(s, typeof(int));  //Add it as a column to the temporary datatable. The column accepts integer values because we're talking about hours worked
				}
			});

			addColumns(projectData);

			i = 0;
			DataRow temp;

			//Why do we need this? Because C# wont let you edit rows after they're added.
			//So we have to put everything into an array first so we can add all the hours
			//then we insert into the datatable.
			DataRow[] tempTable = new DataRow[partTimeEmployeeData.Rows.Count]; 
			foreach (DataRow d in partTimeEmployeeData.Rows) {
				temp = tempDataTable.NewRow();	//Make a new row
				temp["Name"] = d[1];			//The name of the employee is stored in [1]
				employeeHashTable.Add(d[0], i);	//Add the employee's ID to the hashtable to quickly get what row its in later
				tempTable[i] = temp;			//Assign the new row to tempTable
				i++;
			}

			Lambda insertCells = new Lambda(delegate (object o) {
				if (!(o is DataTable))
					return;
				DataTable table = (DataTable)o;
				foreach (DataRow d in table.Rows) {
					if (!employeeHashTable.ContainsKey(d[0]))	//If they're a full time employee skip them
						continue;
					int whatRow = (int)employeeHashTable[d[0]]; //d[0] holds the alna number
					string whatCol = (string)h[d[1]]; //d[1] holds the Project ID
					if (whatCol == null)
						continue;
					tempTable[whatRow][whatCol] = d[2]; //d[2] holds the hours worked on the project
				}
			});

			insertCells(projectHoursData);

			foreach (DataRow d in tempTable)
				tempDataTable.Rows.Add(d);

			dgvProject.DataSource = tempDataTable;
			dgvProject.DataBind();

			//Done with projects, time to move onto the vehicles

			tempDataTable = new DataTable();
			h = new Hashtable();

			tempDataTable.Columns.Add("Name", typeof(string));
			smallestThreshold = requestedColNum >= vehiclesData.Rows.Count ? vehiclesData.Rows.Count : requestedColNum;

			addColumns(vehiclesData);

			i = 0;
			foreach (DataRow d in partTimeEmployeeData.Rows) {
				temp = tempDataTable.NewRow();
				temp["Name"] = d[1];
				tempTable[i] = temp;
				i++;
			}

			insertCells(vehicleHoursData);

			foreach (DataRow d in tempTable)
				tempDataTable.Rows.Add(d);

			dgvCars.DataSource = tempDataTable;
			dgvCars.DataBind();
		}

		private void hoursUpdate() {
			int hoursWorked = 0;

			Lambda howMuchHoursWorked = new Lambda(delegate (object o) {
				if (!(o is DataTable))
					return;
				DataTable temp = (DataTable) o;
				int session = (int)Session["Alna_num"];
				foreach (DataRow d in temp.Rows) {
					if ((int)d[0] == session)
						hoursWorked += (int) d[2];
				}
			});

			Lambda addDDLoptions = new Lambda(delegate(object o) {
				if (!(o is DropDownList))
					return;
				DropDownList temp = (DropDownList) o;
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
		}
	}
}