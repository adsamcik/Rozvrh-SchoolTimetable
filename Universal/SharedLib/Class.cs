namespace SharedLib {
    public class Class {
        public string name;
        public string shortName;

        public Class(string name, string shortName) {
            this.name = name;
            this.shortName = shortName;
        }

        public override string ToString() {
            return name + " (" + shortName + ")";
        }
    }
}
