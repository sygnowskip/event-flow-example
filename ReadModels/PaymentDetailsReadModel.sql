-- =========================================
-- Create table template SQL Azure Database 
-- =========================================

DROP TABLE [dbo].[ReadModel-PaymentDetailsReadModel];

CREATE TABLE [dbo].[ReadModel-PaymentDetailsReadModel](
	[PaymentId] [nvarchar](255) NOT NULL,
	[Country] [nvarchar](255) NOT NULL,
	[Currency] [nvarchar](255) NOT NULL,
	[System] [nvarchar](255) NOT NULL,
	[Amount] decimal(18,2) NOT NULL,
	[ExternalId] [nvarchar](255) NOT NULL,
	[ExternalCallbackUrl] [nvarchar](255) NULL,
	[Status] [nvarchar](255) NULL,
	[Version] [int] NULL,
	PRIMARY KEY(PaymentId)
)