using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using Date = System.DateTime;
using System.Collections.Generic;
using System.Web;

namespace CTBTeam {
	public partial class _Default : HoursPage {
		private SqlConnection objConn;

		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				objConn = openDBConnection();
				objConn.Open();
				SqlDataReader reader = getReader("select Dates from Dates order by Dates desc", null, objConn);
				if (reader == null) {
					throwJSAlert("Can't connect to DB; contact admin");
					return;
				}
				while (reader.Read())
					ddlselectWeek.Items.Add(reader.GetDateTime(0).ToShortDateString());
				reader.Close();
				objConn.Close();
			}
				
		}

		protected void toetruck(object sender, EventArgs e) {
			redirectSafely("~/ToeTruck");
		}

		protected void download(object sender, EventArgs e) {
			if(!Date.TryParse(ddlselectWeek.SelectedValue, out Date date)) {
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
				string s, fileName = @"" + Server.MapPath("~/Logs/DBLog.csv");
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
	}
}