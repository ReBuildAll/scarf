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
using Scarf.MVC;
using Scarf.Tests.Infrastructure;

namespace Scarf.Tests.MVC
{
    [TestClass]
    public class LogAccessAttributeTests : ScarfLoggingAttributeTestBase<LogAccessAttribute>
    {
        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            InitTests();
            var accessElementMock = new Mock<AccessElement>();
            accessElementMock.SetupGet(a => a.Enabled).Returns(true);
            _scarfSectionMock.SetupGet(s => s.Access).Returns(accessElementMock.Object);
        }

        [TestMethod]
        public void Access()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                var attribute = new LogAccessAttribute(MessageType.AccessRead);
                BeforeAction(attribute);
                AfterAction(attribute);
            }

            Assert.AreEqual(1, dataSource.Messages.Count);
        }

    }
}
