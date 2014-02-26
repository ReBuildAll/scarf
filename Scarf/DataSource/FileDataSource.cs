using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Scarf.Configuration;

namespace Scarf.DataSource
{
    public sealed class FileDataSource : ScarfDataSource
    {
        private string folder;

        public void Initialize(DataSourceSection configuration)
        {
            if (Directory.Exists(configuration.Path) == false)
            {
                Directory.CreateDirectory(configuration.Path);
            }
            folder = configuration.Path;
        }

        public void SaveLogMessage(LogMessage message)
        {
            string json = JsonConvert.SerializeObject(message, Formatting.Indented);

            string filename = string.Format("{0}-{1:yyyy-MM-ddTHHmmss}-{2}.json",
                message.Application,
                DateTime.UtcNow,
                message.EntryId.ToString("D"));

            File.WriteAllText(Path.Combine(folder, filename), json);
        }

        public IEnumerable<LogMessage> GetMessages(string application)
        {
            throw new System.NotImplementedException();
        }
    }
}
