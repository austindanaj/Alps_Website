using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.IO;
using Date = System.DateTime;


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
			SqlConnection objConn = openDBConnection();
			objConn.Open();

			if(!Date.TryParse(ddlselectWeek.SelectedValue, out Date date)) {
				throwJSAlert("Not a valid date");
				return;
			}

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

			DataTable allEmployees = getDataTable("select Alna_num, Name from Employees where Active=@value1", true, objConn);
			object[] parameters = { true, false };
			DataTable partTimeEmployees = getDataTable("select Alna_num, Name from Employees where Active=@value1 and Full_time=@value2", parameters, objConn);
			DataTable projects = getDataTable("select ID, Name from Projects where Active=@value1", true, objConn);
			DataTable vehicles = getDataTable("select ID, Name from Vehicles where Active=@value1", true, objConn);
			DataTable projectHours = getDataTable("select Alna_num, Proj_ID, Hours_worked from ProjectHours where Date_ID=@value1", dateID, objConn);
			DataTable vehicleHours = getDataTable("select Alna_num, Vehicle_ID, Hours_worked from VehicleHours where Date_ID=@value1", dateID, objConn);
			objConn.Close();

			if (null == allEmployees || null == partTimeEmployees || null == projects || null == vehicles || null == projectHours || null == vehicleHours) {
				throwJSAlert("Failed to get data");
				return;
			}

			DataTable projectDataTable = new DataTable();      //Data table to temporarily store the table before the gridview
			DataTable vehicleDataTable = new DataTable();
			Hashtable h = new Hashtable();                  //Temp Hash table for projects and vehicles
			Hashtable employeeHashTable = new Hashtable();

			projectDataTable.Columns.Add("Name", typeof(string));

			int i;    //Scrap int to use throughout the method: we have to reuse it a lot
			string s;

			Lambda addColumns = new Lambda(delegate (object o) {
				if (!(o is DataTable))
					return;
				DataTable d = (DataTable)o;
				DataTable whatDataSet = o.Equals(projects) ? projectDataTable : vehicleDataTable;

				for (i = 0; i < d.Rows.Count; i++) {       //Forall rows in projectData: 
					s = d.Rows[i][1].ToString();                //Get the name of the project
					h.Add(projects.Rows[i][0], s);           //Add it to the hash table with the Proj_Id as the key
					whatDataSet.Columns.Add(s, typeof(int));  //Add it as a column to the temporary datatable. The column accepts integer values because we're talking about hours worked
				}
			});

			addColumns(projects);

			i = 0;
			DataRow temp;

			DataRow[] tempTable = new DataRow[allEmployees.Rows.Count];
			foreach (DataRow d in allEmployees.Rows) {
				temp = projectDataTable.NewRow();  //Make a new row
				temp["Name"] = d[1];            //The name of the employee is stored in [1]
				employeeHashTable.Add(d[0], i); //Add the employee's ID to the hashtable to quickly get what row its in later
				tempTable[i] = temp;            //Assign the new row to tempTable
				i++;
			}

			Lambda insertCells = new Lambda(delegate (object o) {
				DataTable table = (DataTable)o;
				foreach (DataRow d in table.Rows) {
					int whatRow = (int)employeeHashTable[d[0]]; //d[0] holds the alna number
					string whatCol = (string)h[d[1]]; //d[1] holds the Project ID
					if (whatCol == null)
						continue;
					tempTable[whatRow][whatCol] = d[2]; //d[2] holds the hours worked on the project
				}
			});

			insertCells(projectHours);

			foreach (DataRow d in tempTable)
				projectDataTable.Rows.Add(d);

			//Done with projects, time to move onto the vehicles

			h = new Hashtable();

			vehicleDataTable.Columns.Add("Name", typeof(string));

			addColumns(vehicles);

			i = 0;
			tempTable = new DataRow[partTimeEmployees.Rows.Count];
			foreach (DataRow d in partTimeEmployees.Rows) {
				temp = vehicleDataTable.NewRow();
				temp["Name"] = d[1];
				tempTable[i] = temp;
				i++;
			}

			insertCells(vehicleHours);

			foreach (DataRow d in tempTable)
				vehicleDataTable.Rows.Add(d);

			//Begin doing the file write
			try {
				string fileName = @"" + Server.MapPath("~/Logs/DBLog.csv");
				File.Create(fileName).Dispose();
				StreamWriter file = new StreamWriter(fileName);

				addColumns = new Lambda(delegate (object o) {
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
				Response.End();
			}
			catch (Exception ex) {
				writeStackTrace("Something wrong in file writing", ex);
				throwJSAlert("Something wrong with the directory structure; contact an admin");
			}
		}
	}
}