namespace Academy.Acceptance.Tests.Core
{
    public interface IStartableHost : IHost
    {
        void Start();
        void Stop();
    }
}