CREATE TABLE [dbo].[User] (
    [Id]       INT          NOT NULL,
    [username] VARCHAR (50) NOT NULL,
    [password] VARCHAR (50) NOT NULL,
    [isValid]  TINYINT      DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

