using System;
using System.Data.OleDb;
using System.Data;
using System.Threading;

namespace CTBTeam {
	public partial class Admin : SuperPage {
		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				populateUsers();
				populateProjects();
				populateVehicles();
				successDialog(successOrFail);
			}
		}

		protected void User_Clicked(object sender, EventArgs e) {
			if (!(txtName.Text.Equals(""))) {
				try {
					OleDbConnection objConn = openDBConnection();
					objConn.Open();

					object[] val1And2 = { txtName.Text, !chkPartTime.Checked};
					executeVoidSQLQuery("INSERT INTO Users (Emp_Name, Full_Time) VALUES (@value1, @value2);", val1And2, objConn);
					executeVoidSQLQuery("INSERT INTO ProjectHours (Emp_Name) VALUES (@value1);", txtName.Text, objConn);
					
					if (chkAddToVehcileHours.Checked == true) {
						executeVoidSQLQuery("INSERT INTO VehicleHours (Emp_Name) VALUES (@value1);", txtName.Text, objConn);
					}

					objConn.Close();
					Session["success?"] = true;
					redirectSafely("~/Admin");
				}
				catch (Exception ex) {
					writeStackTrace("Add User", ex);
				}
			}
			else {
				throwJSAlert("Error: Line blank! Please fill in all fields!");
			}
		}


		protected void Project_Clicked(object sender, EventArgs e) {
			if (!(txtProject.Text.Equals(""))) {
				try {
					string[] array = txtProject.Text.Split(',');
					if (txtProject.Text.Contains(" ")) {
						array[0] = array[0].Replace(" ", "_");
					}
					array[1] = array[1].Replace(" ", "");
					OleDbConnection objConn = openDBConnection();
					objConn.Open();

					object[] parameters = { array[0], array[1] };
					executeVoidSQLQuery("INSERT INTO Projects (Project, Category) VALUES (@value1, @value2);", parameters, objConn);
					executeVoidSQLQuery("ALTER TABLE ProjectHours ADD " + array[0] + " number;", null, objConn);
					executeVoidSQLQuery("UPDATE ProjectHours SET " + array[0] + " =0;", null, objConn);

					objConn.Close();

					Session["success?"] = true;
					redirectSafely("~/Admin");
				}
				catch (Exception ex) {
					writeStackTrace("Add Project", ex);
				}
			}
			else {
				throwJSAlert("Fill in all fields.");
			}
		}

		protected void Car_Clicked(object sender, EventArgs e) {
			if (!(txtCar.Text.Equals(""))) {
				try {
					if (txtCar.Text.Contains(" ")) {
						txtCar.Text = txtCar.Text.Replace(" ", "_");
					}

					OleDbConnection objConn = openDBConnection();
					objConn.Open();

					executeVoidSQLQuery("INSERT INTO Cars (Vehicle) VALUES (@value1);", txtCar.Text, objConn);
					executeVoidSQLQuery("ALTER TABLE VehicleHours ADD " + txtCar.Text + " number;", null, objConn);
					executeVoidSQLQuery("UPDATE VehicleHours SET " + txtCar.Text + " =0;", null, objConn);

					objConn.Close();
					Session["success?"] = true;
					redirectSafely("~/Admin");
				}
				catch (Exception ex) {
					writeStackTrace("Add Vehicle", ex);
				}
			}
			else {
				throwJSAlert("Fill in all fields");
			}

		}

		protected void Remove_Project_Clicked(object sender, EventArgs e) {
			if (!(txtPR.Text.Equals(""))) {
				try {
					if (txtPR.Text.Contains(" ")) {
						txtPR.Text = txtCar.Text.Replace(" ", "_");
					}
					OleDbConnection objConn = openDBConnection();
					objConn.Open();

					executeVoidSQLQuery("DELETE FROM Projects WHERE Project=@value1;", txtPR.Text, objConn);
					executeVoidSQLQuery("ALTER TABLE ProjectHours DROP COLUMN " + txtPR.Text + ";", null, objConn);

					objConn.Close();
					Session["success?"] = true;
					redirectSafely("~/Admin");
				}
				catch (Exception ex) {
					writeStackTrace("Remove Project", ex);
				}
			}
			else {
				throwJSAlert("Error: Line blank! Please fill in all fields!");
			}
		}
		protected void Remove_Car_Clicked(object sender, EventArgs e) {
			if (!(txtCR.Text.Equals(""))) {
				try {
					txtCR.Text = txtCar.Text.Replace(" ", "_");
					
					OleDbConnection objConn = openDBConnection();
					objConn.Open();
					
					executeVoidSQLQuery("DELETE FROM Cars WHERE Vehicle=@value1;", txtCR.Text, objConn);
					executeVoidSQLQuery("ALTER TABLE VehicleHours DROP COLUMN " + txtCR.Text + ";", null, objConn);					

					objConn.Close();
					Session["success?"] = true;
					redirectSafely("~/Admin");
				}
				catch (Exception ex) {
					writeStackTrace("Remove Vehicle", ex);
				}
			}
			else {
				throwJSAlert("Error: Line blank! Please fill in all fields!");
			}
		}
		protected void Remove_User_Clicked(object sender, EventArgs e) {
			if (!(txtNR.Text.Equals(""))) {
				try {
					OleDbConnection objConn = openDBConnection();
					objConn.Open();

					executeVoidSQLQuery("DELETE FROM Users WHERE Emp_Name=@value1;", txtNR.Text, objConn);
					executeVoidSQLQuery("DELETE FROM ProjectHours WHERE Emp_Name=@value1;", txtNR.Text, objConn);
					executeVoidSQLQuery("DELETE FROM VehicleHours WHERE Emp_Name=@value1;", txtNR.Text, objConn);
					
					objConn.Close();
					Session["success?"] = true;
					redirectSafely("~/Admin");
				 }
				catch (Exception ex) {
					writeStackTrace("Remove User", ex);
				}
			}
			else {
				throwJSAlert("Error: Line blank! Please fill in all fields!");
			}
		}

		public void populateUsers() {
			try {
				OleDbConnection objConn = openDBConnection();
				objConn.Open();
				OleDbCommand objCmdSelect = new OleDbCommand("SELECT Emp_Name, Full_Time FROM Users ORDER BY Emp_Name", objConn);
				OleDbDataAdapter objAdapter = new OleDbDataAdapter();
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
				OleDbConnection objConn = openDBConnection();
				objConn.Open();
				OleDbCommand objCmdSelect = new OleDbCommand("SELECT Vehicle FROM Cars ORDER BY Vehicle", objConn);
				OleDbDataAdapter objAdapter = new OleDbDataAdapter();
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
				OleDbConnection objConn = openDBConnection();
				objConn.Open();
				OleDbCommand objCmdSelect = new OleDbCommand("SELECT Project, Category FROM Projects ORDER BY Project", objConn);
				OleDbDataAdapter objAdapter = new OleDbDataAdapter();
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