using SharedLib;

namespace Rozvrh {
    class DisplayClass {
        public ClassInstance classInstance;
        public Task taskInstance;

        public DisplayClass(ClassInstance classInstance) {
            this.classInstance = classInstance;
        }

        public DisplayClass(Task taskInstance) {
            this.taskInstance = taskInstance;
        }
    }
}
