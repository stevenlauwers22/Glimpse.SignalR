using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.SignalR.Invocations.Contracts;
using Glimpse.SignalR.Invocations.Contracts.GetInvocations;
using Glimpse.SignalR.Invocations.Contracts.Profiling;
using Glimpse.SignalR.Invocations.Contracts.Repository;
using Glimpse.SignalR.Invocations.Plumbing.GetInvocations;
using Glimpse.SignalR.Invocations.Plumbing.Profiling;
using Glimpse.SignalR.Invocations.Plumbing.Repository;

namespace Glimpse.SignalR.Invocations
{
    public class Plugin : TabBase, ITabSetup
    {
        private readonly IGetInvocationsHandler _getInvocationsHandler;
        private readonly IProfiler _profiler;

        public Plugin()
            : this(new InvocationRepositoryInMemory())
        {
        }

        public Plugin(IInvocationRepository repository)
            : this(new GetInvocationsHandler(repository), new Profiler(repository))
        {
        }

        public Plugin(IGetInvocationsHandler getInvocationsHandler, IProfiler profiler)
        {
            _getInvocationsHandler = getInvocationsHandler;
            _profiler = profiler;
        }

        public override string Name
        {
            get { return "SignalR - Invocations"; }
        }

        public void Setup(ITabSetupContext context)
        {
            _profiler.Start();
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