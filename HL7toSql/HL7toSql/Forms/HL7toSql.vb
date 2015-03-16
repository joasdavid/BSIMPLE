Public Class HL7toDB

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Export2Sql.Click

    End Sub

    Private Sub OpenFileDialog1_FileOk_1(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk

    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Select_file.Click
        Dim myStream As System.IO.Stream
        'Dim myStream As Stream = Nothing
        Dim openFileDialog1 As New OpenFileDialog()

        openFileDialog1.InitialDirectory = "C:\"
        openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
        openFileDialog1.FilterIndex = 2
        openFileDialog1.RestoreDirectory = True

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

            TextBox1.Text = openFileDialog1.FileName.ToString()
            Try
                myStream = openFileDialog1.OpenFile()

                If (myStream IsNot Nothing) Then
                    ' Insert code to read the stream here. 
                End If
            Catch Ex As Exception
                MessageBox.Show("Cannot read file from disk. Original error: " & Ex.Message)
            Finally
                ' Check this again, since we need to make sure we didn't throw an exception on open. 
                If (myStream IsNot Nothing) Then
                    myStream.Close()
                End If
            End Try
        End If
    End Sub

    Private Sub ShowDatehl7_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ShowDatehl7.SelectedIndexChanged

    End Sub

    Private Sub Load2DB_Click(sender As Object, e As EventArgs) Handles Load2DB.Click

    End Sub

    Private Sub DataFromDB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DataFromDB.SelectedIndexChanged

    End Sub
End Class
