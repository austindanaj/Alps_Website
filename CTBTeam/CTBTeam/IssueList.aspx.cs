using System;
using Date = System.DateTime;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Net.Mail;

namespace CTBTeam {
	public partial class IssueList : SuperPage {
		SqlConnection objConn;
		//	LinkedList<Button> dynamicButtonList = new LinkedList<Button>();

		private static readonly string[] CATEGORIES =
		{
			"1: Inquiry/Request",
			"2: Change Request",
			"3: Problem",
			"4: Memo"
		};

		private static readonly string[] SEVERITY =
		{
			"Minor",
			"Major"
		};

		protected void Page_Load(object sender, EventArgs e) {
			//TEST SCAFFOLD:
			Session["Alna_num"] = 1730317;
			Session["Name"] = "Anthony Hewins";
			Session["Full_time"] = false;
			Session["loginStatus"] = "Signed in as " + Session["Name"] + " (Sign Out)";

			if (Session["Alna_num"] == null) {
				redirectSafely("~/Login");
				return;
			}

			objConn = openDBConnection();
			populateTable();
			populateDropDowns();
			successDialog(successOrFail);
		}

		protected void btnIssueViewAndReport(object sender, EventArgs e) {
			if (Session["temp"] == null)
				Session["temp"] = true;
			else
				Session["temp"] = null;
		}

		public void Send_Notification(string recipient_email, string msg, string subject) {
			try {
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
			catch (Exception ex) {
				writeStackTrace("Sending Notification", ex);
			}
		}

		public string BuildEmailAddress() {
			string temp = "";
			return temp;
		}

		private void populateTable() {
			if (Session["temp"] != null) {   //If this value is not null, this person wants to view a report. Skip the SQL.
				return;
			}
			objConn.Open();
			dgvViewIssues.DataSource = getDataTable("SELECT top 25 Id, Category, Severity, Summary, Due_Date, Status, Updated FROM IssueList", null, objConn);
			dgvViewIssues.DataBind();
			objConn.Close();
		}

		private void populateDropDowns() {
			if (Session["temp"] == null) //If temp is null, this person wants to see reports. Skip the report form.
				return;
			ddlCategory.Items.Clear();
			ddlProject.Items.Clear();
			ddlSeverity.Items.Clear();
			ddlAssign.Items.Clear();
			try {
				objConn.Open();
				DataTable table = getDataTable("SELECT Employees.[Name] FROM Employees WHERE Active=@value1 ORDER BY Alna_num", true, objConn);
				DataSet objDataSet = new DataSet();
				foreach (string item in table.Rows) {
					ddlAssign.Items.Add(item);
				}
				foreach (string item in CATEGORIES) {
					ddlCategory.Items.Add(item);
				}
				foreach (string item in SEVERITY) {
					ddlSeverity.Items.Add(item);
				}
				DataTable projects = getDataTable("SELECT Name FROM Projects where Active=@value1;", true, objConn);
				foreach (string item in projects.Rows) {
					ddlProject.Items.Add(item);
				}
				txtReporter.Text = (string)Session["Name"];
				//   dgvViewIssues.DataSource = objDataSet.Tables[0].DefaultView;
				//   dgvViewIssues.DataBind();
				objConn.Close();
			}
			catch (Exception ex) {
				writeStackTrace("Populate Dropdowns", ex);
			}
		}


		protected void dgvViewIssues_OnSelectedIndexChanged(object sender, EventArgs e) {

		}

		protected void btnReportIssue_OnClick(object sender, EventArgs e) {
			try {
				objConn.Open();
				object[] o = { ddlCategory.SelectedItem, ddlSeverity.SelectedItem, txtSummary.Text, null, "0: Test", Date.Now, 1, 0 };
				executeVoidSQLQuery("INSERT INTO IssueList Category, Severity, Summary, Due_Date, Status, Updated, Reporter_Id, Assignee_Id VALUES" +
									"@value1, @value2, @value3, @value4, @value5, @value6, @value7, @value8", o, objConn);

				objConn.Close();
			}
			catch (Exception ex) {
				writeStackTrace("Sumbit Issue", ex);
			}
		}
	}
}