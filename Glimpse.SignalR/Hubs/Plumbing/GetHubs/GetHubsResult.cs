using System.Collections.Generic;

namespace Glimpse.SignalR.Hubs.Plumbing.GetHubs
{
    public class GetHubsResult
    {
        public IEnumerable<HubModel> Hubs { get; set; }
    }
}