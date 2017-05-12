using System;
using System.Data.OleDb;
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
				file.WriteLine(Date.Today.ToString() + " --" + s + "--" + ex.StackTrace);
				file.Close();
			}
		}

		protected OleDbConnection openDBConnection() {
			return new OleDbConnection(dbVersion12_0 + "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";");
		}
	}
}
