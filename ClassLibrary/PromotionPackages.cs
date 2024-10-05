using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class PromotionPackages : Common
    {
        public int PromotionPackagesID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public string? PropertiestoShow { get; set; }

        public string? Costs { get; set; }

        public List<PromotionCost>? promotionCosts { get; set; }

        public int UserID { get; set; }

        public int Days { get; set; }


        public string? CardNumber { get; set; }
        public int? expireMonth { get; set; }
        public int? expireYear { get; set; }

        public string? cvc { get; set; }

        public string? stripeSubscriptionId { get; set; }

        public int? PPCID { get; set; }

        public string? Details { get; set; }

        public int? PackageCount {  get; set; } 
        public string? PriceId {  get; set; } 

    }

    public class PromotionCost:Common {


        public int PromotionCostID { get; set; }

        public int DaysNumber { get; set; }


        public decimal Cost { get; set; }



    }

    public class PromotionsCostCur
    {

        public int Id { get; set; }

        public string? currency { get; set; }
    }

}
