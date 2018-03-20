DROP TABLE job IF EXISTS;

CREATE TABLE job (
  job_id    INTEGER IDENTITY PRIMARY KEY,
  code      VARCHAR(10),
  name      VARCHAR(200),
  version   INTEGER
);
