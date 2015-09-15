using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozvrh {
    public class Teacher {
        public string name, surname, degree, phone, email;

        [JsonIgnore]
        public string fullName { get { return (!string.IsNullOrWhiteSpace(degree) ? degree + " " : "") + name + " " + surname; } }
        
        public string classes { get {
                List<ClassInstance> cInstance = Data.classInstances.FindAll(x => x.teacher == this).DistinctBy(x => new { x.classData, x.classType}).ToList();
                string result = "";
                foreach (var classInstance in cInstance)
                    result += classInstance.classData.shortName + "(" + Data.loader.GetString(classInstance.classType.ToString()) + "), ";

                return (cInstance.Count > 0) ? result.Substring(0, result.Length-2) : result;
        } }

        public Teacher(string name, string surname, string email = "", string phone = "", string degree = "") {
            this.name = name;
            this.surname = surname;
            this.degree = degree;
            this.email = email;
            this.phone = phone;
        }

        public override string ToString() {
            return fullName;
        }
    }
}
