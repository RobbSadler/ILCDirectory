CREATE TABLE [dbo].[Address] (
    [AddressId]                     INT                 IDENTITY (1, 1) NOT NULL,
    [AddressLine1]                  NVARCHAR (255)      NOT NULL,
    [AddressLine2]                  NVARCHAR (255)      DEFAULT (N'') NULL,
    [City]                          NVARCHAR (255)      NOT NULL,
    [StateProvince]                 NVARCHAR (50)       NULL,
    [PostalCode]                    NVARCHAR (20)       NULL,
    [Country]                       NVARCHAR (50)       DEFAULT (N'United States of America') NOT NULL,
    [ArrivalDate]                   DATETIMEOFFSET (7)  NULL,
    [DepartureDate]                 DATETIMEOFFSET (7)  NULL,
    [ContactPersonName]             NVARCHAR(50)        NULL,
    [ContactPersonPhone]            NVARCHAR(50)        NULL,
    [Notes]                         VARCHAR (2000)      NULL,
    [IncludeInDirectory]            BIT                 DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [IsVerified]                    BIT                 DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [CreateDateTime]                DATETIMEOFFSET (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]              DATETIMEOFFSET (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]            NVARCHAR (256)      NOT NULL,
    [DDDId]                         INT                 NULL,
    SysStartTime DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL,
    SysEndTime DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL,
    PERIOD FOR SYSTEM_TIME (SysStartTime,SysEndTime),
    CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED ([AddressId] ASC)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.AddressHistory));
GO

CREATE NONCLUSTERED INDEX [IX_Address_ZipCode]
    ON [dbo].[Address]([PostalCode] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_Address_City]
    ON [dbo].[Address]([City] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_Address_AddressLine2]
    ON [dbo].[Address]([AddressLine2] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_Address_AddressLine1]
    ON [dbo].[Address]([AddressLine1] ASC);
