using System;
using System.Data.SqlClient;
using System.Data;
using Date = System.DateTime;
using System.Collections;
using System.IO;

namespace CTBTeam {
	public partial class _Default : SuperPage {
		private static readonly string FILENAME = "DBLog.csv";

		protected void Page_Load(object sender, EventArgs e) {

		}

		protected void View_More_onClick(object sender, EventArgs e) {
			redirectSafely("Hours.aspx");
		}

		protected void download(object sender, EventArgs e) {
			SqlConnection objConn = openDBConnection();
			objConn.Open();

			SqlDataReader reader = getReader("select ID, Dates from Dates order by ID desc", null, objConn);
			reader.Read();
			Session["Date_ID"] = reader.GetInt32(0);
			Session["Date"] = reader.GetDateTime(1);
			reader.Close();

			DataTable allEmployees = getDataTable("select Alna_num, Name from Employees where Active=@value1", true, objConn);
			object[] parameters = { true, false };
			DataTable partTimeEmployees = getDataTable("select Alna_num, Name from Employees where Active=@value1 and Full_time=@value2", parameters, objConn);
			DataTable projects = getDataTable("select ID, Name from Projects", null, objConn);
			DataTable vehicles = getDataTable("select ID, Name from Vehicles where Active=@value1", true, objConn);
			DataTable projectHours = getDataTable("select Alna_num, Proj_ID, Hours_worked from ProjectHours where Date_ID=@value1", Session["Date_ID"], objConn);
			DataTable vehicleHours = getDataTable("select Alna_num, Vehicle_ID, Hours_worked from VehicleHours where Date_ID=@value1", Session["Date_ID"], objConn);
			objConn.Close();

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
				DataTable whatDataSet;
				if (o.Equals(projects))
					whatDataSet = projectDataTable;
				else
					whatDataSet = vehicleDataTable;

				for (i = 0; i < d.Rows.Count; i++) {       //Forall rows in projectData: 
					s = d.Rows[i][1].ToString();                //Get the name of the project
					h.Add(projects.Rows[i][0], s);           //Add it to the hash table with the Proj_Id as the key
					whatDataSet.Columns.Add(s, typeof(int));  //Add it as a column to the temporary datatable. The column accepts integer values because we're talking about hours worked
				}
			});

			addColumns(projects);

			i = 0;
			DataRow temp;

			//Why do we need this? Because C# wont let you edit rows after they're added.
			//So we have to put everything into an array first so we can add all the hours
			//then we insert into the datatable.
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
					if (!employeeHashTable.ContainsKey(d[0]))   //If they're a full time employee skip them
						continue;
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

			File.Create(@"" + Server.MapPath("~/Logs/" + FILENAME)).Dispose();
			StreamWriter file = new StreamWriter(@"" + Server.MapPath("~/Logs/" + FILENAME));

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
			Response.AppendHeader("Content-Disposition", "attachment; filename="+FILENAME);
			Response.TransmitFile(Server.MapPath("~/Logs/"+FILENAME));
			Response.End();
		}
	}
}