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
            return string.Format("/Glimpse.axd?n={0}&v={1}", PluginClientResource.InternalName, version);
        }
    }
}