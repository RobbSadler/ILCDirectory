CREATE TABLE [dbo].[PersonHousehold]
(
	[PersonHouseholdId]         INT                 IDENTITY(1,1),
	[PersonId]                  INT                 NOT NULL,
	[HouseholdId]               INT                 NOT NULL,
    [CreateDateTime]            DATETIMEOFFSET (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]          DATETIMEOFFSET (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]        NVARCHAR (256)      NOT NULL,
    CONSTRAINT [PK_PersonHousehold] PRIMARY KEY CLUSTERED ([PersonHouseholdId] ASC),
    CONSTRAINT [UQ_PersonHousehold_Person_Household] UNIQUE ([PersonId], [HouseholdId])
)
