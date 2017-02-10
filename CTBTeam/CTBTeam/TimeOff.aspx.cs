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
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CTBTeam
{
    public partial class TimeOff : Page
    {

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


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
                try
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
                catch (Exception ex)
                {

                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty((string)Session["User"]))
            {
                try
                {
                    if (Session["excelApp"] == null)
                    {
                        app = new Excel.Application();
                        Session["excelApp"] = app;
                    }
                    if (Session["excelWB"] == null)
                    {
                        wb = app.Workbooks.Open(@"" + Server.MapPath("~/InternHours.xlsx"));
                        Session["excelWB"] = wb;
                    }
                    app = (Excel.Application)Session["excelApp"];
                    wb = (Excel.Workbook)Session["excelWB"];
                    ws = wb.Sheets[3];

                    for(int i = 2; i <= ws.UsedRange.Rows.Count; i++)
                    {
                        if ((ws.Cells[i, 1] as Excel.Range).Value.ToString().Equals((string)Session["Users"]) && (ws.Cells[i,2] as Excel.Range).Value.ToString().Contains(cldTimeOff.SelectedDate.ToShortDateString())){
                            ws.Rows[i].Delete();
                        }
                    }
                    wb.Save();
                    cleanUpExcel(app, wb);
                    Session["excelApp"] = app;
                    Session["excelWB"] = wb;



                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("alert('");
                    sb.Append("Your time off for: " + cldTimeOff.SelectedDate.ToShortDateString() + " has been removed");
                    sb.Append("');");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString(), true);
                    populate();
                }
                catch (Exception ex)
                {
                    cleanUpExcel(app, wb);
                }
            }
        }
        private void cleanUpExcel(Excel.Application currentApp, Excel.Workbook currentWB)
        {
            uint processID = 0;
            if (currentApp != null)
            {
                if (currentWB != null)
                {
                    GetWindowThreadProcessId(new IntPtr(app.Hwnd), out processID);
                    wb.Close();
                    app.Quit();
                    Marshal.FinalReleaseComObject(ws);
                    Marshal.FinalReleaseComObject(wb);
                    Marshal.FinalReleaseComObject(app);
                    ws = null;
                    wb = null;
                    app = null;

                }
            }
            try
            {
                Process excelProc = Process.GetProcessById((int)processID);
                excelProc.CloseMainWindow();
                excelProc.Refresh();
                excelProc.Kill();
            }
            catch
            {

            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();

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