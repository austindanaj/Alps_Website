using System;
using Date = System.DateTime;
using System.Data.SqlClient;
using System.Data;

namespace CTBTeam {
	public partial class List : SuperPage {
		SqlConnection objConn;

		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				populateTable();
				if (Session["Alna_num"] != null) {
					btnSubmit.Visible = true;
					successDialog(successOrFail);
				}
				else {
					btnSubmit.Visible = false;
				}
			}
		}

		protected void btnSubmit_Click(object sender, EventArgs e) {
			if (Session["Alna_num"] != null) {
				//Before even making a DB connection, check that parsed arguments are correct.
				if (!int.TryParse(txtQuant.Text, out int quantity)) {
					throwJSAlert("Quantity is not an integer value");
					return;
				}
				if (!decimal.TryParse(txtPrice.Text, out decimal price)) {
					throwJSAlert("Price is not a valid floating point number.");
					return;
				}

				try {
					objConn = openDBConnection();
					objConn.Open();
					object[] o = { txtName.Text, Math.Abs(quantity), txtDesc.Text, Math.Abs(price), drpPrio.Text, txtLink.Text, (string)Session["User"], Date.Now.ToString() };
					executeVoidSQLQuery("INSERT INTO PurchaseOrder (Item, Qty, Description, Price, Priority, Link, Emp_Name, Date_Added) " +
																"VALUES(@value1, @value2, @value3, @value4, @value5, @value6, @value7, @value8)", o, objConn);
					objConn.Close();
					Session["success?"] = true;
					redirectSafely("~/List");
				}
				catch (Exception ex) {
					writeStackTrace("Submit click", ex);
				}
			}
		}

		public void populateTable() {
			try {
				objConn = openDBConnection();
				objConn.Open();
				SqlCommand objCmdSelect = new SqlCommand("SELECT Item, Qty, Description, Price, Priority, Link, Emp_Name, Date_Added FROM PurchaseOrder", objConn);
				SqlDataAdapter objAdapter = new SqlDataAdapter();
				objAdapter.SelectCommand = objCmdSelect;
				DataSet objDataSet = new DataSet();
				objAdapter.Fill(objDataSet);
				grdList.DataSource = objDataSet.Tables[0].DefaultView;
				grdList.DataBind();
				objConn.Close();
			}
			catch (Exception ex) {
				writeStackTrace("Populate List", ex);
			}
		}
	}
}