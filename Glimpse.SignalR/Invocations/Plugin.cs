using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.SignalR.Invocations.Plumbing;
using Glimpse.SignalR.Invocations.Plumbing.GetInvocations;

namespace Glimpse.SignalR.Invocations
{
    public class Plugin : TabBase
    {
        private readonly IGetInvocationsHandler _getInvocationsHandler;

        public Plugin()
            : this(new GetInvocationsHandler())
        {
        }

        public Plugin(IGetInvocationsHandler getInvocationsHandler)
        {
            _getInvocationsHandler = getInvocationsHandler;
        }

        public override string Name
        {
            get { return "SignalR - Invocations"; }
        }

        public override object GetData(ITabContext context)
        {
            var getInvocationsRequest = new GetInvocationsRequest();
            var getInvocationsResult = _getInvocationsHandler.Handle(getInvocationsRequest);
            var data = FormatInvocations(getInvocationsResult.Invocations);
            return data;
        }

        private static object FormatInvocations(IEnumerable<InvocationModel> invocations)
        {
            if (invocations == null)
                return null;

            var data = new List<object> { new object[] { "Hub", "Method", "Result", "Arguments", "Invoked on", "Duration", "Connection ID" } };
            data.AddRange(invocations
                .Select(invocation => new[] { invocation.Hub, invocation.Method, FormatInvocationResult(invocation.Result), FormatInvocationArguments(invocation.Arguments), invocation.StartedOn, (invocation.EndedOn - invocation.StartedOn).TotalMilliseconds + " ms", invocation.ConnectionId })
                .ToList());

            return data;
        }

        private static object FormatInvocationResult(InvocationResultModel invocationResult)
        {
            var data = new List<object> { new object[] { "Value", "Type" } };
            if (invocationResult == null)
            {
                data.Add(new object[] { null, null });
                return data;
            }

            data.Add(new[] { invocationResult.Value, invocationResult.Type.FullName });
            return data;
        }

        private static object FormatInvocationArguments(IEnumerable<InvocationArgumentModel> invocationArguments)
        {
            var data = new List<object> { new object[] { "Value", "Name", "Type" } };
            if (invocationArguments == null)
            {
                data.Add(new object[] { null, null, null });
                return data;
            }

            data.AddRange(invocationArguments
                .Select(invocationParameter => new[] { invocationParameter.Value, invocationParameter.Name, invocationParameter.Type.FullName })
                .ToList());

            return data;
        }
    }
}