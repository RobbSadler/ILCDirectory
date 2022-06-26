CREATE TABLE [dbo].[Vehicle] (
    [VehicleId]          INT                IDENTITY (1, 1) NOT NULL,
    [VehicleOwner]       INT                NOT NULL,
    [VehicleYear]        INT                NULL,
    [VehicleColor]       VARCHAR (32)       NOT NULL,
    [VehicleMake]        VARCHAR (32)       NOT NULL,
    [VehicleModel]       VARCHAR (32)       NOT NULL,
    [TagIssuer]          VARCHAR (32)       NOT NULL,
    [TagNumber]          VARCHAR (16)       NOT NULL,
    [PermitType]         VARCHAR (8)        NOT NULL,
    [PermitNumber]       INT                NULL,
    [PermitExpires]      DATETIME           NULL,
    [AuditTrail]         VARCHAR (2000)     NOT NULL,
    [CreateDateTime]     DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]   DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName] NVARCHAR (256)     DEFAULT ('system') NOT NULL,
    CONSTRAINT [PK_Vehicle] PRIMARY KEY CLUSTERED ([VehicleId] ASC),
    CONSTRAINT [FK_Vehicle_Person] FOREIGN KEY ([VehicleOwner]) REFERENCES [dbo].[Person] ([PersonId])
);




GO
CREATE NONCLUSTERED INDEX [IX_Vehicle_VehicleOwner]
    ON [dbo].[Vehicle]([VehicleOwner] ASC);

