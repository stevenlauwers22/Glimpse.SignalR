using System;
using System.Collections.Generic;

namespace Glimpse.SignalR.Hubs.Plumbing.GetHubs
{
    public class HubModel
    {
        public string Name { get; set; }
        public Type HubType { get; set; }
        public IEnumerable<HubMethodModel> Methods { get; set; }
    }
}