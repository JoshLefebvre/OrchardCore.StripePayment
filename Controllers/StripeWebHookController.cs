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
            //TODO: Add this somewhere global
            // if (!IsDefaultShell())
            // {
            //     return Unauthorized();
            // }

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _options.WebhookSecret);

                switch (stripeEvent.Type)
                {
                    case Events.CustomerSubscriptionCreated:
                    {
                        //Create Stripe Customer Index
                        var subscription = stripeEvent.Data.Object as Subscription; 

                        break;
                    }
                    case Events.InvoicePaymentSucceeded:
                    {
                        //Create payment
                        var invoice = stripeEvent.Data.Object as Invoice; 
                        var tenantId = invoice.Lines.Data[0].Metadata["TenantId"];
                        var amount = invoice.Lines.Data[0].Plan.AmountDecimal.Value;

                        var paymentSuccessEventHandlers = _serviceProvider.GetRequiredService<IEnumerable<IPaymentSuccessEventHandler>>();
                        await paymentSuccessEventHandlers.InvokeAsync(x => x.PaymentSuccess(tenantId, DateTime.Now, amount), _logger);
                        break;
                    }
                    case Events.InvoicePaymentFailed:
                    {
                        //Handle Failed Payment
                         var subscription = stripeEvent.Data.Object as Invoice;

                        _logger.LogError("Unable to process ");
    
                        break;
                    }
                }
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}

