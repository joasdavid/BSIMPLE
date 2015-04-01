Imports System.IO
Imports System.IO.StreamReader
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Threading
Imports System.Text



Public Class HL7toDB
    Private myConn As SqlConnection
    Private myCmd As SqlCommand
    Private da As SqlDataAdapter
    Private ds As DataSet
    Private listMsg As New List(Of Message)
    Private c28 As Char = Chr(28)
    Private ReadOnly lock As New Object

    Private Sub showBDcontent()
        Me.DataGridView1.DataSource = MSSQLControllerMindray.Instance.getTable("Monitorizacao").Tables(0)
        Me.DataGridView1.Columns(3).DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss:fff"
        Me.DataGridView1.Columns(4).DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss:fff"
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        showBDcontent()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Export2Sql.Click

        Dim controler As MSSQLControllerMindray = MSSQLControllerMindray.Instance
        Dim max = Load2DB.Maximum
        Dim inc = max / listMsg.Count
        For Each envia In listMsg
            If (envia.Valide()) Then
                controler.addMSGtoDB(envia)
            End If
            Load2DB.Increment(inc)
        Next
        showBDcontent()

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
        TextBox2.Hide()
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            oReader = New StreamReader(OpenFileDialog1.FileName, True)

            Dim strMSG As String = ""
            Dim line As String = ""
            Dim text As String = ""
            Dim countR As Integer = 0
            Dim m = New Message()
            Dim w As Stopwatch = Stopwatch.StartNew
            Do While oReader.Peek() <> -1
                Try
                    line = oReader.ReadLine() + Chr(10)
                    If line.Chars(0) = Chr(28) Then
                        listMsg.Add(m)
                        m = New Message()
                        TextBox2.AppendText(vbNewLine)
                    Else
                        m.parseData(line)
                        TextBox2.AppendText(line & vbNewLine)
                    End If
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            Loop

            oReader.Close()

            

            Load2DB.Minimum = 0
            Load2DB.Maximum = listMsg.Count
            Load2DB.Value = 0

            'MsgBox(w.Elapsed.Milliseconds)
            Dim w2 = Stopwatch.StartNew

            'TextBox2.Text = text
            TextBox2.AppendText(text)
            TextBox2.Show()
            w.Stop()
            MsgBox(w.Elapsed.Milliseconds)
        End If
    End Sub

End Class
