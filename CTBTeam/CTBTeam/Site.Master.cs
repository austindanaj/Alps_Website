using System;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Date = System.DateTime;
using System.Web.UI.HtmlControls;

namespace CTBTeam
{
    public partial class SiteMaster : MasterPage
    {

        private enum SqlTypes { DataTable, VoidQuery, DataReader };
        SqlConnection objConn;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["loginStatus"] == null)
                Session["loginStatus"] = "Sign In";

            if (Session["Alna_num"] == null)
                Session["admin"] = false;

            if (Session["admin"] != null)
                if ((bool)Session["admin"])
                    admin.Visible = true;

            if (Session["Full_time"] != null)
                if (!(bool)Session["Full_time"])
                    lstSchedule.Visible = true;

            if (Session["Vehicle"] == null)
                Session["Vehicle"] = false;

        }


        private void loadEmployees()
        {

            objConn = new SqlConnection(SuperPage.LOCAL_TO_SERVER_CONNECTION_STRING);
            objConn.Open();
            SqlDataReader reader = getReader("SELECT Name FROM Employees where Active=@value1;", true, objConn);

            while (reader.Read())
            {
                ddl.Items.Add(reader.GetString(0));
            }
        }
        private SqlDataReader getReader(string query, object parameters, SqlConnection objConn)
        {
            if (objConn.State == ConnectionState.Closed)
                throw new Exception("You forgot to open the object connection. You have to leave it open until you're done with the data reader.");

            SqlCommand cmd = new SqlCommand(query, objConn);
            if (parameters != null)
            {
                cmd.Parameters.AddWithValue("@value1", parameters);
            }

            return (SqlDataReader)sqlExecuter(cmd, SqlTypes.DataReader); ;
        }
        private SqlDataReader getReader(string query, object[] parameters, SqlConnection objConn)
        {
            if (parameters == null)
                return getReader(query, (object)parameters, objConn);

            if (objConn.State == ConnectionState.Closed)
                throw new Exception("You forgot to open the object connection. You have to leave it open until you're done with the data reader.");

            SqlCommand cmd = new SqlCommand(query, objConn);
            int i = 1;
            foreach (object o in parameters)
            {
                cmd.Parameters.AddWithValue("@value" + i, o);
                i++;
            }

            return (SqlDataReader)sqlExecuter(cmd, SqlTypes.DataReader);
        }
        private object sqlExecuter(object o, SqlTypes type)
        {
            try
            {
                switch (type)
                {
                    case SqlTypes.DataReader:
                        return ((SqlCommand)o).ExecuteReader();
                    case SqlTypes.VoidQuery:
                        return ((SqlCommand)o).ExecuteNonQuery();
                    case SqlTypes.DataTable:
                        object[] adapterAndDataSet = (object[])o;
                        SqlDataAdapter objAdapter = (SqlDataAdapter)adapterAndDataSet[0];
                        DataSet objDataSet = (DataSet)adapterAndDataSet[1];
                        objAdapter.Fill(objDataSet);
                        return objDataSet;
                }
            }
            catch (ObjectDisposedException e)
            {
                writeStackTrace("either the object you're using is closed or something insane happened like a race condition", e);
            }
            catch (InvalidOperationException e)
            {
                writeStackTrace("some sort of simple error just occurred (data was edited between execution, something was closed that shouldn't have been, etc.", e);
            }
            catch (InvalidCastException e)
            {
                writeStackTrace("Someone may have screwed the code up. Tell someone to check the stack traces", e);
            }
            catch (SqlException e)
            {
                SqlErrorCollection errors = e.Errors;

                bool failure = false, codeError = false;
                foreach (SqlError error in errors)
                {
                    if (error.Class > 16)
                    {
                        failure = true;
                        break;
                    }
                    else if (error.Class > 10)
                    {
                        codeError = true;
                        break;
                    }
                }

                writeStackTrace("sql exception in executeVoidQuery. Error codes from Microsoft are below. Error code above 16?" + failure + "; Else, error code above 10?" + codeError + "\n" +
                                "Severity level of 10 or less: mistakes in information that a user has entered.\n" +
                                "Severity levels from 11 through 16 are generated by the user, and can be corrected by the user\n" +
                                "Severity levels from 17 through 25 indicate software or hardware errors\n" +
                                "When a level 17, 18, or 19 error occurs, you can continue working, although you might not be able to execute a particular statement.\n", e);
            }
            catch (IOException e)
            {
                writeStackTrace("io in executeVoidQuery", e);
            }
            catch (Exception e)
            {
                writeStackTrace("executeVoidSQLQuery", e);
            }
            return null;
        }
        protected void Open_Login(Object sender, EventArgs e)
        {
            if (Session["Alna_num"] != null)
            {
                Session.Clear();
                Session["loginStatus"] = "Sign in";
                redirectSafely("~/");
            }
            mpeLogin.Show();
            loadEmployees();

        }
        protected void Is_SignedIn(Object sender, EventArgs e)
        {
            if (Session["Alna_num"] == null)
            {
                mpeLogin.Show();
                loadEmployees();
                return;
            }
            else
            {

                HtmlAnchor a = (HtmlAnchor)sender;

                switch (a.InnerText)
                {
                    case "Hours":                      
                        redirectSafely("~/Hours");
                        break;
                    case "Time Off":
                        redirectSafely("~/TimeOff");
                        break;
                    case "Purchase List":
                        redirectSafely("~/List");
                        break;
                    case "Phone Checkout":
                        redirectSafely("~/PhoneCheckOut");
                        break;
                    case "Issues":
                        redirectSafely("~/IssueList");
                        break;
                }
            }
        }
        protected void Login_Clicked(Object sender, EventArgs e)
        {
            if (ddl.SelectedIndex == 0)
            {
                throwJSAlert("Select your name");
                return;
            }

            try
            {
                objConn = new SqlConnection(SuperPage.LOCAL_TO_SERVER_CONNECTION_STRING);
                objConn.Open();

                object[] o = { inputUsername.Value, inputPassword.Value };
                SqlDataReader reader = getReader("SELECT User, Admin FROM Accounts WHERE Accounts.[User]=@value1 and Accounts.[Pass]=@value2", o, objConn);
                if (reader == null)
                {
                    throwJSAlert("Error accessing data");
                    return;
                }
                if (!reader.HasRows)
                {
                    throwJSAlert("Incorrect username or password");
                    reader.Close();
                    return;
                }
                reader.Read();
                Session["Admin"] = reader.GetBoolean(1);
                reader.Close();

                reader = getReader("Select Alna_num, Name, Full_time from Employees where Employees.[Name]=@value1;", ddl.Text, objConn);
                reader.Read();
                Session["Alna_num"] = reader.GetValue(0);
                Session["Name"] = reader.GetValue(1);
                Session["Full_time"] = reader.GetValue(2);
                Session["loginStatus"] = "Signed in as " + Session["Name"] + " (Sign out)";
                redirectSafely("~/");
            }
            catch (Exception ex)
            {
                writeStackTrace("Login", ex);
            }
        }
        private void writeStackTrace(string s, Exception ex)
        {
            if (!File.Exists(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
            {
                File.Create(@"" + Server.MapPath("~/Debug/StackTrace.txt"));
            }
            using (StreamWriter file = new StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
            {
                file.WriteLine(Date.Today.ToString() + s + ex.ToString());
                file.Close();
            }
        }
        private void throwJSAlert(string s)
        {
            try
            {
                Response.Write("<script>alert('" + s + "');</script>");
            }
            catch (System.Web.HttpException h)
            {
                writeStackTrace("http exception throwingJs alert", h);
            }
        }

        private void redirectSafely(string path)
        {
            try
            {
                Server.ClearError();
                Response.Redirect(path, false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception e)
            {
                writeStackTrace("problem redirecting", e);
            }
        }
    }
}