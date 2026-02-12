-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'MiBaseDeDatos')
BEGIN
    CREATE DATABASE MiBaseDeDatos;
END
GO

USE MiBaseDeDatos;
GO

-- Tabla: Users
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Users' AND xtype = 'U')
BEGIN
    CREATE TABLE Users (
        id INT IDENTITY(1,1) PRIMARY KEY,
        username NVARCHAR(100) NOT NULL,
        password_hash NVARCHAR(255) NOT NULL
    );
END
GO

-- Tabla: Categories
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Categories' AND xtype = 'U')
BEGIN
    CREATE TABLE Categories (
        id INT IDENTITY(1,1) PRIMARY KEY,
        name NVARCHAR(100) NOT NULL
    );
END
GO

-- Tabla: Products
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Products' AND xtype = 'U')
BEGIN
    CREATE TABLE Products (
        id INT IDENTITY(1,1) PRIMARY KEY,
        id_category INT NOT NULL,
        price DECIMAL(10,2) NOT NULL CHECK (price <= 1000000.00),
        created_at DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Products_Categories
            FOREIGN KEY (id_category)
            REFERENCES Categories(id)
    );
END
GO

-- Datos de prueba
IF NOT EXISTS (SELECT 1 FROM Users WHERE username = 'admin')
BEGIN
    INSERT INTO Users (username, password_hash)
    VALUES ('admin', 'b9ff6b991cdc84277a42cacc41493d5a9dc867445a33999401f50efe8052a022');
    -- "hashedpassword123"
END
GO

IF NOT EXISTS (SELECT 1 FROM Categories WHERE name = 'Limpieza')
BEGIN
    INSERT INTO Categories (name)
    VALUES ('Limpieza'),
           ('Almacén');
END
GO

IF NOT EXISTS (SELECT 1 FROM Products WHERE id_category = 1 AND price = 99.99)
BEGIN
    INSERT INTO Products (id_category, price, created_at)
    VALUES (2, 10.00, '2023-01-25 19:51:32'),
           (1, 60.00, '2024-03-14 08:23:15'),
           (2, 5.00, '2023-07-02 14:37:48'),
           (1, 5.00, '2024-11-30 22:05:11'),
           (2, 15.00, '2023-05-19 11:42:27');
    
    -- Resetear el contador de identidad de Products
    DBCC CHECKIDENT ('Products', RESEED, 5);
END
GO