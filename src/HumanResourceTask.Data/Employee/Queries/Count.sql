SELECT
    COUNT([Id])
FROM
    [dbo].[EmployeeRecordView] e
WHERE 
    (@StatusId IS NULL OR e.[StatusId] = @StatusId)
    AND (@DepartmentId IS NULL OR e.[DepartmentId] = @DepartmentId)
