USE [HL7Mindray]
GO
/****** Object:  Table [dbo].[Componentes]    Script Date: 18-03-2015 15:52:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Componentes](
	[IdComponente] [int] IDENTITY(1,1) NOT NULL,
	[IdValores] [int] NOT NULL,
	[IdPaciente] [varchar](50) NOT NULL,
	[IdOBX] [int] NOT NULL,
	[Valor] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Valores_Componentes] PRIMARY KEY CLUSTERED 
(
	[IdPaciente] ASC,
	[IdOBX] ASC,
	[IdValores] ASC,
	[IdComponente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Monitorizacao]    Script Date: 18-03-2015 15:52:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Monitorizacao](
	[IdOBX] [int] NOT NULL,
	[IdPaciente] [varchar](50) NOT NULL,
	[Descrição_Id] [varchar](15) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdOBX] ASC,
	[IdPaciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Paciente]    Script Date: 18-03-2015 15:52:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Paciente](
	[IdPaciente] [varchar](50) NOT NULL,
	[Frist_Name_Paciente] [varchar](10) NOT NULL,
	[Last_Name_Paciente] [varchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPaciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Valores]    Script Date: 18-03-2015 15:52:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Valores](
	[IdValores] [int] IDENTITY(1,1) NOT NULL,
	[IdPaciente] [varchar](50) NOT NULL,
	[IdOBX] [int] NOT NULL,
	[Sub_OBX_id] [int] NULL,
	[Parametro] [nchar](10) NULL,
	[TimeStamp] [text] NULL,
 CONSTRAINT [PK_Valores] PRIMARY KEY CLUSTERED 
(
	[IdPaciente] ASC,
	[IdOBX] ASC,
	[IdValores] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  View [dbo].[ViewDataGridView]    Script Date: 18-03-2015 15:52:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewDataGridView]
AS
SELECT        dbo.Paciente.IdPaciente, dbo.Paciente.Frist_Name_Paciente, dbo.Monitorizacao.IdOBX, dbo.Monitorizacao.Descrição_Id, dbo.Valores.Sub_OBX_id, dbo.Valores.Valor1, dbo.Valores.Parametro, 
                         dbo.Valores.TimeStamp, dbo.Componentes.Valor
FROM            dbo.Componentes INNER JOIN
                         dbo.Monitorizacao ON dbo.Componentes.IdOBX = dbo.Monitorizacao.IdOBX INNER JOIN
                         dbo.Paciente ON dbo.Monitorizacao.IdPaciente = dbo.Paciente.IdPaciente INNER JOIN
                         dbo.Valores ON dbo.Componentes.IdPaciente = dbo.Valores.IdPaciente AND dbo.Componentes.IdOBX = dbo.Valores.IdOBX AND dbo.Componentes.IdValores = dbo.Valores.IdValores AND 
                         dbo.Monitorizacao.IdOBX = dbo.Valores.IdOBX AND dbo.Monitorizacao.IdPaciente = dbo.Valores.IdPaciente

GO
ALTER TABLE [dbo].[Componentes]  WITH CHECK ADD  CONSTRAINT [FK_Componentes] FOREIGN KEY([IdPaciente], [IdOBX], [IdValores])
REFERENCES [dbo].[Valores] ([IdPaciente], [IdOBX], [IdValores])
GO
ALTER TABLE [dbo].[Componentes] CHECK CONSTRAINT [FK_Componentes]
GO
ALTER TABLE [dbo].[Monitorizacao]  WITH CHECK ADD  CONSTRAINT [FK_Monitorizacao_ToTable] FOREIGN KEY([IdPaciente])
REFERENCES [dbo].[Paciente] ([IdPaciente])
GO
ALTER TABLE [dbo].[Monitorizacao] CHECK CONSTRAINT [FK_Monitorizacao_ToTable]
GO
ALTER TABLE [dbo].[Valores]  WITH CHECK ADD  CONSTRAINT [FK_Valores] FOREIGN KEY([IdOBX], [IdPaciente])
REFERENCES [dbo].[Monitorizacao] ([IdOBX], [IdPaciente])
GO
ALTER TABLE [dbo].[Valores] CHECK CONSTRAINT [FK_Valores]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[46] 4[16] 2[21] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Componentes"
            Begin Extent = 
               Top = 5
               Left = 44
               Bottom = 135
               Right = 214
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "Monitorizacao"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 119
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Paciente"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 119
               Right = 652
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Valores"
            Begin Extent = 
               Top = 20
               Left = 265
               Bottom = 150
               Right = 435
            End
            DisplayFlags = 280
            TopColumn = 3
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
    ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataGridView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'  End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataGridView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ViewDataGridView'
GO
