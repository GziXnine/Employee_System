CREATE DATABASE CompanyDB_ADO
GO

USE CompanyDB_ADO
GO

CREATE TABLE Departments (
    DepartmentID INT PRIMARY KEY IDENTITY(1, 1),
    DepartmentName NVARCHAR(100) NOT NULL
);

CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY(1, 1),
    EmployeeName NVARCHAR(100) NOT NULL,
    EmployeeSalary INT NOT NULL,
    EmployeeGender VARCHAR(6) NOT NULL,

    DepartmentID INT FOREIGN KEY REFERENCES Departments(DepartmentID)
);

INSERT INTO Departments (DepartmentName) 
VALUES
    ('Human Resources'),
    ('Engineering'),
    ('Marketing'),
    ('Finance'),
    ('IT Support');

INSERT INTO Employees (EmployeeName, EmployeeSalary, EmployeeGender, DepartmentID) 
VALUES
    ('Alice Johnson', 5000, 'Female', 1),  -- HR
    ('Bob Smith', 7000, 'Male', 2),        -- Engineering
    ('Charlie Brown', 5500, 'Male', 3),    -- Marketing
    ('Diana White', 8000, 'Female', 4),    -- Finance
    ('Ethan Carter', 6000, 'Male', 5),     -- IT Support
    ('Fiona Green', 7500, 'Female', 2),    -- Engineering
    ('George Black', 5000, 'Male', 1);     -- HR

-- View Data
SELECT * FROM Departments;
SELECT * FROM Employees;