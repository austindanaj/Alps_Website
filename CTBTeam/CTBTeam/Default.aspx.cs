using System;
using System.Data.SqlClient;
using System.Data;
using Date = System.DateTime;
using System.Collections;

namespace CTBTeam {
	public partial class _Default : SuperPage {
		protected void Page_Load(object sender, EventArgs e) {
			
		}

		protected void View_More_onClick(object sender, EventArgs e) {
			redirectSafely("Hours.aspx");
		}
		
		protected void download(object sender, EventArgs e) {
			SqlConnection objConn = openDBConnection();
			objConn.Open();

			

			DataTable tempDataTable = new DataTable();      //Data table to temporarily store the table before the gridview
			Hashtable h = new Hashtable();                  //Temp Hash table for projects and vehicles
			Hashtable employeeHashTable = new Hashtable();  //Hash table for employees (there doesn't need to be 3 but it's simpler this way)

			tempDataTable.Columns.Add("Name", typeof(string));

			int i;    //Scrap int to use throughout the method: we have to reuse it a lot
			string s;

			Lambda addColumns = new Lambda(delegate (object o) {
				if (!(o is DataTable))
					return;
				DataTable d = (DataTable)o;
				for (i = 0; i < smallestThreshold; i++) {       //Forall rows in projectData: 
					s = d.Rows[i][1].ToString();                //Get the name of the project
					h.Add(projectData.Rows[i][0], s);           //Add it to the hash table with the Proj_Id as the key
					tempDataTable.Columns.Add(s, typeof(int));  //Add it as a column to the temporary datatable. The column accepts integer values because we're talking about hours worked
				}
			});

			addColumns(projectData);

			i = 0;
			DataRow temp;

			//Why do we need this? Because C# wont let you edit rows after they're added.
			//So we have to put everything into an array first so we can add all the hours
			//then we insert into the datatable.
			DataRow[] tempTable = new DataRow[partTimeEmployeeData.Rows.Count];
			foreach (DataRow d in partTimeEmployeeData.Rows) {
				temp = tempDataTable.NewRow();  //Make a new row
				temp["Name"] = d[1];            //The name of the employee is stored in [1]
				employeeHashTable.Add(d[0], i); //Add the employee's ID to the hashtable to quickly get what row its in later
				tempTable[i] = temp;            //Assign the new row to tempTable
				i++;
			}

			Lambda insertCells = new Lambda(delegate (object o) {
				if (!(o is DataTable))
					return;
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

			insertCells(projectHoursData);

			foreach (DataRow d in tempTable)
				tempDataTable.Rows.Add(d);

			dgvProject.DataSource = tempDataTable;
			dgvProject.DataBind();

			//Done with projects, time to move onto the vehicles

			tempDataTable = new DataTable();
			h = new Hashtable();

			tempDataTable.Columns.Add("Name", typeof(string));
			smallestThreshold = requestedColNum >= vehiclesData.Rows.Count ? vehiclesData.Rows.Count : requestedColNum;

			addColumns(vehiclesData);

			i = 0;
			foreach (DataRow d in partTimeEmployeeData.Rows) {
				temp = tempDataTable.NewRow();
				temp["Name"] = d[1];
				tempTable[i] = temp;
				i++;
			}

			insertCells(vehicleHoursData);

			foreach (DataRow d in tempTable)
				tempDataTable.Rows.Add(d);

			dgvCars.DataSource = tempDataTable;
			dgvCars.DataBind();
		}
	}
}