use Day8Db

CREATE TABLE Products (
    PId NVARCHAR(7) PRIMARY KEY,
    PName NVARCHAR(50) NOT NULL,
    PPrice DECIMAL(10, 2) NOT NULL,
    MnfDate DATE NOT NULL,
    ExpDate DATE NOT NULL
);

INSERT INTO Products VALUES
('P00001', 'Product1', 10.50, '2023-01-01', '2024-01-01'),
('P00002', 'Product2', 15.75, '2023-02-01', '2024-02-01'),
('P00003', 'Product3', 8.99, '2023-03-01', '2024-03-01'),
('P00004', 'Product4', 12.25, '2023-04-01', '2024-04-01'),
('P00005', 'Product5', 18.50, '2023-05-01', '2024-05-01'),
('P00006', 'Product6', 22.75, '2023-06-01', '2024-06-01'),
('P00007', 'Product7', 30.00, '2023-07-01', '2024-07-01'),
('P00008', 'Product8', 14.99, '2023-08-01', '2024-08-01'),
('P00009', 'Product9', 20.49, '2023-09-01', '2024-09-01'),
('P00011', 'Product11', 19.95, '2023-11-01', '2024-11-01'),
('P00012', 'Product12', 27.50, '2023-12-01', '2024-12-01')

SELECT * FROM Products

SELECT TOP 5 * FROM Products ORDER BY PName DESC;
