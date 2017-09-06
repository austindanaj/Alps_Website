using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace CTBTeam {
	public partial class Schedule : SuperPage {
		SqlConnection objConn;

		protected void Page_Load(object sender, EventArgs e) {
			//SCAFFOLD
			Session["Alna_num"] = 173017;
			objConn = openDBConnection();
			if (!IsPostBack) {
				objConn.Open();
				populateInternSchedules(objConn);
				populateScheduledHoursDdl(objConn);
				objConn.Close();
			}
		}

		private void populateInternSchedules(SqlConnection objConn) {
			/* This function is responsible for populating the schedule tables. It's rather complicated and needs to be fast,
			 * so this will explain it all.
			 * 
			 * 1. First we just get employee information. Nothing special here. We just need to use Linkedlists first because the amount of employees we have may change.
			 *	  Then we convert them to arrays for fast access.
			 * 2. Now we get the schedule data, but while we do it we create a special hashtable: it will take in the Alna_num and the weekday as an object array and 
			 *	  return an object array with the times that person is available for. We do this for speed in populating the table.
			 */

			Session["weekday"] = Session["weekday"] == null ? 1 : Session["weekday"]; //Init session so no null references occur

			//1. First get Alna nums and names

			List<int> temp_alna_nums = new List<int>();
			List<string> temp_names = new List<string>();
			SqlDataReader reader = getReader("select Alna_num, Name from Employees where Active=@value1 and Full_time!=@value1 order by Alna_num asc", true, objConn);
			while (reader.Read()) {
				temp_alna_nums.Add(reader.GetInt32(0));
				temp_names.Add(reader.GetString(1));
			}
			reader.Close();
			int[] alna_nums = temp_alna_nums.ToArray();     //We want fast O(1) access because we are going to be doing a good amount of computation
			string[] names = temp_names.ToArray();


			//2. Get schedule data
			temp_alna_nums = new List<int>();
			List<int> temp_timestart_list = new List<int>();
			List<int> temp_timeend_list = new List<int>();
			reader = getReader("select Alna_num, TimeStart, TimeEnd from Schedule where DayOfWeek=@value1 order by Alna_num asc", Session["weekday"], objConn);
			while (reader.Read()) {
				temp_alna_nums.Add(reader.GetInt32(0));
				temp_timestart_list.Add(reader.GetInt16(1));
				temp_timeend_list.Add(reader.GetInt16(2));
			}
			reader.Close();
			int[] schedule_alna_nums = temp_alna_nums.ToArray();
			int[] timestart = temp_timestart_list.ToArray();
			int[] timeend = temp_timeend_list.ToArray();

			DataTable table = new DataTable();
			table.Columns.Add(ddlSelectScheduleDay.Items[(int)Session["weekday"] - 1].Text);
			foreach (string name in names)
				table.Columns.Add(name);

			DataRow d;
			int[] workday = { 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800 };
			int tableColumns = table.Columns.Count;
			int scheduleColumns = schedule_alna_nums.Length;
			foreach (int i in workday) {
				d = table.NewRow();
				d[0] = military_to_standard(i);
				for (int j = 0; j < alna_nums.Length; j++) {
					for (int k = 0; k < scheduleColumns; k++) {
						if (alna_nums[j] == schedule_alna_nums[k]) {
							if (i <= timestart[k] & i + 100 > timestart[k])
								d[j + 1] = "In @" + military_to_standard(timestart[k]);
							else if (i <= timeend[k] & i + 100 > timeend[k])
								d[j + 1] = "Out @" + military_to_standard(timeend[k]);
							else if (i > timestart[k] & i <= timeend[k])
								d[j + 1] = "Working";
						}
					}
				}
				table.Rows.Add(d);
			}

			dgvSchedule.DataSource = table;
			dgvSchedule.DataBind();
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
			populateInternSchedules(objConn);
			objConn.Close();
		}

		protected void color(object sender, GridViewRowEventArgs e) {
			if (e.Row.RowType == DataControlRowType.DataRow) {
				for (int i = 1; i < e.Row.Cells.Count; i++) {
					string cellText = e.Row.Cells[i].Text;
					if (string.IsNullOrEmpty(cellText)) continue;
					if (cellText.Equals("Working")) {
						e.Row.Cells[i].BackColor = System.Drawing.Color.Blue;
						e.Row.Cells[i].ForeColor = System.Drawing.Color.Yellow;
					}
					else if (cellText.Contains("In ") | cellText.Contains("Out ")) {
						e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
						e.Row.Cells[i].ForeColor = System.Drawing.Color.Blue;
					}
				}
			}
		}

		protected void saveOrDelete(object sender, EventArgs e) {
			if (sender.Equals(btnConfirmTime)) {
				int temp = -1;
				Lambda parse = new Lambda(delegate (object o) {
					bool isStartTime = (bool)o;
					try {
						if (isStartTime)
							temp = int.Parse(txtStartTime.Text.Replace(" ", "").Replace(":", "")) + (ddlStartAmPm.SelectedIndex * 1200);
						else
							temp = int.Parse(txtEndTime.Text.Replace(" ", "").Replace(":", "")) + (ddlEndAmPm.SelectedIndex * 1200);
						if (temp < 1859 & temp >= 700 | temp % 100 < 60) return;
					}
					catch { }
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
						throwJSAlert("Conflicts with another schedule entry you have");
						return;
					}
				}
				reader.Close();

				obj = new object[] { Session["Alna_num"], start, end, ddlDay.SelectedIndex + 1 };
				executeVoidSQLQuery("insert into Schedule (Alna_num, TimeStart, TimeEnd, DayOfWeek) values (@value1, @value2, @value3, @value4)", obj, objConn);
				throwJSAlert("Successfully added new schedule entry");
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
				populateInternSchedules(objConn);
				populateScheduledHoursDdl(objConn);
			}
		}

		private string military_to_standard(int time) {
			if (time >= 1300)
				time -= 1200;
			string s = time.ToString();
			if (s.Length == 3)
				s = s[0] + ":" + s.Substring(1, 2);
			else
				s = s.Substring(0, 2) + ":" + s.Substring(2, 2);
			return s;
		}
	}
}