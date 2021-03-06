USE [zc_data]
GO
/****** Object:  Table [dbo].[account_record]    Script Date: 2018/5/16 0:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[account_record](
	[acc_record_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[acc_type] [int] NOT NULL,
	[cons_type] [int] NOT NULL,
	[acc_record_type] [int] NOT NULL,
	[cons_value] [int] NOT NULL,
	[acc_balance] [int] NOT NULL,
	[oper_id] [int] NULL,
	[acc_remark] [nvarchar](1000) NULL,
	[acc_re_remark1] [nvarchar](1000) NULL,
	[acc_re_remark2] [nvarchar](1000) NULL,
	[acc_record_time] [datetime] NOT NULL,
 CONSTRAINT [PK_account_record] PRIMARY KEY CLUSTERED 
(
	[acc_record_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[bonus_record]    Script Date: 2018/5/16 0:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[bonus_record](
	[bonus_record_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[bouns_type] [int] NOT NULL,
	[bouns_money] [int] NOT NULL,
	[source_id] [int] NOT NULL,
	[create_time] [datetime] NOT NULL,
	[bonus_remark] [nvarchar](1000) NULL,
 CONSTRAINT [PK_bonus_record] PRIMARY KEY CLUSTERED 
(
	[bonus_record_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cash_record]    Script Date: 2018/5/16 0:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cash_record](
	[cash_record_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[cash_type] [int] NOT NULL,
	[cash_money] [int] NOT NULL,
	[cash_status] [int] NOT NULL,
	[cash_time1] [datetime] NOT NULL,
	[oper_id1] [int] NULL,
	[cash_time2] [datetime] NULL,
	[cash_remark1] [nvarchar](1000) NULL,
	[oper_id2] [int] NULL,
	[cash_time3] [datetime] NULL,
 CONSTRAINT [PK_cash_record] PRIMARY KEY CLUSTERED 
(
	[cash_record_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[goods]    Script Date: 2018/5/16 0:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[goods](
	[goods_id] [int] IDENTITY(1,1) NOT NULL,
	[goods_name] [nvarchar](200) NOT NULL,
	[goods_unit] [nvarchar](10) NOT NULL,
	[case_price] [int] NOT NULL,
	[rep_price] [int] NOT NULL,
	[goods_image] [varchar](200) NULL,
	[goods_desc] [nvarchar](max) NULL,
	[goods_remark] [nvarchar](1000) NULL,
	[oper_id] [int] NOT NULL,
	[create_time] [datetime] NOT NULL,
 CONSTRAINT [PK_goods] PRIMARY KEY CLUSTERED 
(
	[goods_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[level]    Script Date: 2018/5/16 0:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[level](
	[level_id] [int] IDENTITY(1,1) NOT NULL,
	[level_name] [nvarchar](50) NOT NULL,
	[level_money] [int] NOT NULL,
	[level_money1] [int] NOT NULL,
	[recom_rate1] [decimal](4, 3) NOT NULL,
	[recom_rate2] [decimal](4, 3) NOT NULL,
	[level_image] [varchar](200) NULL,
	[level_remark] [nvarchar](1000) NULL,
 CONSTRAINT [PK_level] PRIMARY KEY CLUSTERED 
(
	[level_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[menu]    Script Date: 2018/5/16 0:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[menu](
	[menu_id] [int] IDENTITY(1,1) NOT NULL,
	[menu_name] [nvarchar](20) NOT NULL,
	[menu_parent_id] [int] NOT NULL,
	[menu_url] [varchar](200) NULL,
	[menu_remark] [nvarchar](1000) NULL,
 CONSTRAINT [PK_menu] PRIMARY KEY CLUSTERED 
(
	[menu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[operator]    Script Date: 2018/5/16 0:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[operator](
	[oper_id] [int] IDENTITY(1,1) NOT NULL,
	[oper_code] [varchar](20) NOT NULL,
	[oper_name] [nvarchar](100) NOT NULL,
	[oper_phone] [varchar](20) NOT NULL,
	[oper_password] [varchar](50) NOT NULL,
	[oper_department] [nvarchar](100) NOT NULL,
	[oper_permission] [varchar](200) NOT NULL,
	[oper_remark] [nvarchar](1000) NULL,
 CONSTRAINT [PK_operator] PRIMARY KEY CLUSTERED 
(
	[oper_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[operator_sysrole]    Script Date: 2018/5/16 0:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[operator_sysrole](
	[oper_id] [int] NOT NULL,
	[role_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[oper_id] ASC,
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[order]    Script Date: 2018/5/16 0:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[order](
	[order_id] [int] IDENTITY(1,1) NOT NULL,
	[order_num] [varchar](20) NOT NULL,
	[order_type] [int] NOT NULL,
	[user_id] [int] NOT NULL,
	[package_id] [int] NOT NULL,
	[order_cash] [int] NOT NULL,
	[order_rep] [int] NOT NULL,
	[is_pay] [bit] NOT NULL,
	[pay_type] [int] NULL,
	[logistics_company] [nvarchar](100) NULL,
	[logistics_num] [varchar](100) NULL,
	[order_remark] [nvarchar](1000) NULL,
	[create_time] [datetime] NULL,
 CONSTRAINT [PK_order] PRIMARY KEY CLUSTERED 
(
	[order_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[order_detail]    Script Date: 2018/5/16 0:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[order_detail](
	[order_detail_id] [int] IDENTITY(1,1) NOT NULL,
	[order_id] [int] NOT NULL,
	[goods_id] [int] NOT NULL,
	[goods_num] [int] NOT NULL,
	[cash_price] [int] NOT NULL,
	[rep_price] [int] NOT NULL,
 CONSTRAINT [PK_order_detail] PRIMARY KEY CLUSTERED 
(
	[order_detail_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[package]    Script Date: 2018/5/16 0:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[package](
	[package_id] [int] IDENTITY(1,1) NOT NULL,
	[package_name] [nvarchar](200) NOT NULL,
	[package_price] [int] NOT NULL,
	[package_status] [int] NOT NULL,
	[package_remark] [nvarchar](1000) NOT NULL,
	[oper_id] [int] NOT NULL,
	[create_time] [datetime] NOT NULL,
 CONSTRAINT [PK_package] PRIMARY KEY CLUSTERED 
(
	[package_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[package_detail]    Script Date: 2018/5/16 0:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[package_detail](
	[package_detail_id] [int] IDENTITY(1,1) NOT NULL,
	[package_id] [int] NOT NULL,
	[goods_id] [int] NOT NULL,
	[goods_num] [int] NOT NULL,
 CONSTRAINT [PK_package_detail] PRIMARY KEY CLUSTERED 
(
	[package_detail_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[sysrole]    Script Date: 2018/5/16 0:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sysrole](
	[role_id] [int] IDENTITY(1,1) NOT NULL,
	[role_name] [nvarchar](100) NOT NULL,
	[role_remark] [nvarchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[sysrole_menu]    Script Date: 2018/5/16 0:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sysrole_menu](
	[role_id] [int] NOT NULL,
	[menu_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[role_id] ASC,
	[menu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[user]    Script Date: 2018/5/16 0:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[user](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[user_code] [varchar](10) NOT NULL,
	[user_name] [nvarchar](50) NOT NULL,
	[user_phone] [varchar](20) NOT NULL,
	[id_number] [varchar](20) NOT NULL,
	[login_password] [varchar](50) NOT NULL,
	[second_password] [varchar](50) NOT NULL,
	[referrer_id] [int] NOT NULL,
	[level_id] [int] NOT NULL,
	[province] [nvarchar](50) NULL,
	[city] [nvarchar](50) NULL,
	[area] [nvarchar](50) NULL,
	[address] [nvarchar](200) NULL,
	[bank_name] [nvarchar](50) NULL,
	[account_num] [varchar](50) NULL,
	[user_status] [int] NOT NULL,
	[reg_money] [int] NOT NULL,
	[register_time] [datetime] NOT NULL,
	[activate_time] [datetime] NULL,
	[activate_id] [int] NULL,
	[user_remark] [nvarchar](1000) NULL,
	[user_remark1] [nvarchar](1000) NULL,
	[user_remark2] [nvarchar](1000) NULL,
	[user_remark3] [nvarchar](1000) NULL,
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[user_account]    Script Date: 2018/5/16 0:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user_account](
	[account_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[account1] [int] NOT NULL,
	[account2] [int] NOT NULL,
	[account3] [int] NOT NULL,
	[account4] [int] NOT NULL,
 CONSTRAINT [PK_user_account] PRIMARY KEY CLUSTERED 
(
	[account_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[user_bonus]    Script Date: 2018/5/16 0:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user_bonus](
	[bonus_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[dist_money] [int] NOT NULL,
	[dist_balance] [int] NOT NULL,
	[dist_number] [int] NOT NULL,
	[referrer_id1] [int] NOT NULL,
	[referrer_money1] [int] NOT NULL,
	[referrer_balance1] [int] NOT NULL,
	[referrer_number1] [int] NOT NULL,
	[referrer_id2] [int] NULL,
	[referrer_money2] [int] NULL,
	[referrer_balance2] [int] NULL,
	[referrer_number2] [int] NULL,
 CONSTRAINT [PK_user_bonus] PRIMARY KEY CLUSTERED 
(
	[bonus_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账户记录id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'account_record', @level2type=N'COLUMN',@level2name=N'acc_record_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'account_record', @level2type=N'COLUMN',@level2name=N'user_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账户类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'account_record', @level2type=N'COLUMN',@level2name=N'acc_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消费类型 1-增加 -1-减少' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'account_record', @level2type=N'COLUMN',@level2name=N'cons_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'记录类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'account_record', @level2type=N'COLUMN',@level2name=N'acc_record_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消费值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'account_record', @level2type=N'COLUMN',@level2name=N'cons_value'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'account_record', @level2type=N'COLUMN',@level2name=N'acc_balance'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'account_record', @level2type=N'COLUMN',@level2name=N'oper_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'account_record', @level2type=N'COLUMN',@level2name=N'acc_remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预留备注1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'account_record', @level2type=N'COLUMN',@level2name=N'acc_re_remark1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预留备注2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'account_record', @level2type=N'COLUMN',@level2name=N'acc_re_remark2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账户消费记录表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'account_record'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奖金记录id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'bonus_record', @level2type=N'COLUMN',@level2name=N'bonus_record_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'bonus_record', @level2type=N'COLUMN',@level2name=N'user_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-本金分红 2-推荐分红' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'bonus_record', @level2type=N'COLUMN',@level2name=N'bouns_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奖金金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'bonus_record', @level2type=N'COLUMN',@level2name=N'bouns_money'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'来源用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'bonus_record', @level2type=N'COLUMN',@level2name=N'source_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'bonus_record', @level2type=N'COLUMN',@level2name=N'create_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奖金备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'bonus_record', @level2type=N'COLUMN',@level2name=N'bonus_remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奖金记录表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'bonus_record'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提现记录id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cash_record', @level2type=N'COLUMN',@level2name=N'cash_record_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cash_record', @level2type=N'COLUMN',@level2name=N'user_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提现类型 1-分红提现 2-茶票提现' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cash_record', @level2type=N'COLUMN',@level2name=N'cash_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提现金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cash_record', @level2type=N'COLUMN',@level2name=N'cash_money'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提现状态 -1-审核不通过 0-待审核 1-待发放 2-已发放' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cash_record', @level2type=N'COLUMN',@level2name=N'cash_status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cash_record', @level2type=N'COLUMN',@level2name=N'cash_time1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核人id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cash_record', @level2type=N'COLUMN',@level2name=N'oper_id1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cash_record', @level2type=N'COLUMN',@level2name=N'cash_time2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cash_record', @level2type=N'COLUMN',@level2name=N'cash_remark1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发放人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cash_record', @level2type=N'COLUMN',@level2name=N'oper_id2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发放时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cash_record', @level2type=N'COLUMN',@level2name=N'cash_time3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提现记录表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cash_record'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'goods', @level2type=N'COLUMN',@level2name=N'goods_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'goods', @level2type=N'COLUMN',@level2name=N'goods_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'计量单位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'goods', @level2type=N'COLUMN',@level2name=N'goods_unit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'现金单价' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'goods', @level2type=N'COLUMN',@level2name=N'case_price'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'复消单价' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'goods', @level2type=N'COLUMN',@level2name=N'rep_price'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品图片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'goods', @level2type=N'COLUMN',@level2name=N'goods_image'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'goods', @level2type=N'COLUMN',@level2name=N'goods_desc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'goods', @level2type=N'COLUMN',@level2name=N'goods_remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'goods', @level2type=N'COLUMN',@level2name=N'oper_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'goods', @level2type=N'COLUMN',@level2name=N'create_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'goods'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等级id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'level', @level2type=N'COLUMN',@level2name=N'level_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等级名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'level', @level2type=N'COLUMN',@level2name=N'level_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等级金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'level', @level2type=N'COLUMN',@level2name=N'level_money'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分配金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'level', @level2type=N'COLUMN',@level2name=N'level_money1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第一层推荐系数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'level', @level2type=N'COLUMN',@level2name=N'recom_rate1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第二层推荐系数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'level', @level2type=N'COLUMN',@level2name=N'recom_rate2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等级图片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'level', @level2type=N'COLUMN',@level2name=N'level_image'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'level', @level2type=N'COLUMN',@level2name=N'level_remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等级表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'level'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'menu', @level2type=N'COLUMN',@level2name=N'menu_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'menu', @level2type=N'COLUMN',@level2name=N'menu_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父菜单id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'menu', @level2type=N'COLUMN',@level2name=N'menu_parent_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'menu', @level2type=N'COLUMN',@level2name=N'menu_url'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'menu', @level2type=N'COLUMN',@level2name=N'menu_remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'menu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'operator', @level2type=N'COLUMN',@level2name=N'oper_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'operator', @level2type=N'COLUMN',@level2name=N'oper_code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'operator', @level2type=N'COLUMN',@level2name=N'oper_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员手机号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'operator', @level2type=N'COLUMN',@level2name=N'oper_phone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'operator', @level2type=N'COLUMN',@level2name=N'oper_password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'operator', @level2type=N'COLUMN',@level2name=N'oper_department'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员权限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'operator', @level2type=N'COLUMN',@level2name=N'oper_permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'operator', @level2type=N'COLUMN',@level2name=N'oper_remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'operator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', @level2type=N'COLUMN',@level2name=N'order_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', @level2type=N'COLUMN',@level2name=N'order_num'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型 1-注册 2-复消' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', @level2type=N'COLUMN',@level2name=N'order_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', @level2type=N'COLUMN',@level2name=N'user_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套餐id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', @level2type=N'COLUMN',@level2name=N'package_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单现金金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', @level2type=N'COLUMN',@level2name=N'order_cash'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单复消金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', @level2type=N'COLUMN',@level2name=N'order_rep'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否付款' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', @level2type=N'COLUMN',@level2name=N'is_pay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'付款方式 1-在线支付 2-线下支付' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', @level2type=N'COLUMN',@level2name=N'pay_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物流公司名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', @level2type=N'COLUMN',@level2name=N'logistics_company'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物流单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', @level2type=N'COLUMN',@level2name=N'logistics_num'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', @level2type=N'COLUMN',@level2name=N'order_remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order', @level2type=N'COLUMN',@level2name=N'create_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单明细id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order_detail', @level2type=N'COLUMN',@level2name=N'order_detail_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order_detail', @level2type=N'COLUMN',@level2name=N'order_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order_detail', @level2type=N'COLUMN',@level2name=N'goods_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order_detail', @level2type=N'COLUMN',@level2name=N'goods_num'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'现金价格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order_detail', @level2type=N'COLUMN',@level2name=N'cash_price'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'复消价格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order_detail', @level2type=N'COLUMN',@level2name=N'rep_price'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单明细表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'order_detail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套餐id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'package', @level2type=N'COLUMN',@level2name=N'package_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套餐名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'package', @level2type=N'COLUMN',@level2name=N'package_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套餐价格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'package', @level2type=N'COLUMN',@level2name=N'package_price'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套餐状态 0-停用 1-正常' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'package', @level2type=N'COLUMN',@level2name=N'package_status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套餐备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'package', @level2type=N'COLUMN',@level2name=N'package_remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'package', @level2type=N'COLUMN',@level2name=N'oper_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'package', @level2type=N'COLUMN',@level2name=N'create_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套餐表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'package'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套餐明细id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'package_detail', @level2type=N'COLUMN',@level2name=N'package_detail_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套餐id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'package_detail', @level2type=N'COLUMN',@level2name=N'package_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'package_detail', @level2type=N'COLUMN',@level2name=N'goods_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'package_detail', @level2type=N'COLUMN',@level2name=N'goods_num'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套餐明细表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'package_detail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'user_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'user_code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'user_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户手机号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'user_phone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'身份证号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'id_number'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登陆密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'login_password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'二级密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'second_password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐人id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'referrer_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等级id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'level_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'省' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'province'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'市' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'city'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'area'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'详细地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开户银行' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'bank_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'account_num'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户状态；0-冻结 1-正常 2-未激活' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'user_status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'reg_money'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'register_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'激活时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'activate_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'激活人id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'activate_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'user_remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预留备注1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'user_remark1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预留备注2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'user_remark2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预留备注3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user', @level2type=N'COLUMN',@level2name=N'user_remark3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user_account', @level2type=N'COLUMN',@level2name=N'account_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user_account', @level2type=N'COLUMN',@level2name=N'user_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户账户表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user_account'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奖金表id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user_bonus', @level2type=N'COLUMN',@level2name=N'bonus_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user_bonus', @level2type=N'COLUMN',@level2name=N'user_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本金配额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user_bonus', @level2type=N'COLUMN',@level2name=N'dist_money'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配额余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user_bonus', @level2type=N'COLUMN',@level2name=N'dist_balance'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'剩余分红次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user_bonus', @level2type=N'COLUMN',@level2name=N'dist_number'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐人1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user_bonus', @level2type=N'COLUMN',@level2name=N'referrer_id1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐人1金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user_bonus', @level2type=N'COLUMN',@level2name=N'referrer_money1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐人1余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user_bonus', @level2type=N'COLUMN',@level2name=N'referrer_balance1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐人1剩余分红次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user_bonus', @level2type=N'COLUMN',@level2name=N'referrer_number1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐人2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user_bonus', @level2type=N'COLUMN',@level2name=N'referrer_id2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐人2金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user_bonus', @level2type=N'COLUMN',@level2name=N'referrer_money2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐人2余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user_bonus', @level2type=N'COLUMN',@level2name=N'referrer_balance2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐人2剩余分红次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user_bonus', @level2type=N'COLUMN',@level2name=N'referrer_number2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奖金计算表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'user_bonus'
GO
