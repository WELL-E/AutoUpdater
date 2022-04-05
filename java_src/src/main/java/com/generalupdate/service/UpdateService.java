package com.generalupdate.service;

import com.generalupdate.entity.dto.UpdateVersionDto;
import com.generalupdate.entity.dto.VeriosnDto;
import org.springframework.web.multipart.MultipartFile;

import java.util.List;

public interface UpdateService {

    List<UpdateVersionDto> validate(Integer clientType , String clientVersion);

    Boolean upload(MultipartFile multipartFile, VeriosnDto veriosnDto);
}