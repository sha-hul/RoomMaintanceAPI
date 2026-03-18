using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomMaintenanceAPI.DTO;
using RoomMaintenanceAPI.Models;

namespace RoomMaintenanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryMasterController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SubCategoryMasterController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SubCategoryMaster
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.SubCategoryMaster.ToListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "GetAllSubCategory", "SubCategoryMaster", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation 
            }
        }

        // POST: api/SubCategoryMaster
        [HttpPost("Create")]
        public async Task<IActionResult> Create(SubCategoryCreateDto dto)
        {
            try
            {
                var subCategory = new SubCategoryMaster
                {
                    CategoryID = dto.CategoryId,
                    SubCategoryName = dto.Name,
                    IsActive = dto.Status,
                    CreatedBy = "admin", //#Shahul# EmpID JWT Token Implementation 
                    CreatedDate = DateTime.Now
                };

                _context.SubCategoryMaster.Add(subCategory);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "CreateSubCategory", "SubCategoryMaster", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation
            }
            return Ok(new { message = "SubCategory added successfully", status = true });
        }

        // PUT: api/SubCategoryMaster/{id}
        [HttpPut("UpdateSubCategory/{id}")]
        public async Task<IActionResult> Update(int id, SubCategoryUpdateDto dto)
        {
            try
            {
                var subCategory = await _context.SubCategoryMaster.FindAsync(id);
                if (subCategory == null)
                    return NotFound();

                subCategory.CategoryID = dto.CategoryId;
                subCategory.SubCategoryName = dto.Name;
                subCategory.UpdatedBy = "admin"; //#Shahul# EmpID JWT Token Implementation 
                subCategory.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(new { message = "SubCategory updated successfully", status = true });
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "UpdateSubCategory", "SubCategoryMaster", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation
            }
        }

        [HttpPatch("UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, SubCategoryStatusDto dto)
        {
            try
            {
                var subCategory = await _context.SubCategoryMaster.FindAsync(id);
                if (subCategory == null)
                    return NotFound();

                subCategory.IsActive = dto.Status;
                subCategory.UpdatedBy = "admin"; //#Shahul# EmpID JWT Token Implementation 
                subCategory.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Status updated successfully", status = true });
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "UpdateStatusSubCategory", "SubCategoryMaster", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation
            }
        }
    }
}