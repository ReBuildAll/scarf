﻿#region Copyright and license
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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scarf.Tests.Configuration;
using Scarf.Tests.Infrastructure;

namespace Scarf.Tests
{
    [TestClass]
    public class ScarfContext_InlineTests
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
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotNestInlineContexts()
        {
            using (IScarfContext context1 = ScarfLogging.BeginInlineContext())
            using (IScarfContext context2 = ScarfLogging.BeginInlineContext())
            {
            }
        }

        [TestMethod]
        public void SingleAmbientMessage()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                context.CreateMessage(MessageClass.Debug, MessageType.DebugMessage);
                context.Commit();
            }

            Assert.AreEqual(1, dataSource.Messages.Count);
            Assert.AreEqual(MessageClass.Debug, dataSource.Messages.First().MessageClass);
        }

        [TestMethod]
        public void NoChangesWithNoCommit()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                context.CreateMessage(MessageClass.Debug, MessageType.DebugMessage);
            }

            Assert.AreEqual(0, dataSource.Messages.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotCreateSeveralAmbientMessages()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                context.CreateMessage(MessageClass.Debug, MessageType.DebugMessage);
                context.CreateMessage(MessageClass.Debug, MessageType.DebugMessage);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DisposedContextCannotBeUsed()
        {
            using (IScarfContext context = ScarfLogging.BeginInlineContext())
            {
                context.Dispose();
                context.CreateMessage(MessageClass.Debug, MessageType.DebugMessage);
            }
        }
    }
}
