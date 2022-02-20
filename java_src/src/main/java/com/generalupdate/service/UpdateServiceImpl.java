package com.generalupdate.service;

import com.alibaba.fastjson.JSONObject;
import com.generalupdate.dto.UpdateResponseDto;
import com.generalupdate.dto.UpdateVersionDto;
import jdk.nashorn.internal.runtime.Version;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;
import org.springframework.stereotype.Indexed;

import java.util.List;

@Component("UpdateService")
public class UpdateServiceImpl implements UpdateService {

    @Override
    public UpdateResponseDto validate(Integer clientType, String clientVersion, String serverLastVersion, List<UpdateVersionDto> versionDtos) {
        String jsonResult = null;
        try {
            Integer compareResult = compareVersion(clientVersion,serverLastVersion);
            if (compareResult == 0){
                return new UpdateResponseDto(200,jsonResult,"No need to update.");
            }else if(compareResult > 0){
                return new UpdateResponseDto(200,jsonResult,"No need to update.");
            }else if(compareResult < 0){
                jsonResult = JSONObject.toJSONString(versionDtos);
            }
            return new UpdateResponseDto(200,jsonResult,"");
        }catch (Exception exception){
            exception.printStackTrace();
            return new UpdateResponseDto(500,jsonResult,"Server internal error." + exception.getMessage());
        }
    }

    @Override
    public Boolean DifferentialPackage(String oldPath, String newPath, String targetPath) {
        return null;
    }

    /**
     * Compare the size of the version number, the former returns a positive number, the latter returns a negative number, and equal returns 0.
     * @param version1
     * @param version2
     * @return
     */
    private int compareVersion(String version1, String version2) throws Exception {
        if (version1 == null || version2 == null) {
            throw new Exception("compareVersion error:illegal params.");
        }
        String[] versionArray1 = version1.split("\\.");//注意此处为正则匹配，不能用"."；
        String[] versionArray2 = version2.split("\\.");
        int idx = 0;
        int minLength = Math.min(versionArray1.length, versionArray2.length);//取最小长度值
        int diff = 0;
        while (idx < minLength
                && (diff = versionArray1[idx].length() - versionArray2[idx].length()) == 0//先比较长度
                && (diff = versionArray1[idx].compareTo(versionArray2[idx])) == 0) {//再比较字符
            ++idx;
        }
        //如果已经分出大小，则直接返回，如果未分出大小，则再比较位数，有子版本的为大；
        diff = (diff != 0) ? diff : versionArray1.length - versionArray2.length;
        return diff;
    }
}
