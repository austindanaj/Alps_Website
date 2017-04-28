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

namespace CTBTeam
{
    public partial class PhoneCheckOut : Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {

                populateDataPhones(); // Populates table with Phone Data on start up
                populateTable(); // Populates table with other information 
                Period(30); // Populates date of which phones will be checked out (30 days from current day)
            }

        }



        protected void populateDataPerson()
        {
            //Method for the intial population of person 

            drpPerson.Items.Clear(); // Clears drop down list
            drpPerson.Items.Add(new ListItem("--Select A Person--")); //Initializes the drpPerson drop down list

            // gets information form database
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
                    drpPerson.Items.Add(new ListItem(reader.GetString(0))); ;  //adds items to the drop down list
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
            //Method for the Intial population of phones

            drpOs.Items.Clear(); // Clears drop down list 
            drpOs.Items.Add(new ListItem("--Select An OS--")); // Preset
            drpCheckIn.Items.Clear();  // Clears drop down list 
            drpCheckIn.Items.Add(new ListItem("--Select An OS--"));// Preset

            // gets information form database
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

            // Gets all phones that are checked in 
            // Populates phones after Updating the database

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



        public void popV()
        {
            /*
         * Connection to vehicles in accdb
         * Populates Vehicle drop down list
         * 
         * */

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
            /*
             * Button Listener to populate phone drop down list
             * 
             */

            pop2();

        }


        protected void onSelectPhone(object sender, EventArgs e)
        {
            /*
            * Button Listener to populate person's drop down list upon selecting a phone
            * 
            */
            populateDataPerson();
        }

        protected void onSelectPerson(object sender, EventArgs e)
        {
            /*
       * Button Listener to populate vehicle's drop down list upon selecting a person
       * 
       */
            popV();
        }



        public void populateTable()
        {
            /*
             * Initialzes the grid view table(gvTable) with all the current information in the database
             * 
             * */
            try
            {
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                     "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";

                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT Model, Available, Car, Person, Purpose, Period FROM PhoneCheckout ORDER BY Model", objConn);
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
            /*
             * Button listener for cheking out a phone
             * temp is used to get the selected value of the check list boxes
             * 
             * */

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
                    OleDbCommand objCmdSelect = new OleDbCommand("UPDATE PhoneCheckout SET Person=@name, Car=@car, Available=@bool, Purpose=@temp, Period=@period  WHERE Model=@model", objConn);
                    objCmdSelect.Parameters.AddWithValue("@name", drpPerson.Text);
                    objCmdSelect.Parameters.AddWithValue("@car", Vehicle.Text);
                    objCmdSelect.Parameters.AddWithValue("@bool", false);
                    objCmdSelect.Parameters.AddWithValue("@temp", temp);
                    objCmdSelect.Parameters.AddWithValue("@period", drpFrom.Text + " - " + drpTo.Text);
                    objCmdSelect.Parameters.AddWithValue("@model", drpPhone.SelectedItem.Text);
                    objCmdSelect.ExecuteNonQuery();
                    objConn.Close();

                    saveOut();

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

                // Updates the drop down phone list 
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


                //Updates the drop down list for the phone that can be checked out
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

            /* Button listener for the checked in 
             * 
             * 
             * 
             * */
            String temp = "";


            try
            {
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                     "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";

                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("UPDATE PhoneCheckout SET Person=@name, Car=@car, Available=@bool, Purpose=@temp, Period=@period WHERE Model=@model", objConn);
                objCmdSelect.Parameters.AddWithValue("@name", temp);
                objCmdSelect.Parameters.AddWithValue("@car", temp);
                objCmdSelect.Parameters.AddWithValue("@bool", true);
                objCmdSelect.Parameters.AddWithValue("@temp", temp);
                objCmdSelect.Parameters.AddWithValue("@period", temp);
                objCmdSelect.Parameters.AddWithValue("@model", drpCheckIn.SelectedItem.Text);
                objCmdSelect.ExecuteNonQuery();
                objConn.Close();
                saveIn();

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


            populateTable(); // Updates table

            // Updates phone and check in phone drop down list's 
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


            // Updates phone and check in phone drop down list's 
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

        public void saveOut()
        {
            /*
             * Saves information when CheckOut button is clicked
             * 
             * */

            try
            {
                /** Now append to file **/
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Logs/PhoneLog/Phone-report.txt"), true))
                {

                    file.WriteLine("Checked Out," + DateTime.Now.ToShortDateString() + " " + "," + "By: " + drpPerson.Text + "," + "Vehicle: " + Vehicle.Text + "," + drpPhone.Text);

                    file.Close();
                }

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

        public void saveIn()
        {

            /*
            * Saves information when Check In button is clicked
            * 
            * */

            try
            {
                /** Now append to file **/
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Logs/PhoneLog/Phone-report.txt"), true))
                {

                    file.WriteLine("Checked In," + DateTime.Now.ToShortDateString() + " " + "," + drpCheckIn.Text);

                    file.Close();
                }

                // OS.Text = txt;

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

        // adds 30 days in advance to the current day for selecting a check out date
        public void Period(int num)
        {
            drpFrom.Items.Clear();
            drpTo.Items.Clear();

             for(DateTime d = DateTime.Now; d <= DateTime.Now.AddDays(num); d = d.AddDays(1))
             {
                 drpFrom.Items.Add(new ListItem(d.ToShortDateString()));
                 drpTo.Items.Add(new ListItem(d.ToShortDateString()));
             }

          
           
        }

        //once limit on table is met (30 List Items) it add a new page
        protected void OnPageIndexChanging2(object sender, GridViewPageEventArgs e)
        {
            populateTable();
            gvTable.PageIndex = e.NewPageIndex;
            gvTable.DataBind();



        }


    }
}










