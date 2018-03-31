package com.github.teenak.moviedemo.controller;

import com.github.teenak.moviedemo.domain.Movie;
import com.github.teenak.moviedemo.dto.ErrorResponse;
import com.github.teenak.moviedemo.repository.MovieRepository;
import com.google.common.collect.Lists;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.orm.ObjectOptimisticLockingFailureException;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;

import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import java.util.List;
import java.util.Map;

/**
 * Created by teenak on 15/12/15.
 */
@Controller
public class MovieController {


    @Autowired
    private MovieRepository movieRepository; // service is omitted

    @PersistenceContext
    private EntityManager entityManager;

    @RequestMapping(value = "/movies/list", method = RequestMethod.GET)
    public @ResponseBody List<Movie> getMovieList(Map<String, Object> model) {

        List<Movie> movies = Lists.newArrayList(movieRepository.findAll());
        return movies;
    }

    @RequestMapping(value = "/movies/list", method = RequestMethod.PUT)
    public ResponseEntity<Object> putMovieList(@RequestBody List<Movie> movies) {
        try {

            movies.forEach(movie -> {
                entityManager.detach(movie);
            });
            movieRepository.saveAll(movies);

        } catch (ObjectOptimisticLockingFailureException e) {
            ErrorResponse er = new ErrorResponse("The object you are trying to save was changed by another user. Please refresh data.");
            return new ResponseEntity<Object>(er, HttpStatus.CONFLICT);
        }
        return new ResponseEntity<Object>(HttpStatus.NO_CONTENT);
    }

}
