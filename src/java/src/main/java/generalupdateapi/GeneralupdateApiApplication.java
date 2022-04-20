package generalupdateapi;

import org.mybatis.spring.annotation.MapperScan;
import org.mybatis.spring.boot.autoconfigure.MybatisAutoConfiguration;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.autoconfigure.jdbc.DataSourceAutoConfiguration;
import org.springframework.boot.autoconfigure.jdbc.DataSourceTransactionManagerAutoConfiguration;
import org.springframework.boot.autoconfigure.orm.jpa.HibernateJpaAutoConfiguration;

@MapperScan("generalupdateapi.dao.mapper")
@SpringBootApplication
public class GeneralupdateApiApplication {

    public static void main(String[] args) {
        SpringApplication.run(GeneralupdateApiApplication.class, args);
    }

}
