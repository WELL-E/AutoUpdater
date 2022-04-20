package generalupdateapi.entity.db;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import javax.persistence.Table;

@Builder
@AllArgsConstructor
@NoArgsConstructor
@Data
@Table(name = "updateversioninfo")
public class Version {

    private String MD5;
    private Integer pubTime;
    private String name;
    private String url;
    private String version;
    private Integer clientType;
}
