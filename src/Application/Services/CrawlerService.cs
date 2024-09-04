﻿using Serilog;
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
            driver.Navigate().GoToUrl("https://www.alura.com.br/");

            var inputSearchAluraElement = driver.FindElements(By.XPath("/html/body/main/section[1]/header/div/nav/div[2]/form/input"));
            var inputSearchAlura = CheckIfElementExists(inputSearchAluraElement);

            inputSearchAlura.SendKeys("Machine Learning");

            var buttonInputSearchAluraElement = driver.FindElements(By.XPath("/html/body/main/section[1]/header/div/nav/div[2]/form/button"));
            var buttonInputSearchAlura = CheckIfElementExists(buttonInputSearchAluraElement);

            buttonInputSearchAlura.Click();
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            wait.Until(driver => driver.FindElements(By.XPath("/html/body/div[2]/div[2]/section/ul/li")));
            var coursesListElement = driver.FindElements(By.XPath("/html/body/div[2]/div[2]/section/ul/li"));

            ScrapeAluraSearchPageElements(coursesListElement);

            var nextPageExists = driver.FindElements(By.XPath("/html/body/div[2]/div[2]/nav/nav/a"));
            if (nextPageExists.Count > 1)
            {
                var totalPages = Convert.ToInt32(nextPageExists.Last().Text);
                Console.WriteLine($"Existem um total de {totalPages} páginas");
                ClickNextPageButton(driver);

                for (int pageCounter = 2; pageCounter < 4; pageCounter++)
                {
                    wait.Until(driver => driver.FindElements(By.XPath("/html/body/div[2]/div[2]/section/ul/li")));
                    coursesListElement = driver.FindElements(By.XPath("/html/body/div[2]/div[2]/section/ul/li"));
                    ScrapeAluraSearchPageElements(coursesListElement);
                    ClickNextPageButton(driver);
                    Thread.Sleep(new Random().Next(1250,3500));
                    driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);

                }
            }
            FindAndSetInstructorName(driver);
        }

        public List<AluraCourse> FindAndSetInstructorName(IWebDriver driver)
        {
            foreach(var course in this.courseList)
            {
                if (course.Name != null && (course.Name.Contains("Curso") || course.Name.Contains("Formação")))
                {
                    driver.Navigate().GoToUrl(course.Link);
                    Thread.Sleep(new Random().Next(3000, 6000));
                    driver.FindElement(By.TagName("body")).SendKeys(Keys.Escape);
                    WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
                    wait.Until(driver => driver.FindElements(By.ClassName("instructor-title--name")));
                    wait.Until(driver => driver.FindElements(By.ClassName("formacao-instrutor-nome")));
                    var instructorElements = driver.FindElements(By.ClassName("instructor-title--name")).Where(x => x.Text != "").ToList();
                    var formacaoElements = driver.FindElements(By.ClassName("formacao-instrutor-nome")).Where(x => x.Text != "").ToList();
                    if(instructorElements.Count > 0)
                    {
                        foreach (var instructor in instructorElements)
                        {
                            course.Instructor += instructor.Text;
                        }
                        
                    }
                    else if(formacaoElements.Count > 0)
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
            return courseList;
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
                Console.WriteLine("Não há mais cursos");
            }
            else
            {
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
                    if (courseText.Length > 2)
                    {
                        Console.WriteLine("oi");
                    }
                }
            }
        }

        private static void ClickNextPageButton(IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            wait.Until(driver => driver.FindElements(By.XPath("/html/body/div[2]/div[2]/nav/a[2]")));
            var buttonNext = driver.FindElements(By.XPath("/html/body/div[2]/div[2]/nav/a[2]"));
            buttonNext[0].Click();
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