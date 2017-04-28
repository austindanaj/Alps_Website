using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Date = System.DateTime;
using System.Data.OleDb;
using System.Web.UI.WebControls;

namespace CTBTeam
{
    public partial class TimeOff : Page
    {

     
      
   

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                populateNames();
                cldTimeOff.SelectedDate = DateTime.Now;
                getCurrentDate();
            }
        }
        public void getCurrentDate()
        {
            try
            {
                bltList.Items.Clear();
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                              "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();



                OleDbCommand objCmd = new OleDbCommand("SELECT Emp_Name FROM TimeOff WHERE Date_Request=@date;", objConn);
                objCmd.Parameters.AddWithValue("@date", DateTime.Today.ToShortDateString());
                OleDbDataReader reader = objCmd.ExecuteReader();
                while (reader.Read())
                {
                    bltList.Items.Add(reader["Emp_Name"].ToString());
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
                    file.WriteLine(Date.Today.ToString() + "--Calendar Date Picked--" + ex.ToString());
                    file.Close();
                }
            }
        }

        protected void Calendar_SelectionChanged(object sender, EventArgs e)
        {
            
                try
                {
                    bltList.Items.Clear();
                    String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                  "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                    OleDbConnection objConn = new OleDbConnection(connectionString);
                    objConn.Open();



                    OleDbCommand objCmd = new OleDbCommand("SELECT Emp_Name FROM TimeOff WHERE Date_Request=@value1;", objConn);
                    objCmd.Parameters.AddWithValue("@value1", cldTimeOff.SelectedDate.ToShortDateString());
                    OleDbDataReader reader = objCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        bltList.Items.Add(reader["Emp_Name"].ToString());
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
                        file.WriteLine(Date.Today.ToString() + "--Calendar Date Picked--" + ex.ToString());
                        file.Close();
                    }
                }
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty((string)Session["User"]))
            {
                try
                {
                    String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                  "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                    OleDbConnection objConn = new OleDbConnection(connectionString);
                    objConn.Open();



                    OleDbCommand objCmd = new OleDbCommand("INSERT INTO TimeOff " +
                                                            "(Emp_Name, Date_Request) VALUES (@value1, @value2);", objConn);

                   // objCmd.Parameters.AddWithValue("@value1", (int)Session["alna_num"]);
                    objCmd.Parameters.AddWithValue("@value1", ddlNames.Text);
                    objCmd.Parameters.AddWithValue("@value2", cldTimeOff.SelectedDate.ToShortDateString());

                    objCmd.ExecuteNonQuery();
                    objConn.Close();
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("alert('");
                    sb.Append("Your time off for: " + cldTimeOff.SelectedDate.ToShortDateString() + " has been successfully added");
                    sb.Append("');");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString(), true);

                }
                catch (Exception ex)
                {
                    if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                    {
                        System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                    }
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                    {
                        file.WriteLine(Date.Today.ToString() + "--Add Time Off--" + ex.ToString());
                        file.Close();
                    }
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty((string)Session["User"]))
            {
                try
                {
                    String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                  "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                    OleDbConnection objConn = new OleDbConnection(connectionString);
                    objConn.Open();



                    OleDbCommand objCmd = new OleDbCommand("DELETE FROM TimeOff WHERE Emp_Name=@value1 AND Date_Request=@value2", objConn);

                    objCmd.Parameters.AddWithValue("@value1", ddlNames.Text);
                    objCmd.Parameters.AddWithValue("@value2", cldTimeOff.SelectedDate.ToShortDateString());

                    objCmd.ExecuteNonQuery();
                    objConn.Close();
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("alert('");
                    sb.Append("Your time off for: " + cldTimeOff.SelectedDate.ToShortDateString() + " has been successfully removed");
                    sb.Append("');");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString(), true);

                }
                catch (Exception ex)
                {
                    if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                    {
                        System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                    }
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                    {
                        file.WriteLine(Date.Today.ToString() + "--Remove Time Off--" + ex.ToString());
                        file.Close();
                    }
                }
            }
        }
        public void populateNames()
        {
            try
            {
                ddlNames.Items.Clear();
                
                ddlNames.Items.Add("--Select A Name--");
              
                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                       "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                OleDbCommand objCmdSelect = new OleDbCommand("SELECT Emp_Name FROM Users ORDER BY Emp_Name", objConn);
                OleDbDataReader reader = objCmdSelect.ExecuteReader();
                while (reader.Read())
                {
                    ddlNames.Items.Add(new ListItem(reader.GetString(0)));
                  
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


    }
       
    
}