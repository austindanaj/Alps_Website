using System;
using System.Data.SqlClient;

namespace CTBTeam {
	public class Seed : SuperPage {
		private object[] o;

		public bool seed() {
			try {
				SqlConnection conn = openDBConnection();
				seedEmployees(conn);
				seedProjects(conn);
				seedPhones(conn);
				seedVehicles(conn);
				return true;
			}
			catch (Exception e) {
				writeStackTrace("Trouble seeding:", e);
				return false;
			}
		}

		private void seedEmployees(SqlConnection conn) {
			o = new object[2];
			string[] partTimeEmployees = { "Anthony Hewins", "Zarif Ghazi", "Austin Danaj", "Seth Logan", "Francesco Parrinelio", "Levi Hellebuyck", "Fabrisio Ballo" };
			string[] fullTimeEmployees = { "Daniel Vega", "Damien Galloway", "Carlos Velasquez", "Cruz España", "Hugo Moran", "James Dulgerian", "John Cabigao", "Joseph Kielasa", "Kevin Fang", "Leonel Aguilera", "Osamu Inoue", "Natha Vargo", "Xeng Moua" };
			foreach (string s in partTimeEmployees) {
				o[0] = s;
				o[1] = 0;
				executeVoidSQLQuery("INSERT into Employees (@value1, @value2);", o, conn);
			}
			foreach (string s in fullTimeEmployees) {
				o[0] = s;
				o[1] = 1;
				executeVoidSQLQuery("INSERT into Employees (@value1, @value2);", o, conn);
			}
		}

		private void seedProjects(SqlConnection conn) {
			o = new object[2];
			string[] projects = { "BLE_PEPS_High_Accuracy", "BLE_TPMS", "Technology_Research", "General_CTB", "POC", "BLE_Key_Pass_Global_A", "BLE_Key_Pass_Global_A_Testing", "BLE_Key_Pass_Global_B", "BLE_Key_Pass_Autonomus", "USRR_Track_3", "M2R", "MMR", "USRR_2", "Smart_Thermostat", "IR_Transmitter", "Multipurpose_Sensor", "Vacations" };
			string[] categories = { "A", "A", "A", "A", "A", "C", "C", "C", "C", "C", "A", "A", "A", "D", "D", "A", "TimeOff" };

			int n = projects.Length;
			if (n != categories.Length) throw new ArgumentException("categories length doesn't match the projects length");

			for (int i = 0; i < projects.Length; i++) {
				o[0] = projects[i];
				o[1] = categories[i];
				executeVoidSQLQuery("INSERT into Projects (@value1, @value2);", o, conn);
			}
		}

		private void seedVehicles(SqlConnection conn) {
			string[] vehicles = { "M2JC_SPARK", "M2JC_SPARK_EV", "D2JCI_VOLT", "G1UC_TRAX", "K2UC_TAHOE", "G2KCA_BOLT_AV", "D2LC_CRUZE", "D2UC_EQUINOX", "A2LL_CTS" };
			foreach (string s in vehicles)
				executeVoidSQLQuery("INSERT into Vehicles (@value1);", s, conn);
		}

		private void seedPhones(SqlConnection conn) {
			string[] phones = { "iPhone 6-1", "iPhone 6P-1", "iPhone 5S-1", "iPhone 6S-1", "iPhone 7-1", "iPhone6S-2", "Galaxy S6-1", "Galaxy S5-4", "Galaxy S5-3", "Galaxy S6-2", "Galaxy S6-3", "Galaxy S5-1", "Galaxy S7 Edge-1", "Galaxy S7 Edge-2", "Galaxy S7 Edge-3", "Galaxy S7-1", "Galaxy S7-2", "Tablet-1", "Tablet-2", "Big Nexus", "Lil' Nexus" };
			foreach (string s in phones)
				executeVoidSQLQuery("INSERT into PhoneCheckout (@value1);", s, conn);
		}
	}
}