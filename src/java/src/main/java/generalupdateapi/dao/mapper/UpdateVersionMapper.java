package generalupdateapi.dao.mapper;

import generalupdateapi.dao.base.BaseMapper;
import generalupdateapi.entity.db.Version;
import org.apache.ibatis.annotations.Mapper;

@Mapper
public interface UpdateVersionMapper extends BaseMapper<Version> {

}
