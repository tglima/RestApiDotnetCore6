using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace WebApi.Helpers
{
    public class DbSQLiteContext
    {

        public IDbConnection CreateConnection()
        {
            return new SqliteConnection("Data Source=Data/LogDatabase.db");
        }

        public async Task Init()
        {
            using var connection = CreateConnection();
            await _initLogEvent();
            await _initLogError();

            async Task _initLogEvent()
            {
                var createTableLogEvent = @"
                    CREATE TABLE IF NOT EXISTS
                    log_event (
                        id_log_event INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        code_event TEXT,
                        api_key TEXT,
                        dt_start TEXT,
                        dt_finish TEXT,
                        request_data TEXT,
                        response_data TEXT,
                        code_status INTEGER
                    );
                ";
                var createIndexIdLogEvent = @"
                    CREATE INDEX IF NOT EXISTS
                        idx_log_event ON log_event
                    (id_log_event);
                ";
                var createIndexCodeEvent = @"
                    CREATE INDEX IF NOT EXISTS
                        idx_code_event ON log_event
                    (code_event);
                ";
                await connection.ExecuteAsync(createTableLogEvent);
                await connection.ExecuteAsync(createIndexIdLogEvent);
                await connection.ExecuteAsync(createIndexCodeEvent);
            }

            async Task _initLogError()
            {
                var createTableLogError = @"
                    CREATE TABLE IF NOT EXISTS
                    log_error (
                        id_log_error INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        code_event TEXT,
                        dt_register TEXT,
                        method TEXT,
                        exception_message TEXT,
                        stack_trace TEXT);
                ";

                var createIndexIdLogError = @"
                    CREATE INDEX IF NOT EXISTS
                        idx_log_error ON log_error
                    (id_log_error);
                ";
                var createIndexCodeEvent = @"
                    CREATE INDEX IF NOT EXISTS
                        idx_code_event_error ON log_error
                    (code_event);
                ";

                await connection.ExecuteAsync(createTableLogError);
                await connection.ExecuteAsync(createIndexIdLogError);
                await connection.ExecuteAsync(createIndexCodeEvent);
            }
        }
    }
}