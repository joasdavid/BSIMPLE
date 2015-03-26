﻿Imports System.Data.SqlClient

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

    Public Sub execQuery(tableName As String, dataRow As DataRow)
        Connect()
        Try
            Dim sqlQuery = "SELECT * FROM " & tableName
            Dim dataAdapt As New SqlDataAdapter(sqlQuery, myConn)
            Dim table As New DataTable
            table.Rows.Add(dataRow)
            table.AcceptChanges()
            table.Dispose()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
        Disconnect()
    End Sub

    Public Function sendQuery(sqlQuery As String) As Object(,)
        Dim tableReturn(,) As Object = Nothing
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
            Next
            For i = 0 To columnsCount Step 1
                For j = 0 To columnsCount Step 1
                    values(i + 1, j) = table.Rows(i).Item(j)
                Next
            Next
            tableReturn = values
        Catch ex As Exception

        End Try
        Disconnect()
        Return tableReturn
    End Function

    Public Function sendQuery2(sqlQuery As String) As DataTable
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
