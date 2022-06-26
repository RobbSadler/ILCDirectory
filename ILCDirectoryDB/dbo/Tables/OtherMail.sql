CREATE TABLE [dbo].[OtherMail] (
    [OtherMailId]        INT                IDENTITY (1, 1) NOT NULL,
    [Sender]             NVARCHAR (50)      NOT NULL,
    [Receiver]           NVARCHAR (50)      NOT NULL,
    [DeliveryCode]       INT                NULL,
    [Comments]           NVARCHAR (500)     NOT NULL,
    [CreateDateTime]     DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedDateTime]   DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
    [ModifiedByUserName] NVARCHAR (256)     DEFAULT ('system') NOT NULL,
    CONSTRAINT [PK_OtherMail] PRIMARY KEY CLUSTERED ([OtherMailId] ASC)
);



