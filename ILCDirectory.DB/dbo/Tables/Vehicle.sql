CREATE TABLE [dbo].[Vehicle] (
    [VehicleId]          INT                IDENTITY (1, 1) NOT NULL,
    [VehicleOwner]       INT                NOT NULL,
    [Year]        INT                NULL,
    [Color]       VARCHAR (32)       NULL,
    [Make]        VARCHAR (32)       NULL,
    [Model]       VARCHAR (32)       NULL,
    [TagIssuer]          VARCHAR (32)       NULL,
    [TagNumber]          VARCHAR (16)       NULL,
    [PermitType]         VARCHAR (8)        NULL,
    [PermitNumber]       INT                NULL,
    [PermitExpires]      DATETIME           NULL,
    [Notes]              VARCHAR (2000)     NULL,
    [CreateDateTime]     DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]   DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName] NVARCHAR (256)     DEFAULT ('system') NOT NULL,
    [DDDId]              INT                NULL,
    CONSTRAINT [PK_Vehicle] PRIMARY KEY CLUSTERED ([VehicleId] ASC),
    CONSTRAINT [FK_Vehicle_Person] FOREIGN KEY ([VehicleOwner]) REFERENCES [dbo].[Person] ([PersonId])
);



GO
CREATE NONCLUSTERED INDEX [IX_Vehicle_VehicleOwner]
    ON [dbo].[Vehicle]([VehicleOwner] ASC);



