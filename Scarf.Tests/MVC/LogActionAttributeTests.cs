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

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Scarf.Configuration;
using Scarf.MVC;
using Scarf.Tests.Infrastructure;

namespace Scarf.Tests.MVC
{
    [TestClass]
    public class LogActionAttributeTests : ScarfLoggingAttributeTestBase<LogActionAttribute>
    {
        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            InitTests();
            var actionSectionMock = new Mock<ActionElement>();
            actionSectionMock.SetupGet(a => a.Enabled).Returns(true);
            _scarfSectionMock.SetupGet(s => s.Action).Returns(actionSectionMock.Object);
        }

        [TestMethod]
        public void Action()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                var attribute = new LogActionAttribute(MessageType.ActionPayment);
                BeforeAction(attribute);
                AfterAction(attribute);
            }

            Assert.AreEqual(1, dataSource.Messages.Count);
        }

        [TestMethod]
        public void ActionWithMessage()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                var attribute = new LogActionAttribute(MessageType.ActionPayment)
                {
                    Message = "Hello there!"
                };
                BeforeAction(attribute);
                AfterAction(attribute);
            }

            Assert.AreEqual(1, dataSource.Messages.Count);
            Assert.AreEqual("Hello there!", dataSource.Messages.Single().Message);
        }

        [TestMethod]
        public void ActionWithCodeBasedMessage()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                var attribute = new LogActionAttribute(MessageType.ActionPayment);
                BeforeAction(attribute);
                ScarfAction.SetMessage("From code");
                AfterAction(attribute);
            }

            Assert.AreEqual(1, dataSource.Messages.Count);
            Assert.AreEqual("From code", dataSource.Messages.Single().Message);
        }

    }
}
