using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace CTBTeam
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["loginStatus"] == null)
                Session["loginStatus"] = "Sign In";

			if (Session["User"] == null)            
				Session["admin"] = false;

			if (!(bool)Session["admin"])
                admin.Visible = false;

			if (string.IsNullOrEmpty((string)Session["Date"]))
            {
                DateTime dt = DateTime.Now;                
                while (dt.DayOfWeek != DayOfWeek.Monday) dt = dt.AddDays(-1);
                Session["Date"] = dt.ToShortDateString().Replace('/', '_');
            }
        }
    }
}