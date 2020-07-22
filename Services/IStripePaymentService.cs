using System.Threading.Tasks;
using LefeWareSolutions.Payments.Models;
using Stripe;

namespace OrchardCore.StripePayment.Services
{
    public interface IStripePaymentService
    {
        Task<PaymentIntent> CreatePaymentIntent(PaymentPart paymentPart);
    }
}