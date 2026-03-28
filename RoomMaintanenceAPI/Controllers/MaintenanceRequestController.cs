using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomMaintenanceAPI.DTO;
using RoomMaintenanceAPI.Models;

namespace RoomMaintenanceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceRequestController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MaintenanceRequestController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("SubmitRequest")]
        public async Task<IActionResult> SubmitRequest([FromBody] MaintenanceRequestDto dto)
        {
            // Start a database transaction
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Insert into trnrequest
                var request = new TrnRequest
                {
                    StatusId = 1, //Pending
                    UpdatedBy = dto.UpdatedBy,
                    DtTransaction = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.TrnRequest.Add(request);
                await _context.SaveChangesAsync(); // generates RequestId

                // 2. Insert into trnrequestdetails
                var details = new TrnRequestDetails
                {
                    RequestId = request.RequestId,
                    EmpId = dto.EmpId,
                    EmployeeName = dto.EmployeeName,
                    ContactNo = dto.ContactNo,
                    Facility = dto.Facility,
                    Location = dto.Location,
                    Apartment = dto.Apartment,
                    Category = dto.Category,
                    SubCategory = dto.SubCategory,
                    Description = dto.Description,
                    Attachment = dto.Attachment
                };

                _context.TrnRequestDetails.Add(details);
                await _context.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Request submitted successfully",
                    requestId = request.RequestId,
                    status = true
                });
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, transaction, _context, dto, "SubmitRequest", "MaintenanceRequest", "400", dto.EmpId);
            }

        }

        [HttpGet("getFacilities")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.FacilityMaster.Where(f => f.IsActive == true).Select(f => new { id = f.Id, name = f.FacilityName }).ToListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "GetFacilities", "MaintenanceRequest", "400", "C2064");
            }
        }

        [HttpGet("locations/{facilityId}")]
        public async Task<IActionResult> GetLocationsByFacility(int facilityId)
        {
            try
            {
                var list = await _context.LocationMaster
                .Where(a => a.FacilityID == facilityId && a.IsActive == true)
                .Select(a => new { id = a.Id, facilityId = a.FacilityID, name = a.LocationName })
                .ToListAsync();


                return Ok(list);
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "GetApartmentsByFacility", "MaintenanceRequest", "400", "C2064");
            }
        }

        [HttpGet("apartments/{locationId}")]
        public async Task<IActionResult> GetApartmentsByLocation(int locationId)
        {
            try
            {
                var list = await _context.ApartmentMaster.Where(a => a.LocationID == locationId && a.IsActive == true)
                                         .Select(a => new { id = a.Id, name = a.ApartmentName }).ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "GetApartmentsByLocation", "MaintenanceRequest", "400", "C2064");
            }
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var list = await _context.CategoryMaster.Where(c => c.IsActive == true).Select(c => new { id = c.Id, name = c.CategoryName }).ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "GetCategories", "MaintenanceRequest", "400", "C2064");
            }
        }

        [HttpGet("subcategories/{categoryId}")]
        public async Task<IActionResult> GetSubCategoriesByCategory(int categoryId)
        {
            try
            {
                var list = await _context.SubCategoryMaster.Where(s => s.CategoryID == categoryId && s.IsActive == true).Select(s => new { id = s.Id, categoryId = s.CategoryID, name = s.SubCategoryName }).ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "GetSubCategoriesByCategory", "MaintenanceRequest", "400", "C2064");
            }
        }
    }
}