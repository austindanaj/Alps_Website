using System;
using System.Data.SqlClient;

namespace CTBTeam {
	public partial class Schedule : SchedulePage {
		SqlConnection objConn;

		protected void Page_Load(object sender, EventArgs e) {
            /*
               if (Session["Alna_num"] == null) {
                   redirectSafely("~/Login");
                   return;
               }
               */

            objConn = openDBConnection();
			if (!IsPostBack) {
				objConn.Open();
				populateInternSchedules(objConn, dgvSchedule, ddlSelectScheduleDay);
				populateScheduledHoursDdl(objConn);
				objConn.Close();
			}
		}

		private void populateScheduledHoursDdl(SqlConnection objConn) {
			ddlScheduledHours.Items.Clear();

			object[] o = { Session["Alna_num"], Session["weekday"] };
			SqlDataReader reader = getReader("select ID, TimeStart, TimeEnd from Schedule where Alna_num=@value1 and DayOfWeek=@value2", o, objConn);
			if (!reader.HasRows) {
				pnlDelete.Visible = false;
				reader.Close();
				objConn.Close();
				return;
			}

			while (reader.Read()) {
				ddlScheduledHours.Items.Add("ID#" + reader.GetInt32(0) + ">" + military_to_standard(reader.GetInt16(1)) + " - " + military_to_standard(reader.GetInt16(2)));
			}
			reader.Close();
			objConn.Close();
		}

		//----------------------------------------------------------------
		// HTML events
		//----------------------------------------------------------------
		protected void changeScheduleDay(object sender, EventArgs e) {
			Session["weekday"] = ddlSelectScheduleDay.SelectedIndex + 1;
			SqlConnection objConn = openDBConnection();
			objConn.Open();
			populateInternSchedules(objConn, dgvSchedule, ddlSelectScheduleDay);
			populateScheduledHoursDdl(objConn);
			objConn.Close();
		}

		protected void saveOrDelete(object sender, EventArgs e) {
			if (sender.Equals(btnConfirmTime)) {
				int temp = -1;
				Lambda parse = new Lambda(delegate (object o) {
					bool isStartTime = (bool)o;
					try {
						if (isStartTime) {
							temp = int.Parse(txtStartTime.Text.Replace(" ", "").Replace(":", ""));
							temp += temp >= 1200 ? 0 : (ddlStartAmPm.SelectedIndex * 1200);
						}
						else {
							temp = int.Parse(txtEndTime.Text.Replace(" ", "").Replace(":", ""));
							temp += temp >= 1200 ? 0 : (ddlEndAmPm.SelectedIndex * 1200);
						}
							
						if (temp < 1859 & temp >= 700 | temp % 100 < 60) return; 
					}
					catch {
						writeStackTrace("Looks like 1 of 2 things happened here: someone tried hacking the parser for the time in saveOrDelete or there's a logic messup somewhere, but the first case is more likely", new ArgumentException());
					}
					temp = -1;
				});
				parse(true);
				if (temp == -1) {
					throwJSAlert("Start time is not a correct time format");
					return;
				}
				int start = temp;
				parse(false);
				if (temp == -1) {
					throwJSAlert("End time is not a correct time format");
					return;
				}
				int end = temp;

				if (start >= end) {
					throwJSAlert("You cant work impossible hours...");
					return;
				}

				if (start + 100 > end) {
					throwJSAlert("You should schedule yourself for over an hour at least");
					return;
				}

				if (start < 600 | end > 1900) {
					throwJSAlert("You are starting too early or ending too late. Earliest start is 6am, latest time you can be here is 7pm.");
					return;
				}

				objConn.Open();
				object[] obj = { Session["Alna_num"], ddlDay.SelectedIndex + 1 };
				SqlDataReader reader = getReader("select TimeStart, TimeEnd from Schedule where Alna_num=@value1 and DayOfWeek=@value2", obj, objConn);
				while (reader.Read()) {
					int compareStart = reader.GetInt16(0), compareEnd = reader.GetInt16(1);
					if ((compareStart <= start & compareEnd >= start) | (compareStart <= end & compareEnd >= end) | (compareStart >= start & end >= compareEnd)) {
						reader.Close();
						objConn.Close();
						throwJSAlert("Conflicts with another schedule entry you have. Make sure that the day you want to add is selected in the dropdown, you may be adding it for Monday on accident.");
						return;
					}
				}
				reader.Close();

				obj = new object[] { Session["Alna_num"], start, end, ddlDay.SelectedIndex + 1 };
				executeVoidSQLQuery("insert into Schedule (Alna_num, TimeStart, TimeEnd, DayOfWeek) values (@value1, @value2, @value3, @value4)", obj, objConn);
				throwJSAlert("Successfully added new schedule entry");
				pnlDelete.Visible = true;
				redirectSafely("~/Schedule");
			}
			else {
				string string_id = ddlScheduledHours.SelectedValue.Substring(3, ddlScheduledHours.SelectedValue.IndexOf(">") - ddlScheduledHours.SelectedValue.IndexOf("#") - 1);
				if (!int.TryParse(string_id, out int id)) {
					throwJSAlert("Something was wrong with the dropdown selection. It wasn't an integer.");
					return;
				}
				objConn.Open();
				executeVoidSQLQuery("delete from Schedule where ID=@value1", id, objConn);
				populateInternSchedules(objConn, dgvSchedule, ddlSelectScheduleDay);
				populateScheduledHoursDdl(objConn);
			}
		}
	}
}