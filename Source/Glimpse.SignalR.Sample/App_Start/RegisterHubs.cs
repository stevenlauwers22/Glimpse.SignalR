using System.Web;
using System.Web.Routing;
using Glimpse.SignalR.Sample.App_Start;

[assembly: PreApplicationStartMethod(typeof(RegisterHubs), "Start")]

namespace Glimpse.SignalR.Sample.App_Start
{
    public static class RegisterHubs
    {
        public static void Start()
        {
            // Register the default hubs route: ~/signalr/hubs
            RouteTable.Routes.MapHubs();
        }
    }
}