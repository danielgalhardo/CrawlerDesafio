using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using CrawlerAlura.src.Domain.Entities;
using OpenQA.Selenium.Support.UI;
using AngleSharp.Dom;
using System.Text.RegularExpressions;

namespace CrawlerAlura.src.Application.Services
{
    public class CrawlerService
    {
        public ILogger<CrawlerService> _logger;
        public List<AluraCourse> courseList;


        public CrawlerService(ILogger<CrawlerService> logger)
        {

            _logger = logger;
            courseList = new();
        }

        public void CrawlAluraData(IWebDriver driver)
        {
            _logger.LogInformation("[INFO] Iniciando rotina de crawling das informações da Alura");
            driver.Navigate().GoToUrl("https://www.alura.com.br/");

            var inputSearchAluraElement = driver.FindElements(By.XPath("/html/body/main/section[1]/header/div/nav/div[2]/form/input"));
            var inputSearchAlura = CheckIfElementExists(inputSearchAluraElement);

            inputSearchAlura.SendKeys(Env.GetKeyWord);

            _logger.LogInformation($"[INFO] Buscando a palavra chave {Env.GetKeyWord}");
            var buttonInputSearchAluraElement = driver.FindElements(By.XPath("/html/body/main/section[1]/header/div/nav/div[2]/form/button"));
            var buttonInputSearchAlura = CheckIfElementExists(buttonInputSearchAluraElement);

            buttonInputSearchAlura.Click();
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            Thread.Sleep(new Random().Next(1500, 7000));
            wait.Until(driver => driver.FindElements(By.XPath("/html/body/div[2]/div[2]/section/ul/li")));
            var coursesListElement = driver.FindElements(By.XPath("/html/body/div[2]/div[2]/section/ul/li"));

            ScrapeAluraSearchPageElements(coursesListElement);

            var nextPageExists = driver.FindElements(By.XPath("/html/body/div[2]/div[2]/nav/nav/a"));
            if (nextPageExists.Count > 1)
            {
                var totalPages = Convert.ToInt32(nextPageExists.Last().Text);
                Console.WriteLine($"Existem um total de {totalPages} páginas");
                ClickNextPageButton(driver);

                for (int pageCounter = 2; pageCounter < totalPages; pageCounter++)
                {
                    wait.Until(driver => driver.FindElements(By.XPath("/html/body/div[2]/div[2]/section/ul/li")));
                    coursesListElement = driver.FindElements(By.XPath("/html/body/div[2]/div[2]/section/ul/li"));
                    ScrapeAluraSearchPageElements(coursesListElement);
                    ClickNextPageButton(driver);
                    Thread.Sleep(new Random().Next(1250, 3500));
                    driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);

                }
            }
            FindAndSetInstructorAndWorkload(driver);
            driver.Quit();
        }

