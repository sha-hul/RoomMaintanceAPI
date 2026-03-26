using RoomMaintenanceAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace RoomMaintenanceAPI
{
    public class Common
    {
        //To Build the error log
        public ErrorLog WriteErrorLog(string ApiName, string? ControllerName, string? RequestPayload, string ErrorMessage, string? ErrorCode, string? LoggedBy)
        {
            return new ErrorLog
            {
                ApiName = ApiName,
                ControllerName = ControllerName,
                RequestPayload = RequestPayload,
                ErrorMessage = ErrorMessage,
                ErrorCode = ErrorCode,
                LoggedBy = LoggedBy,
                LoggedAt = DateTime.Now
            };
        }
        public string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
    public class URL
    {
        public string urlUI { get; set; }
    }
}