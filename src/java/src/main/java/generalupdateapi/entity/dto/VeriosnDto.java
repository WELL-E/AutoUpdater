package generalupdateapi.entity.dto;

import lombok.Data;

@Data
public class VeriosnDto {
    private String MD5;
    private String name;
    private String version;
    private Integer clientType;
}
