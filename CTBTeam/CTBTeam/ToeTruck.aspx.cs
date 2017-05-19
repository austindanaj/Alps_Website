using System;

namespace CTBTeam {
	public partial class ToeTruck : SuperPage {
		protected void Page_Load(object sender, EventArgs e) {

		}

		protected void foundIt(object sender, EventArgs e) {
			Session["toetruck"] = true;
			redirectSafely("~/ttc823jdnajs3");
		}

		protected void falsepage(object sender, EventArgs e) {
			redirectSafely("~/");
		}
	}
}