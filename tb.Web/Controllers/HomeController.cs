using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using tb.Web.ViewModels;

using tb.Core.Services;


namespace tb.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private IUserService _svc;

        public HomeController(ILogger<HomeController> logger, IUserService ss)
        {
            _logger = logger;
            _svc = ss;
        }
         

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secure(int id)
        {
        
            var user = _svc.GetUser(GetSignedInUserId());

           if( user.Adult != true )
            {
                
                Alert($"Bookings may only be peformed by students aged over 18", AlertType.warning); 
                return RedirectToAction("Index","Student");
            
            }
            return View();
        }

        public IActionResult Privacy()
        {
        
            return View();
        }

        

        public IActionResult About()
        {
            var about = new AboutViewModel
            {
                Title = "About",
                Message = "A Student Management system for independent music tutors",
                Formed = new System.DateOnly(2022,02,8)   
            };
            return View(about);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
