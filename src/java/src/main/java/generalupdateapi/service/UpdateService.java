package generalupdateapi.service;

import generalupdateapi.entity.dto.UpdateVersionDto;
import generalupdateapi.entity.dto.VeriosnDto;
import org.springframework.web.multipart.MultipartFile;

import java.util.List;

public interface UpdateService {

    List<UpdateVersionDto> validate(Integer clientType , String clientVersion);

    Boolean upload(MultipartFile multipartFile, VeriosnDto veriosnDto);
}