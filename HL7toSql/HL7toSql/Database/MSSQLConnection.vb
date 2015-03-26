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
        Connect()
        Try
            'Dim myCmd As SqlCommand = myConn.CreateCommand
            'myCmd.CommandText = sqlQuery
            Dim myCmd As New SqlCommand(sqlQuery, myConn)
            myCmd.Connection.Open()
            myCmd.Connection = myConn
            myCmd.ExecuteNonQuery()
            myCmd.Dispose()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
        Disconnect()
    End Sub

    Public Function sendQuery(sqlQuery As String) As DataTable
        'Dim tableReturn(,) As Object
        Connect()
        Dim table As New DataTable
        Try
            Dim dataAdapt As New SqlDataAdapter(sqlQuery, myConn)


            'dataAdapt = New SqlDataAdapter(sqlQuery, myConn)
            dataAdapt.Fill(table)
            'tableReturn = values
            Return table
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
        Disconnect()
        Return Nothing
    End Function
End Class
