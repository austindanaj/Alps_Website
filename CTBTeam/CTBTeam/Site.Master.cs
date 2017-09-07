using System;
using System.Web.UI;

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

			if (Session["Full_time"] != null)
				if (!(bool)Session["Full_time"])
					lstSchedule.Visible = true;
		}


	}
}