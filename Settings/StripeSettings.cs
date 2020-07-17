using Microsoft.AspNetCore.Http;

namespace LefeWareLearning.StripePayment.Settings
{
    public class StripeSettings
    {
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public PathString CallbackPath { get; set; }
    }
}