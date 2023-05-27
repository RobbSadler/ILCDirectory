-- Create the history table
CREATE TABLE [dbo].[Address_History] (
    [AddressHistoryId]              INT                IDENTITY (1, 1) NOT NULL,
    [AddressId]                     INT                NOT NULL,
    [AddressLine1]                  NVARCHAR (255)     NOT NULL,
    [AddressLine2]                  NVARCHAR (255)     CONSTRAINT [DF__Address_History__AddressLine2] DEFAULT (N'') NULL,
    [AddressLine3]                  NVARCHAR (255)     CONSTRAINT [DF__Address_History__AddressLine3] DEFAULT (N'') NULL,
    [AddressLine4]                  NVARCHAR (255)     CONSTRAINT [DF__Address_History__AddressLine4] DEFAULT (N'') NULL,
    [City]                          NVARCHAR (255)     NOT NULL,
    [State]                         NVARCHAR (50)      NOT NULL,
    [ZipCode]                       NVARCHAR (12)      NOT NULL,
    [ContactPersonId]               INT                NULL,
    [SpecialContactInfo]            NVARCHAR (255)     NULL,
    [AuditTrail]                    VARCHAR (2000)     NULL,
    [DeliveryCode]                  NVARCHAR (255)     CONSTRAINT [DF__Address_History__DeliveryCode] DEFAULT (N'') NULL,
    [IncludeInDirectory]            BIT                CONSTRAINT [DF__Address_History__IncludeInDirectory] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [MailListFlag]                  BIT                NULL,
    [MailOnly]                      BIT                NULL,
    [MailSortName]                  NVARCHAR (255)     CONSTRAINT [DF__Address_History__MailSortName] DEFAULT (N'') NULL,
    [RoomNumber]                    NVARCHAR (255)     CONSTRAINT [DF__Address_History__RoomNumber] DEFAULT (N'') NULL,
    [CreateDateTime]                DATETIMEOFFSET (7) CONSTRAINT [DF__Address_History__CreateDateTime] DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]              DATETIMEOFFSET (7) CONSTRAINT [DF__Address_History__ModifiedDateTime] DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]            NVARCHAR (256)      NOT NULL,
    [DDDId]                         INT                 NULL,
    [ValidFrom]                     DATETIME2           NOT NULL,
    [ValidTo]                       DATETIME2           NOT NULL,
    CONSTRAINT [PK_Address_History] PRIMARY KEY CLUSTERED ([AddressHistoryId] ASC),
    CONSTRAINT [FK_Address_History_Address] FOREIGN KEY ([AddressId]) REFERENCES [dbo].[Address] ([AddressId])
)
GO

-- Create a trigger to populate the history table on update or delete
CREATE TRIGGER trg_Address_History
ON [dbo].[Address]
AFTER UPDATE, DELETE
AS
BEGIN
    -- Insert the modified or deleted rows into the history table
    INSERT INTO [dbo].[Address_History] (
        [AddressId],
        [AddressLine1],
        [AddressLine2],
        [AddressLine3],
        [AddressLine4],
        [City],
        [State],
        [ZipCode],
        [ContactPersonId],
        [SpecialContactInfo],
        [AuditTrail],
        [DeliveryCode],
        [IncludeInDirectory],
        [MailListFlag],
        [MailOnly],
        [MailSortName],
        [RoomNumber],
        [CreateDateTime],
        [ModifiedDateTime],
        [ModifiedByUserName],
        [DDDId],
        [ValidFrom],
        [ValidTo]
    )
    SELECT
        [AddressId],
        [AddressLine1],
        [AddressLine2],
        [AddressLine3],
        [AddressLine4],
        [City],
        [State],
        [ZipCode],
        [ContactPersonId],
        [SpecialContactInfo],
        [AuditTrail],
        [DeliveryCode],
        [IncludeInDirectory],
        [MailListFlag],
        [MailOnly],
        [MailSortName],
        [RoomNumber],
        [CreateDateTime],
        [ModifiedDateTime],
        [ModifiedByUserName],
        [DDDId],
        [ValidFrom],
        [ValidTo]
    FROM deleted; -- Deleted rows in case of UPDATE or DELETE

    -- Update the ValidTo value of the most recent history record
    UPDATE [dbo].[Address_History]
    SET [ValidTo] = DATEADD(MILLISECOND, -1, SYSDATETIMEOFFSET())
    WHERE [AddressId] IN (
        SELECT [AddressId]
        FROM inserted -- Inserted rows in case of UPDATE
    );

