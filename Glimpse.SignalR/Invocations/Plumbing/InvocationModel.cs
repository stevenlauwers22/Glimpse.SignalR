using System;
using System.Collections.Generic;

namespace Glimpse.SignalR.Invocations.Plumbing
{
    public class InvocationModel
    {
        public string ConnectionId { get; set; }
        public string Hub { get; set; }
        public string Method { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime EndedOn { get; set; }
        public InvocationResultModel Result { get; set; }
        public IEnumerable<InvocationArgumentModel> Arguments { get; set; }
    }
}