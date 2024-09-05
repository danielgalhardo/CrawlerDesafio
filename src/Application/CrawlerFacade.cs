using CrawlerAlura.src.Application.Services;
using CrawlerAlura.src.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerAlura.src.Application
{
    public class CrawlerFacade
    {
        CrawlerService _crawlerService;
        AluraCourseService _courseService;
        SeleniumWebDriver _seleniumConfig = new();

        public CrawlerFacade(CrawlerService crawlerService, AluraCourseService courseService)
        {
            _crawlerService = crawlerService;
            _courseService = courseService;
        }

        public void StartCrawlingRoutine()
        {
            _crawlerService.CrawlAluraData(_seleniumConfig.GetDriver());
            var coursesScrapedList = _crawlerService.GetAluraCourseList;
            _courseService.AddCoursesAsync(coursesScrapedList).Wait();
        }
    }
}
