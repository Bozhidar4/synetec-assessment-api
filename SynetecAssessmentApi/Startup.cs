using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SynetecAssessmentApi.ExceptionHandler;
using SynetecAssessmentApi.Persistence;
using SynetecAssessmentApi.Persistence.DAL;
using SynetecAssessmentApi.Persistence.DAL.Interfaces;
using SynetecAssessmentApi.Services;
using SynetecAssessmentApi.Services.Interfaces;

namespace SynetecAssessmentApi
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
            services.AddControllers(options =>
            {
                options.Filters.Add<HttpResponseExceptionFilter>();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SynetecAssessmentApi", Version = "v1" });
            });

            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "HrDb"));

            services.AddScoped<IBonusPoolService, BonusPoolService>();
            services.AddScoped<ICalculationService, CalculationService>();
            services.AddScoped<IDataAccess, DataAccess>();
            services.AddScoped<IMappingService, MappingService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SynetecAssessmentApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
