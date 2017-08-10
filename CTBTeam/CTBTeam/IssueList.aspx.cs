using System;
using Date = System.DateTime;
using System.Data.OleDb;
using System.Data;
using System.Net.Mail;

namespace CTBTeam {
	public partial class IssueList : SuperPage {

		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				
			}
		}

	    protected void btnSendEmail_Click(object sender, EventArgs e)
	    {
	        try
	        {
	            MailMessage mail = new MailMessage();
	            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
	            mail.From = new MailAddress("alnaandroidtest@gmail.com");
	            mail.Subject = "--Please No Reply--";
	            mail.Body = "This is a test email!";

	            SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("alnaandroidtest", "alnatest");
	            SmtpServer.EnableSsl = true;

	            SmtpServer.Send(mail);
	            successDialog(successOrFail);
            }
	        catch (Exception ex)
	        {
	            
	        }
	    }
	}
}