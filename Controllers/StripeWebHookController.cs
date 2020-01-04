using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LefeWareLearning.TenantBilling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrchardCore.Modules;
using Stripe;

namespace LefeWareLearning.StripePayment.Controllers
{
    [Route("api/stripewebhook")]
    [ApiController]
    [IgnoreAntiforgeryToken, AllowAnonymous]
    public class StripeWebHookController : Controller
    {
        private readonly ILogger<StripeWebHookController> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly StripeConfigurationOptions _options;
        public StripeWebHookController(IServiceProvider serviceProvider, ILogger<StripeWebHookController> logger, IOptions<StripeConfigurationOptions> options)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _options = options.Value;
        }

        [HttpPost]
        [Route("sync")]
        public async Task<IActionResult> Sync()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _options.WebhookSecret);

                switch (stripeEvent.Type)
                {
                    case Events.ChargeSucceeded:
                    {
                        var billingDetails = stripeEvent.Data.Object as Stripe.Checkout.Session;
                        var paymentSuccessEventHandlers = _serviceProvider.GetRequiredService<IEnumerable<IPaymentSuccessEventHandler>>();
                        await paymentSuccessEventHandlers.InvokeAsync(x => x.PaymentSuccess("", DateTime.Now, 12), _logger);
                        break;
                    }
                    case Events.ChargeFailed:
                    {
                        var billingDetails = stripeEvent.Data.Object as Stripe.Checkout.Session;
                        break;
                    }
                }
            }
            catch (StripeException e)
            {
                return BadRequest(e);
            }

            return Ok();
        }
    }
}

