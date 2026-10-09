using System.Linq.Expressions;

namespace Academy.Domain
{
    public interface ICourseRepository
    {
        int Create(Course course);
        List<Course> GetAll();
        Course GetBy(int id);
        Course GetBy(string name);
        void Delete(int id);
        bool Exists(Expression<Func<Course, bool>> expression);
    }
}
