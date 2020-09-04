
DROP DATABASE IF EXISTS rentasgt;

CREATE DATABASE rentasgt;

USE rentasgt;

DROP USER IF EXISTS rentasgt;

CREATE USER 'rentasgt'@'%' IDENTIFIED BY 'rentasgt';

GRANT ALL PRIVILEGES ON rentasgt.* TO 'rentasgt'@'%';

DROP FUNCTION IF EXISTS calculateDistance;

DELIMITER //
CREATE FUNCTION calculateDistance(
	aLat DOUBLE, aLon DOUBLE, bLat DOUBLE, bLon DOUBLE )
RETURNS DOUBLE 
BEGIN
	RETURN 111.111 *
		DEGREES(ACOS(LEAST(1.0, COS(RADIANS(aLat))
         * COS(RADIANS(bLat))
         * COS(RADIANS(aLon - bLon))
         + SIN(RADIANS(aLat))
         * SIN(RADIANS(bLat)))));
END//
DELIMITER ;