using System;
using System.Data.SqlClient;
using System.Data;

namespace CTBTeam {
	public partial class Admin : SuperPage {
		SqlConnection objConn;

		protected void Page_Load(object sender, EventArgs e) {
			objConn = openDBConnection();

			if (!IsPostBack) {
				populateUsers();
				populateProjects();
				populateVehicles();
				successDialog(successOrFail);
			}
		}

		protected void User_Clicked(object sender, EventArgs e) {
			if (string.IsNullOrEmpty(txtName.Text)) {
				throwJSAlert("Error: name blank! Please fill in all fields!");
				return;
			}
			try {

				objConn.Open();

				if (!int.TryParse(txtAlna.Text, out int alna)) {
					throwJSAlert("Alna number is not a number");
					return;
				}

				object[] o = { alna, txtName.Text, !chkPartTime.Checked };
				executeVoidSQLQuery("INSERT INTO Employees (Alna_num, Name, Full_Time) VALUES (@value1, @value2, @value3);", o, objConn);

				objConn.Close();
				Session["success?"] = true;
				redirectSafely("~/Admin");
			}
			catch (Exception ex) {
				writeStackTrace("Add User", ex);
			}
		}

		protected void Project_Clicked(object sender, EventArgs e) {
			if (!(txtProject.Text.Equals(""))) {
				try {
					char projectCategory;
					switch (category.SelectedIndex) {
						case 0:
							projectCategory = 'A';
							break;
						case 1:
							projectCategory = 'B';
							break;
						case 2:
							projectCategory = 'C';
							break;
						case 3:
							projectCategory = 'D';
							break;
						default:
							throwJSAlert("Not a valid option (did you select a radio button?)");
							return;
					}


					objConn.Open();

					object[] parameters = { txtProject.Text, projectCategory };
					executeVoidSQLQuery("INSERT INTO Projects (Name, Category) VALUES (@value1, @value2);", parameters, objConn);

					objConn.Close();

					Session["success?"] = true;
					redirectSafely("~/Admin");
				}
				catch (Exception ex) {
					writeStackTrace("Add Project", ex);
				}
			}
			else {
				throwJSAlert("Project must have a name");
			}
		}

		protected void Car_Clicked(object sender, EventArgs e) {
			if (string.IsNullOrEmpty(txtCar.Text)) {
				throwJSAlert("Car needs a name");
				return;
			}

			try {
				txtCar.Text = txtCar.Text.Replace(" ", "_");

				objConn.Open();

				executeVoidSQLQuery("INSERT INTO Vehicles (Name) VALUES (@value1);", txtCar.Text, objConn);

				objConn.Close();
				Session["success?"] = true;
				redirectSafely("~/Admin");
			}
			catch (Exception ex) {
				writeStackTrace("Add Vehicle", ex);
			}
		}

		protected void remove(object sender, EventArgs e) {
			string command;
			string text;

			if (sender.Equals(btnRemoveVehicle)) {
				command = "Update Vehicles set Active=0 WHERE ID=@value1;";
				text = txtRemoveVehicle.Text;
			}
			else if (sender.Equals(btnRemoveUser)) {
				command = "Update Employees set Active=0 where Alna_num=@value1";
				text = txtRemoveUser.Text;
			}
			else if (sender.Equals(btnRemoveProject)) {
				command = "Update Projects set Active=0 WHERE ID=@value1;";
				text = txtRemoveProject.Text;
			}
			else {
				throw new ArgumentException("This button isn't implemented");
			}

			if (!int.TryParse(text, out int id)) {
				throwJSAlert("Not an integer!");
				return;
			}

			try {

				objConn.Open();

				executeVoidSQLQuery(command, id, objConn);

				objConn.Close();
				Session["success?"] = true;
				redirectSafely("~/Admin");
			}
			catch (Exception ex) {
				writeStackTrace("Remove Vehicle", ex);
			}
		}

		public void populateUsers() {
			try {

				objConn.Open();
				SqlCommand objCmdSelect = new SqlCommand("SELECT Alna_num, Employees.[Name], Full_time from Employees where Active=1 ORDER BY Alna_num", objConn);
				SqlDataAdapter objAdapter = new SqlDataAdapter();
				objAdapter.SelectCommand = objCmdSelect;
				DataSet objDataSet = new DataSet();
				objAdapter.Fill(objDataSet);
				dgvUsers.DataSource = objDataSet.Tables[0].DefaultView;
				dgvUsers.DataBind();
				objConn.Close();
			}
			catch (Exception ex) {
				writeStackTrace("Populate users", ex);
			}

		}
		public void populateProjects() {
			try {

				objConn.Open();
				SqlCommand objCmdSelect = new SqlCommand("SELECT ID, Vehicles.[Name] FROM Vehicles where Active=1;", objConn);
				SqlDataAdapter objAdapter = new SqlDataAdapter();
				objAdapter.SelectCommand = objCmdSelect;
				DataSet objDataSet = new DataSet();
				objAdapter.Fill(objDataSet);
				dgvCars.DataSource = objDataSet.Tables[0].DefaultView;
				dgvCars.DataBind();
				objConn.Close();
			}
			catch (Exception ex) {
				writeStackTrace("Populate Projects", ex);
			}

		}
		public void populateVehicles() {
			try {

				objConn.Open();
				SqlCommand objCmdSelect = new SqlCommand("SELECT ID, Name, Category FROM Projects where Active=1;", objConn);
				SqlDataAdapter objAdapter = new SqlDataAdapter();
				objAdapter.SelectCommand = objCmdSelect;
				DataSet objDataSet = new DataSet();
				objAdapter.Fill(objDataSet);
				dgvProjects.DataSource = objDataSet.Tables[0].DefaultView;
				dgvProjects.DataBind();
				objConn.Close();
			}
			catch (Exception ex) {
				writeStackTrace("Populate Vehicles", ex);
			}

		}
	}
}