CREATE TABLE [dbo].[Workgroup] (
    [WorkgroupId]        INT            IDENTITY (1, 1) NOT NULL,
    [WorkgroupCode]      NVARCHAR (255) NOT NULL,
    [Building]           NVARCHAR (255) DEFAULT ((1)) NOT NULL,
    [Room]               NVARCHAR (255) NOT NULL,
    [LongDesc]           NVARCHAR (255) NOT NULL,
    [DirectoryGroupCode] NVARCHAR (255) NOT NULL,
    [Organization]       NVARCHAR (255) NOT NULL,
    [Phone]              NVARCHAR (255) NOT NULL,
    [DirectoryTab]       BIT            NULL,
    [ListDir]            INT            NULL,
    [AuditTrail]         VARCHAR (2000) NOT NULL,
    CONSTRAINT [pk_Workgroup] PRIMARY KEY CLUSTERED ([WorkgroupId] ASC)
);

