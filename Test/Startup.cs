using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Test.DataTiers;
using Test.DataTiers.Repositories;
//using Microsoft.Extensions.DependencyInjection;
using ViewModels.Mapping;

namespace Test.API
{
    public class Startup
    {
        public static IConfiguration StaticConfig { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StaticConfig = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("ConStr")));
            //services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
           
            services.AddAutoMapper(c => c.AddProfile<AutoMapping>(), typeof(Startup));

            //services.AddScoped<ILoggerManager, LoggerManager>();

            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test.Api", Version = "v1" });
            });
        }

        public void ConfigureContainer(ContainerBuilder cbuilder)
        {
            cbuilder.RegisterGeneric(typeof(Repository<>))
            .As(typeof(IRepository<>))
            .InstancePerLifetimeScope();

            cbuilder.RegisterModule(new DataModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test.Api v1"));
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
