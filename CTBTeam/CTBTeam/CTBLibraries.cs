using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Date = System.DateTime;

namespace CTBTeam {
	public delegate void Lambda(object o);
	public class SuperPage : Page {
		private readonly static string LOCALHOST_CONNECTION_STRING = "Data Source=(LocalDB)\\v13.0;Server = (localdb)\\MSSQLLocalDB;Database=Alps;";
		private readonly static string DEPLOYMENT_CONNECTION_STRING = "";

		protected void writeStackTrace(string s, Exception ex) {
			if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt"))) {
				System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
			}
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt"))) {
				file.WriteLine(Date.Today.ToString() + s + ex.ToString());
				file.Close();
			}
		}

		protected SqlConnection openDBConnection() {
			return new SqlConnection(LOCALHOST_CONNECTION_STRING);
			//return new SqlConnection(DEPLOYMENT_CONNECTION_STRING);
		}

		protected void throwJSAlert(string s) {
			Response.Write("<script>alert('" + s + "');</script>");
		}

		protected void executeVoidSQLQuery(string command, object[] parameters, SqlConnection conn) {
			try {
				bool state = conn.State == ConnectionState.Closed;
				if (state)
					conn.Open();
					
				SqlCommand objCmd = new SqlCommand(command, conn);

				int i = 1;
				foreach (object s in parameters) {
					objCmd.Parameters.AddWithValue("@value" + i, s);
					i++;
				}
				objCmd.ExecuteNonQuery();
				if(state)
					conn.Close();
			} catch (Exception e) {
				writeStackTrace("executeVoidSQLQuery", e);
			}
		}

		protected void executeVoidSQLQuery(string command, object parameter, SqlConnection conn) {
			try {
				bool state = conn.State == ConnectionState.Closed;
				if (state)
					conn.Open();

				SqlCommand objCmd = new SqlCommand(command, conn);

				if (null != parameter) {
					objCmd.Parameters.AddWithValue("@value1", parameter);
				}
				objCmd.ExecuteNonQuery();

				if (state)
					conn.Close();
			}
			catch (Exception e) {
				writeStackTrace("executeVoidSQLQuery", e);
			}
		}

		protected void successDialog(System.Web.UI.WebControls.TextBox successOrFail) {
			if (Session["success?"] != null)
				successOrFail.Visible = (bool)Session["success?"];
			Session["success?"] = false;
		}

		protected void redirectSafely(string path) {
			Server.ClearError();
			Response.Redirect(path, false);
			Context.ApplicationInstance.CompleteRequest();
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
			objAdapter.Fill(objDataSet);

			if (state)
				objConn.Close();

			return objDataSet.Tables[0];
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
			objAdapter.Fill(objDataSet);

			if (state)
				objConn.Close();

			return objDataSet.Tables[0];
		}

		protected SqlDataReader getReader(string query, object parameters, SqlConnection objConn) {
			try {
				bool state = objConn.State == ConnectionState.Closed;
				if (state)
					objConn.Open();

				SqlCommand cmd = new SqlCommand(query, objConn);
				if (parameters != null) {
					cmd.Parameters.AddWithValue("@value1", parameters);
				}

				if (state)
					objConn.Close();
				return cmd.ExecuteReader();
			} catch (Exception ex) {
				writeStackTrace("Error trying to get reader", ex);
				return null;
			}
		}

		protected SqlDataReader getReader(string query, object[] parameters, SqlConnection objConn) {
			if (parameters == null)
				return getReader(query, (object) parameters, objConn);
			try {
				bool state = objConn.State == ConnectionState.Closed;
				if (state)
					objConn.Open();

				SqlCommand cmd = new SqlCommand(query, objConn);
				int i = 1;
				foreach (object o in parameters) {
					cmd.Parameters.AddWithValue("@value" + i, o);
					i++;
				}

				if (state)
					objConn.Close();

				return cmd.ExecuteReader();
			} catch (Exception ex) {
				writeStackTrace("Error trying to get reader", ex);
				return null;
			}
		}

		protected void initDate(SqlConnection objConn) {
			bool state = objConn.State == ConnectionState.Closed;
			if (state)
				objConn.Open();

			SqlDataReader reader = new SqlCommand("select top 1 Dates, ID from Dates order by ID DESC;", objConn).ExecuteReader();
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
}