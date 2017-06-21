﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using Date = System.DateTime;

namespace CTBTeam {
	public delegate void Lambda(object o);
	public class SuperPage : Page {
		private readonly static string LOCALHOST_CONNECTION_STRING = "Data Source=(LocalDB)\\v13.0;Server = (localdb)\\MSSQLLocalDB;Database=Alps;";
		private readonly static string DEPLOYMENT_CONNECTION_STRING = "Data Source = (LocalDB)\\v13.0;Server = (localdb)\\MSSQLLocalDB;Database=Alps;";
		private enum SqlTypes { DataTable, VoidQuery, DataReader };

		protected void writeStackTrace(string s, Exception ex) {
			if (!File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt"))) {
				File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
			}
			using (StreamWriter file = new StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt"))) {
				file.WriteLine(Date.Today.ToString() + s + ex.ToString());
				file.Close();
			}
		}

		protected SqlConnection openDBConnection() {
			return new SqlConnection(DEPLOYMENT_CONNECTION_STRING);
			//return new SqlConnection(DEPLOYMENT_CONNECTION_STRING);
		}

		protected void throwJSAlert(string s) {
			try {
				Response.Write("<script>alert('" + s + "');</script>");
			}
			catch (System.Web.HttpException h) {
				writeStackTrace("", h);
			}
		}

		//This code handles all sql related exceptions
		private object sqlExecuter(object o, SqlTypes type) {
			try {
				switch (type) {
					case SqlTypes.DataReader:
						return ((SqlCommand)o).ExecuteReader();
					case SqlTypes.VoidQuery:
						return ((SqlCommand)o).ExecuteNonQuery();
					case SqlTypes.DataTable:
						object[] adapterAndDataSet = (object[])o;
						SqlDataAdapter objAdapter = (SqlDataAdapter)adapterAndDataSet[0];
						DataSet objDataSet = (DataSet)adapterAndDataSet[1];
						objAdapter.Fill(objDataSet);
						return objDataSet;
				}
			}
			catch (ObjectDisposedException e) {
				throwJSAlert("The sql connection was disposed of. This could be a code error.");
				writeStackTrace("either the object you're using is closed or something insane happened like a race condition", e);
			}
			catch (InvalidOperationException e) {
				throwJSAlert("There was a minor error in code: InvalidOperationException");
				writeStackTrace("some sort of simple error just occurred (data was edited between execution, something was closed that shouldn't have been, etc.", e);
			}
			catch (InvalidCastException e) {
				writeStackTrace("Something didn't cast correctly; check params", e);
				throwJSAlert("Someone may have screwed the code up. Tell someone to check the stack traces");
			}
			catch (SqlException e) {
				SqlErrorCollection errors = e.Errors;

				//Severity level of 10 or less: mistakes in information that a user has entered.
				//Severity levels from 11 through 16 are generated by the user, and can be corrected by the user
				//Severity levels from 17 through 25 indicate software or hardware errors
				//When a level 17, 18, or 19 error occurs, you can continue working, although you might not be able to execute a particular statement.
				bool failure = false, codeError = false;
				foreach (SqlError error in errors) {
					if (error.Class > 16) {
						failure = true;
						break;
					}
					else if (error.Class > 10) {
						codeError = true;
						break;
					}
				}

				if (failure)
					throwJSAlert("Program crashed because of hardware of software failure not related to our code. Try again, it may work");
				else if (codeError)
					throwJSAlert("Program crashed either because your input was bad or the code may have screwed up. Try again, it may work");
				else
					throwJSAlert("There was some sort of failure with SQL. Make sure your input is good. Try again, it may work");

				writeStackTrace("sql exception in executeVoidQuery", e);
			}
			catch (IOException e) {
				throwJSAlert("There was a hard crash. This is probably hardware related. Contact Austin or Anthony");
				writeStackTrace("io in executeVoidQuery", e);
			}
			catch (Exception e) {
				throwJSAlert("There was some arbitrary failure. try again, it may work.");
				writeStackTrace("executeVoidSQLQuery", e);
			}
			return null;
		}

