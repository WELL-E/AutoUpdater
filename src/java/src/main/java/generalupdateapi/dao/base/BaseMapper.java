package generalupdateapi.dao.base;

import org.apache.ibatis.annotations.SelectProvider;

import java.util.List;

public interface BaseMapper<T> extends tk.mybatis.mapper.common.BaseMapper<T> {

    @SelectProvider(type = BaseProvider.class,method = "dynamicSQL")
    List<T> selectOnBlur(T record);
}
