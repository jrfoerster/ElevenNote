using System.Net.Http.Headers;
using System.Web.Http;

namespace ElevenNote.WebMvc.App_Start
{
    public class WebApiConfig
    {
        public static void Register()
        {
            GlobalConfiguration.Configure(x =>
            {
                x.Formatters
                .JsonFormatter
                .SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("text/html"));

                x.MapHttpAttributeRoutes();
            });
        }
    }
}