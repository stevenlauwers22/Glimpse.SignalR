using System.Collections.Generic;

namespace Glimpse.SignalR.Invocations.Plumbing.GetInvocations
{
    public class GetInvocationsResult
    {
        public IEnumerable<InvocationModel> Invocations { get; set; }
    }
}