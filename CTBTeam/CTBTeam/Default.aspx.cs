using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CTBTeam
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)Session["loginStatus"]))
            {
                Session["loginStatus"] = "Sign In";
            }
        }
        protected void View_More_onClick(object sender, EventArgs e)
        {

            Response.Redirect("Hours.aspx");

        }
        protected void download_file_crc(object sender, EventArgs e)
        {
            Response.ContentType = "Application/exe";
            Response.AppendHeader("Content-Disposition", "attachment; filename=CRC.exe");
            Response.TransmitFile(Server.MapPath("~/CRC.exe"));
            Response.End();
        }
        protected void download_file_reset(object sender, EventArgs e)
        {
            Response.ContentType = "Application/exe";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Reset.exe");
            Response.TransmitFile(Server.MapPath("~/Reset.exe"));
            Response.End();
        }
        protected void download_file_hexGenerator(object sender, EventArgs e)
        {
            Response.ContentType = "Application/exe";
            Response.AppendHeader("Content-Disposition", "attachment; filename=HexGenerator.exe");
            Response.TransmitFile(Server.MapPath("~/HexGenerator.exe"));
            Response.End();
        }
    }
}