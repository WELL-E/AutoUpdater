package generalupdateapi.exception;

import org.springframework.validation.ObjectError;

import java.util.List;

public class ValidateException extends RuntimeException{

    private static final long serialVersionUID = 7207451175263593487L;
    private List<ObjectError> errors;

    public ValidateException(List<ObjectError> errors){
        this.errors = errors;
    }

    public List<ObjectError> getErrors(){ return errors; }

    public void setErrors(List<ObjectError> errors){ this.errors =errors; }

    @Override
    public String toString(){ return errors.get(0).getDefaultMessage(); }

    @Override
    public String getMessage(){ return toString(); }
}
