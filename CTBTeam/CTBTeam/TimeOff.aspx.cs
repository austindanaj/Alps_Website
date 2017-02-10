using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;
using Date = System.DateTime;
using System.Data.OleDb;
using System.Data;

namespace CTBTeam
{
    public partial class TimeOff : Page
    {

        Excel.Application app;
        Excel.Workbook wb;
        Excel.Worksheet ws;
        string[,] data;
        string userName;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty((string)Session["User"]))
                {
                    if (Session["objData"] == null)
                    {
                        populate();
                    }
                }
            }                  
         }
      

        protected void Calendar_SelectionChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty((string)Session["User"]))
            {
                bltList.Items.Clear();
                DataSet objDataSet = (DataSet)Session["objData"];
                for(int i = 0; i < objDataSet.Tables[0].Rows.Count; i++)
                {
                    if((objDataSet.Tables[0].Rows[i][1].ToString().Contains(cldTimeOff.SelectedDate.ToShortDateString())))
                    {
                        bltList.Items.Add((string)objDataSet.Tables[0].Rows[i][0]);

                       
                    }
                }

            }
     
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty((string)Session["User"]))
            {

                String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                              "Data Source=" + Server.MapPath("~/InternHours.xlsx") + ";" +
                              "Extended Properties=Excel 12.0;";
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();

                OleDbCommand objCmd = new OleDbCommand("INSERT INTO [Sheet3$] " +
                                                      "([Name],[Date/Time]) " +
                                                      "VALUES(@value1, @value2)", objConn);

                objCmd.Parameters.AddWithValue("@value1", (string)Session["User"]);
                objCmd.Parameters.AddWithValue("@value2", cldTimeOff.SelectedDate.ToShortDateString());
              
                objCmd.ExecuteNonQuery();
                objConn.Close();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("alert('");
                sb.Append("Your time off for: " + cldTimeOff.SelectedDate.ToShortDateString() + " has been successfully added");
                sb.Append("');");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString(), true);
                populate();
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty((string)Session["User"]))
            {
                try
                {
                    String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                            "Data Source=" + Server.MapPath("~/InternHours.xlsx") + ";" +
                            "Extended Properties=Excel 12.0;";
                    OleDbConnection objConn = new OleDbConnection(connectionString);
                
                    DataSet objDataSet = (DataSet)Session["objData"];


                    OleDbDataAdapter objCmd = new OleDbDataAdapter();
                  


                    string sql = "DELETE FROM [Sheet3$] WHERE [Date/Time]=@val2 AND [Name]=@val1";
                    objConn.Open();

                    objCmd.DeleteCommand = objConn.CreateCommand();
                    objCmd.DeleteCommand.CommandText = sql;
                    objCmd.DeleteCommand.Parameters.AddWithValue("@val1", (string)Session["User"]);
                    objCmd.DeleteCommand.Parameters.AddWithValue("@val2", cldTimeOff.SelectedDate.ToShortDateString());                  


                    objCmd.DeleteCommand.ExecuteNonQuery();
                    objConn.Close();
                }
                catch(Exception ex)
                {
                    
                }
            }
        }
        public void populate()
        {
            String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                            "Data Source=" + Server.MapPath("~/InternHours.xlsx") + ";" +
                                            "Extended Properties=Excel 12.0;";
            OleDbConnection objConn = new OleDbConnection(connectionString);
            objConn.Open();
            OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [Sheet3$]", objConn);
            OleDbDataAdapter objAdapter = new OleDbDataAdapter();
            objAdapter.SelectCommand = objCmdSelect;
            DataSet objDataSet = new DataSet();
            objAdapter.Fill(objDataSet, "XLData");
            Session["objData"] = objDataSet;
            objConn.Close();
        }

       
    }
}