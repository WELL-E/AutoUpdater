package com.generalupdate.bean;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Scope;
import org.springframework.context.annotation.ScopedProxyMode;

import java.util.List;

@Configuration
public class UpdateBean {

    @Bean
    @Scope(value = "singleton" )
    public List<String> validate(){
        return null;
    }

    @Bean
    @Scope(value = "singleton")
    public List<String> version(){
        return null;
    }
}
