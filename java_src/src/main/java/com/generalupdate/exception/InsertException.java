package com.generalupdate.exception;

public class InsertException extends RuntimeException {
    public InsertException(){ super("exception.normal.insert"); }
    public InsertException(String message){ super(message); }
}
