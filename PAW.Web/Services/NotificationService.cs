using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;
using System.Text.Json;

namespace PAW.Web.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationDTO>> GetNotificationsAsync();
    Task<NotificationDTO?> GetNotificationByIdAsync(int id);
    Task<bool> CreateNotificationAsync(NotificationDTO notificationDTO);
    Task<bool> UpdateNotificationAsync(NotificationDTO notificationDTO);
    Task<bool> DeleteNotificationAsync(int id);
}

public class NotificationService : ServiceBase, INotificationService
{
    private const string _path = "Notification";
    private readonly IRestProvider _restProvider;

    public NotificationService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<NotificationDTO>> GetNotificationsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var notifications = await JsonProvider.DeserializeAsync<IEnumerable<NotificationDTO>>(response);
        return notifications;
    }

    public async Task<NotificationDTO?> GetNotificationByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync($"{SetPathUrl(_path)}/{id}", id: "");
        var notification = await JsonProvider.DeserializeAsync<NotificationDTO>(response);
        return notification;
    }

    public async Task<bool> CreateNotificationAsync(NotificationDTO notificationDTO)
    {
        notificationDTO.NotificationId = 0;
        var json = JsonSerializer.Serialize(notificationDTO);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> UpdateNotificationAsync(NotificationDTO notificationDTO)
    {
        var json = JsonSerializer.Serialize(notificationDTO);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), json);
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<bool> DeleteNotificationAsync(int id)
    {
        var response = await _restProvider.DeleteAsync($"{SetPathUrl(_path)}/{id}", id.ToString());
        return !string.IsNullOrEmpty(response) && response.Contains("true", StringComparison.OrdinalIgnoreCase);
    }
}
