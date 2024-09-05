using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerAlura.src.Domain.Entities
{
    public class AluraCourse
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Link { get; set; }

        public string? Instructor { get; set; }

        public string? Workload { get; set; }
    }
}
