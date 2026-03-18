namespace RoomMaintenanceAPI.Models
{
    public class TrnRequestDetails
    {
        public int RequestId { get; set; }
        public string EmployeeName { get; set; }
        public string EmpId { get; set; }
        public string ContactNo { get; set; }
        public int Facility { get; set; }
        public int Location { get; set; }
        public int Apartment { get; set; }
        public int Category { get; set; }
        public int SubCategory { get; set; }
        public string Description { get; set; }
        public string? Attachment { get; set; }
        public TrnRequest Request { get; set; }
    }

}