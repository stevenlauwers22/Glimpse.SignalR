using Glimpse.SignalR.Invocations.Contracts.Profiling;
using Glimpse.SignalR.Invocations.Contracts.Repository;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Glimpse.SignalR.Invocations.Plumbing.Profiling
{
    public class Profiler : IProfiler
    {
        private readonly IInvocationRepository _repository;

        public Profiler(IInvocationRepository repository)
        {
            _repository = repository;
        }

        public void Start()
        {
            var hubPipeline = GlobalHost.DependencyResolver.Resolve<IHubPipeline>();
            if (hubPipeline == null)
                return;
            
            hubPipeline.AddModule(new ProfilerHubPipelineModule(_repository));
        }
    }
}
