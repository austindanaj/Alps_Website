using System.Data.SqlClient;

using Date = System.DateTime;

using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

namespace CTBTeam {
	public partial class Hours : HoursPage {
		private SqlConnection objConn;
		private DataTable projectData, projectHoursData, vehicleHoursData, vehiclesData, datesData;
		private enum DATA_TYPE { VEHICLE, PROJECT };

		protected void Page_Load(object sender, EventArgs e) {
            /*
			if (Session["Alna_num"] == null) {
				redirectSafely("~/Login");
				return;
			}
            */
			objConn = openDBConnection();

			if (!IsPostBack) {
				if (Session["Date"] == null)
					initDate(objConn);
				if (Session["Active"] == null)
					Session["Active"] = true;

				getDate();
				getData();
				ddlInit();
			}
			populateTables();

			chkInactive.Checked = !(bool)Session["Active"];
		}

		private void getDate() {
			Date date = (Date)Session["Date"];
			lblWeekOf.Text = "Week of " + date.Month + "/" + date.Day + "/" + date.Year;
		}

		private void getData() {
			objConn.Open();

			if (!chkInactive.Checked) {
				projectData = getDataTable("select Project_ID, Name, Category from Projects where Active=@value1 order by Projects.PriorityOrder", true, objConn);
				vehiclesData = getDataTable("select ID, Name from Vehicles where Active=@value1", true, objConn);
			}
			else {
				projectData = getDataTable("select Project_ID, Name, Category from Projects order by Projects.PriorityOrder", null, objConn);
				vehiclesData = getDataTable("select ID, Name from Vehicles;", null, objConn);
			}

			//Everything else
			projectHoursData = getDataTable("select ID, Alna_num, Proj_ID, Hours_worked from ProjectHours where Date_ID=@value1", Session["Date_ID"], objConn);
			vehicleHoursData = getDataTable("select ID, Alna_num, Vehicle_ID, Hours_worked from VehicleHours where Date_ID=@value1", Session["Date_ID"], objConn);
			datesData = getDataTable("select Dates from Dates order by ID desc", null, objConn);
			objConn.Close();

			if (projectData == null || vehiclesData == null || projectHoursData == null || vehicleHoursData == null || datesData == null) {
				throwJSAlert("Problem accessing data; contact Anthony or Austin");
				return;
			}
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

			bool hasHoursWorked = false;
			int alna = (int)Session["Alna_num"];
			foreach (DataRow d in projectHoursData.Rows) {
				if (alna == (int)d[1]) {
					hasHoursWorked = true;
					ddlWorkedHours.Items.Add("P_ID#" + d[0].ToString() + ": worked " + d[3] + " hours on " + h[d[2]]);
				}
			}
			if (hasHoursWorked) pnlDeleteHours.Visible = true;

			hoursUpdate();
			if ((bool)Session["Full_time"]) return;
			pnlVehicleHours.Visible = true;

			h = new Hashtable();
			foreach (DataRow r in vehiclesData.Rows) {
				id = (int)r[0];
				name = r[1].ToString();
				ddlVehicles.Items.Add(name);
				h.Add(id, name);
			}

			foreach (DataRow d in vehicleHoursData.Rows) {
				if (alna == (int)d[1]) {
					hasHoursWorked = true;
					ddlWorkedHours.Items.Add("V_ID#" + d[0].ToString() + ": " + d[3] + " hrs on " + h[d[2]]);
				}
			}
			if (hasHoursWorked) pnlDeleteHours.Visible = true;

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
				Session["Active"] = !chkInactive.Checked;
				redirectSafely("~/Hours");
			}
			else if (sender.Equals(btnSubmitPercent)) {
				if (insertRecord(ddlProjects.SelectedValue, ddlHours.SelectedIndex, DATA_TYPE.PROJECT))
					redirectSafely("~/Hours");
			}
			else if (sender.Equals(btnSubmitVehicles)) {
				if (insertRecord(ddlVehicles.SelectedValue, ddlHoursVehicles.SelectedIndex, DATA_TYPE.VEHICLE))
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
				dgvCars.Visible = !dgvCars.Visible;
				dgvProject.Visible = !dgvProject.Visible;
			}
		}

