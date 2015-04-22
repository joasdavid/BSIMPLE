Imports System.IO
Imports System.IO.StreamReader
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Threading
Imports System.Text
Imports System.ComponentModel
Imports System.Net.Sockets
Imports System.Net
Imports System.Windows.Forms.DataVisualization.Charting
Imports Drive

Public Class MainForm
    Private myConn As SqlConnection
    Private myCmd As SqlCommand
    Private da As SqlDataAdapter
    Private ds As DataSet
    Private listMsg As New List(Of Message)
    Private c28 As Char = Chr(28)
    Private ReadOnly lock As New Object
    Public Delegate Sub dataReceivedDelegate(ByVal TB As TextBox, ByVal txt As String)
    Public Delegate Sub dbUpdateDelegate(ByVal DG As DataGridView)
    Dim mp As New MindrayProtocol

    Private controller As MSSQLController
#Region "HL72DB"
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        controller = New MSSQLController
        showBDcontent(DataGridView1)
        fillCBox()
    End Sub
    Private Sub showBDcontent(ByVal DG As DataGridView)
        If DG.InvokeRequired Then
            DG.Invoke(New dbUpdateDelegate(AddressOf showBDcontent), New Object() {DG})
        Else
            DG.DataSource = controller.getTable("Monitorizacao").Tables(0)
            Me.DataGridView1.Columns(3).DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss:fff"
            Me.DataGridView1.Columns(4).DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss:fff"
        End If
    End Sub

    Private Sub Connecttoserver_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Connecttoserver.Click
        mp.Connect()
        AddHandler mp.OnReceiveMSG, AddressOf getData
    End Sub

    Private Sub getData(msg As MessageORU)
        dataReceived(TextBox2, msg.ToString)
        dataReceived(TextBox2, "")
        showBDcontent(DataGridView1)
    End Sub

    Public Sub dataReceived(ByVal TB As TextBox, ByVal txt As String)
        If TB.InvokeRequired Then
            TB.Invoke(New dataReceivedDelegate(AddressOf dataReceived), New Object() {TB, txt})
        Else
            TB.AppendText(txt)
            TB.AppendText(vbNewLine)
        End If
    End Sub
    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        mp.Disconnect()
    End Sub
