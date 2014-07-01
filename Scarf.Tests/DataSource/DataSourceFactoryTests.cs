using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scarf.DataSource;

namespace Scarf.Tests.DataSource
{
    [TestClass]
    public class DataSourceFactoryTests
    {
        [TestMethod]
        public void DefaultDataSourceFactory()
        {
            DataSourceFactory factory = ScarfConfiguration.DataSourceFactory;
            Assert.AreEqual("Scarf.DataSource.DefaultDataSourceFactory", factory.GetType().FullName);
        }

        [TestMethod]
        public void ChangeDataSourceFactory()
        {
            
        }
    }
}
