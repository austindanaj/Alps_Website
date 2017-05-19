using System;
using System.Data.OleDb;
using System.Data;
using Date = System.DateTime;

namespace CTBTeam {
	public partial class _Default : SuperPage {
		protected void Page_Load(object sender, EventArgs e) {
			if (string.IsNullOrEmpty((string)Session["loginStatus"])) {
				Session["loginStatus"] = "Sign In";
			}
		}
		protected void View_More_onClick(object sender, EventArgs e) {
			redirectSafely("Hours.aspx");
		}

		protected void download_database(object sender, EventArgs e) {
			string[] arrLine = System.IO.File.ReadAllLines(@"" + Server.MapPath("~/Logs/TimeLog/Time-log.txt"));
			string date = arrLine[arrLine.Length - 1];
			string fileName = date.Replace("/", "-");
			try {

				using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Logs/CurrentHours/Current-Hours_" + fileName + ".txt"))) {
					OleDbConnection objConn = openDBConnection();
					objConn.Open();

					OleDbCommand fieldProjectNames = new OleDbCommand("SELECT * FROM ProjectHours", objConn);
					OleDbDataReader readerPNames = fieldProjectNames.ExecuteReader();
					var table = readerPNames.GetSchemaTable();
					var nameCol = table.Columns["ColumnName"];
					string headerRow = "";
					foreach (DataRow row in table.Rows) {
						if (!row[nameCol].Equals("ID")) {
							headerRow += row[nameCol] + ",";
						}

					}
					headerRow = Date.Parse(date).ToShortDateString() + "," + headerRow;
					file.WriteLine(headerRow);
					string text = "";

					int projectCount = 0;

					OleDbCommand getProjectList = new OleDbCommand("SELECT Project From Projects ORDER BY ID", objConn);
					OleDbDataReader readerProjectList = getProjectList.ExecuteReader();
					while (readerProjectList.Read()) {
						projectCount++;      /** Increment counter by 1 **/

					}


					OleDbCommand objProject = new OleDbCommand("SELECT * FROM ProjectHours;", objConn);

					OleDbDataReader readerProject = objProject.ExecuteReader();
					while (readerProject.Read()) {
						text = "";
						for (int i = 1; i <= projectCount; i++) {
							text += readerProject.GetValue(i).ToString() + ",";
						}

						file.WriteLine(text);
					}
					file.WriteLine();
					readerProject.Close();

					OleDbCommand fieldCarNames = new OleDbCommand("SELECT * FROM VehicleHours", objConn);
					OleDbDataReader readerCNames = fieldCarNames.ExecuteReader();
					table = readerCNames.GetSchemaTable();
					nameCol = table.Columns["ColumnName"];
					headerRow = "";
					foreach (DataRow row in table.Rows) {
						if (!row[nameCol].Equals("ID")) {
							headerRow += row[nameCol] + ",";
						}
					}
					headerRow = Date.Parse(date).ToShortDateString() + "," + headerRow;
					file.WriteLine(headerRow);
					readerCNames.Close();

					int carCount = 0;

					/** Command to get list of cars **/
					OleDbCommand getCarList = new OleDbCommand("SELECT Vehicle From Cars ORDER BY ID", objConn);
					OleDbDataReader readerCarList = getCarList.ExecuteReader();
					while (readerCarList.Read()) {
						carCount++;      /** Increment counter by 1 **/

					}
					OleDbCommand objCar = new OleDbCommand("SELECT * FROM VehicleHours;", objConn);
					text = "";
					OleDbDataReader readerCar = objCar.ExecuteReader();
					while (readerCar.Read()) {
						text = "";
						for (int i = 1; i <= carCount; i++) {
							text += readerCar.GetValue(i).ToString() + ",";
						}

						file.WriteLine(text);
					}
					file.WriteLine();
					file.Close();
					readerCar.Close();
					objConn.Close();

				}
			}

			catch (Exception ex) {
				Log.getInstance.WriteToLog("Create Current Project Hours", ex, Server);
			}
			try {
				Response.ContentType = "Application/txt";
				Response.AppendHeader("Content-Disposition", "attachment; filename=Current-Hours_" + fileName + ".txt");
				Response.TransmitFile(Server.MapPath("~/Logs/CurrentHours/Current-Hours_" + fileName + ".txt"));
				Response.End();
			}
			catch (Exception ex) {
				Log.getInstance.WriteToLog("Download Database", ex, Server);
			}
		}

		protected void download_timelog(object sender, EventArgs e) {
			Response.ContentType = "Application/txt";
			Response.AppendHeader("Content-Disposition", "attachment; filename=Time-log.txt");
			Response.TransmitFile(Server.MapPath("~/Logs/TimeLog/Time-log.txt"));
			Response.End();
		}
		protected void download_file_hexGenerator(object sender, EventArgs e) {
			Response.ContentType = "Application/exe";
			Response.AppendHeader("Content-Disposition", "attachment; filename=HexGenerator.exe");
			Response.TransmitFile(Server.MapPath("~/HexGenerator.exe"));
			Response.End();
		}

		protected void download_Phones_file(object sender, EventArgs e) {
			Response.ContentType = "Application/txt";
			Response.AppendHeader("Content-Disposition", "attachment; filename=Phone-report.txt");
			Response.TransmitFile(Server.MapPath("~/Logs/PhoneLog/Phone-report.txt"));
			Response.End();
		}
	}
}