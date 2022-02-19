package com.generalupdate.controller;

import com.generalupdate.bean.UpdateBean;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/update")
public class UpdateController {

    @Autowired
    UpdateBean updateBean;

    @PostMapping("validate")
    public String validate(@RequestBody Integer clientType,@RequestBody String clientVersion){
        return "";
    }

    @PostMapping("version")
    public String version(@RequestBody Integer clientType,@RequestBody String clientVersion){
        return "";
    }
}
