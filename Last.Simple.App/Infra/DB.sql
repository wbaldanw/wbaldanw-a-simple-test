﻿CREATE TABLE USERS (
	ID BIGINT PRIMARY KEY NOT NULL IDENTITY,
	UserName VARCHAR(255) NOT NULL,
	Password VARCHAR(255) NOT NULL
);

CREATE TABLE Products (
	ID BIGINT PRIMARY KEY NOT NULL IDENTITY,
	Name VARCHAR(255) NOT NULL,
	Price DECIMAL(10,2) NOT NULL,
	Description VARCHAR(max) NOT NULL,
	CreatedBy BIGINT NOT NULL
);