using Rozvrh.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozvrh {
    public class ClassInstance {
        public Class classData;
        public TimeSpan from, to;
        public string room;
        public WeekDay day;

        public ClassInstance(Class classData, TimeSpan from, TimeSpan to, string room, WeekDay day) {
            this.classData = classData;
            this.from = from;
            this.to = to;
            this.room = room;
            this.day = day;
        }

        public override string ToString() {
            return classData.ToString() + " " + from.ToString() + " - " + to.ToString();
        }

    }
}
