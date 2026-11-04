using SendNotificationAPI.Models.InterfacesModelType;

namespace SendNotificationAPI.Services
{
    public interface INotificationService<TRequestModel> where TRequestModel : class, TNotificationRequestModel
    {
        Task SendNotificationAsync(TRequestModel request);
    }
}