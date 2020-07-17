using System;
using OrchardCore.TenantBilling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Security.Permissions;
using Stripe;
using LefeWareLearning.StripePayment;
using OrchardCore.Data.Migration;

namespace OrchardCore.StripePayment
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPermissionProvider, Permissions>();
        }
    }

    [Feature(StripePaymentConstants.Features.StripePayment)]
    public class StripePaymentStartup : StartupBase
    {
        private readonly IConfiguration _configuration;

        public StripePaymentStartup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<StripeConfigurationOptions>(_configuration.GetSection("Stripe"));
            services.AddScoped<IDataMigration, StripePaymentMigrations>();
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            //TODO: Need to add setting to add this as part of OC settings
            StripeConfiguration.ApiKey = _configuration["Stripe:StripeAPIKey"];
        }
    }
}
