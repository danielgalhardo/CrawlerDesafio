using CrawlerAlura.src.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerAlura.src.Domain.Interfaces
{
    public interface IAluraCourseRepository
    {
        Task AddAsync(AluraCourse course);
        Task AddRangeAsync(List<AluraCourse> courses);

    }
}
