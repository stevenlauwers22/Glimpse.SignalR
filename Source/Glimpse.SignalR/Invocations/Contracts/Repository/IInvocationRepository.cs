using System.Collections.Generic;

namespace Glimpse.SignalR.Invocations.Contracts.Repository
{
    public interface IInvocationRepository
    {
        IEnumerable<InvocationModel> GetAll();
        void Add(InvocationModel entity);
    }
}
