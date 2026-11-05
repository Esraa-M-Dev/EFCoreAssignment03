using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreAssignment03.Models
{
    public class Instructor
    {
        public int InstructorId { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Bio { get; set; }

        public DateTime JoinDate { get; set; }


        public ICollection<Course> Courses { get; set; }= new List<Course>();

    }
}
