using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozvrh {
    public class Teacher {
        public string name, surname, degree, phone, email;

        public Teacher(string name, string surname, string email = "", string phone = "", string degree = "") {
            this.name = name;
            this.surname = surname;
            this.degree = degree;
            this.email = email;
            this.phone = phone;
        }

        public override string ToString() {
            return (!string.IsNullOrWhiteSpace(degree) ? degree + " " : "") + name + " " + surname;
        }
    }
}
