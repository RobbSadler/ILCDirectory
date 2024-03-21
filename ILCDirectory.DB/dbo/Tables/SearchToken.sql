CREATE TABLE [dbo].[SearchToken]
(
	[SearchTokenId] [int] IDENTITY(1,1) NOT NULL CONSTRAINT PK_SearchToken_SearchTokenId PRIMARY KEY,
	[Token] [nvarchar](255) NOT NULL,
);
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SearchToken_Token]	ON [dbo].[SearchToken]([Token] ASC);
GO
