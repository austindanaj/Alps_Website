using System;
using Date = System.DateTime;
using System.Data.SqlClient;
using System.Net.Mail;

namespace CTBTeam {
	public partial class IssueList : SuperPage {
		//====================================================================================================
		//	To get the best view of how this class works, fold all the methods up.
		//====================================================================================================

		private SqlConnection objConn;
		protected void Page_Load(object sender, EventArgs e) {
			if (Session["Alna_num"] == null) {
				redirectSafely("~/Login");
				return;
			}

			objConn = openDBConnection();

			if (!IsPostBack) {
				//temp_row is a cookie to determine what rows you are looking at
				//It gets Rows between Session[temp_row] and (Session[temp_row] + 25)
				if (Session["temp_row"] == null)
					Session["temp_row"] = 0;

				if (userWantsToView())
					populateTable();
				else
					populateIssuePanel();
			}
			successDialog(successOrFail);
		}

		private bool userWantsToView() {
			//We use cookies to decide what the user wants to do.
			//  1. If Session["temp"] is null, we view issues
			//  2. If Session["temp"] is true, we want to report an issue
			//  3. If Session["temp"] is an int (the else statement), we want to edit the issue with that ID#
			//Then we make things invisible/visible as they need to be.
			if(null != Session["error"]) {
				txtFail.Visible = true;
				Session["error"] = null;
			}

			if (Session["temp"] == null) {
				pnlViewIssues.Visible = true;
				return true;
			}
			else if (Session["temp"] is bool) {
				pnlReportIssue.Visible = true;
				pnlAdd.Visible = true;
				switchView.Text = "View Issues";
				return false;
			}
			else {
				pnlReportIssue.Visible = true;
				pnlSelectedIssue.Visible = true;
				switchView.Text = "View Issues";
				return false;
			}
		}

		//====================================================================================================
		//	Emails
		//====================================================================================================

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

		public string getEmail() { return ddlAssign.Text.ToLower().Replace(' ', '.') + "@alps.com"; }

		//====================================================================================================
		//	Init functions
		//====================================================================================================

		private void populateTable() {
			objConn.Open();
			int whatRows = (int)Session["temp_row"];
			dgvViewIssues.DataSource = getDataTable("select * from (SELECT row_number() over(order by IssueList.ID) as 'Row', IssueList.ID, Projects.Name as Project, IssueList.Title, c1.[Value] as Category, s1.Value as Severity, IssueList.Due_Date as 'Due Date', s2.[Value] as 'Status', IssueList.Updated, e1.Name as Reporter, e2.Name as Assignee from IssueList inner join Categories c1 on c1.ID=IssueList.Category inner join Severity s1 on s1.ID=IssueList.Severity inner join [dbo].[Status] s2 on s2.ID=IssueList.[Status] inner join Employees e1 on e1.Alna_num = IssueList.Reporter inner join Employees e2 on e2.Alna_num = IssueList.Assignee inner join Projects on IssueList.Proj_ID = Projects.ID where IssueList.Active = 1) as table1 where table1.Row between " + (whatRows + 1) + " and " + (whatRows + 25) + ";", true, objConn);
			dgvViewIssues.DataBind();
			objConn.Close();
		}

		private void populateIssuePanel() {
			//This method inits the form to submit or alter an issue.
			//Since many of the dropdowns can be reused for both forms, I reused them to minimize the HTML that gets sent
			//so this one method is a catch-all situation reporting an issue and editing it.
			//The if statement is adding an issue, the else statement is editing it
			objConn.Open();

			if (pnlAdd.Visible) {
				SqlDataReader reader = getReader("SELECT Alna_num, Employees.[Name] FROM Employees WHERE Active=@value1 ORDER BY Alna_num", true, objConn);
				int alna;
				string temp;
				while (reader.Read()) {
					alna = reader.GetInt32(0);
					if (alna == (int)Session["Alna_num"])   //Shouldn't be able to assign yourself to an issue, that's nonsense
						continue;
					temp = reader.GetString(1);
					ddlAssign.Items.Add(temp);
				}
				reader.Close();
				reader = getReader("SELECT Name FROM Projects where Active=@value1;", true, objConn);
				while (reader.Read())
					ddlProject.Items.Add(reader.GetString(0));
				reader.Close();
			}
			else {
				SqlDataReader reader = getReader("Select Severity, Description, Comment, Status, Due_Date from IssueList where ID=@value1", Session["temp"], objConn);
				reader.Read();

				ddlSeverity.SelectedIndex = reader.GetBoolean(0) ? 1 : 0;
				txtDescription.Text = reader.GetString(1);

				object o = reader.GetValue(2);
				txtComment.Text = o.Equals(DBNull.Value) ? "" : (string)o;

				ddlStatus.SelectedIndex = reader.GetInt32(3);
				
				//We get the due date and check if it's null because there doesn't always
				//have to be one. If it's null we check the box "No due date", else we select
				//the due date specified to the correct calendar date.
				o = reader.GetValue(4);
				if (o.Equals(DBNull.Value)) {
					dueDate.Checked = true;
				}
				else {
					dueDate.Checked = false;
					cldDueDate.SelectedDate = (Date)o;
				}
				reader.Close();

				dgvCurrentIssue.Visible = true;
				dgvCurrentIssue.DataSource = getDataTable("select i.ID, i.Category, p1.Name as Project, i.Title, i.Updated, e1.Name as Reporter, e2.Name as Assignee from IssueList as i inner join Employees e1 on e1.Alna_num = i.Reporter inner join Employees e2 on e2.Alna_num = i.Assignee inner join Projects p1 on p1.ID=i.Proj_ID where i.ID=@value1;", Session["temp"], objConn);
				dgvCurrentIssue.DataBind();
			}
			objConn.Close();
		}

