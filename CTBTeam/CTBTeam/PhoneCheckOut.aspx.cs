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
        DropDownList dlist = new DropDownList();
        String[] list = new String[3];
        // DropDownList drpPhone = new DropDownList();

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {

                populateDataPhones();
                populateTable();
                popV();
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
            
            try
            {
                drpPhone.Items.Clear();

                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                         "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();

                OleDbCommand objCmdSelect = new OleDbCommand("SELECT Model FROM PhoneCheckout WHERE OS=@str AND Available=@bool ORDER BY Model;", objConn);
                objCmdSelect.Parameters.AddWithValue("@str", drpOs.SelectedItem.Text);
                objCmdSelect.Parameters.AddWithValue("@bool", true);

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
        
        public void popV()
        {

            try
            {
                drpPhone.Items.Clear();

                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                         "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();

                OleDbCommand objCmdSelect = new OleDbCommand("SELECT Vehicle FROM Cars ORDER BY Vehicle;", objConn);
                

                OleDbDataReader reader = objCmdSelect.ExecuteReader();
                while (reader.Read())
                {
                    Vehicle.Items.Add(new ListItem(reader.GetString(0))); ;
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



        public void populateTable()
        {
            try
            {
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                     "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";

                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT Model, Available, Car, Person, Purpose FROM PhoneCheckout ORDER BY Model", objConn);
                OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                objAdapter.SelectCommand = objCmdSelect;
                DataSet objDataSet = new DataSet();
                objAdapter.Fill(objDataSet);
                gvTable.DataSource = objDataSet.Tables[0].DefaultView;
                gvTable.DataBind();
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
        public void clickCheckout(object sender, EventArgs e)
        {
            String temp = "";

                if (cbl.Items[0].Selected)
                {
                    temp += "Leakage,\n";
                   
                }
                if (cbl.Items[1].Selected)
                {
                    temp += "Range,\n";
                }
                if (cbl.Items[2].Selected)
                {
                    temp += "Passive,\n";

                }
                if (cbl.Items[3].Selected)
                {
                    temp += "Coverage,\n";
                }
                if (cbl.Items[4].Selected)
                {
                    temp += "8-Blocks,\n";
                }
                if (cbl.Items[5].Selected)
                {
                    temp += "Calibration\n";
                }
    
    
            try{
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                     "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";

                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("UPDATE PhoneCheckout SET Person=@name, Available=@bool, Purpose=@temp WHERE Model=@model", objConn);
                objCmdSelect.Parameters.AddWithValue("@name", getPerson.Text);
                 objCmdSelect.Parameters.AddWithValue("@bool", false);
                objCmdSelect.Parameters.AddWithValue("@temp", temp);
                objCmdSelect.Parameters.AddWithValue("@model", drpPhone.SelectedItem.Text);
                objCmdSelect.ExecuteNonQuery();
                objConn.Close();
               
            }
            catch (Exception ex)
            {
                /*
                if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    file.WriteLine(Date.Today.ToString() + "--Populate List--" + ex.ToString());
                    file.Close();
                }
                */
                Response.Write(ex.ToString());
            }
            populateTable();
            getPerson.Text = "";

            try
            {
                drpPhone.Items.Clear();

                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                         "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();

                OleDbCommand objCmdSelect = new OleDbCommand("SELECT Model FROM PhoneCheckout WHERE OS=@str AND Available=@bool ORDER BY Model;", objConn);
                objCmdSelect.Parameters.AddWithValue("@str", drpOs.SelectedItem.Text);
                objCmdSelect.Parameters.AddWithValue("@bool", true);

                OleDbDataReader reader = objCmdSelect.ExecuteReader();
                while (reader.Read())
                {
                    drpPhone.Items.Add(new ListItem(reader.GetString(0))); ;
                }

                objConn.Close();
            }
            catch (Exception eg)
            {
                if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    file.WriteLine(Date.Today.ToString() + "--Populate Phones--" + eg.ToString());
                    file.Close();
                }
            }

        }
        /*TODO:
        * */
        public void clickCheckin(object sender, EventArgs e)
        {
            String temp = "";


            try
            {
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                     "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";

                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("UPDATE PhoneCheckout SET Person=@name, Available=@bool, Purpose=@temp WHERE Model=@model", objConn);
                objCmdSelect.Parameters.AddWithValue("@name", getPerson.Text);
                objCmdSelect.Parameters.AddWithValue("@bool", true);
                objCmdSelect.Parameters.AddWithValue("@temp", temp);
                objCmdSelect.Parameters.AddWithValue("@model", drpPhone.SelectedItem.Text);
                objCmdSelect.ExecuteNonQuery();
                objConn.Close();

            }
            catch (Exception ex)
            {
                /*
                if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    file.WriteLine(Date.Today.ToString() + "--Populate List--" + ex.ToString());
                    file.Close();
                }
                */
                Response.Write(ex.ToString());
            }
            populateTable();
            getPerson.Text = "";
            try
            {
                drpPhone.Items.Clear();

                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                         "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();

                OleDbCommand objCmdSelect = new OleDbCommand("SELECT Model FROM PhoneCheckout WHERE OS=@str AND Available=@bool ORDER BY Model;", objConn);
                objCmdSelect.Parameters.AddWithValue("@str", drpOs.SelectedItem.Text);
                objCmdSelect.Parameters.AddWithValue("@bool", true);

                OleDbDataReader reader = objCmdSelect.ExecuteReader();
                while (reader.Read())
                {
                    drpPhone.Items.Add(new ListItem(reader.GetString(0))); ;
                }

                objConn.Close();
            }
            catch (Exception eg)
            {
                if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    file.WriteLine(Date.Today.ToString() + "--Populate Phones--" + eg.ToString());
                    file.Close();
                }
            }



            try
            {
                drpCheckIn.Items.Clear();
                drpCheckIn.Items.Add(new ListItem("--Choose Phone--"));

                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                         "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();

                OleDbCommand objCmdSelect = new OleDbCommand("SELECT Model FROM PhoneCheckout WHERE OS=@str AND Available=@bool ORDER BY Model;", objConn);
                objCmdSelect.Parameters.AddWithValue("@str", drpOs.SelectedItem.Text);
                objCmdSelect.Parameters.AddWithValue("@bool", false);

                OleDbDataReader reader = objCmdSelect.ExecuteReader();
                while (reader.Read())
                {
                    drpCheckIn.Items.Add(new ListItem(reader.GetString(0))); ;
                }

                objConn.Close();
            }
            catch (Exception ef)
            {
                if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    file.WriteLine(Date.Today.ToString() + "--Populate Phones--" + ef.ToString());
                    file.Close();
                }
            }

        }



    }
}

       
        

      





