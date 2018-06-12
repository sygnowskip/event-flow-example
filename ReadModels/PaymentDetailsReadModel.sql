-- =========================================
-- Create table template SQL Azure Database 
-- =========================================

DROP TABLE [dbo].[ReadModel-PaymentDetails];

CREATE TABLE [dbo].[ReadModel-PaymentDetails](
	[PaymentId] [nvarchar](255) NOT NULL,
	[Username] [nvarchar](255) NOT NULL,
	[OrderId] [uniqueidentifier] NOT NULL,
	[TotalPrice] decimal(18,2) NOT NULL,
	[Status] [nvarchar](255) NULL,
	[Version] [int] NULL,
	PRIMARY KEY(PaymentId)
)