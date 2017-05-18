using System;
using System.Data.OleDb;
using System.Data;

namespace CTBTeam {
	public partial class Admin : SuperPage {
		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				populateUsers();
				populateProjects();
				populateVehicles();
			}
		}
		protected void User_Clicked(object sender, EventArgs e) {
			if (!(txtName.Text.Equals(""))) {
				try {
					OleDbConnection objConn = openDBConnection();
					objConn.Open();

					OleDbCommand objCmd = new OleDbCommand("INSERT INTO Users (Emp_Name, Full_Time) VALUES (@value1, @value2);", objConn);
					objCmd.Parameters.AddWithValue("@value1", txtName.Text);
					objCmd.Parameters.AddWithValue("@value2", !chkPartTime.Checked);
					objCmd.ExecuteNonQuery();

					objCmd = new OleDbCommand("INSERT INTO ProjectHours (Emp_Name) VALUES (@value1);", objConn);
					objCmd.Parameters.AddWithValue("@value1", txtName.Text);
					objCmd.ExecuteNonQuery();

					if (chkAddToVehcileHours.Checked == true) {
						objCmd = new OleDbCommand("INSERT INTO VehicleHours (Emp_Name) VALUES (@value1);", objConn);
						objCmd.Parameters.AddWithValue("@value1", txtName.Text);
						objCmd.ExecuteNonQuery();
					}

					objConn.Close();
					//     populateUsers();
					Response.Redirect("~/Admin");
					ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('User successfully added');", true);
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

					OleDbCommand objCmd = new OleDbCommand("INSERT INTO Projects (Project, Category) VALUES (@value1, @value2);", objConn);
					objCmd.Parameters.AddWithValue("@value1", array[0]);
					objCmd.Parameters.AddWithValue("@value2", array[1]);

					objCmd.ExecuteNonQuery();

					objCmd = new OleDbCommand("ALTER TABLE ProjectHours ADD " + array[0] + " number;", objConn);
					//  objCmd = new OleDbCommand("ALTER TABLE VehicleHours ADD " + txtCar.Text + " number;", objConn);
					//  objCmd.Parameters.AddWithValue("@value1", txtName.Text);
					objCmd.ExecuteNonQuery();

					objCmd = new OleDbCommand("UPDATE ProjectHours SET " + array[0] + " =0;", objConn);
					objCmd.ExecuteNonQuery();

					objConn.Close();
					// populateProjects();
					Response.Redirect("~/Admin");
					ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Project successfully added');", true);
				}
				catch (Exception ex) {
					writeStackTrace("Add Project", ex);
				}
			}
			else {
				ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Error: Line blank! Please fill in all fields!');", true);
			}
		}
		protected void Car_Clicked(object sender, EventArgs e) {
			if (!(txtCar.Text.Equals(""))) {
				try {
					if (txtCar.Text.Contains(" ")) {
						txtCar.Text = txtCar.Text.Replace(" ", "_");
					}
					String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
									"Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
					OleDbConnection objConn = openDBConnection();
					objConn.Open();

					OleDbCommand objCmd = new OleDbCommand("INSERT INTO Cars (Vehicle) VALUES (@value1);", objConn);
					objCmd.Parameters.AddWithValue("@value1", txtCar.Text);
					objCmd.ExecuteNonQuery();

					objCmd = new OleDbCommand("ALTER TABLE VehicleHours ADD " + txtCar.Text + " number;", objConn);
					//  objCmd.Parameters.AddWithValue("@value1", txtName.Text);
					objCmd.ExecuteNonQuery();

					objCmd = new OleDbCommand("UPDATE VehicleHours SET " + txtCar.Text + " =0;", objConn);
					objCmd.ExecuteNonQuery();
					objConn.Close();
					// populateVehicles();
					Response.Redirect("~/Admin");
					ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Vehicle successfully added');", true);
				}
				catch (Exception ex) {
					writeStackTrace("Add Vehicle", ex);
				}


			}
			else {
				ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Error: Line blank! Please fill in all fields!');", true);
			}

		}

		protected void Remove_Project_Clicked(object sender, EventArgs e) {
			if (!(txtPR.Text.Equals(""))) {


				try {
					if (txtPR.Text.Contains(" ")) {
						txtPR.Text = txtCar.Text.Replace(" ", "_");
					}
					String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
									"Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
					OleDbConnection objConn = openDBConnection();
					objConn.Open();

					OleDbCommand objCmd = new OleDbCommand("DELETE FROM Projects WHERE Project=@value1;", objConn);
					objCmd.Parameters.AddWithValue("@value1", txtPR.Text);
					objCmd.ExecuteNonQuery();

					objCmd = new OleDbCommand("ALTER TABLE ProjectHours DROP COLUMN " + txtPR.Text + ";", objConn);
					//  objCmd.Parameters.AddWithValue("@value1", txtName.Text);
					objCmd.ExecuteNonQuery();

					objConn.Close();
					// populateProjects();
					Response.Redirect("~/Admin");
					ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Project successfully removed');", true);
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
					if (txtCR.Text.Contains(" ")) {
						txtCR.Text = txtCar.Text.Replace(" ", "_");
					}
					String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
									"Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
					OleDbConnection objConn = openDBConnection();
					objConn.Open();

					OleDbCommand objCmd = new OleDbCommand("DELETE FROM Cars WHERE Vehicle=@value1;", objConn);
					objCmd.Parameters.AddWithValue("@value1", txtCR.Text);
					objCmd.ExecuteNonQuery();

					objCmd = new OleDbCommand("ALTER TABLE VehicleHours DROP COLUMN " + txtCR.Text + ";", objConn);
					//  objCmd.Parameters.AddWithValue("@value1", txtName.Text);
					objCmd.ExecuteNonQuery();

					objConn.Close();
					//populateVehicles();
					Response.Redirect("~/Admin");
					ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Vehicle successfully removed');", true);
				}
				catch (Exception ex) {
					writeStackTrace("Remove Vehicle", ex);
				}
			}
			else {
				ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Error: Line blank! Please fill in all fields!');", true);
			}
		}
		protected void Remove_User_Clicked(object sender, EventArgs e) {
			if (!(txtNR.Text.Equals(""))) {
				try {
					OleDbConnection objConn = openDBConnection();
					objConn.Open();

					OleDbCommand objCmd = new OleDbCommand("DELETE FROM Users WHERE Emp_Name=@value1;", objConn);
					objCmd.Parameters.AddWithValue("@value1", txtNR.Text);
					objCmd.ExecuteNonQuery();

					objCmd = new OleDbCommand("DELETE FROM ProjectHours WHERE Emp_Name=@value1;", objConn);
					objCmd.Parameters.AddWithValue("@value1", txtNR.Text);
					objCmd.ExecuteNonQuery();

					objCmd = new OleDbCommand("DELETE FROM VehicleHours WHERE Emp_Name=@value1;", objConn);
					objCmd.Parameters.AddWithValue("@value1", txtNR.Text);
					objCmd.ExecuteNonQuery();


					objConn.Close();
					//   populateUsers();
					Response.Redirect("~/Admin");
					ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('User successfully removed');", true);
				}
				catch (Exception ex) {
					writeStackTrace("Remove User", ex);
				}
			}
			else {
				ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Error: Line blank! Please fill in all fields!');", true);
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