DROP TABLE movie IF EXISTS;

CREATE TABLE movie (
  movie_id    INTEGER IDENTITY PRIMARY KEY,
  code      VARCHAR(10),
  name      VARCHAR(200),
  version   INTEGER
);
