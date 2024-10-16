using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class UsefulLinks:Common
    {
        public int Id { get; set; }
        public IFormFile? UsefulLinkFile { get; set; }
        public string? UsefulLinkFilePath { get; set; }
        public string Url { get; set; }
    }
}
