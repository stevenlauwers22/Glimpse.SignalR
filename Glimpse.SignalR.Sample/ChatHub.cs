using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;

namespace Glimpse.SignalR.Sample
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the addMessage method on all clients
            SaveMessage(DateTime.Now.ToLongTimeString(), string.Format("{0}: {1}", name, message));
            Clients.All.addMessage(name, message);
        }

        public string GetMessages()
        {
            var result = JsonConvert.SerializeObject(GetMessagesRepository().Values.ToArray());
            return result;
        }

        private IDictionary<string, string> GetMessagesRepository()
        {
            var messages = Context.Request.GetHttpContext().Application["ChatHubMessages"] as ConcurrentDictionary<string, string> ?? new ConcurrentDictionary<string, string>();
            return messages;
        }

        private void SaveMessage(string name, string message)
        {
            var messages = GetMessagesRepository();
            messages.Add(name, message);
            Context.Request.GetHttpContext().Application["ChatHubMessages"] = messages;
        }
    }
}