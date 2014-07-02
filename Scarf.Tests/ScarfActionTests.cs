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
using Scarf.Tests.Infrastructure;

namespace Scarf.Tests
{
    [TestClass]
    public class ScarfActionTests: InlineScarfContextTestBase
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
                ScarfAction.Start(MessageType.ActionCommand);
                ScarfAction.SetMessage("Performed command");
                context.Commit();
            }

            ScarfLogMessage msg = dataSource.Messages.Single();
            Assert.AreEqual(MessageClass.Action, msg.MessageClass);
            Assert.AreEqual(MessageType.ActionCommand, msg.MessageType);

        }
    }
}
