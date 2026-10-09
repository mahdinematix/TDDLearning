using Academy.Domain;
using Academy.Domain.Exceptions;

namespace Academy.Application;

public class CourseApplication : ICourseApplication
{
    private readonly ICourseRepository _courseRepository;

    public CourseApplication(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public int Create(CreateCourse command)
    {
        if (_courseRepository.Exists(x=>x.Name == command.Name))
        {
            throw new DuplicatedCourseNameException();
        }
        var course = new Course(command.Name, command.IsOnline, command.Tuition, command.Instructor);
        return _courseRepository.Create(course);
    }

    public int Edit(EditCourse command)
    {
        if (_courseRepository.GetBy(command.Id)==null)
        {
            throw new CourseNotExistsException();
        }
        _courseRepository.Delete(command.Id);
        var course = new Course(command.Name, command.IsOnline, command.Tuition, command.Instructor);
        return _courseRepository.Create(course);
    }

    public void Delete(int id)
    {
        _courseRepository.Delete(id);
    }

    public List<Course> GetAll()
    {
        return _courseRepository.GetAll();
    }

    public Course GetBy(int id)
    {
        return _courseRepository.GetBy(id);
    }
}