CREATE TABLE [dbo].[Address] (
    [AddressId]                     INT                IDENTITY (1, 1) NOT NULL,
    [AddressLine1]                  NVARCHAR (255)     NOT NULL,
    [AddressLine2]                  NVARCHAR (255)     CONSTRAINT [DF__tmp_ms_xx__Addre__24285DB4] DEFAULT (N'') NULL,
    [AddressLine3]                  NVARCHAR (255)     CONSTRAINT [DF__tmp_ms_xx__Addre__251C81ED] DEFAULT (N'') NULL,
    [AddressLine4]                  NVARCHAR (255)     CONSTRAINT [DF__tmp_ms_xx__Addre__2610A626] DEFAULT (N'') NULL,
    [City]                          NVARCHAR (255)     NOT NULL,
    [State]                         NVARCHAR (50)      NOT NULL,
    [ZipCode]                       NVARCHAR (12)      NOT NULL,
    [HomePhone]                     NVARCHAR (255)     NOT NULL,
    [CellPhone]                     NVARCHAR (255)     NOT NULL,
    [ContactPersonId]               INT                NULL,
    [SpecialContactInfo]            NVARCHAR (255)     NULL,
    [AuditTrail]                    VARCHAR (2000)     NULL,
    [BuildingCode]                  NVARCHAR (255)     CONSTRAINT [DF__tmp_ms_xx__Build__27F8EE98] DEFAULT (N'') NULL,
    [CubicleNumber]                 NVARCHAR (255)     CONSTRAINT [DF__tmp_ms_xx__Cubic__28ED12D1] DEFAULT (N'') NULL,
    [DeliveryCode]                  NVARCHAR (255)     CONSTRAINT [DF__tmp_ms_xx__Deliv__29E1370A] DEFAULT (N'') NULL,
    [IncludeInDirectory]            BIT                CONSTRAINT [DF__tmp_ms_xx__Inclu__2AD55B43] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [IsActive]                      BIT                CONSTRAINT [DF__tmp_ms_xx__IsAct__2BC97F7C] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [MailListFlag]                  BIT                NULL,
    [MailOnly]                      BIT                NULL,
    [MailSortName]                  NVARCHAR (255)     CONSTRAINT [DF__tmp_ms_xx__MailS__2CBDA3B5] DEFAULT (N'') NULL,
    [RoomNumber]                    NVARCHAR (255)     CONSTRAINT [DF__tmp_ms_xx__RoomN__2DB1C7EE] DEFAULT (N'') NULL,
    [CreateDateTime]                DATETIMEOFFSET (7) CONSTRAINT [DF__tmp_ms_xx__Creat__2EA5EC27] DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]              DATETIMEOFFSET (7) CONSTRAINT [DF__tmp_ms_xx__Modif__2F9A1060] DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]            NVARCHAR (256)     CONSTRAINT [DF__tmp_ms_xx__Modif__308E3499] DEFAULT ('system') NOT NULL,
    [DDDId]                         INT                NULL,
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

