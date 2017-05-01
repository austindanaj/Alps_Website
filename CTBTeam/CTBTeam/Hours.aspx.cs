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
using System.Web.UI.DataVisualization.Charting;

namespace CTBTeam
{
    public partial class Hours : Page
    {


     
        string userName;
      //  int[][] fileLineNumber;
        string date = "";     
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                getDate();

                populateNames();
                populateDataCars();
                populateDataProject();
                populateDataPercentage();
               
                if (datechange())
                {
                    populateDataCars();
                    populateDataProject();

                    getDate();
                    
                }

                if (!string.IsNullOrEmpty((string)Session["User"]))
                {
                   
                    userName = (string)Session["User"];
                    btnSubmitCar.Visible = true;
                    btnSubmitProject.Visible = true;




                }                                   
                                                     
            }

        }
        /**
         * Get the Monday of the current week 
         **/
        public void getDate()
        {/*
            ddlChangeDate.Items.Clear();
            ddlChangeDate.Items.Add(new ListItem("--Select A Date--"));
            */

            /** Read contents of file into array **/
            string[] arrLine = System.IO.File.ReadAllLines(@"" + Server.MapPath("~/Logs/TimeLog/Time-log.txt"));
            date = arrLine[arrLine.Length - 1];
            /** Get the last line in file, (thats where the current monday of the week is saved) **/
            /*
          
            DateTime previous = DateTime.Now;
            ddlChangeDate.Items.Add(new ListItem(DateTime.Parse(arrLine[arrLine.Length - 1]).ToShortDateString()));
            DateTime time = DateTime.Now;
            int count = 0;
            for (int i = arrLine.Length - 2; i >= 0; i--)
            {
                if (!arrLine[i].Equals("")) { 
                    if (DateTime.TryParse(arrLine[i].Substring(0, arrLine[i].IndexOf(',')), out time))
                    {
                        if (!time.Equals(previous))
                        {
                            previous = time;
                            count++;
                            ddlChangeDate.Items.Add(new ListItem(time.ToShortDateString()));
                            if (count == 4)
                            {
                                break;
                            }
                        }
                        }
                    }
                }
            */
            /** Set label **/
            lblWeekOf.Text = "Week Of: " + date;
        }

        /**
        * Checks whether the data needs to be changed or not
        * 
        * Return: bool
        **/
        public bool datechange()
        {
            try
            {
                /** If the date from file, more than a week old **/
                if (Date.Today.AddDays(-6) > Date.Parse(lblWeekOf.Text.Replace("Week Of: ", "")))
                {                    
                   /**
                    * Set SQL connection
                    **/
                    String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                  "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                    OleDbConnection objConn = new OleDbConnection(connectionString);
                    objConn.Open();




                    string projectList = "";      /** List of projects **/
                    int projectCount = 0;         /** Count of projects **/

                    /** Command to get list of projects **/                 
                    OleDbCommand getProjectList = new OleDbCommand("SELECT PROJ_NAME From Projects ORDER BY ID", objConn);
                    OleDbDataReader readerProjectList = getProjectList.ExecuteReader();
                    while (readerProjectList.Read())
                    {
                        projectCount++;      /** Increment counter by 1 **/
                        projectList += readerProjectList.GetString(0) + ",";    /** Add project name to list **/
                    }


                    string carList = "";        /** List of projects **/
                    int carCount = 0;           /** Count of projects **/

                    /** Command to get list of cars **/
                    OleDbCommand getCarList = new OleDbCommand("SELECT Vehicle From Cars ORDER BY ID", objConn);
                    OleDbDataReader readerCarList = getCarList.ExecuteReader();
                    while (readerCarList.Read())
                    {
                        carCount++;      /** Increment counter by 1 **/
                        carList += readerCarList.GetString(0) + ",";    /** Add car name to list **/
                    }


                    /** Command to get project hours ( used to get header ) **/
                    OleDbCommand fieldProjectNames = new OleDbCommand("SELECT * FROM ProjectHours", objConn);
                    OleDbDataReader readerPNames = fieldProjectNames.ExecuteReader();

               
                    var table = readerPNames.GetSchemaTable();      /** Set the table of project hours to variable **/
                    var nameCol = table.Columns["ColumnName"];      /** Set the column name **/
                    string headerRow = "";                          /** Header Row for Project Hours Log file **/

                    /** 
                     * Loop through each row, if column is ID, dont add
                     * This loop populates the header row for project hours
                     **/
                    foreach (DataRow row in table.Rows)
                    {
                        if (!row[nameCol].Equals("ID"))
                        {
                            headerRow += row[nameCol] + ",";
                        }
                        
                    }
                    /** Get the contents of file **/
                    string[] arrLine = System.IO.File.ReadAllLines(@"" + Server.MapPath("~/Logs/TimeLog/Time-log.txt"));

                    /** Replace the last line (the date of the previous week with the header row ) also appending the previous week**/
                    arrLine[arrLine.Length - 1] = Date.Parse(date).ToShortDateString() + "," + headerRow;

                    /** Write array to file, replacing contents with it (basically appending, but need to replace all to replace the last line **/
                    System.IO.File.WriteAllLines(@"" + Server.MapPath("~/Logs/TimeLog/Time-log.txt"), arrLine);

                    /** Now append to file **/
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Logs/TimeLog/Time-log.txt"), true))
                    {

                        string text = "";      /** Initialize variable for storing each line **/

                        /** Command to get project hours  **/
                        OleDbCommand objProject = new OleDbCommand("SELECT * FROM ProjectHours ORDER BY Emp_Name ;", objConn);
                        OleDbDataReader readerProject = objProject.ExecuteReader();

                        /** Loop through each row **/
                        while (readerProject.Read())
                        {
                            text = "";     /** Reset the line to empty **/

                            /** Loop through each row, starting at emp_name to end **/
                            for (int i = 1; i <= projectCount; i++)
                            {
                                text += readerProject.GetValue(i).ToString() + ",";      /** Get value from database and append to line **/
                            }
                            file.WriteLine(text);     /** write line to file **/
                        }
                        file.WriteLine();      /** write blank line to file **/
                        readerProject.Close();

                        /** Command to get car hours ( used to get header ) **/
                        OleDbCommand fieldCarNames = new OleDbCommand("SELECT * FROM VehicleHours ORDER BY Emp_Name", objConn);
                        OleDbDataReader readerCNames = fieldCarNames.ExecuteReader();

                        table = readerPNames.GetSchemaTable();      /** Set the table of vehicle hours to variable **/
                        nameCol = table.Columns["ColumnName"];      /** Set the column name **/
                        headerRow = "";                             /** Header Row for Vehicle Hours Log file **/

                        /** 
                         * Loop through each row, if column is ID, dont add
                         * This loop populates the header row for vehicle hours
                         **/
                        foreach (DataRow row in table.Rows)
                        {
                            if (!row[nameCol].Equals("ID"))
                            {
                                headerRow += row[nameCol] + ",";
                            }
                        }

                        /** Set header row to previous week + created header row **/
                        headerRow = Date.Parse(date).ToShortDateString() + "," + headerRow;

                        /** Write header row to file **/
                        file.WriteLine(headerRow);
                        readerCNames.Close();

                        /** Command to get vehicle hours **/
                        OleDbCommand objCar = new OleDbCommand("SELECT * FROM VehicleHours ORDER BY Emp_Name;", objConn);                         
                        OleDbDataReader readerCar = objCar.ExecuteReader();

                        text = "";      /** Reset the line to empty **/

                        /** Loop through each row **/
                        while (readerCar.Read())
                        {
                            text = "";      /** Reset the line to empty **/

                            /** Loop through each row, starting at emp_name to end **/
                            for (int i = 1; i <= carCount; i++)
                            {
                                text += readerCar.GetValue(i).ToString() + ",";         /** Get value from database and append to line **/
                            }
                            file.WriteLine(text);       /** write line to file **/
                        }
                        file.WriteLine();       /** write blank line to file **/
                        readerCar.Close();

                        /** Second check if date needs to be changed, if yes, find the monday of the week, then set date to monday, write to line **/
                        if (Date.Today.AddDays(-6) > Date.Parse(date))
                        {
                            DateTime dt = DateTime.Now;
                          
                            file.Write(dt.ToShortDateString());


                        }

                        /** split list of projects into an array **/
                        string[] arrayProjectList = projectList.Split(',');
                        /** Start the query string **/
                        string queryProject = "UPDATE ProjectHours SET ";

                        /** Loop through the list of projects, and build the rest of the query, do use last array index, it is empty **/
                        for (int i = 0; i < arrayProjectList.Length - 1; i++)
                        {
                            queryProject += arrayProjectList[i] + "=@value" + (i + 1);
                            if (i != arrayProjectList.Length - 2)
                            {
                                /** If not last, then comma **/
                                queryProject += ", ";
                            } else
                            {
                                /** If last then semicolon to end query **/
                                queryProject += ";";
                            }
                        }
                        /** Command for query **/
                        OleDbCommand objResetPH = new OleDbCommand(queryProject, objConn);
                        for (int i = 0; i < arrayProjectList.Length - 1; i++)
                        {
                            objResetPH.Parameters.AddWithValue("@value" + (i + 1), 0);
                        }                   

                        objResetPH.ExecuteNonQuery();




                     
                        string[] arrayCarList = carList.Split(',');
                        string queryCar = "UPDATE VehicleHours SET ";
                        for (int i = 0; i < arrayCarList.Length - 1; i++)
                        {
                            queryCar += arrayCarList[i] + "=@value" + (i + 1);
                            if (i != arrayCarList.Length - 2)
                            {
                                queryCar += ", ";
                            }
                            else
                            {
                                queryCar += ";";
                            }
                        }

                        OleDbCommand objResetVH = new OleDbCommand(queryCar, objConn);
                        for (int i = 0; i < arrayCarList.Length - 1; i++)
                        {
                            objResetVH.Parameters.AddWithValue("@value" + (i + 1), 0);
                        }

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
        
        //Searches the database to match the pattern given by the
        //searchbox element in Hours.aspx.
        protected void On_Click_Search_DB(object sender, EventArgs e) {
            String s = searchbox.Text;
            
            if (s is null || s.Equals("")) {
                //Some sort of exception or do nothing condition to correct the user
            } else {
                /*
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                        "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                OleDbConnection objconn = new OleDbConnection(connectionString);
                OleDbCommand searchDbCommand = new OleDbCommand("Select * from ");
                */
            }
        }

        //=======================================================
     
        //=======================================================
        protected void On_Click_Submit_Projects(object sender, EventArgs e)
        {
            if (ddlNamesProject.Text == "--Select A Name--")
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("alert('");
                sb.Append("Please Select A Name");
                sb.Append("');");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString(), true);
            }
            else if (ddlProjects.Text == "--Select A Project--")
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("alert('");
                sb.Append("Please Select A Project");
                sb.Append("');");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString(), true);
            }
            else
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
                                                            "SET " + ddlProjects.Text + "=@value1 " +
                                                            "WHERE Emp_Name=@value2", objConn);
                    objCmdProject.Parameters.AddWithValue("@value1", int.Parse(txtHoursProjects.Text));
                    objCmdProject.Parameters.AddWithValue("@value2", ddlNamesProject.Text);


                    objCmdProject.ExecuteNonQuery();

                   
                    objConn.Close();

                   // reset();
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

               // populateDataCars();
                populateDataProject();
            }
        }
        protected void On_Click_Submit_Percent(object sender, EventArgs e)
        {
            if (ddlNamesCar.Text == "--Select A Name--")
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("alert('");
                sb.Append("Please Select A Name");
                sb.Append("');");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString(), true);
            }
            else if (ddlCars.Text == "--Select A Project--")
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("alert('");
                sb.Append("Please Select A Project");
                sb.Append("');");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString(), true);
            }
            else
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




                    OleDbCommand objCmdCars = new OleDbCommand("UPDATE VehicleHours " +
                                                           "SET " + ddlCars.Text + "=@value1 " +
                                                           "WHERE Emp_Name=@value2", objConn);
                    objCmdCars.Parameters.AddWithValue("@value1", int.Parse(txtHoursCars.Text));
                    objCmdCars.Parameters.AddWithValue("@value2", ddlNamesCar.Text);

                    objCmdCars.ExecuteNonQuery();

                    objConn.Close();

                    // reset();
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
                // populateDataProject();
            }
        }
        protected void On_Click_Submit_Cars(object sender, EventArgs e)
        {
            if (ddlNamesCar.Text == "--Select A Name--")
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("alert('");
                sb.Append("Please Select A Name");
                sb.Append("');");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString(), true);
            }
            else if (ddlCars.Text == "--Select A Car--")
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("alert('");
                sb.Append("Please Select A car");
                sb.Append("');");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString(), true);
            }
            else
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



                    
                    OleDbCommand objCmdCars = new OleDbCommand("UPDATE VehicleHours " +
                                                           "SET " + ddlCars.Text + "=@value1 " +
                                                           "WHERE Emp_Name=@value2", objConn);
                    objCmdCars.Parameters.AddWithValue("@value1", int.Parse(txtHoursCars.Text));
                    objCmdCars.Parameters.AddWithValue("@value2", ddlNamesCar.Text);
                          
                    objCmdCars.ExecuteNonQuery();
                    
                    objConn.Close();

                    // reset();
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
               // populateDataProject();
            }
        }
        protected void didSelectNamesPercent(object sender, EventArgs e)
        {
            populateListProjects();
        }
        protected void didSelectNameProject(object sender, EventArgs e)
        {
            populateListProjects();
        }
        protected void didSelectNameCar(object sender, EventArgs e)
        {
            populateListCars();
        }
        public void populateNames()
        {
            try
            {
                ddlNamesProject.Items.Clear();
                ddlNamesCar.Items.Clear();
                ddlAllNames.Items.Clear();
                ddlNamesProject.Items.Add("--Select A Name--");
                ddlNamesCar.Items.Add("--Select A Name--");
                ddlAllNames.Items.Add("--Select A Name--");
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                       "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT Emp_Name FROM Users WHERE Full_Time=@bool ORDER BY Emp_Name", objConn);
                objCmdSelect.Parameters.AddWithValue("@bool", false);
                OleDbDataReader reader = objCmdSelect.ExecuteReader();
                while (reader.Read())
                {
                    ddlNamesProject.Items.Add(new ListItem(reader.GetString(0)));
                    ddlNamesCar.Items.Add(new ListItem(reader.GetString(0)));
                }
                objCmdSelect = new OleDbCommand("SELECT Emp_Name FROM Users ORDER BY Emp_Name", objConn);        
                reader = objCmdSelect.ExecuteReader();
                while (reader.Read())
                {
                    ddlAllNames.Items.Add(new ListItem(reader.GetString(0)));
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
                    file.WriteLine(Date.Today.ToString() + "--Populate Names--" + ex.ToString());
                    file.Close();
                }

            }
        }
        public void populateListProjects()
        {
            try
            {
                ddlProjects.Items.Clear();
                ddlProjects.Items.Add("--Select A Project--");
               
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                       "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT PROJ_NAME FROM Projects ORDER BY PROJ_NAME", objConn);
                OleDbDataReader reader = objCmdSelect.ExecuteReader();
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
        public void populateListCars()
        {
            try
            {
                ddlCars.Items.Clear();
                ddlCars.Items.Add("--Select A Car--");

                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                       "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT Vehicle FROM Cars ORDER BY Vehicle", objConn);
                OleDbDataReader reader = objCmdSelect.ExecuteReader();
                while (reader.Read())
                {
                    ddlCars.Items.Add(new ListItem(reader.GetString(0)));

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

       public void populateDataPercentage()
        {
            try { 
            
                
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                     "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCount = new OleDbCommand("SELECT DISTINCT Emp_Name FROM PercentageLog WHERE Log_Date=@date ORDER BY Emp_Name", objConn);
                objCount.Parameters.AddWithValue("@date", DateTime.Parse(date));
                OleDbDataReader readerCount = objCount.ExecuteReader();
                int empCount = 0;
                while (readerCount.Read())
                {
                    empCount++;
                }
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM PercentageLog WHERE Log_Date=@date ORDER BY Emp_Name", objConn);
                objCmdSelect.Parameters.AddWithValue("@date", DateTime.Parse(date));
                OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                objAdapter.SelectCommand = objCmdSelect;
                DataSet objDataSet = new DataSet();
                objAdapter.Fill(objDataSet);
                objDataSet.Tables[0].Columns.RemoveAt(0);

                DataTable chartData = objDataSet.Tables[0];
                string[] XPointMember = { "A", "B", "C", "D" };
                double[] YPointMember = { 0, 0, 0, 0 };
                int[] numberPeople = { 0, 0, 0, 0 };
                double value = 0;
                double runningSum = 0;
                for(int i = 0; i < chartData.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(chartData.Rows[i]["Full_Time"])) 
                    {
                        value = (Convert.ToDouble(chartData.Rows[i]["Percentage"])) / 100;
                    }
                    else
                    {
                        value = (Convert.ToDouble(chartData.Rows[i]["Percentage"]) / 2) / 100;
                    }
                    value *= 40;
                    runningSum += value;
                    switch (chartData.Rows[i]["Category"].ToString())
                    {
                        
                        case "A":
                            YPointMember[0] += value; 
                            
                            break;
                        case "B":
                            YPointMember[1] += value;
                           
                            break;
                        case "C":
                            YPointMember[2] += value;
                          
                            break;
                        case "D":
                            YPointMember[3] += value; 
                           
                            break;
                    }
                   
                    
                }
                YPointMember[0] = (YPointMember[0] / runningSum);
                YPointMember[1] = (YPointMember[1] / runningSum);
                YPointMember[2] = (YPointMember[2] / runningSum);
                YPointMember[3] = (YPointMember[3] / runningSum);


                chartPercent.Series[0].Points.DataBindXY(XPointMember, YPointMember);
                chartPercent.Series[0].BorderWidth = 10;
                chartPercent.Series[0].ChartType = SeriesChartType.Pie;

                foreach (Series charts in chartPercent.Series)
                {
                    foreach(DataPoint point in charts.Points)
                    {
                        switch (point.AxisLabel)
                        {
                            case "A":
                                point.Color = System.Drawing.Color.Aqua;
                                break;
                            case "B":
                                point.Color = System.Drawing.Color.SpringGreen;
                                break;
                            case "C":
                                point.Color = System.Drawing.Color.Salmon;
                                break;
                            case "D":
                                point.Color = System.Drawing.Color.Violet;
                                break;
                        }
                        point.Label = string.Format("{0:P} - {1}", point.YValues[0], point.AxisLabel);
                   
                    }
                }

                objConn.Close();
                
                // dgvCars.HeaderRow.Cells[0].Visible = false;
            }
            catch (Exception ex)
            {
                if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    file.WriteLine(Date.Today.ToString() + "--Populate Percent--" + ex.ToString());
                    file.Close();
                }

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
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM VehicleHours ORDER BY Emp_Name", objConn);
                OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                objAdapter.SelectCommand = objCmdSelect;
                DataSet objDataSet = new DataSet();
                objAdapter.Fill(objDataSet);
                objDataSet.Tables[0].Columns.RemoveAt(0);
                dgvCars.DataSource = objDataSet.Tables[0].DefaultView;
                dgvCars.DataBind();
               
                objConn.Close();
               // dgvCars.HeaderRow.Cells[0].Visible = false;
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
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM ProjectHours ORDER BY Emp_Name", objConn);
                OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                objAdapter.SelectCommand = objCmdSelect;
                DataSet objDataSet = new DataSet();
                objAdapter.Fill(objDataSet);
                objDataSet.Tables[0].Columns.RemoveAt(0);
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


        protected void OnPageIndexChanging1(object sender, GridViewPageEventArgs e)
        {
            populateDataProject();
            dgvProject.PageIndex = e.NewPageIndex;
            dgvProject.DataBind();



        }

             protected void OnPageIndexChanging2(object sender, GridViewPageEventArgs e)
        {
            populateDataCars();
            dgvCars.PageIndex = e.NewPageIndex;
            dgvCars.DataBind();



        }


    }
}