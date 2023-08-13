CREATE TABLE [dbo].[Address] (
    [AddressId]                     INT                IDENTITY (1, 1) NOT NULL,
    [AddressLine1]                  NVARCHAR (255)     NOT NULL,
    [AddressLine2]                  NVARCHAR (255)     DEFAULT (N'') NULL,
    [AddressLine3]                  NVARCHAR (255)     DEFAULT (N'') NULL,
    [AddressLine4]                  NVARCHAR (255)     DEFAULT (N'') NULL,
    [City]                          NVARCHAR (255)     NOT NULL,
    [State]                         NVARCHAR (50)      NOT NULL,
    [ZipCode]                       NVARCHAR (12)      NOT NULL,
    [ContactPersonId]               INT                NULL,
    [SpecialContactInfo]            NVARCHAR (255)     NULL,
    [Notes]                         VARCHAR (2000)     NULL,
    [DeliveryCode]                  NVARCHAR (255)     DEFAULT (N'') NULL,
    [IncludeInDirectory]            BIT                DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [MailListFlag]                  BIT                NULL,
    [MailOnly]                      BIT                NULL,
    [MailSortName]                  NVARCHAR (255)     DEFAULT (N'') NULL,
    [RoomNumber]                    NVARCHAR (255)     DEFAULT (N'') NULL,
    [CreateDateTime]                DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]              DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
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
    ON [dbo].[Address]([ZipCode] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_Address_City]
    ON [dbo].[Address]([City] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_Address_AddressLine4]
    ON [dbo].[Address]([AddressLine4] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_Address_AddressLine3]
    ON [dbo].[Address]([AddressLine3] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_Address_AddressLine2]
    ON [dbo].[Address]([AddressLine2] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_Address_AddressLine1]
    ON [dbo].[Address]([AddressLine1] ASC);
