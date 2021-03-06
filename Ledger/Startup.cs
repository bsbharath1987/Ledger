﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using AutoMapper;
using Ledger.Data;
using Ledger.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Ledger
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LedgerDBContext>(options =>
                {
                    options.UseSqlServer("Server=LAPTOP-KUC56DD5\\SQLEXPRESS; Database=Ledger;Trusted_Connection=True;MultipleActiveResultSets=true");
                });
            
            services.AddAutoMapper();
            services.AddScoped<ILedgerRepository, LedgerRepository>();
            services.AddScoped<IVMFactory, VMFactory>();
            services.AddScoped<IMapper, Mapper>();
            // Add framework services.
            services.AddMvc();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            app.UseMvc();

        }
    }
}
