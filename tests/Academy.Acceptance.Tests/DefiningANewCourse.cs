using Academy.Acceptance.Tests.ClassFixtures;
using Academy.Application;
using FluentAssertions;
using Newtonsoft.Json;
using RestSharp;
using TestStack.BDDfy;

namespace Academy.Acceptance.Tests
{
    public class DefiningANewCourse : IClassFixture<StartHostFixture>
    {

        [Fact]
        public void CreatingANewCourse()
        {
            var course = SomeCreateCourse();
            this.Given(_ => _.IWantToCreateTheFollowingCourse(course))
                .When(_ => _.IPressAddButton())
                .Then(_ => _.TheFollowingCourseShouldBeAvailableOnList(course.Name))
                .BDDfy();
        }

        [Fact]
        public void DuplicatedCourseCantBeCreated()
        {
            var course = SomeCreateCourse();
            this.Given(_ => _.IHaveAlreadyCreatedTheCourse(course))
                .When(_ => _.ITryToCreateItAgain())
                .Then(_ => _.TheCourseShouldNotBeAppearedInListTwice())
                .BDDfy();
        }

        private CreateCourse _course;

        public void IWantToCreateTheFollowingCourse(CreateCourse course)
        {
            _course = course;
        }

        
        public void IPressAddButton()
        {
            var id = PostTheCourse();
            _course.Id = id;
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

        public void TheFollowingCourseShouldBeAvailableOnList(string courseName)
        {
            _course.Id.Should().BeGreaterThan(0);
        }

        public void IHaveAlreadyCreatedTheCourse(CreateCourse course)
        {
            _course = course;
            PostTheCourse();
        }

        public void ITryToCreateItAgain()
        {
            var id = PostTheCourse();
            _course.Id = id;
        }

        public void TheCourseShouldNotBeAppearedInListTwice()
        {
            _course.Id.Should().Be(0);
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
    }
}
