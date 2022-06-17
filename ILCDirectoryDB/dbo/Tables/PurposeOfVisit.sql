CREATE TABLE [dbo].[PurposeOfVisit] (
    [PurposeOfVisitId]   INT       IDENTITY (1, 1) NOT NULL,
    [PurposeOfVisitDesc] CHAR (50) NOT NULL,
    CONSTRAINT [PK_PurposeOfVisit] PRIMARY KEY CLUSTERED ([PurposeOfVisitId] ASC)
);

