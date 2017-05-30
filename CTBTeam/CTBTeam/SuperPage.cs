using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using Date = System.DateTime;

namespace CTBTeam {
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
				if (conn == null)
					conn = openDBConnection();
				SqlCommand objCmd = new SqlCommand(command, conn);

				int i = 1;
				foreach (object s in parameters) {
					objCmd.Parameters.AddWithValue("@value" + i, s);
					i++;
				}
				objCmd.ExecuteNonQuery();
			} catch (Exception e) {
				writeStackTrace("executeVoidSQLQuery", e);
			}
		}

		protected void executeVoidSQLQuery(string command, object parameter, SqlConnection conn) {
			try {
				if (conn == null)
					conn = openDBConnection();
				SqlCommand objCmd = new SqlCommand(command, conn);

				if (null != parameter) {
					objCmd.Parameters.AddWithValue("@value1", parameter);
				}
				objCmd.ExecuteNonQuery();
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
			SqlDataAdapter objAdapter = new SqlDataAdapter();
			DataSet objDataSet = new DataSet();
			SqlCommand cmd = new SqlCommand(command, objConn);
			if (null != parameter) {
				cmd.Parameters.AddWithValue("@value1", parameter);
			}
			objAdapter.SelectCommand = cmd;
			objAdapter.Fill(objDataSet);
			return objDataSet.Tables[0];
		}

		protected DataTable getDataTable(string command, object[] parameters, SqlConnection objConn) {
			if (parameters == null)
				return getDataTable(command, (object)null, objConn);
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
			return objDataSet.Tables[0];
		}
	}
}
