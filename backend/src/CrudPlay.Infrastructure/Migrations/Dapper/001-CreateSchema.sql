IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'dbo')
BEGIN
    EXEC('CREATE SCHEMA dbo');
END;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Todos')
BEGIN
    CREATE TABLE dbo.Todos (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        Title NVARCHAR(MAX) NOT NULL,
        Description NVARCHAR(MAX) NOT NULL,
        IsCompleted BIT NOT NULL,
        DueDate DATETIME2 NULL,
        Priority INT NOT NULL,
        UserId NVARCHAR(MAX) NOT NULL,
        CreatedAt DATETIME2 NULL,
        UpdatedAt DATETIME2 NULL,

        CONSTRAINT FK_Todos_AspNetUsers FOREIGN KEY (UserId) 
        REFERENCES dbo.AspNetUsers(Id) 
        ON DELETE CASCADE
    );
END;
GO