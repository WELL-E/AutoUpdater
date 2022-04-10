package generalupdateapi.exception;

public class DeleteException extends RuntimeException {
    public DeleteException(){ super("exception.normal.del"); }
    public DeleteException(String message){ super(message); }
}
