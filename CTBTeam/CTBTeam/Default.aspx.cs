using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.IO;
using Date = System.DateTime;
using System.Collections.Generic;
using System.Web;

namespace CTBTeam {
	public partial class _Default : SuperPage {
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

			SqlConnection objConn = openDBConnection();
			objConn.Open();
			SqlDataReader reader = getReader("Select ID from Dates where Dates=@value1", date, objConn);

			if (reader == null) {
				throwJSAlert("Can't connect to DB; contact admin");
				return;
			} else if (!reader.HasRows) {
				throwJSAlert("Date isn't in DB");
				return;
			}

			reader.Read();
			int dateID = reader.GetInt32(0);
			reader.Close();

			DataTable employeesData	   = getDataTable("select Alna_num, Name from Employees where Active=@value1", true, objConn);
			DataTable projectData	   = getDataTable("select ID, Name from Projects where Active=@value1", true, objConn);
			DataTable vehiclesData	   = getDataTable("select ID, Name from Vehicles where Active=@value1", true, objConn);
			DataTable projectHoursData = getDataTable("select Alna_num, Proj_ID, Hours_worked from ProjectHours where Date_ID=@value1", dateID, objConn);
			DataTable vehicleHoursData = getDataTable("select Alna_num, Vehicle_ID, Hours_worked from VehicleHours where Date_ID=@value1", dateID, objConn);
			objConn.Close();

			if (null == employeesData || null == projectData || null == vehiclesData || null == projectHoursData || null == vehicleHoursData) {
				throwJSAlert("Failed to get data");
				return;
			}

			DataRow temp;
			DataTable projectDataTable = new DataTable();
			DataTable vehicleDataTable = new DataTable();
			Dictionary<int, int> employeeHashTable = new Dictionary<int, int>();
			Dictionary<int, int> projHashTable	   = new Dictionary<int, int>();        //I had to make 3 separate hash tables because there might be collisions
			Dictionary<int, int> vehicleHashTable  = new Dictionary<int, int>();		//with primary keys (all the PKs are autoincrements except alnas)

			projectDataTable.Columns.Add("Name");
			vehicleDataTable.Columns.Add("Name");

			int colAndRowTracker = 0;
			foreach (DataRow d in projectData.Rows) {
				projHashTable.Add((int)d[0], colAndRowTracker);
				projectDataTable.Columns.Add((string)d[1]);
				colAndRowTracker++;
			}
			colAndRowTracker = 0;
			foreach (DataRow d in vehiclesData.Rows) {
				vehicleHashTable.Add((int)d[0], colAndRowTracker);
				vehicleDataTable.Columns.Add((string)d[1]);
				colAndRowTracker++;
			}
			colAndRowTracker = 0;
			List<DataRow> projMatrix = new List<DataRow>();
			List<DataRow> vehicleMatrix = new List<DataRow>();
			foreach (DataRow d in employeesData.Rows) {                                  														
				employeeHashTable.Add((int)d[0], colAndRowTracker);
				temp = projectDataTable.NewRow();
				temp["Name"] = d[1];
				projMatrix.Add(temp);
				temp = vehicleDataTable.NewRow();
				temp["Name"] = d[1];
				vehicleMatrix.Add(temp);
				colAndRowTracker++;
			}

			int whatCol, whatRow;
			foreach (DataRow d in projectHoursData.Rows) {
				int alna = (int)d[1];
				if (!employeeHashTable.ContainsKey(alna))   //We skip full time employees since they will not appear in the Hashtable
					continue;
				whatCol = projHashTable[(int)d[2]];
				whatRow = employeeHashTable[alna];
				projMatrix[whatRow][whatCol] = d[3];
			}

			foreach (DataRow d in vehicleHoursData.Rows) {
				whatCol = vehicleHashTable[(int)d[2]];
				whatRow = employeeHashTable[(int)d[1]];
				vehicleMatrix[whatRow][whatCol] = d[3];
			}

			foreach (DataRow d in projMatrix)
				projectDataTable.Rows.Add(d);

			foreach (DataRow d in vehicleMatrix)
				vehicleDataTable.Rows.Add(d);

			//Begin doing the file write
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