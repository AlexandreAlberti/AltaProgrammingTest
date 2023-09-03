using ApiProgrammingTest.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApiProgrammingTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddScoped<IAccountsService, AccountsService>();
            services.AddScoped<IPropertiesService, PropertiesService>();
            services.AddScoped<IPurchasesService, PurchasesService>();

            //setup an SQLite DB
            //services.AddDbContext<PropertyMogulContext>(opt =>
            //    opt.UseLazyLoadingProxies().UseSqlite("Filename=Test.db"));

            //use this for an in memory DB (nothing will save between runs), you will need to create your own seed logic
            //to fill it with the appropriate data, see the end of this file
            services.AddDbContext<PropertyMogulContext>(opt => opt.UseInMemoryDatabase("Test"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            SeedDatabase(app);
        }

        //slightly hacky, normally would use db migrations to achieve the same thing
        static void SeedDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = serviceScope.ServiceProvider.GetService<PropertyMogulContext>())
            {
                bool databaseExists = false;

                try
                {
                    databaseExists = context.Database.CanConnect();
                }
                catch { }

                if (databaseExists)
                {
                    context.Properties.Add(new PropertyInfo
                    {
                        Id = 1,
                        Name = "Bank",
                        BuyPrice = 10000,
                        SellPrice = 7000,
                        RevenuePerHour = 500,
                        AvailableForPurchase = true,
                        OwnedBy = -1
                    });

                    context.Properties.Add(new PropertyInfo
                    {
                        Id = 2,
                        Name = "Cornerstore",
                        BuyPrice = 1000,
                        SellPrice = 500,
                        RevenuePerHour = 50,
                        AvailableForPurchase = true,
                        OwnedBy = -1
                    });

                    context.Properties.Add(new PropertyInfo
                    {
                        Id = 3,
                        Name = "Petrol Station",
                        BuyPrice = 5000,
                        SellPrice = 4000,
                        RevenuePerHour = 150,
                        AvailableForPurchase = true,
                        OwnedBy = -1
                    });

                    context.Properties.Add(new PropertyInfo
                    {
                        Id = 4,
                        Name = "Supermarket",
                        BuyPrice = 3000,
                        SellPrice = 1500,
                        RevenuePerHour = 100,
                        AvailableForPurchase = true,
                        OwnedBy = -1
                    });

                    context.Properties.Add(new PropertyInfo
                    {
                        Id = 5,
                        Name = "Lemonade Stand",
                        BuyPrice = 200,
                        SellPrice = 10,
                        RevenuePerHour = 20,
                        AvailableForPurchase = false,
                        OwnedBy = 2
                    });
                    context.SaveChanges();

                    context.Accounts.Add(new AccountInfoDB
                    {
                        Id = 1,
                        Name = "Alex",
                        Balance = 5000,
                        LastUpdateTime = System.DateTime.Now,
                        SignUpTime = System.DateTime.Now,
                        Purchases = ""
                    });
                    context.Accounts.Add(new AccountInfoDB
                    {
                        Id = 2,
                        Name = "Alex Rich",
                        Balance = 5000000,
                        LastUpdateTime = System.DateTime.Now,
                        SignUpTime = System.DateTime.Now,
                        Purchases = "5"
                    });
                    context.AccountNames.Add(new AccountName
                    {
                        Name = "Alex"
                    });
                    context.AccountNames.Add(new AccountName
                    {
                        Name = "Alex Rich"
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
