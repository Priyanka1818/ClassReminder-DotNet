using Login.model;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

namespace Login
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, Microsoft.Extensions.Hosting.IHostingEnvironment env)
        {
            Configuration = configuration;
        }
        //public IConfigurationRoot Configuration { get; }
        /*public Startup(Microsoft.Extensions.Hosting.IHostingEnvironment env)
        {
            var appsettings = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();
            Configuration = appsettings;
        }*/

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //ConnectionStrings con = new ConnectionStrings();
            ConnectionStrings con = new ConnectionStrings();
            Configuration.Bind("ConnectionStrings", con);
            services.AddSingleton(con);

            services.AddControllers();
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = ".Net Core 3 Web API", Version = "v1" });
                var filePath = Path.Combine(AppContext.BaseDirectory, "NetCore3WebAPI.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", ".Net Core 3 Web API V1");
            });
        }
    }
}

