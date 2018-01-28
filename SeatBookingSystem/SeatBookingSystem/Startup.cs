using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SeatBookingSystem.Startup))]
namespace SeatBookingSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
