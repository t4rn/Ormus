
create table UsersRoles(
Id integer identity(1,1) primary key,
Code varchar(16) not null,
Description varchar(128),
CreatedDate datetime2(2) not null default GETDATE(),
Ghost bit default 0
);

select * from UsersRoles;

create table Users(
Id integer identity(1,1) primary key,
RoleId integer references UsersRoles not null,
Login varchar(32) not null,
Password varchar(32) not null,
FirstName varchar(32),
LastName varchar(64),
Email varchar(64) not null,
CreatedDate datetime2(2) not null default getdate(),
UpdatedDate datetime2(2),
Ghost bit not null default 0
);

select * from Users;