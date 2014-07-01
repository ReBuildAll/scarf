using Scarf.DataSource;

namespace Scarf.Tests.Infrastructure
{
    internal class TestDataSourceFactory : DataSourceFactory
    {
        private readonly ScarfDataSource _dataSource;

        public TestDataSourceFactory(ScarfDataSource dataSource = null)
        {
            this._dataSource = dataSource;
        }

        public ScarfDataSource CreateDataSourceInstance()
        {
            if (_dataSource != null)
            {
                return _dataSource;
            }

            return new TestDataSource();
        }
    }
}
