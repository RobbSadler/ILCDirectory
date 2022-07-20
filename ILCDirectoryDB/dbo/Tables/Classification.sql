CREATE TABLE [dbo].[Classification] (
    [ClassificationId]          INT                IDENTITY (1, 1) NOT NULL,
    [Code]                      NVARCHAR (10)      NOT NULL,
    [Description]               NVARCHAR (255)     NULL,
    [AuditTrail]                NVARCHAR (255)     NULL,
    [CreateDateTime]            DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]          DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]        NVARCHAR (256)     NOT NULL,
    [DDDId]              INT                NULL,
    CONSTRAINT [PK_Classification] PRIMARY KEY CLUSTERED ([ClassificationId] ASC)
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ClassificationCode]
    ON [dbo].[Classification]([Code] ASC);
