package com.generalupdate.entity.dto;

import lombok.Data;

@Data
public class UpdateVersionDto {

    private String MD5;

    private Long PubTime;

    private String Version;

    private String Url;

    private String Name;
}
