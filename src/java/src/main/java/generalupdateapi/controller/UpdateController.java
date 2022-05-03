package generalupdateapi.controller;


import generalupdateapi.entity.db.Version;
import generalupdateapi.entity.dto.UpdateVersionDto;
import generalupdateapi.entity.dto.VeriosnDto;
import generalupdateapi.entity.vo.Response;
import generalupdateapi.service.UpdateService;
import lombok.extern.slf4j.Slf4j;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import javax.annotation.Resource;
import java.util.ArrayList;
import java.util.List;

import static generalupdateapi.entity.consts.ResponseCodes.ERROR;
import static generalupdateapi.entity.consts.ResponseCodes.SUCCESS;

@Slf4j
@RestController
@RequestMapping("/update")
public class UpdateController {

    @Resource
    UpdateService updateService;

    /**
     * 验证当前客户端是否需要更新
     * */
    @GetMapping("validate/{clientType}/{clientVersion}")
    public Response<List<Version>> validate(@PathVariable Integer clientType, @PathVariable String clientVersion){
        try {
            return new Response(SUCCESS,updateService.validate(clientType,clientVersion),"Validation completed successfully.");
        }catch (Exception exception){
            exception.printStackTrace();
            return new Response(ERROR,null,exception.getMessage());
        }
    }

    /**
     * 获取更新版本信息
     * */
    @GetMapping("versions/{clientType}/{clientVersion}")
    public Response<List<UpdateVersionDto>> versions(@PathVariable Integer clientType, @PathVariable String clientVersion)
    {
        try {
            return new Response(SUCCESS,updateService.versions(clientType,clientVersion),"Get versions infomation completed successfully.");
        }catch (Exception exception){
            exception.printStackTrace();
            return new Response(ERROR,null,exception.getMessage());
        }
    }

    /**
     * 上传补丁包信息
     * */
    @PostMapping("upload")
    public Response<Boolean> upload(@RequestParam("file") MultipartFile file, VeriosnDto veriosnDto){
        try {
            Boolean isUpload = updateService.upload(file,veriosnDto);
            Integer status = isUpload ? SUCCESS : ERROR;
            String message = isUpload ? "Uploaded successfully." : "Failed to upload update patch package !";
            return new Response<>(status, isUpload,message);
        }catch (Exception exception){
            exception.printStackTrace();
            log.error("upload error :" + exception.getMessage());
            return new Response<>(ERROR,false,"Failed to upload update patch package !");
        }
    }
}
