USE [servdesk]

CREATE TABLE [dbo].[VINCULO_PERM](
	[OPERACAO] [varchar](30) NOT NULL,
	[PERMISSAO] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[OPERACAO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


