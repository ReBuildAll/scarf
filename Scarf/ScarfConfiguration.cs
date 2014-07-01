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

using System.Configuration;
using Scarf.Configuration;
using Scarf.DataSource;
using Scarf.Web;

namespace Scarf
{
    public static class ScarfConfiguration
    {
        private static DataSourceFactory _dataSourceFactory;

        private static ScarfSection _configuration;

        private static ScarfViewResultFactory _viewResultFactory;

        public static DataSourceFactory DataSourceFactory
        {
            get
            {
                if (_dataSourceFactory == null)
                {
                    _dataSourceFactory = new DefaultDataSourceFactory();
                }

                return _dataSourceFactory;
            }
            set
            {
                _dataSourceFactory = value;
            }
        }

        public static ScarfSection ConfigurationSection
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = ConfigurationManager.GetSection("scarf") as ScarfSection;
                }
                return _configuration;
            }
            set
            {
                _configuration = value;
            }
        }

        public static ScarfViewResultFactory ViewResultFactory
        {
            get
            {
                if (_viewResultFactory == null)
                {
                    _viewResultFactory = new BuildManagerBasedScarfViewResultFactory();
                }

                return _viewResultFactory;
            }
            set
            {
                _viewResultFactory = value;
            }
        }

        public static bool IsActionLoggingEnabled
        {
            get
            {
                var configuration = ConfigurationSection;
                if (configuration.Action != null && configuration.Action.Enabled == false)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
