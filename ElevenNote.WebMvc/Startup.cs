using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ElevenNote.WebMvc.Startup))]
namespace ElevenNote.WebMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
