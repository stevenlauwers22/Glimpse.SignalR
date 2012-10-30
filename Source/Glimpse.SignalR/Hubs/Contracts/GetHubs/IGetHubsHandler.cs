namespace Glimpse.SignalR.Hubs.Contracts.GetHubs
{
    public interface IGetHubsHandler
    {
        GetHubsResult Handle(GetHubsRequest request);
    }
}