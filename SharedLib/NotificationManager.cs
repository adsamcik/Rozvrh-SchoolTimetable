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
            scheduledToast.Id = taskInstance.uid.Substring(0,12);

            ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduledToast);

            //ToastNotification toast = new ToastNotification(toastXml);
            //ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        public static void RemoveScheduledNotification(Task taskInstance) {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            var scheduled = notifier.GetScheduledToastNotifications();

            for (int i = 0; i < scheduled.Count; i++) {
                if(scheduled[i].Id == taskInstance.uid.Substring(0,12)) {
                    notifier.RemoveFromSchedule(scheduled[i]);
                    break;
                }
            }
        }

        public static void CreateTileNotification(ClassInstance classInstance) {
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

            tn.ExpirationTime = new DateTimeOffset(Extensions.WhenIsNext(classInstance));
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

        public static async void RegisterBackgroundTileUpdate() {
            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatus == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                backgroundAccessStatus == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity) {
                foreach (var task in BackgroundTaskRegistration.AllTasks) {
                    if (task.Value.Name == updateBackroundTileTaskName) {
                        task.Value.Unregister(true);
                    }
                }

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                taskBuilder.Name = updateBackroundTileTaskName;
                taskBuilder.TaskEntryPoint = taskEntryPoint;
                taskBuilder.SetTrigger(new TimeTrigger(15, false));
                var registration = taskBuilder.Register();
            }
        }

        public static void PrepareLiveTile() {
            DateTime now = DateTime.Now;
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();

            bool hasNotification = false;
            for (int i = 0; i < Data.tasks.Count; i++) {
                if (Data.tasks[i].notifyInDays != 0 && Data.tasks[i].deadline.AddDays(-1.5 * Data.tasks[i].notifyInDays) <= now) {
                    CreateTileNotification(Data.tasks[0]);
                    hasNotification = true;
                    break;
                }
            }

            if (!hasNotification) {
                long value = -1;
                int key = 0;
                for (int i = 0; i < Data.classInstances.Count; i++) {
                    TimeSpan diff = Extensions.WhenIsNext(Data.classInstances[i]) - now;
                    if (value == -1 || (diff.Ticks > 0 && diff.Ticks < value)) {
                        value = diff.Ticks;
                        key = i;
                    }
                }

                if (value != -1)
                    CreateTileNotification(Data.classInstances[key]);
            }
        }

        const string updateBackroundTileTaskName = "BackgroundTileNotificationUpdate";
        const string taskEntryPoint = "BackgroundTasks.LiveTileBackgroundUpdater";

    }
}
