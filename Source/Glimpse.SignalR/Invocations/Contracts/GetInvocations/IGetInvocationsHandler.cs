namespace Glimpse.SignalR.Invocations.Contracts.GetInvocations
{
    public interface IGetInvocationsHandler
    {
        GetInvocationsResult Handle(GetInvocationsRequest request);
    }
}