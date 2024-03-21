CREATE TABLE [dbo].[Email](
    [EmailId]               INT        IDENTITY (1, 1) NOT NULL,
    [PersonId]              INT        NOT NULL,
    [EmailAddress]          NVARCHAR(200) NOT NULL,
    [EmailAddressType]      NCHAR (6) NOT NULL,
    [IncludeInDirectory]    BIT DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [CreateDateTime]        DATETIMEOFFSET (7) CONSTRAINT [DF_Email_CreateDateTime] DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]      DATETIMEOFFSET (7) CONSTRAINT [DF_Email_ModifiedDateTime] DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]    NVARCHAR (256)     CONSTRAINT [DF_Email_ModifiedByUserName] DEFAULT ('system') NOT NULL,
    [DDDId]              INT                NULL,

    CONSTRAINT [PK_Email] PRIMARY KEY CLUSTERED ([EmailId] ASC)
);
