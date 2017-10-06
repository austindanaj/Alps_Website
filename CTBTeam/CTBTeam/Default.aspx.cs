using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using Date = System.DateTime;
using System.Web;
using System.Web.UI.DataVisualization.Charting;

namespace CTBTeam {
	public partial class _Default : SchedulePage {
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
				populateInternSchedules(objConn, dgvSchedule, ddlSelectScheduleDay);
				objConn.Close();
			}
		}

		//----------------------------------------------------------------
		// Inits
		//----------------------------------------------------------------
		private void populatePieChart(SqlConnection objConn) {
			SqlDataReader reader = getReader("select p1.[Hours_worked], p2.Category from ProjectHours p1 inner join Projects p2 on p2.Project_ID=p1.Proj_ID where p1.Date_ID=(select top 1 ID from Dates order by ID desc);", null, objConn);
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

			HoursPage h = new HoursPage();
			DataTable projectDataTable = h.getProjectHours(date, true, !true);
			DataTable vehicleDataTable = h.getVehicleHours(date, !true);

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

		protected void changeScheduleDay(object sender, EventArgs e) {
			Session["weekday"] = ddlSelectScheduleDay.SelectedIndex + 1;
			SqlConnection objConn = openDBConnection();
			objConn.Open();
			populateInternSchedules(objConn, dgvSchedule, ddlSelectScheduleDay);
			objConn.Close();
		}
	}
}