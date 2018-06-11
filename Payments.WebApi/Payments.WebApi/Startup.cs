using System;
using EventFlow.Extensions;
using EventFlow.MsSql;
using EventFlow.MsSql.EventStores;
using EventFlow.MsSql.Extensions;
using EventFlow.MsSql.SnapshotStores;
using EventFlow.ServiceProvider.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application.Payments;
using Payments.Domain.Payments.Payments;
using Payments.Domain.Payments.Payments.ReadModels;
using Payments.Domain.Payments.Providers;
using Payments.Domain.Payments.Providers.Types;

namespace Payments.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IPaymentProviderFactory, PaymentProviderFactory>();
            services.AddTransient<IPaymentProvider, TestProvider2PaymentProvider>();
            services.AddTransient<IPaymentProvider, TestProvider1PaymentProvider>();
            services.AddTransient<Domain.Payments.Configuration.IConfigurationProvider, Domain.Payments.Configuration.ConfigurationProvider>();
            services.AddTransient<IPaymentsApplicationService, PaymentsApplicationService>();
            services.AddTransient<IOrdersApplicationService, OrdersApplicationService>();
            services.AddEventFlow(options => options
                .ConfigureMsSql(MsSqlConfiguration.New
                    .SetConnectionString(Configuration.GetConnectionString("EventFlowDb")))
                .UseMssqlEventStore()
                .UseMsSqlSnapshotStore()
                .UseMssqlReadModel<PaymentDetailsReadModel>()
                .AddDefaults(typeof(PaymentAggregate).Assembly));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            InitializeEventFlowDbStructure(serviceProvider);
            //THIS FEATURE IS MISSING
            //InitializeReadModels();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private void InitializeEventFlowDbStructure(IServiceProvider serviceProvider)
        {
            var msSqlDatabaseMigrator = serviceProvider.GetService<IMsSqlDatabaseMigrator>();
            EventFlowEventStoresMsSql.MigrateDatabase(msSqlDatabaseMigrator);
            EventFlowSnapshotStoresMsSql.MigrateDatabase(msSqlDatabaseMigrator);
        }
    }
}