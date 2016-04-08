using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.SignalR.Invocations
{
    public class PluginClientScript : IStaticClientScript
    {
        public ScriptOrder Order
        {
            get { return ScriptOrder.IncludeAfterClientInterfaceScript; }
        }

        public string GetUri(string version)
        {
            // TODO: don't override the version (at the moment I override it with a guid to make sure the script is not being cache during development
            version = Guid.NewGuid().ToString();

            return string.Format("/Glimpse.axd?n={0}&v={1}", PluginClientResource.InternalName, version);
        }
    }
}