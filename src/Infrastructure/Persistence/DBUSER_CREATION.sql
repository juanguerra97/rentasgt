
DROP DATABASE IF EXISTS rentasgt;

CREATE DATABASE rentasgt;

USE rentasgt;

DROP USER IF EXISTS rentasgt;

CREATE USER 'rentasgt'@'%' IDENTIFIED BY 'rentasgt';

GRANT ALL PRIVILEGES ON rentasgt.* TO 'rentasgt'@'%';