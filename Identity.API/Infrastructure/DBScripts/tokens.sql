USE [identity]
GO

/****** Object:  Table [dbo].[Tokens]    Script Date: 17.12.2023 18:41:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Tokens](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](150) NULL,
	[Roles] [varchar](150) NULL,
	[Refreshtoken] [varchar](500) NULL,
	[JWT] [varchar](500) NULL,
	[JWT_ExpireDate] [datetime] NULL,
	[RT_ExpireDate] [datetime] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[IP] [varchar](150) NULL,
	[Revoked] [int] NULL,
 CONSTRAINT [PK_Tokens] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


