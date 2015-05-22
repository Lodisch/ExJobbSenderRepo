using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SenderPrototype.Startup))]
namespace SenderPrototype
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