		//====================================================================================================
		//	HTML Evenets
		//====================================================================================================

		protected void selectIssue(object sender, EventArgs e) {
			if (!int.TryParse(dgvViewIssues.SelectedRow.Cells[2].Text, out int id)) {
				throwJSAlert("The ID value was changed.");
				return;
			}

			Session["temp"] = id;
			redirectSafely("~/IssueList");
		}

		protected void submitIssue(object sender, EventArgs e) {
			//This method submits or alters an issue.
			//Since many of the dropdowns can be reused for both forms, I reused them to minimize the HTML that gets sent
			//so this one method is a catch-all situation reporting an issue and editing it.
			//The if statement is adding an issue, the else statement is editing it
			objConn.Open();
			object[] o;
			object date;	//It has to be an object because we may have to set it to DBNull if there's no due date

			if (dueDate.Checked)
				date = DBNull.Value;
			else {
				date = cldDueDate.SelectedDate;
				if (Date.Today.CompareTo(date) > 0 & pnlAdd.Visible) {
					Session["error"] = true;
					redirectSafely("~/IssueList");
					return;
				}
			}

			if (pnlAdd.Visible) {
				SqlDataReader reader = getReader("select Alna_num from Employees where Name=@value1", ddlAssign.Text, objConn);
				reader.Read();
				int alna = reader.GetInt32(0);
				reader.Close();
				reader = getReader("select ID from Projects where Name=@value1", ddlProject.Text, objConn);
				reader.Read();
				int proj_id = reader.GetInt32(0);
				reader.Close();				

				o = new object[] { txtTitle.Text, ddlCategory.SelectedIndex, proj_id, ddlSeverity.SelectedIndex, date, 0, Date.Today, Session["Alna_num"], alna, txtDescription.Text };
				executeVoidSQLQuery("insert into IssueList (Title, Category, Proj_ID, Severity, Due_Date, Status, Updated, Reporter, Assignee, Description) values" +
														  "(@value1, @value2, @value3, @value4, @value5, @value6, @value7, @value8, @value9, @value10)", o, objConn);
				Send_Notification(getEmail(), txtTitle + "\n\n" + txtDescription, "CTBWebsite - New issue");
			}
			else {
				o = new object[] { ddlSeverity.SelectedIndex, txtDescription.Text, txtComment.Text, ddlStatus.SelectedIndex, date, Session["temp"] };
				executeVoidSQLQuery("update IssueList set Severity=@value1, Description=@value2, Comment=@value3, Status=@value4, Due_date=@value5 where ID=@value6", o, objConn);
				Session["temp"] = null;
			}
			objConn.Close();
			Session["success?"] = true;
			redirectSafely("~/IssueList");
		}

		protected void cldUncheckBox(object sender, EventArgs e) { dueDate.Checked = false; }

		protected void btnSwitchView(object sender, EventArgs e) {
			if (Session["temp"] == null)
				Session["temp"] = true;
			else
				Session["temp"] = null;
			redirectSafely("~/IssueList");
		}

		protected void viewOtherRows(object sender, EventArgs e) {
			int addition;
			if (sender.Equals(btnForward)) {
				addition = (int)Session["temp_row"] + 25;
				objConn.Open();
				SqlDataReader reader = getReader("select count(ID) from IssueList;", null, objConn);
				reader.Read();
				int count = reader.GetInt32(0) - 25;
				reader.Close();
				objConn.Close();
				count = count < 0 ? 0 : count;

				Session["temp_row"] = addition < count ? addition : count;
			}
			else {
				addition = (int)Session["temp_row"] - 25;
				Session["temp_row"] = addition < 0 ? 0 : addition;
			}
			redirectSafely("~/IssueList");
		}
	}
}