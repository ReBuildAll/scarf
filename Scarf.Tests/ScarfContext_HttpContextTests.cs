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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scarf.Tests.Configuration;
using Scarf.Tests.Infrastructure;

namespace Scarf.Tests
{
    [TestClass]
    class ScarfContext_HttpContextTests
    {
        private static TestDataSource dataSource;

        [ClassInitialize]
        public static void InitClass(TestContext context)
        {
            dataSource = new TestDataSource();
            ScarfConfiguration.DataSourceFactory = new TestDataSourceFactory(dataSource);

            ScarfConfiguration.ConfigurationSection = ConfigurationMocks.CreateNewScarfSectionMock().Object;
        }

        [TestInitialize]
        public void InitTests()
        {
            dataSource.Clear();
        }
    }
}
