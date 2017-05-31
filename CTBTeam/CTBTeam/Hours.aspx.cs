using System.Data.SqlClient;

using Date = System.DateTime;

using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace CTBTeam {
	public partial class Hours : SuperPage {
		private SqlConnection objConn;
		private Date date;
		private int dateID;
		private DataTable projectData;
		private DataTable projectHoursData;
		private DataTable vehicleHoursData;
		private DataTable partTimeEmployeeData;
		private DataTable fullTimeEmployeeData;
		private DataTable vehiclesData;

		private enum DATA_TYPE { VEHICLE, PERCENT, PROJECT };

		//===================================================
		//PART 1: PAGE INITS
		//===================================================

		//Loads the page with .NET specific stuff
		protected void Page_Load(object sender, EventArgs e) {
			objConn = openDBConnection();
			objConn.Open();
			if (!IsPostBack) {
				//SCAFFOLD for testing purposes
				if (Session["User"] == null) {
					Session["User"] = 173017;
					Session["Name"] = "Anthony Hewins";
					Session["Full_time"] = false;
				}

				if (Session["User"] == null)
					redirectSafely("~/Login");
				getDate();
				getData();
				ddlInit();
				successDialog(successOrFail);
				//In the event that PageLoad class doens't load, we init
				//the session projectcol and vehiclecol cookies since they
				//are used right below.
				if (Session["ProjectCol"] == null)
					Session["ProjectCol"] = 1;
				if (Session["VehicleCol"] == null)
					Session["VehicleCol"] = 1;

				//populateDataCars((int)Session["ProjectCol"]);
				//populateDataProject((int)Session["VehicleCol"]);
				//populateDataPercentage();
			}
			else {
				getData();
			}
			objConn.Close();
			populateTables();
		}

		private void getDate() {
			SqlDataReader reader;
			reader = new SqlCommand("select ID, Dates.Dates from Dates order by ID DESC;", objConn).ExecuteReader();
			reader.Read();
			dateID = reader.GetInt32(0);
			date = (Date)reader.GetValue(1);
			ddlselectWeek.Items.Add(date.ToShortDateString());
			while (reader.Read())
				ddlselectWeek.Items.Add(((Date)reader.GetValue(1)).ToShortDateString());

			if (Date.Today > date.AddDays(6)) {
				date = date.AddDays(7);
				while (Date.Today > date.AddDays(6))
					date = date.AddDays(7);
				executeVoidSQLQuery("insert into Dates (Dates.[Dates]) values (@value1)", date, objConn);

				SqlCommand cmd = new SqlCommand("select Dates from Dates where ID=@value1", objConn);
				cmd.Parameters.AddWithValue("@value1", (int)Session["Date"]);
				reader = cmd.ExecuteReader();
				reader.Read();
				date = (Date)reader.GetValue(0);
			}
			reader.Close();
			lblWeekOf.Text += date.Month + "/" + date.Day + "/" + date.Year;
		}

		//===================================================
		//PART 2: EVENT LISTENERS
		//===================================================

		private void getData() {

			//Employees
			//TODO: make it one sql query to speed up transaction time
			if (chkInactive.Checked) {
				object[] o = { true, false };
				partTimeEmployeeData = getDataTable("select Alna_num, Name from Employees where Active=@value1 and Full_time=@value2;", o, objConn);
				o[1] = true;
				fullTimeEmployeeData = getDataTable("select Alna_num, Name from Employees where Active=@value1 and Full_time=@value2;", o, objConn);
			}
			else {
				partTimeEmployeeData = getDataTable("select Alna_num, Name from Employees where Full_time=@value1", false, objConn);
				fullTimeEmployeeData = getDataTable("select Alna_num, Name from Employees where Full_time=@value1", true, objConn);
			}

			//Everything else
			projectHoursData = getDataTable("select Alna_num, Proj_ID, Hours_worked from ProjectHours where Date_ID=@value1", dateID, objConn);
			vehicleHoursData = getDataTable("select Alna_num, Vehicle_ID, Hours_worked from VehicleHours where Date_ID=@value1", dateID, objConn);
			projectData = getDataTable("select ID, Name from Projects where Active=@value1", true, objConn);
			vehiclesData = getDataTable("select ID, Name from Vehicles;", null, objConn);
			objConn.Close();
		}

		private void ddlInit() {
			ddlHours.Items.Add("--Select A Percent (Out of 40 hrs)--");

			foreach (DataRow r in projectData.Rows)
				ddlProjects.Items.Add(r[1].ToString());

			foreach (DataRow r in vehiclesData.Rows)
				ddlVehicles.Items.Add(r[0].ToString());

			//Init the % hours ddl
			if ((bool)Session["Full_time"]) {
				for (int i = 5; i <= 100; i += 5)
					ddlHours.Items.Add("" + i + "% -- " + (40 * ((float)i / 100)) + " hours");
				return;
			}

			ddlHoursVehicles.Items.Add("--Select A Percent (Out of 40 hrs)--");
			for (int i = 5; i <= 100; i += 5) {
				string s = "" + i + "% -- " + (40 * ((float)i / 100)) + " hours";
				ddlHours.Items.Add(s);
				ddlHoursVehicles.Items.Add(s);
			}

			vehicleHoursTerns.Visible = true;
			ddlVehicles.Visible = true;
			ddlHoursVehicles.Visible = true;
			btnSubmitVehicles.Visible = true;

			/*cmd = new SqlCommand("select Name from Vehicles where Active=1", objConn);
			reader = cmd.ExecuteReader();
			while(reader.Read())
				ddlVehicles.Items.Add(reader.GetString(0));*/

			objConn.Close();
		}

		protected void ddlSelection(object sender, EventArgs e) {
			if (sender.Equals(ddlProjects)) {
				employeesHoursFor(ddlProjects.SelectedValue, DATA_TYPE.PROJECT);
			}
			else if (sender.Equals(ddlVehicles)) {
				employeesHoursFor(ddlVehicles.SelectedValue, DATA_TYPE.VEHICLE);
			}
			else {
				throwJSAlert("DDL action not implemented");
				return;
			}
		}

		private void employeesHoursFor(string selection, Hours.DATA_TYPE type) {

		}

		protected void btnSelection(object sender, EventArgs e) {

		}

		//Listener for clicks on the arrow buttons below the tables
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
				if ((int)Session[s] > 18) {
					Session[s] = 18;
				}
			}
			else {
				Session[s] = (int)Session[s] - 6;
				if ((int)Session[s] < 1) {
					Session[s] = 1;
				}
			}
		}

		//===================================================
		//PART 3: DB UPDATER FUNCTIONS (very straightforward)
		//===================================================

		private void submitProjects() {
			try {

			}
			catch (Exception ex) {
				writeStackTrace("Hours Submit", ex);
			}
		}

		private void submitPercent() {
			try {
				Session["success?"] = true;
				redirectSafely("~/Hours");
			}
			catch (Exception ex) {
				writeStackTrace("Hours Submit", ex);
			}
		}

		private void submitCars() {
			try {

			}
			catch (Exception ex) {
				writeStackTrace("Hours Submit", ex);
			}
		}

		//===================================================
		//PART 4: LIST/TABLE POPULATORS FOR THE HTML PAGE
		//===================================================

		private void populateDataPercentage() {
			/*try {
				objConn = openDBConnection();
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

		private void populateTables() {
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
			 * |employee1|# of hrs | # of hrs ...
			 * +---------+---------+---------
			 * |employee1|# of hrs | # of hrs ...
			 * +---------+---------+---------
			 * |employee1|# of hrs | # of hrs ...
			 * +---------+---------+---------
			 * # of hours cells can be empty if the hours worked are 0, and using that fact we can do this
			 * process best case in under quadratic time (worst case is quadratic, but this is 2D though)
			 * 
			 * 
			 * Essentially we do this:
			 * Step 1: take all the projects and put them in two things: hash table and the DataTable, as columns			 
			 * +------+------+-----+
			 * |just  |colum |ns   |	HashTable1(ProjID1 -> just, ProjID2 -> colum, ...)
			 * +------+------+-----+				^comes from the database
			 * 
			 * Step 2: take all the employees and put them in two things: hash table and DataTable, as rows
			 * +------+------+-----+
			 * |name  | proj1|proj2|	HashTable1(ProjID1 -> just, ProjID2 -> colum, ...)
			 * +------+------+-----+
			 * |emp1  |empty |empty|	HashTable2(Alna_num1 -> emp1, Alna_num2 -> emp2, ...)
			 * +------+------+-----+
			 * |emp2  |empty |empty|
			 * +------+------+-----+
			 * 
			 * Step 3: using the hashtable, fill the temporary datatable (this is the most complicated by far):
			 * foreach DataRow in ProjectHours
			 *	 row# = Hashtable1.getWhatRowThisAlnaNumberIs(alna_num_supplied_from_ProjectHoursTable)
			 *	 col# = Hashtable2.getWhatColThisProjectIs(proj_ID_supplied_from_ProjectHoursTable)
			 *	 tempDatatable[row#][col#] = hours_spent_on_project_supplied_from_ProjectHoursTable
			 * 
			 * Step 4: fill the datatable into the actual DataTable class (foreach loop over it)
			 * Step 5: bind the data to the gridview at the very end
			 */


			DataTable projectHours = new DataTable();		//Data table to temporarily store the table before the gridview
			int i;											//Scrap int to use throughout the method
			Hashtable projectHashTable = new Hashtable();   //Hash table for projects
			Hashtable employeeHashTable = new Hashtable();  //Hash table for employees (there doesn't need to be two but it's simpler this way)

			//This variable holds how many columns the gridview will have. it can't be greater than the
			//Amount of projects there are so the ternary checks to make sure that doesnt happen
			int smallestThreshold = 6 > projectData.Rows.Count ? projectData.Rows.Count : 6;

			projectHours.Columns.Add("Name", typeof(string));


			string s; //Scrap var to hold project name
			for (i = 0; i < smallestThreshold; i++) {
				s = projectData.Rows[i][1].ToString();
				projectHashTable.Add(projectData.Rows[i][0], projectData.Rows[i][1]);
				projectHours.Columns.Add(projectData.Rows[i][1].ToString(), typeof(int));
			}

			i = 0;
			DataRow temp;
			DataRow[] tempTable = new DataRow[partTimeEmployeeData.Rows.Count];
			foreach (DataRow d in partTimeEmployeeData.Rows) {
				temp = projectHours.NewRow();
				temp["Name"] = d[1];
				employeeHashTable.Add(d[0], i);
				tempTable[i] = temp;
				i++;
			}

			foreach (DataRow d in projectHoursData.Rows) {
				int whatRow = (int)employeeHashTable[d[0]]; //d[0] holds the alna number
				string whatCol = (string)projectHashTable[d[1]]; //d[1] holds the Project ID
				tempTable[whatRow][whatCol] = d[2]; //d[2] holds the hours worked on the project
			}

			foreach (DataRow d in tempTable)
				projectHours.Rows.Add(d);

			dgvProject.DataSource = projectHours;
			dgvProject.DataBind();
			abstractPopulateTables2(vehiclesData, vehicleHoursData);
		}

		private void abstractPopulateTables(DataTable t) {
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
			 * |employee1|# of hrs | # of hrs ...
			 * +---------+---------+---------
			 * |employee1|# of hrs | # of hrs ...
			 * +---------+---------+---------
			 * |employee1|# of hrs | # of hrs ...
			 * +---------+---------+---------
			 * # of hours cells can be empty if the hours worked are 0, and using that fact we can do this
			 * process best case in under quadratic time (worst case is quadratic, but this is 2D though)
			 * 
			 * 
			 * Essentially we do this:
			 * Step 1: take all the projects and put them in two things: hash table and the DataTable, as columns			 
			 * +------+------+-----+
			 * |just  |colum |ns   |	HashTable1(ProjID1 -> just, ProjID2 -> colum, ...)
			 * +------+------+-----+				^comes from the database
			 * 
			 * Step 2: take all the employees and put them in two things: hash table and DataTable, as rows
			 * +------+------+-----+
			 * |name  | proj1|proj2|	HashTable1(ProjID1 -> just, ProjID2 -> colum, ...)
			 * +------+------+-----+
			 * |emp1  |empty |empty|	HashTable2(Alna_num1 -> emp1, Alna_num2 -> emp2, ...)
			 * +------+------+-----+
			 * |emp2  |empty |empty|
			 * +------+------+-----+
			 * 
			 * Step 3: using the hashtable, fill the temporary datatable (this is the most complicated by far):
			 * foreach DataRow in ProjectHours
			 *	 row# = Hashtable1.getWhatRowThisAlnaNumberIs(alna_num_supplied_from_ProjectHoursTable)
			 *	 col# = Hashtable2.getWhatColThisProjectIs(proj_ID_supplied_from_ProjectHoursTable)
			 *	 tempDatatable[row#][col#] = hours_spent_on_project_supplied_from_ProjectHoursTable
			 * 
			 * Step 4: fill the datatable into the actual DataTable class (foreach loop over it)
			 * Step 5: bind the data to the gridview at the very end
			 */


			DataTable p = new DataTable();				    //Data table to temporarily store the table before the gridview
			int i;                                          //Scrap int to use throughout the method
			Hashtable h = new Hashtable();					//Hash table for projects
			Hashtable employeeHashTable = new Hashtable();  //Hash table for employees (there doesn't need to be two but it's simpler this way)

			//This variable holds how many columns the gridview will have. it can't be greater than the
			//Amount of projects there are so the ternary checks to make sure that doesnt happen
			int smallestThreshold = 6 > t.Rows.Count ? t.Rows.Count : 6;

			p.Columns.Add("Name", typeof(string));


			string s; //Scrap var to hold project name
			for (i = 0; i < smallestThreshold; i++) {
				s = t.Rows[i][0].ToString();
				h.Add(t.Rows[i][0], t.Rows[i][0]);
				p.Columns.Add(t.Rows[i][0].ToString(), typeof(int));
			}

			i = 0;
			DataRow temp;
			DataRow[] tempTable = new DataRow[partTimeEmployeeData.Rows.Count];
			foreach (DataRow d in partTimeEmployeeData.Rows) {
				temp = p.NewRow();
				temp["Name"] = d[1];
				employeeHashTable.Add(d[0], i);
				tempTable[i] = temp;
				i++;
			}

			foreach (DataRow d in vehicleHoursData.Rows) {
				int whatRow = (int)employeeHashTable[d[0]]; //d[0] holds the alna number
				string whatCol = (string)h[d[1]]; //d[1] holds the Project ID
				tempTable[whatRow][whatCol] = d[2]; //d[2] holds the hours worked on the project
			}

			foreach (DataRow d in tempTable)
				p.Rows.Add(d);

			dgvCars.DataSource = p;
			dgvCars.DataBind();
		}

		private void abstractPopulateTables2(DataTable model, DataTable modelHours) {
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
			 * |employee1|# of hrs | # of hrs ...
			 * +---------+---------+---------
			 * |employee1|# of hrs | # of hrs ...
			 * +---------+---------+---------
			 * |employee1|# of hrs | # of hrs ...
			 * +---------+---------+---------
			 * # of hours cells can be empty if the hours worked are 0, and using that fact we can do this
			 * process best case in under quadratic time (worst case is quadratic, but this is 2D though)
			 * 
			 * 
			 * Essentially we do this:
			 * Step 1: take all the projects and put them in two things: hash table and the DataTable, as columns			 
			 * +------+------+-----+
			 * |just  |colum |ns   |	HashTable1(ProjID1 -> just, ProjID2 -> colum, ...)
			 * +------+------+-----+				^comes from the database
			 * 
			 * Step 2: take all the employees and put them in two things: hash table and DataTable, as rows
			 * +------+------+-----+
			 * |name  | proj1|proj2|	HashTable1(ProjID1 -> just, ProjID2 -> colum, ...)
			 * +------+------+-----+
			 * |emp1  |empty |empty|	HashTable2(Alna_num1 -> emp1, Alna_num2 -> emp2, ...)
			 * +------+------+-----+
			 * |emp2  |empty |empty|
			 * +------+------+-----+
			 * 
			 * Step 3: using the hashtable, fill the temporary datatable (this is the most complicated by far):
			 * foreach DataRow in ProjectHours
			 *	 row# = Hashtable1.getWhatRowThisAlnaNumberIs(alna_num_supplied_from_ProjectHoursTable)
			 *	 col# = Hashtable2.getWhatColThisProjectIs(proj_ID_supplied_from_ProjectHoursTable)
			 *	 tempDatatable[row#][col#] = hours_spent_on_project_supplied_from_ProjectHoursTable
			 * 
			 * Step 4: fill the datatable into the actual DataTable class (foreach loop over it)
			 * Step 5: bind the data to the gridview at the very end
			 */


			DataTable p = new DataTable();                  //Data table to temporarily store the table before the gridview
			int i;                                          //Scrap int to use throughout the method
			Hashtable h = new Hashtable();                  //Hash table for projects
			Hashtable employeeHashTable = new Hashtable();  //Hash table for employees (there doesn't need to be two but it's simpler this way)

			//This variable holds how many columns the gridview will have. it can't be greater than the
			//Amount of projects there are so the ternary checks to make sure that doesnt happen
			int smallestThreshold = 6 > model.Rows.Count ? model.Rows.Count : 6;

			p.Columns.Add("Name", typeof(string));


			string s; //Scrap var to hold project name
			for (i = 0; i < smallestThreshold; i++) {
				s = model.Rows[i][0].ToString();
				h.Add(model.Rows[i][0], model.Rows[i][0]);
				p.Columns.Add(model.Rows[i][0].ToString(), typeof(int));
			}

			i = 0;
			DataRow temp;
			DataRow[] tempTable = new DataRow[partTimeEmployeeData.Rows.Count];
			foreach (DataRow d in partTimeEmployeeData.Rows) {
				temp = p.NewRow();
				temp["Name"] = d[1];
				employeeHashTable.Add(d[0], i);
				tempTable[i] = temp;
				i++;
			}

			foreach (DataRow d in vehicleHoursData.Rows) {
				int whatRow = (int)employeeHashTable[d[0]]; //d[0] holds the alna number
				string whatCol = (string)h[d[1]]; //d[1] holds the Project ID
				tempTable[whatRow][whatCol] = d[2]; //d[2] holds the hours worked on the project
			}

			foreach (DataRow d in tempTable)
				p.Rows.Add(d);

			dgvCars.DataSource = p;
			dgvCars.DataBind();
		}
	}
}