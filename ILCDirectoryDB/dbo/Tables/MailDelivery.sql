CREATE TABLE [dbo].[MailDelivery] (
    [MailDeliveryId]   INT             IDENTITY (1, 1) NOT NULL,
    [DeliveryCode]     NVARCHAR (255)  NOT NULL,
    [DeliveryLocation] NVARCHAR (255)  NOT NULL,
    [AuditTrail]       NVARCHAR (2000) NOT NULL,
    CONSTRAINT [PK_MailDelivery] PRIMARY KEY CLUSTERED ([MailDeliveryId] ASC)
);

