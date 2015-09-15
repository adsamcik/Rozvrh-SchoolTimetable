using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Windows.Storage;

namespace Rozvrh {
    public static class Data {
        static DataStore dataStore = new DataStore();

        public static List<Class> classes { get { return dataStore.classes; } }
        public static List<Teacher> teachers { get { return dataStore.teachers; } }
        public static List<ClassInstance> classInstances { get { return dataStore.classInstances; } }
        public static List<Task> tasks { get { return dataStore.tasks; } }

        public static Windows.ApplicationModel.Resources.ResourceLoader loader = new Windows.ApplicationModel.Resources.ResourceLoader();

        static StorageFolder roamingFolder;

        public static void Save() {
            dataStore.Save();
        }

        public static void Initialize() {
            roamingFolder = ApplicationData.Current.RoamingFolder;
            dataStore.Load();
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
                    dataStore = JsonConvert.DeserializeObject<DataStore>(await FileIO.ReadTextAsync(dataFile));
                }
                catch {
                    Save();
                }
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
