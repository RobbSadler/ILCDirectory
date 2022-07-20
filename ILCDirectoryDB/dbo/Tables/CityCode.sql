CREATE TABLE [dbo].[CityCode] (
    [CityCodeId]         INT                IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (255)     NOT NULL,
    [Desc]               NVARCHAR (255)     NOT NULL,
    [ShortDesc]          NVARCHAR (255)     NOT NULL,
    [AuditTrail]         NVARCHAR (255)     NOT NULL,
    [CreateDateTime]     DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]   DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName] NVARCHAR (256)     NOT NULL,
    [DDDId]              INT                NULL,
    CONSTRAINT [PK_CityCodes] PRIMARY KEY CLUSTERED ([CityCodeId] ASC)
);



