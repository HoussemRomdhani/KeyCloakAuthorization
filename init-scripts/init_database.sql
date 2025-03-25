USE master;
GO

CREATE DATABASE Bookstore;
GO

USE Bookstore;
GO

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FirstName NVARCHAR(100) NULL,
    LastName NVARCHAR(100) NULL
);


CREATE TABLE Books (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    AuthorId INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    Year INT NOT NULL,
    FOREIGN KEY (AuthorId) REFERENCES Users(Id)
);


CREATE TABLE Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) UNIQUE NOT NULL
);

CREATE TABLE UserRoles (
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);