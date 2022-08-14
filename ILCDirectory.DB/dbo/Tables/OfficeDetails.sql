CREATE TABLE [dbo].[OfficeDetails]
(
	[OfficeDetailsId]		INT IDENTITY(1,1) NOT NULL,
	[BuildingCode]			NVARCHAR(20) NULL,
	[RoomNumber]			NVARCHAR(20) NULL,
	[CubicleNumber]			NVARCHAR(50) NULL,
	[Position]				NVARCHAR(255) NULL,
	[SupervisorPersonId]	INT NULL,
	[SupervisorNotes]		NVARCHAR(255) NULL,
	[WoCode]				NVARCHAR(255) NULL,
	[WorkgroupCode]			INT NULL,
	[FieldOfService]		NVARCHAR(255) NULL,
	[IncludeInDirectory]	BIT NOT NULL,
    [CreateDateTime]		DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]		DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]	NVARCHAR (256)     DEFAULT ('system') NOT NULL,
    [DDDPersonId]			INT                NULL,
	CONSTRAINT [PK_OfficeDetails] PRIMARY KEY CLUSTERED ([OfficeDetailsId] ASC)
)
