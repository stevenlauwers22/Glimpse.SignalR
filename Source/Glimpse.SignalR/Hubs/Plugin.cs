using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.SignalR.Hubs.Contracts.GetHubs;
using Glimpse.SignalR.Hubs.Plumbing.GetHubs;

namespace Glimpse.SignalR.Hubs
{
    public class Plugin : TabBase
    {
        private readonly IGetHubsHandler _getHubsHandler;

        public Plugin()
            : this(new GetHubsHandler())
        {
        }

        public Plugin(IGetHubsHandler getHubsHandler)
        {
            _getHubsHandler = getHubsHandler;
        }

        public override string Name
        {
            get { return "SignalR - Hubs"; }
        }

        public override object GetData(ITabContext context)
        {
            var getHubsRequest = new GetHubsRequest();
            var getHubsResult = _getHubsHandler.Handle(getHubsRequest);
            var data = FormatHubs(getHubsResult.Hubs);
            return data;
        }

        private static object FormatHubs(IEnumerable<HubModel> hubs)
        {
            if (hubs == null)
                return null;

            var data = new List<object> { new object[] { "Hub", "Type", "Methods" } };
            data.AddRange(hubs
                    .Select(hub => new[] { hub.Name, hub.Type.FullName, FormatHubMethods(hub.Methods) })
                    .ToList());

            return data;
        }

        private static object FormatHubMethods(IEnumerable<HubMethodModel> hubMethods)
        {
            var data = new List<object> { new object[] { "Name", "Return Type", "Parameters" } };
            if (hubMethods == null)
            {
                data.Add(new object[] { null, null, null });
                return data;
            }

            data.AddRange(hubMethods
                    .Select(hubMethod => new[] { hubMethod.Name, hubMethod.ReturnType.FullName, FormatHubMethodParameters(hubMethod.Parameters) })
                    .ToList());

            return data;
        }

        private static object FormatHubMethodParameters(IEnumerable<HubMethodParameterModel> hubMethodParameters)
        {
            var data = new List<object> { new object[] { "Name", "Type" }};
            if (hubMethodParameters == null)
            {
                data.Add(new object[] { null, null });
                return data;
            }

            data.AddRange(hubMethodParameters
                    .Select(hubMethodParameter => new[] { hubMethodParameter.Name, hubMethodParameter.Type.FullName })
                    .ToList());

            return data;
        }
    }
}