package com.github.taisyun.hrdemo.controller;

import com.github.taisyun.hrdemo.domain.Job;
import com.github.taisyun.hrdemo.repository.JobRepository;
import com.google.common.collect.Lists;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.List;
import java.util.Map;

/**
 * Created by taisyunn on 15/12/15.
 */
@Controller
public class JobController {


    @Autowired
    private JobRepository jobRepository; // service is omitted

    @RequestMapping(value = "/jobs/list", method = RequestMethod.GET)
    public @ResponseBody List<Job> dispJobList(Map<String, Object> model) {

        List<Job> jobs = Lists.newArrayList(jobRepository.findAll());
        return jobs;
    }

    @RequestMapping(value = "/jobs/list", method = RequestMethod.PUT)
    public ResponseEntity<String> putJobList(@RequestBody List<Job> jobs) {

        jobRepository.save(jobs);
        return new ResponseEntity<String>(HttpStatus.NO_CONTENT);
    }

}
