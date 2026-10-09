using Academy.Domain.Tests.Unit.CollectionFixtures;
using FluentAssertions;

namespace Academy.Domain.Tests.Unit.Tests
{
    [Collection("Database Fixture")]
    public class SectionTests
    {
        public SectionTests()
        {
            
        }
        [Fact]
        public void Ctor_Should_Construct_Section_Properly()
        {
            //arrange
            const int id = 1;
            const string name = "Meeting";


            //act
            var section = new Section(id, name);

            //assert
            section.Id.Should().Be(id);
            section.Name.Should().Be(name);
        }
    }
}
