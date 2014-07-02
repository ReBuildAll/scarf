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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Scarf.Configuration;

namespace Scarf.DataSource
{
    public class SqlServerDataSource : ScarfDataSource
    {
        private string connectionStringName;

        public void Initialize(DataSourceElement configuration)
        {
            connectionStringName = configuration.ConnectionStringName;
        }

        public void SaveLogMessages(IEnumerable<ScarfLogMessage> messages)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var message in messages)
                    {
                        SaveLogMessageInternal(message, connection);
                    }
                    transaction.Commit();
                }
                connection.Close();
            }
        }

        private void SaveLogMessageInternal(ScarfLogMessage message, SqlConnection connection)
        {
            using (SqlCommand command = CreateInsertCommand(connection, message))
            {
                int results = command.ExecuteNonQuery();
                // TODO check for errors?                
            }
        }

        public int GetMessages(string application, int pageIndex, int pageSize, ICollection<ScarfLogMessage> messageList)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            using (SqlCommand command = CreateMutipleQueryCommand(connection, application, pageIndex, pageSize))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ReadAllMessagesFromReader(messageList, reader );
                }

                return (int) command.Parameters["@totalMessages"].Value;
            }
        }

        private void ReadAllMessagesFromReader(ICollection<ScarfLogMessage> messageList, SqlDataReader reader)
        {
            while (reader.Read())
            {
                ScarfLogMessage message = ReadMessageFromReader(reader);
                messageList.Add(message);
            }
        }
        
        private ScarfLogMessage ReadMessageFromReader(SqlDataReader reader)
        {
            string messageClassString = reader.GetString(reader.GetOrdinal("Class"));
            string messageAsJson = reader.GetString(reader.GetOrdinal("LogMessageAsJson"));

            var messageClass = (MessageClass)Enum.Parse(typeof(MessageClass), messageClassString);
            
            var message = (ScarfLogMessage) JsonConvert.DeserializeObject(messageAsJson, ScarfLogMessage.GetMessageClassClrType(messageClass));
            
            return message;
        }

        public ScarfLogMessage GetMessageById(Guid messageId)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            using (SqlCommand command = CreateSingleQueryCommand(connection, messageId))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ScarfLogMessage message = ReadMessageFromReader(reader);
                        return message;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        private SqlCommand CreateMutipleQueryCommand(SqlConnection connection, string application, int pageIndex, int pageSize)
        {
            SqlCommand queryCommand = connection.CreateCommand();

            queryCommand.CommandText =
                @"SET NOCOUNT ON;
SELECT @totalMessages = COUNT(1)
                  FROM scarf_log
                  WHERE ApplicationName = @applicationName
;
WITH scarflog AS
(
    SELECT  *,
            ROW_NUMBER() OVER 
                     (ORDER BY LoggedAtUtc DESC, Sequence DESC) AS RN
    FROM    scarf_Log             
	WHERE   ApplicationName = @applicationName
)
SELECT  *
FROM scarflog
WHERE RN BETWEEN @StartRowIndex AND @EndRowIndex

";
            //http://stackoverflow.com/questions/5620758/t-sql-skip-take-stored-procedure

            int startIndex = pageIndex*pageSize;
            int endIndex = startIndex + pageSize;

            AddParameter(queryCommand, "@applicationName", application);
            AddOutParameter<int>(queryCommand, "@totalMessages");
            AddParameter<int>(queryCommand, "@StartRowIndex", startIndex);
            AddParameter<int>(queryCommand, "@EndRowIndex", endIndex);

            return queryCommand;
        }

        private SqlCommand CreateSingleQueryCommand(SqlConnection connection, Guid entryId)
        {
            SqlCommand queryCommand = connection.CreateCommand();

            queryCommand.CommandText =
                @"SET NOCOUNT ON;
                SELECT  *
    FROM    scarf_Log             
	WHERE   Id = @entryId";


            AddParameter(queryCommand, "@entryId", entryId);

            return queryCommand;
            
        }

        private SqlCommand CreateInsertCommand(SqlConnection connection, ScarfLogMessage message)
        {
            SqlCommand insertCommand = connection.CreateCommand();

            insertCommand.CommandText =
                @"INSERT INTO scarf_Log 
                    ( Id, LoggedAtUtc, ApplicationName, Computer, ResourceUri, [User], Class, Type, Message, LogMessageAsJson ) 
                  VALUES 
                    ( @Id, @LoggedAtUtc, @ApplicationName, @Computer, @ResourceUri, @User, @Class, @Type, @Message, @LogMessageAsJson )";
            insertCommand.CommandType = System.Data.CommandType.Text;

            AddParameter(insertCommand, "@Id", message.EntryId);
            AddParameter(insertCommand, "@LoggedAtUtc", message.LoggedAt);
            AddParameter(insertCommand, "@ApplicationName", Truncate(message.Application,100));
            AddParameter(insertCommand, "@Computer", Truncate(message.Computer,100));
            AddParameter(insertCommand, "@ResourceUri", Truncate(message.ResourceUri, 150));
            AddParameter(insertCommand, "@User", Truncate(message.User, 150));
            AddParameter(insertCommand, "@Class", message.MessageClass.ToString());
            AddParameter(insertCommand, "@Type", Truncate(message.MessageType,50));
            AddParameter(insertCommand, "@Message", message.Message);

            string messageJson = JsonConvert.SerializeObject(message);
            AddParameter(insertCommand, "@LogMessageAsJson", messageJson);

            return insertCommand;
        }

        private static string Truncate(string original, int maxLength)
        {
            if (original.Length <= maxLength) return original;

            return original.Substring(0, maxLength);
        }


        private static void AddParameter<T>(SqlCommand sqlCommand, string parameterName, T parameterValue)
        {
            SqlParameter parameterId = sqlCommand.CreateParameter();
            parameterId.ParameterName = parameterName;
            parameterId.DbType = GetSqlType(typeof(T));
            parameterId.Value = parameterValue;
            sqlCommand.Parameters.Add(parameterId);
        }

        private void AddOutParameter<T>(SqlCommand sqlCommand, string parameterName)
        {
            SqlParameter parameterId = sqlCommand.CreateParameter();
            parameterId.ParameterName = parameterName;
            parameterId.DbType = GetSqlType(typeof(T));
            parameterId.Direction = ParameterDirection.Output;
            sqlCommand.Parameters.Add(parameterId);
        }

        private static DbType GetSqlType(Type type)
        {
            if (type == typeof(Guid))
                return DbType.Guid;
            else if (type == typeof(DateTime))
                return DbType.DateTime;
            else if (type == typeof(string))
                return DbType.String;
            else if (type == typeof(Int32))
                return DbType.Int32;
            else if (type == typeof(Int64))
                return DbType.Int64;
            else if (type == typeof(Int16))
                return DbType.Int16;
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
