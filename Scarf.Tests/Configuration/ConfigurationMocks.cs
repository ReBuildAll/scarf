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

using Moq;
using Scarf.Configuration;

namespace Scarf.Tests.Configuration
{
    public static class ConfigurationMocks
    {
        public const string ApplicationName = "Scarf.Tests";

        public static Mock<ScarfSection> CreateNewScarfSectionMock()
        {
            var scarfSectionMock = new Mock<ScarfSection>();
            scarfSectionMock.SetupGet(s => s.ApplicationName).Returns(ApplicationName);
            return scarfSectionMock;
        }
    }
}
