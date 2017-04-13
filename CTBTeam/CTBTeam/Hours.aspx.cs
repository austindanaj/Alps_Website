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
    public partial class Hours : Page
    {


        TextBox[] textBoxes;
        string userName;
        CheckBox[] checkBoxes;
        string date = "";     
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                getDate();

                populateDropDown();
                populateDataCars();
                populateDataProject();
               
                if (datechange())
                {
                    populateDataCars();
                    populateDataProject();

                    getDate();
                    
                }

                if (!string.IsNullOrEmpty((string)Session["User"]))
                {
                    userName = (string)Session["User"];
                    btnSubmit.Visible = true;
                   
                 
                   

                }                                   
                                                     
            }

        }
        public void getDate()
        {
            string[] arrLine = System.IO.File.ReadAllLines(@"" + Server.MapPath("~/Logs/TimeLog/Time-log.txt"));
            date = arrLine[arrLine.Length - 1];
            lblWeekOf.Text = "Week Of: " + date;
        }
        public bool datechange()
        {
            try
            {
                if (Date.Today.AddDays(-6) > Date.Parse(lblWeekOf.Text.Replace("Week Of: ", "")))
                {                    

                    String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                  "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                    OleDbConnection objConn = new OleDbConnection(connectionString);
                    objConn.Open();



                    OleDbCommand fieldProjectNames = new OleDbCommand("SELECT * FROM ProjectHours", objConn);
                    OleDbDataReader readerPNames = fieldProjectNames.ExecuteReader();
                    var table = readerPNames.GetSchemaTable();
                    var nameCol = table.Columns["ColumnName"];
                    string headerRow = "";
                    foreach (DataRow row in table.Rows)
                    {
                        if (!row[nameCol].Equals("Alna_Num"))
                        {
                            headerRow += row[nameCol] + ",";
                        }
                        
                    }
                    string[] arrLine = System.IO.File.ReadAllLines(@"" + Server.MapPath("~/Logs/TimeLog/Time-log.txt"));

                    arrLine[arrLine.Length - 1] = Date.Parse(date).ToShortDateString() + "," + headerRow;
                    System.IO.File.WriteAllLines(@"" + Server.MapPath("~/Logs/TimeLog/Time-log.txt"), arrLine);


                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Logs/TimeLog/Time-log.txt"), true))
                    {
                       
                        string text = "";

                        OleDbCommand objProject = new OleDbCommand("SELECT * FROM ProjectHours ORDER BY Alna_Num ;", objConn);

                        OleDbDataReader readerProject = objProject.ExecuteReader();
                        while (readerProject.Read())
                        {
                            text = "";
                            text += readerProject.GetString(1) + "," + readerProject.GetValue(2).ToString() + "," + readerProject.GetValue(3).ToString() + "," + readerProject.GetValue(4).ToString() + ","
                                  + readerProject.GetValue(5).ToString() + "," + readerProject.GetValue(6).ToString() + "," + readerProject.GetValue(7).ToString() + ",";
                            file.WriteLine(text);
                        }
                        file.WriteLine();
                        readerProject.Close();

                        OleDbCommand fieldCarNames = new OleDbCommand("SELECT * FROM VehicleHours ORDER BY Alna_Num", objConn);
                        OleDbDataReader readerCNames = fieldCarNames.ExecuteReader();
                        table = readerCNames.GetSchemaTable();
                        nameCol = table.Columns["ColumnName"];
                        headerRow = "";
                        foreach (DataRow row in table.Rows)
                        {
                            if (!row[nameCol].Equals("Alna_Num"))
                            {
                                headerRow += row[nameCol] + ",";
                            }
                        }
                        headerRow = Date.Parse(date).ToShortDateString() + "," + headerRow;
                        file.WriteLine(headerRow);
                        readerCNames.Close();

                        OleDbCommand objCar = new OleDbCommand("SELECT * FROM VehicleHours ORDER BY Alna_Num;", objConn);
                        text = "";
                        OleDbDataReader readerCar = objCar.ExecuteReader();
                        while (readerCar.Read())
                        {
                            text = "";
                            text += readerCar.GetString(1) + "," + readerCar.GetValue(2).ToString() + "," + readerCar.GetValue(3).ToString() + "," + readerCar.GetValue(4).ToString() + ","
                                  + readerCar.GetValue(5).ToString() + "," + readerCar.GetValue(6).ToString() + "," + readerCar.GetValue(7).ToString() + "," + readerCar.GetValue(8).ToString() + ","
                                  + readerCar.GetValue(9).ToString() + ",";
                            file.WriteLine(text);
                        }
                        file.WriteLine();
                        readerCar.Close();
                          
                        
                        if (Date.Today.AddDays(-6) > Date.Parse(date))
                        {
                            DateTime dt = DateTime.Now;
                            while (dt.DayOfWeek != DayOfWeek.Monday) dt = dt.AddDays(-1);
                            file.Write(dt.ToShortDateString());
                         

                        }

                        OleDbCommand objResetPH = new OleDbCommand("UPDATE ProjectHours " +
                                          "SET Project_B=@value1, Thermostat=@value2, Global_A=@value3, Radar=@value4, IR_Sensor=@value5, Other=@value6 ", objConn);

                        objResetPH.Parameters.AddWithValue("@value1", 0);
                        objResetPH.Parameters.AddWithValue("@value2", 0);
                        objResetPH.Parameters.AddWithValue("@value3", 0);
                        objResetPH.Parameters.AddWithValue("@value4", 0);
                        objResetPH.Parameters.AddWithValue("@value5", 0);
                        objResetPH.Parameters.AddWithValue("@value6", 0);


                        objResetPH.ExecuteNonQuery();


                        OleDbCommand objResetVH = new OleDbCommand("UPDATE VehicleHours " +
                                                               "SET Cruze=@value1, Trax=@value2, Tahoe=@value3, EV_Spark=@value4, Bolt=@value5, Volt=@value6, Spark=@value7, Equinox=@value8 ", objConn);

                        objResetVH.Parameters.AddWithValue("@value1", 0);
                        objResetVH.Parameters.AddWithValue("@value2", 0);
                        objResetVH.Parameters.AddWithValue("@value3", 0);
                        objResetVH.Parameters.AddWithValue("@value4", 0);
                        objResetVH.Parameters.AddWithValue("@value5", 0);
                        objResetVH.Parameters.AddWithValue("@value6", 0);
                        objResetVH.Parameters.AddWithValue("@value7", 0);
                        objResetVH.Parameters.AddWithValue("@value8", 0);


                        objResetVH.ExecuteNonQuery();

                        objConn.Close();
                        file.Close();



                        return true;

                    }
                  

                }
                return false;
            }
            catch (Exception ex)
            {
                if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    file.WriteLine(Date.Today.ToString() + "--Date Change--" + ex.ToString());
                    file.Close();
                }
                return false;
            }         
            
        }   
        
        //=======================================================
     
        //=======================================================
        protected void On_Click_Submit(object sender, EventArgs e)
        {
            userName = (string)Session["User"];
            if (userName == null)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "myalert", "alert('Error: Please Log in first before submitting!');", true);
                return;
            }
            try
            {
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                    "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";

                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();




                OleDbCommand objCmdProject = new OleDbCommand("UPDATE ProjectHours " +
                                                        "SET Project_B=@value1, Thermostat=@value2, Global_A=@value3, Radar=@value4, IR_Sensor=@value5, Other=@value6 " +
                                                        "WHERE Alna_Num=@value7", objConn);

               

                objCmdProject.ExecuteNonQuery();


                OleDbCommand objCmdCars = new OleDbCommand("UPDATE VehicleHours " +
                                                       "SET Cruze=@value1, Trax=@value2, Tahoe=@value3, EV_Spark=@value4, Bolt=@value5, Volt=@value6, Spark=@value7, Equinox=@value8 " +
                                                       "WHERE Alna_Num=@value9", objConn);

             

                objCmdCars.ExecuteNonQuery();

                objConn.Close();
                
                reset();
            }
            catch (Exception ex)
            {
                if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    file.WriteLine(Date.Today.ToString() + "--Hours Submit--" + ex.ToString());
                    file.Close();
                }
            }    

            populateDataCars();
            populateDataProject();

        }
        /**
         * Resets the text boxes and checkboxes
         */
         public void populateDropDown()
        {
            try
            {
                ddlNames.Items.Clear();
                ddlProjects.Items.Clear();
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                     "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT Emp_Name FROM Users ORDER BY Alna_Num", objConn);
                OleDbDataReader reader = objCmdSelect.ExecuteReader();
                while (reader.Read())
                {
                    ddlNames.Items.Add(new ListItem(reader.GetString(0)));
                }
                objCmdSelect = new OleDbCommand("SELECT PROJ_NAME FROM Projects ORDER BY ID", objConn);
                reader = objCmdSelect.ExecuteReader();
                while (reader.Read())
                {
                    ddlProjects.Items.Add(new ListItem(reader.GetString(0)));
                }
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
                    file.WriteLine(Date.Today.ToString() + "--Populate Vehicles--" + ex.ToString());
                    file.Close();
                }

            }
        }
         public void reset()
        {
            for(int i = 0; i < textBoxes.Length; i++)
            {
                checkBoxes[i].Checked = false;
                textBoxes[i].Enabled = false;
            }
        }
               //==================

        public void populateDataCars()
        {
            try
            {
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                     "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT Emp_Name, Cruze, Trax, Tahoe, EV_Spark, Bolt, Volt, Spark, Equinox FROM VehicleHours ORDER BY Alna_Num", objConn);
                OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                objAdapter.SelectCommand = objCmdSelect;
                DataSet objDataSet = new DataSet();
                objAdapter.Fill(objDataSet);
                dgvCars.DataSource = objDataSet.Tables[0].DefaultView;
                dgvCars.DataBind();
                objConn.Close();
            }
            catch(Exception ex)
            {
                if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    file.WriteLine(Date.Today.ToString() + "--Populate Vehicles--" + ex.ToString());
                    file.Close();
                }
                
            }
        }
    
        public void populateDataProject()
        {
            try
            {
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                   "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT Emp_Name, Project_B, Thermostat, Global_A, Radar, IR_Sensor, Other FROM ProjectHours ORDER BY Alna_Num", objConn);
                OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                objAdapter.SelectCommand = objCmdSelect;
                DataSet objDataSet = new DataSet();
                objAdapter.Fill(objDataSet);
                dgvProject.DataSource = objDataSet.Tables[0].DefaultView;
                dgvProject.DataBind();
                objConn.Close();
            }
            catch(Exception ex)
            {
                if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    file.WriteLine(Date.Today.ToString() + "--Populate Projects--" + ex.ToString());
                    file.Close();
                }
            }
          
        }






    }
}