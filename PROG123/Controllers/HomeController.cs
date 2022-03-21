using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PROG123.DAL;
using PROG123.Models;
using PROG123.utils;

namespace PROG123.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
           
            return View();
        }

        // add your actions here 
        public IActionResult Page2(PersonModel personModel)
        {
            DALPerson dp = new DALPerson(_configuration);
            string PersonID= dp.AddPerson(personModel);

            personModel.PersonID = PersonID;

            HttpContext.Session.SetString("PersonID", PersonID.ToString());

            string strUID = HttpContext.Session.GetString("PersonID");

            return View(personModel);
        }
        public IActionResult EditPerson()
        {
            string PersonID = Convert.ToString( HttpContext.Session.GetString("PersonID"));

            DALPerson dp = new DALPerson(_configuration);
            PersonModel personModel = dp.getPerson(PersonID);

            
            return View(personModel);
        }
        public IActionResult UpdatePerson(PersonModel personModel)
        {
            string PersonID = Convert.ToString(HttpContext.Session.GetString("PersonID"));
            personModel.PersonID = PersonID;

            DALPerson dp = new DALPerson(_configuration);
            dp.UpdatePerson(personModel);



            return View("Page2", personModel);
        }

        public IActionResult DeletePerson()
        {
            string PersonID = Convert.ToString(HttpContext.Session.GetString("PersonID"));
            
            DALPerson dp = new DALPerson(_configuration);

            PersonModel personModel = dp.getPerson(PersonID);

            dp.DeletePerson(PersonID);

            return View(personModel);
        }



    }
}