		protected void executeVoidSQLQuery(string command, object[] parameters, SqlConnection conn) {
			bool state = conn.State == ConnectionState.Closed;
			if (state)
				conn.Open();

			SqlCommand objCmd = new SqlCommand(command, conn);

			int i = 1;
			foreach (object s in parameters) {
				objCmd.Parameters.AddWithValue("@value" + i, s);
				i++;
			}
			sqlExecuter(objCmd, SqlTypes.VoidQuery);
			if (state)
				conn.Close();
		}

		protected void executeVoidSQLQuery(string command, object parameter, SqlConnection conn) {
			bool state = conn.State == ConnectionState.Closed;
			if (state)
				conn.Open();

			SqlCommand objCmd = new SqlCommand(command, conn);

			if (null != parameter) {
				objCmd.Parameters.AddWithValue("@value1", parameter);
			}
			sqlExecuter(objCmd, SqlTypes.VoidQuery);

			if (state)
				conn.Close();
		}

		protected void successDialog(System.Web.UI.WebControls.TextBox successOrFail) {
			if (Session["success?"] != null)
				successOrFail.Visible = (bool)Session["success?"];
			Session["success?"] = false;
		}

		protected void redirectSafely(string path) {
			try {
				Server.ClearError();
				Response.Redirect(path, false);
				Context.ApplicationInstance.CompleteRequest();
			}
			catch (Exception e) {
				throwJSAlert("We tried to redirect your page, but something went wrong, most likely in the network.");
				writeStackTrace("problem redirecting", e);
			}
		}

		protected DataTable getDataTable(string command, object parameter, SqlConnection objConn) {
			bool state = objConn.State == ConnectionState.Closed;
			if (state)
				objConn.Open();

			SqlDataAdapter objAdapter = new SqlDataAdapter();
			DataSet objDataSet = new DataSet();
			SqlCommand cmd = new SqlCommand(command, objConn);
			if (null != parameter)
				cmd.Parameters.AddWithValue("@value1", parameter);
			objAdapter.SelectCommand = cmd;
			object[] o = { objAdapter, objDataSet };
			objDataSet = (DataSet) sqlExecuter(o, SqlTypes.DataTable);

			if (state)
				objConn.Close();

			return objDataSet == null ? null : objDataSet.Tables[0];
		}

		protected DataTable getDataTable(string command, object[] parameters, SqlConnection objConn) {
			if (parameters == null)
				return getDataTable(command, (object)null, objConn);

			bool state = objConn.State == ConnectionState.Closed;
			if (state)
				objConn.Open();

			SqlDataAdapter objAdapter = new SqlDataAdapter();
			DataSet objDataSet = new DataSet();
			SqlCommand cmd = new SqlCommand(command, objConn);
			int i = 1;
			foreach (object s in parameters) {
				cmd.Parameters.AddWithValue("@value" + i, s);
				i++;
			}
			objAdapter.SelectCommand = cmd;
			object[] o = { objAdapter, objDataSet };
			objDataSet = (DataSet)sqlExecuter(o, SqlTypes.DataTable);

			if (state)
				objConn.Close();

			return objDataSet == null ? null : objDataSet.Tables[0];
		}

		protected SqlDataReader getReader(string query, object parameters, SqlConnection objConn) {
			bool state = objConn.State == ConnectionState.Closed;
			if (state)
				objConn.Open();

			SqlCommand cmd = new SqlCommand(query, objConn);
			if (parameters != null) {
				cmd.Parameters.AddWithValue("@value1", parameters);
			}

			SqlDataReader reader = (SqlDataReader) sqlExecuter(cmd, SqlTypes.DataReader);

			if (state)
				objConn.Close();

			return reader;
		}

		protected SqlDataReader getReader(string query, object[] parameters, SqlConnection objConn) {
			if (parameters == null)
				return getReader(query, (object)parameters, objConn);

			bool state = objConn.State == ConnectionState.Closed;
			if (state)
				objConn.Open();

			SqlCommand cmd = new SqlCommand(query, objConn);
			int i = 1;
			foreach (object o in parameters) {
				cmd.Parameters.AddWithValue("@value" + i, o);
				i++;
			}

			SqlDataReader reader = (SqlDataReader)sqlExecuter(cmd, SqlTypes.DataReader);

			if (state)
				objConn.Close();

			return reader;
		}

