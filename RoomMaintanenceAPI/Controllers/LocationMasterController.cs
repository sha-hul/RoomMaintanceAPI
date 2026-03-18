using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomMaintenanceAPI.DTO;
using RoomMaintenanceAPI.Models;

namespace RoomMaintenanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationMasterController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LocationMasterController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/LocationMaster
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.LocationMaster.ToListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "GetAllLocation", "LocationMaster", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation 
            }
        }

        // POST: api/LocationMaster
        [HttpPost("Create")]
        public async Task<IActionResult> Create(LocationCreateDto dto)
        {
            try
            {
                var location = new LocationMaster
                {
                    FacilityID = dto.FacilityId,
                    LocationName = dto.Name,
                    IsActive = dto.Status,
                    CreatedBy = "admin", //#Shahul# EmpID JWT Token Implementation 
                    CreatedDate = DateTime.Now,
                    Gym = dto.Gym,
                    Pool = dto.Pool
                };

                _context.LocationMaster.Add(location);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "CreateLocation", "LocationMaster", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation
            }
            return Ok(new { message = "Location added successfully", status = true });
        }

        // PUT: api/LocationMaster/{id}
        [HttpPut("UpdateLocation/{id}")]
        public async Task<IActionResult> Update(int id, LocationUpdateDto dto)
        {
            try
            {
                var location = await _context.LocationMaster.FindAsync(id);
                if (location == null)
                    return NotFound();

                location.FacilityID = dto.FacilityId;
                location.LocationName = dto.Name;
                location.UpdatedBy = "admin"; //#Shahul# EmpID JWT Token Implementation 
                location.UpdatedDate = DateTime.Now;
                location.Gym = dto.Gym;
                location.Pool = dto.Pool;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Location updated successfully", status = true });
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "UpdateLocation", "LocationMaster", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation
            }
        }

        [HttpPatch("UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, LocationStatusDto dto)
        {
            try
            {
                var location = await _context.LocationMaster.FindAsync(id);
                if (location == null)
                    return NotFound();

                location.IsActive = dto.Status;
                location.UpdatedBy = "admin"; //#Shahul# EmpID JWT Token Implementation 
                location.UpdatedDate = DateTime.Now;

                // If location is deactivated, deactivate all apartment under it
                if (!dto.Status)
                {
                    var locations = _context.ApartmentMaster
                                             .Where(a => a.LocationID == id && a.IsActive == true)
                                             .ToList();

                    foreach (var loc in locations)
                    {
                        loc.IsActive = false;
                        loc.UpdatedBy = "admin";
                        loc.UpdatedDate = DateTime.Now;
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Status updated successfully", status = true });
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "UpdateStatusLocation", "LocationMaster", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation
            }
        }


    }
}