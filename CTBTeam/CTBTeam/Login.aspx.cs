using System;
using System.Data.SqlClient;
using System.Threading;

namespace CTBTeam {
	public partial class Login : SuperPage {
		SqlConnection objConn;

		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				if (Session["User"] != null) {
					Session["User"] = null;
					Session["loginStatus"] = "Sign in";
					redirectSafely("~/");
				}
				loadEmployees();
			}
		}

		private void loadEmployees() {
			objConn = openDBConnection();
			objConn.Open();
			SqlCommand objCmd = new SqlCommand("SELECT Name FROM Employees", objConn);
			SqlDataReader reader = objCmd.ExecuteReader();
			while (reader.Read()) {
				ddl.Items.Add(reader.GetString(0));
			}
		}

		protected void Login_Clicked(Object sender, EventArgs e) {
			try {
				objConn = openDBConnection();
				objConn.Open();

				SqlCommand objCmd = new SqlCommand("SELECT User, Admin FROM Accounts WHERE Accounts.[User]=@value1 and Accounts.[Pass]=@value2", objConn);
				objCmd.Parameters.AddWithValue("@value1", txtUser.Text);
				objCmd.Parameters.AddWithValue("@value2", txtPass.Text);
				SqlDataReader reader = objCmd.ExecuteReader();

				if (!reader.HasRows) {
					throwJSAlert("Incorrect username or password");
					reader.Close();
					return;
				}
				reader.Read();
				Session["Admin"] = reader.GetBoolean(1);
				reader.Close();

				objCmd = new SqlCommand("Select Alna_num from Employees where Employees.[Name]=@value1", objConn);
				objCmd.Parameters.AddWithValue("@value1", ddl.Text);
				reader = objCmd.ExecuteReader();
				reader.Read();
				Session["User"] = reader.GetValue(0);
				Session["loginStatus"] = "Sign Out";
				redirectSafely("~/");
			}
			catch (ThreadAbortException ex) { }
			catch (Exception ex) {
				writeStackTrace("Login", ex);
			}
		}
	}
}