//package generalupdateapi.dao.config;
//
//import com.alibaba.druid.pool.DruidDataSource;
//import org.springframework.boot.context.properties.ConfigurationProperties;
//import org.springframework.context.annotation.Bean;
//import org.springframework.context.annotation.Configuration;
//import org.springframework.transaction.annotation.EnableTransactionManagement;
//
//@Configuration
//@EnableTransactionManagement
//public class DataSourceConfig {
//
//    @Bean(name = "DS_Update",destroyMethod = "close",initMethod = "init")
//    @ConfigurationProperties(prefix = "druid.cloud-survey")
//    public DruidDataSource surveyDataSource(){ return new DruidDataSource(); }
//
//}
