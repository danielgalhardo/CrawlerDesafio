using CrawlerAlura.src.Domain.Entities;
using CrawlerAlura.src.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerAlura.src.Application.Services
{
    public class AluraCourseService
    {
        private readonly IAluraCourseRepository _repository;

        public AluraCourseService(IAluraCourseRepository repository)
        {
            _repository = repository;
        }

        public async Task AddCourseAsync(string name, string description)
        {
            var course = new AluraCourse
            {
                Name = name,
                Description = description
            };

            await _repository.AddAsync(course);
        }

        public async Task AddCoursesAsync(List<AluraCourse> aluraCoursesList)
        {
            try
            {
                await _repository.AddRangeAsync(aluraCoursesList);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}