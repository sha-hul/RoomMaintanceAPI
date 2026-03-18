--use RoomMaintenance

select * from rvp.trnrequest req
left join rvp.trnrequestdetails reqD on req.requestId = reqD.requestId
where req.requestId = 3