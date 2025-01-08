
CREATE DATABASE StudentTrackerDB;


USE StudentTrackerDB;

CREATE TABLE Students (
    StudentID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Grade NVARCHAR(50) NOT NULL,
    Subject NVARCHAR(100) NOT NULL,
    Marks DECIMAL(5, 2) CHECK (Marks >= 0 AND Marks <= 100),
    Attendance DECIMAL(5, 2) CHECK (Attendance >= 0 AND Attendance <= 100),
    CreatedDate DATETIME DEFAULT GETDATE()
);


CREATE TABLE Subjects (
    SubjectID INT IDENTITY(1,1) PRIMARY KEY,
    SubjectName NVARCHAR(100) NOT NULL
);


INSERT INTO Subjects (SubjectName) VALUES 
('Programming Fundamentals'),
('Iformation Security'),
('Data Structures and Algorithms'),
('Visual Programming'),
('Digital Logic Design');


INSERT INTO Students (Name, Grade, Subject, Marks, Attendance) VALUES
('Arsal Ajmal', 'A', 'Programming Fundamentals', 85.5, 90.0),
('Abdul Wasay', 'B', 'Iformation Security', 75.0, 85.0),
('Faseeh Anjum', 'A', 'Data Structures and Algorithms', 92.0, 95.0),
('Mahad Ghauri', 'A', 'Visual Programming', 60.0, 80.0),
('Mahateer Muhammad', 'B', 'Digital Logic Design', 78.5, 88.0);
