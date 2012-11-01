using System.Linq;
using Glimpse.SignalR.Invocations.Contracts.GetInvocations;
using Glimpse.SignalR.Invocations.Contracts.Repository;

namespace Glimpse.SignalR.Invocations.Plumbing.GetInvocations
{
    public class GetInvocationsHandler : IGetInvocationsHandler
    {
        private readonly IInvocationRepository _repository;

        public GetInvocationsHandler(IInvocationRepository repository)
        {
            _repository = repository;
        }

        public GetInvocationsResult Handle(GetInvocationsRequest request)
        {
            var invocations = _repository.GetAll();
            if (invocations != null)
                invocations = invocations.OrderByDescending(i => i.StartedOn);

            var result = new GetInvocationsResult { Invocations = invocations };
            return result;
        }
    }
}