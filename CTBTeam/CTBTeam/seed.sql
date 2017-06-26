CREATE TABLE [dbo].[Accounts] (
    [ID]    INT           IDENTITY (1, 1) NOT NULL,
    [User]  NVARCHAR (10) NOT NULL,
    [Pass]  NVARCHAR (10) NOT NULL,
    [Admin] BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

CREATE TABLE [dbo].[Dates] (
    [ID]    INT  IDENTITY (1, 1) NOT NULL,
    [Dates] DATE NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

CREATE TABLE [dbo].[Phones] (
    [Id]     INT           IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (50) NOT NULL,
    [Active] BIT           DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Projects] (
    [ID]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (50) NOT NULL,
    [Category] NCHAR (1)     NOT NULL,
    [Active]   BIT           DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

CREATE TABLE [dbo].[Vehicles] (
    [ID]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (150) NOT NULL,
    [Active] BIT            DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

CREATE TABLE [dbo].[Employees] (
    [Alna_num]  INT            NOT NULL,
    [Name]      NVARCHAR (150) NOT NULL,
    [Full_time] BIT            DEFAULT ((0)) NOT NULL,
    [Active]    BIT            DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Alna_num] ASC)
);


GO

CREATE TRIGGER [dbo].[Update_Employee]
    ON [dbo].[Employees]
    FOR UPDATE
    AS
    BEGIN
        SET NoCount ON
		if update(Active)
			delete from TimeOff where TimeOff.Alna_num in (select Alna_num from Employees where Active=0);
    END

CREATE TABLE [dbo].[PhoneCheckout] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [Phone_ID]   INT            NOT NULL,
    [Vehicle_ID] INT            NOT NULL,
    [Alna_num]   INT            NOT NULL,
    [Start]      DATE           DEFAULT (getdate()) NOT NULL,
    [End]        DATE           NOT NULL,
    [Active]     BIT            DEFAULT ((1)) NOT NULL,
    [Purpose]    NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_PhoneCheckout_Employees] FOREIGN KEY ([Alna_num]) REFERENCES [dbo].[Employees] ([Alna_num]),
    CONSTRAINT [FK_PhoneCheckout_Vehicle_ID] FOREIGN KEY ([Vehicle_ID]) REFERENCES [dbo].[Vehicles] ([ID]),
    CONSTRAINT [FK_PhoneCheckout_Phones] FOREIGN KEY ([Phone_ID]) REFERENCES [dbo].[Phones] ([Id])
);

CREATE TABLE [dbo].[ProjectHours] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [Alna_num]     INT NOT NULL,
    [Proj_ID]      INT NOT NULL,
    [Hours_worked] INT DEFAULT ((0)) NOT NULL,
    [Date_ID]      INT NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Proj_Hours_Alna_num] FOREIGN KEY ([Alna_num]) REFERENCES [dbo].[Employees] ([Alna_num]),
    CONSTRAINT [FK_Proj_Hours_Date_ID] FOREIGN KEY ([Date_ID]) REFERENCES [dbo].[Dates] ([ID]),
    CONSTRAINT [FK_Proj_Hours_Proj_ID] FOREIGN KEY ([Proj_ID]) REFERENCES [dbo].[Projects] ([ID])
);

CREATE TABLE [dbo].[PurchaseOrders] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Alna_num]    INT            NOT NULL,
    [Name]        NVARCHAR (200) NOT NULL,
    [Qty]         INT            DEFAULT ((1)) NOT NULL,
    [Description] TEXT           NOT NULL,
    [Price]       MONEY          NOT NULL,
    [Priority]    INT            DEFAULT ((1)) NOT NULL,
    [Link]        NVARCHAR (MAX) NOT NULL,
    [Date_added]  DATE           NOT NULL,
    [Purchaser]   INT            DEFAULT (NULL) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_PurchaseOrder_Creator] FOREIGN KEY ([Alna_num]) REFERENCES [dbo].[Employees] ([Alna_num]),
    CONSTRAINT [FK_PurchaseOrder_Purchaser] FOREIGN KEY ([Purchaser]) REFERENCES [dbo].[Employees] ([Alna_num])
);

CREATE TABLE [dbo].[TimeOff] (
    [ID]       INT  IDENTITY (1, 1) NOT NULL,
    [Alna_num] INT  DEFAULT ((0)) NOT NULL,
    [Start]    DATE NOT NULL,
    [End]      DATE NOT NULL,
    [Business] BIT  DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK1] FOREIGN KEY ([Alna_num]) REFERENCES [dbo].[Employees] ([Alna_num])
);

CREATE TABLE [dbo].[VehicleHours] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [Alna_num]     INT NOT NULL,
    [Vehicle_ID]   INT NOT NULL,
    [Hours_worked] INT NOT NULL,
    [Date_ID]      INT NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Vehicle_Hours_Alna_num] FOREIGN KEY ([Alna_num]) REFERENCES [dbo].[Employees] ([Alna_num]),
    CONSTRAINT [FK_VehicleHours_Date_ID] FOREIGN KEY ([Date_ID]) REFERENCES [dbo].[Dates] ([ID]),
    CONSTRAINT [FK_Vehicle_Hours_Vehicle_ID] FOREIGN KEY ([Vehicle_ID]) REFERENCES [dbo].[Vehicles] ([ID])
);

