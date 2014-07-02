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
    public class ScarfAuditTests : InlineScarfContextTestBase
    {
        [ClassInitialize]
        public static void InitializeClass(TestContext ctx)
        {            
            InitTests();
            var auditSectionMock = new Mock<AuditElement>();
            auditSectionMock.SetupGet(a => a.Enabled).Returns(true);
            auditSectionMock.SetupGet(a => a.LogOnlyFailures).Returns(false);
            _scarfSectionMock.SetupGet(s => s.Audit).Returns(auditSectionMock.Object);
        }

        [TestMethod]
        public void LoginAudit()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                ScarfAudit.Start(MessageType.AuditLogin);
                ScarfAudit.LoggedInAs("Test");
                context.Commit();
            }

            ScarfLogMessage msg = dataSource.Messages.Single();
            Assert.AreEqual(MessageClass.Audit, msg.MessageClass);
            Assert.AreEqual(MessageType.AuditLogin, msg.MessageType);
            Assert.AreEqual(true, msg.Success.Value);
        }

        [TestMethod]
        public void LoginAuditFailed()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                ScarfAudit.Start(MessageType.AuditLogin);
                ScarfAudit.Failed();
                context.Commit();
            }

            ScarfLogMessage msg = dataSource.Messages.Single();
            Assert.AreEqual(MessageClass.Audit, msg.MessageClass);
            Assert.AreEqual(MessageType.AuditLogin, msg.MessageType);
            Assert.AreEqual(false, msg.Success.Value);
        }

        [TestMethod]
        public void LogoutAudit()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                ScarfAudit.Start(MessageType.AuditLogout);
                ScarfAudit.LoggedOut("Test");
                context.Commit();
            }

            ScarfLogMessage msg = dataSource.Messages.Single();
            Assert.AreEqual(MessageClass.Audit, msg.MessageClass);
            Assert.AreEqual(MessageType.AuditLogout, msg.MessageType);
        }

        [TestMethod]
        public void CreateUserAudit()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                ScarfAudit.Start(MessageType.AuditCreateUser);
                ScarfAudit.UserCreated("Test");
                context.Commit();
            }

            ScarfLogMessage msg = dataSource.Messages.Single();
            Assert.AreEqual(MessageClass.Audit, msg.MessageClass);
            Assert.AreEqual(MessageType.AuditCreateUser, msg.MessageType);
            Assert.AreEqual(true, msg.Success.Value);
        }

        [TestMethod]
        public void DeleteUserAudit()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                ScarfAudit.Start(MessageType.AuditDeleteUser);
                ScarfAudit.UserDeleted("Test");
                context.Commit();
            }

            ScarfLogMessage msg = dataSource.Messages.Single();
            Assert.AreEqual(MessageClass.Audit, msg.MessageClass);
            Assert.AreEqual(MessageType.AuditDeleteUser, msg.MessageType);
            Assert.AreEqual(true, msg.Success.Value);
        }

        [TestMethod]
        public void ChangePasswordAudit()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                ScarfAudit.Start(MessageType.AuditChangePassword);
                ScarfAudit.PasswordChanged("Test");
                context.Commit();
            }

            ScarfLogMessage msg = dataSource.Messages.Single();
            Assert.AreEqual(MessageClass.Audit, msg.MessageClass);
            Assert.AreEqual(MessageType.AuditChangePassword, msg.MessageType);
            Assert.AreEqual(true, msg.Success.Value);
        }

        [TestMethod]
        public void ResetPasswordAudit()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                ScarfAudit.Start(MessageType.AuditResetPassword);
                ScarfAudit.PasswordReset("Test");
                context.Commit();
            }

            ScarfLogMessage msg = dataSource.Messages.Single();
            Assert.AreEqual(MessageClass.Audit, msg.MessageClass);
            Assert.AreEqual(MessageType.AuditResetPassword, msg.MessageType);
            Assert.AreEqual(true, msg.Success.Value);
        }

        [TestMethod]
        public void ChangeUserAudit()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                ScarfAudit.Start(MessageType.AuditChangeLogin);
                ScarfAudit.LoginChanged("Old", "Test");
                context.Commit();
            }

            ScarfLogMessage msg = dataSource.Messages.Single();
            Assert.AreEqual(MessageClass.Audit, msg.MessageClass);
            Assert.AreEqual(MessageType.AuditChangeLogin, msg.MessageType);
            Assert.AreEqual(true, msg.Success.Value);
        }

    }
}
