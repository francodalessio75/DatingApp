using System;
using API.Extensions;
using API.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices( _config );

            //extension of IServiceColletciont that manages authorization with token
            services.AddIdentityServiceExtensions( _config);

            services.AddControllers();

            services.AddCors();

            
        }

        private void JwtBearerDefault(AuthenticationOptions obj)
        {
            throw new NotImplementedException();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<AuthenticationMiddleware>();
            
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            //position is essential for CORS poklicy to work properly
            app.UseCors( policy =>  policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));/*1*/
            app.UseAuthentication();/*2*/
            app.UseAuthorization();/*3*/
            /**************************/

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}