using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Core;
using EventFlow.MsSql;
using EventFlow.Queries;
using Payments.Domain.Payments.ReadModels;

namespace Payments.Domain.Payments.Queries
{
    public class GetPaymentDetailsQuery : IQuery<PaymentDetailsReadModel>
    {
        public GetPaymentDetailsQuery(string externalId)
        {
            ExternalId = externalId;
        }

        public string ExternalId { get; }
    }

    public class GetPaymentDetailsQueryHandler : IQueryHandler<GetPaymentDetailsQuery, PaymentDetailsReadModel>
    {
        private readonly IMsSqlConnection _msSqlConnection;

        public GetPaymentDetailsQueryHandler(IMsSqlConnection msSqlConnection)
        {
            _msSqlConnection = msSqlConnection;
        }

        public async Task<PaymentDetailsReadModel> ExecuteQueryAsync(GetPaymentDetailsQuery query, CancellationToken cancellationToken)
        {
            var readModel = await _msSqlConnection.QueryAsync<PaymentDetailsReadModel>(
                    Label.Named("mssql-get-payment-details-read-model"),
                    cancellationToken,
                    $"SELECT * FROM [ReadModel-{nameof(PaymentDetailsReadModel)}] WHERE {nameof(PaymentDetailsReadModel.ExternalId)} = @{nameof(query.ExternalId)}",
                    new { query.ExternalId })
                .ConfigureAwait(false);

            return readModel.SingleOrDefault();
        }
    }
}