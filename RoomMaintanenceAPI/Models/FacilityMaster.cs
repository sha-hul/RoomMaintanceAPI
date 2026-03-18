namespace RoomMaintenanceAPI.Models
{
    public class FacilityMaster
    {
        public int Id { get; set; }
        public string FacilityName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Navigation
        public ICollection<LocationMaster> Locations { get; set; }
    }

}
