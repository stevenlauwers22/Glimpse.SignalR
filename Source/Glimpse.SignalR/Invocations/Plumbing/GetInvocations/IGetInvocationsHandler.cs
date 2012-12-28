namespace Glimpse.SignalR.Invocations.Plumbing.GetInvocations
{
    public interface IGetInvocationsHandler
    {
        GetInvocationsResult Handle(GetInvocationsRequest request);
    }
}