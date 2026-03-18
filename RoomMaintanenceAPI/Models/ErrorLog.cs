using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace RoomMaintenanceAPI.Models
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public string ApiName { get; set; }
        public string? ControllerName { get; set; }
        public string? RequestPayload { get; set; }
        public string ErrorMessage { get; set; }
        public string? ErrorCode { get; set; }
        public string? LoggedBy { get; set; }
        public DateTime? LoggedAt { get; set; }
    }
    public static class ErrorHandler
    {
        private static Common objCom = new Common();
        public static async Task<IActionResult> HandleExceptionAsync(
            Exception ex,
            IDbContextTransaction transaction,
            DbContext context,
            object dto,
            string apiName,
            string controllerName,
            string errorCode,
            string empId)
        {
            if(transaction != null)
                await transaction.RollbackAsync();
            
            context.ChangeTracker.Clear();

            try
            {
                var fullError = ex.InnerException?.Message ?? ex.Message;

                var error = objCom.WriteErrorLog(
                    apiName,
                    controllerName,
                    JsonConvert.SerializeObject(dto),
                    fullError,
                    errorCode,
                    empId
                );

                context.Set<ErrorLog>().Add(error);
                await context.SaveChangesAsync();
            }
            catch
            {
                // Trigger mail or message to Developer
            }

            return new BadRequestObjectResult(new { message = ex.Message, status = false });
        }
    }

}