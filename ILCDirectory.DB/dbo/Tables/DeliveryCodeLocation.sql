CREATE TABLE [dbo].[DeliveryCodeLocation]
(
    [DeliveryCodeLocationId]    INT                 IDENTITY (1, 1) NOT NULL,
	[DeliveryCode]              NVARCHAR(30)        NOT NULL,
    [DeliveryLocation]          NVARCHAR (255)      NOT NULL,
    [CreateDateTime]            DATETIMEOFFSET (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]          DATETIMEOFFSET (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName]        NVARCHAR (256)      DEFAULT ('system') NOT NULL
)
