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

Public Class MainForm
    Private myConn As SqlConnection
    Private myCmd As SqlCommand
    Private da As SqlDataAdapter
    Private ds As DataSet
    Private listMsg As New List(Of Message)
    Private c28 As Char = Chr(28)
    Dim worker As BackgroundWorker = New BackgroundWorker
    Private ReadOnly lock As New Object
    Public Delegate Sub dataReceivedDelegate(ByVal TB As TextBox, ByVal txt As String)
    Public Delegate Sub dbUpdateDelegate(ByVal DG As DataGridView)
    Dim mp As New MindrayProtocol

    Private controller As MSSQLControllerMindray
#Region "HL72DB"
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        controller = New MSSQLControllerMindray
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

    Private Sub getData(msg As Message)
        dataReceived(TextBox2, msg.toString)
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
        For Each fill As DataRow In controller.getTableSV("SinaisVitais").Tables(0).Rows
            cbGraph.Items.Add(fill("Descricao"))
        Next

    End Sub

    Private Sub showBDcontentSearch(id As String)
        Dim name2sv As DataSet = controller.getSVidFromName(cbGraph.SelectedItem.ToString)
        Me.DataGridView2.DataSource = controller.getTableGraph(id).Tables(0)
        Me.DataGridView2.Update()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        controller.setidPaciente(tbGraph.Text)
        Dim name2sv As DataSet = controller.getSVidFromName(cbGraph.SelectedItem.ToString)
        Dim id = name2sv.Tables(0).Rows(0).Item(0)
        showBDcontentSearch(id)
        Dim table As DataSet = controller.getTableGraph(id)
        Chart1.Series(0).Points.Clear()
        Chart1.Series(1).Points.Clear()
        'Chart1.Series.Dispose()
        Chart1.Series(1).ToolTip = cbGraph.SelectedItem.ToString & " = #VALY" & vbCrLf & "#AXISLABEL"
        For Each value As DataRow In table.Tables(0).Rows
            Chart1.Series(0).Points.AddXY(value.Item(1).ToString, value.Item(0))
            Chart1.Series(1).Points.AddXY(value.Item(1).ToString, value.Item(0))
            If value.Item(1).ToString() <> value.Item(2).ToString() Then
                Chart1.Series(1).Points.AddXY(value.Item(2).ToString, value.Item(0))
                Chart1.Series(0).Points.AddXY(value.Item(2).ToString, value.Item(0))
            End If
        Next
        Chart1.DataBind()
        Chart1.Update()
    End Sub
#End Region
End Class