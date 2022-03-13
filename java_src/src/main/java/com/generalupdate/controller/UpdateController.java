package com.generalupdate.controller;

import com.generalupdate.dto.UpdateResponseDto;
import com.generalupdate.dto.UpdateVersionDto;
import com.generalupdate.service.UpdateService;
import com.generalupdate.service.UpdateServiceImpl;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.ArrayList;
import java.util.List;

@RestController
@RequestMapping("/update")
public class UpdateController {

    @Autowired
    UpdateService updateService;

    @PostMapping("validate")
    public UpdateResponseDto validate(@RequestBody Integer clientType, @RequestBody String clientVersion, Integer type){
        try {
            List<UpdateVersionDto> resultInfo = null;
            switch (type){
                case 1:
                    resultInfo = getValidateInfos(clientVersion);
                    break;
                case 2:
                    resultInfo = getVersions(clientVersion);
                    break;
            }
            return updateService.validate(clientType,clientVersion,getLastVersion(),resultInfo);
        }catch (Exception exception){
            exception.printStackTrace();
            return new UpdateResponseDto(400,null,"" + exception.getMessage());
        }
    }

    //TODO:The actual application needs to connect to mysql.
    private List<UpdateVersionDto> getValidateInfos(String clientVersion){
        //According to the client version number, find out the update information that is higher than this version in history.
        List<UpdateVersionDto> validateInfoDtos = new ArrayList<UpdateVersionDto>();

        UpdateVersionDto versionDto = new UpdateVersionDto();
        versionDto.setMD5("ce9ef1257467cca72bce8f6bf0e83a54");
        long pubTime = 1626711760;
        versionDto.setPubTime(pubTime);
        versionDto.setVersion("1.1.3");
        validateInfoDtos.add(versionDto);

        UpdateVersionDto versionDto1 = new UpdateVersionDto();
        versionDto1.setMD5("ce9ef1257467cca72bce8f6bf0e83a54");
        long pubTime1 = 1626711820;
        versionDto1.setPubTime(pubTime1);
        versionDto1.setVersion("1.1.4");
        validateInfoDtos.add(versionDto1);

        UpdateVersionDto versionDto2 = new UpdateVersionDto();
        versionDto2.setMD5("224da586553d60315c55e689a789b7bd");
        long pubTime2 = 1626711880;
        versionDto2.setPubTime(pubTime2);
        versionDto2.setVersion("1.1.5");
        validateInfoDtos.add(versionDto2);

        return validateInfoDtos;
    }

    //TODO:The actual application needs to connect to mysql.
    private List<UpdateVersionDto> getVersions(String clientVersion){
        //According to the client version number, find out the update information that is higher than this version in history.
        List<UpdateVersionDto> updateVersionDtos = new ArrayList<UpdateVersionDto>();

        UpdateVersionDto versionDto = new UpdateVersionDto();
        versionDto.setVersion("1.1.3");
        versionDto.setName("version_1.1.3");
        versionDto.setMD5("ce9ef1257467cca72bce8f6bf0e83a54");
        versionDto.setUrl("http://192.168.50.170/Update1.zip");
        long pubTime = 1626711760;
        versionDto.setPubTime(pubTime);
        updateVersionDtos.add(versionDto);

        UpdateVersionDto versionDto1 = new UpdateVersionDto();
        versionDto1.setVersion("1.1.4");
        versionDto1.setName("version_1.1.4");
        versionDto1.setMD5("d9a3785f08ed3dd92872bd807ebfb917");
        versionDto1.setUrl("http://192.168.50.170/Update2.zip");
        long pubTime1 = 1626711820;
        versionDto1.setPubTime(pubTime1);
        updateVersionDtos.add(versionDto1);

        UpdateVersionDto versionDto2 = new UpdateVersionDto();
        versionDto2.setVersion("1.1.5");
        versionDto2.setName("version_1.1.5");
        versionDto2.setMD5("224da586553d60315c55e689a789b7bd");
        versionDto2.setUrl("http://192.168.50.170/Update3.zip");
        long pubTime2 = 1626711880;
        versionDto2.setPubTime(pubTime2);
        updateVersionDtos.add(versionDto2);

        return updateVersionDtos;
    }

    //TODO:The actual application needs to connect to mysql.
    private String getLastVersion(){
        return "99.99.99";
    }
}
