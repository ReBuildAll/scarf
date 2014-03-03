/****** Object:  Table [dbo].[scarf_Log]    Script Date: 3.3.2014 16:15:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[scarf_Log](
	[Id] [uniqueidentifier] NOT NULL,
	[LoggedAtUtc] [datetime] NOT NULL,
	[ApplicationName] [nvarchar](100) NOT NULL,
	[Computer] [nvarchar](100) NOT NULL,
	[ResourceUri] [nvarchar](150) NOT NULL,
	[User] [nvarchar](150) NOT NULL,
	[Class] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Message] [nvarchar](500) NULL,
	[Sequence] [bigint] IDENTITY(1,1) NOT NULL,
	[LogMessageAsJson] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_scarf_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Index [IX_scarf_Log]    Script Date: 3.3.2014 16:16:02 ******/
CREATE NONCLUSTERED INDEX [IX_scarf_Log] ON [dbo].[scarf_Log]
(
	[LoggedAtUtc] DESC,
	[Sequence] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
