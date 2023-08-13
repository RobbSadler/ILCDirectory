CREATE TABLE [dbo].[Family] (
    [FamilyId]                  INT                 IDENTITY (1, 1) NOT NULL,
    [FamilyName]                NVARCHAR (50)       NOT NULL,
    [FamilyAddressId]           INT                 NULL,
    [CreateDateTime]            DATETIMEOFFSET (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]          DATETIMEOFFSET (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]        NVARCHAR (256)      NOT NULL,
    CONSTRAINT [PK_Family] PRIMARY KEY CLUSTERED ([FamilyId] ASC)
    )
