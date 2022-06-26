CREATE TABLE [dbo].[Person] (
    [PersonId]                INT                IDENTITY (1, 1) NOT NULL,
    [Birthdate]               DATETIME2 (7)      NULL,
    [Comment]                 NVARCHAR (255)     NULL,
    [FirstName]               NVARCHAR (25)      NOT NULL,
    [Gender]                  NVARCHAR (1)       NOT NULL,
    [LastName]                NVARCHAR (25)      NOT NULL,
    [MaidenName]              NVARCHAR (25)      NULL,
    [MaritalStatus]           NVARCHAR (1)       NOT NULL,
    [MiddleName]              NVARCHAR (25)      NULL,
    [NickName]                NVARCHAR (25)      NULL,
    [SpouseId]                INT                NULL,
    [Suffix]                  NVARCHAR (50)      NULL,
    [Title]                   NVARCHAR (50)      NULL,
    [DeleteFlag]              BIT                NULL,
    [ClassificationCode]      NVARCHAR (10)      NOT NULL,
    [LanguagesSpoken]         NVARCHAR (255)     NULL,
    [AuditTrail]              NVARCHAR (MAX)     NULL,
    [DirectoryCorrectionForm] DATETIME           NULL,
    [DirCorrFormNote]         NVARCHAR (120)     NULL,
    [CreateDateTime]          DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]        DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]      NVARCHAR (256)     DEFAULT ('system') NOT NULL,
    CONSTRAINT [PK_People] PRIMARY KEY CLUSTERED ([PersonId] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_Person_LastName]
    ON [dbo].[Person]([LastName] ASC);

