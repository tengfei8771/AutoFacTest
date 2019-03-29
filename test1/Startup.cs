using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Entity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace test1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = Context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            string ConStr = GetConnectStr();
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(ConStr));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            return RegisterAutofac(services);
        }

        /// <summary>
        /// 注册autofac
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider RegisterAutofac(IServiceCollection services)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule<AutofacModuleRegister>();
            ILifetimeScope autoContainer = builder.Build();
            return new AutofacServiceProvider(autoContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        /// <summary>
        /// autofac类
        /// </summary>
        public class AutofacModuleRegister : Autofac.Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterAssemblyTypes(GetAssemblyByName("Services")).Where(t => t.Name.EndsWith("Services")).AsImplementedInterfaces();
                builder.RegisterAssemblyTypes(GetAssemblyByName("Repository")).Where(a => a.Name.EndsWith("Repository")).AsImplementedInterfaces();
            }
        }
        public static Assembly GetAssemblyByName(string name)
        {
            return Assembly.Load(name);
        }
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public string GetConnectStr()
        {
            string ConStr = "";
            string DbType = "";
            try
            {
                using (StreamReader fs = File.OpenText(Directory.GetCurrentDirectory() + "\\DBConfig.json"))
                {
                    using(JsonTextReader reader=new JsonTextReader(fs))
                    {
                        JObject t = (JObject)JToken.ReadFrom(reader);
                        DbType = t["DataType"].ToString();
                        ConStr = t[DbType].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return ConStr;
        }
    }
}
