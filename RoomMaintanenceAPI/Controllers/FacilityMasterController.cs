using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomMaintenanceAPI.DTO;
using RoomMaintenanceAPI.Models;

namespace RoomMaintenanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilityMasterController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FacilityMasterController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/FacilityMaster
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.FacilityMaster.ToListAsync();//#Shahul# what's this means in entity framework
                return Ok(list);
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "GetAllFacility", "FacilityMaster", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation 
            }
        }

        // POST: api/FacilityMaster
        [HttpPost("Create")]
        public async Task<IActionResult> Create(FacilityCreateDto dto)
        {
            try
            {
                var facility = new FacilityMaster
                {
                    FacilityName = dto.Name,
                    IsActive = dto.Status,
                    CreatedBy = "admin", //#Shahul# EmpID JWT Token Implementation 
                    CreatedDate = DateTime.Now
                };

                _context.FacilityMaster.Add(facility);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "CreateFacility", "FacilityMaster", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation 
            }
            return Ok(new { message = "Facility added successfully", status = true });
        }

        // PUT: api/FacilityMaster/{id}
        [HttpPut("UpdateFacility/{id}")]
        public async Task<IActionResult> Update(int id, FacilityUpdateDto dto)
        {
            try
            {
                var facility = await _context.FacilityMaster.FindAsync(id);//#Shahul# what's this means in entity framework
                if (facility == null)
                    return NotFound();

                facility.FacilityName = dto.Name;
                facility.UpdatedBy = "admin"; //#Shahul# EmpID JWT Token Implementation 
                facility.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Facility updated successfully", status = true });
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "UpdateFacility", "FacilityMaster", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation 
            }
        }


        [HttpPatch("UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, FacilityStatusDto dto)
        {
            try
            {
                var facility = await _context.FacilityMaster.FindAsync(id);
                if (facility == null)
                    return NotFound();

                facility.IsActive = dto.Status;
                facility.UpdatedBy = "admin";
                facility.UpdatedDate = DateTime.Now;

                if (!dto.Status)
                {
                    // 1️ Deactivate all Locations under this Facility
                    await _context.LocationMaster
                        .Where(l => l.FacilityID == id)
                        .ExecuteUpdateAsync(s => s
                            .SetProperty(l => l.IsActive, false)
                            .SetProperty(l => l.UpdatedBy, "admin")
                            .SetProperty(l => l.UpdatedDate, DateTime.Now)
                        );

                    // 2️ Deactivate all Apartments under those Locations
                    await _context.ApartmentMaster
                        .Where(a => _context.LocationMaster
                                        .Where(l => l.FacilityID == id)
                                        .Select(l => l.Id)
                                        .Contains(a.LocationID))
                        .ExecuteUpdateAsync(s => s
                            .SetProperty(a => a.IsActive, false)
                            .SetProperty(a => a.UpdatedBy, "admin")
                            .SetProperty(a => a.UpdatedDate, DateTime.Now)
                        );
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Status updated successfully", status = true });
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null,"UpdateStatusFacility", "FacilityMaster", "400", "C2064");
            }
        }



        [HttpGet("locations/{facilityId}")]
        public async Task<IActionResult> GetLocationsByFacility(int facilityId)
        {
            try
            {
                var list = await _context.LocationMaster.Where(l => l.FacilityID == facilityId && l.IsActive == true)
                                         .Select(l => new { id = l.Id, name = l.LocationName }).ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(
                    ex, null, _context, null,
                    "GetLocationsByFacility", "Facility", "400", "C2064"
                );
            }
        }

    }
}