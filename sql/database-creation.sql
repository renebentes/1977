CREATE DATABASE IdentityWithDapper;

GO

USE IdentityWithDapper;

GO

CREATE TABLE ApplicationUser
(
    Id INT NOT NULL PRIMARY KEY IDENTITY,
    UserName NVARCHAR(256) NOT NULL,
    NormalizedUserName NVARCHAR(256) NOT NULL,
    Email NVARCHAR(256) NULL,
    NormalizedEmail NVARCHAR(256) NULL,
    EmailConfirmed BIT NOT NULL,
    PasswordHash NVARCHAR(MAX) NULL,
    PhoneNumber NVARCHAR(50) NULL,
    PhoneNumberConfirmed BIT NOT NULL,
    TwoFactorEnabled BIT NOT NULL
)

GO

CREATE INDEX IX_ApplicationUser_NormalizedUserName ON ApplicationUser (NormalizedUserName);

GO

CREATE INDEX IX_ApplicationUser_NormalizedEmail ON ApplicationUser (NormalizedEmail);

GO

CREATE TABLE ApplicationRole
(
    Id INT NOT NULL PRIMARY KEY IDENTITY,
    Name NVARCHAR(256) NOT NULL,
    NormalizedName NVARCHAR(256) NOT NULL
);

GO

CREATE INDEX IX_ApplicationRole_NormalizedName ON ApplicationRole (NormalizedName);

GO

CREATE TABLE ApplicationUserRole
(
    UserId INT NOT NULL,
    RoleId INT NOT NULL
    PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_ApplicationUserRole_User FOREIGN KEY (UserId) REFERENCES ApplicationUser(Id),
    CONSTRAINT FK_ApplicationUserRole_Role FOREIGN KEY (RoleId) REFERENCES ApplicationRole(Id)
);

GO