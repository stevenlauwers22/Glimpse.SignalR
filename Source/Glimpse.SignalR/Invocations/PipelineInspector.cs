using Glimpse.Core.Extensibility;
using Glimpse.SignalR.Invocations.Plumbing.Profiling;

namespace Glimpse.SignalR.Invocations
{
    public class PipelineInspector : IPipelineInspector
    {
        private readonly IProfiler _profiler;

        public PipelineInspector()
            : this(new Profiler())
        {
        }

        public PipelineInspector(IProfiler profiler)
        {
            _profiler = profiler;
        }

        public void Setup(IPipelineInspectorContext context)
        {
            if (context.RuntimePolicyStrategy() == RuntimePolicy.Off)
                return;

            _profiler.Start();
        }

        public void Teardown(IPipelineInspectorContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}