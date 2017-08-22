using System;
using Date = System.DateTime;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace CTBTeam {
	public partial class List : SuperPage {
		SqlConnection objConn;
		LinkedList<Button> dynamicButtonList = new LinkedList<Button>();

		protected void Page_Load(object sender, EventArgs e) {
			if (Session["Alna_num"] == null) {
				redirectSafely("~/Login");
				return;
			}

			objConn = openDBConnection();
			if (!IsPostBack) {
				populateTable();
				successDialog(txtSuccessBox);
				ddlInit();
			}
		}

		private void ddlInit() {
			ddlPriority.Items.Add("Low - 1");
			ddlPriority.Items.Add("Medium - 2");
			ddlPriority.Items.Add("High - 3");
		}

		protected void purchase(object sender, EventArgs e) {
			if(!int.TryParse(txtPurchase.Text, out int id)) {
				throwJSAlert("Not an integer, can't ever be a primary key for ID");
				return;
			}
			objConn.Open();
			object[] o = { Session["Alna_num"], id };
			executeVoidSQLQuery("update PurchaseOrders set Purchaser=@value1 where ID=@value2;", o, objConn);
			objConn.Close();
			Session["success?"] = true;
			redirectSafely("~/List");
		}

		protected void btnSubmit_Click(object sender, EventArgs e) {
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
				objConn.Open();
				object[] o = { txtName.Text, Math.Abs(quantity), txtDesc.Text, Math.Abs(price), ddlPriority.SelectedIndex + 1, txtLink.Text, Session["Alna_num"], Date.Now.ToString() };
				executeVoidSQLQuery("INSERT INTO PurchaseOrders (Name, Qty, Description, Price, Priority, Link, Alna_num, Date_added) " +
												"VALUES(@value1, @value2, @value3, @value4, @value5, @value6, @value7, @value8)", o, objConn);
				objConn.Close();
				Session["success?"] = true;
				redirectSafely("~/List");
			}
			catch (Exception ex) {
				writeStackTrace("Submit click", ex);
			}
		}

		private void populateTable() {
			try {
				objConn.Open();
				SqlDataAdapter objAdapter = new SqlDataAdapter();
				objAdapter.SelectCommand = new SqlCommand("SELECT ID, PurchaseOrders.Name, Qty, [Employees].[Name] as Employee, Description, Price, Priority as 'Priority(1-10)', Link, Date_Added FROM PurchaseOrders inner join Employees on Employees.Alna_num=PurchaseOrders.Alna_num where Purchaser is null order by ID", objConn);
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