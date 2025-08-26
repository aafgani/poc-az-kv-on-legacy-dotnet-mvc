using poc_az_kv_on_legacy_dotnet_mvc.Services.KeyVault;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace poc_az_kv_on_legacy_dotnet_mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        { 
            // Read connection string named "DefaultConnection"
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
            string appSettingKey = ConfigurationManager.AppSettings["MyAppSettingKey"];
            ViewBag.ConnectionString = connStr;
            ViewBag.AppSettingKey = appSettingKey;
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