using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace EducationManual.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task Send(string message, string to)
        {
            var userName = Context.User.Identity.Name;

            if (Context.ConnectionId != to)
                await Clients.User(Context.ConnectionId).displayMessage(message);
            await Clients.User(to).displayMessage(message);
        }
    }
}