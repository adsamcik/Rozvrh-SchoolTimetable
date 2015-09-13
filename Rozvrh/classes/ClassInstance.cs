using Newtonsoft.Json;
using System;

namespace Rozvrh {
    public class ClassInstance {
        public Class classData;
        public ClassType classType;
        [JsonIgnore]
        public string classTypeString { get { return Data.loader.GetString("Class" + classType.ToString()); } }

        public TimeSpan from, to;

        [JsonIgnore]
        public string fromToString { get { return from.ToString(@"hh\:mm") + " - " + to.ToString(@"hh\:mm"); } }

        public string room;
        public WeekDay day;
        public WeekType weekType;
        public Teacher teacher;

        public ClassInstance(Class classData, ClassType classType, TimeSpan from, TimeSpan to, string room, WeekDay day, WeekType weekType, Teacher teacher) {
            this.classData = classData;
            this.classType = classType;
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
