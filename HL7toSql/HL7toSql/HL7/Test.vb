Imports System.Data.SqlClient

Module Test

    Private Property myConn As SqlConnection

    Private Property myCmd As SqlCommand

    Private Property results As Object

    Sub Main()
        myConn = New SqlConnection("Initial Catalog=HL7Mindray;" & _
                "Data Source=NETM4NULTRABOOK;Integrated Security=SSPI;")

        myCmd = myConn.CreateCommand
        myCmd.CommandText = "SELECT IdPaciente, Last_Name_Paciente FROM Paciente"
        myConn.Open()

        Dim myReader As SqlDataReader = myCmd.ExecuteReader()
        Do While myReader.Read()
            results = results & myReader.GetString(1) & ", " & myReader.GetInt32(0) & vbLf
        Loop
        'Display results.
        MsgBox(results)
        myConn.Close()
        
    End Sub

    Private Function myReader() As Object
        Throw New NotImplementedException
    End Function

End Module
