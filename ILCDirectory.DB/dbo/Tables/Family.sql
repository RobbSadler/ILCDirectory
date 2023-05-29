CREATE TABLE [dbo].[Family] (
    [FamilyId]                  INT                 IDENTITY (1, 1) NOT NULL,
    [FamilyName]                NVARCHAR (50)       NOT NULL,
    [FamilyAddressId]           INT                 NULL,
    CONSTRAINT [PK_Family] PRIMARY KEY CLUSTERED ([FamilyId] ASC)
    )
