Imports System.IO
Imports System.IO.StreamReader
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Threading
Imports System.Text
Imports System.ComponentModel



Public Class HL7toDB
    Private myConn As SqlConnection
    Private myCmd As SqlCommand
    Private da As SqlDataAdapter
    Private ds As DataSet
    Private listMsg As New List(Of Message)
    Private c28 As Char = Chr(28)
    Dim worker As BackgroundWorker = New BackgroundWorker
    Private ReadOnly lock As New Object


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
        BackgroundWorker1.RunWorkerAsync()
        'showBDcontent()

    End Sub

    Private Sub OpenFileDialog1_FileOk_1(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk

        TextBox1.Text = OpenFileDialog1.FileName.ToString()


    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Select_file.Click

        Dim oReader As StreamReader
        Dim TextoLinha As String = ""

        listMsg.Clear()
        OpenFileDialog1.CheckFileExists = True
        OpenFileDialog1.CheckPathExists = True
        OpenFileDialog1.DefaultExt = "txt"
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
        OpenFileDialog1.Multiselect = False
        TextBox1.Text = ""
        TextBox2.Text = ""
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            oReader = New StreamReader(OpenFileDialog1.FileName, True)

            Dim strMSG As String = ""
            Dim line As String = ""
            Dim text As String = ""
            Dim countR As Integer = 0
            Dim m = New Message()
            Do While oReader.Peek() <> -1
                Try
                    line = oReader.ReadLine() + Chr(10)
                    If line.Chars(0) = Chr(28) Then
                        listMsg.Add(m)
                        m = New Message()
                    Else
                        m.parseData(line)
                    End If
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            Loop

            oReader.Close()

            Dim breakpoit As Integer = listMsg.Count / 2
            Dim t1 As Task = Task.Run(Sub()
                                          SyncLock lock
                                              Dim _temp As New StringBuilder
                                              'Dim _temp As String = ""
                                              For i = 0 To breakpoit - 1
                                                  _temp.Append(listMsg.Item(i).toString.Replace(Chr(10), vbNewLine) + vbNewLine)
                                              Next

                                              text += _temp.ToString()
                                          End SyncLock
                                      End Sub)
            Dim t2 As Task = Task.Run(Sub()
                                          Dim _temp As New StringBuilder
                                          ' Dim _temp As String = ""
                                          For i = breakpoit To listMsg.Count - 1
                                              _temp.Append(listMsg.Item(i).toString.Replace(Chr(10), vbNewLine) + vbNewLine)
                                          Next
                                          SyncLock lock
                                              text += _temp.ToString
                                          End SyncLock
                                      End Sub)
            t1.Wait()
            t2.Wait()

            Load2DB.Minimum = 0
            Load2DB.Maximum = listMsg.Count
            Load2DB.Value = 0
            'TextBox2.Text = text
            TextBox2.AppendText(text)
            TextBox2.Select(0, 0)
            TextBox2.ScrollToCaret()
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
