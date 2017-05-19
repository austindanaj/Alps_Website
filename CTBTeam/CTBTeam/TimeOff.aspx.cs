using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Date = System.DateTime;
using System.Data.OleDb;
using System.Web.UI.WebControls;
using System.Threading;

namespace CTBTeam {
	public partial class TimeOff : SuperPage {
		private enum DATE_VALID { OUT_OF_ORDER, VALID, INVALID };

		protected void Page_Load(object sender, EventArgs e) {
			if (Session["user"] == null) {
				btnAddTimeOff.Visible = false;
				btnRemoveTimeOff.Visible = false;
			}
			if (!IsPostBack) {
				populateNames();
				cldTimeOffStart.SelectedDate = DateTime.Now;
				successDialog(successOrFail);
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
				if (sender.Equals(cldTimeOffStart))
					cldTimeOffEnd.SelectedDate = cldTimeOffStart.SelectedDate;
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
					if ((start.CompareTo(otherVacationEnd) <= 0 && start.CompareTo(otherVacationStart) >= 0) ||
						(end.CompareTo(otherVacationEnd) <= 0 && end.CompareTo(otherVacationStart) >= 0))
						return false;
				}
				return true;
			}
			catch (Exception e) {
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
							cldTimeOffEnd.SelectedDate = Date.Today;
							throwJSAlert("Can only request days off in the future.");
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

					object[] o = { ddlNames.Text, start, end };
					executeVoidSQLQuery("INSERT INTO TimeOff (Emp_Name, Start_Date, End_Date) VALUES (@value1, @value2, @value3);", o, objConn);

					Session["success?"] = true;
					redirectSafely("~/TimeOff");
				}
				catch (Exception ex) {
					writeStackTrace("Time off", ex);
				}
			}
		}

		protected void nameChange(object sender, EventArgs e) {
			try {
				//if (ddlNames.SelectedValue.ToString().Equals("--Select A Name--")) return;
				ddlTimeTakenOff.Items.Clear();
				OleDbConnection objConn = openDBConnection();
				objConn.Open();

				OleDbCommand cmd = new OleDbCommand("Select ID, Start_Date, End_Date from TimeOff where Emp_Name=@value1", objConn);
				cmd.Parameters.AddWithValue("@value1", ddlNames.SelectedValue.ToString());
				OleDbDataReader reader = cmd.ExecuteReader();
				ddlTimeTakenOff.Items.Add("--Select a time off period--");
				while (reader.Read()) {
					ddlTimeTakenOff.Items.Add(new ListItem(reader.GetValue(0).ToString() + ": " +
						DateTime.Parse(reader.GetValue(1).ToString()).ToShortDateString() + " - " +
						DateTime.Parse(reader.GetValue(2).ToString()).ToShortDateString()));
				}

				objConn.Close();
			}
			catch (Exception ex) {
				writeStackTrace("Name Change", ex);
			}
		}

		protected void removeTimeOff(object sender, EventArgs e) {
			if (!string.IsNullOrEmpty((string)Session["User"])) {
				try {
					OleDbConnection objConn = openDBConnection();
					objConn.Open();

					string s = ddlTimeTakenOff.SelectedValue;
					executeVoidSQLQuery("DELETE FROM TimeOff WHERE ID=@value1", s.Substring(0, s.IndexOf(':')), objConn);

					ddlTimeTakenOff.Items.Remove(s);

					throwJSAlert("Your time off for " + s + " has been successfully removed");
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