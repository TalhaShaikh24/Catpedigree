using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class StripeSettings
    {
        public string? PublishableKey { get; set; }
        public string? SecretKey { get; set; }
    }

    public class ExchangeRateApiSettings
    {
        public string? ApiKey { get; set; }
        public string? ApiUrl { get; set; }
    }

    public class CheckoutSessionRequest
    {
        public string? PriceId { get; set; }
        public string? packageType { get; set; }

        public int PurchasedProductID { get; set; }

        public int? Days { get; set; }
    }
    // Request model
    public class CouponAndPromotionRequest
    {
        public string? Name { get; set; } // This will be used for both the coupon and the promotion code name
        public long AmountOff { get; set; } // Discount amount in smallest currency unit (e.g., cents for USD)
        public string? Currency { get; set; } // Currency for the discount
      //  public long? MaxRedemptions { get; set; } // Maximum number of redemptions for the promotion code
        public List<string>? AllowedUsers { get; set; } // List of allowed users (emails or user IDs)
        public DateTime? ExpiresAt { get; set; } // Optional expiry date for both the coupon and promotion code
    }

    public class PromotionCodeDto
    {
        public string? Id { get; set; }
        public string? Code { get; set; }
        public DateTime? ExpiresAt { get; set; } // Expiry date of the promotion code
        public long? AmountOff { get; set; } // Amount off from the coupon
        public decimal? PercentOff { get; set; } // Amount off from the coupon
        public string? Currency { get; set; } // Currency of the coupon
        public string? Name { get; set; } // Name of the coupon
        public bool IsActive { get; set; } // Name of the coupon

        public string? CoupenCodeID { get; set; }
        public Dictionary<string, string>? Metadata { get; set; } // Metadata associated with the promotion code
  
    }
}
