using Scarf.DataSource;

namespace Scarf.Tests.Infrastructure
{
    internal class TestDataSourceFactory : DataSourceFactory
    {
        public ScarfDataSource CreateDataSourceInstance()
        {
            return new TestDataSource();
        }
    }
}
