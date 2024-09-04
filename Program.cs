using CrawlerAlura.src.Application;
using CrawlerAlura.src.Application.Services;
using CrawlerAlura.src.Domain.Interfaces;
using CrawlerAlura.src.Infrastructure;
using CrawlerAlura.src.Infrastructure.Repository;
using CrawlerAlura.src.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Log = Serilog.Log;

namespace CrawlerAlura
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
             .SetBasePath(Path.GetDirectoryName(typeof(Program).Assembly.Location))
             .AddJsonFile("appsettings.json", true, true)
             .AddEnvironmentVariables()
             .Build();

            Env.SetConfiguration(configuration);
            var services = new ServiceCollection();

            services.AddLogging(configure =>
            {
                ConfigureLogger();
                configure.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                configure.AddSerilog();
            });

            services.AddSingleton<CrawlerService>();
            services.AddSingleton<CrawlerFacade>();
            services.AddSingleton<AluraCourseService>();
            services.AddScoped<IAluraCourseRepository, AluraCourseRepository>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=C:\\Users\\galha\\source\\repos\\CrawlerAlura\\app.db"));
            SeleniumWebDriver crawler = new SeleniumWebDriver();
            var serviceProvider = services.BuildServiceProvider();
            var worker = serviceProvider.GetService<CrawlerFacade>();
            worker.StartCrawlingRoutine();


        }

        public static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(@"C:\Users\galha\Documents\TCC\Projetos\TCC_Crawler\log\", rollingInterval: RollingInterval.Day)
               .CreateLogger();
        }
    }
}