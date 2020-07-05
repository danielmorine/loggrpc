using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using regGRPC.Queries.Sql;
using Scaffolds;

namespace regGRPC
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly string _connectionString;

        public GreeterService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public override async Task<GetAllRegistrationProcessResponse> FilterRegistrationProcess(FilterRegistrationProcessRequest request, ServerCallContext context)
        {
            var registrationProcess = new GetAllRegistrationProcessResponse();
            using (var sql = new SqlConnection(_connectionString))
            {
                using var command = new SqlCommand(RegistrationProcessRequestSQL.QueryFilterRegistrationProcess(), sql);
                await sql.OpenAsync();

                command.Parameters.AddWithValue("@EnvFilterValue", request.EnvFilter);
                command.Parameters.AddWithValue("@LevelFilterValue", request.LevelFilter);
                command.Parameters.AddWithValue("@OrderByValue", request.OrderBy);
                command.Parameters.AddWithValue("@SortDirectionValue", request.SortDirection);
                command.Parameters.AddWithValue("@SearchTypeValue", request.SearchType);
                command.Parameters.AddWithValue("@SearchValueQ", request.SearchValue);

                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    registrationProcess.List.Add(new GetAllRegistrationProcessObject
                    {
                        LevelTypeName = reader.GetString(0),
                        EnvironmentTypeName = reader.GetString(1),
                        Events = reader.GetInt32(2),
                        ReportDescription = reader.GetString(3),
                        ReportSource = reader.GetString(4),
                        RegistrationProcessID = reader.GetGuid(5).ToString(),
                        CreatedDate = reader.GetDateTimeOffset(6).ToString()
                    });
                }

                reader.Close();
                sql.Close();
            }
            return registrationProcess;
        }
        public override async Task<DefaultResponse> Archive(ArchiveRequest request, ServerCallContext context)
        {
            using var sql = new SqlConnection(_connectionString);
            await sql.OpenAsync();
            using var transaction = sql.BeginTransaction();
            SqlCommand command = sql.CreateCommand();
            command.Transaction = transaction;

            for (var i =  0; i < request.List.ToArray().Length; i++)
            {
                command = await ArchiveRegistrationProcessAsync(command, request.List.ToArray()[i].Id, i);
            }

            try
            {
                await transaction.CommitAsync();
                sql.Close();
                return new DefaultResponse { Status = true };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                sql.Close();
                return new DefaultResponse { Status = false };
            }
        }
        public override async Task<DefaultResponse> Delete(DeleteRequest request, ServerCallContext context)
        {
            using var sql = new SqlConnection(_connectionString);
            await sql.OpenAsync();
            using var transaction = sql.BeginTransaction();
            SqlCommand command = sql.CreateCommand();
            command.Transaction = transaction;

            for (var i = 0; i < request.List.ToArray().Length; i++)
            {
                command = await DeleteRegistrationProcessAsync(command, request.List.ToArray()[i].Id, i);
            }

            try
            {
                await transaction.CommitAsync();
                sql.Close();
                return new DefaultResponse { Status = true };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                sql.Close();
                return new DefaultResponse { Status = false };
            }
        }
        public override async Task<DefaultResponse> ValidateRegistrationProcessId(ValidateRegistrationProcessIdRequest request, ServerCallContext context)
        {
            using (var sql = new SqlConnection(_connectionString))
            {
                var query = "SELECT COUNT([RegistrationProcessID]) FROM [dbo].[RegistrationProcess] WHERE [RegistrationProcessID] = @RegistrationProcessID";
                using (var command = new SqlCommand(query, sql))
                {
                    await sql.OpenAsync();
                    command.Parameters.AddWithValue("@RegistrationProcessID", request.RegistrationProcessID);

                    var result = (int) await command.ExecuteScalarAsync();
                    sql.Close();

                    if (result == 1)
                    {
                        return new DefaultResponse { Status = true };
                    }

                    return new DefaultResponse { Status = false };
                }
            }
        }
        public override async Task<GetByIdRegistrationProcessResponse> GetByIdRegistrationProcess(GetByIdRegistrationProcessRequest request, ServerCallContext context)
        {
            var result = new GetByIdRegistrationProcessResponse();
            using (var sql = new SqlConnection(_connectionString))
            {
                var query = @"
                        SELECT [RP].[RegistrationProcessID]
                              ,[RP].[CreatedDate]
                              ,[RP].[OwnerID]
	                          ,[LT].[Name] AS [LevelTypeName]
	                          ,[EV].[Name] AS [EnvironmentTypeName]
	                          ,[RE].[Title] 
	                          ,[RE].[ReportSource]
	                          ,[RE].[Events]
                              ,[RE].[Details]
                          FROM [dbo].[RegistrationProcess] AS [RP]
                          INNER JOIN [dbo].[Report] AS [RE] ON [RP].[ReportID] = [RE].[ReportID]
                          INNER JOIN [dbo].[EnvironmentType] AS [EV] ON [RP].[EnvironmentTypeID] = [EV].[EnvironmentTypeID]
                          INNER JOIN [dbo].[LevelType] AS [LT] ON [RE].[LevelTypeID] = [LT].[LevelTypeID]
                          WHERE [RP].[RegistrationProcessID] = @RegistrationProcessID";

                using (var command = new SqlCommand(query, sql))
                {
                    await sql.OpenAsync();

                    command.Parameters.AddWithValue("@RegistrationProcessID", request.RegistrationProcessID);

                    var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        result.RegistrationProcessID = reader.GetGuid(0).ToString();
                        result.CreatedDate = reader.GetDateTimeOffset(1).ToString();
                        result.OwnerID = reader.GetGuid(2).ToString();
                        result.LevelTypeName = reader.GetString(3);
                        result.EnvironmentTypeName = reader.GetString(4);
                        result.Title = reader.GetString(5);
                        result.ReportSource = reader.GetString(6);
                        result.Events = reader.GetInt32(7);
                        result.Details = reader.GetString(8);
                    }

                    reader.Close();
                    sql.Close();
                }
            }
            return result;
        }
        public override async Task<GetAllRegistrationProcessResponse> SendGetAllRegistrationProcess(GetAllRegistrationProcessRequest request, ServerCallContext context)
        {
            var registrationProcess = new GetAllRegistrationProcessResponse();

            using (var sql = new SqlConnection(_connectionString))
            {
                var query = @"
                    SELECT 
                    [LT].[Name] AS [LevelTypeName],
                    [EV].[Name] AS [EnvironmentTypeName],
                    [RE].[Events],
                    [RE].[ReportDescription],
                    [RE].[ReportSource],
                    [RP].[RegistrationProcessID],
                    [RP].[CreatedDate]
                    FROM [dbo].[RegistrationProcess] AS [RP]
                    INNER JOIN [dbo].[Report] AS [RE] ON [RP].[ReportID] = [RE].[ReportID]
                    INNER JOIN [dbo].[LevelType] AS [LT] ON [RE].[LevelTypeID] = [LT].[LevelTypeID]
                    INNER JOIN [dbo].[EnvironmentType] AS [EV] ON [RP].[EnvironmentTypeID] = [EV].[EnvironmentTypeID]
                    WHERE [RP].[IsActive] = 1
                    ORDER BY [RP].[CreatedDate] DESC";

                using(var command = new SqlCommand(query, sql))
                {
                    await sql.OpenAsync();

                    var reader = await command.ExecuteReaderAsync();
                    
                    while (await reader.ReadAsync())
                    {
                        registrationProcess.List.Add(new GetAllRegistrationProcessObject 
                        { 
                            LevelTypeName = reader.GetString(0),
                            EnvironmentTypeName = reader.GetString(1),
                            Events = reader.GetInt32(2),
                            ReportDescription = reader.GetString(3),
                            ReportSource = reader.GetString(4),
                            RegistrationProcessID = reader.GetGuid(5).ToString(),
                            CreatedDate = reader.GetDateTimeOffset(6).ToString()
                        });
                    }
                    reader.Close();
                    sql.Close();
                }
            }

            return registrationProcess;
        }
        public override async Task<DefaultResponse> SendValidateEnvironmentTypeRequest(ValidateEnvironmentTypeRequest model, ServerCallContext context)
        {
            try
            {
                using (var sql = new SqlConnection(_connectionString))
                {
                    var query = "SELECT COUNT([EnvironmentTypeID]) AS [EnvironmentTypeIDExists] FROM [dbo].[EnvironmentType] WHERE [EnvironmentTypeID] = @EnvironmentTypeID";
                    using var command = new SqlCommand(query, sql);
                    await sql.OpenAsync();
                    command.Parameters.AddWithValue("@EnvironmentTypeID", (byte) model.EnvironmentTypeID);

                    var result = (int) await command.ExecuteScalarAsync();

                    if (result == 1)
                    {
                        sql.Close();
                        return new DefaultResponse { Status = true };
                    } else
                    {
                        return new DefaultResponse { Status = false };
                    }
                }
            }
            catch (Exception)
            {
                return new DefaultResponse { Status = false };
            }                      
        }
        public override async Task<DefaultResponse> SendValidateLevelTypeRequest(ValidateLevelTypeRequest model, ServerCallContext context)
        {
            try
            {
                using (var sql = new SqlConnection(_connectionString))
                {
                    var query = "SELECT COUNT([LevelTypeID]) AS [LevelTypeExists] FROM [dbo].[LevelType] WHERE [LevelTypeID] = @LevelTypeID";
                    using var command = new SqlCommand(query, sql);
                    await sql.OpenAsync();
                    command.Parameters.AddWithValue("@LevelTypeID", model.LevelTypeID);

                    var result = (int)await command.ExecuteScalarAsync();

                    if (result == 1)
                    {
                        sql.Close();
                        return new DefaultResponse { Status = true };
                    }
                    else
                    {
                        return new DefaultResponse { Status = false };
                    }
                }
            }
            catch (Exception)
            {
                return new DefaultResponse { Status = false };
            }           
        }
        public override async Task<DefaultResponse> SendRegistrationProcess(RegistrationProcessRequest request, ServerCallContext context)
        {
            try
            {
                var result = await AddAsync(request);
                return new DefaultResponse { Status = result };
            }
            catch (Exception)
            {
                return new DefaultResponse { Status = false };
            }                                       
        }
        private async Task<bool> AddAsync(RegistrationProcessRequest request)
        {            
            using var sql = new SqlConnection(_connectionString);
            await sql.OpenAsync();
            using var transaction = sql.BeginTransaction();
            SqlCommand command = sql.CreateCommand();
            command.Transaction = transaction;

            var report = new Report
            {
                Events = request.Events,
                ID = Guid.NewGuid(),
                LevelTypeID = (byte)request.LevelTypeID,
                ReportDescription = request.ReportDescription,
                ReportSource = request.ReportSource,
                Title = request.Title
            };

            var registrationProcess = new RegistrationProcess
            {
                ReportID = report.ID,
                CreatedDate = DateTimeOffset.UtcNow,
                ID = Guid.NewGuid(),
                EnvironmentTypeID = (byte)request.EnvironmentTypeID,
                IsActive = true,
                OwnerID = Guid.Parse(request.OwnerID)
            };

            command = await AddReportAsync(command, report);
            command = await AddRegistrationProcessAsync(command, registrationProcess);

            try
            {
                await transaction.CommitAsync();
                sql.Close();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                sql.Close();
                return false;
            }
        }
        private async Task<SqlCommand> AddReportAsync(SqlCommand command, Report model)
        {
            command.CommandText = RegistrationProcessRequestSQL.QueryAddReport();
            command.Parameters.AddWithValue("@ReportID", model.ID);
            command.Parameters.AddWithValue("@Title", model.Title);
            command.Parameters.AddWithValue("@ReportDescription", model.ReportDescription);
            command.Parameters.AddWithValue("@ReportSource", model.ReportSource);
            command.Parameters.AddWithValue("@LevelTypeID", model.LevelTypeID);
            command.Parameters.AddWithValue("@Events", model.Events);

            await command.ExecuteNonQueryAsync();

            return command;
        }
        private async Task<SqlCommand> AddRegistrationProcessAsync(SqlCommand command, RegistrationProcess model)
        {
            command.CommandText = RegistrationProcessRequestSQL.QueryAddRegistrationProcess();

            command.Parameters.AddWithValue("@RegistrationProcessID", model.ID);
            command.Parameters.AddWithValue("@CreatedDate", model.CreatedDate);
            command.Parameters.AddWithValue("@IsActive", model.IsActive);
            command.Parameters.AddWithValue("@ReportIDRegistrationProcess", model.ReportID);
            command.Parameters.AddWithValue("@OwnerID", model.OwnerID);
            command.Parameters.AddWithValue("@EnvironmentTypeID", model.EnvironmentTypeID);

            await command.ExecuteNonQueryAsync();

            return command;
        }
        private async Task<SqlCommand> ArchiveRegistrationProcessAsync(SqlCommand command, string registrationProcessID, int position)
        {
            command.CommandText = string
                .Format(@"UPDATE [dbo].[RegistrationProcess] SET [IsActive] = 0 WHERE [RegistrationProcessID] = @RegistrationProcessID{0}", position);

            command.Parameters.AddWithValue(string.Format("@RegistrationProcessID{0}", position), registrationProcessID);

            await command.ExecuteNonQueryAsync();

            return command;
        }
        private async Task<SqlCommand> DeleteRegistrationProcessAsync(SqlCommand command, string registrationProcessID, int position)
        {
            var reportID = await GetReportIDAsync(registrationProcessID);

            command.CommandText = string
                .Format(@"DELETE [dbo].[Report] WHERE [ReportID] = @ReportID{0}", position);

            command.Parameters.AddWithValue(string.Format("@reportID{0}", position), reportID);

            await command.ExecuteNonQueryAsync();

            command.CommandText = string
              .Format(@"DELETE [dbo].[RegistrationProcess] WHERE [RegistrationProcessID] = @RegistrationProcessID{0}", position);

            command.Parameters.AddWithValue(string.Format("@RegistrationProcessID{0}", position), registrationProcessID);

            await command.ExecuteNonQueryAsync();

            return command;
        }
        private async Task<Guid> GetReportIDAsync(string registrationProcessID)
        {
            using var sql = new SqlConnection(_connectionString);
            using var command = new SqlCommand("SELECT [ReportID] FROM [LogGRPC].[dbo].[RegistrationProcess] WHERE [RegistrationProcessID] = @RegistrationProcessID", sql);
            await sql.OpenAsync();
            command.Parameters.AddWithValue("@RegistrationProcessID", registrationProcessID);

            var reportID = (Guid)await command.ExecuteScalarAsync();
            sql.Close();

            return reportID;
        }
    }
}
