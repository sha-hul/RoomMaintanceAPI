using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomMaintenanceAPI.DTO;
using RoomMaintenanceAPI.Models;

namespace RoomMaintenanceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApartmentMicrositeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApartmentMicrositeController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("getApartmentDetails")]
        public async Task<IActionResult> GetApartmentDetails([FromQuery] ApartmentDetailsDTO dto)
        {
            try
            {
                int faciId = Convert.ToInt32(dto.faciId);
                int locId = Convert.ToInt32(dto.locId);
                int appId = Convert.ToInt32(dto.appId);

                var list = await (
                 from FM in _context.FacilityMaster
                 join LM in _context.LocationMaster
                     on FM.Id equals LM.FacilityID into lmJoin
                 from LM in lmJoin.DefaultIfEmpty()
                 join AM in _context.ApartmentMaster
                     on LM.Id equals AM.LocationID into amJoin
                 from AM in amJoin.DefaultIfEmpty()
                 where FM.Id == faciId
                    && LM.Id == locId
                    && AM.Id == appId
                 select new
                 {
                     facility = FM.FacilityName,
                     location = LM.LocationName,
                     apartment = AM.ApartmentName,
                     subscriptionId = AM.EsubscriptionID,
                     roomCount = AM.RoomCount,
                     isActive = AM.IsActive,
                     gym = LM.Gym,
                     pool = LM.Pool
                 }
             ).ToListAsync();

                var result = list.Select(x => new
                {
                    x.facility,
                    x.location,
                    x.apartment,
                    x.subscriptionId,
                    x.roomCount,
                    x.isActive,
                    amenities = new List<string>()
                        .Concat(x.gym ? new[] { "Gym" } : Array.Empty<string>())
                        .Concat(x.pool ? new[] { "Pool" } : Array.Empty<string>())
                        .ToList()
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpGet("getApartmentRequestDetails/{appId}")]
        public async Task<IActionResult> GetApartmentRequestDetails([FromRoute] int appId)
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
                    join CM in _context.CategoryMaster
                        on reqD.Category equals CM.Id into cmJoin
                    from CM in cmJoin.DefaultIfEmpty()
                    join SCM in _context.SubCategoryMaster
                        on reqD.SubCategory equals SCM.Id into scmJoin
                    from SCM in scmJoin.DefaultIfEmpty()
                    join ST in _context.MstStatus
                        on req.StatusId equals ST.StatusId into stJoin
                    from ST in stJoin.DefaultIfEmpty()
                    where AM.Id == appId 
                       && req.StatusId != 7
                    orderby req.DtTransaction descending
                    select new
                    {
                        id = req.RequestId,
                        category = CM.CategoryName,
                        subCategory = SCM.SubCategoryName,
                        date = req.DtTransaction,
                        status = ST.StatusName,
                        description = reqD.Description,
                        adminRemark = req.AdminRemark,
                        attachment = reqD.Attachment
                    }
                ).ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpPut("cancelMaintenanceRequest/{reqId}")]
        public async Task<IActionResult> CancelMaintenanceRequest([FromRoute]int reqId)
        {
            try
            {
                var request = await _context.TrnRequest.FindAsync(reqId);
                if (request == null)
                    return NotFound();

                request.StatusId = 7;
                request.UpdatedBy = "user";
                request.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "CancelMaintenanceRequest", "ApartmentMicrosite", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation
            }
            return Ok(new { message = "Request Cancelled successfully", status = true });
        }

        [HttpPost("decrypt")]
        public IActionResult DecryptQr([FromBody] QrRequest req)
        {
            var fac = CryptoHelper.Decrypt(req.facid);
            var loc = CryptoHelper.Decrypt(req.locid);
            var apart = CryptoHelper.Decrypt(req.apart);

            return Ok(new
            {
                facid = fac,
                locid = loc,
                apart
            });
        }
    }
}