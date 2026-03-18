using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomMaintenanceAPI.DTO;
using RoomMaintenanceAPI.Models;

namespace RoomMaintenanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentMasterController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApartmentMasterController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.ApartmentMaster
                    .Include(a => a.Location) 
                    .Select(a => new
                    {
                        id = a.Id,
                        apartmentName = a.ApartmentName,
                        esubscriptionID = a.EsubscriptionID,
                        roomCount = a.RoomCount,
                        isActive = a.IsActive,
                        locationId = a.LocationID,
                        facilityId = a.Location.FacilityID,
                        createdBy = a.CreatedBy,
                        createdDate = a.CreatedDate,
                        updatedBy = a.UpdatedBy,
                        updatedDate = a.UpdatedDate
                    })
                    .ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(
                    ex, null, _context, null,
                    "GetAllApartment", "ApartmentMaster", "400", "C2064"
                );
            }
        }

        // POST: api/ApartmentMaster
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ApartmentCreateDto dto)
        {
            try
            {
                var apartment = new ApartmentMaster
                {
                    LocationID = dto.LocationId,
                    ApartmentName = dto.Name,
                    IsActive = dto.Status,
                    CreatedBy = "admin", //#Shahul# EmpID JWT Token Implementation 
                    CreatedDate = DateTime.Now,
                    EsubscriptionID = dto.EsubscriptionID,
                    RoomCount = dto.RoomCount
                };

                _context.ApartmentMaster.Add(apartment);
                await _context.SaveChangesAsync();

            }   
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "CreateApartment", "ApartmentMaster", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation
            }
            return Ok(new { message = "Apartment added successfully", status = true });
        }

        // PUT: api/ApartmentMaster/{id}
        [HttpPut("UpdateApartment/{id}")]
        public async Task<IActionResult> Update(int id, ApartmentUpdateDto dto)
        {
            try
            {
                var apartment = await _context.ApartmentMaster.FindAsync(id);
                if (apartment == null)
                    return NotFound();

                apartment.LocationID= dto.LocationId;
                apartment.ApartmentName = dto.Name;
                apartment.UpdatedBy = "admin"; //#Shahul# EmpID JWT Token Implementation 
                apartment.UpdatedDate = DateTime.Now;
                apartment.EsubscriptionID = dto.EsubscriptionID;
                apartment.RoomCount = dto.RoomCount;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Apartment updated successfully", status = true });
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "UpdateApartment", "ApartmentMaster", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation
            }
        }

        [HttpPatch("UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, ApartmentStatusDto dto)
        {
            try
            {
                var apartment = await _context.ApartmentMaster.FindAsync(id);
                if (apartment == null)
                    return NotFound();

                apartment.IsActive = dto.Status;
                apartment.UpdatedBy = "admin"; //#Shahul# EmpID JWT Token Implementation 
                apartment.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Status updated successfully", status = true });
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "UpdateStatusApartment", "ApartmentMaster", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation
            }
        }
    }
}