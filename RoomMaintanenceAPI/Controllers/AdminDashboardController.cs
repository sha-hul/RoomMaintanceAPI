using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RoomMaintenanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminDashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("getRequestdetails")]
        public async Task<IActionResult> GetRequestDetails([FromQuery] string empId)
        {
            try
            {
                var list = await (
                    from req in _context.TrnRequest
                    join reqD in _context.TrnRequestDetails
                        on req.RequestId equals reqD.RequestId into reqJoin
                    from reqD in reqJoin.DefaultIfEmpty()

                    join FM in _context.FacilityMaster
                        on reqD.Facility equals FM.Id into fmJoin
                    from FM in fmJoin.DefaultIfEmpty()

                    join LM in _context.LocationMaster
                        on reqD.Location equals LM.Id into lmJoin
                    from LM in lmJoin.DefaultIfEmpty()

                    join AM in _context.ApartmentMaster
                        on reqD.Apartment equals AM.Id into amJoin
                    from AM in amJoin.DefaultIfEmpty()

                    join CM in _context.CategoryMaster
                        on reqD.Category equals CM.Id into cmJoin
                    from CM in cmJoin.DefaultIfEmpty()

                    join SCM in _context.SubCategoryMaster
                        on reqD.SubCategory equals SCM.Id into scmJoin
                    from SCM in scmJoin.DefaultIfEmpty()

                    orderby req.DtTransaction descending

                    select new
                    {
                        id = req.RequestId,
                        requestId = "REQ-" + req.RequestId,
                        facility = FM.FacilityName,
                        location = LM.LocationName,
                        apartment = AM.ApartmentName,
                        requestDate = req.DtTransaction.ToString("dd-MM-yyyy"),
                        category = CM.CategoryName,
                        subCategory = SCM.SubCategoryName,
                        employeeName = reqD.EmployeeName,
                        contactNo = reqD.ContactNo,
                        description = reqD.Description,
                        status = req.StatusId,
                        admin = req.Admin,
                        adminRemarks = req.AdminRemark,
                        technician = req.Technician,
                    }
                ).ToListAsync();

                // Apply status mapping AFTER EF query
                var result = list.Select(x => new
                {
                    x.id,
                    x.requestId,
                    x.facility,
                    x.location,
                    x.apartment,
                    x.requestDate,
                    x.category,
                    x.subCategory,
                    x.employeeName,
                    x.contactNo,
                    x.description,
                    x.admin,
                    x.adminRemarks,
                    x.technician,
                    status = getStatusbyID(x.status)
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        private static string getStatusbyID(int statusId)
        {
            switch (statusId)
            {
                case 1: return "Pending";
                case 2: return "InProgress";
                case 3: return "OnHold";
                case 4: return "Rejected";
                case 5: return "Closed";
                case 6: return "ReOpen";
                default: return "Unknown";
            }
        }
    }
}