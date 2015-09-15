using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace Rozvrh {
    public class Task {
        public string uid;
        public string title, description;
        public DateTime deadline;
        public Class classTarget;
        public int notifyInDays;

        public string deadlineString { get { return deadline.ToString(@"dddd dd\.MMMM HH\:mm"); } }

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
