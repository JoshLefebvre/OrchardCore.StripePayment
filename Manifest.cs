using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "StripePayment",
    Author = "LefeWare Solutions",
    Website = "https://orchardproject.net",
    Version = "1.0.0",
    Category = "Payment"
)]

[assembly: Feature(
    Id = "OrchardCore.StripePayment",
    Name = "StripePayment",
    Category = "Payment",
    Description = "Allows users to use stripe as their payment method",
    Dependencies = new[]
    {
        "LefeWareSolutions.Payments",
    }
)]

[assembly: Feature(
    Id = "Stripe.Subscriptions",
    Name = "StripeSubscription",
    Category = "Payment",
    Description = "Allows users to use stripe for payment subscriptions",
    Dependencies = new[]
    {
        "OrchardCore.TenantBilling",
    }
)]
