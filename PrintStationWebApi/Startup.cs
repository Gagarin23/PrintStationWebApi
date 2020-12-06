using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrintStationWebApi.Logger;
using PrintStationWebApi.Models;
using PrintStationWebApi.Models.BL;
using PrintStationWebApi.Models.DataBase;
using PrintStationWebApi.Services;
using PrintStationWebApi.Services.BL;
using PrintStationWebApi.Services.Cache;
using PrintStationWebApi.Services.DataBase;

namespace PrintStationWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BooksContext>(options =>
                options.UseSqlServer(connection));

            services.AddControllers();
            services.AddMemoryCache();

            services.AddScoped<IValidateService, ValidateService>();
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddScoped<IBookSortingService, BookSortingService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PrintStation", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PrintStation v1"));
            }

            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
