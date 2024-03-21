CREATE TABLE [dbo].[Household] (
    [HouseholdId]               INT                 IDENTITY (1, 1) NOT NULL,
    [HouseholdName]             NVARCHAR (50)       NOT NULL,
    [CreateDateTime]            DATETIMEOFFSET (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]          DATETIMEOFFSET (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]        NVARCHAR (256)      NOT NULL,
    CONSTRAINT [PK_Household] PRIMARY KEY CLUSTERED ([HouseholdId] ASC)
    )
