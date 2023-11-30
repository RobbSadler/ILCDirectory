CREATE TABLE [dbo].[ParentChild]
(
	[ParentChildId] INT IDENTITY(1,1),
	[ParentId] INT NOT NULL,
	[ChildId] INT NOT NULL,
	[CreateDateTime] DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
	[ModifiedDateTime] DATETIMEOFFSET (7) DEFAULT (getdate()) NOT NULL,
	[ModifiedByUserName] NVARCHAR (256) NOT NULL,
	FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Person] ([PersonId]),
	FOREIGN KEY ([ChildId]) REFERENCES [dbo].[Person] ([PersonId]),
	CONSTRAINT [PK_ParentChild] PRIMARY KEY CLUSTERED ([ParentChildId] ASC),
	CONSTRAINT [UQ_ParentChild_Parent_Child] UNIQUE ([ParentId], [ChildId])
)
