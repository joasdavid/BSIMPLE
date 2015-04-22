<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Grafico
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.tbGraph = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.cbGraph = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.tb_DataFim = New System.Windows.Forms.TextBox()
        Me.tb_DataIn = New System.Windows.Forms.TextBox()
        Me.DataInicio = New System.Windows.Forms.DateTimePicker()
        Me.DataFim = New System.Windows.Forms.DateTimePicker()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Chart1
        '
        ChartArea1.Name = "ChartArea1"
        Me.Chart1.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.Chart1.Legends.Add(Legend1)
        Me.Chart1.Location = New System.Drawing.Point(368, 60)
        Me.Chart1.Name = "Chart1"
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series1.Color = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Series2.ChartArea = "ChartArea1"
        Series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point
        Series2.Color = System.Drawing.Color.Maroon
        Series2.Legend = "Legend1"
        Series2.Name = "Series2"
        Me.Chart1.Series.Add(Series1)
        Me.Chart1.Series.Add(Series2)
        Me.Chart1.Size = New System.Drawing.Size(673, 552)
        Me.Chart1.TabIndex = 0
        Me.Chart1.Text = "Chart1"
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(12, 60)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(337, 552)
        Me.DataGridView1.TabIndex = 1
        '
        'tbGraph
        '
        Me.tbGraph.Location = New System.Drawing.Point(85, 23)
        Me.tbGraph.Name = "tbGraph"
        Me.tbGraph.Size = New System.Drawing.Size(214, 20)
        Me.tbGraph.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Paciente Id :"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(429, 24)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(87, 20)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Pesquisa"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'cbGraph
        '
        Me.cbGraph.FormattingEnabled = True
        Me.cbGraph.Location = New System.Drawing.Point(344, 23)
        Me.cbGraph.Name = "cbGraph"
        Me.cbGraph.Size = New System.Drawing.Size(68, 21)
        Me.cbGraph.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(305, 26)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Id SV"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(497, 305)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(58, 13)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Data Final:"
        '
        'tb_DataFim
        '
        Me.tb_DataFim.Location = New System.Drawing.Point(561, 302)
        Me.tb_DataFim.Name = "tb_DataFim"
        Me.tb_DataFim.Size = New System.Drawing.Size(137, 20)
        Me.tb_DataFim.TabIndex = 19
        '
        'tb_DataIn
        '
        Me.tb_DataIn.Location = New System.Drawing.Point(354, 302)
        Me.tb_DataIn.Name = "tb_DataIn"
        Me.tb_DataIn.Size = New System.Drawing.Size(137, 20)
        Me.tb_DataIn.TabIndex = 18
        '
        'DataInicio
        '
        Me.DataInicio.CustomFormat = "yyyy-MM-dd"
        Me.DataInicio.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DataInicio.Location = New System.Drawing.Point(541, 26)
        Me.DataInicio.Name = "DataInicio"
        Me.DataInicio.Size = New System.Drawing.Size(90, 20)
        Me.DataInicio.TabIndex = 77
        '
        'DataFim
        '
        Me.DataFim.CustomFormat = "yyyy-MM-dd"
        Me.DataFim.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DataFim.Location = New System.Drawing.Point(656, 26)
        Me.DataFim.Name = "DataFim"
        Me.DataFim.Size = New System.Drawing.Size(90, 20)
        Me.DataFim.TabIndex = 78
        '
        'Grafico
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1053, 624)
        Me.Controls.Add(Me.DataFim)
        Me.Controls.Add(Me.DataInicio)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.tb_DataFim)
        Me.Controls.Add(Me.tb_DataIn)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cbGraph)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbGraph)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Chart1)
        Me.Name = "Grafico"
        Me.Text = "Grafico"
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Chart1 As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents tbGraph As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents cbGraph As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tb_DataFim As System.Windows.Forms.TextBox
    Friend WithEvents tb_DataIn As System.Windows.Forms.TextBox
    Friend WithEvents DataInicio As System.Windows.Forms.DateTimePicker
    Friend WithEvents DataFim As System.Windows.Forms.DateTimePicker
End Class
