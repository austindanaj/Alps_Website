USE [CTBwebsite]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 8/23/2017 10:08:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[User] [nvarchar](10) NOT NULL,
	[Pass] [nvarchar](10) NOT NULL,
	[Admin] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Categories]    Script Date: 8/23/2017 10:08:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[ID] [int] IDENTITY(0,1) NOT NULL,
	[Value] [nchar](18) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dates]    Script Date: 8/23/2017 10:08:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dates](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Dates] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Employees]    Script Date: 8/23/2017 10:08:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[Alna_num] [int] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Full_time] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Alna_num] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[IssueList]    Script Date: 8/23/2017 10:08:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IssueList](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Category] [int] NOT NULL,
	[Proj_ID] [int] NOT NULL,
	[Severity] [bit] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Due_Date] [date] NULL,
	[Status] [int] NOT NULL,
	[Updated] [date] NOT NULL,
	[Reporter] [int] NOT NULL,
	[Assignee] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[Attachment] [varbinary](max) NULL,
	[Filename] [nvarchar](500) NULL,
	[Content_type] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PhoneCheckout]    Script Date: 8/23/2017 10:08:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhoneCheckout](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Phone_ID] [int] NOT NULL,
	[Vehicle_ID] [int] NOT NULL,
	[Alna_num] [int] NOT NULL,
	[Start] [date] NOT NULL,
	[End] [date] NOT NULL,
	[Active] [bit] NOT NULL,
	[Purpose] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Phones]    Script Date: 8/23/2017 10:08:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Phones](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProjectHours]    Script Date: 8/23/2017 10:08:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectHours](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Alna_num] [int] NOT NULL,
	[Proj_ID] [int] NOT NULL,
	[Hours_worked] [int] NOT NULL,
	[Date_ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Projects]    Script Date: 8/23/2017 10:08:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projects](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Category] [nchar](1) NOT NULL,
	[Active] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PurchaseOrders]    Script Date: 8/23/2017 10:08:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseOrders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Alna_num] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Qty] [int] NOT NULL,
	[Description] [text] NOT NULL,
	[Price] [money] NOT NULL,
	[Priority] [int] NOT NULL,
	[Link] [nvarchar](max) NOT NULL,
	[Date_added] [date] NOT NULL,
	[Purchaser] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Severity]    Script Date: 8/23/2017 10:08:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Severity](
	[ID] [bit] NOT NULL,
	[Value] [char](5) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Status]    Script Date: 8/23/2017 10:08:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[ID] [int] IDENTITY(0,1) NOT NULL,
	[Value] [nchar](9) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TimeOff]    Script Date: 8/23/2017 10:08:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TimeOff](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Alna_num] [int] NOT NULL,
	[Start] [date] NOT NULL,
	[End] [date] NOT NULL,
	[Business] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleHours]    Script Date: 8/23/2017 10:08:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleHours](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Alna_num] [int] NOT NULL,
	[Vehicle_ID] [int] NOT NULL,
	[Hours_worked] [int] NOT NULL,
	[Date_ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Vehicles]    Script Date: 8/23/2017 10:08:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehicles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Active] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Accounts] ON 

INSERT [dbo].[Accounts] ([ID], [User], [Pass], [Admin]) VALUES (1, N'User', N'alna', 0)
INSERT [dbo].[Accounts] ([ID], [User], [Pass], [Admin]) VALUES (2, N'Admin', N'alnatest', 1)
SET IDENTITY_INSERT [dbo].[Accounts] OFF
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([ID], [Value]) VALUES (0, N'1: Inquiry/Request')
INSERT [dbo].[Categories] ([ID], [Value]) VALUES (1, N'2: Change Request ')
INSERT [dbo].[Categories] ([ID], [Value]) VALUES (2, N'3: Problem        ')
INSERT [dbo].[Categories] ([ID], [Value]) VALUES (3, N'4: Memo           ')
SET IDENTITY_INSERT [dbo].[Categories] OFF
SET IDENTITY_INSERT [dbo].[Dates] ON 

