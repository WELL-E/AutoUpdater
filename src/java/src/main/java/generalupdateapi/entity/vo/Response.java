package generalupdateapi.entity.vo;


import generalupdateapi.entity.consts.ResponseCodes;

import java.io.Serializable;

public class Response<T> implements Serializable {

    private int _status = ResponseCodes.SUCCESS;
    private T _data = null;
    private String _message = null;

    public Response(int status,T data,String message){
        _status = status;
        _data = data;
        _message = message;
    }

    public Response(T data){
        _status = ResponseCodes.SUCCESS;
        _data = data;
    }

    public Response(T data,String message){
        _status = ResponseCodes.SUCCESS;
        _data = data;
        _message = message;
    }

    public Response(){
        _status = ResponseCodes.SUCCESS;
    }
}
