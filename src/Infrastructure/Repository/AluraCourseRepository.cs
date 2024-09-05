using CrawlerAlura.src.Domain.Entities;
using CrawlerAlura.src.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerAlura.src.Infrastructure.Repository
{
    public class AluraCourseRepository : IAluraCourseRepository
    {
        private readonly ApplicationDbContext _context;

        public AluraCourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AluraCourse course)
        {
            _context.AluraCourses.Add(course);   
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(List<AluraCourse> courses)
        {           
            await _context.AluraCourses.AddRangeAsync(courses);
            await _context.SaveChangesAsync();
        }

    }
}