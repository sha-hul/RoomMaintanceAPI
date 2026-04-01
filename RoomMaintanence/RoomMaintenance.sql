--use RoomMaintenance

SELECT 
    req.requestId,
    req.statusId,
    req.updatedBy,
    req.dtTransaction,
    req.updatedAt,

    reqD.empId,
    reqD.employeeName,
    reqD.contactNo,

    FM.FacilityName      AS facility,
    LM.LocationName      AS Location,
    AM.ApartmentName     AS Apartment,
    CM.CategoryName      AS category,
    SCM.SubCategoryName  AS subCategory,

    reqD.description,
    reqD.attachment

FROM rvp.trnrequest req
LEFT JOIN rvp.trnrequestdetails reqD 
       ON req.requestId = reqD.requestId

LEFT JOIN rvp.FacilityMaster FM
       ON reqD.facility = FM.Id

LEFT JOIN rvp.LocationMaster LM
       ON reqD.location = LM.Id

LEFT JOIN rvp.ApartmentMaster AM
       ON reqD.Apartment = AM.Id

LEFT JOIN rvp.CategoryMaster CM
       ON reqD.category = CM.Id

LEFT JOIN rvp.SubCategoryMaster SCM
       ON reqD.subCategory = SCM.Id

ORDER BY req.requestId ASC;

select * from rvp.trnrequest req

--select * from rvp.trnrequestdetails

--select * from rvp.trnErrorLog

select * from rvp.FacilityMaster

select * from rvp.LocationMaster where IsActive =1

select * from rvp.ApartmentMaster

select * from rvp.CategoryMaster

select * from rvp.SubCategoryMaster

select * from rvp.EmployeeMaster

--update rvp.trnrequestdetails set empid = 'EMP001' where empid = 'C2064'

select * from rvp.mstStatus order by 1


--update rvp.mstStatus set statusName = 'InProgress' where statusId = 2

--ALTER TABLE rvp.trnrequest
--ADD admin NVARCHAR(200),
--    adminRemark NVARCHAR(1000),
--    technician NVARCHAR(200);

--SELECT 
--    FM.FacilityName    AS Facility,
--    LM.LocationName    AS Location,
--    AM.ApartmentName   AS Apartment,
--    AM.EsubscriptionID AS SubscriptionId,
--    AM.RoomCount       AS RoomCount,
--    AM.IsActive        AS IsActive,
--    LM.Gym             AS Gym,
--    LM.Pool            AS Pool
--FROM rvp.FacilityMaster FM
--LEFT JOIN rvp.LocationMaster LM          -- Step 1: FM → LM (one-to-many)
--       ON LM.FacilityID = FM.Id
--LEFT JOIN rvp.ApartmentMaster AM         -- Step 2: LM → AM (one-to-many)
--       ON AM.LocationID = LM.Id
--WHERE FM.Id = 4
--  AND LM.Id = 5
--  AND AM.Id  = 6

--sp_help 'rvp.EmployeeMaster'

--ALTER TABLE rvp.EmployeeMaster
--ADD EmpName NVARCHAR(200),
--    Mail NVARCHAR(300),
--    dtTransaction DateTime Default GetDate();

--update rvp.trnrequest set statusId = 1 where requestId= 12