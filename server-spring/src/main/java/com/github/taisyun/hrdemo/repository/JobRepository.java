package com.github.taisyun.hrdemo.repository;

import com.github.taisyun.hrdemo.domain.Job;
import org.springframework.data.repository.PagingAndSortingRepository;
import org.springframework.data.repository.query.Param;

import java.util.List;

public interface JobRepository
        extends PagingAndSortingRepository<Job, Integer>  {

    List<Job> findByName(@Param("name") String name);

}
