using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Radzen;
using Radzen.Blazor;
using Server.Hubs;
using BlazorApp.Data;
using BlazorApp.Models;

namespace BlazorApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // 設定ファイル読込
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddJsonFile("disettings.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            // サンプル画面で使うサービスをDIコンテナに登録
            services.AddSingleton<WeatherForecastService>();

            // Radzenで使用するサービスをDIコンテナに登録
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();

            // DIの有効期間がMVCモデルと異なるので注意が必要
            // Blazor ServerはURLが同じ場合はコンポーネント内の変数がその間保持される
            // (Submitボタン押下しても変数を保持している)
            // 注意点として/movie/createと/movie/create/1は同じURLとして扱われる
            // DIでインジェクションした変数も同じ扱い
            // DbContextはマルチスレッド非対応なので問題が起きる            
            //services.AddDbContext<BlazorAppContext>(options =>
            //      options.UseSqlite(Configuration.GetConnectionString("BlazorAppDatabase")));

            services.AddDbContextFactory<BlazorAppContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("BlazorAppDatabase")));

            // DIコンテナに対象クラスを登録
            RegisterDiContainer(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        /// <summary>
        /// DIsettingsに記載したクラスを登録します。
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        private void RegisterDiContainer(IServiceCollection services) {
            foreach (IConfigurationSection section in Configuration.GetSection("disettings").GetChildren().ToList()) {
                var _scope = section.GetValue<string>("scope")?? "Request";
                var _interface = section.GetValue<string>("interface");
                var _extend = section.GetValue<string>("extend");

                switch(_scope.ToLower()) {
                    case "instance" : {
                        services.AddTransient(Type.GetType(_interface), Type.GetType(_extend));
                        break;
                    }
                    case "request" : {
                        services.AddScoped(Type.GetType(_interface), Type.GetType(_extend));
                        break;
                    }
                    case "singleton" : {
                        services.AddSingleton(Type.GetType(_interface), Type.GetType(_extend));
                        break;
                    }
                }
            }
        }
    }
}
