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
using Scarf.Tests.Configuration;
using Scarf.Tests.Infrastructure;

namespace Scarf.Tests
{
    [TestClass]
    public class ScarfContext_SecondaryMessagesTests
    {
        private static TestDataSource dataSource;

        [ClassInitialize]
        public static void InitClass(TestContext context)
        {
            dataSource = new TestDataSource();
            ScarfConfiguration.DataSourceFactory = new TestDataSourceFactory(dataSource);

            ScarfConfiguration.ConfigurationSection = ConfigurationMocks.CreateNewScarfSectionMock().Object;
        }

        [TestInitialize]
        public void InitTests()
        {
            dataSource.Clear();
        }

        [TestMethod]
        public void NoMessages()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                context.Commit();
            }

            Assert.AreEqual(0, dataSource.Messages.Count);
        }

        [TestMethod]
        public void OnePrimary_ZeroSecondary()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                context.CreateMessage(MessageClass.Access, MessageType.AccessRead);
                context.Commit();
            }

            Assert.AreEqual(1, dataSource.Messages.Count);
        }

        [TestMethod]
        public void OnePrimary_OneSecondary()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                context.CreateMessage(MessageClass.Access, MessageType.AccessRead);
                ScarfLogging.Debug("Hello world!");
                context.Commit();
            }

            Assert.AreEqual(2, dataSource.Messages.Count);
        }

        [TestMethod]
        public void ZeroPrimary_OneSecondary()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                ScarfLogging.Debug("Hello world!");
                context.Commit();
            }

            Assert.AreEqual(1, dataSource.Messages.Count);
        }

        [TestMethod]
        public void ZeroPrimary_MultipleSecondary()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                ScarfLogging.Debug("Hello world!");
                ScarfLogging.Debug("Hello world!");
                ScarfLogging.Debug("Hello world!");
                context.Commit();
            }

            Assert.AreEqual(3, dataSource.Messages.Count);
        }

        [TestMethod]
        public void DebugMessage()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                ScarfLogging.Debug("Testing");
                ScarfLogging.Debug("Testing 123");
                context.Commit();
            }

            Assert.AreEqual(2, dataSource.Messages.Count);
        }
    }
}
