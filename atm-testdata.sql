USE [AtmDb]
GO
SET IDENTITY_INSERT [dbo].[CardHolders] ON 
GO
INSERT [dbo].[CardHolders] ([Id], [Name], [PIN]) VALUES (1, N'Matias', N'$2b$10$jN4P6ptgmEgcgCBwVmw9j.4Wlo0pytIyztTeEkmpvA5f8.mP/L65q')
GO
INSERT [dbo].[CardHolders] ([Id], [Name], [PIN]) VALUES (2, N'Nicolas', N'$2b$10$LtxwcRDMaqLZHOqWZqOpRuTeKMOl4IAK6b8/1xJvIsGVUw2C/pR8W
')
GO
INSERT [dbo].[CardHolders] ([Id], [Name], [PIN]) VALUES (5, N'Gustavo', N'$2b$10$LtxwcRDMaqLZHOqWZqOpRuTeKMOl4IAK6b8/1xJvIsGVUw2C/pR8W
')
GO
INSERT [dbo].[CardHolders] ([Id], [Name], [PIN]) VALUES (6, N'Marcelo', N'$2b$10$LtxwcRDMaqLZHOqWZqOpRuTeKMOl4IAK6b8/1xJvIsGVUw2C/pR8W
')
GO
SET IDENTITY_INSERT [dbo].[CardHolders] OFF
GO
SET IDENTITY_INSERT [dbo].[Cards] ON 
GO
INSERT [dbo].[Cards] ([Id], [Number], [Status], [CardHolderId]) VALUES (1, N'1234567891234567', 0, 1)
GO
INSERT [dbo].[Cards] ([Id], [Number], [Status], [CardHolderId]) VALUES (2, N'4537584930586956', 1, 2)
GO
INSERT [dbo].[Cards] ([Id], [Number], [Status], [CardHolderId]) VALUES (5, N'3464758349583942', 0, 5)
GO
INSERT [dbo].[Cards] ([Id], [Number], [Status], [CardHolderId]) VALUES (6, N'4555095483495834', 0, 6)
GO
SET IDENTITY_INSERT [dbo].[Cards] OFF
GO
SET IDENTITY_INSERT [dbo].[Accounts] ON 
GO
INSERT [dbo].[Accounts] ([Id], [Number], [Balance], [CardId]) VALUES (1, N'0220434556', CAST(398768.00 AS Decimal(18, 2)), 1)
GO
SET IDENTITY_INSERT [dbo].[Accounts] OFF
GO
SET IDENTITY_INSERT [dbo].[Transactions] ON 
GO
INSERT [dbo].[Transactions] ([Id], [Amount], [Date], [CardId]) VALUES (1, CAST(30000.00 AS Decimal(18, 2)), CAST(N'2024-12-12T00:00:00.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Transactions] ([Id], [Amount], [Date], [CardId]) VALUES (3, CAST(15000.00 AS Decimal(18, 2)), CAST(N'2024-12-15T00:00:00.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Transactions] ([Id], [Amount], [Date], [CardId]) VALUES (4, CAST(3000.00 AS Decimal(18, 2)), CAST(N'2024-11-04T00:00:00.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Transactions] ([Id], [Amount], [Date], [CardId]) VALUES (5, CAST(550.00 AS Decimal(18, 2)), CAST(N'2024-07-23T00:00:00.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Transactions] ([Id], [Amount], [Date], [CardId]) VALUES (6, CAST(500.00 AS Decimal(18, 2)), CAST(N'2024-11-19T14:53:35.6513285' AS DateTime2), 1)
GO
INSERT [dbo].[Transactions] ([Id], [Amount], [Date], [CardId]) VALUES (8, CAST(350.00 AS Decimal(18, 2)), CAST(N'2024-11-13T00:00:00.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Transactions] ([Id], [Amount], [Date], [CardId]) VALUES (9, CAST(200.00 AS Decimal(18, 2)), CAST(N'2024-11-13T00:00:00.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Transactions] ([Id], [Amount], [Date], [CardId]) VALUES (10, CAST(235.00 AS Decimal(18, 2)), CAST(N'2024-11-13T00:00:00.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Transactions] ([Id], [Amount], [Date], [CardId]) VALUES (11, CAST(1000.00 AS Decimal(18, 2)), CAST(N'2024-11-13T00:00:00.0000000' AS DateTime2), 1)
GO
INSERT [dbo].[Transactions] ([Id], [Amount], [Date], [CardId]) VALUES (1002, CAST(300.00 AS Decimal(18, 2)), CAST(N'2024-11-21T14:01:08.6723976' AS DateTime2), 1)
GO
INSERT [dbo].[Transactions] ([Id], [Amount], [Date], [CardId]) VALUES (1003, CAST(432.00 AS Decimal(18, 2)), CAST(N'2024-11-22T13:01:12.1799268' AS DateTime2), 1)
GO
SET IDENTITY_INSERT [dbo].[Transactions] OFF
GO
