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
using Newtonsoft.Json;

namespace Scarf.Tests
{
    [TestClass]
    public class ScarfLogMessageTests
    {
        [TestMethod]
        public void Serialization()
        {
            AuditLogMessage message = new AuditLogMessage(null)
            {
                Application = "1",
                Computer = "2",
                Details = "3",
                EntryId = Guid.NewGuid(),
                LoggedAt = DateTime.UtcNow,
                Message = "4",
                MessageClass = MessageClass.Audit,
                MessageType = "5",
                ResourceUri = "6",
                Source = "7",
                Success = true,
                User = "8"
            };

            string serializedMessage = JsonConvert.SerializeObject(message);

            ScarfLogMessage message2 = JsonConvert.DeserializeObject<ScarfLogMessage>(serializedMessage);

            Assert.IsNotNull(message2);

            Assert.AreEqual(message.Application, message2.Application);
            Assert.AreEqual(message.Computer, message2.Computer);
            Assert.AreEqual(message.Details, message2.Details);
            Assert.AreEqual(message.EntryId, message2.EntryId);
            Assert.AreEqual(message.LoggedAt, message2.LoggedAt);
            Assert.AreEqual(message.Message, message2.Message);
            Assert.AreEqual(message.MessageClass, message2.MessageClass);
            Assert.AreEqual(message.MessageType, message2.MessageType);
            Assert.AreEqual(message.ResourceUri, message2.ResourceUri);
            Assert.AreEqual(message.Source, message2.Source);
            Assert.AreEqual(message.User, message2.User);

        }
    }
}
