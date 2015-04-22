Imports System.Data.SqlClient

Public Class MSSQLConnection

    Dim connectionString As String
    Private Property myConn As SqlConnection
    Dim isConnected As Boolean = False

    Private debug As Boolean

    Sub New(connstring As String)
        connectionString = connstring
        debug = System.Configuration.ConfigurationManager.AppSettings("Debug")
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

    Public Function execQuery(sqlQuery As String) As Boolean
        Dim returnValue = False
        Connect()
        Try
            'Dim myCmd As SqlCommand = myConn.CreateCommand
            'myCmd.CommandText = sqlQuery
            Dim myCmd As New SqlCommand(sqlQuery, myConn)
            myCmd.Connection.Open()
            myCmd.Connection = myConn
            myCmd.ExecuteNonQuery()
            returnValue = True
            myCmd.Dispose()

            If debug Then
                Logger.Instance.log("SQL.log", "execQuery", sqlQuery)
            End If

        Catch ex As Exception
            Logger.Instance.log("err.log", "execQuery", ex.Message)
            returnValue = False
        Finally
            Disconnect()
        End Try
        Return returnValue
    End Function

    Public Function sendQuery(sqlQuery As String) As DataTable
        'Dim tableReturn(,) As Object
        Connect()
        Dim table As New DataTable
        Try
            Dim dataAdapt As New SqlDataAdapter(sqlQuery, myConn)
            dataAdapt.Fill(table)

            If debug Then
                Logger.Instance.log("SQL.log", "execQuery", sqlQuery)
            End If

            Return table
        Catch ex As Exception
            Logger.Instance.log("err.log", "execQuery", ex.Message)
        End Try
        Disconnect()
        Return Nothing
    End Function
End Class
