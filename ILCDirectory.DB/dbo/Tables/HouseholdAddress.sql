CREATE TABLE [dbo].[HouseholdAddress]
(
	[HouseholdAddressId]        INT                 IDENTITY(1,1),
	[HouseholdId]               INT                 NOT NULL,
	[AddressId]                 INT                 NOT NULL,
    [CreateDateTime]            DATETIMEOFFSET (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]          DATETIMEOFFSET (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]        NVARCHAR (256)      NOT NULL,
    CONSTRAINT [PK_HouseholdAddress] PRIMARY KEY CLUSTERED ([HouseholdAddressId] ASC),
    CONSTRAINT [UQ_HouseholdAddress_Address_Household] UNIQUE ([HouseholdId], [AddressId])
)
