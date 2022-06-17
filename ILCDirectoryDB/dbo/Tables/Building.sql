CREATE TABLE [dbo].[Building] (
    [BuildingId]        INT            IDENTITY (1, 1) NOT NULL,
    [BuildingCode]      NVARCHAR (255) NOT NULL,
    [BuildingLongDesc]  NVARCHAR (255) NOT NULL,
    [BuildingShortDesc] NVARCHAR (255) NOT NULL,
    [AuditTrail]        NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Buildings] PRIMARY KEY CLUSTERED ([BuildingId] ASC)
);

