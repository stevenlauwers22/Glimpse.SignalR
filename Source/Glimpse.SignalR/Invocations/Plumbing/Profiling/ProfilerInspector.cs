using Glimpse.Core.Extensibility;

namespace Glimpse.SignalR.Invocations.Plumbing.Profiling
{
    public class ProfilerInspector : IInspector
    {
        private readonly IProfiler _profiler;

        public ProfilerInspector()
            : this(new Profiler())
        {
        }

        public ProfilerInspector(IProfiler profiler)
        {
            _profiler = profiler;
        }

        public void Setup(IInspectorContext context)
        {
            if (context.RuntimePolicyStrategy() == RuntimePolicy.Off)
                return;

            _profiler.Start();
        }
    }
}