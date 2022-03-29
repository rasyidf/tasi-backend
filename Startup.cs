using System;
using System.Reflection;
using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.EntityFrameworkCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TASI.Backend.Domain.Users.Mappers;
using TASI.Backend.Infrastructure.Configs;
using TASI.Backend.Infrastructure.Database;
using TASI.Backend.Infrastructure.Filters;
using TASI.Backend.Infrastructure.Registrations;
using TASI.Backend.Infrastructure.Services;

namespace TASI.Backend
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            foreach (var a in Configuration.AsEnumerable())
            {
                Console.WriteLine($"{a.Key}={a.Value}");
            }

            services.AddOptions();
            services.AddMemoryCache();
            services.AddAutoMapper(typeof(UserDomainMapperProfile));
            services.AddMediatR(Assembly.GetExecutingAssembly());
            
            services.AddDbContext<TasiContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("MainConnection"));
            });
            
            if (_environment.IsDevelopment())
            {
                services.AddDatabaseDeveloperPageExceptionFilter();
            }

            services.Configure<JwtConfig>(Configuration.GetSection(nameof(JwtConfig)));
            services.Configure<BingMapsConfig>(Configuration.GetSection(nameof(BingMapsConfig)));
            services.Configure<DefaultTasiConfig>(Configuration.GetSection(nameof(DefaultTasiConfig)));

            services.AddControllers()
                .AddCustomNewtonsoftJson()
                .AddCustomValidator();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddCustomCors();
            services.AddCustomAuth(Configuration);
            services.AddCustomSwagger("TASI Backend API", "v1");
            services.AddCustomHealthChecks();

            services.AddHttpClient<IBingMapsService, BingMapsService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAuthorizationEvaluator, CustomAuthorizationEvaluator>();
            services.AddScoped<IBingMapsService, BingMapsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseElasticApm(Configuration, new HttpDiagnosticsSubscriber(), new EfCoreDiagnosticsSubscriber());
            app.UseHttpsRedirection();
            app.UseCustomSerilog();

            app.UseCustomSwagger("TASI Backend API", "v1");
            //app.UseCustomHealthChecks();
            app.UseCustomResponseWrapper(env);

            app.UseRouting();
            app.UseCustomCors();
            app.UseCustomAuth();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapCustomHealthChecks();
                endpoints.MapControllers();
            });

            app.UseCustomNotFoundMiddleware();
        }
    }
}
