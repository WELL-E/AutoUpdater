package com.generalupdate.exception;

public class UpdateException extends RuntimeException{
    public UpdateException(){ super("exception.normal.update"); }
    public UpdateException(String message){ super(message); }
}
