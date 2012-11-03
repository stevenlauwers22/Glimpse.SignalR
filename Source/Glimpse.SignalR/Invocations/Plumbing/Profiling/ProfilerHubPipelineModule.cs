using System;
using System.Linq;
using Glimpse.SignalR.Invocations.Contracts;
using Microsoft.AspNet.SignalR.Hubs;

namespace Glimpse.SignalR.Invocations.Plumbing.Profiling
{
    public class ProfilerHubPipelineModule : HubPipelineModule
    {
        protected override bool OnBeforeIncoming(IHubIncomingInvokerContext context)
        {
            context.State["ProfilingHubPipelineModule-Invocation-StartedOn"] = DateTime.Now;
            return base.OnBeforeIncoming(context);
        }

        protected override object OnAfterIncoming(object result, IHubIncomingInvokerContext context)
        {
            var startedOn = (DateTime)context.State["ProfilingHubPipelineModule-Invocation-StartedOn"];
            var invocation = new InvocationModel
            {
                Hub = context.MethodDescriptor.Hub.Name,
                Method = context.MethodDescriptor.Name,
                Result = new InvocationResultModel 
                    { 
                        Value = result,
                        Type = context.MethodDescriptor.ReturnType
                    },
                Arguments = context.Args.Length > 0 ? context.Args
                    .Select((t, i) => new InvocationArgumentModel
                    {
                        Value = t, 
                        Name = context.MethodDescriptor.Parameters[i].Name,
                        Type = context.MethodDescriptor.Parameters[i].Type
                    })
                    .ToList() : null,
                StartedOn = startedOn,
                EndedOn = DateTime.Now,
                ConnectionId = context.Hub.Context.ConnectionId
            };

            PluginSettings.StoreInvocation(invocation);
            return base.OnAfterIncoming(result, context);
        }

        protected override void OnIncomingError(Exception ex, IHubIncomingInvokerContext context)
        {
            var startedOn = (DateTime)context.State["ProfilingHubPipelineModule-Invocation-StartedOn"];
            var invocation = new InvocationModel
            {
                Hub = context.MethodDescriptor.Hub.Name,
                Method = context.MethodDescriptor.Name,
                Result = null,
                Arguments = context.Args.Length > 0 ? context.Args
                    .Select((t, i) => new InvocationArgumentModel
                    {
                        Value = t,
                        Name = context.MethodDescriptor.Parameters[i].Name,
                        Type = context.MethodDescriptor.Parameters[i].Type
                    })
                    .ToList() : null,
                StartedOn = startedOn,
                EndedOn = DateTime.Now,
                ConnectionId = context.Hub.Context.ConnectionId
            };

            PluginSettings.StoreInvocation(invocation);
            base.OnIncomingError(ex, context);
        }
    }
}