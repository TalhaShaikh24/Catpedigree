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
   
}
