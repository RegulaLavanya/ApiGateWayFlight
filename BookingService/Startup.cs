using BookingService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Microsoft.Extensions.ObjectPool;

namespace BookingService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookingService", Version = "v1" });
            });

          //  var rabbitConfig = Configuration.GetSection("rabbit");
          //  services.Configure<Rabbitsettings>(rabbitConfig);

          //  services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
         //   services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();

           // services.AddSingleton<IRabbitManager, RabbitManager>();

            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer
            (@"Server=(localdb)\mssqllocaldb;Initial Catalog=AirlineManagement;Integrated Security=True;Trusted_Connection=True;"));
            //(@"Server=tcp:bookingservicedbserver1.database.windows.net,1433;Initial Catalog=BookingService_db;Persist Security Info=False;User ID=Lavanya;Password=N@ncy424;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookingService v1"));
            }

            app.UseCors(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
