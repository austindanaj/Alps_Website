using System;
using Date = System.DateTime;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Net.Mail;

namespace CTBTeam {
    public partial class IssueList : SuperPage
    {
        SqlConnection objConn;
        //	LinkedList<Button> dynamicButtonList = new LinkedList<Button>();

        public static string[] CATEGORIES =
        {
            "1: Inquiry/Request",
            "2: Change Request",
            "3: Problem",
            "4: Memo"
        };

        public static string[] SEVERITY =
        {
            "Minor",
            "Major"
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Alna_num"] == null)
            {
                redirectSafely("~/Login");
                return;
            }

            objConn = openDBConnection();
            if (!IsPostBack)
            {
                populateTable();
                populateDropDowns();
                successDialog(successOrFail);

            }
        }

        protected void btnReportIssue_Click(object sender, EventArgs e)
        {
            pnlViewIssues.Visible = false;
            pnlReportIssue.Visible = true;
        }

        protected void btnViewIssue_Click(object sender, EventArgs e)
        {
            pnlReportIssue.Visible = false;
            pnlViewIssues.Visible = true;
        }

        public void Send_Notification(string recipient_email, string msg, string subject)
        {


            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("alnaandroidtest@gmail.com");
                mail.To.Add(recipient_email);
                mail.Subject = subject;
                mail.Body = msg;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("alnaandroidtest@gmail.com", "alnatest");

                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {
                writeStackTrace("Sending Notification", ex);
            }
        }

        public string BuildEmailAddress(string name)
        {
            string temp = "";

            return temp;
        }

        private void populateTable()
        {
            try
            {
                objConn.Open();
                SqlDataAdapter objAdapter = new SqlDataAdapter();
                objAdapter.SelectCommand =
                    new SqlCommand("SELECT Id, Category, Severity, Summary, Due_Date, Status, Updated FROM IssueList",
                        objConn);
                DataSet objDataSet = new DataSet();
                objAdapter.Fill(objDataSet);
                dgvViewIssues.DataSource = objDataSet.Tables[0].DefaultView;
                dgvViewIssues.DataBind();
                objConn.Close();
            }
            catch (Exception ex)
            {
                writeStackTrace("Populate List", ex);
            }
        }

        private void populateDropDowns()
        {
            ddlCategory.Items.Clear();
            ddlProject.Items.Clear();
            ddlSeverity.Items.Clear();
            ddlAssign.Items.Clear();
            try
            {
                objConn.Open();
                SqlDataAdapter objAdapter = new SqlDataAdapter();
                objAdapter.SelectCommand =
                    new SqlCommand("SELECT Employees.[Name] FROM Employees WHERE Active=1 ORDER BY Alna_num", objConn);
                DataSet objDataSet = new DataSet();
                objAdapter.Fill(objDataSet);
                foreach (string item in objDataSet.Tables[0].Rows)
                {
                    ddlAssign.Items.Add(item);
                }
                foreach (string item in CATEGORIES)
                {
                    ddlCategory.Items.Add(item);
                }
                foreach (string item in SEVERITY)
                {
                    ddlSeverity.Items.Add(item);
                }
                objAdapter.SelectCommand = new SqlCommand("SELECT Name FROM Projects where Active=1;", objConn);
                objDataSet = new DataSet();
                objAdapter.Fill(objDataSet);
                foreach (string item in objDataSet.Tables[0].Rows)
                {
                    ddlProject.Items.Add(item);
                }
                txtReporter.Text = (string) Session["Name"];
                //   dgvViewIssues.DataSource = objDataSet.Tables[0].DefaultView;
                //   dgvViewIssues.DataBind();
                objConn.Close();
            }
            catch (Exception ex)
            {
                writeStackTrace("Populate Dropdowns", ex);
            }
        }


        protected void dgvViewIssues_OnSelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnReportIssue_OnClick(object sender, EventArgs e)
        {
            try
            {
                objConn.Open();
                SqlDataAdapter objAdapter = new SqlDataAdapter();
              /*  objAdapter.SelectCommand = new SqlCommand(
                    "INSERT INTO IssueList Category, Severity, Summary, Due_Date, Status, Updated, Reporter_Id, Assignee_Id VALUES" +
                    "@value1, @value2, @value3, @value4, @value5, @value6, @value7, @value8", objConn);*/
                object[] o = {ddlCategory.SelectedItem, ddlSeverity.SelectedItem, txtSummary.Text, null, "0: Test", Date.Now, 1, 0 };
                executeVoidSQLQuery("INSERT INTO IssueList Category, Severity, Summary, Due_Date, Status, Updated, Reporter_Id, Assignee_Id VALUES" +
                                    "@value1, @value2, @value3, @value4, @value5, @value6, @value7, @value8", o, objConn);

                objConn.Close();
            }
            catch (Exception ex)
            {
                writeStackTrace("Sumbit Issue", ex);
            }
        }
    }
}