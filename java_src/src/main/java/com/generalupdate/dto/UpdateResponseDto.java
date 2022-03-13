package com.generalupdate.dto;

import lombok.Data;

@Data
public class UpdateResponseDto {

    private Integer code;

    private String body;

    private String message;

    public UpdateResponseDto(Integer code_u , String body_u , String message_u){
        code = code_u;
        body = body_u;
        message = message_u;
    }
}
