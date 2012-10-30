using System;
using System.Collections.Generic;

namespace Glimpse.SignalR.Invocations.Contracts
{
    public class InvocationModel
    {
        public string Hub { get; set; }
        public string Method { get; set; }
        public InvocationResultModel Result { get; set; }
        public IEnumerable<InvocationArgumentModel> Arguments { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime EndedOn { get; set; }
        public string ConnectionId { get; set; }
    }
}