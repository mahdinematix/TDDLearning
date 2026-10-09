using Academy.Domain.Exceptions;
using Academy.Domain.Tests.Unit.Builders;
using Academy.Domain.Tests.Unit.ClassFixtures;
using Academy.Domain.Tests.Unit.CollectionFixtures;
using Academy.Domain.Tests.Unit.Factories;
using FluentAssertions;

namespace Academy.Domain.Tests.Unit.Tests;

[Collection("Database Collection")]
public class CourseTests : IClassFixture<IdentifierFixture>
{
    private readonly CourseTestBuilder _courseBuilder;
    public CourseTests(DatabaseFixture databaseFixture)
    {
        _courseBuilder = new CourseTestBuilder();
    }

    [Fact]
    public void Ctor_ShouldConstructCourseProperly()
    {
        //arrange
        const string name = "Scrum";
        const bool isOnline = true;
        const double tuition = 1600;
        const string instructor = "Mahdi";
        var guid = IdentifierFixture.Id;

        //act
        var course = new Course(name,isOnline,tuition,instructor);

        //assert
        course.Name.Should().Be(name);
        course.IsOnline.Should().Be(isOnline);
        course.Tuition.Should().Be(tuition);
        course.Sections.Should().BeEmpty();
    }

    [Fact]
    public void Ctor_ShouldThrowsExceptionProperly_When_NamePropIsNotProvided()
    {
        //arrange
        var guid = IdentifierFixture.Id;

        //act
        Action course = () => _courseBuilder.WithName("").Build();

        //assert
        course.Should().ThrowExactly<CourseNameIsInvalidException>();
    }

    [Fact]
    public void Ctor_ShouldThrowsExceptionProperly_When_TuitionPropIsNotProvided()
    {
        //act
        Action course = () => _courseBuilder.WithTuition(0).Build();

        //assert
        course.Should().ThrowExactly<CourseTuitionIsInvalidException>();
    }

    [Fact]
    public void AddSection_ShouldAddASectionToSectionsProperly_When_IdAndNamePassed()
    {
        //arrange
        var course = _courseBuilder.Build();
        var section = SectionFactory.Create();

        //act
        course.AddSection(section);


        //assert
        course.Sections.Should().ContainEquivalentOf(section);
    }

    [Theory]
    [InlineData(1,1,true)]
    [InlineData(1,2,false)]
    public void Equals_ShouldWorkProperly_When_IsEqualsById(int id1, int id2, bool expected)
    {
        //arrange
        var course1 = _courseBuilder.Build();
        course1.Id = id1;
        var course2 = _courseBuilder.Build();
        course2.Id = id2;

        //act
        var actual = course1.Equals(course2);

        //assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_When_CourseIsNull()
    {
        //arrange
        var course = _courseBuilder.Build();

        //act
        var actual = course.Equals(null);

        //assert
        actual.Should().BeFalse();
    }
}