USE [HL7Mindray]
GO
/****** Object:  Table [dbo].[Alarme]    Script Date: 30-03-2015 15:01:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Alarme](
	[IdAlarme] [int] NOT NULL,
	[Descricao] [varchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdAlarme] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Monitorizacao]    Script Date: 30-03-2015 15:01:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Monitorizacao](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdPaciente] [varchar](40) NOT NULL,
	[IdOBX] [int] NOT NULL,
	[Valor] [numeric](5, 2) NOT NULL,
	[DataInicio] [datetime] NOT NULL,
	[DataFinal] [datetime] NOT NULL,
	[IdSV] [int] NOT NULL,
	[IdAlarme] [int] NOT NULL,
 CONSTRAINT [PK_Monitorizacao] PRIMARY KEY CLUSTERED 
(
	[IdPaciente] ASC,
	[IdOBX] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Paciente]    Script Date: 30-03-2015 15:01:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Paciente](
	[IdPaciente] [varchar](40) NOT NULL,
	[Frist_Name_Paciente] [varchar](10) NOT NULL,
	[Last_Name_Paciente] [varchar](10) NOT NULL,
	[DataNas] [date] NULL,
	[Sexo] [varchar](1) NULL,
	[Morada] [varchar](20) NULL,
	[Sangue] [varchar](1) NULL,
	[TipoPaciente] [varchar](1) NULL,
	[Pace_Switch] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPaciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[SinaisVitais]    Script Date: 30-03-2015 15:01:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SinaisVitais](
	[IdSV] [int] NOT NULL,
	[Descricao] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdSV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[Monitorizacao]  WITH CHECK ADD  CONSTRAINT [FK_Alarme] FOREIGN KEY([IdAlarme])
REFERENCES [dbo].[Alarme] ([IdAlarme])
GO
ALTER TABLE [dbo].[Monitorizacao] CHECK CONSTRAINT [FK_Alarme]
GO
ALTER TABLE [dbo].[Monitorizacao]  WITH CHECK ADD  CONSTRAINT [FK_Monit] FOREIGN KEY([IdPaciente])
REFERENCES [dbo].[Paciente] ([IdPaciente])
GO
ALTER TABLE [dbo].[Monitorizacao] CHECK CONSTRAINT [FK_Monit]
GO
ALTER TABLE [dbo].[Monitorizacao]  WITH CHECK ADD  CONSTRAINT [FK_Sv] FOREIGN KEY([IdSV])
REFERENCES [dbo].[SinaisVitais] ([IdSV])
GO
ALTER TABLE [dbo].[Monitorizacao] CHECK CONSTRAINT [FK_Sv]
GO
