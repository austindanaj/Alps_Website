using System;
using System.Web.UI.WebControls;

using Date = System.DateTime;
using System.Data.SqlClient;
using System.Data;

namespace CTBTeam {
	public partial class PhoneCheckOut : SuperPage {
		SqlConnection objConn;

		protected void Page_Load(object sender, EventArgs e) {
            /*
           if (Session["Alna_num"] == null) {
               redirectSafely("~/Login");
               return;
           }
           */

            objConn = openDBConnection();

			if (!IsPostBack) {
				populateDropdowns(); // Populates table with Phone Data on start up
				populateTable(); // Populates table with other information 
			}
		}

		private void populateDropdowns() {
			objConn.Open();
			SqlDataReader reader = getReader("select Name from Phones where Active=1 and ID not in (select Phone_ID from PhoneCheckout) order by Name", null, objConn);
			if (reader == null) return;

			while (reader.Read())
				ddlPhones.Items.Add(reader.GetString(0));

			reader.Close();

			reader = getReader("select Name from Vehicles where Active=1 order by Name", null, objConn);
			if (reader == null) return;
			while (reader.Read())
				ddlVehicles.Items.Add(reader.GetString(0));
			reader.Close();

			Date d = Date.Today;
			for (int i = 0; i < 30; i++)
				ddlEnd.Items.Add(d.AddDays(i).ToShortDateString());

			reader = getReader("select ID from PhoneCheckout", null, objConn);
			if (reader == null) return;
			while (reader.Read())
				ddlCheckIn.Items.Add(""+reader.GetInt32(0));
			reader.Close();

			objConn.Close();
		}

		protected void checkIn(object sender, EventArgs e) {
			if(!int.TryParse(ddlCheckIn.SelectedValue, out int id)) {
				throwJSAlert("Valid ID not selected");
				return;
			}
			objConn.Open();
			executeVoidSQLQuery("update PhoneCheckout set Active=0 where ID=@value1", id, objConn);
			objConn.Close();
			redirectSafely("~/PhoneCheckOut");
		}

		protected void insert(object sender, EventArgs e) {
			if (!Date.TryParse(ddlEnd.SelectedValue, out Date selection)) {
				throwJSAlert("Date selection invalid, try again");
				return;
			}

			string selectedReasonsForCheckingOut = "";
			if (string.IsNullOrEmpty(chkPurpose.SelectedValue))
				selectedReasonsForCheckingOut = "Other";
			else
				foreach (ListItem item in chkPurpose.Items)
					if (item.Selected) selectedReasonsForCheckingOut += item.Value + " ";

			objConn.Open();
			object[] o = { ddlPhones.SelectedValue, ddlVehicles.SelectedValue, Session["Alna_num"], selection, selectedReasonsForCheckingOut };
			executeVoidSQLQuery("insert into PhoneCheckout (Phone_ID, Vehicle_ID, Alna_num, PhoneCheckout.[End], Purpose) values" +
								"((select ID from Phones where Name=@value1), (select ID from Vehicles where Name=@value2), @value3, @value4, @value5);", o, objConn);
			objConn.Close();
			redirectSafely("~/PhoneCheckOut");
		}

		public void populateTable() {
			objConn.Open();
			SqlDataAdapter objAdapter = new SqlDataAdapter();
			objAdapter.SelectCommand = new SqlCommand("select PhoneCheckout.ID, Phones.Name as Phone, Vehicles.Name as Vehicle, Employees.Name, PhoneCheckout.Start, PhoneCheckout.[End], PhoneCheckout.Purpose from PhoneCheckout," +
													 "Vehicles, Phones, Employees where PhoneCheckout.Phone_ID = Phones.ID and PhoneCheckout.Vehicle_ID = Vehicles.ID and " +
													 "Employees.Alna_num = PhoneCheckout.Alna_num and PhoneCheckout.Active = 1; ", objConn);
			DataSet objDataSet = new DataSet();
			objAdapter.Fill(objDataSet);
			gvTable.DataSource = objDataSet.Tables[0].DefaultView;


			gvTable.DataBind();
			objConn.Close();
		}


		public void clickCheckout(object sender, EventArgs e) {

		}
	}
}