		protected void initDate(SqlConnection objConn) {
			bool state = objConn.State == ConnectionState.Closed;
			if (state)
				objConn.Open();

			SqlDataReader reader = getReader("select top 1 Dates, ID from Dates order by ID DESC;", null, objConn);
			if (reader == null) return;
			reader.Read();
			Date date = (Date)reader.GetValue(0);
			int id = (int)reader.GetValue(1);
			reader.Close();
			if (Date.Today > date.AddDays(6)) {
				date = date.AddDays(7);
				while (Date.Today > date.AddDays(6))
					date = date.AddDays(7);

				string sqlDateString = date.Year + "-" + date.Month + "-" + date.Day;
				executeVoidSQLQuery("insert into Dates (Dates.[Dates]) values (@value1)", sqlDateString, objConn);
				reader = getReader("select top 1 ID, Dates from Dates order by ID desc", null, objConn);

				if (reader == null) return;

				reader.Read();
				Session["Date_ID"] = (int)reader.GetValue(0);
				Session["Date"] = (Date)reader.GetValue(1);
				reader.Close();
			}
			else {
				Session["Date"] = date;
				Session["Date_ID"] = id;
			}

			if (state)
				objConn.Close();
		}
	}

	public class HoursPage : SuperPage {
		SqlConnection objConn;

		protected DataTable getProjectHours(object date, bool includeFullTimers) {
			return getFormattedDataTable(date, includeFullTimers, true);
		}

		protected DataTable getVehicleHours(object date) {
			return getFormattedDataTable(date, false, false);
		}

