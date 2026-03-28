namespace RoomMaintenanceAPI.DTO
{
    public class MaintenanceRequestDto
    {
        public int Facility { get; set; }
        public int Location { get; set; }
        public int Apartment { get; set; }
        public int Category { get; set; }
        public int SubCategory { get; set; }
        public string Description { get; set; }
        public string? Attachment { get; set; }
        public string EmployeeName { get; set; }
        public string ContactNo { get; set; }
        public string UpdatedBy { get; set; }
        public string EmpId { get; set; }
    }
    public class FacilityCreateDto
    {
        public string Name { get; set; }
        public bool Status { get; set; }
    }
    public class FacilityUpdateDto
    {
        public string Name { get; set; }
    }
    public class FacilityStatusDto
    {
        public bool Status { get; set; }
    }
    public class LocationCreateDto
    {
        public int FacilityId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public bool Gym { get; set; }
        public bool Pool { get; set; }
    }
    public class LocationUpdateDto
    {
        public int FacilityId { get; set; }
        public string Name { get; set; }
        public bool Gym { get; set; }
        public bool Pool { get; set; }
    }
    public class LocationStatusDto
    {
        public bool Status { get; set; }
    }
    //--------Apartment Master--------
    public class ApartmentCreateDto
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string EsubscriptionID { get; set; }
        public string RoomCount { get; set; }
        public bool Status { get; set; }
    }
    public class ApartmentUpdateDto
    {
        public int LocationId { get; set; }
        public string EsubscriptionID { get; set; }
        public string RoomCount { get; set; }
        public string Name { get; set; }
    }
    public class ApartmentStatusDto
    {
        public bool Status { get; set; }
    }

    public class CategoryCreateDto
    {
        public string Name { get; set; }
        public bool Status { get; set; }
    }
    public class CategoryUpdateDto
    {
        public string Name { get; set; }
    }
    public class CategoryStatusDto
    {
        public bool Status { get; set; }
    }
    public class SubCategoryCreateDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }
    public class SubCategoryUpdateDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
    public class SubCategoryStatusDto
    {
        public bool Status { get; set; }
    }
    public class LoginDTO
    {
        public string EmpId { get; set; }
        public string Password { get; set; }
    }
}