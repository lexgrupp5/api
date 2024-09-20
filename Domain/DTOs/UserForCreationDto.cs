using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UserForCreationDto
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
