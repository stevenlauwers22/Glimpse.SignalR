using Glimpse.SignalR.Invocations.Contracts.Profiling;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Glimpse.SignalR.Invocations.Plumbing.Profiling
{
    public class Profiler : IProfiler
    {
        public void Start()
        {
            var hubPipeline = GlobalHost.DependencyResolver.Resolve<IHubPipeline>();
            if (hubPipeline == null)
                return;

            hubPipeline.AddModule(new ProfilerHubPipelineModule());
        }
    }
}