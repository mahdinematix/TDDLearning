using Academy.Application;
using Academy.Domain;
using Academy.Presentation.Controllers;
using FluentAssertions;
using NSubstitute;

namespace Academy.Presentation.Tests.Unit
{
    public class CourseControllerTests
    {
        private readonly ICourseApplication _courseApplication;
        private readonly CourseController _courseController;

        public CourseControllerTests()
        {
            _courseApplication = Substitute.For<ICourseApplication>();
            _courseController = new CourseController(_courseApplication);
        }

        [Fact]
        public void Should_ReturnAllCourses()
        {
            //act
            _courseController.GetCourses();

            //assert
            _courseApplication.Received().GetAll();
        }


        [Fact]
        public void Should_ReturnListOfCourses()
        {
            //arrange
            _courseApplication.GetAll().Returns(new List<Course>());

            //act
            var courses = _courseController.GetCourses();

            //assert
            courses.Should().BeOfType<List<Course>>();
        }

        [Fact]
        public void Should_CreateACourse()
        {
            //arrange
            var command = SomeCreateCourse();

            //act
            _courseController.Create(command);

            //assert
            _courseApplication.Received().Create(command);
        }

        [Fact]
        public void Should_EditExistingCourse()
        {
            //arrange
            var command = SomeEditCourse();

            //act
            _courseController.Edit(command);

            //assert
            _courseApplication.Received().Edit(command);
        }

        private static EditCourse SomeEditCourse()
        {
            return new EditCourse()
            {
                Id = 5,
                Name = "Tdd bdd",
                Instructor = "Hossein",
                IsOnline = true,
                Tuition = 5890
            };
        }

        [Fact]
        public void Should_DeleteACourseById()
        {
            //arrange
            const int id = 6;

            //act
            _courseController.Delete(id);

            //assert
            _courseApplication.Received().Delete(id);
        }

        private static CreateCourse SomeCreateCourse()
        {
            return new CreateCourse
            {
                Name = "Tdd bdd",
                Instructor = "Hossein",
                IsOnline = true,
                Tuition = 5890
            };
        }
    }
}
