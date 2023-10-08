IF db_id('studyzen') IS NULL CREATE DATABASE studyzen;
GO USE studyzen;
CREATE TABLE courses (
    id INT IDENTITY(1, 1) PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    description VARCHAR(200) NOT NULL,
    createdby_user VARCHAR(50) NOT NULL,
    createdby_date DATETIME2 NOT NULL,
    updatedby_user VARCHAR(50) NOT NULL,
    updatedby_date DATETIME2 NOT NULL
);
CREATE TABLE l,
ectures (
    id INT IDENTITY(1, 1) PRIMARY KEY,
    course_id INT NOT NULL,
    FOREIGN KEY (course_id) REFERENCES courses(id),
    name VARCHAR(50) NOT NULL,
    content VARCHAR(2000) NOT NULL,
    createdby_user VARCHAR(50) NOT NULL,
    createdby_date DATETIME2 NOT NULL,
    updatedby_user VARCHAR(50) NOT NULL,
    updatedby_date DATETIME2 NOT NULL
);
CREATE TABLE flashcardsets (
    id INT IDENTITY(1, 1) PRIMARY KEY,
    lecture_id INT,
    FOREIGN KEY (lecture_id) REFERENCES lectures(id),
    name VARCHAR(50) NOT NULL,
    createdby_user VARCHAR(50) NOT NULL,
    createdby_date DATETIME2 NOT NULL,
    updatedby_user VARCHAR(50) NOT NULL,
    updatedby_date DATETIME2 NOT NULL
);
CREATE TABLE flashcards (
    id INT IDENTITY(1, 1) PRIMARY KEY,
    flashcardset_id INT,
    FOREIGN KEY (flashcardset_id) REFERENCES flashcardsets(id),
    question VARCHAR(50) NOT NULL,
    answer VARCHAR(50) NOT NULL,
    createdby_user VARCHAR(50) NOT NULL,
    createdby_date DATETIME2 NOT NULL,
    updatedby_user VARCHAR(50) NOT NULL,
    updatedby_date DATETIME2 NOT NULL
);