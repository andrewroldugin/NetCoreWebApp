CREATE DATABASE persons;
USE persons;

CREATE TABLE tblpersons (
id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
first_name VARCHAR(30) NOT NULL,
last_name VARCHAR(30) NOT NULL,
phone VARCHAR(13) NOT NULL
);

INSERT INTO tblpersons VALUES (0, "Joe", "Satriani", "+380954564545");
INSERT INTO tblpersons VALUES (0, "Steve", "Vai", "+705145236987");
INSERT INTO tblpersons VALUES (0, "John", "Petrucci", "+555145236446");