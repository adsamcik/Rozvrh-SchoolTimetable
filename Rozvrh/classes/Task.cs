using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace Rozvrh {
    public class Task {
        public string title, description;
        public DateTime deadline;
        public Class classTarget;
        public int notifyInDays;

        public Task(string title, string description, DateTime deadline, int notifyInDays = 0, Class classTarget = null) {
            this.title = title;
            this.description = description;
            this.deadline = deadline;
            this.notifyInDays = notifyInDays;
            this.classTarget = classTarget;
            //ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;
            //XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
        }
    }
}
