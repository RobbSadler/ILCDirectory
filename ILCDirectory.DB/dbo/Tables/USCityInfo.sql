CREATE TABLE [dbo].[USCityInfo] (
    [CityInfoId]            INT                 IDENTITY (1, 1) NOT NULL,
    [ZipCode]               NVARCHAR (5)        NOT NULL,
    [City]                  NVARCHAR (255)      NOT NULL,
    [State]                 NVARCHAR (255)      NOT NULL,
    [CreateDateTime]        DATETIMEOFFSET (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]      DATETIMEOFFSET (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]    NVARCHAR (255)      NOT NULL,
    [DDDId]                 INT                 NULL,
    CONSTRAINT [PK_CityCodes] PRIMARY KEY CLUSTERED ([CityInfoId] ASC)
);



