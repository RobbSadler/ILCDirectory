CREATE TABLE [dbo].[CityCode] (
    [CityCodeId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (255) NOT NULL,
    [Desc]       NVARCHAR (255) NOT NULL,
    [ShortDesc]  NVARCHAR (255) NOT NULL,
    [AuditTrail] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_CityCodes] PRIMARY KEY CLUSTERED ([CityCodeId] ASC)
);

