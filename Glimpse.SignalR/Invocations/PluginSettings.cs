using System;
using System.Collections.Generic;
using Glimpse.SignalR.Invocations.Plumbing;

namespace Glimpse.SignalR.Invocations
{
    public static class PluginSettings
    {
        private static Func<IEnumerable<InvocationModel>> _getInvocations;
        public static Func<IEnumerable<InvocationModel>> GetInvocations
        {
            get
            {
                return _getInvocations ?? PluginSettingsDefaults.GetInvocationsFunc();
            }
            set
            {
                _getInvocations = value;
            }
        }

        private static Action<InvocationModel> _storeInvocation;
        public static Action<InvocationModel> StoreInvocation 
        { 
            get
            {
                return _storeInvocation ?? PluginSettingsDefaults.StoreInvocation();
            }
            set
            {
                _storeInvocation = value;
            }
        }
    }
}