Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Module Test2
    Private Property myConn As SqlConnection

    Private Property myCmd As SqlCommand

    Private Property results As Object
    Private Sub DebugCreatMSG()

        Dim numero As String
        Dim text As String


        'readfile
        Dim oReader = New StreamReader("D:\workspace\B-simple\REP2\Alarmes.txt", True)
        Dim strMSG As String = ""
        Dim line As String = ""

        Do While oReader.Peek() <> -1
            line = oReader.ReadLine
            numero = line.Substring(0, 5)
            text = line.Replace((numero & " "), "").Replace("'", "")
            myConn = New SqlConnection(ConfigurationManager.AppSettings("StrgConn").ToString)
            Dim sql = "insert into Alarme (IdAlarme, Descricao) values(" & numero & ",'" & text & "')"
            myCmd = New SqlCommand(sql, myConn)
            myCmd.Connection.Open()
            myCmd.ExecuteNonQuery()
            myCmd.Dispose()
            myConn.Close()
        Loop

        oReader.Close()
        'end readFile

        Console.ReadKey()
    End Sub
    Private Sub DebugCreatSV()

        Dim numero As String
        Dim text As String


        'readfile
        Dim oReader = New StreamReader("D:\workspace\B-simple\REP2\SV.txt", True)
        Dim strMSG As String = ""
        Dim line As String = ""
        Dim count As Integer = 0
        Do While oReader.Peek() <> -1
            line = oReader.ReadLine
            count = 0
            For i = 0 To line.Length()
                If (line.Chars(i) = " ") Then
                    numero = line.Substring(0, count)
                    Exit For
                End If
                count += 1
            Next
            For j = count + 1 To line.Length()
                Dim c = line.Chars(j)
                If (c = " ") Then
                    text = line.Substring(0, count + 1).Replace(numero, "")
                    Exit For
                End If
                count += 1
            Next
            'numero = line.Substring(0, 5)
            'text = line.Replace((numero & " "), "").Replace("'", "")
            myConn = New SqlConnection(ConfigurationManager.AppSettings("StrgConn").ToString)
            Dim sql = "insert into SinaisVitais (IdSV, Descricao) values(" & numero & ",'" & text & "')"
            myCmd = New SqlCommand(sql, myConn)
            myCmd.Connection.Open()
            myCmd.ExecuteNonQuery()
            myCmd.Dispose()
            myConn.Close()
        Loop

        oReader.Close()
        'end readFile

        Console.ReadKey()
    End Sub

    Private Sub DebugCreatSVu()

        Dim numero As String
        Dim text As String


        'readfile
        Dim oReader = New StreamReader("D:\workspace\B-simple\REP2\SV2.txt", True)
        Dim strMSG As String = ""
        Dim line As String = ""
        Dim count As Integer = 0
        Do While oReader.Peek() <> -1
            line = oReader.ReadLine
            count = 0
            For i = 0 To line.Length()
                If (line.Chars(i) = " ") Then
                    numero = line.Substring(0, count)
                    Exit For
                End If
                count += 1
            Next
            text = line.Replace((numero & " "), "")
            If text.Chars(text.Length - 1) = " " Then
                text = text.Substring(0, text.Length - 1)
            End If
            'numero = line.Substring(0, 5)
            'text = line.Replace((numero & " "), "").Replace("'", "")
            myConn = New SqlConnection(ConfigurationManager.AppSettings("StrgConn").ToString)
            Dim sql = "UPDATE SinaisVitais set Descricao ='" & text & "' where idSV =" & numero
            myCmd = New SqlCommand(sql, myConn)
            myCmd.Connection.Open()
            myCmd.ExecuteNonQuery()
            myCmd.Dispose()
            myConn.Close()
        Loop

        oReader.Close()
        'end readFile

        Console.ReadKey()
    End Sub
    Sub Main()
        'DebugCreatMSG()
        DebugCreatSVu()
    End Sub

End Module
