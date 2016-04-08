using Glimpse.Core.Extensibility;
using Glimpse.Core.Resource;

namespace Glimpse.SignalR.Invocations
{
    public class PluginClientResource : FileResource, IKey
    {
        internal const string InternalName = "glimpse_signalr_invocations_client";

        private EmbeddedResourceInfo GlimpseSignalREmbeddedResourceInfo { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientResource" /> class.
        /// </summary>
        public PluginClientResource()
        {
            Name = InternalName;

            GlimpseSignalREmbeddedResourceInfo = new EmbeddedResourceInfo(
                GetType().Assembly,
                "Glimpse.SignalR.Invocations.plugin.js",
                "application/x-javascript");
        }

        public string Key
        {
            get { return Name; }
        }

        protected override EmbeddedResourceInfo GetEmbeddedResourceInfo(IResourceContext context)
        {
            return GlimpseSignalREmbeddedResourceInfo;
        }
    }
}