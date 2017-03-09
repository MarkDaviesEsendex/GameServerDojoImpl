using System.Web.Http;

namespace Server.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.SuppressDefaultHostAuthentication();
            config.MapHttpAttributeRoutes();
        }
    }
}
