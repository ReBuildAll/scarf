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

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Scarf.Configuration;
using Scarf.DataSource;
using Scarf.Tests.Infrastructure;
using DataSourceElement = Scarf.Configuration.DataSourceElement;

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

        [TestMethod]
        public void DefaultFactory_NoConfiguration()
        {
            var configMock = new Mock<ScarfSection>();
            ScarfConfiguration.ConfigurationSection = configMock.Object;

            var dataSource = ScarfConfiguration.DataSourceFactory.CreateDataSourceInstance();
            Assert.AreEqual("Scarf.DataSource.MemoryDataSource", dataSource.GetType().FullName);
        }

        [TestMethod]
        public void DefaultFactory_NoDataSourceConfiguration()
        {
            var dsconfigMock = new Mock<Scarf.Configuration.DataSourceElement>();
            var configMock = new Mock<ScarfSection>();
            configMock.SetupGet(c => c.DataSource).Returns(dsconfigMock.Object);
            ScarfConfiguration.ConfigurationSection = configMock.Object;

            var dataSource = ScarfConfiguration.DataSourceFactory.CreateDataSourceInstance();
            Assert.AreEqual("Scarf.DataSource.MemoryDataSource", dataSource.GetType().FullName);
        }

        [TestMethod]
        public void DefaultFactory_InvalidDataSourceConfiguration()
        {
            var dsconfigMock = new Mock<Scarf.Configuration.DataSourceElement>();
            dsconfigMock.SetupGet(c => c.Type).Returns("Foo Bar");
            var configMock = new Mock<ScarfSection>();
            configMock.SetupGet(c => c.DataSource).Returns(dsconfigMock.Object);
            ScarfConfiguration.ConfigurationSection = configMock.Object;

            var dataSource = ScarfConfiguration.DataSourceFactory.CreateDataSourceInstance();
            Assert.AreEqual("Scarf.DataSource.MemoryDataSource", dataSource.GetType().FullName);
        }

        [TestMethod]
        public void DefaultFactory_HandleError()
        {
            var dsconfigMock = new Mock<Scarf.Configuration.DataSourceElement>();
            var failingDataSourceType =
                string.Format("{0}, {1}", typeof (FailingDataSource).FullName,
                    typeof (FailingDataSource).Assembly.GetName().Name);
            dsconfigMock.SetupGet(c => c.Type).Returns(failingDataSourceType);
            var configMock = new Mock<ScarfSection>();
            configMock.SetupGet(c => c.DataSource).Returns(dsconfigMock.Object);
            ScarfConfiguration.ConfigurationSection = configMock.Object;

            var dataSource = ScarfConfiguration.DataSourceFactory.CreateDataSourceInstance();
            Assert.AreEqual("Scarf.DataSource.MemoryDataSource", dataSource.GetType().FullName);
        }

        public class FailingDataSource : ScarfDataSource
        {
            public FailingDataSource()
            {
                throw new InvalidOperationException();
            }
            public void Initialize(DataSourceElement configuration)
            {
                throw new NotImplementedException();
            }

            public void SaveLogMessages(IEnumerable<ScarfLogMessage> messages)
            {
                throw new NotImplementedException();
            }

            public int GetMessages(string application, int pageIndex, int pageSize, ICollection<ScarfLogMessage> messageList)
            {
                throw new NotImplementedException();
            }

            public ScarfLogMessage GetMessageById(Guid messageId)
            {
                throw new NotImplementedException();
            }
        }
    }
}
