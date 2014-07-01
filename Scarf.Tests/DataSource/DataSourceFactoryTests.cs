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
