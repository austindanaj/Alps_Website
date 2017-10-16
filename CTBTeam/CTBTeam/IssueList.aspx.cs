using System;
using System.Data;
using Date = System.DateTime;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Web.UI.WebControls;

namespace CTBTeam {
	public partial class IssueList : SuperPage {
		//====================================================================================================
		//	To get the best view of how this class works, fold all the methods up.
		//====================================================================================================

		private SqlConnection objConn;
		private static readonly string[] STATUS = { "Initial", "Countermeasure", "Analysis", "Completed" };
		protected void Page_Load(object sender, EventArgs e) {
            /*
			if (Session["Alna_num"] == null) {
				redirectSafely("~/Login");
				return;
			}
            */

			objConn = openDBConnection();

			if (!IsPostBack) {
				//temp_row is a cookie to determine what rows you are looking at
				//It gets Rows between Session[temp_row] and (Session[temp_row] + 25)
				if (Session["temp_row"] == null)
					Session["temp_row"] = 0;

				if (userWantsToView()) {
					populateTable();
				}

				else
					populateIssuePanel();
			}
			successDialog(txtSuccessBox);
		}

		private bool userWantsToView() {
			//We use cookies to decide what the user wants to do.
			//First we check if there's an error and display that first.
			//  1. If Session["temp"] is null, we view issues
			//  2. If Session["temp"] is true, we want to report an issue
			//  3. If Session["temp"] is an int (the else statement), we want to edit the issue with that ID#
			//Then we make things invisible/visible as they need to be.

			if (Session["temp"] == null) {
				pnlViewIssues.Visible = true;
				return true;
			}
			else if (Session["temp"] is bool) {
				pnlReportIssue.Visible = true;
				pnlAddIssue.Visible = true;
				switchView.Text = "View Issues";
				return false;
			}
			else {
				pnlReportIssue.Visible = true;
				pnlEditIssue.Visible = true;
				switchView.Text = "View Issues";
				return false;
			}
		}

		//====================================================================================================
		//	Emails
		//====================================================================================================

		public void Send_Notification(string msg, string subject) {
			try {
				MailMessage mail = new MailMessage();
				SmtpClient SmtpServer = new SmtpClient("10.0.40.55");

				mail.From = new MailAddress("noreply-CTBWebsite@alps.com");
				mail.To.Add(ddlAssign.Text.ToLower().Replace(' ', '.') + "@alps.com");
				mail.Subject = subject;
				mail.Body = msg;

				//SmtpServer.Port = 587;
				//SmtpServer.UseDefaultCredentials = true;
				//SmtpServer.Credentials = new System.Net.NetworkCredential("noreply@alps.com", "alnatest");

				SmtpServer.Send(mail);
			}
			catch (Exception ex) {
				writeStackTrace("Sending Notification", ex);
			}
		}

		//====================================================================================================
		//	Init functions
		//====================================================================================================

		private void populateTable() {
			objConn.Open();
			int whatRows = (int)Session["temp_row"];
			DataTable dt = getDataTable("select * from (SELECT row_number() over(order by IssueList.ID) " +
										"as 'Row', IssueList.ID, Projects.Name as Project, IssueList.Title, c1.[Value] " +
										"as Category, s1.Value as Severity, IssueList.Due_Date as 'Due Date', s2.[Value] " +
										"as 'Status', IssueList.Updated, e1.Name as Reporter, e2.Name as Assignee from IssueList " +
										"inner join Categories c1 on c1.ID=IssueList.Category inner join Severity s1 " +
										"on s1.ID=IssueList.Severity inner join [dbo].[Status] s2 on s2.ID=IssueList.[Status] " +
										"inner join Employees e1 on e1.Alna_num = IssueList.Reporter inner join Employees e2 " +
										"on e2.Alna_num = IssueList.Assignee inner join Projects on IssueList.Proj_ID = Projects.Project_ID " +
										"where IssueList.Active = @value1) as table1 ORDER BY ID Desc;", true, objConn);

			dgvViewIssues.DataSource = dt;
			dgvViewIssues.DataBind();
			objConn.Close();
		}

		protected void color(object sender, GridViewRowEventArgs e) {
			int count = 0;
			if (e.Row.RowType == DataControlRowType.DataRow) {
				string status = e.Row.Cells[8].Text;
				foreach (TableCell cell in e.Row.Cells) {
					if (count == 0) {
						count++;
						continue;
					}
					if (status.Trim().Equals("Initial")) {
						cell.BackColor = System.Drawing.Color.Red;
					}
					else if (status.Trim().Equals("Analysis")) {
						cell.BackColor = System.Drawing.Color.MediumPurple;
					}
					else if (status.Trim().Equals("Completed")) {
						cell.BackColor = System.Drawing.Color.LimeGreen;
					}
					else {
						cell.BackColor = System.Drawing.Color.DeepSkyBlue;
					}
					count++;
				}
			}
		}

