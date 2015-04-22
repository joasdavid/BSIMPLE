Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO

Module Test

    Private Property myConn As SqlConnection

    Private Property myCmd As SqlCommand

    Private Property results As Object

    Private Sub DebugCreatMSG()

        Dim listMSG = New List(Of Message)
        'readfile
        Dim oReader = New StreamReader("D:\workspace\B-simple\MindrayFull.txt", True)
        Dim strMSG As String = ""
        Dim line As String = ""
        Do While oReader.Peek() <> -1
            line = oReader.ReadLine() + Chr(10)
            If line.Chars(0) = Chr(28) Then
                Dim m = New Message(strMSG)
                listMSG.Add(m)
                strMSG = ""
            Else
                strMSG += line
            End If
        Loop
        oReader.Close()
        'end readFile
        Console.WriteLine("{0} Done . . .", listMSG.Count)
        Console.WriteLine()
        Dim op = "0"

        Console.Write(".>")
        op = Console.ReadLine()
        'Console.WriteLine("{0}XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", Integer.Parse(op))
        'Dim c As MSSQLController = MSSQLController.Instance
        Dim c As New MSSQLControllerMindray
        Dim count = 0
        For Each msg In listMSG
            c.addMSGtoDB(msg)
            count += 1

        Next


        Console.WriteLine("upload done!")
        Console.WriteLine(count)
        Console.ReadKey()
    End Sub

    Private Sub DebugGetBD_Paciente()
        Dim strConn As String = ConfigurationManager.AppSettings("StrgConn").ToString
        Dim sql As New MSSQLConnection(strConn)
        sql.sendQuery("SELECT * FROM Paciente")
    End Sub

    Sub Main()
        DebugCreatMSG()
        ''DebugGetBD_Paciente()
        'Dim strConn = ConfigurationManager.AppSettings("StrgConn").ToString
        'Dim asd As MSSQLConnection = New MSSQLConnection(strConn)
        'asd.execQuery("insert int paciente values('1123','asd','fdgh')")
        Console.ReadKey()
    End Sub


End Module