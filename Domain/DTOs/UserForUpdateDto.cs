using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;

namespace Domain.DTOs
{
    public class UserForUpdateDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public Course Course { get; set; }
    }
}
