-- Create AddressWord table
CREATE TABLE [dbo].[AddressWord](
	[AddressWordID] [int] IDENTITY(1,1) NOT NULL,
	[AddressID] [int] NOT NULL,
	[WordPosition] [int] NOT NULL,
	[Word] [nvarchar](50) NOT NULL,

)