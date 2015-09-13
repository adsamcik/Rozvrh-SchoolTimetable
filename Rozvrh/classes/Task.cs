using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozvrh {
    public class Task {
        public string title, description;
        public DateTime when;
        public Class classTarget;
        public int notifyInDays;

        public Task(string title, string description, DateTime when, int notifyInDays = 0, Class classTarget = null) {
            this.title = title;
            this.description = description;
            this.when = when;
            this.notifyInDays = notifyInDays;
            this.classTarget = classTarget;
        }
    }
}
