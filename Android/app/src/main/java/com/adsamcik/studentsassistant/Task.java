package com.adsamcik.studentsassistant;

import java.util.Date;

/**
 * Created by adsamcik on 19.09.2015.
 */
public class Task {
    public String uid;
    public String title, description;
    public Date deadline;
    public Class classTarget;
    public int notifyInDays;

    //public string deadlineString { get { return deadline.ToString(@"dddd dd\.MMMM HH\:mm"); } }

    public Task(String title, String description, Date deadline, int notifyInDays, Class classTarget, String uid) {
        this.title = title;
        this.description = description;
        this.deadline = deadline;
        this.notifyInDays = notifyInDays;
        this.classTarget = classTarget;
        this.uid = uid;
    }

}
