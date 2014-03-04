using System.Web.Http;
using Scarf;
using Scarf.WebApi;

namespace DemoWebsite.Controllers
{
    public class DemoController : ApiController
    {
        [LogApiAccess(MessageType.AccessRead, SaveAdditionalInfo = false)]
        public string[] GetTodaysMenu()
        {
            return new string[]
            {
                "Pepperoni Pizza",
                "Nachos"
            };
        }
    }
}
