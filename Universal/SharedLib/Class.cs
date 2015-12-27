using System;

namespace SharedLib {
    public class Class {
        public string uid;
        public string name;
        public string shortName;

        public Class(string name, string shortName, string uid = "") {
            this.name = name;
            this.shortName = shortName;

            if (string.IsNullOrWhiteSpace(uid))
                this.uid = Guid.NewGuid().ToString();
            else
                this.uid = uid;
        }

        public override string ToString() {
            return name + " (" + shortName + ")";
        }
    }
}
