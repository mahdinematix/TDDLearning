using Academy.Domain;
using Academy.Domain.Tests.Unit.Builders;
using Academy.Infrastructure.Tests.Integration.ClassFixtures;
using FluentAssertions;

namespace Academy.Infrastructure.Tests.Integration
{
    public class CourseRepositoryTests : IClassFixture<RealDatabaseFixture>
    {
        private readonly ICourseRepository _repository;
        private readonly CourseTestBuilder _courseBuilder; 
        public CourseRepositoryTests(RealDatabaseFixture databaseFixture)
        {
            _repository = new CourseRepository(databaseFixture.Context);
            _courseBuilder = new CourseTestBuilder();
        }
        [Fact]
        public void GetAll_ShouldReturnListOfCourses()
        {
            //act
            var courses = _repository.GetAll();

            //assert
            courses.Should().HaveCountGreaterThanOrEqualTo(3);
        }

        [Fact]
        public void Should_CreateACourse()
        {
            //arrange
            var course = _courseBuilder.Build();

            //act
            _repository.Create(course);

            //assert
            var courses = _repository.GetAll();
            courses.Should().Contain(course);
        }

        [Fact]
        public void Should_ReturnCreatedCourseId()
        {
            //arrange
            var course = _courseBuilder.Build();

            //act
            var id = _repository.Create(course);

            //assert
            id.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Should_GetCourseByName()
        {
            //arrange
            const string expectedName = "Tdd & Bdd";
            var course = _courseBuilder.WithName(expectedName).Build();
            _repository.Create(course);

            //act
            var actual = _repository.GetBy(expectedName);

            //assert
            actual.Should().Be(course);
        }

        [Fact]
        public void Should_DeleteCourseById()
        {
            //arrange
            var course = _courseBuilder.Build();
            var id = _repository.Create(course);

            //act
            _repository.Delete(id);

            //assert
            var expectedCourse = _repository.GetBy(id);
            expectedCourse.Should().BeNull();
        }

        [Fact]
        public void Should_GetCourseById()
        {
            //arrange
            var course = _courseBuilder.Build();
            var id= _repository.Create(course);

            //act
            var actual = _repository.GetBy(id);

            //assert
            actual.Should().Be(course);
        }
    }
}
