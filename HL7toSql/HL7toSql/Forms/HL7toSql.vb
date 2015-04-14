Imports System.IO
Imports System.IO.StreamReader
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Threading
Imports System.Text
Imports System.ComponentModel
Imports System.Net.Sockets
Imports System.Net



Public Class HL7toDB
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


    Private Sub showBDcontent(ByVal DG As DataGridView)
        If DG.InvokeRequired Then
            DG.Invoke(New dbUpdateDelegate(AddressOf showBDcontent), New Object() {DG})
        Else
            DG.DataSource = MSSQLControllerMindray.Instance.getTable("Monitorizacao").Tables(0)
            Me.DataGridView1.Columns(3).DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss:fff"
            Me.DataGridView1.Columns(4).DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss:fff"
        End If
    End Sub
    Private Sub showBDcontent()
        Me.DataGridView1.DataSource = MSSQLControllerMindray.Instance.getTable("Monitorizacao").Tables(0)
        Me.DataGridView1.Columns(3).DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss:fff"
        Me.DataGridView1.Columns(4).DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss:fff"

    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        worker.WorkerReportsProgress = True
        worker.WorkerSupportsCancellation = True
        AddHandler worker.ProgressChanged, AddressOf BackgroundWorker1_ProgressChanged
        showBDcontent()
    End Sub


    Private Sub UploadToDB(ByVal sender As System.Object, _
                     ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim controler As MSSQLControllerMindray = MSSQLControllerMindray.Instance

        Dim count As Integer = 0
        For Each envia In listMsg
            If (envia.Valide()) Then
                controler.addMSGtoDB(envia)
            End If
            count += 1
            worker.ReportProgress(count)
        Next
    End Sub




    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Export2Sql.Click
        Load2DB.Value = 0
        Load2DB.Maximum = listMsg.Count
        BackgroundWorker1.RunWorkerAsync()
        'showBDcontent()

    End Sub

    Private Sub OpenFileDialog1_FileOk_1(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk

        TextBox1.Text = OpenFileDialog1.FileName.ToString()


    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Select_file.Click
        'Dim t = New Thread(AddressOf getData)

        't.SetApartmentState(ApartmentState.STA)
        't.IsBackground = True
        't.Start()
        Dim mp As New MindrayProtocol
        mp.Connect()
        AddHandler mp.OnReceiveMSG, AddressOf getData
    End Sub

    Private Sub getData(msg As Message)
        MSSQLControllerMindray.Instance.addMSGtoDB(msg)
        dataReceived(TextBox2, msg.toString)
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

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        'Load2DB.Value = e.ProgressPercentage
        Invoke(Sub()
                   Load2DB.Increment(1)
               End Sub)
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        showBDcontent()
    End Sub


End Class
