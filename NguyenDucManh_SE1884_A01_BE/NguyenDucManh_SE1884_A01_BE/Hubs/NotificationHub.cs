using Microsoft.AspNetCore.SignalR;

namespace NguyenDucManh_SE1884_A01_BE.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
