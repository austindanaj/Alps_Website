using System;
using Date = System.DateTime;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Data;

namespace CTBTeam {
	public partial class TimeOff : SuperPage {
		private enum DATE_VALID { OUT_OF_ORDER, VALID, INVALID };
		SqlConnection objConn;

		protected void Page_Load(object sender, EventArgs e) {
			if (Session["Alna_num"] == null) {
				redirectSafely("~/Login");
				return;
			}

			objConn = openDBConnection();

			if (!IsPostBack) {
				cldTimeOffStart.SelectedDate = DateTime.Now;
				init();
				successDialog(successOrFail);
			}
		}

		private void init() {
			objConn.Open();
			SqlDataReader reader = getReader("select ID, Start, TimeOff.[End] from TimeOff where Alna_num=@value1", Session["Alna_num"], objConn);
			if (reader == null) return;
			while (reader.Read())
				ddlTimeTakenOff.Items.Add("ID#" + reader.GetInt32(0) + ":" + ((Date) reader.GetValue(1)).ToShortDateString() + " - " + ((Date) reader.GetValue(2)).ToShortDateString());
			reader.Close();

			DataTable gridview = new DataTable();
			gridview.Columns.Add("Name", typeof(string));
			gridview.Columns.Add("Monday", typeof(string));
			gridview.Columns.Add("Tuesday", typeof(string));
			gridview.Columns.Add("Wednesday", typeof(string));
			gridview.Columns.Add("Thursday", typeof(string));
			gridview.Columns.Add("Friday", typeof(string));

			initDate(objConn);

			int i;
			Date monday = (Date)Session["Date"];
			Date[] weekdays = new Date[5];
			for (i = 0; i <= 4; i++)
				weekdays[i] = monday.AddDays(i);

			DataTable employees = getDataTable("select Alna_num, Name from Employees order by Alna_num", null, objConn);
			DataTable timeOff = getDataTable("select Alna_num, Start, TimeOff.[End], Business from TimeOff order by Alna_num", null, objConn);

			i = 0;
			int maxIndex = timeOff.Rows.Count;
			Date start, end;
			DataRow newRow, currentRecord;
			foreach(DataRow employee in employees.Rows) {
				newRow = gridview.NewRow();
				newRow["Name"] = employee[1];
				if (i == maxIndex) {
					gridview.Rows.Add(newRow);
					continue;
				}
				currentRecord = timeOff.Rows[i];
				while ((int)currentRecord[0] == (int)employee[0]) {
					start = (Date) currentRecord[1];
					end = (Date) currentRecord[2];
					foreach (Date day in weekdays)
						if (day.CompareTo(start) >= 0 && day.CompareTo(end) <= 0)
							newRow[day.DayOfWeek.ToString()] = (bool) currentRecord[3] ? "Business" : "Vacation";
					i++;
					if (i == maxIndex) break;
				}
				gridview.Rows.Add(newRow);
			}

			gv.DataSource = gridview;
			gv.DataBind();
			objConn.Close();
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

		private bool doesntConflict(SqlConnection o, Date start, Date end) {
			SqlDataReader reader = getReader("Select TimeOff.[Start], TimeOff.[End] from TimeOff where Alna_num=@value1", Session["Alna_num"], objConn);
			if (reader == null) return false;
			if (!reader.HasRows) {
				reader.Close();
				return true;
			}
			while (reader.Read()) {
				Date otherVacationStart = (Date)reader.GetValue(0);
				Date otherVacationEnd = (Date)reader.GetValue(1);
				//The only time a vacation time is valid is if it starts after the others end, or it
				//begins and ends before the rest.
				if ((start.CompareTo(otherVacationEnd) <= 0 && start.CompareTo(otherVacationStart) >= 0) ||
					(end.CompareTo(otherVacationEnd) <= 0 && end.CompareTo(otherVacationStart) >= 0)) {
					reader.Close();
					return false;
				}
			}
			reader.Close();
			return true;
		}

		protected void addTimeOff(object sender, EventArgs e) {
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

			objConn.Open();

			if (!doesntConflict(objConn, start, end)) {
				throwJSAlert("This time conflicts with another vacation time you have.");
				return;
			}

			object[] o = { Session["Alna_num"], start, end, chkBusinessTrip.Checked };
			executeVoidSQLQuery("INSERT INTO TimeOff (Alna_num, TimeOff.[Start], TimeOff.[End], Business) VALUES (@value1, @value2, @value3, @value4);", o, objConn);

			objConn.Close();
			Session["success?"] = true;
			redirectSafely("~/TimeOff");
		}

		protected void removeTimeOff(object sender, EventArgs e) {
			string s = ddlTimeTakenOff.SelectedValue;
			if (string.IsNullOrEmpty(s))
				return;
			executeVoidSQLQuery("DELETE FROM TimeOff WHERE ID=@value1", s.Substring(3, s.IndexOf(":") - 3), objConn);
			ddlTimeTakenOff.Items.Remove(s);
			throwJSAlert("Your time off for " + s + " has been successfully removed");
		}
	}
}