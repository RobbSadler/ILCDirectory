CREATE TABLE [dbo].[SearchTokenAddress]
(
	[SearchTokenAddressId] INT NOT NULL IDENTITY(1,1),
	[SearchTokenId] INT NOT NULL,
	[AddressId] INT NOT NULL,
	CONSTRAINT FK_SearchTokenAddress_Address_AddressId FOREIGN KEY ([AddressId]) REFERENCES [dbo].[Address] ([AddressId]),
	CONSTRAINT FK_SearchTokenAddress_SearchToken_SearchTokenId FOREIGN KEY ([SearchTokenId]) REFERENCES [dbo].[SearchToken] ([SearchTokenId])
)
