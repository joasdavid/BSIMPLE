Imports System.IO
Imports System.IO.StreamReader

Public Class HL7toDB

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Export2Sql.Click

    End Sub

    Private Sub OpenFileDialog1_FileOk_1(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk

        TextBox1.Text = OpenFileDialog1.FileName.ToString()


    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Select_file.Click

        Dim oReader As StreamReader
        Dim TextoLinha As String

        OpenFileDialog1.CheckFileExists = True
        OpenFileDialog1.CheckPathExists = True
        OpenFileDialog1.DefaultExt = "txt"
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
        OpenFileDialog1.Multiselect = False

        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            oReader = New StreamReader(OpenFileDialog1.FileName, True)
            Do While oReader.Peek() <> -1

                TextoLinha = TextoLinha & oReader.ReadLine() & vbNewLine

            Loop

            TextBox2.Text = TextoLinha

            oReader.Close()
        End If

    End Sub

    'Dim myStream As System.IO.Stream
    ''Dim myStream As Stream = Nothing
    'Dim openFileDialog1 As New OpenFileDialog()

    'openFileDialog1.InitialDirectory = "C:\"
    'openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
    'openFileDialog1.FilterIndex = 2
    'openFileDialog1.RestoreDirectory = True

    'If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

    '    TextBox1.Text = openFileDialog1.FileName.ToString()
    '    Try
    '        myStream = openFileDialog1.OpenFile()

    '        If (myStream IsNot Nothing) Then
    '            ' Insert code to read the stream here. 
    '        End If
    '    Catch Ex As Exception
    '        MessageBox.Show("Cannot read file from disk. Original error: " & Ex.Message)
    '    Finally
    '        ' Check this again, since we need to make sure we didn't throw an exception on open. 
    '        If (myStream IsNot Nothing) Then
    '            myStream.Close()
    '        End If
    '    End Try
    'End If


    Private Sub Load2DB_Click(sender As Object, e As EventArgs) Handles Load2DB.Click

    End Sub

    Private Sub DataFromDB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DataFromDB.SelectedIndexChanged

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub
End Class
