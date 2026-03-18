

namespace RoomMaintenanceAPI.Models
{
    public class LocationMaster
    {
        public int Id { get; set; }
        public int FacilityID { get; set; }
        public string LocationName { get; set; }
        public bool IsActive { get; set; }
        public bool Gym { get; set; }
        public bool Pool { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        // Navigation properties
        public FacilityMaster Facility { get; set; }
        public ICollection<ApartmentMaster> Apartments { get; set; }
    }
}