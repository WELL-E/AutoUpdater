package com.generalupdate.service;

import com.generalupdate.dto.UpdateResponseDto;
import com.generalupdate.dto.UpdateVersionDto;

import java.util.List;

public interface UpdateService {

    public UpdateResponseDto validate(Integer clientType , String clientVersion, String serverLastVersion, List<UpdateVersionDto> versionDtos);

    public Boolean DifferentialPackage(String oldPath,String newPath,String targetPath);
}