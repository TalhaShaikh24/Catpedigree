using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class CouponCodes
    {

        public int DiscountPercentage { get; set; }
        public int CouponID { get; set; }

        public string? CouponCode { get; set; }

        public string? CouponName { get; set; }

        public int UserId { get; set; }

        public int? CouponsDays { get; set; }

        public string UserName { get; set; }

        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }
        public int CreatedBy { get; set; }


        public string? UsedBy { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
