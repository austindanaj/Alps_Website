using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

namespace HoursControl {
	[Serializable]
	public class Employee {
		public string fname { get; set; }
		public string lname { get; set; }
		public ArrayList hours;

		public Employee(string f, string l, ArrayList list) {
			this.fname = f;
			this.lname = l;
			this.hours = list;
		}

	}

	[Serializable]
	public class HoursFile {
		public ArrayList employees { get; }
		public ArrayList columns { get; }

		//Use this one to use today's date
		public HoursFile() {
			this.employees = new ArrayList();
			this.columns = new ArrayList();
		}

		//Use this one to push new hours file on the stack
		public HoursFile(HoursFile h) {
			this.employees = new ArrayList();
			this.columns = new ArrayList();
			foreach (Employee e in h.employees) {
				//I forgot how to deep copy, so I brute forced. Plus, there's no clone method either
				this.employees.Add(new Employee(e.fname, e.lname, e.hours));
			}
			foreach (string s in h.columns) {
				this.columns.Add(s);
			}
		}

		public void addColumn(string s) {
			this.columns.Add(s);
			foreach (Employee e in this.employees)
				e.hours.Add(0);
		}

		public void addEmployee(string fname, string lname) {
			ArrayList l = new ArrayList();
			for (int i = 0; i < this.columns.Count; i++)
				l.Add(0);
			this.employees.Add(new Employee(fname, lname, l));
		}

		//Removes an employee with any params you specify
		//Leaving fname null: treats fname as wildcard
		//Leaving lname null: treats lname as wildcard
		//Returns true if successful
		//Returns false if not successful
		public bool removeEmployee(string fname, string lname) {
			int x = -1;
			if (fname != null && lname != null) {
				foreach (Employee e in this.employees) {
					if (e.fname == fname && e.lname == lname) {
						x = this.employees.IndexOf(e);
						break;
					}
				}
			}
			else if (fname != null) {
				foreach (Employee e in this.employees) {
					if (e.fname == fname) {
						x = this.employees.IndexOf(e);
						break;
					}
				}
			}
			else if (lname != null) {
				foreach (Employee e in this.employees) {
					if (e.lname == lname) {
						x = this.employees.IndexOf(e);
						break;
					}
				}
			}
			if (x != -1) {
				this.employees.RemoveAt(x);
				return true;
			}
			return false;
		}

		public bool removeColumn(string col) {
			int x = -1;
			foreach (string s in this.columns) {
				if (s == col)
					x = this.columns.IndexOf(s);
			}
			if (x == -1)
				return false;
			this.columns.RemoveAt(x);
			return true;
		}

		private bool changeTheHours(string f, string l, string col, int n, bool appendornah) {
			foreach (Employee e in this.employees)
				if (e.fname == f && e.lname == l) {
					int x = this.columns.IndexOf(col);
					if (x != -1) {
						if (appendornah)
							e.hours[x] = n + (int)e.hours[x];
						else
							e.hours[x] = n;
						return true;
					}
				}
			return false;
		}

		public bool addHours(string f, string l, string col, int n) {
			return this.changeTheHours(f, l, col, n, true);
		}

		public bool changeHours(string f, string l, string col, int n) {
			return this.changeTheHours(f, l, col, n, false);
		}

		public string print() {
			string s = "Employee,";
			foreach (string col in this.columns) {
				s += col + ",";
			}
			s += "\n";
			foreach (Employee e in this.employees) {
				s += e.fname + " " + e.lname + ",";
				foreach (int j in e.hours)
					s += j + ",";
				s += "\n";
			}
			return s;
		}
	}

	[Serializable]
	public class HoursManagement {
		public HoursFile projectHours;
		public HoursFile vehicleHours;
		public DateTime date;
		public HoursManagement previous = null;
		public readonly static string PATH = @"C:\Users\alna173017\Desktop\hours.daddy"; //@"~/hours.daddy";

		public HoursManagement() {
			this.projectHours = new HoursFile();
			this.vehicleHours = new HoursFile();
			this.date = DateTime.Today;
		}

		public HoursManagement(HoursManagement h) {
			this.projectHours = new HoursFile(h.projectHours); //Deep copies the object
			this.vehicleHours = new HoursFile(h.vehicleHours);
			this.date = DateTime.Today;
			this.previous = h;
		}

		public void save() {
			Stream s = File.OpenWrite(PATH);
			new BinaryFormatter().Serialize(s, this);
			s.Close();
		}

		//Finds the deepest thing buried in the stack and pops it off to be saved
		public bool popAndSave(string dir) {
			//Follow the stack down until you hit the last one, named "twoDeep"
			HoursManagement oneDeep = this.previous;
			if (oneDeep == null)
				return false;
			HoursManagement twoDeep = oneDeep.previous;
			if (twoDeep != null) {
				while (twoDeep.previous != null) {
					oneDeep = twoDeep;
					twoDeep = twoDeep.previous;
				}
				oneDeep.previous = null;
			}
			else {
				twoDeep = oneDeep;
			}

			//Make sure the month is two characters long, we need it to be
			string month = "" + twoDeep.date.Month;
			if (twoDeep.date.Month < 10)
				month = "0" + month;

			//Write the object
			//In the future, maybe this will end up being human readable text? Idk
			try {
				Stream s = File.OpenWrite(dir + "HoursManagement_" + twoDeep.date.Year + month + twoDeep.date.Day);
				new BinaryFormatter().Serialize(s, twoDeep);
				s.Close();
			} catch (Exception e) {
				return false;
			}
			return true;
		}

		public static HoursManagement open() {
			HoursManagement h;
			if (!File.Exists(PATH)) {
				h = new HoursManagement();
			}
			else {
				Stream s = File.OpenRead(PATH);
				h = (HoursManagement)new BinaryFormatter().Deserialize(s);
				s.Close();
			}
			return h;
		}

		public void addEmployee(string s, string t) {
			this.projectHours.addEmployee(s, t);
			this.vehicleHours.addEmployee(s, t);
		}

		public bool removeEmployee(string s, string t) {
			return this.projectHours.removeEmployee(s, t) && this.vehicleHours.removeEmployee(s, t);
		}

		public string print() {
			string s = "Project Hours\n";
			s += this.projectHours.print();
			s += "\nVehicle Hours\n";
			s += this.vehicleHours.print();
			return s;
		}
		/*
        public static void Main(string[] args) {
            HoursManagement h = new HoursManagement();
            string[] arr1 = { "Porject b", "pp", "NIASNOFDOF", "daddy", "qq" };
            string[] arr2 = { "Anthony", "Austin", "Evi", "d", "boi" };
            foreach (string s in arr1)
                h.projectHours.addColumn(s);
            foreach (string s in arr2) {
                h.addEmployee(s, "h");
            }
            Console.Write(h.projectHours.print());
            Console.WriteLine("\n");
            
            HoursManagement t = new HoursManagement(h);
            t.removeEmployee("Anthony", null);
            Console.Write(t.print());
            Console.ReadLine();
         }*/
	}
}