using System.Collections.Generic;

namespace Glimpse.SignalR.Hubs.Contracts.GetHubs
{
    public class GetHubsResult
    {
        public IEnumerable<HubModel> Hubs { get; set; }
    }
}