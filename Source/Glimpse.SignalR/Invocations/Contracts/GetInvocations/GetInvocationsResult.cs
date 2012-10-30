using System.Collections.Generic;

namespace Glimpse.SignalR.Invocations.Contracts.GetInvocations
{
    public class GetInvocationsResult
    {
        public IEnumerable<InvocationModel> Invocations { get; set; }
    }
}