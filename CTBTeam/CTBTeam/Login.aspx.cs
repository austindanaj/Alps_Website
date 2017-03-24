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
                    if ((bool)Session["admin"])
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



                    OleDbCommand objCmd = new OleDbCommand("SELECT * FROM Users WHERE Alna_Num=@value1 AND Emp_Pass=@value2", objConn);

                    objCmd.Parameters.AddWithValue("@value1", int.Parse(txtUser.Text.Replace("alna", "")));
                    objCmd.Parameters.AddWithValue("@value2", txtPass.Text);

                    OleDbDataReader reader = objCmd.ExecuteReader();
                    int count = 0;
                    while (reader.Read())
                    {
                        count++;
                        Session["User"] = reader.GetString(1);
                        Session["alna_num"] = reader.GetValue(0);
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

                }

            }  
           
        }
        protected void Register_Clicked(object sender, EventArgs e)
        {
            if (!(txtRPass.Text.Equals("") || txtRConfirm.Text.Equals("") || txtName.Text.Equals("")))
            {
                if (txtRPass.Text.Equals(txtRConfirm.Text))
                {

                    try
                    {
                        String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                      "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                        OleDbConnection objConn = new OleDbConnection(connectionString);
                        objConn.Open();



                        OleDbCommand objCmd = new OleDbCommand("INSERT INTO Users (Alna_Num, Emp_Name, Emp_Pass, Admin) VALUES (@value1, @value2, @value3, @value4);", objConn);

                        objCmd.Parameters.AddWithValue("@value1", int.Parse(txtRUser.Text.Replace("alna", "")));
                        objCmd.Parameters.AddWithValue("@value2", txtName.Text);
                        objCmd.Parameters.AddWithValue("@value3", txtRPass.Text);
                        objCmd.Parameters.AddWithValue("@value4", 0);

                        objCmd.ExecuteNonQuery();
                        objConn.Close();
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('User successfully added');", true);
                    }
                    catch (Exception ex)
                    {

                    }                  
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