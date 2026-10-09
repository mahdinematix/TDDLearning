using System.Linq.Expressions;
using Academy.Domain;

namespace Academy.Infrastructure;

public class CourseRepository : ICourseRepository
{
    private readonly AcademyContext _context;

    public CourseRepository(AcademyContext context)
    {
        _context = context;
    }

    public int Create(Course course)
    {
        _context.Courses.Add(course);
        _context.SaveChanges();
        return course.Id;
    }

    public List<Course> GetAll()
    {
        return _context.Courses.ToList();
    }

    public Course GetBy(int id)
    {
        return _context.Courses.Find(id);
    }

    public Course GetBy(string name)
    {
        return _context.Courses.FirstOrDefault(x=>x.Name == name);
    }

    public void Delete(int id)
    {
        var course = GetBy(id);
        _context.Courses.Remove(course);
        _context.SaveChanges();
    }

    public bool Exists(Expression<Func<Course, bool>> expression)
    {
        return _context.Set<Course>().Any(expression);
    }
}