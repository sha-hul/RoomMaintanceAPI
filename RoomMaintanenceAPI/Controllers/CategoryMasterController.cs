using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomMaintenanceAPI.DTO;
using RoomMaintenanceAPI.Models;

namespace RoomMaintenanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryMasterController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryMasterController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CategoryMaster
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.CategoryMaster.ToListAsync();//#Shahul# what's this means in entity framework
                return Ok(list);
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "GetAllCategory", "CategoryMaster", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation 
            }
        }

        // POST: api/CategoryMaster
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CategoryCreateDto dto)
        {
            try
            {
                var category = new CategoryMaster
                {
                    CategoryName = dto.Name,
                    IsActive = dto.Status,
                    CreatedBy = "admin", //#Shahul# EmpID JWT Token Implementation 
                    CreatedDate = DateTime.Now
                };

                _context.CategoryMaster.Add(category);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "CreateCategory", "CategoryMaster", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation 
            }
            return Ok(new { message = "Category added successfully", status = true });
        }

        // PUT: api/CategoryMaster/{id}
        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> Update(int id, CategoryUpdateDto dto)
        {
            try
            {
                var category = await _context.CategoryMaster.FindAsync(id);//#Shahul# what's this means in entity framework
                if (category == null)
                    return NotFound();

                category.CategoryName = dto.Name;
                category.UpdatedBy = "admin"; //#Shahul# EmpID JWT Token Implementation 
                category.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Category updated successfully", status = true });
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "UpdateCategory", "CategoryMaster", "400", "C2064"); //#Shahul# EmpID JWT Token Implementation 
            }
        }


        [HttpPatch("UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, CategoryStatusDto dto)
        {
            try
            {
                var category = await _context.CategoryMaster.FindAsync(id);
                if (category == null)
                    return NotFound();

                category.IsActive = dto.Status;
                category.UpdatedBy = "admin";
                category.UpdatedDate = DateTime.Now;

                // If category is deactivated, deactivate all subCategorys under it
                if (!dto.Status)
                {
                    var subCategorys = _context.SubCategoryMaster
                                             .Where(a => a.CategoryID == id && a.IsActive == true)
                                             .ToList();

                    foreach (var apt in subCategorys)
                    {
                        apt.IsActive = false;
                        apt.UpdatedBy = "admin";
                        apt.UpdatedDate = DateTime.Now;
                    }
                }

                //If category is activated, activate all subCategorys under need to ask..  #Doubt#
                await _context.SaveChangesAsync();

                return Ok(new { message = "Status updated successfully", status = true });
            }
            catch (Exception ex)
            {
                return await ErrorHandler.HandleExceptionAsync(ex, null, _context, null, "UpdateStatusCategory", "CategoryMaster", "400", "C2064");
            }
        }

    }
}