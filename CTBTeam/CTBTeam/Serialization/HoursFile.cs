using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.UI.WebControls;

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
        public HoursManagement previous;
        
        public static string PATH = @"~/Serialization/hours.daddy";
            //@"C:\Users\alna173017\Desktop\hours.daddy"; 

        public HoursManagement() {
            this.projectHours = new HoursFile();
            this.vehicleHours = new HoursFile();
            this.date = DateTime.Today;
            this.previous = null;
        }

        public HoursManagement(HoursManagement h) {
            this.projectHours = h.projectHours;
            this.vehicleHours = h.vehicleHours;
            this.date = DateTime.Today;
            this.previous = h;
        }

        public void save(string newPath) {
           // bool test = File.Exists(PATH);
            Stream s = File.OpenWrite(newPath);
            new BinaryFormatter().Serialize(s, this);
            s.Close();
        }

        public static HoursManagement open(string newPath) {
            HoursManagement h;
            if (!File.Exists(newPath)) {
                h = new HoursManagement();
            }
            else {
                Stream s = File.OpenRead(newPath);
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