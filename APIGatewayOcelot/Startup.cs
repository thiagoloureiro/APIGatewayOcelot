using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Cache;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using System.Text;

namespace APIGatewayOcelot
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
            JwtConfiguration(services);

            services
                .AddOcelot(new ConfigurationBuilder()
                    .AddJsonFile("ocelot.json")
                    .Build())
                .AddConsul()
                .AddPolly()
                .AddCacheManager(x => x.WithDictionaryHandle());
            //   .AddAdministration("/administration", "secret");
            //                .AddAdministration("/administration", options);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            //.AddIdentityServerAuthentication("TestKey", options);

            services.AddSingleton<IOcelotCache<CachedResponse>, MyRedisCache>();

            //RedisHelper.Initialization(new CSRedis.CSRedisClient("localhost"));

            services.AddApplicationInsightsTelemetry();

            services.AddSingleton<IDistributedCache>(new Microsoft.Extensions.Caching.Redis.CSRedisCache(RedisHelper.Instance));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            await app.UseOcelot();
        }

        private void JwtConfiguration(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("Audience:Secret").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }
    }
}