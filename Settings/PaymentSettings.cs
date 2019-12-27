using Microsoft.AspNetCore.Http;

namespace LefeWareLearning.StripePayment.Settings
{
    public class PaymentSettings
    {
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public PathString CallbackPath { get; set; }
    }
}
