using System.Data.SqlClient;

using Date = System.DateTime;

using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.IO;

namespace CTBTeam {
	public partial class Hours : SuperPage {
		SqlConnection objConn;

		string userName;
		Date date;
		private enum DATA_TYPE { VEHICLE, PERCENT, PROJECT };

		//===================================================
		//PART 1: PAGE INITS
		//===================================================

		//Loads the page with .NET specific stuff
		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				getDate();
				populatePastWeekDropDown();
				populateNames();
				successDialog(successOrFail);
				//In the event that PageLoad class doens't load, we init
				//the session projectcol and vehiclecol cookies since they
				//are used right below.
				if (Session["ProjectCol"] == null)
					Session["ProjectCol"] = 1;
				if (Session["VehicleCol"] == null)
					Session["VehicleCol"] = 1;

				populateDataCars((int)Session["ProjectCol"]);
				populateDataProject((int)Session["VehicleCol"]);
				populateDataPercentage();

				if (Session["User"] != null) {
					userName = (string)Session["User"];
					btnSubmitCar.Visible = true;
					btnSubmitProject.Visible = true;
					btnSubmitPercent.Visible = true;
				}
				else {
					btnSubmitCar.Visible = false;
					btnSubmitProject.Visible = false;
					btnSubmitPercent.Visible = false;
				}
			}
		}

		//Gets the Monday of the current week from the Time-log.txt file.
		private void getDate() {
			objConn = openDBConnection();
			objConn.Open();
			SqlCommand cmd = new SqlCommand("SELECT MAX(Dates.[Dates]) FROM Dates", objConn);
			SqlDataReader readerPNames = cmd.ExecuteReader();
			readerPNames.Read();
			date = (Date) readerPNames.GetValue(0);
			if (Date.Today > date.AddDays(6))
				datechange();
			lblWeekOf.Text += date.Month + "/" + date.Day + "/" + date.Year;
		}

		//backs up the DB if we move into a new week
		private void datechange() {
			date = date.AddDays(7);

			//if we go on vacation it'll screw up. Add another 7.
			if (Date.Today > date.AddDays(6))
				datechange();
			executeVoidSQLQuery("insert into Dates (Dates.[Date]) values (@value1)", date, objConn);
		}

		//===================================================
		//PART 2: EVENT LISTENERS
		//===================================================

		//On click listener for all the ddls
		protected void ddlSelection(object sender, EventArgs e) {
			if (sender.Equals(ddlFullTimeNames)) {
				populateListProjectPercent();
			}
			else if (sender.Equals(ddlAllProjects)) {
				ddlPercentage.Items.Clear();
				ddlPercentage.Items.Add("--Select A Percent (Out of 40 hrs)--");
				for (int i = 5; i <= 100; i += 5) {
					ddlPercentage.Items.Add("" + i + "% -- " + (40 * ((float)i / 100)) + " hours");
				}
			}
			else if (sender.Equals(ddlNamesProject)) {
				populateListProjects();
			}
			else if (sender.Equals(ddlNamesCar)) {
				try {
					ddlCars.Items.Clear();
					ddlCars.Items.Add("--Select A Car--");

					objConn = openDBConnection();
					objConn.Open();
					SqlCommand objCmdSelect = new SqlCommand("SELECT Vehicles.[Name] FROM Vehicles ORDER BY Vehicles.[Name]", objConn);
					SqlDataReader reader = objCmdSelect.ExecuteReader();
					while (reader.Read()) {
						ddlCars.Items.Add(new ListItem(reader.GetString(0)));
					}
					objConn.Close();
				}
				catch (Exception ex) {
					writeStackTrace("Populate Vehicles", ex);
				}
			}
			else {
				writeStackTrace("Dropdown list object not implemented in code", new NotImplementedException(sender.ToString()));
			}
		}

		//On click listener for all submit buttons
		protected void buttonSelection(object sender, EventArgs e) {
			if (Session["User"] == null) {
				throwJSAlert("Log in before submitting");
				return;
			}
			if (sender.Equals(btnSubmitProject)) {
				if (ddlNamesProject.Text == "--Select A Name--") {
					throwJSAlert("Please Select A Name");
				}
				else if (ddlProjects.Text == "--Select A Project--") {
					throwJSAlert("Please Select A Project");
				}
				else {
					submitProjects();
				}
			}
			else if (sender.Equals(btnSubmitCar)) {
				if (ddlNamesCar.Text == "--Select A Name--") {
					throwJSAlert("Please Select A Name");
				}
				else if (ddlCars.Text == "--Select A Car--") {
					throwJSAlert("Please Select A car");
				}
				else {
					submitCars();
				}
			}
			else if (sender.Equals(btnSubmitPercent)) {
				if (ddlFullTimeNames.Text == "--Select A Name--") {
					throwJSAlert("Please Select A Name");
				}
				else if (ddlAllProjects.Text == "--Select A Project--") {
					throwJSAlert("Please Select A Project");
				}
				else if (ddlPercentage.Text == "--Select A Percent--") {
					throwJSAlert("Please Select A Percent");
				}
				else {
					submitPercent();
				}
			}
		}

		//Listener for table update events
		protected void tableUpdate(object sender, GridViewPageEventArgs e) {
			if (sender.Equals(dgvProject)) {
				populateDataProject((int)Session["ProjectCol"]);
				dgvProject.PageIndex = e.NewPageIndex;
				dgvProject.DataBind();
			}
			else if (sender.Equals(dgvCars)) {
				populateDataCars((int)Session["VehicleCol"]);
				dgvCars.PageIndex = e.NewPageIndex;
				dgvCars.DataBind();
			}
			else {
				writeStackTrace("On index update button not implemented", new NotImplementedException(sender.ToString()));
			}
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

			populateData((int)Session[s], type);
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

			populateDataCars((int)Session["VehicleCol"]);
		}

		//===================================================
		//PART 4: LIST/TABLE POPULATORS FOR THE HTML PAGE
		//===================================================

		private void populatePastWeekDropDown() {
			ddlselectWeek.Items.Add("Not implemented yet, so this doesn't work.");
		}

		private void populateListProjectPercent() {
			try {
				ddlAllProjects.Items.Clear();
				ddlAllProjects.Items.Add("--Select A Project--");

				objConn = openDBConnection();
				objConn.Open();
				SqlCommand objCmdSelect = new SqlCommand("SELECT Projects.[Name] FROM Projects ORDER BY Projects.[Name]", objConn);
				SqlDataReader reader = objCmdSelect.ExecuteReader();
				while (reader.Read()) {
					ddlAllProjects.Items.Add(new ListItem(reader.GetString(0)));
				}
				objConn.Close();
				chartPercent.EnableViewState = true;
			}
			catch (Exception ex) {
				writeStackTrace("Populate Project Percent", ex);
			}
		}

		private void populateNames() {
			try {
				ddlNamesProject.Items.Clear();
				ddlNamesCar.Items.Clear();
				ddlFullTimeNames.Items.Clear();

				string temp = "--Select A Name--";

				ddlNamesProject.Items.Add(temp);
				ddlNamesCar.Items.Add(temp);
				ddlFullTimeNames.Items.Add(temp);

				objConn = openDBConnection();
				objConn.Open();

				SqlCommand objCmdSelect = new SqlCommand("SELECT Employees.[Name] FROM Employees WHERE Full_time=@bool ORDER BY Employees.[Name]", objConn);
				objCmdSelect.Parameters.AddWithValue("@bool", false);

				SqlDataReader reader = objCmdSelect.ExecuteReader();
				while (reader.Read()) {
					ddlNamesProject.Items.Add(new ListItem(reader.GetString(0)));
					ddlNamesCar.Items.Add(new ListItem(reader.GetString(0)));
				}
				reader.Close();

				objCmdSelect = new SqlCommand("SELECT Employees.[Name] FROM Employees WHERE Full_time=@bool ORDER BY Employees.[Name]", objConn);
				objCmdSelect.Parameters.AddWithValue("@bool", true);
				reader = objCmdSelect.ExecuteReader();
				while (reader.Read()) {
					ddlFullTimeNames.Items.Add(new ListItem(reader.GetString(0)));
				}


				objConn.Close();
			}
			catch (Exception ex) {
				writeStackTrace("Populate Names", ex);
			}
		}

		private void populateListProjects() {
			try {
				ddlProjects.Items.Clear();
				ddlProjects.Items.Add("--Select A Project--");
				objConn = openDBConnection();
				objConn.Open();
				SqlCommand objCmdSelect = new SqlCommand("SELECT Projects.[Name] FROM Projects ORDER BY Projects.[Name]", objConn);
				SqlDataReader reader = objCmdSelect.ExecuteReader();
				while (reader.Read()) {
					ddlProjects.Items.Add(new ListItem(reader.GetString(0)));
				}
				objConn.Close();
			}
			catch (Exception ex) {
				writeStackTrace("Populate projects", ex);
			}
		}

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

		//Populates data in the tables.
		//populateDataCars and populateDataProject call this function
		//with different enums
		private void populateData(int startIndex, Hours.DATA_TYPE type) {
			objConn = openDBConnection();
			objConn.Open();
			SqlCommand cmd = new SqlCommand("select Employees.Name,TempProjectHours.Proj_ID, TempProjectHours.Hours_worked from Employees inner join TempProjectHours On TempProjectHours.Alna_num = Employees.Alna_num;", objConn);

			SqlDataReader reader = cmd.ExecuteReader();
			DataTable table = new DataTable();
			while (reader.Read()) {
				//table.Columns.Add(reader.GetValue());
			}
		}

		private void populateDataCars(int startIndex) {
			populateData(startIndex, DATA_TYPE.VEHICLE);
		}

		private void populateDataProject(int startIndex) {
			populateData(startIndex, DATA_TYPE.PROJECT);
		}

		//===================================================
		//PART 5: HELPER FUNCTIONS TO REDUCE REDUNDANT CODE
		//===================================================

		private object[] getList(CTBTeam.Hours.DATA_TYPE enumSwitch, SqlConnection objConn) {
			string s;

			switch (enumSwitch) {
				case DATA_TYPE.PROJECT:
					s = "SELECT Project From Projects ORDER BY ID";
					break;
				case DATA_TYPE.VEHICLE:
					s = "SELECT Vehicle From Cars ORDER BY ID";
					break;
				case DATA_TYPE.PERCENT:
					s = "SELECT Column_Name From PercentColumns order by ID";
					break;
				default:
					return null;
			}

			object[] o = new object[2];
			try {
				SqlDataReader reader = new SqlCommand(s, objConn).ExecuteReader();

				int counter = 0;
				s = ""; //Reassign s to reuse the string pointer, micro optimization.

				while (reader.Read()) {
					counter++;      /** Increment counter by 1 **/
					s += reader.GetValue(0).ToString() + ",";    //Add it to the list of things
				}

				o[0] = counter;
				o[1] = s;
			}
			catch (Exception e) {
				writeStackTrace("getList", e);
			}
			return o;
		}

		private void backUpToTxt(SqlDataReader reader, StreamWriter file, int count) {
			/** Loop through each row **/
			try {
				string text;
				while (reader.Read()) {
					text = "";     /** Reset the line to empty **/

					/** Loop through each row, starting at emp_name to end **/
					for (int i = 1; i <= count; i++) {
						text += reader.GetValue(i).ToString() + ",";      /** Get value from database and append to line **/
					}
					file.WriteLine(text);     /** write line to file **/
				}
				file.WriteLine();      /** write blank line to file **/
			}
			catch (Exception e) {
				writeStackTrace("backUpToTxt", e);
			}
		}
	}
}