		private void populateIssuePanel() {
			//This method inits the form to submit or alter an issue.
			//Since many of the dropdowns can be reused for both forms, I reused them to minimize the HTML that gets sent
			//so this one method is a catch-all situation reporting an issue and editing it.
			//The if statement is adding an issue, the else statement is editing it
			objConn.Open();

			if (pnlAddIssue.Visible) {
				SqlDataReader reader = getReader("SELECT Alna_num, Employees.[Name] FROM Employees WHERE Active=@value1 ORDER BY Alna_num", true, objConn);
				int alna;

				ddlAssign.Items.Add("-- Please select someone to Assign --");
				ddlProject.Items.Add("-- Please select a project --");

				while (reader.Read()) {
					alna = reader.GetInt32(0);
					if (alna == (int)Session["Alna_num"])   //Shouldn't be able to assign yourself to an issue, that's nonsense
						continue;
					ddlAssign.Items.Add(reader.GetString(1));
				}
				reader.Close();
				reader = getReader("SELECT Name FROM Projects where Active=@value1;", true, objConn);
				while (reader.Read())
					ddlProject.Items.Add(reader.GetString(0));
				reader.Close();
			}
			else {
				btnSubmit.Text = "Save changes";
				ddlSeverity.Items.RemoveAt(0);  //Item[0] says "please select", but we already have a selection since we are editing the issue.
				SqlDataReader reader = getReader("Select Severity, Description, Comment, Status, Due_Date, Filename, e.Name from IssueList inner join Employees e on e.Alna_num=IssueList.Assignee where ID=@value1", Session["temp"], objConn);
				reader.Read();

				ddlSeverity.SelectedIndex = reader.GetBoolean(0) ? 1 : 0; //booleans cant be cast to ints so heres the workaround
				txtDescription.Text = reader.GetString(1);

				object o = reader.GetValue(2);
				txtComment.Text = o.Equals(DBNull.Value) ? "" : (string)o;

				int status = reader.GetInt32(3);
				ddlStatus.Items.Add(STATUS[status]);
				switch (status) {
					case 0:
						ddlStatus.Items.Add("Countermeasure");
						ddlStatus.Items.Add("Analysis");
						break;
					case 1:
						ddlStatus.Items.Add("Analysis");
						ddlStatus.Items.Add("Completed");
						break;
					case 2:
						ddlStatus.Items.Add("Initial");
						ddlStatus.Items.Add("Countermeasure");
						break;
					default:
						ddlStatus.Items.Add("Initial");
						ddlStatus.Items.Add("Countermeasure");
						ddlStatus.Items.Add("Analysis");
						break;
				}

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

				if (!reader.IsDBNull(5)) {
					string filename = reader.GetString(5);
					int length = filename.Length;
					if (length > 23) { //make sure there's not too much text that it makes the button larger
						btnDownload.Text += filename.Substring(0, 17) + "..." + filename.Substring(length - 4, length - 1);
					}
					else {
						btnDownload.Text += filename;
					}
				}
				else
					btnDownload.Visible = false;

				string currentAssignee = reader.GetString(6);
				ddlAssign.Items.Add(currentAssignee);
				reader.Close();

				reader = getReader("select Name from Employees", null, objConn);
				while (reader.Read()) {
					string temp = reader.GetString(0);
					if (temp.Equals(currentAssignee))
						continue;
					ddlAssign.Items.Add(temp);
				}
				reader.Close();

				dgvCurrentIssue.Visible = true;
				dgvCurrentIssue.DataSource = getDataTable("select i.ID, i.Category, p1.Name as Project, i.Title, s.Value as Severity, i.Updated, i.Status, e1.Name as Reporter, e2.Name as Assignee from IssueList as i inner join Employees e1 on e1.Alna_num = i.Reporter inner join Employees e2 on e2.Alna_num = i.Assignee inner join Projects p1 on p1.Project_ID=i.Proj_ID inner join Severity s on s.ID=i.Severity where i.ID=@value1;", Session["temp"], objConn);
				dgvCurrentIssue.DataBind();
			}
			objConn.Close();
		}

		//====================================================================================================
		//	HTML Events
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
			if (string.IsNullOrEmpty(txtDescription.Text)) {
				throwJSAlert("Description must not be empty");
				return;
			}

			objConn.Open();
			object[] o;
			object date;    //It has to be an object because we may have to set it to DBNull if there's no due date

