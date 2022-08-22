using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebForms.Models;
using TestWebForms.Hubs.Storage;

namespace TestWebForms.Hubs
{
    public class SheepsHub : Hub
    {

        public void TryReady(object MyFleet)
        {

        }

        public void EnemyConnect()
        {

        }

        // Отключение пользователя
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = Container.Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                Container.Users.Remove(item);
                var id = Context.ConnectionId;
                //Clients.All.onUserDisconnected(id, item.Name);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}