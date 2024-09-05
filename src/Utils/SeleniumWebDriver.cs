using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager;
using WebDriverManager.Helpers;

namespace CrawlerAlura.src.Utils
{
    public class SeleniumWebDriver
    {
        EdgeOptions? options = new();
        IWebDriver? driver;

        public SeleniumWebDriver()
        {
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-crash-reporter");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-in-process-stack-traces");
            options.AddArgument("--disable-logging");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--log-level=3");
            options.AddArgument("--output=/dev/null");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--dns-prefetch-disable");
            options.AddArgument("user-agent= Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36");

            // Set page load strategy to eager
            options.PageLoadStrategy = PageLoadStrategy.Eager; // Or PageLoadStrategy.None
        }

        public IWebDriver GetDriver()
        {
            new DriverManager().SetUpDriver(new EdgeConfig(), VersionResolveStrategy.MatchingBrowser);
            driver = new EdgeDriver(options);
            driver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(400));
            driver.Manage().Window.Maximize();
            return driver;
        }
    }
}
