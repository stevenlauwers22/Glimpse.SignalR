using System;
using System.Collections.Generic;

namespace Glimpse.SignalR.Hubs.Contracts.GetHubs
{
    public class HubModel
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public IEnumerable<HubMethodModel> Methods { get; set; }
    }
}