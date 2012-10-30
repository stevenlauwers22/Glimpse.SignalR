using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;

namespace Glimpse.SignalR.Sample
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        public void Send(string message)
        {
            // Call the addMessage method on all clients
            SaveMessage(message);
            Clients.All.addMessage(message);
        }

        public string[] GetMessages()
        {
            return GetMessagesRepository().ToArray();
        }

        private static IList<string> GetMessagesRepository()
        {
            var messages = HttpContext.Current.Application["ChatHubMessages"] as IList<string> ?? new List<string>();
            return messages;
        }

        public static void SaveMessage(string message)
        {
            var messages = GetMessagesRepository();
            messages.Add(message);

            HttpContext.Current.Application["ChatHubMessages"] = messages;
        }
    }
}