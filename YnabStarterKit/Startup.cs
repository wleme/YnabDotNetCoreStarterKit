using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace YnabStarterKit
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            this._config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                  .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<Services.Ynab>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "YnabStarterKit";
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "YnabStarterKit";
            })
            .AddOAuth("YnabStarterKit", options =>
            {
                options.ClientId = _config["Ynab:ClientId"];
                options.ClientSecret = _config["Ynab:ClientSecret"];
                options.CallbackPath = new PathString("/signin-ynab");

                options.AuthorizationEndpoint = _config["Ynab:Uri:AuthorizationEndpoint"];
                options.TokenEndpoint = _config["Ynab:Uri:TokenEndpoint"];
                options.UserInformationEndpoint = _config["Ynab:Uri:UserInformationEndpoint"];

                options.SaveTokens = true;

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {

                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                        var refreshToken = context.RefreshToken;
                        var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        var user = JObject.Parse(await response.Content.ReadAsStringAsync());
                        var ynabId = user.SelectToken("data.user.id").Value<string>();

                        //store the id in the cookie
                        context.Identity.AddClaim(new Claim(ClaimTypes.Sid, ynabId, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                    }
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=budgets}/{action=Index}/{id?}");
            });
        }
    }
}
