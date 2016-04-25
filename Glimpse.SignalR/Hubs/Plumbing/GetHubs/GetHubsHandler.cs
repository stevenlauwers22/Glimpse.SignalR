using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Glimpse.SignalR.Hubs.Plumbing.GetHubs
{
    public interface IGetHubsHandler
    {
        GetHubsResult Handle(GetHubsRequest request);
    }

    public class GetHubsHandler : IGetHubsHandler
    {
        public GetHubsResult Handle(GetHubsRequest request)
        {
            var hubManager = GlobalHost.DependencyResolver.Resolve<IHubManager>();
            if (hubManager == null)
                return new GetHubsResult();

            var hubs = GetHubs(hubManager);
            var result = new GetHubsResult { Hubs = hubs };
            return result;
        }

        private static IEnumerable<HubModel> GetHubs(IHubManager hubManager)
        {
            var hubDescriptors = hubManager.GetHubs(hd => true).ToList();
            if (!hubDescriptors.Any())
                return null;

            var hubData = hubDescriptors
                .Select(hubDescriptor => new HubModel
                {
                    Name = hubDescriptor.Name,
                    HubType = hubDescriptor.HubType,
                    Methods = GetHubMethodInfo(hubManager, hubDescriptor)
                })
                .ToList();

            return hubData;
        }

        private static IEnumerable<HubMethodModel> GetHubMethodInfo(IHubManager hubManager, HubDescriptor hubDescriptor)
        {
            var hubMethodDescriptors = hubManager.GetHubMethods(hubDescriptor.Name, md => true).ToList();
            if (!hubMethodDescriptors.Any())
                return null;

            var hubMethodData = hubMethodDescriptors
                .Select(hubMethodDescriptor => new HubMethodModel
                {
                    Name = hubMethodDescriptor.Name,
                    ReturnType = hubMethodDescriptor.ReturnType,
                    Parameters = GetHubMethodParametersInfo(hubMethodDescriptor)
                })
                .ToList();

            return hubMethodData;
        }

        private static IEnumerable<HubMethodParameterModel> GetHubMethodParametersInfo(MethodDescriptor hubMethodDescriptor)
        {
            var hubMethodParameterDescriptors = hubMethodDescriptor.Parameters;
            if (hubMethodParameterDescriptors == null || !hubMethodParameterDescriptors.Any())
                return null;

            var hubMethodParameterData = hubMethodParameterDescriptors
                .Select(hubMethodParameterDescriptor => new HubMethodParameterModel
                {
                    Name = hubMethodParameterDescriptor.Name,
                    ParameterType = hubMethodParameterDescriptor.ParameterType
                })
                .ToList();

            return hubMethodParameterData;
        }
    }
}