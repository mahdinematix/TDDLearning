namespace Academy.Domain.Tests.Unit.Builders
{
    public class CourseTestBuilder
    {
        private string Name = "Scrum";
        private bool IsOnline = true;
        private double Tuition = 1600;
        private string Instructor = "Mahdi Nemati";

        public CourseTestBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public CourseTestBuilder WithIsOnline(bool isOnline)
        {
            IsOnline = isOnline;
            return this;
        }

        public CourseTestBuilder WithTuition(double tuition)
        {
            Tuition = tuition;
            return this;
        }
        public Course Build()
        {
            return new Course(Name, IsOnline, Tuition, Instructor);
        }

        
    }
}
