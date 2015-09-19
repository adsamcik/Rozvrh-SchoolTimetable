package com.adsamcik.studentsassistant;

/**
 * Created by adsamcik on 19.09.2015.
 */
public class Class {
    public String name, shortName;

    public Class(String name, String shortName) {
        this.name = name;
        this.shortName = shortName;
    }

    @Override
    public String toString() {
        return name + " (" + shortName + ")";
    }
}
