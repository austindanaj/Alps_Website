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
using System.IO;

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CTBTeam
{
    public partial class Login : Page
    {

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                if (!string.IsNullOrEmpty((string)Session["User"]))
                {
                    txtUser.Visible = false;
                    txtPass.Visible = false;
                    btnLogin.Text = "Sign Out";
                    lblLogin.Text = "Sign Out";
                    string userName = (string)Session["User"];
                    if (userName.Equals("admin"))
                    {
                        lblRegister.Visible = true;
                        txtName.Visible = true;
                        txtRUser.Visible = true;
                        txtRPass.Visible = true;
                        txtRConfirm.Visible = true;
                        btnRegister.Visible = true;

                    }
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
                    GetWindowThreadProcessId(new IntPtr(currentApp.Hwnd), out processID);
                    currentWB.Close();
                    currentApp.Quit();
               
                    Marshal.FinalReleaseComObject(currentWB);
                    Marshal.FinalReleaseComObject(currentApp);
                    Session["excelWB"] = null;
                    Session["excelApp"] = null;
                    
                   

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
        protected void Login_Clicked(Object sender, EventArgs e)
        {
            if (btnLogin.Text.Equals("Sign Out"))
            {
                Excel.Workbook wb = (Excel.Workbook)Session["excelWB"];
                Excel.Application app = (Excel.Application)Session["excelApp"];
                cleanUpExcel(app, wb);
                Session["loginStatus"] = "Sign In";
                Session["User"] = null;
                Response.Redirect("~/");
            }
            else
            {
                string[] userList = File.ReadAllLines(Server.MapPath("~/Users.txt"));
                for (int i = 0; i < userList.Length; i += 3)
                {
                    if (txtUser.Text.Equals(userList[i]))
                    {
                        if (txtPass.Text.Equals(userList[i + 1]))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('You have been successfully logged in!');", true);
                            txtPass.Text = "";
                            txtUser.Text = "";
                            Session["User"] = userList[i + 2];
                            Session["loginStatus"] = "Sign Out";
                            Response.Redirect("Hours.aspx");
                            return;
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Error: Incorrect Username or Password');", true);
                            txtPass.Text = "";
                            txtUser.Text = "";
                            return;
                        }
                    }

                }
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Error: User has not been registered yet, please contact admin');", true);
                txtPass.Text = "";
                txtUser.Text = "";
                return;
            }
        }
        protected void Register_Clicked(object sender, EventArgs e)
        {
            if (!(txtRPass.Text.Equals("") || txtRConfirm.Text.Equals("") || txtName.Text.Equals("")))
            {
                if (txtRPass.Text.Equals(txtRConfirm.Text))
                {
                    string[] addUser = { txtRUser.Text.ToLower(), txtRPass.Text, txtName.Text };
                    File.AppendAllLines(Server.MapPath("~/Users.txt"), addUser);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Error: Passwords don't match!');", true);

                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Error: Line blank! Please fill in all fields!');", true);
            }
          
        }


    }
    



    
}