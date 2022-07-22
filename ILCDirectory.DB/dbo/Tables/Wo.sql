CREATE TABLE [dbo].[Wo] (
    [WoId]               INT                IDENTITY (1, 1) NOT NULL,
    [WoCode]             NVARCHAR (255)     NOT NULL,
    [WoEntity]           NVARCHAR (255)     NOT NULL,
    [CreateDateTime]     DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]   DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName] NVARCHAR (256)     DEFAULT ('system') NOT NULL,
    [DDDId]              INT                NULL,
    CONSTRAINT [PK_Wos] PRIMARY KEY CLUSTERED ([WoId] ASC)
);



