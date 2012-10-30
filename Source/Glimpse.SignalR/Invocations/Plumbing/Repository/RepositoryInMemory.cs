using System.Collections.Generic;
using System.Collections.ObjectModel;
using Glimpse.SignalR.Invocations.Contracts;
using Glimpse.SignalR.Invocations.Contracts.Repository;

namespace Glimpse.SignalR.Invocations.Plumbing.Repository
{
    public class InvocationRepositoryInMemory : IInvocationRepository
    {
        private readonly ICollection<InvocationModel> _invocations;

        public InvocationRepositoryInMemory()
        {
            _invocations = new Collection<InvocationModel>();
        }

        public IEnumerable<InvocationModel> GetAll()
        {
            return _invocations;
        }

        public void Add(InvocationModel invocation)
        {
            _invocations.Add(invocation);
        }
    }
}