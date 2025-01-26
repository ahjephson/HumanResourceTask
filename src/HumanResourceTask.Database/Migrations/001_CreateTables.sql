CREATE TABLE [dbo].[Department]
(
    [Id]               UNIQUEIDENTIFIER      NOT NULL    PRIMARY KEY,
    [Name]             NVARCHAR(100)         NOT NULL    UNIQUE,
    [CreatedAtUtc]     DATETIME2             NOT NULL    DEFAULT GETUTCDATE(),
    [UpdatedAtUtc]     DATETIME2             NULL,

    [ValidFrom]        DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL,
    [ValidTo]          DATETIME2 GENERATED ALWAYS AS ROW END   NOT NULL,
    PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])

) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[DepartmentHistory]))

CREATE TABLE [dbo].[Status]
(
    [Id]               UNIQUEIDENTIFIER      NOT NULL    PRIMARY KEY,
    [Name]             NVARCHAR(100)         NOT NULL    UNIQUE,
    [CreatedAtUtc]     DATETIME2             NOT NULL    DEFAULT GETUTCDATE(),
    [UpdatedAtUtc]     DATETIME2             NULL,

    [ValidFrom]        DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL,
    [ValidTo]          DATETIME2 GENERATED ALWAYS AS ROW END   NOT NULL,
    PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])

) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[StatusHistory]))

CREATE TABLE [dbo].[EmployeeRecord]
(
    [Id]               UNIQUEIDENTIFIER      NOT NULL    PRIMARY KEY,
    [FirstName]        NVARCHAR(100)         NOT NULL,
    [LastName]         NVARCHAR(100)         NOT NULL,
    [Email]            NVARCHAR(320)         NOT NULL,
    [DateOfBirth]      DATE                  NOT NULL,
    [DepartmentId]     UNIQUEIDENTIFIER      NOT NULL,
    [StatusId]         UNIQUEIDENTIFIER      NOT NULL,
    [EmployeeNumber]   BIGINT                NOT NULL,
    [CreatedAtUtc]     DATETIME2             NOT NULL    DEFAULT GETUTCDATE(),
    [UpdatedAtUtc]     DATETIME2             NULL,
    [Deleted]          BIT                   NOT NULL    DEFAULT 0,
    [ValidFrom]        DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL,
    [ValidTo]          DATETIME2 GENERATED ALWAYS AS ROW END   NOT NULL,
    PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo]),
    CONSTRAINT [FK_Employee_Department]  FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Department]([Id]),
    CONSTRAINT [FK_Employee_Status]      FOREIGN KEY ([StatusId])     REFERENCES [dbo].[Status]([Id]),
) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[EmployeeRecordHistory]))

CREATE UNIQUE INDEX [UQ_EmployeeRecord_Email]
ON [dbo].[EmployeeRecord] ([Email])

CREATE UNIQUE INDEX [UQ_EmployeeRecord_EmployeeNumber_Accepted]
ON [dbo].[EmployeeRecord] ([EmployeeNumber])
WHERE [StatusId] = 'e9f51604-41de-4582-a5ba-627a27f54adc'

CREATE INDEX IX_EmployeeRecord_StatusId
ON [dbo].[EmployeeRecord] ([StatusId]);

CREATE INDEX IX_EmployeeRecord_DepartmentId
ON [dbo].[EmployeeRecord] ([DepartmentId]);

CREATE INDEX IX_EmployeeRecord_StatusId_DepartmentId
ON [dbo].[EmployeeRecord] ([StatusId], [DepartmentId]);

CREATE INDEX IX_EmployeeRecord_DateOfBirth
ON [dbo].[EmployeeRecord] ([DateOfBirth]);

CREATE INDEX IX_EmployeeRecord_CreatedAtUtc
ON [dbo].[EmployeeRecord] ([CreatedAtUtc]);
