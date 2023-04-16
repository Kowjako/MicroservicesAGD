CREATE TABLE Coupon (
	ID SERIAL PRIMARY KEY NOT NULL,
	ProductName VARCHAR(24) NOT NULL,
	Description TEXT,
	Amount INT
)

INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('IPhone X', 'IPhone discount', 150),
														     ('Samsung 10', 'Samsung discount', 100);