CREATE TABLE [dbo].[Person] (
    [PersonId]                INT                IDENTITY (1, 1) NOT NULL,
    [DateOfBirth]             DATETIME2 (7)      NULL,
    [Comment]                 NVARCHAR (255)     NULL,
    [FirstName]               NVARCHAR (25)      NOT NULL,
    [Gender]                  NVARCHAR (1)       NOT NULL,
    [LastName]                NVARCHAR (25)      NOT NULL,
    [MaidenName]              NVARCHAR (25)      NULL,
    [MaritalStatus]           NVARCHAR (1)       NOT NULL,
    [MiddleName]              NVARCHAR (25)      NULL,
    [NickName]                NVARCHAR (25)      NULL,
    [HouseholdId]             INT                NULL,
    [Suffix]                  NVARCHAR (50)      NULL,
    [Title]                   NVARCHAR (50)      NULL,
	[Position]				  NVARCHAR(255)      NULL,
	[SupervisorName]	      NVARCHAR(100)      NULL,
	[SupervisorNotes]		  NVARCHAR(255)      NULL,
	[WoCode]				  NVARCHAR(255)      NULL,
	[WorkgroupCode]			  INT                NULL,
	[FieldOfService]		  NVARCHAR(255)      NULL,
    [DeleteFlag]              BIT                NULL,
    [ClassificationCode]      NVARCHAR (10)      NOT NULL,
    [LanguagesSpoken]         NVARCHAR (255)     NULL,
    [Notes]                   NVARCHAR (MAX)     NULL,
    [DirectoryCorrectionForm] DATETIME           NULL,
    [DirCorrFormNote]         NVARCHAR (120)     NULL,
    [IsDeceased]              BIT                DEFAULT (0) NOT NULL,
    [IsDeleted]               BIT                DEFAULT (0) NOT NULL,
    [IncludeInDirectory]      BIT                DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [CreateDateTime]          DATETIMEOFFSET (7) CONSTRAINT [DF__Person__CreateDa__7B264821] DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]        DATETIMEOFFSET (7) CONSTRAINT [DF__Person__Modified__7C1A6C5A] DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]      NVARCHAR (256)     CONSTRAINT [DF__Person__Modified__7D0E9093] DEFAULT ('system') NOT NULL,
    [DDDId]              INT                NULL,
    CONSTRAINT [PK_People] PRIMARY KEY CLUSTERED ([PersonId] ASC),
    FOREIGN KEY ([HouseholdId]) REFERENCES [dbo].[Household] ([HouseholdId])
);

GO
CREATE NONCLUSTERED INDEX [IX_Person_LastName]
    ON [dbo].[Person]([LastName] ASC);

