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
        string date;
        

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!IsPostBack)
            {
               
                TextBox[] txtBoxProjects = { txtPB, txtTherm, txtGA, txtRadar, txtIR, txtOther };
                CheckBox[] chkBoxProjects = { chkPB, chkTherm, chkGA, chkRadar, chkIR, chkOther };
                TextBox[] txtBoxCars = { txtCar1, txtCar2, txtCar3, txtCar4, txtCar5, txtCar6, txtCar7, txtCar8, txtCar9, txtCar10 };
                CheckBox[] chkBoxCars = { chkCar1, chkCar2, chkCar3, chkCar4, chkCar5, chkCar6, chkCar7, chkCar8, chkCar9, chkCar10 };
                populateDataCars();
                populateDataProject();
                lblWeekOf.Text = "Week Of: " + dgvProject.HeaderRow.Cells[0].Text;
                if (datechange())
                {
                    populateDataCars();
                    populateDataProject();
                    lblWeekOf.Text = "Week Of: " + dgvProject.HeaderRow.Cells[0].Text;
                }

                if (!string.IsNullOrEmpty((string)Session["User"]))
                {

                    
                    
                    userName = (string)Session["User"];
                  
                 
                   

                  
                    btnSubmit.Visible = true;

                    if (userName.Equals("admin"))
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
                        txtUsers.Visible = true;
                        txtProjects.Visible = true;
                        txtCars.Visible = true;
                        btnUpdate.Visible = true;
                       
                        ws = wb.Sheets[1];
                        for(int i = 2; i <= ws.UsedRange.Rows.Count; i++)
                        {
                          
                            txtUsers.Text += ws.Cells[i, 1].Value.ToString() + ",";
                        }
                        if (!(txtUsers.Text.Equals(""))){
                            txtUsers.Text = txtUsers.Text.Substring(0, txtUsers.Text.Length - 1);
                        }
                     
                        for (int i = 2; i <= ws.UsedRange.Columns.Count; i++)
                        {
                            
                            txtProjects.Text += ws.Cells[1, i].Value.ToString() + ",";
                        }
                        txtProjects.Text = txtProjects.Text.Substring(0, txtProjects.Text.Length - 1);
                        ws = wb.Sheets[4];
                        for (int i = 2; i <= ws.UsedRange.Columns.Count; i++)
                        {
                           
                            txtCars.Text += ws.Cells[1, i].Value.ToString() + ",";
                        }
                        txtCars.Text = txtCars.Text.Substring(0, txtCars.Text.Length - 1);
                        wb.Save();
                        Session["excelWB"] = wb;
                        Session["excelApp"] = app;

                    }
                    else
                    {
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



        }
        public bool datechange()
        {
            try
            {
                if (Date.Today.AddDays(-6) > Date.Parse(dgvProject.HeaderRow.Cells[0].Text))
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


                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Time-log.txt"), true))
                    {
                        string text = "";
                        ws = wb.Sheets[1];
                     
                        for (int i = 1; i <= ws.UsedRange.Rows.Count; i++)
                        {
                            //Loop through each column
                            for (int j = 1; j <= ws.UsedRange.Columns.Count; j++)
                            {
                                //Get the value of the cells, save to string, seperated by commas
                                text += ws.Cells[i,j].Value+ ",";
                              
                            }
                            //write a line to the file
                            file.WriteLine(text);
                            //reset the text variable to empty string, before getting the next line of text
                            text = "";
                        }
                        //finished writing, write a spacer line to view easier in log file
                        file.WriteLine();
                        wb.Save();

                        ws = wb.Sheets[4];
                        //Loop through each row
                        for (int i = 1; i <= ws.UsedRange.Rows.Count; i++)
                        {
                            //Loop through each column
                            for (int j = 1; j <= ws.UsedRange.Columns.Count; j++)
                            {
                                //Get the value of the cells, save to string, seperated by commas
                                text += ws.Cells[i, j].Value + ",";                              
                            }
                            //write a line to the file
                            file.WriteLine(text);
                            //reset the text variable to empty string, before getting the next line of text
                            text = "";
                        }
                        file.WriteLine();
                    }
                    wb.Save();


                    ws = wb.Sheets[2];
                    //Delete the existing rows
                    ws.UsedRange.Rows.Delete();
                    ws.Cells[1, "A"].Value = "Date";
                    ws.Cells[1, "B"].Value = "Name";
                    ws.Cells[1, "C"].Value = "Detail";
                    ws.Cells[1, "D"].Value = "Hours";
                    wb.Save();
                    ws = wb.Sheets[1];



                    DateTime weekOf = DateTime.Parse(ws.Cells[1, 1].Value.ToString().Substring(0, ws.Cells[1, 1].Value.ToString().IndexOf(" ")));

                    if (Date.Today.AddDays(-6) > weekOf)
                    {
                        DateTime dt = DateTime.Now;
                        while (dt.DayOfWeek != DayOfWeek.Monday) dt = dt.AddDays(-1);
                        ws.Cells[1, 1].Value = dt.ToShortDateString();


                    }
                    // lblWeekOf.Text = "Week Of: " + ws.Cells[1, 1].Value.ToString().Substring(0, ws.Cells[1, 1].Value.ToString().IndexOf(" "));
                    this.wb.Save();
                    cleanUpExcel(app, wb);
                    return true;














                }
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }         
            
        }
        protected void Update(object sender, EventArgs e)
        {
            app = (Excel.Application)Session["excelApp"];
            wb = (Excel.Workbook)Session["excelWB"];
            txtUsers.Text = Regex.Replace(txtUsers.Text, " *, *", ",");
            string[] users = txtUsers.Text.Split(',');
            txtProjects.Text = Regex.Replace(txtProjects.Text, " *, *", ",");
            string[] projects = txtProjects.Text.Split(',');
            txtCars.Text = Regex.Replace(txtCars.Text, " *, *", ",");
            string[] cars = txtCars.Text.Split(',');
            
            string[] rows = new string[users.Length];
            string[] col1 = new string[projects.Length];
            string[] col4 = new string[cars.Length];
            int colCount = 0;
            int rowCount = 0;
            try
            {
               
                
                //Sheet 1

                ws = wb.Sheets[1];
                for (int k = 2; k < projects.Length + 2; k++)
                {
                    (ws.Cells[1, k] as Excel.Range).Value = projects[k - 2];
                }
                colCount = ws.UsedRange.Columns.Count;
                for (int m = 2; m <= colCount; m++)
                {
                    if (m - 2 >= col1.Length)
                    {
                        ws.Columns[m].Delete();
                    }
                    else
                    {
                        col1[m - 2] = ColumnIndexToColumnLetter(m);
                    }
                }
                wb.Save();
                rowCount = ws.UsedRange.Rows.Count;
                for (int i = 2; i <= rows.Length + 1; i++)
                {
                        rows[i - 2] = "A" + (i).ToString();
                        ws.Cells[i, "A"].Value = users[i - 2];
                        for (int j = 2; j <= ws.UsedRange.Columns.Count; j++)
                        {
                            ws.Cells[i, j].Value = "=SUMIFS(Sheet2!D:D,Sheet2!C:C,\"*\"&" + col1[j - 2] +  "1&\"*\",Sheet2!B:B,\"*\"&" + rows[i - 2] + "&\"*\",Sheet2!A:A,A1)";
                        }
                    
                }



                //Sheet 4

                ws = wb.Sheets[4];
                for (int l = 2; l < cars.Length + 2; l++)
                {
                    ws.Cells[1, l].Value = cars[l - 2];
                }
                colCount = ws.UsedRange.Columns.Count;
                for (int n = 2; n <= colCount; n++)
                {
                    if (n - 2 >= col4.Length)
                    {
                        ws.Columns[n].Delete();
                    }
                    else
                    {
                        col4[n - 2] = ColumnIndexToColumnLetter(n);
                    }
                }
                wb.Save();
                rowCount = ws.UsedRange.Rows.Count;
                for (int i = 2; i <= rows.Length + 1; i++)
                {
                    
                    
                        rows[i - 2] = "A" + (i).ToString();
                        ws.Cells[i, "A"].Value = users[i - 2];
                        for (int j = 2; j <= ws.UsedRange.Columns.Count; j++)
                        {
                            ws.Cells[i, j].Value = "=SUMIFS(Sheet2!D:D,Sheet2!C:C,\"*\"&" + col4[j - 2] + "1&\"*\",Sheet2!B:B,\"*\"&" + rows[i - 2] + "&\"*\",Sheet2!A:A,A1)";
                        }
                    
                }
                wb.Save();
                TextBox[] txtBoxProjects = { txtPB, txtTherm, txtGA, txtRadar, txtIR, txtOther };
                CheckBox[] chkBoxProjects = { chkPB, chkTherm, chkGA, chkRadar, chkIR, chkOther };
                TextBox[] txtBoxCars = { txtCar1, txtCar2, txtCar3, txtCar4, txtCar5, txtCar6, txtCar7, txtCar8, txtCar9, txtCar10 };
                CheckBox[] chkBoxCars = { chkCar1, chkCar2, chkCar3, chkCar4, chkCar5, chkCar6, chkCar7, chkCar8, chkCar9, chkCar10 };

                ws = wb.Sheets[1];
                for (int i = 2; i <= ws.UsedRange.Columns.Count; i++)
                {
                    //Projects 
                    chkBoxProjects[i - 2].Visible = true;
                    chkBoxProjects[i - 2].Text = ws.Cells[1, i].Value.ToString();
                    txtBoxProjects[i - 2].Visible = true;

                }

                ws = wb.Sheets[4];
                for (int i = 2; i <= ws.UsedRange.Columns.Count; i++)
                {
                    //Cars
                    chkBoxCars[i - 2].Visible = true;
                    chkBoxCars[i - 2].Text = ws.Cells[1, i].Value.ToString();
                    txtBoxCars[i - 2].Visible = true;

                }
             
                wb.Save();
                cleanUpExcel(app, wb);



                populateDataCars();
                populateDataProject();


                app = new Excel.Application();
                wb = app.Workbooks.Open(@"" + Server.MapPath("~/InternHours.xlsx"));
                Session["excelWB"] = wb;
                Session["excelApp"] = app;
            }
            catch(Exception ex)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("alert('");
                sb.Append(ex.ToString());
                sb.Append("');");
                ClientScript.RegisterOnSubmitStatement(this.GetType(), "alert", sb.ToString());


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
                                "Data Source=" + Server.MapPath("~/InternHours.xlsx") + ";" +
                                "Extended Properties=Excel 12.0;";
            OleDbConnection objConn = new OleDbConnection(connectionString);
            objConn.Open();          
                   

            TextBox[] temp = { txtPB, txtTherm, txtGA, txtRadar, txtIR, txtOther, txtCar1, txtCar2, txtCar3, txtCar4, txtCar5, txtCar6, txtCar7, txtCar8, txtCar9, txtCar10 };
            textBoxes = temp;
            CheckBox[] temp2 = { chkPB, chkTherm, chkGA, chkRadar, chkIR, chkOther, chkCar1, chkCar2, chkCar3, chkCar4, chkCar5, chkCar6, chkCar7, chkCar8, chkCar9, chkCar10 };
            checkBoxes = temp2;
            DateTime dt = DateTime.Now;
            while (dt.DayOfWeek != DayOfWeek.Monday) dt = dt.AddDays(-1);
            string date = dt.ToShortDateString();
            for (int i = 0; i < checkBoxes.Length; i++)
            {
                if (!textBoxes[i].Text.Equals("0"))
                {
                    
                    OleDbCommand objCmd = new OleDbCommand("INSERT INTO [Sheet2$] " +
                                                          "([Date],[Name],[Detail],[Hours]) " +
                                                          "VALUES(@value1, @value2, @value3, @value4)", objConn);
                    
                    objCmd.Parameters.AddWithValue("@value1", date);
                    objCmd.Parameters.AddWithValue("@value2", (string)Session["User"]);
                    objCmd.Parameters.AddWithValue("@value3", checkBoxes[i].Text);                    
                    objCmd.Parameters.AddWithValue("@value4", int.Parse(textBoxes[i].Text));

                    objCmd.ExecuteNonQuery();
                }
            }
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
                                     "Data Source=" + Server.MapPath("~/InternHours.xlsx") + ";" +
                                     "Extended Properties=Excel 12.0;";
            OleDbConnection objConn = new OleDbConnection(connectionString);
            objConn.Open();
            OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [Sheet4$]", objConn);
            OleDbDataAdapter objAdapter = new OleDbDataAdapter();
            objAdapter.SelectCommand = objCmdSelect;
            DataSet objDataSet = new DataSet();
            objAdapter.Fill(objDataSet, "XLData");
            dgvCars.DataSource = objDataSet.Tables[0].DefaultView;
            dgvCars.DataBind();
            objConn.Close();
        }
    
        public void populateDataProject()
        {
            String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                   "Data Source=" + Server.MapPath("~/InternHours.xlsx") + ";" +
                                   "Extended Properties=Excel 12.0;";
            OleDbConnection objConn = new OleDbConnection(connectionString);
            objConn.Open();
            OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [Sheet1$]", objConn);
            OleDbDataAdapter objAdapter = new OleDbDataAdapter();
            objAdapter.SelectCommand = objCmdSelect;
            DataSet objDataSet = new DataSet();
            objAdapter.Fill(objDataSet, "XLData");
            dgvProject.DataSource = objDataSet.Tables[0].DefaultView;
            dgvProject.DataBind();
            objConn.Close();
          
        }






    }
}