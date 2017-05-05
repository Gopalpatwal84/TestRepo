namespace PowerBI.Jobs.Web.API.Controllers
{
    using System.Web.Http;

    public class PingController : ApiController
    {
        public string Get()
        {
            return "pong";
        }
    }
}