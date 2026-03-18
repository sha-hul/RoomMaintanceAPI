--Create database RoomMaintenance
--Create Schema rvp


--use RoomMaintenance

---------------rvp.trnrequestdetails---------------

--CREATE TABLE rvp.trnrequestdetails (
--    requestId INT PRIMARY KEY,  --same as reqID in trnrequest
--	  empId NVARCHAR(10) NOT NULL,
--    employeeName NVARCHAR(100) NOT NULL,
--    contactNo NVARCHAR(20) NOT NULL,
--    facility INT NOT NULL,
--    apartment INT NOT NULL,
--    roomNo NVARCHAR(20) NOT NULL,
--    category INT NOT NULL,
--    subCategory INT NOT NULL,
--    description NVARCHAR(500) NOT NULL,
--    attachment NVARCHAR(400) NULL,

--    CONSTRAINT FK_RequestDetails_Request
--        FOREIGN KEY (requestId) REFERENCES rvp.trnrequest(requestId)
--);


---------------rvp.trnrequest---------------

--CREATE TABLE rvp.trnrequest (
--    requestId INT IDENTITY(1,1) PRIMARY KEY,
--    statusId INT NOT NULL,   -- normalized FK
--    updatedBy NVARCHAR(100) NOT NULL,
--    dtTransaction DATETIME NOT NULL DEFAULT SYSDATETIME(),
--    updatedAt DATETIME NOT NULL DEFAULT SYSDATETIME(),

--    CONSTRAINT FK_Request_Status
--        FOREIGN KEY (statusId) REFERENCES rvp.mstStatus(statusId)
--);

---------------rvp.mstStatus---------------

--CREATE TABLE rvp.mstStatus (
--    statusId INT IDENTITY(1,1) PRIMARY KEY,
--    statusName NVARCHAR(50) NOT NULL UNIQUE
--);


---------------rvp.mstStatus--------------- Insert Query

--INSERT INTO rvp.mstStatus (statusName) VALUES ('Pending');
--INSERT INTO rvp.mstStatus (statusName) VALUES ('InProcess');
--INSERT INTO rvp.mstStatus (statusName) VALUES ('OnHold');
--INSERT INTO rvp.mstStatus (statusName) VALUES ('Completed');
--INSERT INTO rvp.mstStatus (statusName) VALUES ('Closed');
--INSERT INTO rvp.mstStatus (statusName) VALUES ('ReOpen');


--select * from rvp.mstStatus ORDER BY 1


--CREATE TABLE rvp.trnErrorLog (
--    Id INT IDENTITY(1,1) PRIMARY KEY,
--    ApiName NVARCHAR(200) NOT NULL,              
--    ControllerName NVARCHAR(100) NULL,               
--    RequestPayload NVARCHAR(MAX) NULL,           
--    ErrorMessage NVARCHAR(MAX) NOT NULL,         
--    ErrorCode NVARCHAR(50) NULL,                 
--    LoggedBy NVARCHAR(100) NULL,                 
--    LoggedAt DATETIME NULL
--);

--ALTER TABLE rvp.trnErrorLog
--ALTER COLUMN ;

--select * from rvp.trnErrorLog

---------------Employee Master---------------

--CREATE TABLE [rvp].[EmployeeMaster] (
--    EmpId NVARCHAR(50) NOT NULL PRIMARY KEY,
--    Password NVARCHAR(100) NOT NULL,
--    Role NVARCHAR(50) NOT NULL
--);

---------------Employee Master--------------- Insert Query

--INSERT INTO [rvp].[EmployeeMaster] (EmpId, Password, Role)
--VALUES 
--('EMP001', 'pass123', 'Admin'),
--('EMP002', 'pass456', 'User');

--update rvp.employeemaster set password = 'HUWY0ZSbR/fyERNLY57DIjjOcwhqg8L3RXE7PxL4F+U=' where empid= 'EMP002'