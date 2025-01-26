CREATE VIEW [dbo].[EmployeeRecordView] AS
SELECT 
    e.[Id],
    e.[FirstName],
    e.[LastName],
    e.[Email],
    e.[DateOfBirth],
    e.[EmployeeNumber],
    e.[CreatedAtUtc],
    e.[UpdatedAtUtc],
    e.[Deleted],
    e.[DepartmentId],
    e.[StatusId],
    d.[Name] AS DepartmentName,
    s.[Name] AS StatusName
FROM [dbo].[EmployeeRecord] e
LEFT JOIN [dbo].[Department] d ON e.[DepartmentId] = d.[Id]
LEFT JOIN [dbo].[Status] s ON e.[StatusId] = s.[Id]
WHERE e.[Deleted] = 0
