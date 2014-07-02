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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Scarf.Configuration;
using Scarf.Tests.Infrastructure;
using Scarf.WebApi;

namespace Scarf.Tests.WebApi
{
    [TestClass]
    public class LogApiAuditAttributeTests : ScarfApiLoggingAttributeTestBase<LogApiAuditAttribute>
    {
        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {
            InitTests();
            var auditSectionMock = new Mock<AuditElement>();
            auditSectionMock.SetupGet(a => a.Enabled).Returns(true);
            auditSectionMock.SetupGet(a => a.LogOnlyFailures).Returns(true);
            _scarfSectionMock.SetupGet(s => s.Audit).Returns(auditSectionMock.Object);
        }

        [TestMethod]
        public void AuditSuccess_IsNotLogged()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                var attribute = new LogApiAuditAttribute(MessageType.AuditLogin);
                BeforeAction(attribute);
                ScarfAudit.LoggedInAs("Test");
                AfterAction(attribute);
            }

            Assert.AreEqual(0, dataSource.Messages.Count);
        }

        [TestMethod]
        public void AuditFailure_IsLogged()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                var attribute = new LogApiAuditAttribute(MessageType.AuditLogin);
                BeforeAction(attribute);
                ScarfAudit.Failed();
                AfterAction(attribute);
            }

            Assert.AreEqual(1, dataSource.Messages.Count);
        }

        [TestMethod]
        public void AuditFailureWithModelStateErrors_IsLogged()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                var attribute = new LogApiAuditAttribute(MessageType.AuditLogin);
                BeforeAction(attribute);
                ScarfAudit.Failed();
                AfterAction(attribute, true);
            }

            Assert.AreEqual(1, dataSource.Messages.Count);
        }

        [TestMethod]
        public void Audit_DetectSuccess()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                var attribute = new LogApiAuditAttribute(MessageType.AuditLogin);
                BeforeAction(attribute);
                AfterAction(attribute);
            }

            Assert.AreEqual(0, dataSource.Messages.Count);
        }
        
        [TestMethod]
        public void Audit_DetectUnhandledException()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                var attribute = new LogApiAuditAttribute(MessageType.AuditLogin);
                BeforeAction(attribute);
                ActionThrewException(new InvalidOperationException());
                AfterAction(attribute);
            }

            Assert.AreEqual(1, dataSource.Messages.Count);
        }

    }
}
