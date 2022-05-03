package generalupdateapi.dao.mapper;

import generalupdateapi.dao.base.BaseMapper;
import generalupdateapi.entity.db.Version;
import org.apache.ibatis.annotations.Mapper;

import java.util.List;

@Mapper
public interface UpdateVersionMapper extends BaseMapper<Version> {

    List<Version> queryLastVersion(Integer clientType);

    List<Version> queryVersion(Integer pubTime , Integer clientType);

    List<Version> queryValidateInfo(Integer pubTime , Integer clientType);
}
