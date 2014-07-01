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

using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Scarf.Configuration;
using Scarf.Tests.Configuration;
using Scarf.Web;

namespace Scarf.Tests.Infrastructure
{
    [TestClass]
    public class ScarfMvcControllerTestBase<TController> where TController : Controller,new()
    {
        private TController _controller;

        private Mock<ScarfSection> _scarfSectionMock;
        private Mock<ScarfViewResultFactory> _viewResultFactory;

        public ScarfMvcControllerTestBase()
        {
            MockConfiguration();
            MockViewResultFactory();
        }

        private void MockViewResultFactory()
        {
            var actionResultMock = new Mock<ActionResult>();

            _viewResultFactory = new Mock<ScarfViewResultFactory>();

            _viewResultFactory.Setup(r => r.Create(It.IsAny<Controller>(), It.IsAny<string>(), It.IsAny<object>()))
                .Returns(actionResultMock.Object);

            ScarfConfiguration.ViewResultFactory = _viewResultFactory.Object;
        }

        private void MockConfiguration()
        {
            var testDataSourceTypeName = string.Format("{0}, {1}", typeof (TestDataSource).FullName,
                typeof (TestDataSource).Assembly.GetName().Name);

            var dataSourceMock = new Mock<Scarf.Configuration.DataSourceElement>();
            dataSourceMock.SetupGet(ds => ds.Type).Returns(testDataSourceTypeName);

            _scarfSectionMock = ConfigurationMocks.CreateNewScarfSectionMock();
            _scarfSectionMock.SetupGet(s => s.DataSource).Returns(dataSourceMock.Object);

            ScarfConfiguration.ConfigurationSection = _scarfSectionMock.Object;
        }


        protected TController Controller
        {
            get
            {
                if (_controller == null)
                {
                    _controller = new TController();
                }

                return _controller;
            }
        }
    }
}
