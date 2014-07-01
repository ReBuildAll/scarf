#region Copyright and license
//
// SCARF - Security Audit, Access and Action Logging
// Copyright (c) 2014 ReBuildAll Solutions Ltd
//
// Author:
//    Lenard Gunda 
//
// Licensed under MIT license, see included LICENSE file for details
#endregion

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

            _scarfSectionMock = ConfigurationMocks.CreateNewScarfSectionMock();
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
