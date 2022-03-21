using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PROG123.DAL;
using PROG123.Models;

namespace PROG123.Controllers
{
    public class AzmyController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public AzmyController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LogIn(LogInCredentialsModel logInCredentialsModel)
        {
            DALPerson dp = new DALPerson(_configuration);
            PersonModel personModel = dp.CheckLogInCredentials(logInCredentialsModel);

            if(personModel == null)
            {
                ViewBag.LoginMessage = "Login failed, please try again";
                ViewBag.LoginForm = true;
            }
            else
            {
                

                HttpContext.Session.SetString("UserName", personModel.UserName.ToString());
                HttpContext.Session.SetString("FName", personModel.FName);

                ViewBag.UserFirstName = personModel.FName;
                ViewBag.LoginForm = false;
                ViewBag.LogIn = string.Empty;
  
            }

            return View("Index");
        }
        public IActionResult EnterNewProduct()
        {
            ViewBag.LoginForm = string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"));

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("FName")))
            {
                ViewBag.UserFirstName = HttpContext.Session.GetString("FName");
            }
            if (ViewBag.LoginForm)
            {
                ViewBag.LoginMessage = "User is not logged in.";
                return View("Index");
            }


            return View();
        }
        public IActionResult AddProductToDB(ProductModel productModel)
        {

            ViewBag.LoginForm = string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"));

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("FName")))
            {
                ViewBag.UserFirstName = HttpContext.Session.GetString("FName");
            }

            DALProducts dp = new DALProducts(_configuration);
            string PID = dp.AddNewProduc(productModel);

            productModel.PID = PID;





            return View(productModel);
        }
        public IActionResult ListAllProducts()
        {
            ViewBag.LoginForm = string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"));

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("FName")))
            {
                ViewBag.UserFirstName = HttpContext.Session.GetString("FName");
            }
            if (ViewBag.LoginForm)
            {
                ViewBag.LoginMessage = "User is not logged in.";
                return View("Index");
            }

            DALProducts dp = new DALProducts(_configuration);
            LinkedList<ProductModel> productModel =  dp.GetAllProducts();




            return View(productModel);
        }

        public IActionResult OneClickBuy(string PID)
        {
            ViewBag.LoginForm = string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"));

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("FName")))
            {
                ViewBag.UserFirstName = HttpContext.Session.GetString("FName");
            }
            if (ViewBag.LoginForm)
            {
                ViewBag.LoginMessage = "User is not logged in.";
                return View("Index");
            }

            DALPerson personDal = new DALPerson(_configuration);
            PersonModel personModel = personDal.getPerson(HttpContext.Session.GetString("PersonID"));

            DALProducts productDal = new DALProducts(_configuration);
            ProductModel productModel = productDal.GetProduc(PID);
            productDal.UpdateInventory(PID, 1);

            

            

            SalesTransactionModel salesTransactionModel = new SalesTransactionModel()
            {
                PersonID = personModel.PersonID,
                ProductID = productModel.PID,
                SalesDataTime = DateTime.Now,
                PQuantity = 1
            };

            

            PurchaseModel purchaseModel = new PurchaseModel()
            {
                PersonModel = personModel,
                ProductModel = productModel,
                
            };

            return View(purchaseModel);

           
        }
    }

}
