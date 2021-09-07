
CREATE TABLE [dbo].[SYS_RECLAMACAO_VIA](
	[COD_VIA] [int] IDENTITY(1,1) NOT NULL,
	[DESC_VIA] [varchar](100) NOT NULL,
 CONSTRAINT [SYS_RECLAMACAO_VIA_pk] PRIMARY KEY CLUSTERED 
(
	[COD_VIA] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

