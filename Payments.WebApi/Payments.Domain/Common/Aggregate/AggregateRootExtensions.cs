using System.Reflection;
using EventFlow.Aggregates;
using EventFlow.Core;

namespace Payments.Domain.Common.Aggregate
{
    public static class AggregateRootExtensions
    {
        public static Metadata MetadataFor<TAggregate, TIdentity>(this AggregateRoot<TAggregate, TIdentity> aggregateRoot, dynamic properties)
            where TAggregate : AggregateRoot<TAggregate, TIdentity>
            where TIdentity : IIdentity
        {
            var metadata = new Metadata();
            foreach (PropertyInfo pi in properties.GetType().GetProperties())
            {
                metadata[pi.Name] = pi.GetValue(properties).ToString();
            }

            return metadata;
        }
    }
}