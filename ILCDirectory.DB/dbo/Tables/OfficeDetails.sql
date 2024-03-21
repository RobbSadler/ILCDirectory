CREATE TABLE [dbo].[OfficeDetails]
(
	[OfficeDetailsId]		INT IDENTITY(1,1) NOT NULL,
	[PersonId]				INT NOT NULL,
	[BuildingId]			INT NULL,
	[RoomNumber]			NVARCHAR(25) NULL,
	[CubicleNumber]			NVARCHAR(50) NULL,
	[IncludeInDirectory]	BIT NOT NULL,
    [CreateDateTime]		DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]		DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]	NVARCHAR (256)     DEFAULT ('system') NOT NULL,
    [DDDPersonId]			INT                NULL,
	CONSTRAINT [PK_OfficeDetails] PRIMARY KEY CLUSTERED ([OfficeDetailsId] ASC)
)
