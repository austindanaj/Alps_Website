using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace CTBTeam {
	public partial class SiteMaster : MasterPage {
		protected void Page_Load(object sender, EventArgs e) {
			if (Session["loginStatus"] == null)
				Session["loginStatus"] = "Sign In";

			if (Session["Alna_num"] == null)
				Session["admin"] = false;

			if (Session["admin"] != null)
				if ((bool)Session["admin"])
					admin.Visible = true;
		}
	}
}