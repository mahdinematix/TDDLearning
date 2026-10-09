using Academy.Application;
using Academy.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RESTFulSense.Clients;

namespace Academy.Presentation.Tests.Integration
{
    public class CourseControllerTests
    {
        private const string Path = "api/Course";
        private readonly RESTFulApiFactoryClient _restClient;
        public CourseControllerTests()
        {
            var webApplicationFactory = new WebApplicationFactory<Program>();
            var httpClient = webApplicationFactory.CreateClient();
            _restClient = new RESTFulApiFactoryClient(httpClient);
        }
        [Fact]
        public async Task Should_GetAllCourses()
        {
            //act
            var actual = await _restClient.GetContentAsync<List<Course>>(Path);

            //assert
            actual.Should().HaveCountGreaterThanOrEqualTo(0);
        }

        [Fact]
        public async Task Should_CreateACourse()
        {
            //arrange
            var command = SomeCreateCourse();

            //act
            var id = await _restClient.PostContentAsync<CreateCourse, int>(Path, command);

            //assert
            var course = await _restClient.GetContentAsync<Course>($"{Path}/{id}");
            course.Id.Should().Be(id);

            //teardown
            await _restClient.DeleteContentAsync($"{Path}/{id}");

        }

        [Fact]
        public async Task Should_EditExistingCourse()
        {
            //arrange
            var commandCreateCourse = SomeCreateCourse();
            var id = await _restClient.PostContentAsync<CreateCourse, int>(Path, commandCreateCourse);

            var commandEditCourse = SomeEditCourse(id, commandCreateCourse);


            //act
            var newId = await _restClient.PutContentAsync<EditCourse,int>(Path, commandEditCourse);


            //assert
            var courses = await _restClient.GetContentAsync<List<Course>>(Path);
            courses.Should().ContainSingle(x => x.Id == newId);

            //teardown
            await _restClient.DeleteContentAsync($"{Path}/{newId}");

        }

        [Fact]
        public async Task Should_DeleteCourseById()
        {
            //arrange
            var command = SomeCreateCourse();
            var id = await _restClient.PostContentAsync<CreateCourse, int>(Path, command);

            //act
            await _restClient.DeleteContentAsync($"{Path}/{id}");


            //assert
            var course = await _restClient.GetContentAsync<Course>($"{Path}/{id}");
            course.Should().BeNull();
        }

        private static EditCourse SomeEditCourse(int id, CreateCourse commandCreateCourse)
        {
            return new EditCourse
            {
                Id = id,
                Name = "Possession",
                Instructor = commandCreateCourse.Instructor,
                IsOnline = commandCreateCourse.IsOnline,
                Tuition = commandCreateCourse.Tuition
            };
        }

        private static CreateCourse SomeCreateCourse()
        {
            return new CreateCourse
            {
                Name = Guid.NewGuid().ToString(),
                Instructor = "Hossein",
                Tuition = 7890,
                IsOnline = true
            };
        }
    }
}
