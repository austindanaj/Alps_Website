using System;
using System.Data.OleDb;
using System.Threading;
using System.Web;
using System.Web.UI;
using Date = System.DateTime;

namespace CTBTeam {
	public class SuperPage : Page {
		private readonly string dbVersion12_0 = "Provider=Microsoft.ACE.OLEDB.12.0;";

		protected void writeStackTrace(string s, Exception ex) {
			if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt"))) {
				System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
			}
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt"))) {
				file.WriteLine(Date.Today.ToString() + s + ex.ToString());
				file.Close();
			}
		}

		protected OleDbConnection openDBConnection() {
			return new OleDbConnection(dbVersion12_0 + "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";");
		}

		protected void throwJSAlert(string s) {
			ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + s + "');");
		}

		protected void executeVoidSQLQuery(string command, object[] parameters, OleDbConnection conn) {
			try {
				if (conn == null)
					conn = openDBConnection();
				OleDbCommand objCmd = new OleDbCommand(command, conn);

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
		
		protected void successDialog(System.Web.UI.WebControls.TextBox successOrFail) {
			if (Session["success?"] != null)
				successOrFail.Visible = (bool)Session["success?"];
			Session["success?"] = false;
		}
	}
}
