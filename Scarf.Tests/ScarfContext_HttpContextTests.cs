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
using System.Security.Principal;
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
        private const string RequestPath = "/request/to/resource";
        private const string CurrentUsername = "testuser";

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
            var identity = new Mock<IIdentity>();
            identity.SetupGet(i => i.Name).Returns(CurrentUsername);

            var principal = new Mock<IPrincipal>();
            principal.SetupGet(p => p.Identity).Returns(identity.Object);

            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.Path).Returns(RequestPath);

            var httpContext = new Mock<HttpContextBase>();
            var items = new Dictionary<string, object>();
            httpContext.SetupGet(h => h.Items).Returns(items);
            httpContext.SetupGet(h => h.Request).Returns(request.Object);
            httpContext.SetupGet(h => h.User).Returns(principal.Object);

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

        [TestMethod]
        public void ExtractHttpContextInformation()
        {
            var httpContext1 = CreateHttpContext();

            using (ScarfContext context = ScarfContext.CreateInlineContext(httpContext1.Object))
            {
                context.CreatePrimaryMessage(MessageClass.Action, MessageType.ActionPayment);
                context.Commit();
            }

            ScarfLogMessage msg = dataSource.Messages.Single();

            Assert.AreEqual(RequestPath, msg.ResourceUri);
            Assert.AreEqual(CurrentUsername, msg.User);
            Assert.AreEqual(Environment.MachineName, msg.Computer);
            Assert.AreEqual(ConfigurationMocks.ApplicationName, msg.Application);
        }
    }
}
