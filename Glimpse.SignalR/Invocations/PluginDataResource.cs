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
            {
                throw new ArgumentNullException("context");
            }

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

            var data = new List<object> { new object[] { "Hub", "Method", "Result", "Arguments", "Invoked on", "Duration", "Connection ID" } };
            data.AddRange(invocations
                .Select(invocation => new[] { invocation.Hub, invocation.Method, FormatInvocationResult(invocation.Result), FormatInvocationArguments(invocation.Arguments), invocation.StartedOn, (invocation.EndedOn - invocation.StartedOn).TotalMilliseconds + " ms", invocation.ConnectionId })
                .ToList());

            return data;
        }

        private static object FormatInvocationResult(InvocationResultModel invocationResult)
        {
            var data = new List<object> { new object[] { "Value", "Type" } };
            if (invocationResult == null)
            {
                data.Add(new object[] { null, null });
                return data;
            }

            data.Add(new[] { invocationResult.Value, invocationResult.Type.FullName });
            return data;
        }

        private static object FormatInvocationArguments(IEnumerable<InvocationArgumentModel> invocationArguments)
        {
            var data = new List<object> { new object[] { "Value", "Name", "Type" } };
            if (invocationArguments == null)
            {
                data.Add(new object[] { null, null, null });
                return data;
            }

            data.AddRange(invocationArguments
                .Select(invocationParameter => new[] { invocationParameter.Value, invocationParameter.Name, invocationParameter.Type.FullName })
                .ToList());

            return data;
        }
    }
}