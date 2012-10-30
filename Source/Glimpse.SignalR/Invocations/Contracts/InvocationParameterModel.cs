using System;

namespace Glimpse.SignalR.Invocations.Contracts
{
    public class InvocationArgumentModel
    {
        public object Value { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
    }
}