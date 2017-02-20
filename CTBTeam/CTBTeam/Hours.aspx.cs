using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;
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
        Excel.Application app;
        Excel.Workbook wb;
        Excel.Worksheet ws;
        int rowCount = 0;
        //int dateIncrease = 0;
        TextBox[] textBoxes;
        string userName;
        CheckBox[] checkBoxes;
        string date = "";
        

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!IsPostBack)
            {

                getDate();

                TextBox[] txtBoxProjects = { txtPB, txtTherm, txtGA, txtRadar, txtIR, txtOther };
                CheckBox[] chkBoxProjects = { chkPB, chkTherm, chkGA, chkRadar, chkIR, chkOther };
                TextBox[] txtBoxCars = { txtCar1, txtCar2, txtCar3, txtCar4, txtCar5, txtCar6, txtCar7, txtCar8, txtCar9, txtCar10 };
                CheckBox[] chkBoxCars = { chkCar1, chkCar2, chkCar3, chkCar4, chkCar5, chkCar6, chkCar7, chkCar8, chkCar9, chkCar10 };
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

                 
                    int colCount = dgvProject.Rows[0].Cells.Count;
                    for (int i = 1; i < colCount; i++)
                    {
                        //Projects 
                        chkBoxProjects[i - 1].Visible = true;
                        chkBoxProjects[i - 1].Text = dgvProject.HeaderRow.Cells[i].Text;
                        txtBoxProjects[i - 1].Visible = true;
                    }

                    colCount = dgvCars.Rows[0].Cells.Count;
                    for (int i = 1; i < colCount; i++)
                    {
                        //Cars
                        chkBoxCars[i - 1].Visible = true;
                        chkBoxCars[i - 1].Text = dgvCars.HeaderRow.Cells[i].Text;
                        txtBoxCars[i - 1].Visible = true;

                    }

                }                                   
                                                     
            }

        }
        public void getDate()
        {
            string[] arrLine = System.IO.File.ReadAllLines(@"" + Server.MapPath("~/Time-log.txt"));
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
                    string[] arrLine = System.IO.File.ReadAllLines(@"" + Server.MapPath("~/Time-log.txt"));

                    arrLine[arrLine.Length - 1] = Date.Parse(date).ToShortDateString() + "," + headerRow;
                    System.IO.File.WriteAllLines(@"" + Server.MapPath("~/Time-log.txt"), arrLine);


                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Time-log.txt"), true))
                    {

                        string text = "";

                        OleDbCommand objProject = new OleDbCommand("SELECT * FROM ProjectHours;", objConn);

                        OleDbDataReader readerProject = objProject.ExecuteReader();
                        while (readerProject.Read())
                        {

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

                            text += readerCar.GetString(1) + "," + readerCar.GetValue(2).ToString() + "," + readerCar.GetValue(3).ToString() + "," + readerCar.GetValue(4).ToString() + ","
                                  + readerCar.GetValue(5).ToString() + "," + readerCar.GetValue(6).ToString() + "," + readerCar.GetValue(7).ToString() + "," + readerCar.GetValue(8).ToString() + ","
                                  + readerCar.GetValue(9).ToString() + ",";
                            file.WriteLine(text);
                        }
                        file.WriteLine();
                        readerCar.Close();
                        objConn.Close();       
                        
                        if (Date.Today.AddDays(-6) > Date.Parse(date))
                        {
                            DateTime dt = DateTime.Now;
                            while (dt.DayOfWeek != DayOfWeek.Monday) dt = dt.AddDays(-1);
                            file.Write(dt.ToShortDateString());


                        }
                      
                       
                        return true;

                    }
                  

                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }         
            
        }
       

        private void cleanUpExcel(Excel.Application currentApp, Excel.Workbook currentWB)
        {
            uint processID = 0;
            if(currentApp != null)
            {
                if(currentWB != null)
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
        static string ColumnIndexToColumnLetter(int colIndex)
        {
            int div = colIndex;
            string colLetter = String.Empty;
            int mod = 0;

            while (div > 0)
            {
                mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = (int)((div - mod) / 26);
            }
            return colLetter;
        }






        //=======================================================
        protected void On_Click_PB(object sender, EventArgs e)
        {
            if (chkPB.Checked == true)
            {
                txtPB.Enabled = true;
            }
            else
            {
                txtPB.Enabled = false;
                txtPB.Text = "0";

            }
        }
        protected void On_Click_Therm(object sender, EventArgs e)
        {
            if (chkTherm.Checked == true)
            {
                txtTherm.Enabled = true;
            }
            else
            {
                txtTherm.Enabled = false;
                txtTherm.Text = "0";

            }
        }
        protected void On_Click_GA(object sender, EventArgs e)
        {
            if (chkGA.Checked == true)
            {
                txtGA.Enabled = true;
            }
            else
            {
                txtGA.Enabled = false;
                txtGA.Text = "0";
            }
        }
        protected void On_Click_Other(object sender, EventArgs e)
        {
            if (chkOther.Checked == true)
            {
                txtOther.Enabled = true;
            }
            else
            {
                txtOther.Enabled = false;
                txtOther.Text = "0";
            }
        }
        protected void On_Click_Radar(object sender, EventArgs e)
        {
            if (chkRadar.Checked == true)
            {
                txtRadar.Enabled = true;
            }
            else
            {
                txtRadar.Enabled = false;
                txtRadar.Text = "0";
            }
        }
        protected void On_Click_IR(object sender, EventArgs e)
        {
            if (chkIR.Checked == true)
            {
                txtIR.Enabled = true;
            }
            else
            {
                txtIR.Enabled = false;
                txtIR.Text = "0";
            }
        }
        //=======================================================
        protected void On_Click_Car1(object sender, EventArgs e)
        {
            if (chkCar1.Checked == true)
            {
                txtCar1.Enabled = true;
            }
            else
            {
                txtCar1.Enabled = false;
                txtCar1.Text = "0";
            }
        }
        protected void On_Click_Car2(object sender, EventArgs e)
        {
            if (chkCar2.Checked == true)
            {
                txtCar2.Enabled = true;
            }
            else
            {
                txtCar2.Enabled = false;
                txtCar2.Text = "0";
            }
        }
        protected void On_Click_Car3(object sender, EventArgs e)
        {
            if (chkCar3.Checked == true)
            {
                txtCar3.Enabled = true;
            }
            else
            {
                txtCar3.Enabled = false;
                txtCar3.Text = "0";
            }
        }
        protected void On_Click_Car4(object sender, EventArgs e)
        {
            if (chkCar4.Checked == true)
            {
                txtCar4.Enabled = true;
            }
            else
            {
                txtCar4.Enabled = false;
                txtCar4.Text = "0";
            }
        }
        protected void On_Click_Car5(object sender, EventArgs e)
        {
            if (chkCar5.Checked == true)
            {
                txtCar5.Enabled = true;
            }
            else
            {
                txtCar5.Enabled = false;
                txtCar5.Text = "0";
            }
        }
        protected void On_Click_Car6(object sender, EventArgs e)
        {
            if (chkCar6.Checked == true)
            {
                txtCar6.Enabled = true;
            }
            else
            {
                txtCar6.Enabled = false;
                txtCar6.Text = "0";
            }
        }
        protected void On_Click_Car7(object sender, EventArgs e)
        {
            if (chkCar7.Checked == true)
            {
                txtCar7.Enabled = true;
            }
            else
            {
                txtCar7.Enabled = false;
                txtCar7.Text = "0";
            }
        }
        protected void On_Click_Car8(object sender, EventArgs e)
        {
            if (chkCar8.Checked == true)
            {
                txtCar8.Enabled = true;
            }
            else
            {
                txtCar8.Enabled = false;
                txtCar8.Text = "0";
            }
        }
        protected void On_Click_Car9(object sender, EventArgs e)
        {
            if (chkCar9.Checked == true)
            {
                txtCar9.Enabled = true;
            }
            else
            {
                txtCar9.Enabled = false;
                txtCar9.Text = "0";
            }
        }
        protected void On_Click_Car10(object sender, EventArgs e)
        {
            if (chkCar10.Checked == true)
            {
                txtCar10.Enabled = true;
            }
            else
            {
                txtCar10.Enabled = false;
                txtCar10.Text = "0";
            }
        }
        //=======================================================
        protected void On_Click_Submit(object sender, EventArgs e)
        {
            userName = (string)Session["User"];
            if (userName == null)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "myalert", "alert('Error: Please Log in first before submitting!');", true);
                return;
            }
            String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
        
            OleDbConnection objConn = new OleDbConnection(connectionString);
            objConn.Open();          
                   

            TextBox[] temp = { txtPB, txtTherm, txtGA, txtRadar, txtIR, txtOther, txtCar1, txtCar2, txtCar3, txtCar4, txtCar5, txtCar6, txtCar7, txtCar8, txtCar9, txtCar10 };
            textBoxes = temp;
            CheckBox[] temp2 = { chkPB, chkTherm, chkGA, chkRadar, chkIR, chkOther, chkCar1, chkCar2, chkCar3, chkCar4, chkCar5, chkCar6, chkCar7, chkCar8, chkCar9, chkCar10 };
            checkBoxes = temp2;
        
                    
            OleDbCommand objCmdProject = new OleDbCommand("UPDATE ProjectHours " +
                                                    "SET Project_B=@value1, Thermostat=@value2, Global_A=@value3, Radar=@value4, IR_Sensor=@value5, Other=@value6 "+
                                                    "WHERE Alna_Num=@value7", objConn);

            objCmdProject.Parameters.AddWithValue("@value1", int.Parse(txtPB.Text));
            objCmdProject.Parameters.AddWithValue("@value2", int.Parse(txtTherm.Text));
            objCmdProject.Parameters.AddWithValue("@value3", int.Parse(txtGA.Text));
            objCmdProject.Parameters.AddWithValue("@value4", int.Parse(txtRadar.Text));
            objCmdProject.Parameters.AddWithValue("@value5", int.Parse(txtIR.Text));
            objCmdProject.Parameters.AddWithValue("@value6", int.Parse(txtOther.Text));
            objCmdProject.Parameters.AddWithValue("@value7", (int)(Session["alna_num"]));

            objCmdProject.ExecuteNonQuery();

            /*
            OleDbCommand objCmdCars = new OleDbCommand("UPDATE ProjectHours " +
                                                   "SET Project_B=@value1, Thermostat=@value2, Global_A=@value3, Radar=@value4, IR_Sensor=@value5, Other=@value6 " +
                                                   "WHERE Alna_Num=@value7", objConn);

            objCmdCars.Parameters.AddWithValue("@value1", int.Parse(txtPB.Text));
            objCmdCars.Parameters.AddWithValue("@value2", int.Parse(txtTherm.Text));
            objCmdCars.Parameters.AddWithValue("@value3", int.Parse(txtGA.Text));
            objCmdCars.Parameters.AddWithValue("@value4", int.Parse(txtRadar.Text));
            objCmdCars.Parameters.AddWithValue("@value5", int.Parse(txtIR.Text));
            objCmdCars.Parameters.AddWithValue("@value6", int.Parse(txtOther.Text));
            objCmdCars.Parameters.AddWithValue("@value7", (int)(Session["alna_num"]));
           
            objCmdCars.ExecuteNonQuery();
             */

            objConn.Close();       
            reset();      

            populateDataCars();
            populateDataProject();

        }
        /**
         * Resets the text boxes and checkboxes
         */
        public void reset()
        {

            for (int i = 0; i < checkBoxes.Length; i++)
            {
                checkBoxes[i].Checked = false;
                textBoxes[i].Text = "0";
                textBoxes[i].Enabled = false;
            }

        }
        //==================

        public void populateDataCars()
        {
            String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                 "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
            OleDbConnection objConn = new OleDbConnection(connectionString);
            objConn.Open();
            OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM VehicleHours ", objConn);
            OleDbDataAdapter objAdapter = new OleDbDataAdapter();
            objAdapter.SelectCommand = objCmdSelect;
            DataSet objDataSet = new DataSet();

            objAdapter.Fill(objDataSet);
            objDataSet.Tables[0].Columns.RemoveAt(0);
            dgvCars.DataSource = objDataSet.Tables[0].DefaultView;            
            dgvCars.DataBind();           
            objConn.Close();
        }
    
        public void populateDataProject()
        {
            String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                               "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
            OleDbConnection objConn = new OleDbConnection(connectionString);
            objConn.Open();
            OleDbCommand objCmdSelect = new OleDbCommand("SELECT Emp_Name, Project_B, Thermostat, Global_A, Radar, IR_Sensor, Other FROM ProjectHours", objConn);
            OleDbDataAdapter objAdapter = new OleDbDataAdapter();
            objAdapter.SelectCommand = objCmdSelect;
            DataSet objDataSet = new DataSet();
            objAdapter.Fill(objDataSet);
            dgvProject.DataSource = objDataSet.Tables[0].DefaultView;
            dgvProject.DataBind();       
            objConn.Close();
          
        }






    }
}