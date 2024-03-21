CREATE TABLE [dbo].[SearchTokenPerson]
(
	[SearchTokenPersonId] INT NOT NULL IDENTITY(1,1),
	[SearchTokenId] INT NOT NULL,
	[PersonId] INT NOT NULL,
	CONSTRAINT FK_SearchTokenPerson_Person_PersonId FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]),
	CONSTRAINT FK_SearchTokenPerson_SearchToken_SearchTokenId FOREIGN KEY ([SearchTokenId]) REFERENCES [dbo].[SearchToken] ([SearchTokenId])
)
