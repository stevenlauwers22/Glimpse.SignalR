using System.Linq;

namespace Glimpse.SignalR.Invocations.Plumbing.GetInvocations
{
    public class GetInvocationsHandler : IGetInvocationsHandler
    {
        public GetInvocationsResult Handle(GetInvocationsRequest request)
        {
            var invocations = PluginSettings.GetInvocations();
            if (invocations != null)
                invocations = invocations.OrderByDescending(i => i.StartedOn);

            var result = new GetInvocationsResult { Invocations = invocations };
            return result;
        }
    }
}