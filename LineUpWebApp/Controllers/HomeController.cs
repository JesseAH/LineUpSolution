using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LineUpLibrary.DTOs;
using LineUpLibrary.DALs;

namespace LineUpWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //UserDAL myUserDAL = new UserDAL();
            //IEnumerable<userDTO> x = myUserDAL.GetList();
            //RoundDTO test = new RoundDTO();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Create()
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