			if (dueDate.Checked)
				date = DBNull.Value;
			else {
				date = cldDueDate.SelectedDate;
				if (Date.Today.CompareTo(date) > 0 & pnlAddIssue.Visible) {
					throwJSAlert("Due date needs to be in the future or today");
					return;
				}
			}

			if (pnlAddIssue.Visible) {
				if (ddlAssign.SelectedIndex == 0) {
					throwJSAlert("You need to pick someone to assign");
					return;
				}

				if (ddlProject.SelectedIndex == 0) {
					throwJSAlert("You need to select a project");
					return;
				}

				if (ddlCategory.SelectedIndex == 0) {
					throwJSAlert("You need to pick a valid category");
					return;
				}

				if (string.IsNullOrEmpty(txtTitle.Text)) {
					throwJSAlert("Need an issue title");
					return;
				}

				SqlDataReader reader = getReader("select Alna_num from Employees where Name=@value1", ddlAssign.Text, objConn);
				reader.Read();
				int alna = reader.GetInt32(0);
				reader.Close();
				reader = getReader("select Project_ID from Projects where Name=@value1", ddlProject.Text, objConn);
				reader.Read();
				int proj_id = reader.GetInt32(0);
				reader.Close();
				//  string path = null;
				object file, filename = DBNull.Value, contentType = DBNull.Value;
				if (fileUpload.HasFile) {
					using (var binReader = new BinaryReader(fileUpload.FileContent)) {
						file = binReader.ReadBytes((int)fileUpload.FileContent.Length);
					}
					filename = fileUpload.FileName;
					contentType = fileUpload.PostedFile.ContentType;
				}
				else {
					file = System.Data.SqlTypes.SqlBinary.Null; //There are two kinds of nulls, binary and not binary for some stupid reason
				}
				o = new object[] { txtTitle.Text, ddlCategory.SelectedIndex, proj_id, ddlSeverity.SelectedIndex, date, 0, DateTime.Now, Session["Alna_num"], alna, txtDescription.Text, file, filename, contentType };
				executeVoidSQLQuery("insert into IssueList (Title, Category, Proj_ID, Severity, Due_Date, Status, Updated, Reporter, Assignee, Description, Attachment, Filename, Content_type) values" +
														  "(@value1, @value2, @value3, @value4, @value5, @value6, @value7, @value8, @value9, @value10, @value11, @value12, @value13)", o, objConn);
				Send_Notification(txtTitle.Text + "\n\n" + txtDescription.Text, "CTBWebsite - New issue from " + (string) Session["Name"]);
			}
			else {
				int statusIndex = 0;
				for (int i = 0; i < STATUS.Length; i++)
					if (STATUS[i].Equals(ddlStatus.SelectedValue)) {
						statusIndex = i;
						break;
					}
				o = new object[] { ddlSeverity.SelectedIndex, txtDescription.Text, txtComment.Text, statusIndex, date, DateTime.Now, Session["temp"] };
				executeVoidSQLQuery("update IssueList set Severity=@value1, Description=@value2, Comment=@value3, Status=@value4, Due_date=@value5, Updated=@value6 where ID=@value7", o, objConn);
				Session["temp"] = null;
			}
			objConn.Close();
			Session["success?"] = true;
			Send_Notification("Title:\n" + dgvCurrentIssue.Rows[0].Cells[3].Text + "\n\nDescription:\n" + txtDescription.Text + "\n\nComment:\n" + txtComment.Text, "CTBWebsite - " + (string) Session["Name"] + " edited an issue you're tagged in");
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

		protected void nextIssuePage(object sender, GridViewPageEventArgs e) {
			dgvViewIssues.PageIndex = e.NewPageIndex;
			populateTable();
		}

		protected void btnDownload_OnClick(object sender, EventArgs e) {
			objConn.Open();
			SqlDataReader reader = getReader("select Filename, Attachment, Content_type from IssueList where ID=@value1", int.Parse(dgvCurrentIssue.Rows[0].Cells[0].Text), objConn);
			if (reader == null) {
				throwJSAlert("There is no file associated with this issue.");
				objConn.Close();
				return;
			}

			reader.Read();
			string filename = reader.GetString(0);
			string extension = filename == null ? null : filename.Substring(filename.LastIndexOf('.'));

			if (filename == null) {
				throwJSAlert("There is no file associated with this issue.");
				objConn.Close();
				reader.Close();
				return;
			}

			byte[] blob = (byte[])reader["Attachment"];
			string contentType = reader.GetString(2);
			reader.Close();
			objConn.Close();

			Response.Clear();
			Response.Buffer = true;
			Response.Charset = "";
			Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
			Response.ContentType = contentType;
			Response.AddHeader("content-disposition", $"attachment; filename=\"{filename}\"");
			Response.BinaryWrite(blob);
			Response.Flush();
			Response.End();
		}
	}
}