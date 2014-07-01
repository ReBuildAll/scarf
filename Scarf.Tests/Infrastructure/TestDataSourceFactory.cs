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

using Scarf.DataSource;

namespace Scarf.Tests.Infrastructure
{
    internal class TestDataSourceFactory : DataSourceFactory
    {
        private readonly ScarfDataSource _dataSource;

        public TestDataSourceFactory(ScarfDataSource dataSource = null)
        {
            this._dataSource = dataSource;
        }

        public ScarfDataSource CreateDataSourceInstance()
        {
            if (_dataSource != null)
            {
                return _dataSource;
            }

            return new TestDataSource();
        }
    }
}
