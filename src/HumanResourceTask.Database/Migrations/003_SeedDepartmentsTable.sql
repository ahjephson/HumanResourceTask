INSERT INTO [dbo].[Department]
(
    [Id],
    [Name],
    [CreatedAtUtc],
    [UpdatedAtUtc]
)
VALUES
    ('14ece09e-8460-421a-8a67-3e84d4ef2b82', 'HR', GETUTCDATE(), NULL),
    ('fbffb64b-b07e-433a-86e4-ae722b770300', 'Finance', GETUTCDATE(), NULL),
    ('7e01906c-426b-42a7-96ee-cb59f6df5f13', 'Engineering', GETUTCDATE(), NULL),
    ('6a0546a5-171c-4f0c-bbfb-fab0a33be98c', 'Marketing', GETUTCDATE(), NULL)
