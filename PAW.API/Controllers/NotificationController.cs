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
            return NotificationDTO.ConvertFrom(notification);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<Notification> Notifications)
        {
            foreach (var n in Notifications)
            {
                if (n.Id > 0)
                    await notificationRepository.CreateAsync(n);
                else 
                    await notificationRepository.UpdateAsync(n);
            }

            return true;
        }
    }
}
