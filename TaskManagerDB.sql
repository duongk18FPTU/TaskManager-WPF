CREATE DATABASE TaskManagerDB;
GO

USE TaskManagerDB;
GO

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL, 
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Role NVARCHAR(20) DEFAULT 'User'
);
GO

CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE
);
GO

CREATE TABLE Tasks (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    DueDate DATETIME NOT NULL,
    Status NVARCHAR(20) CHECK (Status IN ('Pending', 'In Progress', 'Completed')) NOT NULL DEFAULT 'Pending',
    UserId INT NOT NULL,
    CategoryId INT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE SET NULL
);
GO

INSERT INTO Categories (Name) VALUES 
(N'Công việc cá nhân'),       
(N'Việc nhà'),                
(N'Sức khỏe & Thể dục'),      
(N'Học tập'),                 
(N'Sở thích'),                
(N'Công việc văn phòng'),      
(N'Cuộc họp'),                
(N'Báo cáo & Kế hoạch'),       
(N'Email & Liên hệ'),          
(N'Công việc theo dự án'),     
(N'Phát triển phần mềm'),      
(N'Marketing'),                
(N'Kinh doanh'),               
(N'Công việc tài chính & quản lý'), 
(N'Quản lý tài chính'),        
(N'Hành chính & Nhân sự');     
GO

SELECT * FROM Categories;
GO
