using System.ComponentModel.DataAnnotations;

namespace RoomMaintenanceAPI.Models
{
    public class Users
    {
        [Key]
        public string EmpId { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string EmpName { get; set; }
        public string Mail { get; set; }
        public DateTime dtTransaction { get; set; }
    }
}