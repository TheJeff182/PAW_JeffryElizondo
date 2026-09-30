using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class NotificationDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("notificationId")]
    public int NotificationId { get; set; }
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
    [JsonPropertyName("isRead")]
    public bool? IsRead { get; set; }
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }
    [JsonPropertyName("comments")]
    public string Comments { get; set; }
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }

    public static NotificationDTO ConvertFrom(Notification notification)
    {
        return new NotificationDTO
        {
            Id = Guid.NewGuid(),
            NotificationId = notification.Id,
            UserId = notification.UserId,
            Message = notification.Message,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt,
            Comments = string.Empty,
            CreatedDate = notification.CreatedAt ?? DateTime.Now,
            ModifiedDate = notification.CreatedAt ?? DateTime.Now
        };
    }

    public static Notification ConvertTo(NotificationDTO notificationDTO)
    {
        return new Notification
        {
            Id = notificationDTO.NotificationId,
            UserId = notificationDTO.UserId,
            Message = notificationDTO.Message,
            IsRead = notificationDTO.IsRead,
            CreatedAt = notificationDTO.CreatedAt
        };
    }
}