END
GO
       




CREATE TABLE AddressHistory
(
    [AddressId]                     INT                 NOT NULL,
    [AddressLine1]                  NVARCHAR (255)      NOT NULL,
    [AddressLine2]                  NVARCHAR (255)      NULL,
    [AddressLine3]                  NVARCHAR (255)      NULL,
    [AddressLine4]                  NVARCHAR (255)      NULL,
    [City]                          NVARCHAR (255)      NOT NULL,
    [State]                         NVARCHAR (50)       NOT NULL,
    [ZipCode]                       NVARCHAR (12)       NOT NULL,
    [ContactPersonId]               INT                 NULL,
    [SpecialContactInfo]            NVARCHAR (255)      NULL,
    [AuditTrail]                    VARCHAR (2000)      NULL,
    [DeliveryCode]                  NVARCHAR (255)      NULL,
    [IncludeInDirectory]            BIT                 NOT NULL,
    [MailListFlag]                  BIT                 NULL,
    [MailOnly]                      BIT                 NULL,
    [MailSortName]                  NVARCHAR (255)      NULL,
    [RoomNumber]                    NVARCHAR (255)      NULL,
    [CreateDateTime]                DATETIMEOFFSET (7)  NOT NULL,
    [ModifiedDateTime]              DATETIMEOFFSET (7)  NOT NULL,
    [ModifiedByUserName]            NVARCHAR (256)      NOT NULL,
    [DDDId]                         INT                 NULL,
    [ValidFrom]                     DATETIME2           NOT NULL,
    [ValidTo]                       DATETIME2           NOT NULL,
);
GO

CREATE TABLE [dbo].[Address] (
    [AddressId]                     INT                IDENTITY (1, 1) NOT NULL,
    [AddressLine1]                  NVARCHAR (255)     NOT NULL,
    [AddressLine2]                  NVARCHAR (255)     CONSTRAINT [DF__tmp_ms_xx__Addre__24285DB4] DEFAULT (N'') NULL,
    [AddressLine3]                  NVARCHAR (255)     CONSTRAINT [DF__tmp_ms_xx__Addre__251C81ED] DEFAULT (N'') NULL,
    [AddressLine4]                  NVARCHAR (255)     CONSTRAINT [DF__tmp_ms_xx__Addre__2610A626] DEFAULT (N'') NULL,
    [City]                          NVARCHAR (255)     NOT NULL,
    [State]                         NVARCHAR (50)      NOT NULL,
    [ZipCode]                       NVARCHAR (12)      NOT NULL,
    [ContactPersonId]               INT                NULL,
    [SpecialContactInfo]            NVARCHAR (255)     NULL,
    [AuditTrail]                    VARCHAR (2000)     NULL,
    [DeliveryCode]                  NVARCHAR (255)     CONSTRAINT [DF__tmp_ms_xx__Deliv__29E1370A] DEFAULT (N'') NULL,
    [IncludeInDirectory]            BIT                CONSTRAINT [DF__tmp_ms_xx__Inclu__2AD55B43] DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [MailListFlag]                  BIT                NULL,
    [MailOnly]                      BIT                NULL,
    [MailSortName]                  NVARCHAR (255)     CONSTRAINT [DF__tmp_ms_xx__MailS__2CBDA3B5] DEFAULT (N'') NULL,
    [RoomNumber]                    NVARCHAR (255)     CONSTRAINT [DF__tmp_ms_xx__RoomN__2DB1C7EE] DEFAULT (N'') NULL,
    [CreateDateTime]                DATETIMEOFFSET (7) CONSTRAINT [DF__tmp_ms_xx__Creat__2EA5EC27] DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]              DATETIMEOFFSET (7) CONSTRAINT [DF__tmp_ms_xx__Modif__2F9A1060] DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]            NVARCHAR (256)      NOT NULL,
    [DDDId]                         INT                 NULL,
    [ValidFrom]                     DATETIME2           NOT NULL,
    [ValidTo]                       DATETIME2           NOT NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED ([AddressId] ASC)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.AddressHistory));
GO

CREATE NONCLUSTERED INDEX IX_AddressHistory_ID_PERIOD_COLUMNS
    ON AddressHistory (ValidTo, ValidFrom, AddressId);
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
