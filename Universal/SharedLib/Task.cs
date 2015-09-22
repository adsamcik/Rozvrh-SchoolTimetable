using Newtonsoft.Json;
using System;

namespace SharedLib {
    public class Task {
        public string uid;
        public string title, description;
        public DateTime deadline;
        public Class classTarget;
        public int notifyInDays;

        [JsonIgnore]
        public string deadlineString { get { return deadline.ToString(@"dddd dd\.MMMM HH\:mm"); } }

        [JsonIgnore]
        public string deadlineStringShort { get { return deadline.ToString(@"dd\.MMMM HH\:mm"); } }

        [JsonIgnore]
        public bool isSoon { get { _daysLeft = (deadline - DateTime.Now).TotalDays; return _daysLeft <= notifyInDays * 1.5 || (_daysLeft <= 7 && _daysLeft > 0); } }
        double _daysLeft;

        public Task(string title, string description, DateTime deadline, int notifyInDays = 0, Class classTarget = null, string uid = "") {
            this.title = title;
            this.description = description;
            this.deadline = deadline;
            this.notifyInDays = notifyInDays;
            this.classTarget = classTarget;

            if (string.IsNullOrWhiteSpace(uid))
                //Hopefuly there will be no collision :P
                this.uid = Guid.NewGuid().ToString();
            else
                this.uid = uid;
        }

    }
}
