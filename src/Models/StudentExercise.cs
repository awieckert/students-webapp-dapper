using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workforce.Models
{
    public class StudentExercise
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public int StudentId { get; set; }
        public int InstructorId { get; set; }
    }
}
