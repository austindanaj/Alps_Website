using System.Data.OleDb;

using Date = System.DateTime;

using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.IO;

namespace CTBTeam {
	public partial class Hours : SuperPage {
		string userName;
		string date = "0/00/0000";
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
					Session["ProjectCol"] = 0;
				if (Session["VehicleCol"] == null)
					Session["VehicleCol"] = 0;

				populateDataCars((int)Session["ProjectCol"]);
				populateDataProject((int)Session["VehicleCol"]);
				populateDataPercentage();

				if (!string.IsNullOrEmpty((string)Session["User"])) {
					userName = (string)Session["User"];
					btnSubmitCar.Visible = true;
					btnSubmitProject.Visible = true;
					btnSubmitPercent.Visible = true;
				} else {
					btnSubmitCar.Visible = false;
					btnSubmitProject.Visible = false;
					btnSubmitPercent.Visible = false;
				}
			}
		}

		//Gets the Monday of the current week from the Time-log.txt file.
		private void getDate() {
			try {
				string[] arrLine = System.IO.File.ReadAllLines(@"" + Server.MapPath("~/Logs/TimeLog/Time-log.txt"));
				date = arrLine[arrLine.Length - 1];
				if (Date.Today.AddDays(-6) > Date.Parse(date)) {
					datechange();
				}
				lblWeekOf.Text = "Week Of: " + date;
			}
			catch (Exception ex) {
				writeStackTrace("getDate", ex);
				lblWeekOf.Text = "Week Of: " + Date.Today.Day + "/" + Date.Today.Month + "/" + Date.Today.Year + " (Error reading Time-log.txt)";
			}
		}

		//backs up the DB if we move into a new week
		private void datechange() {
			try {
				OleDbConnection objConn = openDBConnection();
				objConn.Open();

				//Get the data: 2 element object array.
				//o[0] is counter, o[1] is the string list. Repeat for all tables.
				object[] o = getList(DATA_TYPE.PROJECT, objConn);
				int projectCount = (int)o[0];         /** Count of projects **/
				string projectList = (string)o[1];      /** List of projects **/

				o = getList(DATA_TYPE.VEHICLE, objConn);
				int carCount = (int)o[0];           /** Count of projects **/
				string carList = (string)o[1];        /** List of projects **/

				o = getList(DATA_TYPE.PERCENT, objConn);
				int percentCount = (int)o[0];
				string percentList = (string)o[1];        /** List of percent **/

				/** Command to get project hours header **/
				OleDbCommand fieldProjectNames = new OleDbCommand("SELECT * FROM ProjectHours", objConn);
				OleDbDataReader readerPNames = fieldProjectNames.ExecuteReader();

				var table = readerPNames.GetSchemaTable();      /** Set the table of project hours to variable **/
				var nameCol = table.Columns["ColumnName"];      /** Set the column name **/
				string headerRow = "";                          /** Header Row for Project Hours Log file **/

				/** 
				 * Loop through each row, if column is ID, dont add
				 * This loop populates the header row for project hours
				 **/
				foreach (DataRow row in table.Rows) {
					if (!row[nameCol].Equals("ID")) {
						headerRow += row[nameCol] + ",";
					}
				}

				/** Get the contents of file **/
				string[] arrLine = System.IO.File.ReadAllLines(@"" + Server.MapPath("~/Logs/TimeLog/Time-log.txt"));

				/** Replace the last line (the date of the previous week with the header row ) also appending the previous week**/
				arrLine[arrLine.Length - 1] = Date.Parse(date).ToShortDateString() + "," + headerRow;

				/** Write array to file, replacing contents with it (basically appending, but need to replace all to replace the last line **/
				System.IO.File.WriteAllLines(@"" + Server.MapPath("~/Logs/TimeLog/Time-log.txt"), arrLine);

				/** Now append to file **/
				using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Logs/TimeLog/Time-log.txt"), true)) {
					/** Command to get project hours  **/
					OleDbCommand objProject = new OleDbCommand("SELECT * FROM ProjectHours ORDER BY Emp_Name;", objConn);
					OleDbDataReader readerProject = objProject.ExecuteReader();

					backUpToTxt(readerProject, file, projectCount);
					readerProject.Close();

					objProject = new OleDbCommand("SELECT ID, Emp_Name, Project, Category, Percentage, Full_Time FROM PercentageLog ORDER BY Emp_Name ;", objConn);
					readerProject = objProject.ExecuteReader();

					backUpToTxt(readerProject, file, percentCount);

					/** Command to get car hours ( used to get header ) **/
					OleDbCommand fieldCarNames = new OleDbCommand("SELECT * FROM VehicleHours ORDER BY Emp_Name", objConn);
					OleDbDataReader readerCNames = fieldCarNames.ExecuteReader();

					table = readerPNames.GetSchemaTable();      /** Set the table of vehicle hours to variable **/
					nameCol = table.Columns["ColumnName"];      /** Set the column name **/
					headerRow = "";                             /** Header Row for Vehicle Hours Log file **/

					/** 
					 * Loop through each row, if column is ID, dont add
					 * This loop populates the header row for vehicle hours
					 **/
					foreach (DataRow row in table.Rows) {
						if (!row[nameCol].Equals("ID")) {
							headerRow += row[nameCol] + ",";
						}
					}

					/** Set header row to previous week + created header row **/
					headerRow = Date.Parse(date).ToShortDateString() + "," + headerRow;

					/** Write header row to file **/
					file.WriteLine(headerRow);
					readerCNames.Close();

					/** Command to get vehicle hours **/
					OleDbCommand objCar = new OleDbCommand("SELECT * FROM VehicleHours ORDER BY Emp_Name;", objConn);
					OleDbDataReader readerCar = objCar.ExecuteReader();

					/** Loop through each row **/
					backUpToTxt(readerCar, file, carCount);
					readerCar.Close();

					/** Second check if date needs to be changed, if yes, find the monday of the week, then set date to monday, write to line **/
					if (Date.Today.AddDays(-6) > Date.Parse(date)) {
						DateTime dt = DateTime.Now;
						file.Write(dt.ToShortDateString());
					}




					/** split list of projects into an array**/
					string[] arrayProjectList = projectList.Split(',');
					/** Start the query string **/
					string queryProject = "UPDATE ProjectHours SET ";

					/** Loop through the list of projects, and build the rest of the query, do use last array index, it is empty **/
					for (int i = 0; i < arrayProjectList.Length - 1; i++) {
						queryProject += arrayProjectList[i] + "=@value" + (i + 1);
						if (i != arrayProjectList.Length - 2) {
							/** If not last, then comma **/
							queryProject += ", ";
						}
						else {
							/** If last then semicolon to end query **/
							queryProject += ";";
						}
					}
					/** Command for query **/
					OleDbCommand objResetPH = new OleDbCommand(queryProject, objConn);
					for (int i = 0; i < arrayProjectList.Length - 1; i++) {
						objResetPH.Parameters.AddWithValue("@value" + (i + 1), 0);
					}

					objResetPH.ExecuteNonQuery();

					string[] arrayCarList = carList.Split(',');
					string queryCar = "UPDATE VehicleHours SET ";
					for (int i = 0; i < arrayCarList.Length - 1; i++) {
						queryCar += arrayCarList[i] + "=@value" + (i + 1);
						if (i != arrayCarList.Length - 2) {
							queryCar += ", ";
						}
						else {
							queryCar += ";";
						}
					}

					OleDbCommand objResetVH = new OleDbCommand(queryCar, objConn);
					for (int i = 0; i < arrayCarList.Length - 1; i++) {
						objResetVH.Parameters.AddWithValue("@value" + (i + 1), 0);
					}

					objResetVH.ExecuteNonQuery();

					objConn.Close();
					file.Close();
				}
			}
			catch (Exception ex) {
				writeStackTrace("Date Change", ex);
			}
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

					OleDbConnection objConn = openDBConnection();
					objConn.Open();
					OleDbCommand objCmdSelect = new OleDbCommand("SELECT Vehicle FROM Cars ORDER BY Vehicle", objConn);
					OleDbDataReader reader = objCmdSelect.ExecuteReader();
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
				OleDbConnection objConn = openDBConnection();
				objConn.Open();

				OleDbCommand objCmdProject = new OleDbCommand("UPDATE ProjectHours " +
														"SET " + ddlProjects.Text + "=@value1 " +
														"WHERE Emp_Name=@value2", objConn);
				objCmdProject.Parameters.AddWithValue("@value1", int.Parse(txtHoursProjects.Text));
				objCmdProject.Parameters.AddWithValue("@value2", ddlNamesProject.Text);

				objCmdProject.ExecuteNonQuery();

				OleDbCommand objCmdName = new OleDbCommand("SELECT Full_Time FROM Users WHERE Emp_Name=@name;", objConn);
				objCmdName.Parameters.AddWithValue("@name", ddlFullTimeNames.Text);
				OleDbDataReader namereader = objCmdName.ExecuteReader();
				bool fulltime = false;
				while (namereader.Read()) {
					fulltime = namereader.GetBoolean(0);
				}


				OleDbCommand objCmdCat = new OleDbCommand("SELECT Category FROM Projects WHERE Project=@project;", objConn);
				objCmdCat.Parameters.AddWithValue("@project", ddlAllProjects.Text);
				OleDbDataReader catreader = objCmdCat.ExecuteReader();
				string cat = "";
				while (catreader.Read()) {
					cat = catreader.GetValue(0).ToString();
				}

				DateTime dt = DateTime.Now;
				while (dt.DayOfWeek != DayOfWeek.Monday) dt = dt.AddDays(-1);

				OleDbCommand objCmdCars = new OleDbCommand("INSERT INTO PercentageLog (Emp_Name, Project, Category, Percentage, Log_Date, Full_Time) " +
															"VALUES (@name, @project, @cat, @percent, @date, @fulltime);", objConn);
				objCmdCars.Parameters.AddWithValue("@name", ddlFullTimeNames.Text);
				objCmdCars.Parameters.AddWithValue("@project", ddlAllProjects.Text);
				objCmdCars.Parameters.AddWithValue("@cat", cat);
				objCmdCars.Parameters.AddWithValue("@percent", Math.Round((double.Parse(txtHoursProjects.Text) / 40) * 100, 0));
				objCmdCars.Parameters.AddWithValue("@date", DateTime.Parse(dt.ToShortDateString()));
				objCmdCars.Parameters.AddWithValue("@fulltime", fulltime);

				objCmdCars.ExecuteNonQuery();



				objConn.Close();

				Session["success?"] = true;
				Response.Redirect("~/Hours");
			}
			catch (Exception ex) {
				writeStackTrace("Hours Submit", ex);
			}
		}

		private void submitPercent() {
			try {
				OleDbConnection objConn = openDBConnection();
				objConn.Open();

				OleDbCommand objCmdName = new OleDbCommand("SELECT Full_Time FROM Users WHERE Emp_Name=@name;", objConn);
				objCmdName.Parameters.AddWithValue("@name", ddlFullTimeNames.Text);
				OleDbDataReader namereader = objCmdName.ExecuteReader();
				bool fulltime = false;
				while (namereader.Read()) {
					fulltime = namereader.GetBoolean(0);
				}


				OleDbCommand objCmdCat = new OleDbCommand("SELECT Category FROM Projects WHERE Project=@project;", objConn);
				objCmdCat.Parameters.AddWithValue("@project", ddlAllProjects.Text);
				OleDbDataReader catreader = objCmdCat.ExecuteReader();
				string cat = "";
				while (catreader.Read()) {
					cat = catreader.GetValue(0).ToString();
				}

				DateTime dt = DateTime.Now;
				while (dt.DayOfWeek != DayOfWeek.Monday) dt = dt.AddDays(-1);

				OleDbCommand objCmdCars = new OleDbCommand("INSERT INTO PercentageLog (Emp_Name, Project, Category, Percentage, Log_Date, Full_Time) " +
															"VALUES (@name, @project, @cat, @percent, @date, @fulltime);", objConn);
				objCmdCars.Parameters.AddWithValue("@name", ddlFullTimeNames.Text);
				objCmdCars.Parameters.AddWithValue("@project", ddlAllProjects.Text);
				objCmdCars.Parameters.AddWithValue("@cat", cat);
				objCmdCars.Parameters.AddWithValue("@percent", double.Parse(ddlPercentage.Text.Substring(0, ddlPercentage.Text.IndexOf('%'))));
				objCmdCars.Parameters.AddWithValue("@date", DateTime.Parse(dt.ToShortDateString()));
				objCmdCars.Parameters.AddWithValue("@fulltime", fulltime);

				objCmdCars.ExecuteNonQuery();

				objConn.Close();
			}
			catch (Exception ex) {
				writeStackTrace("Hours Submit", ex);
			}

			Session["success?"] = true;
			Response.Redirect("~/Hours");
		}

		private void submitCars() {
			try {
				OleDbConnection objConn = openDBConnection();
				objConn.Open();

				OleDbCommand objCmdCars = new OleDbCommand("UPDATE VehicleHours " +
														"SET " + ddlCars.Text + "=@value1 " +
														"WHERE Emp_Name=@value2", objConn);
				objCmdCars.Parameters.AddWithValue("@value1", int.Parse(txtHoursCars.Text));
				objCmdCars.Parameters.AddWithValue("@value2", ddlNamesCar.Text);

				objCmdCars.ExecuteNonQuery();

				objConn.Close();

				// reset();
			}
			catch (Exception ex) {
				writeStackTrace("Hours Submit", ex);
			}

			populateDataCars((int)Session["VehicleCol"]);
			// populateDataProject();
		}

		//===================================================
		//PART 4: LIST/TABLE POPULATORS FOR THE HTML PAGE
		//===================================================

		private void populatePastWeekDropDown() {
			ddlselectWeek.Items.Add(date);
			ddlselectWeek.Items.Add("Not implemented yet, so this doesn't work.");
		}

		private void populateListProjectPercent() {
			try {
				ddlAllProjects.Items.Clear();
				ddlAllProjects.Items.Add("--Select A Project--");

				OleDbConnection objConn = openDBConnection();
				objConn.Open();
				OleDbCommand objCmdSelect = new OleDbCommand("SELECT Project FROM Projects ORDER BY Project", objConn);
				OleDbDataReader reader = objCmdSelect.ExecuteReader();
				while (reader.Read()) {
					ddlAllProjects.Items.Add(new ListItem(reader.GetString(0)));
				}
				objConn.Close();
				chartPercent.EnableViewState = true;
			}
			catch (Exception ex) {
				writeStackTrace("Populate Vehicles", ex);
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

				OleDbConnection objConn = openDBConnection();
				objConn.Open();

				OleDbCommand objCmdSelect = new OleDbCommand("SELECT Emp_Name FROM Users WHERE Full_Time=@bool ORDER BY Emp_Name", objConn);
				objCmdSelect.Parameters.AddWithValue("@bool", false);

				OleDbDataReader reader = objCmdSelect.ExecuteReader();
				while (reader.Read()) {
					ddlNamesProject.Items.Add(new ListItem(reader.GetString(0)));
					ddlNamesCar.Items.Add(new ListItem(reader.GetString(0)));
				}


				objCmdSelect = new OleDbCommand("SELECT Emp_Name FROM Users WHERE Full_Time=@bool ORDER BY Emp_Name", objConn);
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
				OleDbConnection objConn = openDBConnection();
				objConn.Open();
				OleDbCommand objCmdSelect = new OleDbCommand("SELECT Project FROM Projects ORDER BY Project", objConn);
				OleDbDataReader reader = objCmdSelect.ExecuteReader();
				while (reader.Read()) {
					ddlProjects.Items.Add(new ListItem(reader.GetString(0)));
				}
				objConn.Close();
			}
			catch (Exception ex) {
				writeStackTrace("Populate Vehicles", ex);
			}
		}

		private void populateDataPercentage() {
			try {
				OleDbConnection objConn = openDBConnection();
				objConn.Open();
				OleDbCommand objCount = new OleDbCommand("SELECT DISTINCT Emp_Name FROM PercentageLog WHERE Log_Date=@date ORDER BY Emp_Name", objConn);
				objCount.Parameters.AddWithValue("@date", DateTime.Parse(date));
				OleDbDataReader readerCount = objCount.ExecuteReader();
				int empCount = 0;
				while (readerCount.Read()) {
					empCount++;
				}
				OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM PercentageLog WHERE Log_Date=@date ORDER BY Emp_Name", objConn);
				objCmdSelect.Parameters.AddWithValue("@date", DateTime.Parse(date));
				OleDbDataAdapter objAdapter = new OleDbDataAdapter();
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
				objConn.Close();

				// dgvCars.HeaderRow.Cells[0].Visible = false;
			}
			catch (Exception ex) {
				writeStackTrace("Populate Percent", ex);
			}
		}

		//Populates data in the tables.
		//populateDataCars and populateDataProject call this function
		//with different enums
		private void populateData(int startIndex, Hours.DATA_TYPE type) {
			try {
				OleDbConnection objConn = openDBConnection();
				objConn.Open();
				OleDbCommand objCmdSelect;

				switch (type) {
					case DATA_TYPE.VEHICLE:
						objCmdSelect = new OleDbCommand("SELECT * FROM VehicleHours ORDER BY Emp_Name", objConn);
						break;
					case DATA_TYPE.PROJECT:
						objCmdSelect = new OleDbCommand("SELECT * FROM ProjectHours where Emp_Name in (select Emp_Name from Users where Full_Time=false) ORDER BY Emp_Name", objConn);
						break;
					default:
						writeStackTrace("Unexpected input into populateData", new ArgumentException(type.ToString()));
						return;
				}

				OleDbDataAdapter objAdapter = new OleDbDataAdapter();
				objAdapter.SelectCommand = objCmdSelect;
				DataSet objDataSet = new DataSet();
				objAdapter.Fill(objDataSet);
				objDataSet.Tables[0].Columns.RemoveAt(0);

				for (int i = 1; i < startIndex; i++) {
					objDataSet.Tables[0].Columns.RemoveAt(1);
				}
				int length = objDataSet.Tables[0].Columns.Count;
				for (int i = 7; i < length; i++) {
					objDataSet.Tables[0].Columns.RemoveAt(7);
				}

				for (int i = 0; i < objDataSet.Tables[0].Columns.Count; i++)
					objDataSet.Tables[0].Columns[i].ColumnName = objDataSet.Tables[0].Columns[i].ColumnName.Replace('_', ' ');

				switch (type) {
					case DATA_TYPE.VEHICLE:
						dgvCars.DataSource = objDataSet.Tables[0].DefaultView;
						dgvCars.DataBind();
						break;
					case DATA_TYPE.PROJECT:
						dgvProject.DataSource = objDataSet.Tables[0].DefaultView;
						dgvProject.DataBind();
						break;
					default:
						return;
				}

				objConn.Close();
				// dgvCars.HeaderRow.Cells[0].Visible = false;
			}
			catch (Exception ex) {
				switch (type) {
					case DATA_TYPE.VEHICLE:
						writeStackTrace("populateData", ex);
						break;
					case DATA_TYPE.PROJECT:
						writeStackTrace("populateData", ex);
						break;
					case DATA_TYPE.PERCENT:
						writeStackTrace("populateData", ex);
						break;
					default:
						writeStackTrace("populateData", ex);
						return;
				}
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

		private object[] getList(CTBTeam.Hours.DATA_TYPE enumSwitch, OleDbConnection objConn) {
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
				OleDbDataReader reader = new OleDbCommand(s, objConn).ExecuteReader();

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

		private void backUpToTxt(OleDbDataReader reader, StreamWriter file, int count) {
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