using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NguyenThuanThien_BigSchool.Startup))]
namespace NguyenThuanThien_BigSchool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
