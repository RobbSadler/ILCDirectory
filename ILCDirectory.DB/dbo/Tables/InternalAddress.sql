CREATE TABLE [dbo].[InternalAddress]
(
	[InternalAddressId]     INT                 NOT NULL IDENTITY (1,1),
    [PersonId]              INT                 NOT NULL,
    [SpecialContactInfo]    NVARCHAR (255)      NULL,
    [DeliveryCode]          NVARCHAR (50)       NULL,
    [MailListFlag]          BIT                 NULL,
    [MailOnly]              BIT                 NULL,
    [MailSortName]          NVARCHAR (50)       NULL,
    [RoomNumber]            NVARCHAR (100)      NULL
)
