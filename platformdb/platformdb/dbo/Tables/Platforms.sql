CREATE TABLE [dbo].[Platforms] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (MAX) NOT NULL,
    [Publisher] NVARCHAR (MAX) NOT NULL,
    [Cost]      NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Platforms] PRIMARY KEY CLUSTERED ([Id] ASC)
);

