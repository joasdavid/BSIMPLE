﻿Imports System.IO
Imports System.IO.StreamReader
Imports System.Data.SqlClient
Imports System.Configuration



Public Class HL7toDB
    Private myConn As SqlConnection
    Private myCmd As SqlCommand
    Private da As SqlDataAdapter
    Private ds As DataSet
    Private listMsg As New List(Of Message)
    Private c28 As Char = Chr(28)

    Private Sub showBDcontent()
        Dim strConn As String = ConfigurationManager.AppSettings("StrgConn").ToString
        myConn = New SqlConnection(strConn)
        Dim SQL As String = "SELECT IdPaciente,IdOBX,Sub_id,Valor,DataInicio,DataFinal FROM Valores"
        'Atualiza dataset
        da = New SqlDataAdapter(SQL, myConn)
        'coloca a infomação em memoria
        ds = New DataSet
        'coloca a informação defenida no dataset
        da.Fill(ds, "Valores")
        ' Define a DataSet é a fonte de dados do datagridview
        Me.DataGridView1.DataSource = ds.Tables("Valores")
        'Limpa a ligação à base de dados
        myConn = Nothing



    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        showBDcontent()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Export2Sql.Click
        Dim controler As MSSQLControllerMindray = MSSQLControllerMindray.Instance

        For Each envia In listMsg
            If (envia.Valide()) Then
                controler.addMSGtoDB(envia)
            End If
        Next
        showBDcontent()
    End Sub

    Private Sub OpenFileDialog1_FileOk_1(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk

        TextBox1.Text = OpenFileDialog1.FileName.ToString()


    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Select_file.Click

        Dim oReader As StreamReader
        Dim TextoLinha As String = ""


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


            '    Do While oReader.Peek() <> -1
            '        Dim Linha = oReader.ReadLine() + Chr(10)
            '        If (Linha.Chars(0) = c28) Then
            '            Dim asd As New Message()
            '            asd.parseData(TextoLinha)
            '            If (asd.Valide) Then
            '                listMsg.Add(asd)
            '            End If
            '            TextoLinha = ""
            '        Else
            '            TextoLinha += Linha '+ Chr(10)
            '        End If
            '    Loop
            '    oReader.Close()
            'End If
            'Dim oReader = New StreamReader("C:\Users\Tiago\Copy ventiago@gmail.com\IPCA\3Ano\Estagio\MindrayMini.txt", True)
            Dim strMSG As String = ""
            Dim line As String = ""
            Do While oReader.Peek() <> -1
                line = oReader.ReadLine() + Chr(10)
                If line.Chars(0) = Chr(28) Then
                    Dim m = New Message(strMSG)
                        listMsg.Add(m)
                        strMSG = ""
                        TextBox2.Text += vbNewLine
                    Else
                        strMSG += line
                        TextBox2.Text += line + vbNewLine
                    End If
            Loop
            oReader.Close()
            End If
    End Sub
    Private Sub Load2DB_Click(sender As Object, e As EventArgs) Handles Load2DB.Click

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

End Class
