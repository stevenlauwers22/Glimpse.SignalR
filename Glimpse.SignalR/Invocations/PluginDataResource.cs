using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;
using Glimpse.SignalR.Invocations.Plumbing;
using Glimpse.SignalR.Invocations.Plumbing.GetInvocations;

namespace Glimpse.SignalR.Invocations
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse client a listing of Ajax requests, filtered by parent request ID.
    /// </summary>
    public class PluginDataResource : IResource, IKey
    {
        internal const string InternalName = "glimpse_signalr_invocations_data";

        /// <summary>
        /// Gets the name of the resource.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        /// <remarks>
        /// Resource name's should be unique across a given web application. If multiple <see cref="IResource" /> implementations contain the same name, Glimpse may throw an exception resulting in an Http 500 Server Error.
        /// </remarks>
        public string Name
        {
            get { return InternalName; }
        }

        /// <summary>
        /// Gets the required or optional parameters that a resource needs as processing input.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IEnumerable<ResourceParameterMetadata> Parameters
        {
            get { return new[] { ResourceParameter.Hash, ResourceParameter.Callback }; }
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key. Only valid JavaScript identifiers should be used for future compatibility.
        /// </value>
        public string Key
        {
            get { return Name; }
        }

        /// <summary>
        /// Executes the specified resource with the specific context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <see cref="IResourceResult" /> that can be executed when the Http response is ready to be returned.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Exception thrown if <paramref name="context"/> is <c>null</c>.</exception>
        public IResourceResult Execute(IResourceContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var getInvocationsHandler = new GetInvocationsHandler();
            var getInvocationsRequest = new GetInvocationsRequest();
            var getInvocationsResult = getInvocationsHandler.Handle(getInvocationsRequest);
            var data = FormatInvocations(getInvocationsResult.Invocations);
            return new CacheControlDecorator(0, CacheSetting.NoCache, new JsonResourceResult(data, context.Parameters.GetValueOrDefault(ResourceParameter.Callback.Name)));
        }

        private static object FormatInvocations(IEnumerable<InvocationModel> invocations)
        {
            if (invocations == null)
                return null;

            var data = new List<object>();
            data.AddRange(invocations
                .OrderByDescending(invocation => invocation.StartedOn)
                .Select(invocation => new
                {
                    hub = invocation.Hub,
                    method = invocation.Method,
                    result = FormatInvocationResult(invocation.Result),
                    arguments = FormatInvocationArguments(invocation.Arguments),
                    invokedOn = invocation.StartedOn,
                    duration = (invocation.EndedOn - invocation.StartedOn).TotalMilliseconds + " ms",
                    connectionId = invocation.ConnectionId
                })
                .ToList());

            return data;
        }

        private static object FormatInvocationResult(InvocationResultModel invocationResult)
        {
            if (invocationResult == null)
                return null;

            return new
            {
                value = invocationResult.Value,
                type = invocationResult.Type.FullName
            };
        }

        private static object FormatInvocationArguments(IEnumerable<InvocationArgumentModel> invocationArguments)
        {
            if (invocationArguments == null)
                return null;

            var data = new List<object>();
            data.AddRange(invocationArguments
                .Select(invocationParameter => new
                {
                    value = invocationParameter.Value,
                    name = invocationParameter.Name,
                    type = invocationParameter.Type.FullName
                })
                .ToList());

            return data;
        }
    }
}