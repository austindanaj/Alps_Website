using System;
using Date = System.DateTime;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Net.Mail;

namespace CTBTeam {
	public partial class IssueList : SuperPage {
		private SqlConnection objConn;

		protected void Page_Load(object sender, EventArgs e) {
			//TEST SCAFFOLD:
			Session["Alna_num"] = 173017;
			Session["Name"] = "Anthony Hewins";
			Session["Full_time"] = false;
			Session["loginStatus"] = "Signed in as " + Session["Name"] + " (Sign Out)";

			if (Session["Alna_num"] == null) {
				redirectSafely("~/Login");
				return;
			}

			objConn = openDBConnection();

			if (!IsPostBack) {
				if (userWantsToView()) {
					populateTable();
				}
				else {
					populateDropDowns();
				}
			}
			successDialog(successOrFail);
		}

		protected void btnSwitchView(object sender, EventArgs e) {
			if (Session["temp"] == null)
				Session["temp"] = true;
			else
				Session["temp"] = null;
			redirectSafely("~/IssueList");
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

		public string getEmail() {
			return ddlAssign.Text.ToLower().Replace(' ', '.') + "@alps.com";
		}

		private bool userWantsToView() {
			if (Session["temp"] == null) {
				pnlViewIssues.Visible = true;
				pnlReportIssue.Visible = false;
				switchView.Text = "Report Issue";
				return true;
			}
			else if (Session["temp"] is bool) {
				pnlViewIssues.Visible = false;
				pnlReportIssue.Visible = true;
				pnlAdd.Visible = true;
				pnlSelectedIssue.Visible = false;
				switchView.Text = "View Issues";
				return false;
			}
			else {
				pnlViewIssues.Visible = false;
				pnlReportIssue.Visible = true;
				pnlAdd.Visible = false;
				pnlSelectedIssue.Visible = true;
				switchView.Text = "View Issues";
				return false;
			}
		}

		private void populateTable() {
			objConn.Open();
			DataTable table = getDataTable("SELECT top 25 IssueList.ID, IssueList.Title, IssueList.Category, Projects.Name as Project, IssueList.Severity, IssueList.Due_Date as 'Due Date', IssueList.Status, IssueList.Updated, e1.Name as Reporter, e2.Name as Assignee from IssueList inner join Employees e1 on e1.Alna_num=IssueList.Reporter inner join Employees e2 on e2.Alna_num=IssueList.Assignee inner join Projects on IssueList.Proj_ID=Projects.ID where IssueList.Active=@value1;", true, objConn);
			dgvViewIssues.DataSource = table;
			dgvViewIssues.DataBind();
			objConn.Close();
		}

		private void populateDropDowns() {
			objConn.Open();
			if (pnlAdd.Visible) {
				DataTable table = getDataTable("SELECT Employees.[Name] FROM Employees WHERE Active=@value1 ORDER BY Alna_num", true, objConn);
				foreach (DataRow item in table.Rows) {
					ddlAssign.Items.Add(item[0].ToString());
				}
				DataTable projects = getDataTable("SELECT Name FROM Projects where Active=@value1;", true, objConn);
				foreach (DataRow item in projects.Rows) {
					ddlProject.Items.Add(item[0].ToString());
				}
			}
			else {
				SqlDataReader reader = getReader("Select Description, Comment, Due_Date from IssueList where ID=@value1", Session["temp"], objConn);
				reader.Read();
				txtDescription.Text = reader.GetString(0);
				object o = reader.GetValue(1);
				txtComment.Text = o.Equals(DBNull.Value) ? "" : (string)o;
				o = reader.GetValue(2);
				if(o.Equals(DBNull.Value)) {
					dueDate.Checked = true;
				} else {
					dueDate.Checked = false;
					cldDueDate.SelectedDate = (Date)o;
				}
				reader.Close();
			}
			objConn.Close();
		}

		protected void selectIssue(object sender, EventArgs e) {
			if (!int.TryParse(dgvViewIssues.SelectedRow.Cells[1].Text, out int id)) {
				throwJSAlert("The ID value was changed.");
				return;
			}

			Session["temp"] = id;
			redirectSafely("~/IssueList");
		}

		protected void submitIssue(object sender, EventArgs e) {
			objConn.Open();
			object[] o;
			if (pnlAdd.Visible) {
				SqlDataReader reader = getReader("select Alna_num from Employees where Name=@value1", ddlAssign.Text, objConn);
				reader.Read();
				int alna = reader.GetInt32(0);
				reader.Close();
				reader = getReader("select ID from Projects where Name=@value1", ddlProject.Text, objConn);
				reader.Read();
				int proj_id = reader.GetInt32(0);
				reader.Close();

				object date;
				if (dueDate.Checked)
					date = DBNull.Value;
				else
					date = cldDueDate.SelectedDate;
				o = new object[]{ txtTitle.Text, ddlCategory.SelectedValue, proj_id, ddlSeverity.SelectedValue, date, "Initial", Date.Today, Session["Alna_num"], alna, txtDescription.Text };
				executeVoidSQLQuery("insert into IssueList (Title, Category, Proj_ID, Severity, Due_Date, Status, Updated, Reporter, Assignee, Description) values" +
														  "(@value1, @value2, @value3, @value4, @value5, @value6, @value7, @value8, @value9, @value10)", o, objConn);
			} else {
				object date;
				if (dueDate.Checked)
					date = DBNull.Value;
				else
					date = cldDueDate.SelectedDate;
				o = new object[] { ddlSeverity.SelectedValue, txtDescription.Text, txtComment.Text, ddlStatus.SelectedValue, date, Session["temp"]};
				executeVoidSQLQuery("update IssueList set Severity=@value1, Description=@value2, Comment=@value3, Status=@value4, Due_date=@value5 where ID=@value6", o,objConn);
				Session["temp"] = null;
			}
			objConn.Close();
		}

		protected void cldUncheckBox(object sender, EventArgs e) { dueDate.Checked = false; }
		}
	}