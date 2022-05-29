using InternshipAPI.Manager;
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
using System.Linq;
using System.Threading.Tasks;

namespace InternshipAPI
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

            services.AddControllers();
            // John
            services.AddDbContext<PersonContext>(opt => opt.UseSqlServer(@"Server=DESKTOP-8NVTJM1\SQLEXPRESS; Database=HovedOpgave; Trusted_Connection=True;MultipleActiveResultSets=true"));
            services.AddDbContext<ActivityContext>(opt => opt.UseSqlServer(@"Server=DESKTOP-8NVTJM1\SQLEXPRESS; Database=HovedOpgave; Trusted_Connection=True;MultipleActiveResultSets=true"));
            services.AddDbContext<CommentContext>(opt => opt.UseSqlServer(@"Server=DESKTOP-8NVTJM1\SQLEXPRESS; Database=HovedOpgave; Trusted_Connection=True;MultipleActiveResultSets=true"));
            services.AddDbContext<ActivityStatusContext>(opt => opt.UseSqlServer(@"Server=DESKTOP-8NVTJM1\SQLEXPRESS; Database=HovedOpgave; Trusted_Connection=True;MultipleActiveResultSets=true"));
            services.AddDbContext<StatusContext>(opt => opt.UseSqlServer(@"Server=DESKTOP-8NVTJM1\SQLEXPRESS; Database=HovedOpgave; Trusted_Connection=True;MultipleActiveResultSets=true"));
            services.AddDbContext<GroupContext>(opt => opt.UseSqlServer(@"Server=DESKTOP-8NVTJM1\SQLEXPRESS; Database=HovedOpgave; Trusted_Connection=True;MultipleActiveResultSets=true"));
            services.AddDbContext<GroupOfPeopleContext>(opt => opt.UseSqlServer(@"Server=DESKTOP-8NVTJM1\SQLEXPRESS; Database=HovedOpgave; Trusted_Connection=True;MultipleActiveResultSets=true"));
            services.AddDbContext<ActivityAndGroupsOfPeopleContext>(opt => opt.UseSqlServer(@"Server=DESKTOP-8NVTJM1\SQLEXPRESS; Database=HovedOpgave; Trusted_Connection=True;MultipleActiveResultSets=true"));

            // Søren
            /*
            services.AddDbContext<PersonContext>(opt => opt.UseSqlServer(@"Server=localhost; Database=HovedOpgave; User=SA; Password=Padgok-mizdok-4cakbe; MultipleActiveResultSets=true"));
            services.AddDbContext<ActivityContext>(opt => opt.UseSqlServer(@"Server=localhost; Database=HovedOpgave; User=SA; Password=Padgok-mizdok-4cakbe; MultipleActiveResultSets=true"));
            services.AddDbContext<CommentContext>(opt => opt.UseSqlServer(@"Server=localhost; Database=HovedOpgave; User=SA; Password=Padgok-mizdok-4cakbe; MultipleActiveResultSets=true"));
            services.AddDbContext<ActivityStatusContext>(opt => opt.UseSqlServer(@"Server=localhost; Database=HovedOpgave; User=SA; Password=Padgok-mizdok-4cakbe; MultipleActiveResultSets=true"));
            services.AddDbContext<StatusContext>(opt => opt.UseSqlServer(@"Server=localhost; Database=HovedOpgave; User=SA; Password=Padgok-mizdok-4cakbe; MultipleActiveResultSets=true"));
            */
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "InternshipAPI", Version = "v1" });
            });
            services.AddCors(options =>

            {

                options.AddPolicy("allowAnythingFromZealand",

                    builder =>

                        builder.WithOrigins("http://zealand.dk")

                            .AllowAnyHeader()

                            .AllowAnyMethod());

                options.AddPolicy("allowGetPut",

                    builder =>

                        builder.AllowAnyOrigin()

                        .WithMethods("GET", "PUT", "POST", "DELETE")

                        .AllowAnyHeader());

                options.AddPolicy("allowAnything", // similar to * in Azure

                    builder =>

                        builder.AllowAnyOrigin()

                            .AllowAnyMethod()

                            .AllowAnyHeader());

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InternshipAPI v1"));
            }

            app.UseRouting();

            app.UseCors("allowAnything");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
