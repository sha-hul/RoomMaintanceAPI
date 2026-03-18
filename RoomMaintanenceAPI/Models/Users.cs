using System.ComponentModel.DataAnnotations;

namespace RoomMaintenanceAPI.Models
{
    public class Users
    {
        [Key]
        public string EmpId { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
