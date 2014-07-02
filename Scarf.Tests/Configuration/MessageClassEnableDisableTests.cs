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

namespace Scarf.Tests.Configuration
{
    [TestClass]
    public class MessageClassEnableDisableTests
    {
        private static TestDataSource dataSource;

        private static Mock<AuditElement> auditElementMock;
        private static Mock<AccessElement> accessElementMock;
        private static Mock<ActionElement> actionElementMock;
        private static Mock<DebugElement> debugElementMock;

        [ClassInitialize]
        public static void InitClass(TestContext context)
        {
            dataSource = new TestDataSource();
            ScarfConfiguration.DataSourceFactory = new TestDataSourceFactory(dataSource);

            var scarfSectionMock = ConfigurationMocks.CreateNewScarfSectionMock();

            debugElementMock = new Mock<DebugElement>();
            auditElementMock = new Mock<AuditElement>();
            accessElementMock = new Mock<AccessElement>();
            actionElementMock = new Mock<ActionElement>();

            scarfSectionMock.SetupGet(s => s.Access).Returns(accessElementMock.Object);
            scarfSectionMock.SetupGet(s => s.Action).Returns(actionElementMock.Object);
            scarfSectionMock.SetupGet(s => s.Audit).Returns(auditElementMock.Object);
            scarfSectionMock.SetupGet(s => s.Debug).Returns(debugElementMock.Object);

            ScarfConfiguration.ConfigurationSection = scarfSectionMock.Object;
        }

        [TestInitialize]
        public void InitTests()
        {
            dataSource.Clear();

            auditElementMock.SetupGet(d => d.Enabled).Returns(true);
            auditElementMock.SetupGet(d => d.LogOnlyFailures).Returns(false);
            accessElementMock.SetupGet(d => d.Enabled).Returns(true);
            actionElementMock.SetupGet(d => d.Enabled).Returns(true);
            debugElementMock.SetupGet(d => d.Enabled).Returns(true);
        }

        [TestMethod]
        public void DebugMessages_Enabled()
        {
            debugElementMock.SetupGet(d => d.Enabled).Returns(true);

            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                context.CreateMessage(MessageClass.Debug, MessageType.DebugMessage);
                context.Commit();
            }

            Assert.AreEqual(1, dataSource.Messages.Count);
            Assert.AreEqual(MessageClass.Debug, dataSource.Messages.First().MessageClass);
        }

        [TestMethod]
        public void DebugMessages_SettingsNoConflict()
        {
            debugElementMock.SetupGet(d => d.Enabled).Returns(false);

            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                context.CreateMessage(MessageClass.Access, MessageType.AccessRead);
                context.Commit();
                context.CreateMessage(MessageClass.Action, MessageType.ActionCommand);
                context.Commit();
                context.CreateMessage(MessageClass.Audit, MessageType.AuditLogout);
                context.Commit();
            }

            Assert.AreEqual(3, dataSource.Messages.Count);
        }

        [TestMethod]
        public void DebugMessages_Disabled()
        {
            debugElementMock.SetupGet(d => d.Enabled).Returns(false);

            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                context.CreateMessage(MessageClass.Debug, MessageType.DebugMessage);
                context.Commit();
            }

            Assert.AreEqual(0, dataSource.Messages.Count);
        }

        [TestMethod]
        public void AccessMessages_Enabled()
        {
            accessElementMock.SetupGet(d => d.Enabled).Returns(true);

            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                context.CreateMessage(MessageClass.Access, MessageType.AccessRead);
                context.Commit();
            }

            Assert.AreEqual(1, dataSource.Messages.Count);
            Assert.AreEqual(MessageClass.Access, dataSource.Messages.First().MessageClass);
        }

        [TestMethod]
        public void AccessMessages_Disabled()
        {
            accessElementMock.SetupGet(d => d.Enabled).Returns(false);

            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                context.CreateMessage(MessageClass.Access, MessageType.AccessRead);
                context.Commit();
            }

            Assert.AreEqual(0, dataSource.Messages.Count);
        }

        [TestMethod]
        public void ActionMessages_Enabled()
        {
            actionElementMock.SetupGet(d => d.Enabled).Returns(true);

            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                context.CreateMessage(MessageClass.Action, MessageType.ActionCommand);
                context.Commit();
            }

            Assert.AreEqual(1, dataSource.Messages.Count);
            Assert.AreEqual(MessageClass.Action, dataSource.Messages.First().MessageClass);
        }

        [TestMethod]
        public void ActionMessages_Disabled()
        {
            actionElementMock.SetupGet(d => d.Enabled).Returns(false);
            
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                context.CreateMessage(MessageClass.Action, MessageType.ActionCommand);
                context.Commit();
            }

            Assert.AreEqual(0, dataSource.Messages.Count);
        }

        [TestMethod]
        public void AuditMessages_Enabled()
        {
            auditElementMock.SetupGet(d => d.Enabled).Returns(true);

            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                context.CreateMessage(MessageClass.Audit, MessageType.AuditResetPassword);
                context.Commit();
            }

            Assert.AreEqual(1, dataSource.Messages.Count);
            Assert.AreEqual(MessageClass.Audit, dataSource.Messages.First().MessageClass);
        }

        [TestMethod]
        public void AuditSuccess_EnabledNotFailuresOnly()
        {
            auditElementMock.SetupGet(d => d.Enabled).Returns(true);
            auditElementMock.SetupGet(d => d.LogOnlyFailures).Returns(false);

            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                ScarfAudit.Start(MessageType.AuditResetPassword);
                ScarfAudit.Succeeded();
                context.Commit();
            }

            Assert.AreEqual(1, dataSource.Messages.Count);
            Assert.AreEqual(MessageClass.Audit, dataSource.Messages.First().MessageClass);
        }

        [TestMethod]
        public void AuditSuccess_EnabledFailuresOnly()
        {
            auditElementMock.SetupGet(d => d.Enabled).Returns(true);
            auditElementMock.SetupGet(d => d.LogOnlyFailures).Returns(true);

            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                ScarfAudit.Start(MessageType.AuditResetPassword);
                ScarfAudit.Succeeded();
                context.Commit();
            }

            Assert.AreEqual(0, dataSource.Messages.Count);
        }

        [TestMethod]
        public void AuditFailure_EnabledFailuresOnly()
        {
            auditElementMock.SetupGet(d => d.Enabled).Returns(true);
            auditElementMock.SetupGet(d => d.LogOnlyFailures).Returns(true);

            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                ScarfAudit.Start(MessageType.AuditResetPassword);
                ScarfAudit.Failed();
                context.Commit();
            }

            Assert.AreEqual(1, dataSource.Messages.Count);
            Assert.AreEqual(MessageClass.Audit, dataSource.Messages.First().MessageClass);
        }

        [TestMethod]
        public void AuditMessages_Disabled()
        {
            auditElementMock.SetupGet(d => d.Enabled).Returns(false);

            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                context.CreateMessage(MessageClass.Audit, MessageType.AuditResetPassword);
                context.Commit();
            }

            Assert.AreEqual(0, dataSource.Messages.Count);
        }

    }
}
