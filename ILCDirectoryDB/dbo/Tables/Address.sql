CREATE TABLE [dbo].[Address] (
    [AddressId]                     INT                IDENTITY (1, 1) NOT NULL,
    [AddressLine1]                  NVARCHAR (255)     NOT NULL,
    [AddressLine2]                  NVARCHAR (255)     DEFAULT (N'') NOT NULL,
    [AddressLine3]                  NVARCHAR (255)     DEFAULT (N'') NOT NULL,
    [AddressLine4]                  NVARCHAR (255)     DEFAULT (N'') NOT NULL,
    [City]                          NVARCHAR (255)     NOT NULL,
    [State]                         NVARCHAR (50)      NOT NULL,
    [ZipCode]                       NVARCHAR (12)      NOT NULL,
    [HomePhone]                     NVARCHAR (255)     NOT NULL,
    [CellPhone]                     NVARCHAR (255)     NOT NULL,
    [SpecialContactInfo]            NVARCHAR (255)     NOT NULL,
    [SpecialForwardingInstructions] NVARCHAR (255)     NOT NULL,
    [SpecialHandlingInstructions]   NVARCHAR (255)     NOT NULL,
    [AuditTrail]                    VARCHAR (2000)     NOT NULL,
    [BoxNumber]                     NVARCHAR (255)     DEFAULT (N'') NOT NULL,
    [BuildingCode]                  NVARCHAR (255)     DEFAULT (N'') NOT NULL,
    [CubicleNumber]                 NVARCHAR (255)     DEFAULT (N'') NOT NULL,
    [DeliveryCode]                  NVARCHAR (255)     DEFAULT (N'') NOT NULL,
    [IncludeInDirectory]            BIT                DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [IsActive]                      BIT                DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [MailListFlag]                  BIT                NULL,
    [MailOnly]                      BIT                NULL,
    [MailSortName]                  NVARCHAR (255)     DEFAULT (N'') NOT NULL,
    [RoomNumber]                    NVARCHAR (255)     DEFAULT (N'') NOT NULL,
    [CreateDateTime]                DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]              DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]            NVARCHAR (256)     DEFAULT ('system') NOT NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED ([AddressId] ASC)
);




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

