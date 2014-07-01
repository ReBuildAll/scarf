using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Scarf.Configuration;
using Scarf.DataSource;
using Scarf.Tests.Infrastructure;

namespace Scarf.Tests.Configuration
{
    [TestClass]
    public class DataSourceElementTests
    {
        private Mock<ScarfSection> _scarfSectionMock;

        public DataSourceElementTests()
        {
            var testDataSourceTypeName = string.Format("{0}, {1}", typeof (TestDataSource).FullName,
                typeof (TestDataSource).Assembly.GetName().Name);

            var dataSourceMock = new Mock<Scarf.Configuration.DataSourceElement>();
            dataSourceMock.SetupGet(ds => ds.Type).Returns(testDataSourceTypeName);

            _scarfSectionMock = new Mock<ScarfSection>();
            _scarfSectionMock.SetupGet(s => s.DataSource).Returns(dataSourceMock.Object);

            ScarfConfiguration.ConfigurationSection = _scarfSectionMock.Object;
        }

        [TestMethod]
        public void DataSourceLoadedFromConfiguration()
        {
            ScarfDataSource dataSource = ScarfConfiguration.DataSourceFactory.CreateDataSourceInstance();
            Assert.IsInstanceOfType(dataSource, typeof (TestDataSource));
        }
    }
}
