SELECT
    *
FROM
    [dbo].[EmployeeRecordView] e
WHERE 
    (@StatusId IS NULL OR e.[StatusId] = @StatusId)
    AND (@DepartmentId IS NULL OR e.[DepartmentId] = @DepartmentId)
ORDER BY
    CASE WHEN @OrderBy = 'Id ASC' THEN e.[Id] END ASC,
    CASE WHEN @OrderBy = 'Id DESC' THEN e.[Id] END DESC,
    CASE WHEN @OrderBy = 'FirstName ASC' THEN e.[FirstName] END ASC,
    CASE WHEN @OrderBy = 'FirstName DESC' THEN e.[FirstName] END DESC,
    CASE WHEN @OrderBy = 'LastName ASC' THEN e.[LastName] END ASC,
    CASE WHEN @OrderBy = 'LastName DESC' THEN e.[LastName] END DESC,
    CASE WHEN @OrderBy = 'Email ASC' THEN e.[Email] END ASC,
    CASE WHEN @OrderBy = 'Email DESC' THEN e.[Email] END DESC,
    CASE WHEN @OrderBy = 'EmployeeNumber ASC' THEN e.[EmployeeNumber] END ASC,
    CASE WHEN @OrderBy = 'EmployeeNumber DESC' THEN e.[EmployeeNumber] END DESC,
    CASE WHEN @OrderBy = 'DateOfBirth ASC' THEN e.[DateOfBirth] END ASC,
    CASE WHEN @OrderBy = 'DateOfBirth DESC' THEN e.[DateOfBirth] END DESC,
    CASE WHEN @OrderBy = 'DepartmentName ASC' THEN e.[DepartmentName] END ASC,
    CASE WHEN @OrderBy = 'DepartmentName DESC' THEN e.[DepartmentName] END DESC,
    CASE WHEN @OrderBy = 'StatusName ASC' THEN e.[StatusName] END ASC,
    CASE WHEN @OrderBy = 'StatusName DESC' THEN e.[StatusName] END DESC
OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY
