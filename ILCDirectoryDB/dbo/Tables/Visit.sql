CREATE TABLE [dbo].[Visit] (
    [VisitId]            INT                IDENTITY (1, 1) NOT NULL,
    [Description]        CHAR (50)          NOT NULL,
    [FamilyId]           INT                NULL,
    [PrimaryPersonId]    INT                NULL,
    [ArrivalDate]        DATE               NULL,
    [DepartureDate]      DATE               NULL,
    [DepartComment]      NVARCHAR (255)     NOT NULL,
    [CreateDateTime]     DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]   DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName] NVARCHAR (256)     NOT NULL,
    CONSTRAINT [PK_Visit] PRIMARY KEY CLUSTERED ([VisitId] ASC)
);

