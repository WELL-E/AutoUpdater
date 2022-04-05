package com.generalupdate.dao.base;

import org.apache.ibatis.mapping.MappedStatement;
import tk.mybatis.mapper.entity.EntityColumn;
import tk.mybatis.mapper.mapperhelper.EntityHelper;
import tk.mybatis.mapper.mapperhelper.MapperHelper;
import tk.mybatis.mapper.mapperhelper.MapperTemplate;
import tk.mybatis.mapper.mapperhelper.SqlHelper;

import java.util.Set;

import static tk.mybatis.mapper.mapperhelper.SqlHelper.getIfNotNull;

public class BaseProvider extends MapperTemplate {
    public BaseProvider(Class<?> mapperClass, MapperHelper mapperHelper) {
        super(mapperClass, mapperHelper);
    }

    public String selectOnBlur(MappedStatement ms){
        final Class<?> entityClass = getEntityClass(ms);
        setResultType(ms,entityClass);
        StringBuilder sql = new StringBuilder();
        sql.append(SqlHelper.selectAllColumns(entityClass));
        sql.append(SqlHelper.fromTable(entityClass,tableName(entityClass)));
        sql.append("<where>");
        Set<EntityColumn> columnSet = EntityHelper.getColumns(entityClass);
        for (EntityColumn column : columnSet){
            if(!column.isId()){
                if (column.getJavaType().equals(String.class)){
                    sql.append(getIfNotNull(column,getBind(column) + "and" + getColumnLikeHolder(column),isNotEmpty()));
                }else{
                    sql.append(getIfNotNull(column," AND " + column.getColumnEqualsHolder(),isNotEmpty()));
                }
            }
        }
        sql.append("</where>");
        return sql.toString();
    }

    private String getColumnLikeHolder(EntityColumn column){
        return String.format("%s like #{%s_bind}", column.getColumn(),column.getProperty());
    }

    private String getBind(EntityColumn column){
        return "<bind name=\"" + column.getProperty() + "_bind\" value=\"'%'"+ column.getProperty() +"+'%'\" />";
    }
}