        public List<AluraCourse> FindAndSetInstructorAndWorkload(IWebDriver driver)
        {
            this.courseList = this.courseList.Where(crs => crs.Name.IndexOf("curso", StringComparison.OrdinalIgnoreCase) >= 0 || crs.Name.IndexOf("formação", StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            foreach (var course in this.courseList)
            {
                _logger.LogInformation($"[INFO] [Curso: {course.Name}] - Buscando instrutor e carga horaria");
                SetInstructorName(course, driver);
                SetWorkload(course, driver);
            }
            return courseList;
        }

        private void SetInstructorName(AluraCourse? course, IWebDriver driver)
        {
            if (course?.Name != null && (course.Name.Contains("Curso") || course.Name.Contains("Formação")))
            {
                driver.Navigate().GoToUrl(course.Link);
                Thread.Sleep(new Random().Next(3000, 6000));
                driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
                wait.Until(driver => driver.FindElements(By.ClassName("instructor-title--name")));
                wait.Until(driver => driver.FindElements(By.ClassName("formacao-instrutor-nome")));

                List<IWebElement> instructorElements = new();
                List<IWebElement> formacaoElements = new();
                int retry = 0;
                while (retry != 2)
                {
                    instructorElements = driver.FindElements(By.ClassName("instructor-title--name")).Where(x => x.Text != "").ToList();
                    formacaoElements = driver.FindElements(By.ClassName("formacao-instrutor-nome")).Where(x => x.Text != "").ToList();
                    if (instructorElements.Any() || formacaoElements.Any()) retry = 2;
                    else
                    {
                        retry++;
                    }
                }
                if (instructorElements.Count > 0)
                {
                    foreach (var instructor in instructorElements)
                    {
                        course.Instructor += instructor.Text;
                    }
                }
                else if (formacaoElements.Count > 0)
                {
                    foreach (var instructor in formacaoElements)
                    {
                        course.Instructor += instructor.Text + " ";
                    }
                }
                else
                {
                    Console.WriteLine("Nenhuma informação de instrutor encontrada");
                }
            }
        }


        private void SetWorkload(AluraCourse? course, IWebDriver driver)
        {
            if (course != null && course?.Name != null && (course.Name.Contains("Curso") || course.Name.Contains("Formação")))
            {
                driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
                wait.Until(driver => driver.FindElements(By.ClassName("formacao-passo-carga")));
                int retry = 0;
                List<IWebElement> workloadFormation = new();
                List<IWebElement> workloadCourse = new();
                while (retry != 2)
                {
                    workloadFormation = driver.FindElements(By.ClassName("formacao-passo-carga")).Where(x => x.Text != "").ToList();
                    workloadCourse = driver.FindElements(By.ClassName("courseInfo-card-wrapper-infos")).Where(x => x.Text != "").ToList();
                    if (workloadFormation.Any() || workloadCourse.Any()) retry = 2;
                    else
                    {
                        retry++;
                    }
                }

                if (workloadFormation.Count > 0)
                {
                    var courseWorkloadTotal = 0;
                    foreach (var workload in workloadFormation)
                    {
                        var convertWorkloadString = Convert.ToInt32(workload.Text.Replace("h", ""));
                        courseWorkloadTotal = Convert.ToInt32(course?.Workload?.Replace("h", "")) + convertWorkloadString;

                        if (course != null) course.Workload = courseWorkloadTotal + "h";
                    }

                }
                else if (workloadCourse.Count > 0)
                {
                    var courseWorkloadFromSite = GetFirstMatchingElement(workloadCourse);
                    if (courseWorkloadFromSite != null) course.Workload = courseWorkloadFromSite.Text;
                }
                else
                {
                    Console.WriteLine("Nenhuma informação da carga horaria encontrada");
                }

            }
        }

        private static IWebElement CheckIfElementExists(ReadOnlyCollection<IWebElement> element)
        {
            if (element.Count == 0) throw new Exception("Input não foi encontrado");
            return element.First();
        }

        private void ScrapeAluraSearchPageElements(ReadOnlyCollection<IWebElement> coursesListElement)
        {


            if (coursesListElement.Count == 0)
            {
                _logger.LogInformation($"[INFO] [0] - Nenhum curso encontrado");
            }
            else
            {
                _logger.LogInformation($"[INFO] [{coursesListElement.Count()}] - Cursos encontrado(s)");
                foreach (var course in coursesListElement)
                {
                    var courseText = course.Text.Split("\n");
                    if (courseText.Length >= 2)
                    {

                        this.courseList.Add(new AluraCourse
                        {
                            Name = courseText[0],
                            Description = courseText[1],
                            Link = (course.FindElement(By.TagName("a"))).GetAttribute("href")
                        });
                    }
                }
            }
        }

        private static void ClickNextPageButton(IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            wait.Until(driver => driver.FindElements(By.XPath("/html/body/div[2]/div[2]/nav/a[2]")));
            Thread.Sleep(new Random().Next(1500, 6000));
            var buttonNext = driver.FindElements(By.XPath("/html/body/div[2]/div[2]/nav/a[2]"));
            buttonNext[0].Click();
        }

        public static IWebElement? GetFirstMatchingElement(List<IWebElement> elements)
        {
            string pattern = @"^\d+h$";
            Regex regex = new Regex(pattern);

            foreach (IWebElement element in elements)
            {
                string text = element.Text;

                if (regex.IsMatch(text))
                {
                    return element;
                }
            }
            return null;
        }

        public List<AluraCourse> GetAluraCourseList
        {
            get
            {
                return this.courseList;
            }
        }
    }
}