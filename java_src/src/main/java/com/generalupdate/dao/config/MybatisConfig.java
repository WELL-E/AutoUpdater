package com.generalupdate.dao.config;

import com.alibaba.druid.pool.DruidDataSource;
import com.github.pagehelper.PageInterceptor;
import org.apache.commons.lang3.ArrayUtils;
import org.apache.commons.lang3.ObjectUtils;
import org.apache.ibatis.plugin.Interceptor;
import org.apache.ibatis.session.SqlSessionFactory;
import org.mybatis.spring.SqlSessionFactoryBean;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.io.Resource;
import org.springframework.core.io.support.PathMatchingResourcePatternResolver;
import org.springframework.core.io.support.ResourcePatternResolver;

@Configuration
public class MybatisConfig {

    @Autowired
    @Qualifier("DS_Update")
    private DruidDataSource monitor;

    @Autowired
    PageInterceptor interceport;

    @Bean(name = "SF_Update")
    public SqlSessionFactory SessionFactory() throws Exception{
        SqlSessionFactoryBean bean = new SqlSessionFactoryBean();
        bean.setPlugins(new Interceptor[]{interceport});
        Resource[] resources = null;
        try {
            bean.setDataSource(monitor);
            ResourcePatternResolver resolver = new PathMatchingResourcePatternResolver();
            Resource[] monitorResources = resolver.getResources("classpath:mapper/*.xml");
            Resource[] commonResources = resolver.getResources("classpath:mapper/common.xml");
            if(ObjectUtils.allNotNull(monitorResources,commonResources)){
                resources = ArrayUtils.addAll(monitorResources,commonResources);
            }
            bean.setMapperLocations(resources);
            return bean.getObject();
        }catch (Exception exception){
            throw new RuntimeException("sqlSessionFactory init fail", exception);
        }
    }
}
