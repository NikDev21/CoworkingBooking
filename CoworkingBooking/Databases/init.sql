CREATE DATABASE IF NOT EXISTS CoworkingDB;
USE CoworkingDB;

-- Создание таблицы пользователей
CREATE TABLE IF NOT EXISTS Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    PasswordHash TEXT NOT NULL
);

-- Создание таблицы рабочих мест (коворкинг-зон)
CREATE TABLE IF NOT EXISTS Workspaces (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Location TEXT NOT NULL,
    IsAvailable BOOLEAN NOT NULL DEFAULT TRUE
);

-- Создание таблицы бронирований
CREATE TABLE IF NOT EXISTS Bookings (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    WorkspaceId INT NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (WorkspaceId) REFERENCES Workspaces(Id) ON DELETE CASCADE
);

-- Добавляем тестовые данные (по желанию)
INSERT INTO Users (Name, Email, PasswordHash) VALUES 
('Иван Иванов', 'ivan@example.com', 'hashedpassword123'),
('Мария Петрова', 'maria@example.com', 'hashedpassword456');

INSERT INTO Workspaces (Name, Location, IsAvailable) VALUES
('Коворкинг Центр #1', 'Москва, ул. Ленина, 10', TRUE),
('Офисное пространство #2', 'Санкт-Петербург, Невский пр., 24', TRUE);