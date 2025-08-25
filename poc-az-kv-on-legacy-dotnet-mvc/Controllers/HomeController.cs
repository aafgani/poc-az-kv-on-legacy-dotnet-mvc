using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace poc_az_kv_on_legacy_dotnet_mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        { 
            // Read connection string named "DefaultConnection" (replace with your actual name)
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
            ViewBag.ConnectionString = connStr;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {

            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}