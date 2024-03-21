CREATE TABLE [dbo].[HouseholdAddress]
(
	[HouseholdAddressId]        INT                 IDENTITY(1,1),
	[HouseholdId]               INT                 NOT NULL,
	[AddressId]                 INT                 NULL,
    [InternalAddressId]		    INT                 NULL,
    [IsPermanent]               BIT                 DEFAULT (1) NOT NULL,
    [IncludeInDirectory]        BIT                 DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [MailOnly]                  BIT                 DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [PurposeOfVisit]            INT                 NULL,
    [ArrivalDate]               DATETIMEOFFSET (7)  NULL,
    [DepartureDate]             DATETIMEOFFSET (7)  NULL,
    [CreateDateTime]            DATETIMEOFFSET (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]          DATETIMEOFFSET (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]        NVARCHAR (256)      NOT NULL,
    CONSTRAINT [PK_HouseholdAddress] PRIMARY KEY CLUSTERED ([HouseholdAddressId] ASC),
    CONSTRAINT [UQ_HouseholdAddress_Address_Household] UNIQUE ([HouseholdId], [AddressId])
)
