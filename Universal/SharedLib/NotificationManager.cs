using AdaptiveTileExtensions;
using Newtonsoft.Json;
using System;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace SharedLib {
    public static class NotificationManager {
        public static void ScheduleToastNotification(Task taskInstance) {
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
            scheduledToast.Id = taskInstance.uid.Substring(0, 12);

            ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduledToast);
        }

        public static void RemoveScheduledNotification(Task taskInstance) {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            var scheduled = notifier.GetScheduledToastNotifications();

            for (int i = 0; i < scheduled.Count; i++) {
                if (scheduled[i].Id == taskInstance.uid.Substring(0, 12)) {
                    notifier.RemoveFromSchedule(scheduled[i]);
                    break;
                }
            }
        }

        public static void CreateTileNotification(ClassInstance classInstance, DateTime next) {
            var tile = AdaptiveTile.CreateTile();
            var binding = TileBinding.Create(TemplateType.TileWide);
            binding.Branding = Branding.None;
            var subgroup = new SubGroup();

            if (classInstance.classData != null) {
                var header = new Text(classInstance.classData.ToString()) { Style = TextStyle.Body };
                subgroup.AddText(header);
            }

            if (classInstance.from != null) {
                var dateTime = new Text(Data.loader.GetString(classInstance.weekDay.ToString()) + " " + classInstance.from) { Style = TextStyle.Body };
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

            tn.ExpirationTime = new DateTimeOffset(next);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tn);
        }

        public static void CreateTileNotification(Task taskInstance) {
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
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tn);
        }

        public static DateTime PrepareLiveTile() {
            DateTime now = DateTime.Now;
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();

            for (int i = 0; i < Data.tasks.Count; i++) {
                if (Data.tasks[i].deadline < now) continue;
                var deadline = Data.tasks[i].deadline.AddDays(-1.5 * Data.tasks[i].notifyInDays);
                //The new deadline is moved back a few days, it should be smaller because we need it to fit in notification interval
                //past time is catched in the first line in this for
                if (Data.tasks[i].notifyInDays != 0 && deadline <= now) {
                    CreateTileNotification(Data.tasks[0]);
                    return Data.tasks[i].deadline;
                }
            }

            double value = -1;
            int key = 0;
            for (int i = 0; i < Data.classInstances.Count; i++) {
                TimeSpan diff = Extensions.WhenIsNext(Data.classInstances[i], now) - now;
                if (value == -1 || (diff.TotalMinutes >= 15 && diff.TotalMinutes < value)) {
                    value = diff.TotalMinutes;
                    key = i;
                }
            }

            if (value != -1) {
                DateTime next = Extensions.WhenIsNext(Data.classInstances[key], now);
                CreateTileNotification(Data.classInstances[key], next);
                return next;
            }

            return default(DateTime);
        }

    }
}
