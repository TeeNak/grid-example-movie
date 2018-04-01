package com.github.teenak.moviedemo.repository;

import com.github.teenak.moviedemo.domain.Movie;
import org.springframework.data.repository.PagingAndSortingRepository;
import org.springframework.data.repository.query.Param;

import java.util.List;

public interface MovieRepository
        extends PagingAndSortingRepository<Movie, Integer>  {

    List<Movie> findByName(@Param("name") String name);

}
