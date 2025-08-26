using poc_az_kv_on_legacy_dotnet_mvc.Services.KeyVault;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace poc_az_kv_on_legacy_dotnet_mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IKeyVaultService _keyVault;

        public HomeController(IKeyVaultService keyVault)
        {
            _keyVault = keyVault;
        }

        public async Task<ActionResult> Index()
        { 
            // Read connection string named "DefaultConnection" (replace with your actual name)
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
            ViewBag.ConnectionString = connStr;
            ViewBag.SecretFromKeyVault = await _keyVault.GetSecretAsync("my-secret");
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