using System.Transactions;
using Academy.Domain.Tests.Unit.Builders;
using Microsoft.EntityFrameworkCore;

namespace Academy.Infrastructure.Tests.Integration.ClassFixtures
{
    public class RealDatabaseFixture : IDisposable
    {
        public AcademyContext Context;
        private readonly TransactionScope _scope;
        public CourseTestBuilder CourseTestBuilder = new CourseTestBuilder();

        public RealDatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<AcademyContext>()
                .UseSqlServer("Data Source=.; Initial Catalog=TddAcademy; Persist Security Info=True; User ID=sa; Password=123456; TrustServerCertificate=True;")
                .Options;
            Context = new AcademyContext(options);
            _scope = new TransactionScope();
            var asp = CourseTestBuilder.WithName("Asp .Net Core 10").WithTuition(4520).WithIsOnline(true).Build();
            var git = CourseTestBuilder.WithName("Git").WithTuition(4780).WithIsOnline(false).Build();
            var webDesign = CourseTestBuilder.WithName("Web Design").WithTuition(1250).WithIsOnline(true).Build();
            Context.Add(asp);
            Context.Add(git);
            Context.Add(webDesign);
            Context.SaveChanges();
        }

        public void Dispose()
        {
            _scope.Dispose();
            Context.Database.ExecuteSqlRaw("truncate table [TddAcademy].[dbo].[Courses]");
            Context.Database.ExecuteSqlRaw("truncate table [TddAcademy].[dbo].[Sections]");
            Context.SaveChanges();
            Context.Dispose();
        }

        
    }
}
