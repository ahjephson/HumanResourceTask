UPDATE [dbo].[EmployeeRecord]
SET
    [FirstName]       = @FirstName,
    [LastName]        = @LastName,
    [Email]           = @Email,
    [DateOfBirth]     = @DateOfBirth,
    [DepartmentId]    = @DepartmentId,
    [StatusId]        = @StatusId,
    [EmployeeNumber]  = @EmployeeNumber,
    [UpdatedAtUtc]    = COALESCE(@UpdatedAtUtc, GETUTCDATE()),
    [Deleted]         = @Deleted
WHERE [Id] = @Id
