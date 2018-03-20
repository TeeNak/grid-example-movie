package com.github.taisyun.hrdemo.configuration;

import com.github.taisyun.hrdemo.domain.Job;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.rest.core.config.RepositoryRestConfiguration;
import org.springframework.data.rest.webmvc.config.RepositoryRestMvcConfiguration;

/**
 * Created by taisyunn on 15/12/15.
 */
@Configuration
public class RestConfiguration  extends RepositoryRestMvcConfiguration {

    @Override
    protected void configureRepositoryRestConfiguration(RepositoryRestConfiguration config) {
        config.exposeIdsFor(Job.class);
    }
}
