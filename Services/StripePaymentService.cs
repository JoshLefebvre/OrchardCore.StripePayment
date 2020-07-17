using System.Collections.Generic;
using System.Threading.Tasks;
using LefeWareSolutions.Payments.Models;
using Stripe;

namespace OrchardCore.StripePayment.Services
{
    public class StripePaymentService
    {
        public async Task<PaymentIntent> CreatePaymentIntent(PaymentPart paymentPart)
        {
            StripeConfiguration.ApiKey = "";

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)paymentPart.Amount.Value,
                Currency = "usd",
                // Verify your integration in this guide by including this parameter
                Metadata = new Dictionary<string, string>
                {
                  { "integration_checkk", "accept_a_paymentt" },
                },
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);

            return paymentIntent;
        }
    }
}
