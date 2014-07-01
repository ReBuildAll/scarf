using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scarf.Tests.Infrastructure;

namespace Scarf.Tests
{
    [TestClass]
    public class ScarfContextTests
    {
        private static TestDataSource dataSource;

        [ClassInitialize]
        public static void InitClass(TestContext context)
        {
            dataSource = new TestDataSource();
            ScarfConfiguration.DataSourceFactory = new TestDataSourceFactory(dataSource);
        }

        [TestInitialize]
        public void InitTests()
        {
            dataSource.Clear();
        }

        [TestMethod]
        public void SingleAmbientMessage()
        {
            using (ScarfContext context = ScarfContext.CreateInlineContext())
            {
                
            }
        }
    }
}
