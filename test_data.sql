USE [zc_data]
GO
SET IDENTITY_INSERT [dbo].[level] ON 

INSERT [dbo].[level] ([level_id], [level_name], [level_money], [level_money1], [recom_rate1], [recom_rate2], [level_image], [level_remark]) VALUES (1, N'铜卡', 18000, 50000, CAST(0.500 AS Decimal(4, 3)), CAST(0.200 AS Decimal(4, 3)), NULL, NULL)
INSERT [dbo].[level] ([level_id], [level_name], [level_money], [level_money1], [recom_rate1], [recom_rate2], [level_image], [level_remark]) VALUES (2, N'银卡', 32000, 100000, CAST(0.500 AS Decimal(4, 3)), CAST(0.200 AS Decimal(4, 3)), NULL, NULL)
INSERT [dbo].[level] ([level_id], [level_name], [level_money], [level_money1], [recom_rate1], [recom_rate2], [level_image], [level_remark]) VALUES (3, N'金卡', 76000, 250000, CAST(0.800 AS Decimal(4, 3)), CAST(0.200 AS Decimal(4, 3)), NULL, NULL)
SET IDENTITY_INSERT [dbo].[level] OFF
SET IDENTITY_INSERT [dbo].[operator] ON 

INSERT [dbo].[operator] ([oper_id], [oper_code], [oper_name], [oper_phone], [oper_password], [oper_department], [oper_permission], [oper_remark]) VALUES (2, N'001', N'super', N'15108357422', N'E3CEB5881A0A1FDAAD01296D7554868D', N'综合部', N'sysadmin', NULL)
SET IDENTITY_INSERT [dbo].[operator] OFF
SET IDENTITY_INSERT [dbo].[user] ON 

INSERT [dbo].[user] ([user_id], [user_code], [user_name], [user_phone], [id_number], [login_password], [second_password], [referrer_id], [level_id], [province], [city], [area], [address], [bank_name], [account_num], [user_status], [reg_money], [register_time], [activate_time], [activate_id], [user_remark], [user_remark1], [user_remark2], [user_remark3]) VALUES (2, N'00000000', N'张某某', N'13900000000', N'000000000000000000', N'E3CEB5881A0A1FDAAD01296D7554868D', N'E3CEB5881A0A1FDAAD01296D7554868D', 0, 3, NULL, NULL, NULL, NULL, NULL, NULL, 1, 76000, CAST(0x0000A8E400000000 AS DateTime), CAST(0x0000A8E400000000 AS DateTime), 2, NULL, NULL, NULL, NULL)
INSERT [dbo].[user] ([user_id], [user_code], [user_name], [user_phone], [id_number], [login_password], [second_password], [referrer_id], [level_id], [province], [city], [area], [address], [bank_name], [account_num], [user_status], [reg_money], [register_time], [activate_time], [activate_id], [user_remark], [user_remark1], [user_remark2], [user_remark3]) VALUES (4, N'00000001', N'李某某', N'13911111111', N'111111111111111111', N'E3CEB5881A0A1FDAAD01296D7554868D', N'E3CEB5881A0A1FDAAD01296D7554868D', 2, 3, NULL, NULL, NULL, NULL, NULL, NULL, 2, 76000, CAST(0x0000A8E400000000 AS DateTime), CAST(0x0000A8E5001608B2 AS DateTime), 2, NULL, NULL, NULL, NULL)
INSERT [dbo].[user] ([user_id], [user_code], [user_name], [user_phone], [id_number], [login_password], [second_password], [referrer_id], [level_id], [province], [city], [area], [address], [bank_name], [account_num], [user_status], [reg_money], [register_time], [activate_time], [activate_id], [user_remark], [user_remark1], [user_remark2], [user_remark3]) VALUES (5, N'00000003', N'王某某', N'13922222222', N'222222222222222222', N'E3CEB5881A0A1FDAAD01296D7554868D', N'E3CEB5881A0A1FDAAD01296D7554868D', 4, 1, NULL, NULL, NULL, NULL, NULL, NULL, 2, 18000, CAST(0x0000A8E50012D318 AS DateTime), CAST(0x0000A8E500160C34 AS DateTime), 2, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[user] OFF
