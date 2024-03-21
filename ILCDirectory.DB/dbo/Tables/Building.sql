CREATE TABLE [dbo].[Building] (
    [BuildingId]         INT                IDENTITY (1, 1) NOT NULL,
    [BuildingCode]       NVARCHAR (255)     NOT NULL,
    [BuildingLongDesc]   NVARCHAR (255)     NULL,
    [BuildingShortDesc]  NVARCHAR (255)     NULL,
    [Notes]              NVARCHAR (255)     NULL,
    [CreateDateTime]     DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]   DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName] NVARCHAR (256)     DEFAULT ('system') NOT NULL,
    CONSTRAINT [PK_Buildings] PRIMARY KEY CLUSTERED ([BuildingId] ASC)
);



