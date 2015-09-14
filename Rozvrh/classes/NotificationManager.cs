using AdaptiveTileExtensions;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Rozvrh {
    class NotificationManager {
        public static void SetNotification(Task taskInstance) {
            DateTime notificationTime = taskInstance.deadline.AddDays(-taskInstance.notifyInDays);
            if (notificationTime <= DateTime.Now) return;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            XmlNodeList toastTextAttributes = toastXml.GetElementsByTagName("text");
            toastTextAttributes[0].InnerText = taskInstance.title;
            toastTextAttributes[1].InnerText = taskInstance.description;
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "long");
            ((XmlElement)toastNode).SetAttribute("launch", JsonConvert.SerializeObject(new LaunchData(taskInstance.GetType(), taskInstance.uid)));

            ScheduledToastNotification scheduledToast = new ScheduledToastNotification(toastXml, notificationTime);
            scheduledToast.Id = taskInstance.uid.Substring(0,12);

            ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduledToast);

            //ToastNotification toast = new ToastNotification(toastXml);
            //ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        public static void CreateClassNotification(ClassInstance classInstance) {
            var tile = AdaptiveTile.CreateTile();
            var binding = TileBinding.Create(TemplateType.TileWide);
            binding.Branding = Branding.None;
            var subgroup = new SubGroup();

            if (classInstance.classData != null) {
                var header = new Text(classInstance.classData.ToString()) { Style = TextStyle.Body };
                subgroup.AddText(header);
            }

            if (classInstance.from != null) {
                var dateTime = new Text(Data.loader.GetString(classInstance.day.ToString()) + " " + classInstance.from) { Style = TextStyle.Body };
                subgroup.AddText(dateTime);
            }

            if (classInstance.room != null) {
                var classRoom = new Text(classInstance.room) { Style = TextStyle.Caption };
                subgroup.AddText(classRoom);
            }

            if (classInstance.teacher != null) {
                var teacher = new Text(classInstance.teacher.ToString()) { Style = TextStyle.Caption };
                subgroup.AddText(teacher);
            }


            binding.Add(subgroup);

            tile.Tiles.Add(binding);
            TileNotification tn = tile.GetNotification();

            tn.ExpirationTime = new DateTimeOffset(WhenIsNextInDays(classInstance));
            Debug.WriteLine("Expire on " + tn.ExpirationTime);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tn);
        }

        public static void CreateTaskNotification(Task taskInstance) {
            var tile = AdaptiveTile.CreateTile();
            var binding = TileBinding.Create(TemplateType.TileWide);
            binding.Branding = Branding.None;
            var subgroup = new SubGroup();

            if (!string.IsNullOrWhiteSpace(taskInstance.title)) {
                var header = new Text(taskInstance.title) { Style = TextStyle.Body };
                subgroup.AddText(header);
            }

            if (taskInstance.deadline != null) {
                var dateTime = new Text(taskInstance.deadline.ToString()) { Style = TextStyle.Body, IsNumeralStyle = true };
                subgroup.AddText(dateTime);
            }

            if (taskInstance.classTarget != null) {
                var classRoom = new Text(taskInstance.classTarget.ToString()) { Style = TextStyle.Caption, IsSubtleStyle = true };
                subgroup.AddText(classRoom);
            }

            if (taskInstance.description != null) {
                var description = new Text(taskInstance.description);
                subgroup.AddText(description);
            }


            binding.Add(subgroup);

            tile.Tiles.Add(binding);
            TileNotification tn = tile.GetNotification();

            tn.ExpirationTime = new DateTimeOffset(taskInstance.deadline);
            Debug.WriteLine("Expire on " + tn.ExpirationTime);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tn);
        }

        static DateTime WhenIsNextInDays(ClassInstance classInstance) {
            DateTime now = DateTime.Now;
            int currentDay = (int)now.DayOfWeek - 1;
            if (currentDay == -1) currentDay = 6;


            int expireIn;
            if (currentDay == (int)classInstance.day)
                expireIn = classInstance.from > now.TimeOfDay ? 0 : 7;
            else
                expireIn = currentDay <= (int)classInstance.day ? (int)classInstance.day - currentDay : 6 - currentDay + (int)classInstance.day;
            now = now.AddDays(expireIn);

            return new DateTime(now.Year, now.Month, now.Day, classInstance.from.Hours, classInstance.from.Minutes, classInstance.from.Seconds);
        }
    }
}
