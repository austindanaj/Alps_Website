using System;
using System.Data.SqlClient;

namespace CTBTeam {
	public class Seed : SuperPage {
		private object[] o;

		public bool seed() {
			/*
			 * ===========================================================
			 * THIS CLASS SHOULD ONLY BE USED ONCE. ONLY ONE TIME, THEN 
			 * IMMEDIATELY HIDDEN. IT CAN BE BROUGHT BACK ONLY, ONLY IF
			 * THE DB IS DESTROYED FOR SOME REASON.
			 * YOU WILL SCREW UP THE ENTIRE DB OTHERWISE
			 * ALSO DONT SCREW UP THE ORDERING OF ANY ARRAY
			 * ===========================================================
			 */
			try {
				SqlConnection conn = openDBConnection();
				conn.Open();
				//seedEmployees(conn);
				//seedProjects(conn);
				seedPhones(conn);
				//seedVehicles(conn);
				conn.Close();
				return true;
			}
			catch (Exception e) {
				writeStackTrace("Trouble seeding:", e);
				return false;
			}
		}

		private void seedEmployees(SqlConnection conn) {

			//We use the Alna_num as a primary key. Since it's a primary key, in order to seed the DB,
			//we gotta say let us tell you the PK. Then we can turn it back off and autoincrement if we want.
			executeVoidSQLQuery("set identity_insert Alps.dbo.Employees ON;", null, conn);

			o = new object[3];
			int i = 170000;
			int[] partTimeAlna = { 173017, 173018, 172906, 172991, 172990, 172923, 173036, 173037 };
			int[] fullTimeAlna = { 172872, 172947, 172363, 172945, 172981, 172148, 172336, 172909, 172787, 172813, 172915, 172889, 172281 };
			string[] partTimeEmployees = { "Anthony Hewins", "Zarif Ghazi", "Austin Danaj", "Seth Logan", "Francesco Parrinelio", "Levi Hellebuyck", "Fabrisio Ballo", "Chad Miller" };
			string[] fullTimeEmployees = { "Daniel Vega", "Damien Galloway", "Carlos Velasquez", "Cruz España", "Hugo Moran", "James Dulgerian", "John Cabigao", "Joseph Kielasa", "Kevin Fang", "Leonel Aguilera", "Osamu Inoue", "Natha Vargo", "Xeng Moua" };

			stupidCheck(partTimeAlna.Length, partTimeEmployees);
			stupidCheck(partTimeAlna.Length, partTimeEmployees);

			foreach (string s in partTimeEmployees) {
				i++;
				o[0] = i;
				o[1] = s;
				o[2] = 0;
				executeVoidSQLQuery("INSERT into Employees (Alna_num, Name, Full_time) VALUES (@value1, @value2, @value3);", o, conn);
			}
			foreach (string s in fullTimeEmployees) {
				i++;
				o[0] = i;
				o[1] = s;
				o[2] = 1;
				executeVoidSQLQuery("INSERT into Employees (Alna_num, Name, Full_time) VALUES (@value1, @value2, @value3);", o, conn);
			}
			executeVoidSQLQuery("set identity_insert Alps.dbo.Employees OFF;", null, conn);
		}

		private void seedProjects(SqlConnection conn) {
			o = new object[2];
			string[] projects = { "BLE_PEPS_High_Accuracy", "BLE_TPMS", "Technology_Research", "General_CTB", "POC", "BLE_Key_Pass_Global_A", "BLE_Key_Pass_Global_A_Testing", "BLE_Key_Pass_Global_B", "BLE_Key_Pass_Autonomus", "USRR_Track_3", "M2R", "MMR", "USRR_2", "Smart_Thermostat", "IR_Transmitter", "Multipurpose_Sensor", "Vacations" };
			string[] categories = { "A", "A", "A", "A", "A", "C", "C", "C", "C", "C", "A", "A", "A", "D", "D", "A", "TimeOff" };

			stupidCheck(projects, categories);

			for (int i = 0; i < projects.Length; i++) {
				o[0] = projects[i];
				o[1] = categories[i];
				executeVoidSQLQuery("INSERT into Projects (Projects.[Name], Category) values (@value1, @value2);", o, conn);
			}
		}

		private void seedVehicles(SqlConnection conn) {
			string[] vehicles = { "M2JC_SPARK", "M2JC_SPARK_EV", "D2JCI_VOLT", "G1UC_TRAX", "K2UC_TAHOE", "G2KCA_BOLT_AV", "D2LC_CRUZE", "D2UC_EQUINOX", "A2LL_CTS" };
			foreach (string s in vehicles)
				executeVoidSQLQuery("INSERT into Vehicles (Vehicles.[Name]) values (@value1);", s, conn);
		}

		private void seedPhones(SqlConnection conn) {
			string[] phones = { "iPhone 6-1", "iPhone 6P-1", "iPhone 5S-1", "iPhone 6S-1", "iPhone 7-1", "iPhone6S-2", "Galaxy S6-1", "Galaxy S5-4", "Galaxy S5-3", "Galaxy S6-2", "Galaxy S6-3", "Galaxy S5-1", "Galaxy S7 Edge-1", "Galaxy S7 Edge-2", "Galaxy S7 Edge-3", "Galaxy S7-1", "Galaxy S7-2", "Tablet-1", "Tablet-2", "Big Nexus", "Lil' Nexus" };
			o = new object[3];
			o[1] = DBNull.Value;
			o[2] = DBNull.Value;
			foreach (string s in phones) {
				o[0] = s;
				executeVoidSQLQuery("INSERT into PhoneCheckout (PhoneCheckout.[Name], Vehicle_ID, Alna_num) values (@value1, @value2, @value3);", o, conn);
			}
		}

		private void seedAccounts(SqlConnection conn) {
			//Needs to be changed
			string[] accounts = { "User", "Admin" };
			string[] passwords = { "alna", "alnatest" };
			bool[] isAdmin = { false, true };
			stupidCheck(accounts, passwords);
			for (int i= 0;i < accounts.Length;i++) {
				o[i] = accounts[i];
				o[i] = passwords[i];
				o[i] = isAdmin[i];
				executeVoidSQLQuery("insert into Accounts (User, Pass, Admin) values (@value1, @value2, @value3);", o, conn);
			}
		}

		private void stupidCheck(object[] array1, object[] array2) {
			int n = array1.Length;
			if (n != array2.Length) throw new ArgumentException("Inequal lengths, your seeds will not work!");
		}

		private void stupidCheck(int n, object[] array2) {
			if (n != array2.Length) throw new ArgumentException("Inequal lengths, your seeds will not work!");
		}
	}
}