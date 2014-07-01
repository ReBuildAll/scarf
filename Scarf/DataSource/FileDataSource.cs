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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Scarf.Configuration;

namespace Scarf.DataSource
{
    public sealed class FileDataSource : ScarfDataSource
    {
        private string loggingFolder;

        public void Initialize(DataSourceElement configuration)
        {
            if (Directory.Exists(configuration.Path) == false)
            {
                Directory.CreateDirectory(configuration.Path);
            }
            loggingFolder = configuration.Path;
        }

        public void SaveLogMessage(ScarfLogMessage message)
        {
            string json = JsonConvert.SerializeObject(message, Formatting.Indented);

            string filename = string.Format("{0}-{1:yyyy-MM-ddTHHmmss}-{2}.json",
                message.Application,
                DateTime.UtcNow,
                message.EntryId.ToString("D"));

            File.WriteAllText(Path.Combine(loggingFolder, filename), json);
        }

        public void SaveLogMessages(params ScarfLogMessage[] messages)
        {
            foreach (var scarfLogMessage in messages)
            {
                SaveLogMessage(scarfLogMessage);
            }
        }

        public int GetMessages(string application, int pageIndex, int pageSize, ICollection<ScarfLogMessage> messageList)
        {
            var directoryInfo = new DirectoryInfo(loggingFolder);
            FileInfo[] files = directoryInfo.GetFiles(application + "*.json");

            if (files.Length == 0) return 0;

            string[] orderedFiles = files.Where(info => IsUserFile(info.Attributes))
                .OrderBy(info => info.Name, StringComparer.OrdinalIgnoreCase)
                .Select(info => Path.Combine(loggingFolder, info.Name))
                .Reverse()
                .ToArray();

            if (orderedFiles.Length > 0)
            {
                IEnumerable<ScarfLogMessage> filteredFiles = orderedFiles.Skip(pageIndex*pageSize)
                    .Take(pageSize)
                    .Select(LoadLogMessage);

                foreach (var logMessage in filteredFiles)
                {
                    messageList.Add(logMessage);
                }
            }

            return orderedFiles.Length;
        }

        public ScarfLogMessage GetMessageById(Guid messageId)
        {
            var directoryInfo = new DirectoryInfo(loggingFolder);
            FileInfo[] files = directoryInfo.GetFiles(string.Format("*{0}.json", messageId));
            if (files.Length != 1) return null;

            return LoadLogMessage(Path.Combine(loggingFolder, files[0].Name));
        }

        private ScarfLogMessage LoadLogMessage(string filename)
        {
            string json = File.ReadAllText(filename);

            ScarfLogMessage logMessage = JsonConvert.DeserializeObject<ScarfLogMessage>(json);

            return logMessage;
        }

        private static bool IsUserFile(FileAttributes attributes)
        {
            return 0 == (attributes & (FileAttributes.Directory |
                                       FileAttributes.Hidden |
                                       FileAttributes.System));
        }

    }
}
