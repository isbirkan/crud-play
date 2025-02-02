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
           CreatedAt, 
           UpdatedAt 
    FROM dbo.Todos
    WHERE Id = @Id;
END;
GO

-- Insert Todo
CREATE OR ALTER PROCEDURE AddTodo
    @Id UNIQUEIDENTIFIER,
    @Title NVARCHAR(MAX),
    @Description NVARCHAR(MAX),
    @DueDate DATETIME2,
    @Priority INT
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
    @Priority INT
AS
BEGIN
    UPDATE dbo.Todos
    SET 
        Title = @Title,
        Description = @Description,
        IsCompleted = @IsCompleted,
        DueDate = @DueDate,
        Priority = @Priority,
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