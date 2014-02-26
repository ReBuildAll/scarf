using System.Collections.Generic;
using Scarf.Configuration;

namespace Scarf.DataSource
{
    public interface ScarfDataSource
    {
        void Initialize(DataSourceSection configuration);

        void SaveLogMessage(LogMessage message);

        IEnumerable<LogMessage> GetMessages(string application);

    }
}
