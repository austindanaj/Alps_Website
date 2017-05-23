using System;
using System.Web.UI.WebControls;

using Date = System.DateTime;
using System.Data.SqlClient;
using System.Data;

namespace CTBTeam {
	public partial class PhoneCheckOut : SuperPage {

		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				populateDataPhones(); // Populates table with Phone Data on start up
				populateTable(); // Populates table with other information 
				Period(30); // Populates date of which phones will be checked out (30 days from current day)
			}
		}

		protected void populateDataPerson() {
			//Method for the intial population of person 

			drpPerson.Items.Clear(); // Clears drop down list
			drpPerson.Items.Add(new ListItem("--Select A Person--")); //Initializes the drpPerson drop down list

			// gets information form database
			try {
				SqlConnection objConn = openDBConnection();
				objConn.Open();
				SqlCommand objCmdSelect = new SqlCommand("SELECT DISTINCT Emp_Name FROM Users ORDER BY Emp_Name", objConn);
				SqlDataReader reader = objCmdSelect.ExecuteReader();
				while (reader.Read()) {
					drpPerson.Items.Add(new ListItem(reader.GetString(0))); ;  //adds items to the drop down list
				}

				objConn.Close();
			}
			catch (Exception e) {
				writeStackTrace("Populate Phones", e);
			}
		}

		protected void populateDataPhones() {
			//Method for the Intial population of phones

			drpOs.Items.Clear(); // Clears drop down list 
			drpOs.Items.Add(new ListItem("--Select An OS--")); // Preset
			drpCheckIn.Items.Clear();  // Clears drop down list 
			drpCheckIn.Items.Add(new ListItem("--Select An OS--"));// Preset

			// gets information form database
			try {
				SqlConnection objConn = openDBConnection();
				objConn.Open();
				SqlCommand objCmdSelect = new SqlCommand("SELECT DISTINCT os FROM PhoneCheckout", objConn);
				SqlDataReader reader = objCmdSelect.ExecuteReader();
				while (reader.Read()) {
					drpOs.Items.Add(new ListItem(reader.GetString(0))); ;
				}

				objConn.Close();
			}
			catch (Exception e) {
				writeStackTrace("Populate Phones", e);
			}
		}


		public void pop2() {

			// Gets all phones that are checked in 
			// Populates phones after Updating the database

			try {
				drpPhone.Items.Clear();
				drpPhone.Items.Add(new ListItem("--Select A Phone--"));


				SqlConnection objConn = openDBConnection();
				objConn.Open();

				SqlCommand objCmdSelect = new SqlCommand("SELECT Model FROM PhoneCheckout WHERE OS=@str AND Available=@bool ORDER BY Model;", objConn);
				objCmdSelect.Parameters.AddWithValue("@str", drpOs.SelectedItem.Text);
				objCmdSelect.Parameters.AddWithValue("@bool", true);

				SqlDataReader reader = objCmdSelect.ExecuteReader();
				while (reader.Read()) {
					drpPhone.Items.Add(new ListItem(reader.GetString(0))); ;
				}

				objConn.Close();
			}
			catch (Exception e) {
				writeStackTrace("Populate Phones", e);
			}


			try {
				drpCheckIn.Items.Clear();
				drpCheckIn.Items.Add(new ListItem("--Select A Phone--"));



				SqlConnection objConn = openDBConnection();
				objConn.Open();

				SqlCommand objCmdSelect = new SqlCommand("SELECT Model FROM PhoneCheckout WHERE OS=@str AND Available=@bool ORDER BY Model;", objConn);
				objCmdSelect.Parameters.AddWithValue("@str", drpOs.SelectedItem.Text);
				objCmdSelect.Parameters.AddWithValue("@bool", false);

				SqlDataReader reader = objCmdSelect.ExecuteReader();
				while (reader.Read()) {
					drpCheckIn.Items.Add(new ListItem(reader.GetString(0))); ;
				}

				objConn.Close();
			}
			catch (Exception ef) {
				writeStackTrace("Populate Phones", ef);
			}

		}



		public void popV() {
			try {
				Vehicle.Items.Clear();
				Vehicle.Items.Add(new ListItem("--Select A Vehicle--"));



				SqlConnection objConn = openDBConnection();
				objConn.Open();

				SqlCommand objCmdSelect = new SqlCommand("SELECT Vehicle FROM Cars ORDER BY Vehicle;", objConn);


				SqlDataReader reader = objCmdSelect.ExecuteReader();
				while (reader.Read()) {
					Vehicle.Items.Add(new ListItem(reader.GetString(0))); ;
				}

				objConn.Close();
			}
			catch (Exception e) {
				writeStackTrace("Populate Phones", e);
			}

		}

		protected void onSelec(object sender, EventArgs e) {
			/*
             * Button Listener to populate phone drop down list
             * 
             */

			pop2();

		}


		protected void onSelectPhone(object sender, EventArgs e) {
			/*
            * Button Listener to populate person's drop down list upon selecting a phone
            * 
            */
			populateDataPerson();
		}

		protected void onSelectPerson(object sender, EventArgs e) {
			/*
       * Button Listener to populate vehicle's drop down list upon selecting a person
       * 
       */
			popV();
		}



		public void populateTable() {
			/*
             * Initialzes the grid view table(gvTable) with all the current information in the database
             * 
             * */
			try {
				SqlConnection objConn = openDBConnection();
				objConn.Open();
				SqlCommand objCmdSelect = new SqlCommand("SELECT Model, Available, Car, Person, Purpose, Period FROM PhoneCheckout ORDER BY Model", objConn);
				SqlDataAdapter objAdapter = new SqlDataAdapter();
				objAdapter.SelectCommand = objCmdSelect;
				DataSet objDataSet = new DataSet();
				objAdapter.Fill(objDataSet);
				gvTable.DataSource = objDataSet.Tables[0].DefaultView;


				gvTable.DataBind();
				objConn.Close();
			}
			catch (Exception ex) {
				writeStackTrace("Populate Phones", ex);
			}
		}


		public void clickCheckout(object sender, EventArgs e) {
			/*
             * Button listener for cheking out a phone
             * temp is used to get the selected value of the check list boxes
             * 
             * */

			String temp = "";

			if (cbl.Items[0].Selected) {
				temp += "Leakage,\n";
			}
			if (cbl.Items[1].Selected) {
				temp += "Range,\n";
			}
			if (cbl.Items[2].Selected) {
				temp += "Passive,\n";

			}
			if (cbl.Items[3].Selected) {
				temp += "Coverage,\n";
			}
			if (cbl.Items[4].Selected) {
				temp += "8-Blocks,\n";
			}
			if (cbl.Items[5].Selected) {
				temp += "Calibration,\n";
			}
			if (cbl.Items[6].Selected) {
				temp += "Other";
			}

			if (drpPerson.Text == "--Select A Person--") {
				throwJSAlert("Please Select A Name");
			}
			else if (Vehicle.Text == "--Select A Vehicle--") {
				throwJSAlert("Please Select A Vehicle");
			}
			/* Commands to update the PhoneCheckout accdb tab
             * Populates gridview 
             */
			else {
				try {
					SqlConnection objConn = openDBConnection();
					objConn.Open();
					object[] o = { drpPerson.Text, Vehicle.Text, false, temp, drpFrom.Text + " - " + drpTo.Text, drpPhone.SelectedItem.Text };

					executeVoidSQLQuery("UPDATE PhoneCheckout SET Person=@value1, Car=@value2, Available=@value3, Purpose= @value4, Period = @value5  WHERE Model=@value6", o, objConn);
					objConn.Close();

					saveOut();
				}
				catch (Exception ex) {
					Response.Write(ex.ToString());
				}

				//Populates gridview 
				populateTable();

				// Updates the drop down phone list 
				try {
					drpPhone.Items.Clear();
					drpCheckIn.Items.Add(new ListItem("--Select A Phone--"));


					SqlConnection objConn = openDBConnection();
					objConn.Open();

					SqlCommand objCmdSelect = new SqlCommand("SELECT Model FROM PhoneCheckout WHERE OS=@str AND Available=@bool ORDER BY Model;", objConn);
					objCmdSelect.Parameters.AddWithValue("@str", drpOs.SelectedItem.Text);
					objCmdSelect.Parameters.AddWithValue("@bool", true);

					SqlDataReader reader = objCmdSelect.ExecuteReader();
					while (reader.Read()) {
						drpPhone.Items.Add(new ListItem(reader.GetString(0))); ;
					}

					objConn.Close();
				}
				catch (Exception eg) {
					if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt"))) {

					}
					using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt"))) {
						file.WriteLine(Date.Today.ToString() + "--Populate Phones--" + eg.ToString());
						file.Close();
					}
				}


				//Updates the drop down list for the phone that can be checked out
				try {
					drpCheckIn.Items.Clear();
					drpCheckIn.Items.Add(new ListItem("--Select A Phone--"));



					SqlConnection objConn = openDBConnection();
					objConn.Open();

					SqlCommand objCmdSelect = new SqlCommand("SELECT Model FROM PhoneCheckout WHERE OS=@str AND Available=@bool ORDER BY Model;", objConn);
					objCmdSelect.Parameters.AddWithValue("@str", drpOs.SelectedItem.Text);
					objCmdSelect.Parameters.AddWithValue("@bool", false);

					SqlDataReader reader = objCmdSelect.ExecuteReader();
					while (reader.Read()) {
						drpCheckIn.Items.Add(new ListItem(reader.GetString(0))); ;
					}

					objConn.Close();
				}
				catch (Exception ef) {
					writeStackTrace("Populate Phones", ef);
				}
			}

		}


		public void clickCheckin(object sender, EventArgs e) {

			/* Button listener for the checked in 
             * 
             * 
             * 
             * */
			String temp = "";


			try {
				SqlConnection objConn = openDBConnection();
				objConn.Open();
				object[] o = { temp, temp, true, temp, temp, drpCheckIn.SelectedItem.Text };
				executeVoidSQLQuery("UPDATE PhoneCheckout SET Person=@value1, Car=@value2, Available=@value3, Purpose= @value4, Period = @value5 WHERE Model=@value6", o, objConn);
				objConn.Close();
				saveIn();
			}
			catch (Exception ex) {
				/*
                if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    file.WriteLine(Date.Today.ToString() + "--Populate List--" + ex.ToString());
                    file.Close();
                }
                */
				Response.Write(ex.ToString());
			}


			populateTable(); // Updates table

			// Updates phone and check in phone drop down list's 
			try {
				drpPhone.Items.Clear();
				drpCheckIn.Items.Add(new ListItem("--Select A Phone--"));

				SqlConnection objConn = openDBConnection();
				objConn.Open();

				SqlCommand objCmdSelect = new SqlCommand("SELECT Model FROM PhoneCheckout WHERE OS=@str AND Available=@bool ORDER BY Model;", objConn);
				objCmdSelect.Parameters.AddWithValue("@str", drpOs.SelectedItem.Text);
				objCmdSelect.Parameters.AddWithValue("@bool", true);

				SqlDataReader reader = objCmdSelect.ExecuteReader();
				while (reader.Read()) {
					drpPhone.Items.Add(new ListItem(reader.GetString(0))); ;
				}

				objConn.Close();
			}
			catch (Exception eg) {
				writeStackTrace("Populate Phones", eg);
			}


			// Updates phone and check in phone drop down list's 
			try {
				drpCheckIn.Items.Clear();
				drpCheckIn.Items.Add(new ListItem("--Select A Phone--"));



				SqlConnection objConn = openDBConnection();
				objConn.Open();

				SqlCommand objCmdSelect = new SqlCommand("SELECT Model FROM PhoneCheckout WHERE OS=@str AND Available=@bool ORDER BY Model;", objConn);
				objCmdSelect.Parameters.AddWithValue("@str", drpOs.SelectedItem.Text);
				objCmdSelect.Parameters.AddWithValue("@bool", false);

				SqlDataReader reader = objCmdSelect.ExecuteReader();
				while (reader.Read()) {
					drpCheckIn.Items.Add(new ListItem(reader.GetString(0))); ;
				}

				objConn.Close();
			}
			catch (Exception ef) {
				writeStackTrace("Populate phones", ef);
			}

		}

		public void saveOut() {
			/*
             * Saves information when CheckOut button is clicked
             * 
             * */

			try {
				/** Now append to file **/
				using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Logs/PhoneLog/Phone-report.txt"), true)) {

					file.WriteLine("Checked Out," + DateTime.Now.ToShortDateString() + " " + "," + "By: " + drpPerson.Text + "," + "Vehicle: " + Vehicle.Text + "," + drpPhone.Text);

					file.Close();
				}
			}
			catch (Exception ef) {
				writeStackTrace("Populate phones", ef);
			}

		}

		public void saveIn() {
			try {
				/** Now append to file **/
				using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Logs/PhoneLog/Phone-report.txt"), true)) {

					file.WriteLine("Checked In," + DateTime.Now.ToShortDateString() + " " + "," + drpCheckIn.Text);

					file.Close();
				}
			}
			catch (Exception ef) {
				writeStackTrace("Populate phones", ef);
			}


		}

		// adds 30 days in advance to the current day for selecting a check out date
		public void Period(int num) {
			drpFrom.Items.Clear();
			drpTo.Items.Clear();

			for (DateTime d = DateTime.Now; d <= DateTime.Now.AddDays(num); d = d.AddDays(1)) {
				drpFrom.Items.Add(new ListItem(d.ToShortDateString()));
				drpTo.Items.Add(new ListItem(d.ToShortDateString()));
			}
		}

		//once limit on table is met (30 List Items) it add a new page
		protected void OnPageIndexChanging2(object sender, GridViewPageEventArgs e) {
			populateTable();
			gvTable.PageIndex = e.NewPageIndex;
			gvTable.DataBind();
		}
	}
}