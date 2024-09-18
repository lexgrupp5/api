using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public record ModuleDto(int Id, string Name, string Description, DateTime StartDate, DateTime EndDate /*, Course Course, ICollection<Activity> Activities, ICollection<Document> Documents*/ );
}
