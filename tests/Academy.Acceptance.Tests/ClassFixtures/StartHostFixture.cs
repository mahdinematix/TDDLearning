using Academy.Acceptance.Tests.Core;
using Academy.Acceptance.Tests.NetCoreHosting;

namespace Academy.Acceptance.Tests.ClassFixtures
{
    public class StartHostFixture : IDisposable
    {
        private readonly IStartableHost _host = new DotNetCoreHost(new DotNetCoreHostOptions
        {
            Port = HostConstants.Port,
            CsProjectPath = HostConstants.CsProjectPath
        });
        public StartHostFixture()
        {
            _host.Start();
        }
        public void Dispose()
        {
            _host.Stop();
        }
    }
}
