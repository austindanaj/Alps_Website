using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Date = System.DateTime;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CTBTeam
{
    public partial class PhoneCheckOut : Page
    {
        TextBox[] textBoxes;
        DropDownList dlist = new DropDownList();
        String[] list = new String[3];
       // DropDownList drpPhone = new DropDownList();

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {

                populateDataPhones();

            }

        }

        protected void populateDataPhones()
        {
            drpOs.Items.Clear();
            drpOs.Items.Add(new ListItem("--Choose An Option--"));
            try
            {
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                         "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";

                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT DISTINCT os FROM PhoneCheckout", objConn);
                OleDbDataReader reader = objCmdSelect.ExecuteReader();
                while (reader.Read())
                {
                    drpOs.Items.Add(new ListItem(reader.GetString(0))); ;
                }

                objConn.Close();
            }
            catch (Exception e)
            {
                if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    file.WriteLine(Date.Today.ToString() + "--Populate Phones--" + e.ToString());
                    file.Close();
                }
            }
        }


        public void pop2()
        {
            //String str = dlist.SelectedItem.Text;
            try
            {
                drpPhone.Items.Clear();
             
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                         "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();

                OleDbCommand objCmdSelect = new OleDbCommand("SELECT Model FROM PhoneCheckout WHERE OS=@str ORDER BY Model;", objConn);
                objCmdSelect.Parameters.AddWithValue("@str", drpOs.SelectedItem.Text);
                
                OleDbDataReader reader = objCmdSelect.ExecuteReader();
                while (reader.Read())
                {
                    drpPhone.Items.Add(new ListItem(reader.GetString(0))); ;
                }

                objConn.Close();
            }
            catch (Exception e)
            {
                if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    file.WriteLine(Date.Today.ToString() + "--Populate Phones--" + e.ToString());
                    file.Close();
                }
            }

        }

        protected void onSelec(object sender, EventArgs e)
        {
          
                pop2();
            
        }

        public void popList()
        {

           

        }

        public void populateTable()
        {
            try
            {
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                     "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";

                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT Model, Status, Car, Person FROM PhoneCheckout ORDER BY Model", objConn);
                OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                objAdapter.SelectCommand = objCmdSelect;
                DataSet objDataSet = new DataSet();
                objAdapter.Fill(objDataSet);
                //grdPhone.DataSource = objDataSet.Tables[0].DefaultView;
                //grdPhone.DataBind();
                objConn.Close();         
            }
            catch (Exception ex)
            {
                if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    file.WriteLine(Date.Today.ToString() + "--Populate List--" + ex.ToString());
                    file.Close();
                }
            }
        }


    }
}

       
        

      





