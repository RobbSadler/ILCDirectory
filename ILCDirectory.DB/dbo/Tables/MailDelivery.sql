CREATE TABLE [dbo].[MailDelivery] (
    [MailDeliveryId]     INT                IDENTITY (1, 1) NOT NULL,
    [DeliveryCode]       NVARCHAR (255)     NOT NULL,
    [DeliveryLocation]   NVARCHAR (255)     NOT NULL,
    [BoxNumber]                     NVARCHAR (255)     CONSTRAINT [DF__tmp_ms_xx__BoxNu__2704CA5F] DEFAULT (N'') NULL,
    [SpecialForwardingInstructions] NVARCHAR (255)     NULL,
    [SpecialHandlingInstructions]   NVARCHAR (255)     NULL,
    [AuditTrail]         NVARCHAR (2000)    NOT NULL,
    [CreateDateTime]     DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]   DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName] NVARCHAR (256)     DEFAULT ('system') NOT NULL,
    [DDDId]              INT                NULL,
    CONSTRAINT [PK_MailDelivery] PRIMARY KEY CLUSTERED ([MailDeliveryId] ASC)
);



