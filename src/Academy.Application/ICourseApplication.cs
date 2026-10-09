using Academy.Domain;

namespace Academy.Application
{
    public interface ICourseApplication
    {
        int Create(CreateCourse command);
        int Edit(EditCourse command);
        void Delete(int id);
        List<Course> GetAll();
        Course GetBy(int id);
    }
}