		//Can be date or dateID
		private DataTable getFormattedDataTable(object date, bool includeFullTimers, bool isProjectHours) {
			/*
			 * Need to put the partTimeEmployee/vehicle records into gridview.
			 * Due to really annoying limitations of SQL and C#, this is
			 * hard to do. Performance is also something I wanted to keep in mind because
			 * network latency and many SQL queries is an issue with this page more than any other.
			 * 
			 * We need this:
			 * +---------+---------+---------
			 * |theName  |project1 |moreProj  ...
			 * +---------+---------+---------
			 * |employee1|# of hrs | # of hrs ...
			 * +---------+---------+---------
			 * |employee2|# of hrs | # of hrs ...
			 * +---------+---------+---------
			 * |employee3|# of hrs | # of hrs ...
			 * +---------+---------+---------
			 * |employee4|# of hrs | # of hrs ...
			 * +---------+---------+---------
			 * You might think "We could do this in SQL with complicated joins", but you can't because it requires using rows in the Projects tables as the columns.
			 * Sql doesn't allow that, so we do it in C#, but it's a pain and potentially slow; BUT since the # of hours cells can be empty if the hours worked are 0,
			 * we can pull off ~Omega(2*#ofEmployees + #ofProjects + #ofVehicles), which is pretty damn good (still O(n^2), but this is 2D though, what can you expect)
			 * 
			 * 
			 * 
			 * Here's how it works:
			 * Step 1: take all the projects and put them in two things: hash table and the DataTable, as columns			 
			 * +------+------+-----+
			 * |name  |proj1 |proj2|	HashTable1(ProjID1 -> proj1'sIndexInTable (which is 1), ProjID2 -> proj2'sIndexInTable (which is 2), ...)
			 * +------+------+-----+
			 * 
			 * Step 2: This was the annoying part thanks to C#. The next intuitive step would be to start populating rows, but if we do that,
			 * then we cant edit them. So what I did instead was make a List<DataRow> so they can be edited. While I put them in there, I also put them in their own Hashtable.
			 * 
			 * The Datatable
			 * +------+------+-----+
			 * |name  | proj1|proj2|	HashTable1(ProjID1 -> proj1'sIndexInTable (which is 1), ProjID2 -> proj2'sIndexInTable (which is 2), ...)
			 * +------+------+-----+	HashTable2(
			 * 
			 * The List<DataRow>
			 * 
			 * List[0] = DataRow(Name: "Anthony Hewins", proj1: null, proj2: null, ...)
			 * List[1] = DataRow(Name: "Austin Danaj", proj1: null, proj2: null, ...)
			 * etc.
			 * 
			 * Step 3: using the hashtable, finally fill the DataTable:
			 * 
			 * Pretend Austin worked 3 hours for proj2. It would go like this
			 * 
			 * foreach DataRow in ProjectHours
			 *	 row# = Hashtable1.getWhatRowThisAlnaNumberIs(alna_num_supplied_from_ProjectHoursTable) //Remember: hash table takes the employee Alna and returns what row they are in the List
			 *	 col# = Hashtable2.getWhatColThisProjectIs(proj_ID_supplied_from_ProjectHoursTable)
			 *	 tempDatatable[row#][col#] = hours_spent_on_project_supplied_from_ProjectHoursTable
			 * 
			 *																  col#
			 *																	|
			 *																	V
			 * 
			 *			List[0] = DataRow(Name: "Anthony Hewins", proj1: null, proj2: null, ...)
			 *	row#->	List[1] = DataRow(Name: "Austin Danaj", proj1: null, proj2: 3, ...)
			 *	
			 *	
			 *	When done, the whole thing is filled.
			 *
			 *
			 * Step 4: Now the easy part, forall datarows in the List, add them to the DataTable
			 * Step 5: bind the data to the gridview at the very end
			 * Step 6: repeat for vehicles
			 */

			string constraint;
			if (date is Date) {
				date = (Date)date;
				constraint = "(select ID from Dates where Dates=@value1)";
			}
			else if (date is int) {
				date = (int)date;
				constraint = "@value1";
			}
			else
				return null;

			string modelTable, hoursTable, innerID;
			if (isProjectHours) {
				modelTable = "Projects";
				hoursTable = "ProjectHours";
				innerID = "Proj_ID";
			} else {
				modelTable = "Vehicles";
				hoursTable = "VehicleHours";
				innerID = "Vehicle_ID"; 
			}


			objConn = objConn == null ? openDBConnection() : objConn;
			bool state = objConn.State == ConnectionState.Closed;
			if (state) objConn.Open();
			DataTable employeesData = getDataTable("select Alna_num, Name, Full_time from Employees where Active=@value1", true, objConn);
			DataTable modelData = getDataTable("select ID, Name from " + modelTable + "  where Active=@value1", true, objConn);
			DataTable hoursData = getDataTable("select Alna_num, " + innerID+ ", Hours_worked from " + hoursTable + " where Date_ID=" + constraint, date, objConn);
			if (state) objConn.Close();

			if (null == employeesData || null == modelData || null == hoursData)
				return null;

			int colAndRowTracker = 0;
			Dictionary<int, int> employeeHashTable = new Dictionary<int, int>();
			Dictionary<int, int> modelHashTable = new Dictionary<int, int>();        //I had to make 3 separate hash tables because there might be collisions

			DataTable modelDataTable = new DataTable();
			modelDataTable.Columns.Add("Name");

			foreach (DataRow d in modelData.Rows) {
				modelHashTable.Add((int)d[0], colAndRowTracker + 1); //Add one because column 0 is name
				modelDataTable.Columns.Add((string)d[1]);
				colAndRowTracker++;
			}

			colAndRowTracker = 0;

			DataRow temp;
			List<DataRow> tempMatrix = new List<DataRow>();
			foreach (DataRow d in employeesData.Rows) {
				if ((bool)d[2] & !includeFullTimers)
					continue;
				employeeHashTable.Add((int)d[0], colAndRowTracker);
				temp = modelDataTable.NewRow();
				temp["Name"] = d[1];
				tempMatrix.Add(temp);
				colAndRowTracker++;
			}

			int whatCol, whatRow;
			foreach (DataRow d in hoursData.Rows) {
				int alna = (int)d[0];
				if (!employeeHashTable.ContainsKey(alna))   //We skip full time employees since they will not appear in the Hashtable
					continue;
				whatCol = modelHashTable[(int)d[1]];
				whatRow = employeeHashTable[alna];
				tempMatrix[whatRow][whatCol] = d[2];
			}

			foreach (DataRow d in tempMatrix)
				modelDataTable.Rows.Add(d);

			return modelDataTable;
		}
	}
}