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
using Scarf.Tests.Configuration;

namespace Scarf.Tests.Infrastructure
{
    [TestClass]
    public abstract class InlineScarfContextTestBase
    {
        internal static TestDataSource dataSource;

        protected static Mock<ScarfSection> _scarfSectionMock;

        protected static void InitTests()
        {
            dataSource = new TestDataSource();
            ScarfConfiguration.DataSourceFactory = new TestDataSourceFactory(dataSource);

            _scarfSectionMock = ConfigurationMocks.CreateNewScarfSectionMock();
            ScarfConfiguration.ConfigurationSection = _scarfSectionMock.Object;
        }

        [TestInitialize]
        public void BeforeEachTest()
        {
            dataSource.Clear();
        }
    }
}
