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

--update rvp.mstStatus set statusName = 'Rejected' where statusId = 4

ALTER TABLE rvp.trnrequest
ADD admin NVARCHAR(200),
    adminRemark NVARCHAR(1000),
    technician NVARCHAR(200);

sp_help 'rvp.trnrequestdetails'