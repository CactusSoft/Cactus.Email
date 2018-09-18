using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cactus.Email.Core.Utils;
using Cactus.Email.Simple.Services;
using Cactus.Email.Smtp;
using Cactus.Email.Smtp.Configurations;
using Cactus.Email.Templates.EntityFraemwork.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Cactus.Email.Templates.EntityFraemwork.Repositories;
using Cactus.Email.Templates.Razor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cactus.Email.Simple
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {         
            services.AddDbContext<TemplatesDbContext>(x => x.UseSqlServer("Server=POTOTSKY\\SQLEXPRESS;Database=Email1;Trusted_Connection=True;MultipleActiveResultSets=True;Integrated Security=true"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<TemplatesRepository>().AsImplementedInterfaces();
            containerBuilder.RegisterType<SubjectTemplateParser>().AsImplementedInterfaces();
            containerBuilder.RegisterType<TemplatesService>().AsImplementedInterfaces();
            containerBuilder.RegisterType<RazorTemplateRenderer>().AsImplementedInterfaces();
            containerBuilder.RegisterType<EmailSender>().AsImplementedInterfaces();


            containerBuilder.Register(x => new SmtpSender(new SmtpConfiguration("smtp.gmail.com", 587, "sometestemail@gmail.com", "1234", null, true))).AsImplementedInterfaces();

            containerBuilder.Populate(services);

            var applicationContainer = containerBuilder.Build();
            return new AutofacServiceProvider(applicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<TemplatesDbContext>();
                context.Database.EnsureCreated();
            }
            app.UseMvc();
        }
    }
}
