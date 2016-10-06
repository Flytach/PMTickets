using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PMTickets.Startup))]
namespace PMTickets
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
