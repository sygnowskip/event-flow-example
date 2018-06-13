using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Sagas;

namespace Payments.Domain.Common.Saga
{
    public abstract class BaseIdSagaLocator : ISagaLocator
    {
        private readonly Func<string, ISagaId> _sagaIdGenerator;
        protected string MetadataKey { get; }

        protected BaseIdSagaLocator(string metadataKey, Func<string, ISagaId> sagaIdGenerator)
        {
            _sagaIdGenerator = sagaIdGenerator;
            MetadataKey = metadataKey;
        }

        public Task<ISagaId> LocateSagaAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var id = domainEvent.Metadata[MetadataKey];
            return Task.FromResult(_sagaIdGenerator(id));
        }
    }
}