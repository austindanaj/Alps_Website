using System;
using System.Data.SqlClient;

namespace CTBTeam {
	public partial class Login : SuperPage {
		SqlConnection objConn;

		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				if (Session["Alna_num"] != null) {
					Session.Clear();
					Session["loginStatus"] = "Sign in";
					redirectSafely("~/");
				}
				loadEmployees();
			}
		}

		private void loadEmployees() {
			objConn = openDBConnection();
			objConn.Open();
			SqlDataReader reader = getReader("SELECT Name FROM Employees where Active=@value1;", true, objConn);

			while (reader.Read()) {
				ddl.Items.Add(reader.GetString(0));
			}
		}

		protected void Login_Clicked(Object sender, EventArgs e) {
			if (ddl.SelectedIndex == 0) {
				throwJSAlert("Select your name");
				return;
			}

			try {
				objConn = openDBConnection();
				objConn.Open();

				object[] o = { txtUser.Text, txtPass.Text };
				SqlDataReader reader = getReader("SELECT User, Admin FROM Accounts WHERE Accounts.[User]=@value1 and Accounts.[Pass]=@value2", o, objConn);
				if (reader == null) {
					throwJSAlert("Error accessing data");
					return;
				}
				if(!reader.HasRows) {
					throwJSAlert("Incorrect username or password");
					reader.Close();
					return;
				}
				reader.Read();
				Session["Admin"] = reader.GetBoolean(1);
				reader.Close();

				reader = getReader("Select Alna_num, Name, Full_time, Vehicle from Employees where Employees.[Name]=@value1;", ddl.Text, objConn);
				reader.Read();
				Session["Alna_num"] = reader.GetValue(0);
				Session["Name"] = reader.GetValue(1);
				Session["Full_time"] = reader.GetValue(2);
				Session["Vehicle"] = reader.GetValue(3);
				Session["loginStatus"] = "Signed in as " + Session["Name"] + " (Sign out)";
				redirectSafely("~/");
			}
			catch (Exception ex) {
				writeStackTrace("Login", ex);
			}
		}
	}
}