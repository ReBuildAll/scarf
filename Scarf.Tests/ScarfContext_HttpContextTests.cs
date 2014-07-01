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

using System.Collections.Generic;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Scarf.Tests.Configuration;
using Scarf.Tests.Infrastructure;

namespace Scarf.Tests
{
    [TestClass]
    public class ScarfContext_HttpContextTests
    {
        private static TestDataSource dataSource;

        [ClassInitialize]
        public static void InitClass(TestContext context)
        {
            dataSource = new TestDataSource();
            ScarfConfiguration.DataSourceFactory = new TestDataSourceFactory(dataSource);

            ScarfConfiguration.ConfigurationSection = ConfigurationMocks.CreateNewScarfSectionMock().Object;
        }

        private Mock<HttpContextBase> CreateHttpContext()
        {
            var httpContext = new Mock<HttpContextBase>();

            var items = new Dictionary<string, object>();
            httpContext.SetupGet(h => h.Items).Returns(items);

            return httpContext;
        }

        [TestInitialize]
        public void InitTests()
        {
            dataSource.Clear();
        }

        [TestMethod]
        public void ContextSeparation()
        {
            var httpContext1 = CreateHttpContext();
            var httpContext2 = CreateHttpContext();

            using (ScarfContext context1 = ScarfContext.CreateInlineContext(httpContext1.Object))
            using (ScarfContext context2 = ScarfContext.CreateInlineContext(httpContext2.Object))
            {                
            }
        }
    }
}
