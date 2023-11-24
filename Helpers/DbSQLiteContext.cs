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
                var sql = @"
                    CREATE TABLE IF NOT EXISTS
                    `log_event` (
                        `id_log_event` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        `code_event` TEXT,
                        `dt_start` TEXT,
                        `dt_finish` TEXT,
                        `request_data` TEXT,
                        `response_data` TEXT,
                        `was_error` INTEGER
                    );
                ";

                await connection.ExecuteAsync(sql);
            }

            async Task _initLogError()
            {
                var sql = @"
                    CREATE TABLE IF NOT EXISTS
                    `log_error` (
                        `id_log_error` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        `code_event` TEXT,
                        `dt_register` TEXT,
                        `method` TEXT,
                        `exception_message` TEXT,
                        `stack_trace` TEXT
                    );
                ";
                await connection.ExecuteAsync(sql);
            }
        }
    }
}