using System.ComponentModel.DataAnnotations;

namespace LefeWareLearning.StripePayment.ViewModels
{
    public class PaymentSettingsViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "API key is required")]
        public string ClientID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "API secret key is required")]
        public string ClientSecret { get; set; }

        [RegularExpression(@"\/[-A-Za-z0-9+&@#\/%?=~_|!:,.;]+[-A-Za-z0-9+&@#\/%=~_|]", ErrorMessage = "Invalid path")]
        public string CallbackUrl { get; set; }
    }
}
