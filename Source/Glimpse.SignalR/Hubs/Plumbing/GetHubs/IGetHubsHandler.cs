namespace Glimpse.SignalR.Hubs.Plumbing.GetHubs
{
    public interface IGetHubsHandler
    {
        GetHubsResult Handle(GetHubsRequest request);
    }
}