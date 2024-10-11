using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class AssignPromotionPackage
    {
        public int PPCID { get; set; }

        public string PromotionPackageName { get; set; }

        public int userId { get; set; }

        public int CreatedBy { get; set; }

    }

    public class AssignPromotionPackageDTO
    {
        public List<AssignPromotionPackage> assignPromotionPackages { get; set; }


        public List<Register> Users { get; set; }

    }


    public class GetAllUsersPromotionPackage
    {
        public string Username { get; set; }
        public string PromotionPackageName { get; set; }

        public int? PackageCount { get; set; }
        public DateTime SubscriptionDate { get; set; }

       public DateTime CreatedOn { get; set; }

    }

}