		private bool insertRecord(string projectOrVehicle, int hours, DATA_TYPE type) {
			string table, modelTable, column, id;
			DataTable tableToUpdate;
			switch (type) {
				case DATA_TYPE.PROJECT:
					table = "ProjectHours";
					column = "Proj_ID";
					modelTable = "Projects";
                    id = "Project_ID";
					tableToUpdate = projectData;
					break;
				case DATA_TYPE.VEHICLE:
					table = "VehicleHours";
					column = "Vehicle_ID";
					modelTable = "Vehicles";
                    id = "ID";
                    tableToUpdate = vehiclesData;
					break;
				default:
					new NotImplementedException("Method has not been implemented");
					return false;
			}

			try {
				objConn.Open();
				object[] o = { Session["Alna_num"], projectOrVehicle, Session["Date_ID"] };
				SqlDataReader reader = getReader("select ID, Hours_worked from " + table + " where Alna_num=@value1 and " + column + "=(select " + id + " from " + modelTable + " where Name=@value2) and Date_ID=@value3", o, objConn);
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

				o = new object[] { o[0], projectOrVehicle, hours, o[2] };
				executeVoidSQLQuery("insert into " + table + " values(@value1, (select " + id + " from " + modelTable + " where Name=@value2), @value3, @value4)", o, objConn);
				objConn.Close();
			}
			catch (Exception ex) {
				throwJSAlert("Error connecting to database, check network connection");
				writeStackTrace("Hours Submit", ex);
				return false;
			}
			return true;
		}

		private void populateTables() {
			dgvProject.DataSource = getProjectHours(Session["Date_ID"], true, (bool)Session["Active"]);
			dgvProject.DataBind();
			dgvCars.DataSource = getVehicleHours(Session["Date_ID"], (bool)Session["Active"]);
			dgvCars.DataBind();
		}

		protected void color(object sender, GridViewRowEventArgs e) {
			if (e.Row.RowType == DataControlRowType.DataRow) {
				int n = e.Row.Cells.Count;
				int accumulator = 0;
				for (int i = 1; i < n; i++) {
					string text = e.Row.Cells[i].Text;
					if (text.Equals("&nbsp;"))  //If the cell is blank ASP puts this there for whatever reason
						continue;
					if (!int.TryParse(text, out int hour)) {
						e.Row.Cells[0].BackColor = System.Drawing.Color.Red;
						return;
					}
					accumulator += hour;
				}
				if (accumulator != 40)
					e.Row.Cells[0].BackColor = System.Drawing.Color.Red;
				else
					e.Row.Cells[0].BackColor = System.Drawing.Color.Green;
			}
		}

		private void hoursUpdate() {
			int hoursWorked = 0, totalHours = 0;

			Lambda howMuchHoursWorked = new Lambda(delegate (object o) {
				DataTable temp = (DataTable)o;
				int session = (int)Session["Alna_num"];
				foreach (DataRow d in temp.Rows) {
					if ((int)d[1] == session)
						hoursWorked += (int)d[3];
					totalHours += (int)d[3];
				}
			});

			Lambda addDDLoptions = new Lambda(delegate (object o) {
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
			lblUserHours.Text = "Your Hours: " + hoursWorked + "/40";
			if (hoursWorked == 40) pnlAddHours.Visible = false;

			if ((bool)Session["Full_time"]) return;

			hoursWorked = 0;
			ddlHoursVehicles.Items.Add("--Select A Percent (Out of 40 hrs)--");
			howMuchHoursWorked(vehicleHoursData);
			addDDLoptions(ddlHoursVehicles);

			lblUserHours.Text = "Logged " + hoursWorked + "/40";
		}
	}
}