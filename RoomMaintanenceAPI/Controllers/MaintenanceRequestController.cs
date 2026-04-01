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
        public async Task<IActionResult> SubmitRequest([FromForm] MaintenanceRequestDto dto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Handle file upload
                string? savedFileName = null;
                if (dto.Attachment != null && dto.Attachment.Length > 0)
                {
                    // Validate file type
                    var allowedExtensions = new[] {
                        ".jpg", ".jpeg", ".png", ".webp", ".heic",  // Images
                        ".pdf",                                      // PDF
                        ".doc", ".docx",                             // Word
                        ".xls", ".xlsx",                             // Excel
                        ".mp4", ".mov", ".avi"                       // Videos
                    };
                    var extension = Path.GetExtension(dto.Attachment.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                        return BadRequest(new { message = "Invalid file type", status = false });

                    // Validate file size (10MB)
                    if (dto.Attachment.Length > 10 * 1024 * 1024)
                        return BadRequest(new { message = "File size exceeds 10MB", status = false });

                    // Generate unique file name to avoid overwrite
                    var uniqueFileName = $"{Guid.NewGuid()}_{dto.Attachment.FileName}";

                    // Save to Upload folder
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Upload");
                    if (!Directory.Exists(uploadFolder))
                        Directory.CreateDirectory(uploadFolder);

                    var filePath = Path.Combine(uploadFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.Attachment.CopyToAsync(stream);
                    }

                    savedFileName = uniqueFileName;
                }

                // 2. Insert into trnrequest
                var request = new TrnRequest
                {
                    StatusId = 1,
                    UpdatedBy = dto.UpdatedBy,
                    DtTransaction = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.TrnRequest.Add(request);
                await _context.SaveChangesAsync();

                // 3. Insert into trnrequestdetails
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
                    Attachment = savedFileName
                };
                _context.TrnRequestDetails.Add(details);
                await _context.SaveChangesAsync();

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