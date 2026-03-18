using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RoomMaintenanceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpDashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmpDashboardController(AppDbContext context)
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

                    join AM in _context.ApartmentMaster
                        on reqD.Apartment equals AM.Id into amJoin
                    from AM in amJoin.DefaultIfEmpty()

                    join SCM in _context.SubCategoryMaster
                        on reqD.SubCategory equals SCM.Id into scmJoin
                    from SCM in scmJoin.DefaultIfEmpty()
                        
                    where reqD.EmpId == empId
                    orderby req.RequestId ascending

                    select new
                    {
                        id = req.RequestId,
                        requestId = "REQ-" + req.RequestId,
                        apartment = AM.ApartmentName,
                        requestDate = req.DtTransaction,
                        subCategory = SCM.SubCategoryName,
                        status = req.StatusId
                    }
                ).ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }

}