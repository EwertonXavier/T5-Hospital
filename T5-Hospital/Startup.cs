using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(T5_Hospital.Startup))]
namespace T5_Hospital
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
