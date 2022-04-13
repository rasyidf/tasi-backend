BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
	"MigrationId"	TEXT NOT NULL,
	"ProductVersion"	TEXT NOT NULL,
	CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY("MigrationId")
);
CREATE TABLE IF NOT EXISTS "Products" (
	"ProductId"	INTEGER NOT NULL,
	"Barcode"	TEXT,
	"Name"	TEXT,
	"Stock"	INTEGER NOT NULL,
	"Price"	TEXT NOT NULL,
	"Weight"	REAL NOT NULL,
	"CanManufacture"	INTEGER NOT NULL,
	"Unit"	INTEGER NOT NULL,
	"ModifiedDate"	TEXT NOT NULL DEFAULT (DATETIME('NOW')),
	CONSTRAINT "PK_Products" PRIMARY KEY("ProductId" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "Suppliers" (
	"SupplierId"	INTEGER NOT NULL,
	"Name"	TEXT,
	"Address"	TEXT,
	"Latitude"	REAL NOT NULL,
	"Longitude"	REAL NOT NULL,
	"ShippingCost"	TEXT NOT NULL,
	"ModifiedDate"	TEXT NOT NULL DEFAULT (DATETIME('NOW')),
	CONSTRAINT "PK_Suppliers" PRIMARY KEY("SupplierId" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "Users" (
	"UserId"	INTEGER NOT NULL,
	"FullName"	TEXT,
	"Role"	INTEGER NOT NULL,
	"Username"	TEXT,
	"Password"	TEXT,
	"Address"	TEXT,
	"Latitude"	REAL NOT NULL,
	"Longitude"	REAL NOT NULL,
	"ShippingCost"	TEXT NOT NULL,
	"ModifiedDate"	TEXT NOT NULL DEFAULT (DATETIME('NOW')),
	CONSTRAINT "PK_Users" PRIMARY KEY("UserId" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "Manufacture" (
	"ManufactureId"	INTEGER NOT NULL,
	"ProductId"	INTEGER,
	"ExpectedProduce"	INTEGER NOT NULL,
	"ExpectedCompletion"	TEXT NOT NULL,
	"FinalProduce"	INTEGER NOT NULL,
	"ModifiedDate"	TEXT NOT NULL DEFAULT (DATETIME('NOW')),
	CONSTRAINT "FK_Manufacture_Products_ProductId" FOREIGN KEY("ProductId") REFERENCES "Products"("ProductId"),
	CONSTRAINT "PK_Manufacture" PRIMARY KEY("ManufactureId" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "Orders" (
	"OrderId"	INTEGER NOT NULL,
	"Type"	INTEGER NOT NULL,
	"TotalWeight"	REAL NOT NULL,
	"TotalSales"	TEXT NOT NULL,
	"TotalShipping"	TEXT NOT NULL,
	"TotalTax"	TEXT NOT NULL,
	"SubTotal"	TEXT NOT NULL,
	"SupplierId"	INTEGER,
	"PicUserUserId"	INTEGER,
	"ModifiedDate"	TEXT NOT NULL DEFAULT (DATETIME('NOW')),
	CONSTRAINT "FK_Orders_Users_PicUserUserId" FOREIGN KEY("PicUserUserId") REFERENCES "Users"("UserId"),
	CONSTRAINT "FK_Orders_Suppliers_SupplierId" FOREIGN KEY("SupplierId") REFERENCES "Suppliers"("SupplierId"),
	CONSTRAINT "PK_Orders" PRIMARY KEY("OrderId" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "ManufactureMaterial" (
	"MaterialId"	INTEGER NOT NULL,
	"OrderManufactureId"	INTEGER,
	"ProductId"	INTEGER,
	"Quantity"	INTEGER NOT NULL,
	"ModifiedDate"	TEXT NOT NULL DEFAULT (DATETIME('NOW')),
	CONSTRAINT "FK_ManufactureMaterial_Manufacture_OrderManufactureId" FOREIGN KEY("OrderManufactureId") REFERENCES "Manufacture"("ManufactureId"),
	CONSTRAINT "FK_ManufactureMaterial_Products_ProductId" FOREIGN KEY("ProductId") REFERENCES "Products"("ProductId"),
	CONSTRAINT "PK_ManufactureMaterial" PRIMARY KEY("MaterialId" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "ManufactureStatus" (
	"ManufactureStatusId"	INTEGER NOT NULL,
	"Code"	INTEGER NOT NULL,
	"Message"	TEXT,
	"OrderManufactureId"	INTEGER,
	"ModifiedDate"	TEXT NOT NULL DEFAULT (DATETIME('NOW')),
	CONSTRAINT "FK_ManufactureStatus_Manufacture_OrderManufactureId" FOREIGN KEY("OrderManufactureId") REFERENCES "Manufacture"("ManufactureId"),
	CONSTRAINT "PK_ManufactureStatus" PRIMARY KEY("ManufactureStatusId" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "OrderDetails" (
	"OrderDetailId"	INTEGER NOT NULL,
	"Quantity"	INTEGER NOT NULL,
	"UnitPrice"	TEXT NOT NULL,
	"TotalPrice"	TEXT NOT NULL,
	"TotalWeight"	REAL NOT NULL,
	"Unit"	INTEGER NOT NULL,
	"ProductId"	INTEGER,
	"OrderId"	INTEGER,
	"ModifiedDate"	TEXT NOT NULL DEFAULT (DATETIME('NOW')),
	CONSTRAINT "FK_OrderDetails_Orders_OrderId" FOREIGN KEY("OrderId") REFERENCES "Orders"("OrderId"),
	CONSTRAINT "FK_OrderDetails_Products_ProductId" FOREIGN KEY("ProductId") REFERENCES "Products"("ProductId"),
	CONSTRAINT "PK_OrderDetails" PRIMARY KEY("OrderDetailId" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "OrderStatus" (
	"OrderStatusHistoryId"	INTEGER NOT NULL,
	"Code"	INTEGER NOT NULL,
	"Message"	TEXT,
	"OrderId"	INTEGER,
	"ModifiedDate"	TEXT NOT NULL DEFAULT (DATETIME('NOW')),
	CONSTRAINT "FK_OrderStatus_Orders_OrderId" FOREIGN KEY("OrderId") REFERENCES "Orders"("OrderId"),
	CONSTRAINT "PK_OrderStatus" PRIMARY KEY("OrderStatusHistoryId" AUTOINCREMENT)
);
INSERT INTO "__EFMigrationsHistory" VALUES ('20220330050733_Initial','6.0.0');
CREATE INDEX IF NOT EXISTS "IX_Manufacture_ProductId" ON "Manufacture" (
	"ProductId"
);
CREATE INDEX IF NOT EXISTS "IX_ManufactureMaterial_OrderManufactureId" ON "ManufactureMaterial" (
	"OrderManufactureId"
);
CREATE INDEX IF NOT EXISTS "IX_ManufactureMaterial_ProductId" ON "ManufactureMaterial" (
	"ProductId"
);
CREATE INDEX IF NOT EXISTS "IX_ManufactureStatus_OrderManufactureId" ON "ManufactureStatus" (
	"OrderManufactureId"
);
CREATE INDEX IF NOT EXISTS "IX_OrderDetails_OrderId" ON "OrderDetails" (
	"OrderId"
);
CREATE INDEX IF NOT EXISTS "IX_OrderDetails_ProductId" ON "OrderDetails" (
	"ProductId"
);
CREATE INDEX IF NOT EXISTS "IX_Orders_PicUserUserId" ON "Orders" (
	"PicUserUserId"
);
CREATE INDEX IF NOT EXISTS "IX_Orders_SupplierId" ON "Orders" (
	"SupplierId"
);
CREATE INDEX IF NOT EXISTS "IX_OrderStatus_OrderId" ON "OrderStatus" (
	"OrderId"
);
CREATE UNIQUE INDEX IF NOT EXISTS "IX_Products_Name" ON "Products" (
	"Name"
);
CREATE UNIQUE INDEX IF NOT EXISTS "IX_Suppliers_Name" ON "Suppliers" (
	"Name"
);
CREATE UNIQUE INDEX IF NOT EXISTS "IX_Users_FullName" ON "Users" (
	"FullName"
);
CREATE UNIQUE INDEX IF NOT EXISTS "IX_Users_Username" ON "Users" (
	"Username"
);
COMMIT;
