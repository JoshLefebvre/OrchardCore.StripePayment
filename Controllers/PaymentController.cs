using System.Collections.Generic;
using System.Threading.Tasks;
using LefeWareLearning.TenantBilling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Modules;
using Stripe;
using Stripe.Checkout;

namespace LefeWareLearning.StripePayment
{
    [Feature(StripePaymentConstants.Features.StripePayment)]
    public class PaymentController : Controller
    {
        private const long AMOUNT = 4499;

        private readonly IAuthorizationService _authorizationService;
        private readonly StripeConfigurationOptions _options;

        public PaymentController(IAuthorizationService authorizationService, StripeConfigurationOptions options)
        {
            _authorizationService = authorizationService;
            _options = options;
        }

        public async Task<IActionResult> Index()
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageTenantBilling))
            {
                return Unauthorized();
            }

            StripeConfiguration.ApiKey = _options.StripeAPIKey;

            var options = new SessionCreateOptions
            {
                CustomerEmail = User.Identity.Name,
                PaymentMethodTypes = new List<string> { "card" },
                SubscriptionData = new SessionSubscriptionDataOptions
                {
                    Items = new List<SessionSubscriptionDataItemOptions>
                    {
                        new SessionSubscriptionDataItemOptions {Plan = _options.StripePlanKey},
                    },
                    Metadata = new Dictionary<string, string>()
                    {
                        { "TenantId","" }
                    }
                },
                SuccessUrl = $"https://{HttpContext.Request.Host.Value}/admin/success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"https://{HttpContext.Request.Host.Value}/admin",
            };

            var service = new SessionService();

            var session = service.Create(options);

            ViewBag.SessionId = session.Id;
            return View();
        }

        public async Task<IActionResult> AddPayment()
        {
            return View();
        }


    }
}
