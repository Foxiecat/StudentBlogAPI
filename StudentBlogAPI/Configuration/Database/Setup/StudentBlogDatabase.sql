drop database if exists StudentBlog;
create database StudentBlog;
use StudentBlog;

create user if not exists 'StudentBlogUser'@'localhost' identified by 'Stud3ntBl0g!';
create user if not exists  'StudentBlogUser'@'%' identified by 'Stud3ntBl0g!';

grant all privileges on StudentBlog.* to 'StudentBlogUser'@'%';
grant all privileges on StudentBlog.* to 'StudentBlogUser'@'localhost';

flush privileges;

create table Users
(
    Id varchar(36) primary key,
    UserName varchar(30),
    FirstName varchar(50),
    LastName varchar(100),
    HashedPassword longtext,
    Email varchar(255),
    Created DATETIME(6),
    Updated datetime(6),
    IsAdminUser tinyInt(1)
);

create table Posts
(
    Id varchar(36) primary key,
    UserId varchar(36),
    Title varchar(100),
    Content longtext,
    DatePosted datetime(6),

    foreign key (UserId) references Users(Id)
);

create table Comments
(
    Id varchar(36) primary key,
    PostId varchar(36),
    UserId varchar(36),
    Content longtext,
    DateCommented datetime(6),

    foreign key (PostId) references Posts(Id),
    foreign key (UserId) references Users(Id)
);