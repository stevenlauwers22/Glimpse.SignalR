using Microsoft.Owin;

[assembly: OwinStartup(typeof(Glimpse.SignalR.Sample.Startup))]

namespace Glimpse.SignalR.Sample
{
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}   