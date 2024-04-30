\c HoneyRaes

INSERT INTO Customer (Name, Address) VALUES ('Zavier', '123 Loopy Lane');
INSERT INTO Customer (Name, Address) VALUES ('Chad', '345 Birch Boulevard');
INSERT INTO Customer (Name, Address) VALUES ('Ezra', '678 Colander Cove');

INSERT INTO Employee (Name, Specialty) VALUES ('Zachary', 'PCs and Phones');
INSERT INTO Employee (Name, Specialty) VALUES ('Lincoln', 'Instruments');

INSERT INTO ServiceTicket (CustomerId, EmployeeId, Description, Emergency, DateCompleted) VALUES ( 1, 1, 'Laptop battery is bulging', true, TIMESTAMP '2024-03-19' );
INSERT INTO ServiceTicket (CustomerId, EmployeeId, Description, DateCompleted) VALUES ( 1, 2, 'Keyboard keys inconsistent', null );
INSERT INTO ServiceTicket (CustomerId, EmployeeId, Description, DateCompleted) VALUES ( 2, 2, 'Guitar strings won''t stay taut', TIMESTAMP '2024-04-08' );
INSERT INTO ServiceTicket (CustomerId, EmployeeId, Description, Emergency, DateCompleted) VALUES ( 3, null, 'Computer suddenly running too slow, can''t do coursework', true, null );
INSERT INTO ServiceTicket (CustomerId, EmployeeId, Description, DateCompleted) VALUES ( 2, null, 'RGB not working', null );