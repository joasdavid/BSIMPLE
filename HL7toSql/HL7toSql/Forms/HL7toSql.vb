Imports System.IO
Imports System.IO.StreamReader
Imports System.Data.SqlClient
Imports System.Configuration



Public Class HL7toDB
    Private myConn As SqlConnection
    Private myCmd As SqlCommand
    Private da As SqlDataAdapter
    Private ds As DataSet
    Private listMsg As New List(Of Message)


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim strConn As String = ConfigurationManager.AppSettings("StrgConn").ToString
        myConn = New SqlConnection(strConn)
        Dim SQL As String = "SELECT * FROM ViewDataGridView"
        'Atualiza dataset
        da = New SqlDataAdapter(SQL, myConn)
        'coloca a infomação em memoria
        ds = New DataSet
        'coloca a informação defenida no dataset
        da.Fill(ds, "ViewDataGridView")
        ' Define a DataSet é a fonte de dados do datagridview
        Me.DataGridView1.DataSource = ds.Tables("ViewDataGridView")
        'Limpa a ligação à base de dados
        myConn = Nothing

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Export2Sql.Click

    End Sub

    Private Sub OpenFileDialog1_FileOk_1(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk

        TextBox1.Text = OpenFileDialog1.FileName.ToString()


    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Select_file.Click

        Dim oReader As StreamReader
        Dim TextoLinha As String
        Dim c28 As Char = Chr(28)

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
        'Do While oReader.Peek() <> -1
        '    Dim Linha = oReader.ReadLine()
        '    If (Linha.Chars(0) = c28) Then
        '        Dim asd As New Message()
        '        asd.parseData(TextoLinha)
        '        listMsg.Add(asd)
        '        TextoLinha = ""
        '    Else
        '        TextoLinha = TextoLinha & Linha & vbNewLine
        '    End If
        'Loop

        'TextBox2.Text = listMsg.Item(0).ToString
    End Sub
    Private Sub Load2DB_Click(sender As Object, e As EventArgs) Handles Load2DB.Click

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        Label1.Text = Val(Label1.Text) + 1
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs)

        Label1.Text = Val(Label1.Text) - 1
        If (Val(Label1.Text < 1)) Then
            Label1.Text = 1
        End If
    End Sub
End Class
