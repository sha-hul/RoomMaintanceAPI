namespace RoomMaintenanceAPI.Models
{
    public class ApartmentMaster
    {
        public int Id { get; set; }
        public int LocationID { get; set; }
        public string ApartmentName { get; set; }
        public string EsubscriptionID { get; set; }
        public string RoomCount { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        //navigation property
        public LocationMaster Location { get; set; }
    }
}   