using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
  

    public class MollieTransaction
    {
        public int TransactionID { get; set; }
        public string? PaymentID { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
        public string? Method { get; set; }
        public string? CustomerID { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? CanceledAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string? WebhookURL { get; set; }
        public string? RedirectURL { get; set; }
        public string? PackageType { get; set; }

        public string? name { get; set; }
        public string? email { get; set; }

        public string? subscriptionId { get; set; }

        public string interval { get; set; }




    }

    public class MandateRequest
    {
        public string? Method { get; set; }
        public MandateConsumerName? ConsumerName { get; set; }
        public MandateConsumerAccount? ConsumerAccount { get; set; }
    }

    public class MandateConsumerName
    {
        public string? ConsumerName { get; set; }
    }

    public class MandateConsumerAccount
    {
        public string ?ConsumerAccount { get; set; }
    }
    public class MandateResponse
    {
        public string? Id { get; set; }
        public string? Status { get; set; }
        // Add other fields as needed
    }


}
