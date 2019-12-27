using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "StripePayment",
    Author = "LefeWareLearning",
    Website = "https://orchardproject.net",
    Version = "1.0.0",
    Category = "LefeWare Learning"
)]

[assembly: Feature(
    Id = "LefeWareLearning.StripePayment",
    Name = "StripePayment",
    Category = "LefeWare Learning Payment Types",
    Description = "Allows users to use stripe as their payment methods",
    Dependencies = new[]
    {
        "LefeWareLearning.TenantBilling",
    }
)]
