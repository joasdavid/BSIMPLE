Imports System.Data.SqlClient

Public Class MSSQLConnection

    Dim connectionString As String
    Private Property myConn As SqlConnection
    Dim isConnected As Boolean = False

    Sub New(connstring As String)
        connectionString = connstring
    End Sub

    Private Sub Connect()
        Try
            myConn = New SqlConnection(connectionString)
            isConnected = True
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Disconnect()
        Try
            myConn.Close()
            isConnected = False
        Catch ex As Exception

        End Try
    End Sub

    Public Sub execQuery(sqlQuery As String)
        Dim myCmd As SqlCommand = myConn.CreateCommand
        myCmd.CommandText = "SELECT IdPaciente, Last_Name_Paciente FROM Paciente"
        myCmd.ExecuteNonQuery()
        myCmd.Dispose()
    End Sub

    Public Function sendQuery(sqlQuery As String) As String(,)
        Dim tableReturn(,) As String
        Connect()
        Try
            Dim dataAdapt As New SqlDataAdapter(sqlQuery, myConn)
            Dim table As New DataTable

            'dataAdapt = New SqlDataAdapter(sqlQuery, myConn)
            dataAdapt.Fill(table)

            Dim columnsCount As Integer = table.Columns.Count - 1
            Dim rowsCount As Integer = table.Rows.Count

            Dim values(rowsCount, columnsCount) As Object

            ' nome das colunas
            For i = 0 To columnsCount Step 1
                values(0, i) = table.Columns.Item(i).Caption
                Console.WriteLine(values(0, i))
            Next
            For i = 0 To columnsCount Step 1
                For j = 0 To columnsCount Step 1
                    values(i + 1, j) = table.Rows(i).Item(j)
                    Console.WriteLine(values(i + 1, j))
                Next
            Next
            tableReturn = values
        Catch ex As Exception

        End Try
        Disconnect()
        Return tableReturn
    End Function

End Class
