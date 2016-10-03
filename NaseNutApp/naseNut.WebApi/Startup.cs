using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using naseNut.WebApi.Models.Business.Services;

[assembly: OwinStartup(typeof(naseNut.WebApi.Startup))]

namespace naseNut.WebApi
{
    
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            ConfigureAuth(app);
            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);

            var roleService = new RoleService();
            var userService = new UserService();
            roleService.CreateRoles();
            userService.CreateAdminUser();
            var naseNEntitiesSyncService = new NaseNEntitiesSyncService();
            //naseNEntitiesSyncService.SetServerSyncConfiguration();
            //naseNEntitiesSyncService.SetClientSyncConfiguration();
            naseNEntitiesSyncService.ExecuteSyncTask();
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthAuthorizationServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new OAuthAuthorizationServerProvider()
            };
            app.UseOAuthAuthorizationServer(oAuthAuthorizationServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
