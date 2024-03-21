CREATE TABLE [dbo].[Classification] (
    [ClassificationId]          INT                IDENTITY (1, 1) NOT NULL,
    [ClassificationCode]        NVARCHAR (10)      NOT NULL,
    [Description]               NVARCHAR (255)     NULL,
    [Notes]                     NVARCHAR (255)     NULL,
    [CreateDateTime]            DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]          DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]        NVARCHAR (256)     NOT NULL,
    CONSTRAINT [PK_Classification] PRIMARY KEY CLUSTERED ([ClassificationId] ASC)
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ClassificationCode]
    ON [dbo].[Classification]([ClassificationCode] ASC);
