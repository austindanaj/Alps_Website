using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Date = System.DateTime;
using System.Data.OleDb;
using System.Data;


namespace CTBTeam
{
    public partial class List : Page
    {
     

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty((string)Session["User"]))
                {
                    populateTable();
                }
            }
         



        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty((string)Session["User"]))
            {

                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                 "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCmd = new OleDbCommand("INSERT INTO PurchaseOrder " +
                                                            "(Item, Qty, Description, Price, Priority, Link, Emp_Name, Date_Added) " +
                                                            "VALUES(@value1, @value2, @value3, @value4, @value5, @value6, @value7, @value8)", objConn);
                objCmd.Parameters.AddWithValue("@value1", txtName.Text);
                objCmd.Parameters.AddWithValue("@value2", int.Parse(txtQuant.Text));
                objCmd.Parameters.AddWithValue("@value3", txtDesc.Text);
                objCmd.Parameters.AddWithValue("@value4", decimal.Parse(txtPrice.Text));
                objCmd.Parameters.AddWithValue("@value5", drpPrio.Text);
                objCmd.Parameters.AddWithValue("@value6", txtLink.Text);
                objCmd.Parameters.AddWithValue("@value7", (string)Session["User"]);
                objCmd.Parameters.AddWithValue("@value8", Date.Now.ToString());
                objCmd.ExecuteNonQuery();              
                objConn.Close();
                populateTable();
                reset();
            }
        }
        private void reset()
        {
            txtName.Text = "";
            txtQuant.Text = "";
            txtDesc.Text = "";
            txtPrice.Text = "";
            drpPrio.Text = "";
            txtLink.Text = "";
        }
       
        public void populateTable()
        {
            String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                 "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
            
            OleDbConnection objConn = new OleDbConnection(connectionString);
            objConn.Open();
            OleDbCommand objCmdSelect = new OleDbCommand("SELECT Item, Qty, Description, Price, Priority, Link, Emp_Name, Date_Added FROM PurchaseOrder", objConn);
            OleDbDataAdapter objAdapter = new OleDbDataAdapter();
            objAdapter.SelectCommand = objCmdSelect;
            DataSet objDataSet = new DataSet();
            objAdapter.Fill(objDataSet);        
            grdList.DataSource = objDataSet.Tables[0].DefaultView;
            grdList.DataBind();
            objConn.Close();
        }
    }
}