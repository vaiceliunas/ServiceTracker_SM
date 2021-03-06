USE [FoobarServices]
GO

/****** Object:  Table [dbo].[Label]    Script Date: 12/2/2021 4:51:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Label](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceId] [int] NOT NULL,
	[LabelKey] [nvarchar](30) NOT NULL,
	[LabelValue] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Label] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Label]  WITH CHECK ADD  CONSTRAINT [FK_Service_label] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Service] ([Id])
GO

ALTER TABLE [dbo].[Label] CHECK CONSTRAINT [FK_Service_label]
GO