-- Get Todo List
CREATE OR ALTER PROCEDURE GetTodoList
AS
BEGIN
    SELECT Id, 
           Title, 
           Description, 
           IsCompleted,
           DueDate, 
           Priority,
           UserId,
           CreatedAt, 
           UpdatedAt 
    FROM dbo.Todos;
END;
GO

-- Get Todo by Id
CREATE OR ALTER PROCEDURE GetTodoById
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SELECT Id, 
           Title, 
           Description, 
           IsCompleted,
           DueDate, 
           Priority,
           UserId,
           CreatedAt, 
           UpdatedAt 
    FROM dbo.Todos
    WHERE Id = @Id;
END;
GO

-- Get Todo by Property
CREATE OR ALTER PROCEDURE GetTodoByProperty
    @PropertyName NVARCHAR(100),
    @PropertyValue NVARCHAR(MAX)
AS
BEGIN
    DECLARE @Sql NVARCHAR(MAX);
    
    IF @PropertyName NOT IN ('Id', 'Title', 'Description', 'IsCompleted', 'DueDate', 'Priority', 'UserId', 'CreatedAt', 'UpdatedAt')
    BEGIN
        RAISERROR('Invalid column name.', 16, 1);
        RETURN;
    END;

    SET @Sql = 'SELECT Id, Title, Description, IsCompleted, DueDate, Priority, UserId, CreatedAt, UpdatedAt 
                FROM dbo.Todos 
                WHERE ' + QUOTENAME(@PropertyName) + ' = @PropertyValue';

    EXEC sp_executesql @Sql, N'@PropertyValue NVARCHAR(MAX)', @PropertyValue;
END;
GO


-- Insert Todo
CREATE OR ALTER PROCEDURE AddTodo
    @Id UNIQUEIDENTIFIER,
    @Title NVARCHAR(MAX),
    @Description NVARCHAR(MAX),
    @DueDate DATETIME2,
    @Priority INT,
    @UserId NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO dbo.Todos 
    (
        Id,
        Title,
        Description,
        IsCompleted,
        DueDate,
        Priority,
        UserId,
        CreatedAt,
        UpdatedAt
    ) 
    VALUES
    (
        @Id,
        @Title,
        @Description,
        0,
        @DueDate,
        @Priority,
        @UserId,
        GETUTCDATE(),
        GETUTCDATE()
    );
END;
GO

-- Update Todo
CREATE OR ALTER PROCEDURE UpdateTodo
    @Id UNIQUEIDENTIFIER,
    @Title NVARCHAR(MAX),
    @Description NVARCHAR(MAX),
    @IsCompleted BIT,
    @DueDate DATETIME2,
    @Priority INT,
    @UserId NVARCHAR(MAX)
AS
BEGIN
    UPDATE dbo.Todos
    SET 
        Title = @Title,
        Description = @Description,
        IsCompleted = @IsCompleted,
        DueDate = @DueDate,
        Priority = @Priority,
        UserId = @UserId,
        UpdatedAt = GETUTCDATE()
    WHERE Id = @Id;
END;
GO

-- Delete Todo
CREATE OR ALTER PROCEDURE DeleteTodo
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    DELETE FROM dbo.Todos WHERE Id = @Id;
END;
GO