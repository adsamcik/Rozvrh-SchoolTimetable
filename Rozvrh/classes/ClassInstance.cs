﻿using Rozvrh.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozvrh {
    public class ClassInstance {
        public Class classData;
        public TimeSpan from, to;

        public string fromTo {
            get {
                return from.ToString(@"hh\:mm") + " - " + to.ToString(@"hh\:mm");
            }
        }

        public string room;
        public WeekDay day;
        public WeekType weekType;
        public Teacher teacher;

        public ClassInstance(Class classData, TimeSpan from, TimeSpan to, string room, WeekDay day, WeekType weekType, Teacher teacher) {
            this.classData = classData;
            this.from = from;
            this.to = to;
            this.room = room;
            this.day = day;
            this.weekType = weekType;
            this.teacher = teacher;
        }

        public override string ToString() {
            return classData.ToString() + " " + from.ToString() + " - " + to.ToString();
        }

    }
}
