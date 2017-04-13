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

namespace CTBTeam
{
    public partial class Admin : Page
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {
         



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
                        if (!System.IO.File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                        {
                            System.IO.File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
                        }
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
                        {
                            file.WriteLine(Date.Today.ToString() + "--Register--" + ex.ToString());
                            file.Close();
                        }
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
        protected void Project_Clicked(object sender, EventArgs e)
        {

        }













    }
}