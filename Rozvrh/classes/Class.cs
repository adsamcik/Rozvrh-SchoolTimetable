using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozvrh {
    public class Class {
        public string name;
        public string shortName;
        public Teacher teacher;

        public Class(string name, string shortName, Teacher teacher) {
            this.name = name;
            this.shortName = shortName;
            this.teacher = teacher;
        }

        public override string ToString() {
            return name + " (" + shortName + ")";
        }
    }
}
