using Microsoft.AspNetCore.Mvc;
using Practical_10.Models;

namespace Practical_10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private static readonly List<Student> students =
            new List<Student>()
            {
                new Student
                {
                    Id = 1,
                    Name = "Aarav Sharma",
                    Email = "aarav.sharma@gmail.com",
                    Course = "Computer Engineering",
                    Age = 21
                },

                new Student
                {
                    Id = 2,
                    Name = "Diya Mehta",
                    Email = "diya.mehta@gmail.com",
                    Course = "Information Technology",
                    Age = 20
                },

                new Student
                {
                    Id = 3,
                    Name = "Rohan Verma",
                    Email = "rohan.verma@gmail.com",
                    Course = "Mechanical Engineering",
                    Age = 22
                },

                new Student
                {
                    Id = 4,
                    Name = "Sneha Kulkarni",
                    Email = "sneha.k@gmail.com",
                    Course = "Electrical Engineering",
                    Age = 21
                }
            };

        // GET: api/students
        [HttpGet]
        public ActionResult<List<Student>> GetStudents()
        {
            return Ok(students);
        }

        // GET: api/students/1
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound("Student not found");
            }

            return Ok(student);
        }

        // POST: api/students
        [HttpPost]
        public ActionResult<Student> AddStudent(Student student)
        {
            student.Id = students.Count > 0
                ? students.Max(s => s.Id) + 1
                : 1;

            students.Add(student);

            return CreatedAtAction(
                nameof(GetStudent),
                new { id = student.Id },
                student
            );
        }

        // PUT: api/students/1
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(
            int id,
            Student updatedStudent)
        {
            var student = students.FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound("Student not found");
            }

            student.Name = updatedStudent.Name;
            student.Email = updatedStudent.Email;
            student.Course = updatedStudent.Course;
            student.Age = updatedStudent.Age;

            return Ok(student);
        }

        // DELETE: api/students/1
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound("Student not found");
            }

            students.Remove(student);

            return Ok("Student deleted successfully");
        }
    }
}