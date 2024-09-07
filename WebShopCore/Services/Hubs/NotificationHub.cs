using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopCore.Interfaces;
using WebShopCore.Model;
using WebShopCore.ViewModel.Notification;

namespace WebShopCore.Services.Hubs
{
    public class NotificationHub : Hub, INotificationHub
    {
        private readonly IUnitOfWork _uow;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationHub(IUnitOfWork uow, IHubContext<NotificationHub> hubContext)
        {
            _uow = uow;
            _hubContext = hubContext;
        }

        public async Task SendNotificationToAll(NotificationViewModel data)
        {
            await _hubContext.Clients.All.SendAsync("ReceivedNotification", JsonConvert.SerializeObject(data));
        }

        public async Task SendNotificationToClient(NotificationViewModel data, int userId)
        {
            var hubConnections = _uow.HubConnectionRepository.BuildQuery(x => x.UserId == userId).ToList();

            foreach(var hub in hubConnections)
            {
                await _hubContext.Clients.Client(hub.ConnectionId).SendAsync("ReceivedNotification", JsonConvert.SerializeObject(data));
            }
        }

        public async Task SaveUserConnection(string userId)
        {
            int id = int.Parse(userId);
            var user = _uow.UserRepository.FirstOrDefault(x => x.UserId == id);
            if (user != null)
            {
                var connectionId = Context.ConnectionId;
                HubConnection hubConnection = new()
                {
                    ConnectionId = connectionId,
                    UserId = id
                };
                _uow.HubConnectionRepository.Add(hubConnection);
                await _uow.CommitAsync();
            }
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("OnConnected");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var hubConnection = _uow.HubConnectionRepository.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if(hubConnection != null)
            {
                _uow.HubConnectionRepository.Delete(hubConnection);
                await _uow.CommitAsync();
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
