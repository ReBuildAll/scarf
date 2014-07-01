using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scarf.DataSource;
using Scarf.Tests.Infrastructure;

namespace Scarf.Tests.DataSource
{
    [TestClass]
    public class DataSourceFactoryTests
    {
        [TestMethod]
        public void DefaultDataSourceFactory()
        {
            ScarfConfiguration.DataSourceFactory = null;
            DataSourceFactory factory = ScarfConfiguration.DataSourceFactory;
            Assert.AreEqual("Scarf.DataSource.DefaultDataSourceFactory", factory.GetType().FullName);
        }

        [TestMethod]
        public void ChangeDataSourceFactory()
        {
            ScarfConfiguration.DataSourceFactory = new TestDataSourceFactory();

            DataSourceFactory factory = ScarfConfiguration.DataSourceFactory;
            Assert.IsInstanceOfType(factory, typeof (TestDataSourceFactory));

            ScarfConfiguration.DataSourceFactory = null;
        }
    }
}
