INSERT INTO [dbo].[Status]
(
    [Id],
    [Name],
    [CreatedAtUtc],
    [UpdatedAtUtc]
)
VALUES
    ('e9f51604-41de-4582-a5ba-627a27f54adc', 'Approved', GETUTCDATE(), NULL),
    ('f3d53c1c-3dcd-45c0-b038-96187bea1744', 'Pending', GETUTCDATE(), NULL),
    ('9e123595-1b11-4abf-9736-5c33adf9a0a3', 'Disabled', GETUTCDATE(), NULL)
