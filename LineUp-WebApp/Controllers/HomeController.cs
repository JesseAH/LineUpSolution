using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LineUpLibrary.DALs;

namespace LineUp_WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            UserDAL myUserDAL = new UserDAL();
            //IEnumerable<userDTO> x = myUserDAL.GetList();
            //RoundDTO test = new RoundDTO();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Info about how to create a league here";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
