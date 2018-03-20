package com.github.taisyun.hrdemo.repository.handler;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.data.rest.core.annotation.HandleBeforeSave;
import org.springframework.data.rest.core.annotation.RepositoryEventHandler;
import org.springframework.stereotype.Component;

import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;

/**
 * Created by taisyunn on 15/12/15.
 */
@Component
@RepositoryEventHandler(Object.class)
public class ObjectRepositoryEventHandler {

    private static final Logger logger = LoggerFactory.getLogger(ObjectRepositoryEventHandler.class);

    @PersistenceContext
    private EntityManager entityManager;

    @HandleBeforeSave
    public void handleBeforeSave(Object obj) {

        logger.debug("Saving Obj " + obj.toString());
        entityManager.detach(obj); // necessary to make optimistic lock work

    }
}