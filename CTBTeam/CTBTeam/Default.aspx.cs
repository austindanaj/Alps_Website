using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using Date = System.DateTime;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace CTBTeam {
	public partial class _Default : HoursPage {
		private delegate string Lambda1(int time);

		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				SqlConnection objConn = openDBConnection();
				objConn.Open();
				SqlDataReader reader = getReader("select Dates from Dates order by Dates desc", null, objConn);
				if (reader == null) {
					throwJSAlert("Can't connect to DB; contact admin");
					return;
				}
				while (reader.Read())
					ddlselectWeek.Items.Add(reader.GetDateTime(0).ToShortDateString());
				reader.Close();
				populatePieChart(objConn);
				populateDaysOffTable(objConn);
				populateInternSchedules(objConn);
				objConn.Close();
			}
		}

		//----------------------------------------------------------------
		// Inits
		//----------------------------------------------------------------
		private void populatePieChart(SqlConnection objConn) {
			SqlDataReader reader = getReader("select p1.[Hours_worked], p2.Category from ProjectHours p1 inner join Projects p2 on p2.ID=p1.Proj_ID where p1.Date_ID=(select top 1 ID from Dates order by ID desc);", null, objConn);
			if (!reader.HasRows) {
				chartPercent.Visible = false;
				reader.Close();
				return;
			}
			double[] projectHours = new double[4];

			int hours, totalHours = 0;
			while (reader.Read()) {
				hours = reader.GetInt32(0);
				if (reader.GetString(1).Equals("A"))
					projectHours[0] += hours;
				else if (reader.GetString(1).Equals("B"))
					projectHours[1] += hours;
				else if (reader.GetString(1).Equals("C"))
					projectHours[2] += hours;
				else
					projectHours[3] += hours;
				totalHours += hours;
			}
			reader.Close();

			for (int i = 0; i < projectHours.Length; i++)
				projectHours[i] /= totalHours;

			chartPercent.Series[0].Points.DataBindXY(new string[] { "A", "B", "C", "D" }, projectHours);
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
		}

		private void populateDaysOffTable(SqlConnection objConn) {
			dgvOffThisWeek.DataSource = getDataTable("select e.Name as 'Employees out this week' from Employees e where e.Alna_num in (select Alna_num from TimeOff where (select top 1 Dates from Dates order by ID desc) between TimeOff.Start and TimeOff.[End]);", null, objConn);
			dgvOffThisWeek.DataBind();
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
			table.Columns.Add("Time");
			foreach (string name in names)
				table.Columns.Add(name);

			Lambda1 military_to_standard = new Lambda1(delegate (int time) {
				if (time >= 1300)
					time -= 1200;
				string s = time.ToString();
				if (s.Length == 3)
					s = s[0] + ":" + s.Substring(1, 2);
				else
					s = s.Substring(0,2) + ":" + s.Substring(1, 2);
				return s;
			});

			DataRow d;
			int[] workday = { 800, 900, 1000, 1100, 1200, 1300, 1400, 1500, 1600, 1700 };
			int tableColumns = table.Columns.Count;
			int scheduleColumns = schedule_alna_nums.Length;
			foreach (int i in workday) {
				d = table.NewRow();
				d[0] = military_to_standard(i);
				for (int j= 0;j < alna_nums.Length;j++) {
					for (int k=0;k<scheduleColumns;k++) {
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

			switch ((int)Session["weekday"]) {
				case 1:
					dgvMonday.Visible = true;
					dgvMonday.DataSource = table;
					dgvMonday.DataBind();
					break;
				case 2:
					dgvTuesday.Visible = true;
					dgvTuesday.DataSource = table;
					dgvTuesday.DataBind();
					break;
				case 3:
					dgvWednesday.Visible = true;
					dgvWednesday.DataSource = table;
					dgvWednesday.DataBind();
					break;
				case 4:
					dgvThursday.Visible = true;
					dgvThursday.DataSource = table;
					dgvThursday.DataBind();
					break;
				default:
					dgvFriday.Visible = true;
					dgvFriday.DataSource = table;
					dgvFriday.DataBind();
					break;
			}
		}

		//----------------------------------------------------------------
		// HTML events
		//----------------------------------------------------------------
		protected void toetruck(object sender, EventArgs e) {
			redirectSafely("~/ToeTruck");
		}

		protected void download(object sender, EventArgs e) {
			if (!Date.TryParse(ddlselectWeek.SelectedValue, out Date date)) {
				throwJSAlert("Not a valid date");
				return;
			}

			DataTable projectDataTable = getProjectHours(date, true);
			DataTable vehicleDataTable = getVehicleHours(date);

			if (projectDataTable == null | vehicleDataTable == null) {
				throwJSAlert("Data could not be downloaded; some sort of error");
				return;
			}

			//Write file then transmit it
			try {
				string s, fileName = @"" + Server.MapPath("~/Logs/" + Date.Today.Year + "-" + Date.Today.Month + "-" + Date.Today.Day + "_DBLog.csv");
				File.Create(fileName).Dispose();
				StreamWriter file = new StreamWriter(fileName);

				Lambda addColumns = new Lambda(delegate (object o) {
					DataTable tmp = (DataTable)o;
					s = "";
					foreach (DataColumn d in tmp.Columns)
						s += d.ToString() + ",";
					file.Write(s);
					file.WriteLine();
				});

				Lambda insertRows = new Lambda(delegate (object o) {
					DataTable tmp = (DataTable)o;
					foreach (DataRow d in tmp.Rows) {
						s = "";
						foreach (object obj in d.ItemArray)
							s += obj.ToString() + ",";
						file.Write(s);
						file.WriteLine();
					}
				});

				addColumns(projectDataTable);
				insertRows(projectDataTable);
				file.WriteLine();
				addColumns(vehicleDataTable);
				insertRows(vehicleDataTable);

				file.Close();

				Response.ContentType = "Application/txt";
				Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
				Response.TransmitFile(fileName);

				HttpResponse response = HttpContext.Current.Response; //These 4 lines kill the response without any exceptions
				response.Flush();
				response.SuppressContent = true;
				HttpContext.Current.ApplicationInstance.CompleteRequest();
			}
			catch (Exception ex) {
				writeStackTrace("Something wrong in file writing", ex);
				throwJSAlert("Something wrong with the directory structure/file IO; contact an admin");
			}
		}

		protected void color(object sender, GridViewRowEventArgs e) {
			if (e.Row.RowType == DataControlRowType.DataRow) {
				for (int i = 1; i < e.Row.Cells.Count; i++) {
					string cellText = e.Row.Cells[i].Text;
					if (string.IsNullOrEmpty(cellText)) continue;
					if (cellText.Equals("Working")) {
						e.Row.Cells[i].BackColor = System.Drawing.Color.Purple;
						e.Row.Cells[i].ForeColor = System.Drawing.Color.White;
					}
					else if (cellText.Contains("In ") | cellText.Contains("Out ")) {
						e.Row.Cells[i].BackColor = System.Drawing.Color.Red;
						e.Row.Cells[i].ForeColor = System.Drawing.Color.White;
					}
				}
			}
		}

	}
}