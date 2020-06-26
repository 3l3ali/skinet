using API.Extentions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
            services.AddControllers();
            services.AddDbContext<StoreContext>(x => x.UseSqlite(_config.GetConnectionString("DefaultConnection")));

            services.AddSingleton<ConnectionMultiplixer>(config => {
                var configuration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);
                return ConnectionMultiplixer.Connect(configuration);
            }); //Add Redis

            services.AddAutoMapper(typeof(MappingProfiles)); //Maping Entites to Dtos 
            services.AddApplicationServices(); //in Api/Extentions
            services.AddSwaggerDocumentation(); //in Api/Extentions
            services.AddCors(option => {     //Cross Origin Research Support 
                option.AddPolicy("CorsPolicy", policy => {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            app.UseMiddleware<ExceptionMiddleware>(); // middleware in api/middleware to handle exceptions in both dev and prod

            app.UseStatusCodePagesWithReExecute("/errors/{0}"); // no end point requests go to errors controller

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles(); //for wwwroot to work

            app.UseCors("CorsPolicy"); //enabling CORS 

            app.UseAuthorization();

            app.UseSwaggerDocumentation(); //in Api/Extentions

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
