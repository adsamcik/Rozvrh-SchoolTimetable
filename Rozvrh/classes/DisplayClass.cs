using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
