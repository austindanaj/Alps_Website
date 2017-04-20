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
using System.Drawing;
using Microsoft.Office.Interop.Excel;

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
            }

        }
        protected void populateDataPerson()
        {
            drpPerson.Items.Clear();
            drpPerson.Items.Add(new ListItem("--Select A Person--"));
            try
            {
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                         "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";

                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT DISTINCT Emp_Name FROM Users ORDER BY Emp_Name", objConn);
                OleDbDataReader reader = objCmdSelect.ExecuteReader();
                while (reader.Read())
                {
                    drpPerson.Items.Add(new ListItem(reader.GetString(0))); ;
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
        protected void populateDataPhones()
        {
            drpOs.Items.Clear();
            drpOs.Items.Add(new ListItem("--Select An OS--"));
            drpCheckIn.Items.Clear();
            drpCheckIn.Items.Add(new ListItem("--Select An OS--"));
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
                drpPhone.Items.Add(new ListItem("--Select A Phone--"));
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


            try
            {
                drpCheckIn.Items.Clear();
                drpCheckIn.Items.Add(new ListItem("--Select A Phone--"));

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
        /*
         * Connection to vehicles in accdb
         * Populates Vehicle drop down list
         * 
         * */

          
        public void popV()
        {

            try
            {
                Vehicle.Items.Clear();
                Vehicle.Items.Add(new ListItem("--Select A Vehicle--"));

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
        protected void onSelectPhone(object sender, EventArgs e)
        {
            populateDataPerson();
        }
        protected void onSelectPerson(object sender, EventArgs e)
        {
            popV();
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
                temp += "Calibration,\n";
            }
            if (cbl.Items[6].Selected)
            {
                temp += "Other";
            }

            if (drpPerson.Text == "--Select A Person--")
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("alert('");
                sb.Append("Please Select A Name");
                sb.Append("');");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString(), true);
            }
            else if (Vehicle.Text == "--Select A Vehicle--")

            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("alert('");
                sb.Append("Please Select A Vehicle");
                sb.Append("');");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString(), true);

            }
            /* Commands to update the PhoneCheckout accdb tab
             * Populates gridview 
             */
            else
            {
                try
                {
                    String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                         "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";

                    OleDbConnection objConn = new OleDbConnection(connectionString);
                    objConn.Open();
                    OleDbCommand objCmdSelect = new OleDbCommand("UPDATE PhoneCheckout SET Person=@name, Car=@car, Available=@bool, Purpose=@temp WHERE Model=@model", objConn);
                    objCmdSelect.Parameters.AddWithValue("@name", drpPerson.Text);
                    objCmdSelect.Parameters.AddWithValue("@car", Vehicle.Text);
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

                //Populates gridview 
                populateTable();


                try
                {
                    drpPhone.Items.Clear();
                    drpCheckIn.Items.Add(new ListItem("--Select A Phone--"));
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
                    drpCheckIn.Items.Add(new ListItem("--Select A Phone--"));

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

      
        public void clickCheckin(object sender, EventArgs e)
        {
            String temp = "";


            try
            {
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                     "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";

                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("UPDATE PhoneCheckout SET Person=@name, Car=@car, Available=@bool, Purpose=@temp WHERE Model=@model", objConn);
                objCmdSelect.Parameters.AddWithValue("@name", temp);
                objCmdSelect.Parameters.AddWithValue("@car", temp);
                objCmdSelect.Parameters.AddWithValue("@bool", true);
                objCmdSelect.Parameters.AddWithValue("@temp", temp);
                objCmdSelect.Parameters.AddWithValue("@model", drpCheckIn.SelectedItem.Text);
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
            
            try
            {
                drpPhone.Items.Clear();
                drpCheckIn.Items.Add(new ListItem("--Select A Phone--"));

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
                drpCheckIn.Items.Add(new ListItem("--Select A Phone--"));

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


        public void ExcelExport()
        {
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            //Sets column width to 20 units
            excel.Columns.ColumnWidth = 20;

            // Tries this method
            try
            {

                worksheet = workbook.ActiveSheet;

                string d = DateTime.Today.ToString("M-d-y");



                worksheet.Name = ("Report On " + d);

                int cellRowIndex = 2;
                int cellColumnIndex = 1;

                //gets the header first 
                for (int i = 1; i < gvTable.Columns.Count + 1; i++)
                {
                    worksheet.Cells[1, i] = gvTable.Columns[i - 1].HeaderText;
                }

                //Loops through each row and read value from each column. 
                for (int i = 0; i < gvTable.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < gvTable.Columns.Count; j++)
                    {

                        worksheet.Cells[cellRowIndex, cellColumnIndex] = gvTable.Rows[i].Cells[j].ToString();


                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }
                /*
                //Getting the location and file name of the excel to save from user. 
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveDialog.FilterIndex = 2;

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Export Successful");
                }
                */
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
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
            }
            
        }

    }
}










