package com.adsamcik.studentsassistant;

/**
 * Created by adsamcik on 19.09.2015.
 */
public class DisplayClass {
    public ClassInstance classInstance;
    public Task taskInstance;

    public DisplayClass(ClassInstance classInstance) {
        this.classInstance = classInstance;
    }

    public DisplayClass(Task taskInstance) {
        this.taskInstance = taskInstance;
    }
}
