namespace regGRPC.Queries.Sql
{
    public static class RegistrationProcessRequestSQL
    {
        public static string QueryAddReport()
        {
            return @"
                    INSERT INTO [dbo].[Report]
                    (
	                       [ReportID]
                          ,[Title]
                          ,[ReportDescription]
                          ,[ReportSource]
                          ,[LevelTypeID]
                          ,[Events]
                          ,[Details]
                    ) VALUES (
	                       @ReportID
                          ,@Title
                          ,@ReportDescription
                          ,@ReportSource
                          ,@LevelTypeID
                          ,@Events
                          ,@Details
                    )";
        }

        public static string QueryAddRegistrationProcess()
        {
            return @"
                INSERT INTO [dbo].[RegistrationProcess] 
                  (
                     [RegistrationProcessID]
                      ,[CreatedDate]
                      ,[IsActive]
                      ,[ReportID]
                      ,[OwnerID]
                      ,[EnvironmentTypeID]
                  ) VALUES (
                       @RegistrationProcessID
                      ,@CreatedDate
                      ,@IsActive
                      ,@ReportIDRegistrationProcess
                      ,@OwnerID
                      ,@EnvironmentTypeID
                  )";
        }

        public static string QueryFilterRegistrationProcess()
        {
            return @"
                    DECLARE @EnvFilter NVARCHAR(2);
                    DECLARE @HasEnv BIT;
                    SET @EnvFilter = @EnvFilterValue;
                    SET @HasEnv = CASE WHEN @EnvFilter IS  NULL OR TRIM(@EnvFilter) = '' THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END;

                    DECLARE @LevelFilter NVARCHAR(2);
                    DECLARE @HasLevel BIT;
                    SET @LevelFilter = @LevelFilterValue;
                    SET @HasLevel = CASE WHEN @LevelFilter IS NULL OR TRIM(@LevelFilter) = '' THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END;

                    DECLARE @OrderBy NVARCHAR(100);
                    SET @OrderBy = @OrderByValue

                    DECLARE @SortDirection NVARCHAR(5);
                    SET @SortDirection = @SortDirectionValue

                    DECLARE @SearchType NVARCHAR(20);
                    DECLARE @SearchValue NVARCHAR(50);
                    DECLARE @HasSearch BIT;

                    SET @SearchType = @SearchTypeValue
                    SET @SearchValue = @SearchValueQ

                    SET @HasSearch = CASE WHEN @SearchType IS NULL OR TRIM(@SearchType) = '' THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END;

                    SELECT 
					    [LT].[Name] AS [LevelTypeName],
                        [EV].[Name] AS [EnvironmentTypeName],
                        [RE].[Events],
                        [RE].[ReportDescription],
                        [RE].[ReportSource],
                        [RP].[RegistrationProcessID],
                        [RP].[CreatedDate]
                    FROM [LogGRPC].[dbo].[Report] AS RE
                    INNER JOIN [dbo].[RegistrationProcess] AS RP ON RE.ReportID = RP.ReportID
                    INNER JOIN [dbo].[EnvironmentType] AS [EV] ON [RP].[EnvironmentTypeID] = [EV].[EnvironmentTypeID]
                    INNER JOIN [dbo].[LevelType] AS [LT] ON [RE].[LevelTypeID] = [LT].[LevelTypeID]
                    WHERE [RP].[IsActive] = @IsActiveValue AND
                          (@HasEnv = 0 OR (RP.EnvironmentTypeID = @EnvFilter)) AND 
	                      (@HasLevel = 0 OR ([RE].[LevelTypeID] = @LevelFilter)) AND
	                      (@HasSearch = 0 OR (@SearchType = 'reportSource' AND [RE].[ReportSource] LIKE '%' + @SearchValueQ +'%') OR (RE.ReportDescription LIKE '%' + @SearchValueQ +'%'))
                    ORDER BY
                        CASE WHEN @OrderBy IS NULL OR TRIM(@OrderBy) = '' AND @SortDirection = 'asc' THEN [RP].CreatedDate END ASC,
	                    CASE WHEN @OrderBy IS NULL OR TRIM(@OrderBy) = '' THEN [RP].[CreatedDate] END DESC,                        
	                    CASE WHEN LOWER(@OrderBy) = 'level' AND LOWER(@SortDirection) = 'asc' THEN [RE].[LevelTypeID] END ASC,
	                    CASE WHEN LOWER(@OrderBy) = 'level' AND LOWER(@SortDirection) = 'desc' THEN [RE].[LevelTypeID] END DESC,
	                    CASE WHEN LOWER(@OrderBy) = 'events' AND LOWER(@SortDirection) = 'asc' THEN [RE].[Events] END ASC,
	                    CASE WHEN LOWER(@OrderBy) = 'events' AND LOWER(@SortDirection) = 'desc' THEN [RE].[Events] END DESC";
        }
    }
}
