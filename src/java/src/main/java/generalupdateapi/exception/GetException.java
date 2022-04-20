package generalupdateapi.exception;

public class GetException extends RuntimeException {
    public GetException(){ super("exception.normal.get"); }
    public GetException(String message){ super(message); }
}
