package com.generalupdate.controller;

import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/update")
public class UpdateController {

    @PostMapping("validate")
    public String validate(@RequestBody Integer clientType,@RequestBody String clientVersion){
        return "";
    }

    @PostMapping("version")
    public String version(@RequestBody Integer clientType,@RequestBody String clientVersion){
        return "";
    }
}
