using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using Date = System.DateTime;


namespace CTBTeam
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)Session["loginStatus"]))
            {
                Session["loginStatus"] = "Sign In";
            }
        }
        protected void View_More_onClick(object sender, EventArgs e)
        {

            Response.Redirect("Hours.aspx");

        }
        protected void download_database(object sender, EventArgs e)
        {
            string[] arrLine = System.IO.File.ReadAllLines(@"" + Server.MapPath("~/Logs/TimeLog/Time-log.txt"));
            string date = arrLine[arrLine.Length - 1];
            string fileName = date.Replace("/", "-");
            try
            {



                if (!System.IO.File.Exists(@"" + Server.MapPath("~/Logs/CurrentHours/Current-Hours_" + fileName + ".txt")))
                {
                    System.IO.File.Create(@"" + Server.MapPath("~/Logs/CurrentHours/Current-Hours_" + fileName + ".txt"));
                }


                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Logs/CurrentHours/Current-Hours_" + fileName + ".txt")))
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
                    headerRow = Date.Parse(date).ToShortDateString() + "," + headerRow;
                    file.WriteLine(headerRow);
                    string text = "";

                    OleDbCommand objProject = new OleDbCommand("SELECT * FROM ProjectHours;", objConn);

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

                    OleDbCommand fieldCarNames = new OleDbCommand("SELECT * FROM VehicleHours", objConn);
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

                    OleDbCommand objCar = new OleDbCommand("SELECT * FROM VehicleHours;", objConn);
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
                    file.Close();
                    readerCar.Close();
                    objConn.Close();

                }
            }

            catch (Exception ex)
            {

                if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                {
                    file.WriteLine(Date.Today.ToString() + "--Create Current Hours--" + ex.ToString());
                    file.Close();
                }
            }
            try
            {
                Response.ContentType = "Application/txt";
                Response.AppendHeader("Content-Disposition", "attachment; filename=Current-Hours_" + fileName + ".txt");
                Response.TransmitFile(Server.MapPath("~/Logs/CurrentHours/Current-Hours_" + fileName + ".txt"));
                Response.End();
            }
                catch (Exception ex)
            {
                    if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                    {
                        System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                    }
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                    {
                        file.WriteLine(Date.Today.ToString() + "--Download Current Hours--" + ex.ToString());
                        file.Close();
                    }
                }
        }
             





        
        protected void download_timelog(object sender, EventArgs e)
        {
            Response.ContentType = "Application/txt";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Time-log.txt");
            Response.TransmitFile(Server.MapPath("~/Logs/TimeLog/Time-log.txt"));
            Response.End();
        }
        protected void download_file_hexGenerator(object sender, EventArgs e)
        {
            Response.ContentType = "Application/exe";
            Response.AppendHeader("Content-Disposition", "attachment; filename=HexGenerator.exe");
            Response.TransmitFile(Server.MapPath("~/HexGenerator.exe"));
            Response.End();
        }
    }
}