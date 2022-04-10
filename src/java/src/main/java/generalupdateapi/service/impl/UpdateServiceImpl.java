package generalupdateapi.service.impl;

import generalupdateapi.entity.db.Version;
import generalupdateapi.entity.dto.UpdateVersionDto;
import generalupdateapi.entity.dto.VeriosnDto;
import generalupdateapi.dao.mapper.UpdateVersionMapper;
import generalupdateapi.service.UpdateService;
import generalupdateapi.utils.VersionUtil;
import lombok.SneakyThrows;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;

import javax.annotation.Resource;
import java.util.List;

@Slf4j
@Service
public class UpdateServiceImpl implements UpdateService {

    @Resource
    private UpdateVersionMapper versionMapper;

    private String fileServerUrl;

    @Override
    public List<UpdateVersionDto> validate(Integer clientType, String clientVersion) {
        String jsonResult = null;
        try {
            String serverLastVersion = "99.99.99.99";
            Integer compareResult = compareVersion(clientVersion,serverLastVersion);
            if (compareResult == 0){
                //return new UpdateResponseDto(200,jsonResult,"No need to update.");
            }else if(compareResult > 0){
                //return new UpdateResponseDto(200,jsonResult,"No need to update.");
            }else if(compareResult < 0){
                //DOTO:select
            }
            return null;
        }catch (Exception exception){
            exception.printStackTrace();
            return null;
        }
    }

    @SneakyThrows
    @Override
    public Boolean upload(MultipartFile multipartFile, VeriosnDto veriosnDto) {
        String md5 = VersionUtil.getMD5(multipartFile.getBytes());
        if(!veriosnDto.getMD5().equals(md5))return false;
        if (transferToFileServer(multipartFile,veriosnDto.getMD5())) {
            Integer pubTime = (int)(System.currentTimeMillis() / 1000);
            Version version = Version.builder().
                    version(veriosnDto.getVersion()).
                    MD5(veriosnDto.getMD5()).
                    clientType(veriosnDto.getClientType()).
                    name(veriosnDto.getName()).
                    pubTime(pubTime).
                    url(fileServerUrl + veriosnDto.getMD5()).
                    build();
            return versionMapper.insert(version) > 0;
        }
        return false;
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

    /**
     * TODO: upload file to the file server.
     */
    private Boolean transferToFileServer(MultipartFile file,String md5){
        return true;
    }
}
