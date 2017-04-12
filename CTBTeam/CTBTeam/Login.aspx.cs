using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                   
                }
            }
        }
    
        protected void Login_Clicked(Object sender, EventArgs e)
        {
            if (btnLogin.Text.Equals("Sign Out"))
            {
                Session["loginStatus"] = "Sign In";
                Session["User"] = null;
                Response.Redirect("~/");
            }
            else
            {
                try
                {
                    String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                  "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                    OleDbConnection objConn = new OleDbConnection(connectionString);
                    objConn.Open();



                    OleDbCommand objCmd = new OleDbCommand("SELECT * FROM Account WHERE ACCT_ROLE=@value1 AND ACCT_PASSWORD=@value2", objConn);

                    objCmd.Parameters.AddWithValue("@value1", txtUser.Text);
                    objCmd.Parameters.AddWithValue("@value2", txtPass.Text);

                    OleDbDataReader reader = objCmd.ExecuteReader();
                    int count = 0;
                    while (reader.Read())
                    {
                        count++;
                        Session["User"] = reader.GetString(1);
                     //   Session["alna_num"] = reader.GetValue(0);
                        Session["admin"] = reader.GetBoolean(3);
                    }
                    if (count > 0)
                    {
                        Session["loginStatus"] = "Sign Out";
                        Response.Redirect("Hours.aspx");
                        objConn.Close();
                        return;
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Error: Incorrect Username or Password');", true);
                        txtPass.Text = "";
                        txtUser.Text = "";
                        objConn.Close();
                        return;
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
                        file.WriteLine(Date.Today.ToString() + "--Login--" + ex.ToString());
                        file.Close();
                    }
                }

            }  
           
        }
       


    }
    



    
}