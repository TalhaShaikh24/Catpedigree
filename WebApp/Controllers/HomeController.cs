using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        } 
        public IActionResult Advertising()
        {
            return View();
        } 
        public IActionResult Enhancement()
        {
            return View();
        } 
        public IActionResult CatteryBanners()
        {
            return View();
        } 
        public IActionResult BusinessAdvertising()
        {
            return View();
        }
        public IActionResult HowToPaceYourListing()
        {
            return View();
        }
        public IActionResult WhyCatPedigree()
        {
            return View();
        } 
        public IActionResult Login()
        {
            return View();
        } 
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult UserRegistration()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Terms()
        {
            return View();
        }  
        public IActionResult Gallery()
        {
            return View();
        }

        public IActionResult CatMediation()
        {
            return View();
        }
        public IActionResult BreederMediation()
        {
            return View();
        }
        public IActionResult We_are_almost_Live()
        {
            return View();
        }
        public IActionResult Coaching()
        {
            return View();
        }
        public IActionResult Banners()
        {
            return View();
        }
        public IActionResult HouseRules()
        {
            return View();
        }
        public IActionResult PoisonousPlants()
        {
            return View();
        }
        public IActionResult Breeds()
        {
            return View();
        }
        public IActionResult Others()
        {
            return View();
        }
        public IActionResult Shows()
        {
            return View();
        }
        public IActionResult PedigreeService()
        {
            return View();
        }
        public IActionResult Studs()
        {
            return View();
        }
        public IActionResult Video()
        {
            return View();
        }
        public IActionResult WhatDoWeDo()
        {
            return View();
        }
        public IActionResult Relocation()
        {
            return View();
        }
        public IActionResult WhyDidWeStart()
        {
            return View();
        }
        public IActionResult WhatToCheckFor()
        {
            return View();
        }
        public IActionResult FelineHealth()
        {
            return View();
        }
        public IActionResult CostOfPedigreeCat()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult PackageDeals()
        {
            return View();
        }
        public IActionResult FelineFood()
        {
            return View();
        }
        public IActionResult FelineCare()
        {
            return View();
        }  
        public IActionResult PaymentOptions()
        {
            return View();
        }  
        public IActionResult UsefulLinks()
        {
            return View();
        }
        public IActionResult PedigreeInfo()
        {
            return View();
        }
        public IActionResult Kittens()
        {
            return View();
        }


        public IActionResult Advertisementpackages()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