-----------------------------------------------------------------------------------------------------------------
-- BEGIN SEEDS --------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------

--Employees
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (172148,	'James Dulgerian',	1,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (172281,	'Xeng Moua',	1,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (172336,	'John Cabigao',	1,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (172363,	'Carlos Velasquez',	1,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (172787,	'Kevin Fang',	1,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (172813,	'Leonel Aguilera',	1,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (172872,	'Daniel Vega',	1,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (172889,	'Nathan Vargo',	1,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (172906,	'Austin Danaj',	0,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (172909,	'Joseph Kielasa',	1,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (172915,	'Osamu Inoue',	1,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (172923,	'Levi Hellebuyck',	0,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (172945,	'Cruz España',	1,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (172947,	'Damien Galloway',	1,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (172981,	'Hugo Moran',	1,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (172990,	'Francesco Parrinelio',	0,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (172991,	'Seth Logan',	0,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (173017,	'Anthony Hewins',	0,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (173018,	'Zarif Ghazi',	0,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (173036,	'Fabrisio Ballo',	0,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (173037,	'Chad Miller',	0,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (173039,	'Benjamin Thelen',	0,	1);
	INSERT into Employees (Alna_num, Name, Full_time, Active) VALUES (173043,	'Luke Pridemore', 0, 1);

	--Projects;
	INSERT into Projects (Name, Category, Active) VALUES ('BLE_PEPS_High_Accuracy',	'A',	1);
	INSERT into Projects (Name, Category, Active) VALUES ('BLE_TPMS',	'A',	1);
	INSERT into Projects (Name, Category, Active) VALUES ('Technology_Research',	'A',	1);
	INSERT into Projects (Name, Category, Active) VALUES ('General_CTB',	'A',	1);
	INSERT into Projects (Name, Category, Active) VALUES ('POC',	'A',	1);
	INSERT into Projects (Name, Category, Active) VALUES ('BLE_Key_Pass_Global_A',	'C',	1);
	INSERT into Projects (Name, Category, Active) VALUES ('BLE_Key_Pass_Global_A_Testing',	'C',	1);
	INSERT into Projects (Name, Category, Active) VALUES ('BLE_Key_Pass_Global_B',	'C',	1);
	INSERT into Projects (Name, Category, Active) VALUES ('BLE_Key_Pass_Autonomus',	'C',	1);
	INSERT into Projects (Name, Category, Active) VALUES ('USRR_Track_3',	'C',	1);
	INSERT into Projects (Name, Category, Active) VALUES ('M2R',	'A',	1);
	INSERT into Projects (Name, Category, Active) VALUES ('MMR',	'A',	1);
	INSERT into Projects (Name, Category, Active) VALUES ('USRR_2',	'A',	1);
	INSERT into Projects (Name, Category, Active) VALUES ('Smart_Thermostat',	'D',	1);
	INSERT into Projects (Name, Category, Active) VALUES ('IR_Transmitter',	'D',	1);
	INSERT into Projects (Name, Category, Active) VALUES ('Multipurpose_Sensor',	'A',	1);
	INSERT into Projects (Name, Category, Active) VALUES ('Vacations',	'B',	1);
	;
	--Phones;
	insert into Phones (Name) values ('iPhone 6-1');
	insert into Phones (Name) values ('iPhone 6P-1');
	insert into Phones (Name) values ('iPhone 5S-1');
	insert into Phones (Name) values ('iPhone 6S-1');
	insert into Phones (Name) values ('iPhone 7-1');
	insert into Phones (Name) values ('iPhone 6S-2');
	insert into Phones (Name) values ('Galaxy S6-1');
	insert into Phones (Name) values ('Galaxy S5-4');
	insert into Phones (Name) values ('Galaxy S5-3');
	insert into Phones (Name) values ('Galaxy S6-2');
	insert into Phones (Name) values ('Galaxy S6-3');
	insert into Phones (Name) values ('Galaxy S5-1');
	insert into Phones (Name) values ('Galaxy S7 Edge-1');
	insert into Phones (Name) values ('Galaxy S7 Edge-2');
	insert into Phones (Name) values ('Galaxy S7 Edge-3');
	insert into Phones (Name) values ('Galaxy S7-1');
	insert into Phones (Name) values ('Galaxy S7-2');
	insert into Phones (Name) values ('Tablet-1');
	insert into Phones (Name) values ('Tablet-2');
	insert into Phones (Name) values ('Big Nexus');
	insert into Phones (Name) values ('Lil Nexus');

	--Accounts;
	insert into Accounts (Accounts.[User], Pass, Admin) values ('User',	'alna', 0);
	insert into Accounts (Accounts.[User], Pass, Admin) values ('Admin', 'alnatest', 1);
	
	---------------------------------------------------------------------------------------------------------------
	--Indices------------------------------------------------------------------------------------------------------
	---------------------------------------------------------------------------------------------------------------
	create index index_latest_date on Dates (Dates desc);
	create index index_this_weeks_proj_hours on ProjectHours (Date_ID desc);
	create index index_this_weeks_vehicle_hours on VehicleHours (Date_ID desc);