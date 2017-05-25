using System;
using System.Data.SqlClient;
using System.Data;
using Date = System.DateTime;

namespace CTBTeam {
	public partial class _Default : SuperPage {
		SqlConnection objConn;

		protected void Page_Load(object sender, EventArgs e) {
			if (Session["loginStatus"] == null) {
				Session["loginStatus"] = "Sign In";
			}
		}
		protected void View_More_onClick(object sender, EventArgs e) {
			redirectSafely("Hours.aspx");
		}
		/*
		protected void download_database(object sender, EventArgs e) {
			objConn = openDBConnection();
			
			try {
				Response.ContentType = "Application/txt";
				Response.AppendHeader("Content-Disposition", "attachment; filename=Current-Hours_" + fileName + ".txt");
				Response.TransmitFile(Server.MapPath("~/Logs/CurrentHours/Current-Hours_" + fileName + ".txt"));
				Response.End();
			}
			catch (Exception ex) {
				Log.getInstance.WriteToLog("Download Database", ex, Server);
			}
		}

		protected void download_timelog(object sender, EventArgs e) {
			Response.ContentType = "Application/txt";
			Response.AppendHeader("Content-Disposition", "attachment; filename=Time-log.txt");
			Response.TransmitFile(Server.MapPath("~/Logs/TimeLog/Time-log.txt"));
			Response.End();
		}
		*/
		protected void download_file_hexGenerator(object sender, EventArgs e) {
			Response.ContentType = "Application/exe";
			Response.AppendHeader("Content-Disposition", "attachment; filename=HexGenerator.exe");
			Response.TransmitFile(Server.MapPath("~/HexGenerator.exe"));
			Response.End();
		}

		protected void download_Phones_file(object sender, EventArgs e) {
			Response.ContentType = "Application/txt";
			Response.AppendHeader("Content-Disposition", "attachment; filename=Phone-report.txt");
			Response.TransmitFile(Server.MapPath("~/Logs/PhoneLog/Phone-report.txt"));
			Response.End();
		}
	}
}