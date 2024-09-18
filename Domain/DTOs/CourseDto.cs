using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities;

namespace Domain.DTOs
{
    public record CourseDto(int Id, string Name, string Description, DateTime StartDate, DateTime EndDate/*, ICollection<string> modules*/);
}
