namespace RoomMaintenanceAPI.Models
{
    public class SubCategoryMaster
    {
        public int Id { get; set; }
        public int CategoryID { get; set; }
        public string SubCategoryName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}