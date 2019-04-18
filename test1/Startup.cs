using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Entity.Models;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using test1.Filter;
using Until.TokenHelper;

namespace test1
{
    public class Startup
    {
        public static ILoggerRepository Repository { get; set; }//注入log4net
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Repository = LogManager.CreateRepository("test");
            XmlConfigurator.Configure(Repository, new FileInfo("log4netConfig"));

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors();//注册跨域
            services.AddMvc(o=> 
            {
                o.Filters.Add(typeof(GlobalExceptionsFilter));
            });//注入全局异常捕捉
            #region JWT解析验证注册
            services.AddAuthentication(t =>
            {
                t.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                t.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,//验证安全key
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["securityKey"].ToString())),//获取安全key
                    ValidateIssuer = true,//验证发行人
                    ValidIssuer = Configuration["issuer"],//发行人
                    ValidateAudience = true,//验证订阅者
                    ValidAudience =Configuration["audience"],// 
                    ValidateLifetime = true,//验证过期时间
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                };
            });
            #endregion
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = Context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            string ConStr = GetConnectStr();
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(ConStr).ConfigureWarnings(warning=>warning.Throw(CoreEventId.IncludeIgnoredWarning)));//显式抛出EFCORE INCLUDE错误
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            return RegisterAutofac(services);//AutoFac接管自带容器
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
            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
            });
            //app.UseMiddleware<JwtCustomerAuthorizeMiddleware>();//添加拦截器验证JWT中间件
            app.UseAuthentication();
            app.UseMvc();
        }
        #region autofac帮助类
        /// <summary>
        /// autofac类
        /// </summary>
        public class AutofacModuleRegister : Autofac.Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterAssemblyTypes(GetAssemblyByName("Services")).Where(t => t.Name.EndsWith("Services")).AsImplementedInterfaces();//注册服务层所有已Services结尾的实现类
                builder.RegisterAssemblyTypes(GetAssemblyByName("test1")).Where(t => t.Name.EndsWith("Helper")).AsImplementedInterfaces();
                builder.RegisterAssemblyTypes(GetAssemblyByName("Repository")).Where(a => a.Name.EndsWith("Repository")).AsImplementedInterfaces();//注册仓储层所有已Repository结尾的实现类
                builder.RegisterAssemblyTypes(GetAssemblyByName("Redis")).Where(a => a.Name.EndsWith("Helper")).AsImplementedInterfaces();//注册Redis
            }
        }
        public static Assembly GetAssemblyByName(string name)
        {
            return Assembly.Load(name);
        }
        #endregion
        #region 获取连接字符串
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
        #endregion
    }
}
