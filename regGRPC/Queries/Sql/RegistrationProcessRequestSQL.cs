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
                    ) VALUES (
	                       @ReportID
                          ,@Title
                          ,@ReportDescription
                          ,@ReportSource
                          ,@LevelTypeID
                          ,@Events
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
    }
}
