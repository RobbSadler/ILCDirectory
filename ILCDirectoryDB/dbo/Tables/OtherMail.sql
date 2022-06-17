CREATE TABLE [dbo].[OtherMail] (
    [OtherMailId]  INT            IDENTITY (1, 1) NOT NULL,
    [Sender]       NVARCHAR (50)  NOT NULL,
    [Receiver]     NVARCHAR (50)  NOT NULL,
    [DeliveryCode] INT            NULL,
    [Comments]     NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_OtherMail] PRIMARY KEY CLUSTERED ([OtherMailId] ASC)
);