#End Region
#Region "Grafico"

    Private Sub fillCBox()

        ' table As DataSet = controller.getTableCB("SinaisVitais").Tables(0)
        cbSelectType.Items.Clear()
        If (RadioButton1.Checked = True) Then
            For Each fill As DataRow In controller.getTable("SinaisVitais").Tables(0).Rows
                cbSelectType.Items.Add(fill("Descricao"))
            Next
        ElseIf (RadioButton2.Checked = True) Then
            cbSelectType.Items.Add("Todos alarmes")
            For Each fill As DataRow In controller.getTable("Alarme").Tables(0).Rows
                cbSelectType.Items.Add(fill("Descricao"))
            Next

        End If
    End Sub

    Private Sub showBDcontentSearch(id As String)
        Me.DataGridView2.DataSource = controller.getTableGraph(id, DataInicio.Value.Date.ToString("yyyy-MM-dd"), DataFim.Value.Date.ToString("yyyy-MM-dd")).Tables(0)
        Me.DataGridView2.Columns(1).DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss:fff"
        Me.DataGridView2.Columns(2).DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss:fff"
        Me.DataGridView2.Update()
    End Sub

    Private Sub showBDcontentSearch()
        Me.DataGridView2.DataSource = controller.getTableNumAlarm(DataInicio.Value.Date.ToString("yyyy-MM-dd"), DataFim.Value.Date.ToString("yyyy-MM-dd")).Tables(0)
        Me.DataGridView2.Columns(1).DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss:fff"
        Me.DataGridView2.Update()
    End Sub

    Private Sub showBDcontentSearchAlarme(id As String)
        Me.DataGridView2.DataSource = controller.getTableNumAlarm(id, DataInicio.Value.Date.ToString("yyyy-MM-dd"), DataFim.Value.Date.ToString("yyyy-MM-dd")).Tables(0)
        Me.DataGridView2.Columns(1).DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss:fff"
        Me.DataGridView2.Update()
    End Sub

    Private Sub radioChange() Handles RadioButton1.Click, RadioButton2.Click
        fillCBox()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Chart1.ChartAreas(0).AxisX.ScaleView.ZoomReset(0)
        Chart1.ChartAreas(0).AxisY.ScaleView.ZoomReset(0)
        controller.setidPaciente(tbGraph.Text)

        If (RadioButton1.Checked = True) Then
            Dim name2sv As DataSet = controller.getSVidFromName(cbSelectType.SelectedItem.ToString)
            Dim id = name2sv.Tables(0).Rows(0).Item(0)
            Dim table As DataSet = controller.getTableGraph(id, DataInicio.Value.Date.ToString("yyyy-MM-dd"), DataFim.Value.Date.ToString("yyyy-MM-dd"))
            graphSV(table)
            showBDcontentSearch(id)
        Else
            If (cbSelectType.SelectedItem.ToString = "Todos alarmes") Then
                Dim table As DataSet = controller.getTableNumAlarm(DataInicio.Value.Date.ToString("yyyy-MM-dd"), DataFim.Value.Date.ToString("yyyy-MM-dd"))
                showBDcontentSearch()
                graphAL(table)
            Else
                Dim name2sv As DataSet = controller.getAlarmeidFromName(cbSelectType.SelectedItem.ToString)
                Dim id = name2sv.Tables(0).Rows(0).Item(0)
                Dim table As DataSet = controller.getTableNumAlarm(id, DataInicio.Value.Date.ToString("yyyy-MM-dd"), DataFim.Value.Date.ToString("yyyy-MM-dd"))
                graphAL(table)
                showBDcontentSearchAlarme(id)
            End If
        End If

        'Chart1.DataBind()
        'Chart1.Update()
    End Sub

    Private Sub graphSV(table As DataSet)
        Chart1.Series.Clear()
        Dim s1 As Series = New Series(" ")
        Dim s2 As Series = New Series(cbSelectType.SelectedItem.ToString)
        s1.ChartType = SeriesChartType.Line
        s2.ChartType = SeriesChartType.Point
        s1.IsVisibleInLegend = False
        s2.IsValueShownAsLabel = False

        Chart1.Series.Add(s1)
        Chart1.Series.Add(s2)
        Chart1.ChartAreas(0).AxisX.Interval = 0
        Chart1.ChartAreas(0).CursorX.IsUserSelectionEnabled = True
        Chart1.ChartAreas(0).CursorY.IsUserSelectionEnabled = True
        Chart1.DataManipulator.FilterSetEmptyPoints = True
        Chart1.Series(1).ToolTip = cbSelectType.SelectedItem.ToString & " = #VALY" & vbCrLf & "#AXISLABEL"
        For Each value As DataRow In table.Tables(0).Rows
            Chart1.Series(0).Points.AddXY(value.Item(1).ToString, value.Item(0))
            Chart1.Series(1).Points.AddXY(value.Item(1).ToString, value.Item(0))
            If value.Item(1).ToString() <> value.Item(2).ToString() Then
                Chart1.Series(1).Points.AddXY(value.Item(2).ToString, value.Item(0))
                Chart1.Series(0).Points.AddXY(value.Item(2).ToString, value.Item(0))
            End If
        Next
    End Sub

    Private Sub graphAL(table As DataSet)
        Chart1.Series.Clear()
        Dim s1 As Series = New Series("Nivel - 1")
        Dim s2 As Series = New Series("Nivel - 2")
        Dim s3 As Series = New Series("Nivel - 3")

        s1.ChartType = SeriesChartType.StackedColumn
        s2.ChartType = SeriesChartType.StackedColumn
        s3.ChartType = SeriesChartType.StackedColumn
        s1.Color = Color.Red
        s2.Color = Color.Orange
        s3.Color = Color.Yellow
        s1.IsValueShownAsLabel = True
        s2.IsValueShownAsLabel = True
        s3.IsValueShownAsLabel = True
        Chart1.ChartAreas(0).CursorX.IsUserSelectionEnabled = True
        s1.ToolTip = "#VALY alarmes do nivel 1" & vbCrLf & "#AXISLABEL"
        s2.ToolTip = "#VALY alarmes do nivel 2" & vbCrLf & "#AXISLABEL"
        s3.ToolTip = "#VALY alarmes do nivel 3" & vbCrLf & "#AXISLABEL"

        Chart1.ChartAreas(0).AxisX.Interval = 1

        For Each value As DataRow In table.Tables(0).Rows
            If (value.Item(1).ToString() = "1") Then
                s1.Points.AddXY(value.Item(2).ToString, value.Item(0))
            ElseIf (value.Item(1).ToString() = "2") Then
                s2.Points.AddXY(value.Item(2).ToString, value.Item(0))
            ElseIf (value.Item(1).ToString() = "3") Then
                s3.Points.AddXY(value.Item(2).ToString, value.Item(0))
            End If
        Next

        Chart1.Series.Add(s1)
        Chart1.Series.Add(s2)
        Chart1.Series.Add(s3)
    End Sub
#End Region
End Class