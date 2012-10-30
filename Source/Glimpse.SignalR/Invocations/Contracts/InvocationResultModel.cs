using System;

namespace Glimpse.SignalR.Invocations.Contracts
{
    public class InvocationResultModel
    {
        public object Value { get; set; }
        public Type Type { get; set; }
    }
}