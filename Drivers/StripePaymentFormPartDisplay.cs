using System.Threading.Tasks;
using GraphQL;
using LefeWareSolutions.Payments.Models;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.StripePayment.Services;
using OrchardCore.StripePayment.ViewModels;

namespace OrchardCore.StripePayment
{
    public class StripePaymentFormPartDisplay : ContentPartDisplayDriver<StripePaymentFormPart>
    {
        private readonly IStripePaymentService _stripePaymentService;

        public StripePaymentFormPartDisplay(IStripePaymentService stripePaymentService)
        {
            _stripePaymentService = stripePaymentService;
        }

        public override async Task<IDisplayResult> DisplayAsync(StripePaymentFormPart stripePaymentFormPart, BuildPartDisplayContext context)
        {
            var paymentPart = stripePaymentFormPart.ContentItem.As<PaymentPart>();

            paymentPart = new PaymentPart() { Cost = 200 };
        
            var paymentIntent = await _stripePaymentService.CreatePaymentIntent(paymentPart);

            return Initialize<StripePaymentFormPartViewModel>("StripePaymentFormPart", m =>
            {
                m.IntentCleintSecret = paymentIntent.ClientSecret;
            })
            .Location("Detail", "Content:5")
            .Location("Summary", "Content:5");
        }
    }
}
