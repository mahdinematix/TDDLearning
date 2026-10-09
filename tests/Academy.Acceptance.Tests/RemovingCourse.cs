using Academy.Acceptance.Tests.ClassFixtures;
using Academy.Application;
using Academy.Domain;
using FluentAssertions;
using Newtonsoft.Json;
using RestSharp;
using System.Net.WebSockets;
using TestStack.BDDfy;

namespace Academy.Acceptance.Tests
{
    public class RemovingCourse : IClassFixture<StartHostFixture>
    {

        [Fact]
        public void DeletingExistingCourse()
        {
            var course = SomeCreateCourse();
            this.Given(_ => _.IHaveCreatedACourse(course))
                .When(_ => _.IPressRemoveButton())
                .Then(_ => _.TheCourseShouldNotBeAvailableOnListAnyMore())
                .BDDfy();
        }

        private CreateCourse _course;

        public void IHaveCreatedACourse(CreateCourse course)
        {
            _course = course;
            var id = PostTheCourse();
            _course.Id = id;
        }

        public void IPressRemoveButton()
        {
            DeleteCourseBy(_course.Id);
        }

        public void TheCourseShouldNotBeAvailableOnListAnyMore()
        {
            var course = GetCourseBy(_course.Id);
            course.Should().BeNull();
        }

        private static CreateCourse SomeCreateCourse()
        {
            return new CreateCourse
            {
                Name = Guid.NewGuid().ToString(),
                Instructor = "Mahdi",
                IsOnline = true,
                Tuition = 450
            };
        }

        private int PostTheCourse()
        {
            var restClient = new RestClient(HostConstants.Endpoint);
            var restRequest = new RestRequest("Course");
            restRequest.AddJsonBody(_course);
            var restResponse = restClient.Post(restRequest);
            var id = JsonConvert.DeserializeObject<int>(restResponse.Content);
            return id;
        }

        private void DeleteCourseBy(int id)
        {
            var restClient = new RestClient(HostConstants.Endpoint);
            var restRequest = new RestRequest($"Course/{id}");
            var response = restClient.Delete(restRequest);
        }
        private Course GetCourseBy(int id)
        {
            var restClient = new RestClient(HostConstants.Endpoint);
            var restRequest = new RestRequest($"Course/{id}");
            var response = restClient.Get(restRequest);
            var course = JsonConvert.DeserializeObject<Course>(response.Content);
            return course;
        }
    }
}
