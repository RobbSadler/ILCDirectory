CREATE TABLE [dbo].[DeliveryCodeLocation]
(
	[DeliveryCode] NVARCHAR(30) NOT NULL PRIMARY KEY,
    [DeliveryLocation]   NVARCHAR (255)     NOT NULL,
    [CreateDateTime]     DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]   DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName] NVARCHAR (256)     DEFAULT ('system') NOT NULL
)
