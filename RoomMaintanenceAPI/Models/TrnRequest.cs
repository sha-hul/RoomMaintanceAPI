namespace RoomMaintenanceAPI.Models
{
    public class TrnRequest
    {
        public int RequestId { get; set; }   // EF Core recognizes this as PK
        public int StatusId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DtTransaction { get; set; }
        public DateTime UpdatedAt { get; set; }

        public TrnRequestDetails Details { get; set; }
    }


}