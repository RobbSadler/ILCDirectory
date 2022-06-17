CREATE TABLE [dbo].[Wos] (
    [WoId]     INT            IDENTITY (1, 1) NOT NULL,
    [WoCode]   NVARCHAR (255) NOT NULL,
    [WoEntity] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Wos] PRIMARY KEY CLUSTERED ([WoId] ASC)
);

