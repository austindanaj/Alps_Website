using System;
using System.Data.OleDb;
using System.Threading;

namespace CTBTeam {
	public partial class Login : SuperPage {
		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				if (!string.IsNullOrEmpty((string)Session["User"])) {
					Session["User"] = null;
					Session["loginStatus"] = "Sign in";
					Response.Redirect("~/");
				}
			}
		}

		protected void Login_Clicked(Object sender, EventArgs e) {
			try {
				OleDbConnection objConn = openDBConnection();
				objConn.Open();

				OleDbCommand objCmd = new OleDbCommand("SELECT * FROM Account WHERE ACCT_ROLE=@value1 AND ACCT_PASSWORD=@value2", objConn);

				objCmd.Parameters.AddWithValue("@value1", txtUser.Text);
				objCmd.Parameters.AddWithValue("@value2", txtPass.Text);

				OleDbDataReader reader = objCmd.ExecuteReader();
				int count = 0;
				while (reader.Read()) {
					count++;
					Session["User"] = reader.GetString(1);
					Session["admin"] = reader.GetBoolean(3);
				}
				if (count > 0) {
					Session["loginStatus"] = "Sign Out";
					Response.Redirect("Hours.aspx");
					objConn.Close();
					return;
				}
				else {
					throwJSAlert("Incorrect Username or Password");
					objConn.Close();
					return;
				}
			}catch (ThreadAbortException ex) {}
			catch (Exception ex) {
				writeStackTrace("Login", ex);
			}
		}
	}
}