using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "StripePayment",
    Author = "LefeWareLearning",
    Website = "https://orchardproject.net",
    Version = "1.0.0",
    Category = "Payment"
)]

[assembly: Feature(
    Id = "OrchardCore.StripePayment",
    Name = "StripePayment",
    Category = "Payment",
    Description = "Allows users to use stripe as their payment methods",
    Dependencies = new[]
    {
        "OrchardCore.TenantBilling",
    }
)]
