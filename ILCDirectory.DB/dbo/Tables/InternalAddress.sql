CREATE TABLE [dbo].[InternalAddress]
(
	[InternalAddressId]     INT                 NOT NULL IDENTITY (1,1),
    [PersonId]              INT                 NOT NULL,
    [BoxNumber]             NVARCHAR (255)      NULL,
    [SpecialHandling]       NVARCHAR (255)      NULL,
    [DeliveryCode]          NVARCHAR (50)       NULL,
    [MailListFlag]          BIT                 NULL,
    [IncludeInSort]         BIT                 NULL,
)
