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

        static Windows.Storage.StorageFolder roamingFolder;

        public static void Initialize() {
            roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
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

        class DataStore {
            public List<Teacher> teachers = new List<Teacher>();
            public List<Class> classes = new List<Class>();
            public List<ClassInstance> classInstances = new List<ClassInstance>();

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