INSERT [dbo].[Dates] ([ID], [Dates]) VALUES (10, CAST(N'2017-08-21' AS Date))
INSERT [dbo].[Dates] ([ID], [Dates]) VALUES (9, CAST(N'2017-08-14' AS Date))
INSERT [dbo].[Dates] ([ID], [Dates]) VALUES (8, CAST(N'2017-08-07' AS Date))
INSERT [dbo].[Dates] ([ID], [Dates]) VALUES (7, CAST(N'2017-07-31' AS Date))
INSERT [dbo].[Dates] ([ID], [Dates]) VALUES (6, CAST(N'2017-07-24' AS Date))
INSERT [dbo].[Dates] ([ID], [Dates]) VALUES (5, CAST(N'2017-07-17' AS Date))
INSERT [dbo].[Dates] ([ID], [Dates]) VALUES (4, CAST(N'2017-07-10' AS Date))
INSERT [dbo].[Dates] ([ID], [Dates]) VALUES (3, CAST(N'2017-07-03' AS Date))
INSERT [dbo].[Dates] ([ID], [Dates]) VALUES (2, CAST(N'2017-06-26' AS Date))
INSERT [dbo].[Dates] ([ID], [Dates]) VALUES (1, CAST(N'2017-06-19' AS Date))
SET IDENTITY_INSERT [dbo].[Dates] OFF
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (172148, N'James Dulgerian', 1, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (172281, N'Xeng Moua', 1, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (172336, N'John Cabigao', 1, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (172363, N'Carlos Velasquez', 1, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (172787, N'Kevin Fang', 1, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (172813, N'Leonel Aguilera', 1, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (172872, N'Daniel Vega', 1, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (172889, N'Nathan Vargo', 1, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (172906, N'Austin Danaj', 0, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (172909, N'Joseph Kielasa', 1, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (172915, N'Osamu Inoue', 1, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (172923, N'Levi Hellebuyck', 0, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (172945, N'Cruz España', 1, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (172947, N'Damien Galloway', 1, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (172981, N'Hugo Moran', 1, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (172990, N'Francesco Parrinelio', 0, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (172991, N'Seth Logan', 0, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (173017, N'Anthony Hewins', 0, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (173018, N'Zarif Ghazi', 0, 0)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (173034, N'Maher Zibdawi', 1, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (173036, N'Fabrisio Ballo', 0, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (173037, N'Chad Miller', 0, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (173039, N'Benjamin Thelen', 0, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (173043, N'Luke Pridemore', 0, 1)
INSERT [dbo].[Employees] ([Alna_num], [Name], [Full_time], [Active]) VALUES (173046, N'Pratik Manandhar', 1, 1)
SET IDENTITY_INSERT [dbo].[IssueList] ON 

INSERT [dbo].[IssueList] ([Id], [Category], [Proj_ID], [Severity], [Title], [Due_Date], [Status], [Updated], [Reporter], [Assignee], [Active], [Description], [Comment], [Attachment], [Filename], [Content_type]) VALUES (3, 0, 1, 0, N'Test 2', NULL, 0, CAST(N'2017-08-22' AS Date), 172906, 172947, 1, N'', NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[IssueList] OFF
SET IDENTITY_INSERT [dbo].[PhoneCheckout] ON 

INSERT [dbo].[PhoneCheckout] ([ID], [Phone_ID], [Vehicle_ID], [Alna_num], [Start], [End], [Active], [Purpose]) VALUES (1, 16, 7, 172906, CAST(N'2017-08-22' AS Date), CAST(N'2017-08-27' AS Date), 0, N'Leakage Range ')
SET IDENTITY_INSERT [dbo].[PhoneCheckout] OFF
SET IDENTITY_INSERT [dbo].[Phones] ON 

INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (1, N'iPhone 6-1', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (2, N'iPhone 6P-1', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (3, N'iPhone 5S-1', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (4, N'iPhone 6S-1', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (5, N'iPhone 7-1', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (6, N'iPhone 6S-2', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (7, N'Galaxy S6-1', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (8, N'Galaxy S5-4', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (9, N'Galaxy S5-3', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (10, N'Galaxy S6-2', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (11, N'Galaxy S6-3', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (12, N'Galaxy S5-1', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (13, N'Galaxy S7 Edge-1', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (14, N'Galaxy S7 Edge-2', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (15, N'Galaxy S7 Edge-3', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (16, N'Galaxy S7-1', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (17, N'Galaxy S7-2', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (18, N'Tablet-1', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (19, N'Tablet-2', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (20, N'Big Nexus', 1)
INSERT [dbo].[Phones] ([Id], [Name], [Active]) VALUES (21, N'Lil Nexus', 1)
SET IDENTITY_INSERT [dbo].[Phones] OFF
SET IDENTITY_INSERT [dbo].[ProjectHours] ON 

INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (2, 173017, 4, 39, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (3, 173017, 17, 1, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (5, 172923, 5, 27, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (6, 172990, 5, 11, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (7, 172923, 4, 4, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (8, 172923, 15, 5, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (9, 172923, 17, 4, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (10, 172990, 1, 20, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (11, 172990, 17, 9, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (12, 172991, 6, 35, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (13, 172991, 17, 5, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (14, 172363, 2, 2, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (16, 172363, 10, 16, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (18, 172363, 1, 2, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (19, 172363, 6, 8, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (20, 172363, 17, 8, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (21, 172363, 4, 2, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (22, 172363, 15, 2, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (24, 172981, 17, 24, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (25, 172981, 6, 16, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (27, 173037, 6, 8, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (28, 173043, 7, 9, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (29, 173037, 7, 8, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (30, 173037, 1, 2, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (31, 173037, 4, 14, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (32, 173043, 1, 1, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (33, 173043, 4, 29, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (34, 173034, 13, 40, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (35, 172787, 6, 32, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (36, 172787, 6, 32, 1)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (37, 172787, 17, 8, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (38, 172906, 6, 39, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (39, 172906, 17, 1, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (40, 172872, 4, 32, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (41, 172872, 3, 6, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (42, 172872, 15, 2, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (43, 172923, 5, 27, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (44, 172923, 8, 6, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (46, 172923, 4, 6, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (47, 172923, 17, 1, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (48, 172990, 1, 15, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (49, 172990, 5, 6, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (50, 172990, 17, 19, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (51, 172947, 17, 40, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (54, 172909, 3, 1, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (55, 172909, 4, 8, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (56, 172909, 1, 31, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (57, 173017, 17, 1, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (58, 173017, 6, 39, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (59, 173039, 6, 6, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (60, 173039, 7, 14, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (61, 173039, 4, 15, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (62, 173018, 6, 17, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (63, 173018, 4, 5, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (65, 173036, 6, 9, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (66, 173036, 7, 23, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (67, 173036, 1, 2, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (68, 173036, 4, 2, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (69, 173036, 17, 4, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (70, 173017, 17, 17, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (71, 173017, 4, 23, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (72, 173043, 4, 6, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (74, 173043, 6, 5, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (76, 173043, 6, 4, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (77, 173043, 4, 6, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (78, 173037, 4, 6, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (79, 173037, 7, 18, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (80, 173037, 6, 5, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (81, 173037, 7, 17, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (82, 173037, 1, 2, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (83, 173037, 4, 15, 2)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (84, 173036, 6, 6, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (85, 173036, 7, 14, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (86, 173036, 17, 20, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (89, 172947, 17, 40, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (90, 172906, 6, 28, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (91, 172906, 17, 12, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (93, 173043, 4, 7, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (94, 172923, 5, 26, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (95, 172923, 12, 2, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (96, 172923, 17, 12, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (97, 173036, 4, 15, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (99, 173036, 1, 1, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (101, 173036, 17, 14, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (106, 172872, 17, 16, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (107, 172872, 15, 4, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (108, 172872, 4, 20, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (109, 172872, 15, 16, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (110, 172872, 3, 8, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (111, 172872, 4, 16, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (113, 173017, 6, 35, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (114, 173017, 17, 5, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (115, 173017, 6, 39, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (116, 173017, 17, 1, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (117, 172906, 6, 39, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (118, 172906, 17, 1, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (119, 172906, 6, 39, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (120, 172906, 17, 1, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (121, 172906, 1, 39, 6)
GO
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (122, 172906, 17, 1, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (123, 172923, 17, 40, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (128, 172923, 17, 12, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (129, 172923, 4, 8, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (130, 172923, 5, 20, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (131, 172923, 5, 8, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (132, 172923, 1, 31, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (133, 172923, 17, 1, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (134, 172889, 17, 24, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (135, 172889, 15, 12, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (136, 172889, 14, 4, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (138, 172889, 14, 8, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (139, 172889, 2, 2, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (140, 172889, 3, 2, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (141, 172889, 15, 28, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (142, 172889, 14, 16, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (143, 172889, 15, 18, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (144, 172889, 2, 6, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (146, 172990, 17, 40, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (147, 172990, 17, 40, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (148, 173043, 7, 12, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (151, 172990, 1, 30, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (152, 172990, 4, 2, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (154, 172990, 17, 8, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (157, 172990, 1, 14, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (158, 173037, 7, 4, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (159, 172990, 4, 20, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (160, 173037, 6, 12, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (161, 172990, 17, 6, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (162, 173037, 17, 24, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (164, 173037, 7, 24, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (166, 173037, 4, 7, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (167, 173037, 17, 9, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (168, 173043, 17, 18, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (169, 173043, 7, 20, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (170, 173043, 6, 12, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (171, 173043, 17, 1, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (172, 173043, 6, 7, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (173, 172872, 13, 15, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (174, 173043, 7, 14, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (175, 173043, 4, 15, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (176, 173043, 17, 4, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (177, 172872, 2, 15, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (178, 172872, 4, 10, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (179, 173043, 6, 16, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (180, 173043, 4, 1, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (181, 173043, 17, 1, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (182, 173043, 7, 22, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (183, 172872, 13, 15, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (184, 172872, 2, 15, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (185, 172872, 4, 10, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (188, 172991, 6, 5, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (189, 172991, 7, 12, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (190, 172991, 17, 23, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (191, 172991, 6, 25, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (192, 172991, 7, 2, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (193, 172991, 17, 13, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (194, 172991, 6, 36, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (195, 172991, 17, 4, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (197, 172991, 7, 7, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (198, 172991, 6, 30, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (199, 172991, 17, 3, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (201, 173039, 6, 6, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (202, 173039, 7, 12, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (203, 173039, 7, 19, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (204, 173039, 6, 11, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (205, 173039, 4, 6, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (206, 173039, 17, 4, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (207, 173039, 4, 2, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (208, 173039, 17, 20, 3)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (209, 173039, 4, 10, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (210, 173039, 6, 7, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (211, 173039, 7, 14, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (213, 173039, 17, 9, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (214, 173039, 7, 18, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (215, 173039, 6, 16, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (216, 173039, 4, 1, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (217, 173039, 17, 5, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (218, 173017, 6, 24, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (219, 173017, 4, 15, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (220, 173017, 17, 1, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (221, 173036, 7, 5, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (222, 173036, 6, 5, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (223, 173036, 6, 3, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (226, 173036, 4, 21, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (227, 173036, 17, 19, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (228, 173036, 4, 21, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (229, 173036, 17, 16, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (230, 172148, 15, 32, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (231, 172148, 14, 8, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (236, 172947, 17, 10, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (237, 172947, 5, 20, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (238, 172947, 1, 10, 4)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (239, 172947, 1, 20, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (240, 172947, 5, 8, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (241, 172947, 4, 10, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (242, 172947, 17, 2, 5)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (243, 172947, 1, 21, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (244, 172947, 5, 15, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (245, 173037, 7, 12, 6)
GO
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (246, 173037, 6, 15, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (247, 173037, 17, 1, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (248, 173037, 4, 12, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (249, 172787, 6, 36, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (250, 172787, 17, 4, 6)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (251, 172945, 6, 32, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (252, 172945, 8, 8, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (253, 172813, 17, 16, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (254, 172813, 6, 12, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (255, 172813, 7, 12, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (256, 173017, 7, 3, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (257, 173017, 4, 36, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (259, 173037, 6, 5, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (260, 173036, 17, 13, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (261, 173017, 17, 1, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (262, 173037, 7, 12, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (263, 173037, 4, 22, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (264, 173037, 17, 1, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (265, 173036, 4, 21, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (268, 173036, 6, 4, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (269, 173036, 7, 2, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (270, 173017, 7, 32, 8)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (271, 173017, 4, 7, 8)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (272, 173017, 17, 1, 8)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (273, 173036, 7, 35, 8)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (274, 173036, 17, 5, 8)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (275, 173039, 7, 9, 8)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (277, 173039, 6, 5, 8)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (278, 173039, 17, 26, 8)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (279, 172990, 1, 31, 8)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (280, 172990, 5, 4, 8)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (281, 172990, 17, 5, 8)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (282, 172990, 1, 29, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (283, 172990, 5, 5, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (284, 172990, 17, 6, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (285, 172872, 1, 20, 8)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (286, 172872, 13, 20, 8)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (287, 172872, 15, 8, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (289, 172872, 4, 10, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (290, 172872, 13, 22, 7)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (291, 173046, 8, 40, 8)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (292, 172923, 17, 10, 8)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (294, 172923, 4, 5, 8)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (295, 172923, 5, 25, 8)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (297, 173017, 4, 38, 9)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (298, 172923, 5, 36, 9)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (299, 172923, 17, 4, 9)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (300, 173017, 7, 1, 9)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (301, 173017, 17, 1, 9)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (302, 173037, 4, 16, 9)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (303, 173037, 7, 23, 9)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (304, 173037, 17, 1, 9)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (306, 173036, 7, 9, 9)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (307, 173036, 6, 10, 9)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (308, 173036, 4, 3, 9)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (309, 173036, 14, 3, 9)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (310, 173036, 17, 15, 9)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (311, 172990, 1, 20, 9)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (312, 172990, 5, 8, 9)
INSERT [dbo].[ProjectHours] ([ID], [Alna_num], [Proj_ID], [Hours_worked], [Date_ID]) VALUES (313, 172990, 17, 12, 9)
SET IDENTITY_INSERT [dbo].[ProjectHours] OFF
SET IDENTITY_INSERT [dbo].[Projects] ON 

INSERT [dbo].[Projects] ([ID], [Name], [Category], [Active]) VALUES (1, N'BLE_PEPS_High_Accuracy', N'A', 1)
INSERT [dbo].[Projects] ([ID], [Name], [Category], [Active]) VALUES (2, N'BLE_TPMS', N'A', 1)
INSERT [dbo].[Projects] ([ID], [Name], [Category], [Active]) VALUES (3, N'Technology_Research', N'A', 1)
INSERT [dbo].[Projects] ([ID], [Name], [Category], [Active]) VALUES (4, N'General_CTB', N'A', 1)
INSERT [dbo].[Projects] ([ID], [Name], [Category], [Active]) VALUES (5, N'POC', N'A', 1)
INSERT [dbo].[Projects] ([ID], [Name], [Category], [Active]) VALUES (6, N'BLE_Key_Pass_Global_A', N'C', 1)
INSERT [dbo].[Projects] ([ID], [Name], [Category], [Active]) VALUES (7, N'BLE_Key_Pass_Global_A_Testing', N'C', 1)
INSERT [dbo].[Projects] ([ID], [Name], [Category], [Active]) VALUES (8, N'BLE_Key_Pass_Global_B', N'C', 1)
INSERT [dbo].[Projects] ([ID], [Name], [Category], [Active]) VALUES (9, N'BLE_Key_Pass_Autonomus', N'C', 1)
INSERT [dbo].[Projects] ([ID], [Name], [Category], [Active]) VALUES (10, N'USRR_Track_3', N'C', 1)
INSERT [dbo].[Projects] ([ID], [Name], [Category], [Active]) VALUES (11, N'M2R', N'A', 1)
INSERT [dbo].[Projects] ([ID], [Name], [Category], [Active]) VALUES (12, N'MMR', N'A', 1)
INSERT [dbo].[Projects] ([ID], [Name], [Category], [Active]) VALUES (13, N'USRR_2', N'A', 1)
INSERT [dbo].[Projects] ([ID], [Name], [Category], [Active]) VALUES (14, N'Smart_Thermostat', N'D', 1)
INSERT [dbo].[Projects] ([ID], [Name], [Category], [Active]) VALUES (15, N'IR_Transmitter', N'D', 1)
INSERT [dbo].[Projects] ([ID], [Name], [Category], [Active]) VALUES (16, N'Multipurpose_Sensor', N'A', 1)
INSERT [dbo].[Projects] ([ID], [Name], [Category], [Active]) VALUES (17, N'Vacations', N'B', 1)
SET IDENTITY_INSERT [dbo].[Projects] OFF
SET IDENTITY_INSERT [dbo].[PurchaseOrders] ON 

INSERT [dbo].[PurchaseOrders] ([ID], [Alna_num], [Name], [Qty], [Description], [Price], [Priority], [Link], [Date_added], [Purchaser]) VALUES (1, 172906, N'Mini DisplayPort to HDMI', 1, N'converts mini display port to vga, hdmi, or dvi', 12.9900, 1, N'https://www.amazon.com/DisplayPort-Thunderbolt-FOINNEX-Converter-ThinkPad/dp/B01GX7WP56/ref=sr_1_21?ie=UTF8&qid=1498138452&sr=8-21&keywords=mini+display+to+hdmi+and+vga', CAST(N'2017-06-22' AS Date), 172923)
INSERT [dbo].[PurchaseOrders] ([ID], [Alna_num], [Name], [Qty], [Description], [Price], [Priority], [Link], [Date_added], [Purchaser]) VALUES (3, 172872, N'Automotive Latex Gloves', 1, N'Automotive Latex Gloves; 1 box; Medium size', 11.9100, 1, N'https://www.amazon.com/AMMEX-GPNB46100-BX-GlovePlus-Disposable-Industrial/dp/B004BR571K/ref=sr_1_4?ie=UTF8&qid=1500320247&sr=8-4&keywords=automotive%2Blatex%2Bgloves&th=1', CAST(N'2017-07-17' AS Date), 172872)
SET IDENTITY_INSERT [dbo].[PurchaseOrders] OFF
INSERT [dbo].[Severity] ([ID], [Value]) VALUES (0, N'Minor')
INSERT [dbo].[Severity] ([ID], [Value]) VALUES (1, N'Major')
SET IDENTITY_INSERT [dbo].[Status] ON 

INSERT [dbo].[Status] ([ID], [Value]) VALUES (0, N'Initial  ')
INSERT [dbo].[Status] ([ID], [Value]) VALUES (1, N'Analysis ')
INSERT [dbo].[Status] ([ID], [Value]) VALUES (2, N'Completed')
SET IDENTITY_INSERT [dbo].[Status] OFF
SET IDENTITY_INSERT [dbo].[TimeOff] ON 

INSERT [dbo].[TimeOff] ([ID], [Alna_num], [Start], [End], [Business]) VALUES (3, 172363, CAST(N'2017-07-05' AS Date), CAST(N'2017-07-07' AS Date), 0)
INSERT [dbo].[TimeOff] ([ID], [Alna_num], [Start], [End], [Business]) VALUES (5, 172981, CAST(N'2017-06-22' AS Date), CAST(N'2017-06-23' AS Date), 0)
INSERT [dbo].[TimeOff] ([ID], [Alna_num], [Start], [End], [Business]) VALUES (7, 172906, CAST(N'2017-06-23' AS Date), CAST(N'2017-06-23' AS Date), 0)
INSERT [dbo].[TimeOff] ([ID], [Alna_num], [Start], [End], [Business]) VALUES (8, 172148, CAST(N'2017-06-23' AS Date), CAST(N'2017-07-14' AS Date), 0)
INSERT [dbo].[TimeOff] ([ID], [Alna_num], [Start], [End], [Business]) VALUES (9, 173037, CAST(N'2017-07-13' AS Date), CAST(N'2017-07-14' AS Date), 0)
INSERT [dbo].[TimeOff] ([ID], [Alna_num], [Start], [End], [Business]) VALUES (11, 173037, CAST(N'2017-08-10' AS Date), CAST(N'2017-08-11' AS Date), 0)
INSERT [dbo].[TimeOff] ([ID], [Alna_num], [Start], [End], [Business]) VALUES (12, 172909, CAST(N'2017-07-05' AS Date), CAST(N'2017-07-07' AS Date), 0)
INSERT [dbo].[TimeOff] ([ID], [Alna_num], [Start], [End], [Business]) VALUES (13, 172909, CAST(N'2017-08-18' AS Date), CAST(N'2017-08-25' AS Date), 0)
INSERT [dbo].[TimeOff] ([ID], [Alna_num], [Start], [End], [Business]) VALUES (14, 173039, CAST(N'2017-08-14' AS Date), CAST(N'2017-08-18' AS Date), 0)
INSERT [dbo].[TimeOff] ([ID], [Alna_num], [Start], [End], [Business]) VALUES (16, 172923, CAST(N'2017-08-25' AS Date), CAST(N'2017-08-25' AS Date), 0)
INSERT [dbo].[TimeOff] ([ID], [Alna_num], [Start], [End], [Business]) VALUES (17, 173037, CAST(N'2017-08-21' AS Date), CAST(N'2017-08-21' AS Date), 0)
SET IDENTITY_INSERT [dbo].[TimeOff] OFF
SET IDENTITY_INSERT [dbo].[VehicleHours] ON 

INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (1, 172991, 3, 6, 1)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (2, 172991, 5, 2, 1)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (3, 172991, 1, 2, 1)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (4, 173043, 1, 5, 1)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (5, 173037, 1, 5, 1)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (6, 173037, 5, 3, 1)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (7, 173043, 5, 1, 1)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (8, 173043, 3, 3, 1)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (9, 173039, 1, 14, 2)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (10, 173018, 3, 12, 2)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (11, 173018, 5, 5, 2)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (12, 173036, 5, 12, 2)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (13, 173036, 3, 6, 2)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (14, 173043, 1, 24, 2)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (16, 173043, 1, 12, 3)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (17, 173037, 3, 3, 3)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (18, 173037, 5, 15, 3)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (19, 173037, 5, 9, 2)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (20, 173037, 1, 3, 2)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (21, 173037, 3, 5, 2)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (22, 173036, 3, 11, 3)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (23, 173036, 5, 3, 3)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (24, 173043, 1, 19, 4)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (25, 173043, 5, 1, 4)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (27, 173036, 5, 3, 4)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (33, 173037, 5, 4, 4)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (34, 173037, 5, 15, 5)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (35, 173037, 1, 9, 5)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (36, 173043, 1, 12, 5)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (37, 173043, 5, 2, 5)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (38, 173043, 1, 20, 6)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (39, 173043, 5, 2, 6)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (41, 172991, 3, 12, 3)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (42, 172991, 3, 2, 4)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (43, 172991, 1, 7, 6)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (45, 173039, 1, 12, 3)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (47, 173039, 1, 19, 4)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (48, 173039, 1, 12, 5)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (49, 173039, 1, 20, 6)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (51, 173036, 3, 2, 4)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (52, 173037, 5, 10, 6)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (53, 173037, 1, 2, 6)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (54, 173017, 1, 3, 7)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (55, 173017, 5, 2, 7)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (56, 173037, 1, 9, 7)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (57, 173037, 5, 3, 7)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (59, 173036, 5, 2, 7)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (60, 173017, 2, 32, 8)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (61, 173036, 6, 3, 8)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (62, 173036, 2, 32, 8)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (63, 173039, 1, 9, 8)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (64, 173017, 1, 1, 9)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (65, 173037, 1, 18, 9)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (66, 173037, 6, 5, 9)
INSERT [dbo].[VehicleHours] ([ID], [Alna_num], [Vehicle_ID], [Hours_worked], [Date_ID]) VALUES (67, 173036, 1, 9, 9)
SET IDENTITY_INSERT [dbo].[VehicleHours] OFF
SET IDENTITY_INSERT [dbo].[Vehicles] ON 

INSERT [dbo].[Vehicles] ([ID], [Name], [Active]) VALUES (1, N'M2JC_SPARK', 1)
INSERT [dbo].[Vehicles] ([ID], [Name], [Active]) VALUES (2, N'M2JC_SPARK_EV', 1)
INSERT [dbo].[Vehicles] ([ID], [Name], [Active]) VALUES (3, N'D2JCI_VOLT', 1)
INSERT [dbo].[Vehicles] ([ID], [Name], [Active]) VALUES (4, N'G1UC_TRAX', 1)
INSERT [dbo].[Vehicles] ([ID], [Name], [Active]) VALUES (5, N'K2UC_TAHOE', 1)
INSERT [dbo].[Vehicles] ([ID], [Name], [Active]) VALUES (6, N'G2KCA_BOLT_AV', 1)
INSERT [dbo].[Vehicles] ([ID], [Name], [Active]) VALUES (7, N'D2LC_CRUZE', 1)
INSERT [dbo].[Vehicles] ([ID], [Name], [Active]) VALUES (8, N'D2UC_EQUINOX', 1)
INSERT [dbo].[Vehicles] ([ID], [Name], [Active]) VALUES (9, N'A2LL_CTS', 1)
SET IDENTITY_INSERT [dbo].[Vehicles] OFF
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT ((0)) FOR [Admin]
GO
ALTER TABLE [dbo].[Employees] ADD  DEFAULT ((0)) FOR [Full_time]
GO
ALTER TABLE [dbo].[Employees] ADD  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[IssueList] ADD  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[PhoneCheckout] ADD  DEFAULT (getdate()) FOR [Start]
GO
ALTER TABLE [dbo].[PhoneCheckout] ADD  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[Phones] ADD  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[ProjectHours] ADD  DEFAULT ((0)) FOR [Hours_worked]
GO
ALTER TABLE [dbo].[Projects] ADD  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[PurchaseOrders] ADD  DEFAULT ((1)) FOR [Qty]
GO
ALTER TABLE [dbo].[PurchaseOrders] ADD  DEFAULT ((1)) FOR [Priority]
GO
ALTER TABLE [dbo].[PurchaseOrders] ADD  DEFAULT (NULL) FOR [Purchaser]
GO
ALTER TABLE [dbo].[TimeOff] ADD  DEFAULT ((0)) FOR [Alna_num]
GO
ALTER TABLE [dbo].[TimeOff] ADD  DEFAULT ((0)) FOR [Business]
GO
ALTER TABLE [dbo].[Vehicles] ADD  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[IssueList]  WITH CHECK ADD  CONSTRAINT [FK_Assignee_to_alna] FOREIGN KEY([Assignee])
REFERENCES [dbo].[Employees] ([Alna_num])
GO
ALTER TABLE [dbo].[IssueList] CHECK CONSTRAINT [FK_Assignee_to_alna]
GO
ALTER TABLE [dbo].[IssueList]  WITH CHECK ADD  CONSTRAINT [FK_Categories_to_Constants] FOREIGN KEY([Category])
REFERENCES [dbo].[Categories] ([ID])
GO
ALTER TABLE [dbo].[IssueList] CHECK CONSTRAINT [FK_Categories_to_Constants]
GO
ALTER TABLE [dbo].[IssueList]  WITH CHECK ADD  CONSTRAINT [FK_Reporter_to_alna] FOREIGN KEY([Reporter])
REFERENCES [dbo].[Employees] ([Alna_num])
GO
ALTER TABLE [dbo].[IssueList] CHECK CONSTRAINT [FK_Reporter_to_alna]
GO
ALTER TABLE [dbo].[IssueList]  WITH CHECK ADD  CONSTRAINT [FK_Severity_to_Constants] FOREIGN KEY([Severity])
REFERENCES [dbo].[Severity] ([ID])
GO
ALTER TABLE [dbo].[IssueList] CHECK CONSTRAINT [FK_Severity_to_Constants]
GO
ALTER TABLE [dbo].[IssueList]  WITH CHECK ADD  CONSTRAINT [FK_Status_to_Constants] FOREIGN KEY([Status])
REFERENCES [dbo].[Status] ([ID])
GO
ALTER TABLE [dbo].[IssueList] CHECK CONSTRAINT [FK_Status_to_Constants]
GO
ALTER TABLE [dbo].[PhoneCheckout]  WITH CHECK ADD  CONSTRAINT [FK_PhoneCheckout_Employees] FOREIGN KEY([Alna_num])
REFERENCES [dbo].[Employees] ([Alna_num])
GO
ALTER TABLE [dbo].[PhoneCheckout] CHECK CONSTRAINT [FK_PhoneCheckout_Employees]
GO
ALTER TABLE [dbo].[PhoneCheckout]  WITH CHECK ADD  CONSTRAINT [FK_PhoneCheckout_Phones] FOREIGN KEY([Phone_ID])
REFERENCES [dbo].[Phones] ([Id])
GO
ALTER TABLE [dbo].[PhoneCheckout] CHECK CONSTRAINT [FK_PhoneCheckout_Phones]
GO
ALTER TABLE [dbo].[PhoneCheckout]  WITH CHECK ADD  CONSTRAINT [FK_PhoneCheckout_Vehicle_ID] FOREIGN KEY([Vehicle_ID])
REFERENCES [dbo].[Vehicles] ([ID])
GO
ALTER TABLE [dbo].[PhoneCheckout] CHECK CONSTRAINT [FK_PhoneCheckout_Vehicle_ID]
GO
ALTER TABLE [dbo].[ProjectHours]  WITH CHECK ADD  CONSTRAINT [FK_Proj_Hours_Alna_num] FOREIGN KEY([Alna_num])
REFERENCES [dbo].[Employees] ([Alna_num])
GO
ALTER TABLE [dbo].[ProjectHours] CHECK CONSTRAINT [FK_Proj_Hours_Alna_num]
GO
ALTER TABLE [dbo].[ProjectHours]  WITH CHECK ADD  CONSTRAINT [FK_Proj_Hours_Date_ID] FOREIGN KEY([Date_ID])
REFERENCES [dbo].[Dates] ([ID])
GO
ALTER TABLE [dbo].[ProjectHours] CHECK CONSTRAINT [FK_Proj_Hours_Date_ID]
GO
ALTER TABLE [dbo].[ProjectHours]  WITH CHECK ADD  CONSTRAINT [FK_Proj_Hours_Proj_ID] FOREIGN KEY([Proj_ID])
REFERENCES [dbo].[Projects] ([ID])
GO
ALTER TABLE [dbo].[ProjectHours] CHECK CONSTRAINT [FK_Proj_Hours_Proj_ID]
GO
ALTER TABLE [dbo].[PurchaseOrders]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseOrder_Creator] FOREIGN KEY([Alna_num])
REFERENCES [dbo].[Employees] ([Alna_num])
GO
ALTER TABLE [dbo].[PurchaseOrders] CHECK CONSTRAINT [FK_PurchaseOrder_Creator]
GO
ALTER TABLE [dbo].[PurchaseOrders]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseOrder_Purchaser] FOREIGN KEY([Purchaser])
REFERENCES [dbo].[Employees] ([Alna_num])
GO
ALTER TABLE [dbo].[PurchaseOrders] CHECK CONSTRAINT [FK_PurchaseOrder_Purchaser]
GO
ALTER TABLE [dbo].[TimeOff]  WITH CHECK ADD  CONSTRAINT [FK1] FOREIGN KEY([Alna_num])
REFERENCES [dbo].[Employees] ([Alna_num])
GO
ALTER TABLE [dbo].[TimeOff] CHECK CONSTRAINT [FK1]
GO
ALTER TABLE [dbo].[VehicleHours]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_Hours_Alna_num] FOREIGN KEY([Alna_num])
REFERENCES [dbo].[Employees] ([Alna_num])
GO
ALTER TABLE [dbo].[VehicleHours] CHECK CONSTRAINT [FK_Vehicle_Hours_Alna_num]
GO
ALTER TABLE [dbo].[VehicleHours]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_Hours_Vehicle_ID] FOREIGN KEY([Vehicle_ID])
REFERENCES [dbo].[Vehicles] ([ID])
GO
ALTER TABLE [dbo].[VehicleHours] CHECK CONSTRAINT [FK_Vehicle_Hours_Vehicle_ID]
GO
ALTER TABLE [dbo].[VehicleHours]  WITH CHECK ADD  CONSTRAINT [FK_VehicleHours_Date_ID] FOREIGN KEY([Date_ID])
REFERENCES [dbo].[Dates] ([ID])
GO
ALTER TABLE [dbo].[VehicleHours] CHECK CONSTRAINT [FK_VehicleHours_Date_ID]
GO
/****** Object:  Trigger [dbo].[Update_Employee]    Script Date: 8/23/2017 10:08:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
GO
ALTER TABLE [dbo].[Employees] ENABLE TRIGGER [Update_Employee]
GO
