using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController(ILogger<NotificationController> logger, INotificationRepository notificationRepository) : ControllerBase
    {
        [HttpGet(Name = "GetNotifications")]
        public async Task<IEnumerable<NotificationDTO>> GetAll()
        {
            var notifications = await notificationRepository.ReadAsync() ?? [];
            return notifications.Select(NotificationDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetNotificationById")]
        public async Task<ActionResult<NotificationDTO>> GetById(int id)
        {
            var notification = await notificationRepository.FindAsync(id);
            if (notification == null)
                return NotFound();
            return NotificationDTO.ConvertFrom(notification);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] NotificationDTO notificationDTO)
        {
            try
            {
                logger.LogInformation($"Save POST: Processing NotificationId={notificationDTO.NotificationId}, UserId={notificationDTO.UserId}");

                bool isUpdate = notificationDTO.NotificationId > 0;

                if (isUpdate)
                {
                    var existingNotification = await notificationRepository.FindAsync(notificationDTO.NotificationId);
                    if (existingNotification == null)
                    {
                        logger.LogWarning($"Save: Notification with ID {notificationDTO.NotificationId} not found for update");
                        return false;
                    }

                    existingNotification.UserId = notificationDTO.UserId;
                    existingNotification.Message = notificationDTO.Message;
                    existingNotification.IsRead = notificationDTO.IsRead;

                    logger.LogInformation($"Save: Updating existing notification - ID={existingNotification.Id}");
                    await notificationRepository.UpdateAsync(existingNotification);
                }
                else
                {
                    var notification = new Notification
                    {
                        UserId = notificationDTO.UserId,
                        Message = notificationDTO.Message,
                        IsRead = notificationDTO.IsRead,
                        CreatedAt = DateTime.Now
                    };

                    logger.LogInformation($"Save: Creating new notification");

                    await notificationRepository.CreateAsync(notification);
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"Save error: {ex.Message}");
                return false;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> Delete(int id)
        {
            try
            {
                logger.LogInformation($"Delete POST: NotificationId={id}");

                var notification = await notificationRepository.FindAsync(id);
                if (notification == null)
                {
                    logger.LogWarning($"Delete: Notification with ID {id} not found");
                    return false;
                }

                await notificationRepository.DeleteAsync(notification);
                logger.LogInformation($"Delete: Notification {id} deleted successfully");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"Delete error: {ex.Message}");
                return false;
            }
        }
    }
}
