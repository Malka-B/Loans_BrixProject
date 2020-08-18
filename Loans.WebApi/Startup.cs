using AutoMapper;
using Loans.Data;
using Loans.Data.Profiles;
using Loans.Service;
using Loans.Service.Interfaces;
using Loans.Service.Profiles;
using Loans.WebApi.Middleware;
using Loans.WebApi.Profiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Loans.WebApi
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
            services.AddScoped<ILoanService, LoanService>();
            services.AddScoped<IManagerLoanService, ManagerLoanService>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IManagerLoanRepository, ManagerLoanRepository>();
            services.AddDbContext<LoanContext>(options =>
                          options.UseSqlServer(
                              Configuration.GetConnectionString("Loans")));
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new LoanProfile());
                mc.AddProfile(new LoanRepositoryProfile());
                mc.AddProfile(new LoanServiceProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                    "LoansOpenAPISpecification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "Loans API",
                        Version = "1"
                    });
            });
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware(typeof(LoanErrorsHandlerMiddleware));
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    "/swagger/LoansOpenAPISpecification/swagger.json",
                    "Loans API"
                    );
            });
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
