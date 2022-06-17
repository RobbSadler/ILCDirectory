CREATE TABLE [dbo].[Classification] (
    [ClassificationId]  INT            IDENTITY (1, 1) NOT NULL,
    [StatusCode]        NVARCHAR (255) NOT NULL,
    [StatusDescription] NVARCHAR (255) NOT NULL,
    [StatusExplanation] NVARCHAR (255) NOT NULL,
    [AuditTrail]        NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Classification] PRIMARY KEY CLUSTERED ([ClassificationId] ASC)
);

