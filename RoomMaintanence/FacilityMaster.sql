-----------------rvp.FacilityMaster-----------------

--CREATE TABLE rvp.FacilityMaster (
--    Id INT IDENTITY(1,1) PRIMARY KEY,
--    FacilityName NVARCHAR(100) Unique NOT NULL,
--    IsActive BIT NOT NULL DEFAULT 1,
--    CreatedBy NVARCHAR(200) NOT NULL,
--    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
--    UpdatedBy NVARCHAR(200) NULL,
--    UpdatedDate DATETIME NULL
--);

-----------------rvp.LocationMaster-----------------

--CREATE TABLE rvp.LocationMaster (
--    Id INT IDENTITY(1,1) PRIMARY KEY,
--    FacilityID INT NOT NULL,
--    LocationName NVARCHAR(100) NOT NULL,
--    IsActive BIT NOT NULL DEFAULT 1,
--    CreatedBy NVARCHAR(200) NOT NULL,
--    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
--    UpdatedBy NVARCHAR(200) NULL,
--    UpdatedDate DATETIME NULL,

--    CONSTRAINT FK_Location_Facility
--        FOREIGN KEY (FacilityID) REFERENCES rvp.FacilityMaster(Id)
--);

--ALTER TABLE rvp.LocationMaster 
--ADD 
--    Gym BIT NOT NULL DEFAULT 0,
--    Pool BIT NOT NULL DEFAULT 0;

-----------------rvp.ApartmentMaster-----------------

--CREATE TABLE rvp.ApartmentMaster (
--    Id INT IDENTITY(1,1) PRIMARY KEY,
--    LocationID INT NOT NULL,
--    ApartmentName NVARCHAR(100) NOT NULL,
--    EsubscriptionID NVARCHAR(100) NOT NULL,
--    IsActive BIT NOT NULL DEFAULT 1,
--    CreatedBy NVARCHAR(200) NOT NULL,
--    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
--    UpdatedBy NVARCHAR(200) NULL,
--    UpdatedDate DATETIME NULL,

--    CONSTRAINT FK_Apartment_Location
--        FOREIGN KEY (LocationID) REFERENCES rvp.LocationMaster(Id)
--);

--ALTER TABLE rvp.ApartmentMaster
--ALTER COLUMN RoomCount NVARCHAR(100) NOT NULL;

-----------------rvp.CategoryMaster-----------------

--CREATE TABLE rvp.CategoryMaster (
--    Id INT IDENTITY(1,1) PRIMARY KEY,
--    CategoryName NVARCHAR(100) Unique NOT NULL,
--    IsActive BIT NOT NULL DEFAULT 1,
--    CreatedBy NVARCHAR(200) NOT NULL,
--    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
--    UpdatedBy NVARCHAR(200) NULL,
--    UpdatedDate DATETIME NULL
--);

-----------------rvp.SubCategoryMaster-----------------

--CREATE TABLE rvp.SubCategoryMaster (
--    Id INT IDENTITY(1,1) PRIMARY KEY,
--    CategoryID INT NOT NULL,
--    SubCategoryName NVARCHAR(100) NOT NULL,
--    IsActive BIT NOT NULL DEFAULT 1,
--    CreatedBy NVARCHAR(200) NOT NULL,
--    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
--    UpdatedBy NVARCHAR(200) NULL,
--    UpdatedDate DATETIME NULL,

--    CONSTRAINT FK_SubCategory_Category
--        FOREIGN KEY (CategoryID) REFERENCES rvp.CategoryMaster(Id),
--);

--SELECT * FROM rvp.SubCategoryMaster;