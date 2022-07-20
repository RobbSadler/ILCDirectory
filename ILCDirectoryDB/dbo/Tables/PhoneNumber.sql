CREATE TABLE [dbo].[PhoneNumber] (
    [PhoneNumberId]         INT        IDENTITY (1, 1) NOT NULL,
    [CountryCode]           NCHAR (4)  CONSTRAINT [DF_PhoneNumber_CountryCode] DEFAULT ('1') NOT NULL,
    [Number]                NCHAR (20) NOT NULL,
    [NumberType]            NCHAR (6) NOT NULL,
    [IncludeInDirectory]    BIT DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [CreateDateTime]        DATETIMEOFFSET (7) CONSTRAINT [DF_PhoneNumber_CreateDateTime] DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]      DATETIMEOFFSET (7) CONSTRAINT [DF_PhoneNumber_ModifiedDateTime] DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]    NVARCHAR (256)     CONSTRAINT [DF_PhoneNumber_ModifiedByUserName] DEFAULT ('system') NOT NULL,
    [DDDId]              INT                NULL,

CONSTRAINT [PK_PhoneNumber] PRIMARY KEY CLUSTERED ([PhoneNumberId] ASC)
);

