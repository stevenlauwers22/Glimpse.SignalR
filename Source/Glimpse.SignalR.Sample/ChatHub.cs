using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Glimpse.SignalR.Sample
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    using Newtonsoft.Json;

    [HubName("chat")]
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the addMessage method on all clients
            this.SaveMessage(DateTime.Now.ToLongTimeString(), string.Format("{0}: {1}", name, message));
            this.Clients.All.addMessage(name, message);
        }

        public string GetMessages()
        {
            string result = JsonConvert.SerializeObject(this.GetMessagesRepository().Values.ToArray());
            return result;
        }

        private IDictionary<string, string> GetMessagesRepository()
        {
            var messages =
                this.Context.Request.GetHttpContext().Application["ChatHubMessages"] as
                ConcurrentDictionary<string, string> ?? new ConcurrentDictionary<string, string>();
            return messages;
        }

        public void SaveMessage(string name, string message)
        {
            var messages = this.GetMessagesRepository();
            messages.Add(name, message);

            this.Context.Request.GetHttpContext().Application["ChatHubMessages"] = messages;
        }
    }
}