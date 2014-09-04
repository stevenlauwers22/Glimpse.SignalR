using System;
using System.Collections.Generic;
using Glimpse.SignalR.Invocations.Plumbing;

namespace Glimpse.SignalR.Invocations
{
    public static class PluginSettingsDefaults
    {
        private static readonly ICollection<InvocationModel> Invocations = new List<InvocationModel>();
        private static readonly object Locker = new object();

        public static Func<IEnumerable<InvocationModel>> GetInvocationsFunc()
        {
            return () => Invocations;
        }

        public static Action<InvocationModel> StoreInvocation()
        {
            return invocation =>
                {
                    lock (Locker)
                    {
                        Invocations.Add(invocation);
                    }
                };
        }
    }
}