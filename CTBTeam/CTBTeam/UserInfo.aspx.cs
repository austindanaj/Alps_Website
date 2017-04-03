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
    public partial class UserInfo : Page
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty((string)Session["User"]))
            {
                txtOldPass.Visible = true;
                txtNewPass.Visible = true;
                txtConfirmPass.Visible = true;
                btnSubmit.Visible = true;
                lblChangePass.Visible = true;
                
            }





        }
        protected void Change_Password(object sender, EventArgs e)
        {

            if ( !(txtOldPass.Text.Equals("") || txtNewPass.Text.Equals("") || txtConfirmPass.Text.Equals("")) )
            {
                
                try
                {
                    String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                  "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
                    OleDbConnection objConn = new OleDbConnection(connectionString);
                    objConn.Open();

                    OleDbCommand objCheck = new OleDbCommand("SELECT * FROM Users WHERE Alna_Num=@value1 AND Emp_Pass=@value2", objConn);

                    objCheck.Parameters.AddWithValue("@value1", (int)Session["alna_num"]);
                    objCheck.Parameters.AddWithValue("@value2", txtOldPass.Text);

                    OleDbDataReader reader = objCheck.ExecuteReader();
                    int count = 0;
                    while (reader.Read())
                    {
                        count++;
                    }
                   
                    if (count == 1)
                    {
                        if (txtNewPass.Text.Equals(txtConfirmPass.Text))
                        {                       
                            OleDbCommand objCmd = new OleDbCommand("UPDATE Users SET Emp_Pass=@value1 WHERE Alna_Num=@value2", objConn);

                            objCmd.Parameters.AddWithValue("@value1", txtNewPass.Text);
                            objCmd.Parameters.AddWithValue("@value2", (int)Session["alna_num"]);

                            objCmd.ExecuteNonQuery();
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Your password has been successfully changed');", true);

                            objConn.Close();
                            return;
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Passwords dont match!');", true);

                            objConn.Close();
                            return;
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Old Password doesn't match!');", true);
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
                        file.WriteLine(Date.Today.ToString() + "--Change Password--" + ex.ToString());
                        file.Close();
                    }
                }
            }


        }








    }
}