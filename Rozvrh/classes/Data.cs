using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Rozvrh {
    public static class Data {
        public static List<Class> classes = new List<Class>();
        public static List<Teacher> teachers = new List<Teacher>();
        public static List<ClassInstance> classInstances = new List<ClassInstance>();

        public void AddClass(Class Class) {
            classes.Add(Class);
            
        }

        public void Save() {
            JsonObject.
        }
    }
}
