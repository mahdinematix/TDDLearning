using Academy.Domain;
using Academy.Domain.Tests.Unit.Builders;
using Academy.Infrastructure.Tests.Integration.ClassFixtures;
using FluentAssertions;

namespace Academy.Infrastructure.Tests.Unit
{
    public class CourseRepositoryTests : IClassFixture<RealDatabaseFixture>
    {
        private readonly CourseTestBuilder _courseBuilder;
        private readonly ICourseRepository _courseRepository;
        public CourseRepositoryTests(RealDatabaseFixture databaseFixture)
        {
            _courseBuilder = new CourseTestBuilder();
            _courseRepository = new CourseRepository(databaseFixture.Context);
        }
        [Fact]
        public void Create_ShouldCreateCourseProperly()
        {
            //arrange
            var courseBuilder = new CourseTestBuilder();
            var course = courseBuilder.Build();
            

            //act
            _courseRepository.Create(course);

            //assert
            var courses = _courseRepository.GetAll();
            courses.Should().Contain(course);

        }

        [Fact]
        public void GetAll_ShouldReturnListOfCourses()
        {
            //act
            var courses = _courseRepository.GetAll();

            //assert
            courses.Should().HaveCountGreaterThanOrEqualTo(0);
        }

        [Fact]
        public void GetBy_ShouldReturnCourseById()
        {
            //arrange
            var course = _courseBuilder.Build();
            var id = _courseRepository.Create(course);

            //act
            var actual = _courseRepository.GetBy(id);

            //assert
            actual.Name.Should().Be(course.Name);
            actual.Tuition.Should().Be(course.Tuition);
            actual.Instructor.Should().Be(course.Instructor);
        }

        [Fact]
        public void GetBy_ShouldReturnNull_When_CourseWithPassedIdDoesNotExist()
        {
            //arrange
            const int id = 454584;

            //act
            var actual = _courseRepository.GetBy(id);

            //assert
            actual.Should().BeNull();
        }

        [Fact]
        public void Delete_ShouldDeleteACourseById()
        {
            //arrange
            var course = _courseBuilder.Build();
            var id = _courseRepository.Create(course);

            //act
            _courseRepository.Delete(id);

            //assert
            var courses = _courseRepository.GetAll();
            courses.Should().NotContain(course);
        }

        [Fact]
        public void Exists_ShouldReturnNull_When_CourseWithNameNotExists()
        {
            //arrange
            const string name = "Bdd";
            var course = _courseBuilder.WithName("Bdd").Build();
            _courseRepository.Create(course);
            //act
            var actual = _courseRepository.Exists(x => x.Name == name);

            //assert
            actual.Should().BeTrue();
        }
    }
}
