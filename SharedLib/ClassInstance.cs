using Newtonsoft.Json;
using System;

namespace SharedLib {
    public class ClassInstance {
        public string uid;
        public Class classData;
        public ClassType classType;
        public TimeSpan from, to;
        public string room;
        public WeekDay day;
        public WeekType weekType;
        public Teacher teacher;

        [JsonIgnore]
        public string classTypeString { get { return Data.loader.GetString("Class" + classType.ToString()); } }
        [JsonIgnore]
        public string fromToString { get { return from.ToString(@"hh\:mm") + " - " + to.ToString(@"hh\:mm"); } }
        [JsonIgnore]
        public string nextFullString { get { return Next.ToString(@"dddd dd\.MMMM HH\:mm"); } }
        [JsonIgnore]
        public DateTime Next { get { return Extensions.WhenIsNext(this, DateTime.Now); } }

        public ClassInstance(Class classData, ClassType classType, TimeSpan from, TimeSpan to, string room, WeekDay day, WeekType weekType, Teacher teacher, string uid = "") {
            this.classData = classData;
            this.classType = classType;
            this.from = from;
            this.to = to;
            this.room = room;
            this.day = day;
            this.weekType = weekType;
            this.teacher = teacher;

            if (string.IsNullOrWhiteSpace(uid))
                this.uid = Guid.NewGuid().ToString();
            else
                this.uid = uid;
        }

        public override string ToString() {
            return classData.ToString() + " " + from.ToString() + " - " + to.ToString();
        }

    }
}
