using System;
using System.Linq;
using Microsoft.AspNet.SignalR.Hubs;

namespace Glimpse.SignalR.Invocations.Plumbing.Profiling
{
    public class ProfilerHubPipelineModule : HubPipelineModule
    {
        protected override bool OnBeforeIncoming(IHubIncomingInvokerContext context)
        {
            context.StateTracker["ProfilingHubPipelineModule-Invocation-StartedOn"] = DateTime.Now;
            return base.OnBeforeIncoming(context);
        }

        protected override object OnAfterIncoming(object result, IHubIncomingInvokerContext context)
        {
            var startedOn = (DateTime)context.StateTracker["ProfilingHubPipelineModule-Invocation-StartedOn"];
            var invocation = new InvocationModel
            {
                ConnectionId = context.Hub.Context.ConnectionId,
                Hub = context.MethodDescriptor.Hub.Name,
                Method = context.MethodDescriptor.Name,
                StartedOn = startedOn,
                EndedOn = DateTime.Now,
                Result = new InvocationResultModel
                    {
                        Value = result,
                        Type = context.MethodDescriptor.ReturnType
                    },
                Arguments = context.Args.Count > 0 ? context.Args
                    .Select((t, i) => new InvocationArgumentModel
                    {
                        Value = t,
                        Name = context.MethodDescriptor.Parameters[i].Name,
                        Type = context.MethodDescriptor.Parameters[i].ParameterType
                    })
                    .ToList() : null
            };

            PluginSettings.StoreInvocation(invocation);
            return base.OnAfterIncoming(result, context);
        }
    }
}