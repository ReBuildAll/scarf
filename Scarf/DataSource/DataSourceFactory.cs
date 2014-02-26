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
using Scarf.Configuration;

namespace Scarf.DataSource
{
    internal static class DataSourceFactory
    {
        public static ScarfDataSource CreateDataSourceInstance()
        {
            ScarfSection configuration = ScarfLogging.GetConfiguration();
            if (configuration.DataSource == null) return new MemoryDataSource();

            try
            {
                DataSourceSection dataAccessConfiguration = configuration.DataSource;
                if (string.IsNullOrWhiteSpace(dataAccessConfiguration.Type))
                {
                    return new MemoryDataSource();
                }

                Type dataSourceType = Type.GetType(dataAccessConfiguration.Type);
                if (dataSourceType == null)
                {
                    return new MemoryDataSource();
                }

                var dataSource = (ScarfDataSource)Activator.CreateInstance(dataSourceType);
                dataSource.Initialize(dataAccessConfiguration);
                return dataSource;
            }
            catch
            {
                return new MemoryDataSource();
            }
        }
    }
}
