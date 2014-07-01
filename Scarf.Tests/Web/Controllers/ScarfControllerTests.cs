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
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scarf.Tests.Configuration;
using Scarf.Tests.Infrastructure;
using Scarf.Web.Controllers;

namespace Scarf.Tests.Web.Controllers
{
    [TestClass]
    public class ScarfControllerTests : ScarfMvcControllerTestBase<ScarfController>
    {
        private static TestDataSource dataSource;

        private static Guid firstMessageId;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            dataSource = new TestDataSource();
            ScarfConfiguration.DataSourceFactory = new TestDataSourceFactory(dataSource);

            DateTime startTime = DateTime.UtcNow.AddDays(-30);
            for (int i = 0; i < ScarfController.PAGE_SIZE * 2 + 1; i++)
            {
                var testMessage = new ScarfLogMessage(null)
                {
                    Application = ConfigurationMocks.ApplicationName,
                    EntryId = Guid.NewGuid(),
                    LoggedAt = startTime,
                    MessageClass = MessageClass.Audit,
                    MessageType = MessageType.AuditCreateUser,
                    Message = MessageType.GetDefaultMessage(MessageType.AuditCreateUser),
                };
                if (i == 0)
                {
                    firstMessageId = testMessage.EntryId;
                }

                startTime = startTime.AddHours(1.5);

                dataSource.SaveLogMessage(testMessage);
            }
        }

        [TestMethod]
        public void Index()
        {
            ActionResult result = Controller.Index(null);

            Assert.IsNotNull(result);

            Assert.AreEqual(ScarfController.PAGE_SIZE * 2 + 1, (int)Controller.ViewBag.TotalMessages);
            Assert.AreEqual(1, (int)Controller.ViewBag.CurrentPage);
            Assert.AreEqual(3, (int)Controller.ViewBag.TotalPages);
        }

        [TestMethod]
        public void Index_WithPaging()
        {
            ActionResult result = Controller.Index(2);

            Assert.IsNotNull(result);

            Assert.AreEqual(ScarfController.PAGE_SIZE * 2 + 1, (int)Controller.ViewBag.TotalMessages);
            Assert.AreEqual(2, (int)Controller.ViewBag.CurrentPage);
            Assert.AreEqual(3, (int)Controller.ViewBag.TotalPages);
        }

        [TestMethod]
        public void Index_WithNegativePage()
        {
            ActionResult result = Controller.Index(-1);

            Assert.IsNotNull(result);

            Assert.AreEqual(ScarfController.PAGE_SIZE * 2 + 1, (int)Controller.ViewBag.TotalMessages);
            Assert.AreEqual(1, (int)Controller.ViewBag.CurrentPage);
            Assert.AreEqual(3, (int)Controller.ViewBag.TotalPages);
        }

        [TestMethod]
        public void Index_WithIllegalPageNumber()
        {
            ActionResult result = Controller.Index(99);

            Assert.IsNotNull(result);

            Assert.AreEqual(ScarfController.PAGE_SIZE * 2 + 1, (int)Controller.ViewBag.TotalMessages);
            Assert.AreEqual(99, (int)Controller.ViewBag.CurrentPage);
            Assert.AreEqual(3, (int)Controller.ViewBag.TotalPages);
        }

        [TestMethod]
        public void Details_Success()
        {
            ActionResult result = Controller.Details(firstMessageId);
        }

        [TestMethod]
        public void Details_NullParameter()
        {
            ActionResult result = Controller.Details(null);
            Assert.IsInstanceOfType(result, typeof (RedirectToRouteResult));
        }

        [TestMethod]
        public void Details_MissingId()
        {
            ActionResult result = Controller.Details(Guid.Empty);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }
    }
}
