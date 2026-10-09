using System.Linq.Expressions;
using Academy.Domain;
using Academy.Domain.Exceptions;
using FluentAssertions;
using NSubstitute;
using Academy.Domain.Tests.Unit.Builders;
using NSubstitute.ReturnsExtensions;

namespace Academy.Application.Tests.Unit
{
    public class CourseApplicationTests
    {
        private readonly ICourseRepository _courseRepository;
        private readonly CourseApplication _courseApplication;
        private readonly CourseTestBuilder _courseBuilder;

        public CourseApplicationTests()
        {
            _courseRepository = Substitute.For<ICourseRepository>();
            _courseApplication = new CourseApplication(_courseRepository);
            _courseBuilder = new CourseTestBuilder();
        }
        [Fact]
        public void Create_ShouldCreateACourse()
        {
            //arrange
            var command = SomeCreateCourse();
            
            //act
            _courseApplication.Create(command);


            //assert
            _courseRepository.Received().Create(Arg.Any<Course>());
        }

        [Fact]
        public void Create_ShouldReturnCreatedCourseId()
        {
            //arrange
            var command = SomeCreateCourse();
            _courseRepository.Create(default).ReturnsForAnyArgs(10);
            
            //act
            var actual = _courseApplication.Create(command);


            //assert
            actual.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Create_ShouldThrowException_When_CourseNamePassedIsDuplicated()
        {
            //arrange
            const string name = "Onion Arch";
            var command = SomeCreateCourse(name);
            _courseRepository.Exists(Arg.Any<Expression<Func<Course, bool>>>()).Returns(true);
            
            //act
            Action actual = () => _courseApplication.Create(command);


            //assert
            actual.Should().ThrowExactly<DuplicatedCourseNameException>();

        }

        [Fact]
        public void Edit_ShouldUpdateTheCourse()
        {
            //arrange
            var command = SomeEditCourse();
            var course = _courseBuilder.Build();
            _courseRepository.GetBy(command.Id).Returns(course);

            //act
            _courseApplication.Edit(command);


            //assert
            Received.InOrder((() =>
            {
                _courseRepository.Delete(command.Id);
                _courseRepository.Create(Arg.Any<Course>());
            }));

        }

        [Fact]
        public void Edit_ShouldReturnUpdatedCourseId()
        {
            //arrange
            var command = SomeEditCourse();
            _courseRepository.Create(default).ReturnsForAnyArgs(10);
            var course = _courseBuilder.Build();
            _courseRepository.GetBy(Arg.Any<int>()).Returns(course);

            //act
            var actual = _courseApplication.Edit(command);

            //assert
            actual.Should().BeGreaterThan(0);

        }

        [Fact]
        public void Edit_ShouldThrowException_When_TheUpdatingCourseNotExists()
        {
            //arrange
            var command = SomeEditCourse();
            _courseRepository.GetBy(command.Id).ReturnsNull();

            //act
            Action actual = () => _courseApplication.Edit(command);

            //assert
            actual.Should().ThrowExactly<CourseNotExistsException>();
        }

        [Fact]
        public void Delete_ShouldDeleteCourse()
        {
            //arrange
            const int id = 13;

            //act
            _courseApplication.Delete(id);

            //assert
            _courseRepository.Received().Delete(id);
        }

        [Fact]
        public void GetAll_ShouldReturnListOfCourses()
        {
            //arrange
            _courseRepository.GetAll().Returns(new List<Course>());

            //act
            var courses = _courseApplication.GetAll();

            //assert
            courses.Should().BeOfType<List<Course>>();
            _courseRepository.Received().GetAll();
        }

        private static EditCourse SomeEditCourse()
        {
            return new EditCourse
            {
                Id = 16,
                Name = "Tdd",
                Instructor = "Mahdi",
                IsOnline = false,
                Tuition = 450
            };
        }

        private static CreateCourse SomeCreateCourse(string name="Clean Architecture")
        {
            var command = new CreateCourse
            {
                Name = name,
                Instructor = "Mahdi",
                IsOnline = false,
                Tuition = 450
            };
            return command;
        }
    }
}
