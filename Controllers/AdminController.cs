using System.Collections.Generic;
using System.Threading.Tasks;
using LefeWareLearning.TenantBilling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OrchardCore.Environment.Shell;
using OrchardCore.Modules;
using Stripe;
using Stripe.Checkout;

namespace LefeWareLearning.StripePayment
{
    [Feature(StripePaymentConstants.Features.StripePayment)]
    public class AdminController : Controller
    {
        private const long AMOUNT = 4499;

        private readonly IAuthorizationService _authorizationService;
        private readonly StripeConfigurationOptions _options;
        private readonly ShellSettings _shellSettings;

        public AdminController(IAuthorizationService authorizationService, IOptions<StripeConfigurationOptions> options, ShellSettings shellSettings)
        {
            _authorizationService = authorizationService;
            _options = options.Value;
            _shellSettings = shellSettings;
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
                        { "TenantId", _shellSettings["Secret"]},
                        { "TenantName", _shellSettings.Name }
                    }
                },
                SuccessUrl = $"https://{HttpContext.Request.Host.Value}/{_shellSettings.Name}/LefeWareLearning.StripePayment/admin/paymentsuccess?sessionid={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"https://{HttpContext.Request.Host.Value}/{_shellSettings.Name}/admin",
            };

            var service = new SessionService();

            var session = service.Create(options);

            ViewBag.SessionId = session.Id;
            return View();
        }

        public async Task<IActionResult> PaymentSuccess(string sessionId)
        {
            return View();
        }


    }
}
