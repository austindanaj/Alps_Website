using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Date = System.DateTime;
using System.Data.OleDb;
using System.Web.UI.WebControls;

namespace CTBTeam {
	public partial class TimeOff : SuperPage {
		private enum DATE_VALID {OUT_OF_ORDER, VALID, INVALID};

		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				calendarInit();
				populateNames();
				cldTimeOffStart.SelectedDate = DateTime.Now;
			}
		}

		private void calendarInit() {
			if (Session["user"] == null) {
				cldTimeOffStart.Visible = false;
				cldTimeOffEnd.Visible = false;
			}
		}

		protected void getCurrentDate(object sender, EventArgs e) {
			try {
				bltList.Items.Clear();
				OleDbConnection objConn = openDBConnection();
				objConn.Open();

				OleDbCommand objCmd = new OleDbCommand("SELECT DISTINCT Emp_Name FROM TimeOff WHERE Start_Date between @start AND @end OR End_Date between @start and @end;", objConn);
				objCmd.Parameters.AddWithValue("@start", cldTimeOffStart.SelectedDate);
				objCmd.Parameters.AddWithValue("@end", cldTimeOffEnd.SelectedDate);
				OleDbDataReader reader = objCmd.ExecuteReader();
				while (reader.Read()) {
					bltList.Items.Add(reader["Emp_Name"].ToString());
				}

				objConn.Close();
			}
			catch (Exception ex) {
				writeStackTrace("Calendar Date Picked", ex);
			}
		}

		private TimeOff.DATE_VALID validCalendarSelection(Date start, Date end) {
			Date today = Date.Today;
			if (start <= today || end <= today) {
				return TimeOff.DATE_VALID.INVALID;
			}
			//If the dates are reversed, it'll still work.
			else if (start.CompareTo(end) > 0) {
				return DATE_VALID.OUT_OF_ORDER;
			}
			return DATE_VALID.VALID;
		}

		private bool doesntConflict(OleDbConnection o, string name, Date start, Date end) {
			try {
				OleDbCommand cmd = new OleDbCommand("Select Start_Date, End_Date from TimeOff where Emp_Name=@value1", o);
				cmd.Parameters.AddWithValue("@value1", name);
				OleDbDataReader reader = cmd.ExecuteReader();
				if (!reader.HasRows)
					return true;
				while (reader.Read()) {
					Date otherVacationStart = (Date)reader.GetValue(0);
					Date otherVacationEnd = (Date)reader.GetValue(1);
					//The only time a vacation time is valid is if it starts after the others end, or it
					//begins and ends before the rest.
					if (start.CompareTo(otherVacationEnd) > 0 || end.CompareTo(otherVacationStart) < 0) {
						return true;
					}
				}
				return false;
			} catch (Exception e) {
				writeStackTrace("doesntConflict", e);
				return false;
			}
		}

		protected void addTimeOff(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty((string)Session["User"])) {
				try {
					Date start = cldTimeOffStart.SelectedDate;
					Date end = cldTimeOffEnd.SelectedDate;

					switch (validCalendarSelection(start, end)) {
						case DATE_VALID.INVALID:
							cldTimeOffStart.SelectedDate = Date.Today;
							throwJSAlert("Selection makes no sense.");
							return;
						case DATE_VALID.OUT_OF_ORDER:
							Date d = end;
							end = start;
							start = d;
							break;
						default:
							break;
					}

					OleDbConnection objConn = openDBConnection();
					objConn.Open();

					if (!doesntConflict(objConn, ddlNames.Text, start, end)) {
						throwJSAlert("This time conflicts with another vacation time you have.");
						return;
					}

					OleDbCommand objCmd = new OleDbCommand("INSERT INTO TimeOff " +
															"(Emp_Name, Start_Date, End_Date) VALUES (@value1, @value2, @value3);", objConn);

					objCmd.Parameters.AddWithValue("@value1", ddlNames.Text);
					objCmd.Parameters.AddWithValue("@value2", start);
					objCmd.Parameters.AddWithValue("@value3", end);
					objCmd.ExecuteNonQuery();

					objCmd = new OleDbCommand("Select ID from TimeOff where Start_Date=@value1 and End_Date=@value2 and Emp_Name=@value3;", objConn);
					objCmd.Parameters.AddWithValue("@value3", ddlNames.Text);
					objCmd.Parameters.AddWithValue("@value1", start);
					objCmd.Parameters.AddWithValue("@value2", end);
					OleDbDataReader reader = objCmd.ExecuteReader();

					ddlTimeTakenOff.Items.Add(new ListItem(reader.Read().ToString() + ": " + start.ToShortDateString() + " - " + end.ToShortDateString()));
					objConn.Close();

					throwJSAlert("Time off for " + start.ToShortDateString() + " to " + end.ToShortDateString() + " has been added successfully.");
				}
				catch (Exception ex) {
					writeStackTrace("Time off", ex);
				}
			}
		}

		protected void nameChange(object sender, EventArgs e) {
			//if (ddlNames.SelectedValue.ToString().Equals("--Select A Name--")) return;
			ddlTimeTakenOff.Items.Clear();
			OleDbConnection objConn = openDBConnection();
			objConn.Open();

			OleDbCommand cmd = new OleDbCommand("Select ID, Start_Date, End_Date from TimeOff where Emp_Name=@value1", objConn);
			cmd.Parameters.AddWithValue("@value1", ddlNames.SelectedValue.ToString());
			OleDbDataReader reader = cmd.ExecuteReader();
			ddlTimeTakenOff.Items.Add("--Select a time off period--");
			while (reader.Read()) {
				ddlTimeTakenOff.Items.Add(new ListItem(reader.GetValue(0).ToString() + ": " + reader.GetValue(1).ToString() + " - " + reader.GetValue(2).ToString()));
			}

			objConn.Close();
		}

		protected void removeTimeOff(object sender, EventArgs e) {
			if (string.IsNullOrEmpty((string)Session["User"])) {
				try {
					OleDbConnection objConn = openDBConnection();
					objConn.Open();

					OleDbCommand objCmd = new OleDbCommand("DELETE FROM TimeOff WHERE ID=@value1", objConn);

					string selection = ddlTimeTakenOff.SelectedValue, s = ddlTimeTakenOff.SelectedValue;
					s = s.Substring(0,s.IndexOf(':'));
					objCmd.Parameters.AddWithValue("@value1", s);

					objCmd.ExecuteNonQuery();

					ddlTimeTakenOff.Items.Remove(selection);

					throwJSAlert("Your time off for " + selection + " has been successfully removed");
				}
				catch (Exception ex) {
					writeStackTrace("Remove Time Off", ex);
				}
			}
		}
		public void populateNames() {
			try {
				ddlNames.Items.Clear();

				ddlNames.Items.Add("--Select A Name--");

				OleDbConnection objConn = openDBConnection();
				objConn.Open();
				OleDbCommand objCmdSelect = new OleDbCommand("SELECT Emp_Name FROM Users ORDER BY Emp_Name", objConn);
				OleDbDataReader reader = objCmdSelect.ExecuteReader();
				while (reader.Read()) {
					ddlNames.Items.Add(new ListItem(reader.GetString(0)));
				}
				objConn.Close();
			}
			catch (Exception ex) {
				writeStackTrace("Populate names", ex);
			}
		}
	}
}