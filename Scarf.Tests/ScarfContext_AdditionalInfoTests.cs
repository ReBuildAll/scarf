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
using Scarf.Tests.Infrastructure;

namespace Scarf.Tests
{
    [TestClass]
    public class ScarfContext_AdditionalInfoTests : InlineScarfContextTestBase
    {
        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            InitTests();
        }

        [TestMethod]
        public void TestAddCustomInfo()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                ScarfAction.Start(MessageType.ActionPayment);
                ScarfLogging.AddCustomInfo("organizationId", "5");
                context.Commit();
            }
            
            ScarfLogMessage msg = dataSource.Messages.Single();

            Assert.IsTrue(msg.AdditionalInfo.ContainsKey(ScarfLogMessage.AdditionalInfo_Custom));
            Assert.AreEqual("5", msg.AdditionalInfo[ScarfLogMessage.AdditionalInfo_Custom]["organizationId"]);
        }
    }
}
