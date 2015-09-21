using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Streams;

namespace SharedLib {
    public static class Data {
        static StorageFolder roamingFolder = ApplicationData.Current.RoamingFolder;

        static DataStore dataStore = new DataStore();

        public static List<Class> classes { get { return dataStore.classes; } }
        public static List<Teacher> teachers { get { return dataStore.teachers; } }
        public static List<ClassInstance> classInstances { get { return dataStore.classInstances; } }
        public static List<Task> tasks { get { return dataStore.tasks; } }
        static bool _loadingFinished;
        public static bool loadingFinished { get { return _loadingFinished; } }

        public static Windows.ApplicationModel.Resources.ResourceLoader loader = new Windows.ApplicationModel.Resources.ResourceLoader();

        public static void Initialize() {
            dataStore.Load();
        }

        public static void Save() {
            dataStore.Save();
        }

        public static async void SaveToFile() {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Javascript object", new List<string>() { ".json" });
            savePicker.FileTypeChoices.Add("iCalendar", new List<string>() { ".ics" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "Students_Assistent-data";

            StorageFile file = await savePicker.PickSaveFileAsync();

            if (file != null) {
                CachedFileManager.DeferUpdates(file);
                // write to file
                if (file.ContentType == "text/calendar")
                    await FileIO.WriteTextAsync(file, dataStore.ExportToiCalendar());
                else if (file.FileType == ".json")
                    await FileIO.WriteTextAsync(file, dataStore.ExportToJson());

                Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete) {
                    //this.textBlock.Text = "File " + file.Name + " was saved.";
                }
                else {
                    //this.textBlock.Text = "File " + file.Name + " couldn't be saved.";
                }
            }
        }

        public static async void LoadFromFile() {
            var loadPicker = new Windows.Storage.Pickers.FileOpenPicker();
            loadPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            loadPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            loadPicker.FileTypeFilter.Add(".json");

            StorageFile file = await loadPicker.PickSingleFileAsync();

            if (file != null) {
                var inputStream = await file.OpenAsync(FileAccessMode.Read);
                var readStream = inputStream.GetInputStreamAt(0);
                var reader = new DataReader(readStream);
                uint fileLength = await reader.LoadAsync((uint)inputStream.Size);
                dataStore.ImportFromJson(reader.ReadString(fileLength));
            }
        }

        public static void AddClass(Class Class) {
            classes.Add(Class);
            dataStore.Save();
        }

        public static void AddTeacher(Teacher teacher) {
            teachers.Add(teacher);
            dataStore.Save();
        }

        public static void AddClassInstance(ClassInstance classInstance) {
            classInstances.Add(classInstance);
            dataStore.Save();
        }

        public static void AddTask(Task task) {
            tasks.Add(task);
            dataStore.Save();
        }

        public static void ArchiveTask(Task task) {
            int index = tasks.FindIndex(x => x.uid == task.uid);
            dataStore.archivedTasks.Add(tasks[index]);
            tasks.RemoveAt(index);
            dataStore.Save();
        }

        public static void DeleteTeacher(Teacher teacher) {
            dataStore.teachers.Remove(teacher);
            dataStore.Save();
        }

        public static void DeleteTask(Task task) {
            dataStore.tasks.Remove(task);
            dataStore.Save();
        }

        public static void DeleteClass(Class Class) {
            dataStore.classes.Remove(Class);
            dataStore.Save();
        }

        public static void DeleteClassInstance(ClassInstance classInstance) {
            dataStore.classInstances.Remove(classInstance);
            dataStore.Save();
        }

        class DataStore {
            public List<Teacher> teachers = new List<Teacher>();
            public List<Class> classes = new List<Class>();
            public List<ClassInstance> classInstances = new List<ClassInstance>();
            public List<Task> tasks = new List<Task>();
            public List<Task> archivedTasks = new List<Task>();

            public async void Save() {
                StorageFile dataFile = await roamingFolder.CreateFileAsync("dataFile", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(dataFile, JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects }));
            }

            public async void Load() {
                System.Diagnostics.Debug.WriteLine(roamingFolder.Path);
                try {
                    StorageFile dataFile = await roamingFolder.GetFileAsync("dataFile");
                    string json = await FileIO.ReadTextAsync(dataFile);
                    dataStore = JsonConvert.DeserializeObject<DataStore>(json);
                    if (dataStore == null)
                        Save();
                }
                catch {
                    Save();
                }
                _loadingFinished = true;
            }


            public string ExportToJson() {
                return JsonConvert.SerializeObject(this, Formatting.None);
            }

            public void ImportFromJson(string json) {
                dataStore = JsonConvert.DeserializeObject<DataStore>(json);
                Save();
            }

            public string ExportToiCalendar() {
                string iCal = WNLiCal("BEGIN:VCALENDAR");
                iCal += WNLiCal("PRODID:-//Adsamcik//Student's Assistant//" + Windows.ApplicationModel.Package.Current.Id.Version.Major + "." + Windows.ApplicationModel.Package.Current.Id.Version.Minor + "." + Windows.ApplicationModel.Package.Current.Id.Version.Build);
                iCal += WNLiCal("VERSION:2.0");

                DateTime now = DateTime.Now;
                DateTime semestrEnd = now.Month > 8 ? new DateTime(now.Year, 12, 24) : new DateTime(now.Year, 5, 30);

                foreach (var item in Data.classInstances)
                    iCal += WriteiCalEvent(iCal, item, now, semestrEnd);

                foreach (var item in Data.tasks)
                    iCal += WriteiCalEvent(iCal, item, now, semestrEnd);

                iCal += WNLiCal("END:VCALENDAR");
                return iCal;
            }

            string WriteiCalEvent(string iCal, ClassInstance cInstance, DateTime now, DateTime semestrEnd) {
                DateTime next = Extensions.WhenIsNext(cInstance, now);
                string Event = WNLiCal("BEGIN:VEVENT");
                Event += WNLiCal("SUMMARY:" + cInstance.classData.ToString());
                Event += WNLiCal("DTSTART:" + ToICalDateFormat(next));
                Event += WNLiCal("DTEND:" + ToICalDateFormat(next.AddMinutes((cInstance.to - cInstance.from).TotalMinutes)));
                Event += WNLiCal("LOCATION:" + cInstance.room);
                Event += WNLiCal("RRULE:FREQ=WEEKLY;UNTIL=" + ToICalDateFormat(semestrEnd) + (cInstance.weekType != WeekType.EveryWeek ? ";INTERVAL=2" : ""));
                Event += WNLiCal("END:VEVENT");
                return Event;
            }

            string WriteiCalEvent(string iCal, Task tInstance, DateTime now, DateTime semestrEnd) {
                string Event = WNLiCal("BEGIN:VEVENT");
                if (tInstance.classTarget != null) {
                    Event += WNLiCal("SUMMARY:" + tInstance.title + "(" + tInstance.classTarget.shortName + ")");
                    Event += WNLiCal("DESCRIPTION:" + tInstance.classTarget.ToString() + @"\n" + tInstance.description);
                }
                else {
                    Event += WNLiCal("SUMMARY:" + tInstance.title);
                    Event += WNLiCal("DESCRIPTION:" + tInstance.description);
                }
                Event += WNLiCal("DTSTART:" + ToICalDateFormat(tInstance.deadline));
                Event += WNLiCal("DTEND:" + ToICalDateFormat(tInstance.deadline));
                if (tInstance.notifyInDays > 0) {
                    Event += WNLiCal("BEGIN:VALARM");
                    Event += WNLiCal("ACTION:DISPLAY");
                    Event += WNLiCal("DESCRIPTION:This is an event reminder");
                    Event += WNLiCal("TRIGGER:-P" + tInstance.notifyInDays + "D");
                    Event += WNLiCal("END:VALARM");
                }
                Event += WNLiCal("END:VEVENT");
                return Event;
            }

            string WNLiCal(string data) {
                return data + Environment.NewLine;
            }

            string ToICalDateFormat(DateTime dateTime) {
                TimeZoneInfo timeZone = TimeZoneInfo.Local;
                TimeSpan offset = timeZone.GetUtcOffset(DateTime.Now);
                dateTime = dateTime.Add(-offset);
                //Save times internally in UTC
                return dateTime.Year.ToString() + dateTime.Month.ToString("00") + dateTime.Day.ToString("00") + "T" + dateTime.Hour.ToString("00") + dateTime.Minute.ToString("00") + dateTime.Second.ToString("00") + "Z";
            }

            public void ClearAll() {
                classes.Clear();
                teachers.Clear();
                classInstances.Clear();
                Save();
            }

        }
    }
}
