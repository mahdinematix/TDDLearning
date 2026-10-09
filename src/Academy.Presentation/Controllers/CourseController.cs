using Academy.Application;
using Academy.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseApplication _courseApplication;

        public CourseController(ICourseApplication courseApplication)
        {
            _courseApplication = courseApplication;
        }
        [HttpGet()]
        public List<Course> GetCourses()
        {
            return _courseApplication.GetAll();
        }

        [HttpPost]
        public int Create(CreateCourse command)
        {
            try
            {
                return _courseApplication.Create(command);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        [HttpPut]
        public int Edit(EditCourse command)
        {
            return _courseApplication.Edit(command);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _courseApplication.Delete(id);
        }

        [HttpGet("{id}")]
        public Course GetBy(int id)
        {
            return _courseApplication.GetBy(id);
        }
    }
}
