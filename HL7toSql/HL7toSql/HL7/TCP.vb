Imports System.Net.Sockets
Imports System.IO
Imports System.Threading

Public Class TCP

    Public Event OnReceiveDataTCP(ByVal dataReceived As Char)

    Private itcp As TcpClient
    Private etcp As TcpClient
    Private read As StreamReader
    Private write As StreamWriter
    Private ping_msg As String
    Private ping_time As Integer = -1
    Private thrd_ping As Thread
    Private thrd_read As Thread

    Public Sub New(ip As String, read_port As Integer, write_port As Integer)
        Try
            itcp = New TcpClient(ip, read_port)
            etcp = New TcpClient(ip, write_port)
            etcp = New TcpClient("", write_port)
            read = New StreamReader(itcp.GetStream)
            write = New StreamWriter(etcp.GetStream)
        Catch ex As Exception
            Logger.Instance.log("err.log", "TCP/New", ex.Message)
        End Try
    End Sub

    Public Sub send(str As String)
        Try
            write.WriteLine(str)
            write.Flush()
        Catch ex As Exception
            Logger.Instance.log("err.log", "TCP/send", ex.Message)
        End Try
    End Sub

    Public Function start()
        If (itcp.Connected And etcp.Connected) Then
            Try
                thrd_read = New Thread(AddressOf readStrem)
                thrd_read.Start()
                thrd_ping = New Thread(AddressOf sendPing)
                thrd_ping.Start()
            Catch ex As Exception
                Logger.Instance.log("err.log", "TCP/start", ex.Message)
                Return False
            End Try
            Return True
        End If
        Return False
    End Function

    Private Sub readStrem()
        While True
            Dim data = read.Read
            RaiseEvent OnReceiveDataTCP(Chr(data))
        End While
    End Sub

    Public Sub Disconnect()
        read.Close()
        write.Close()
        etcp.Close()
        itcp.Close()
    End Sub

    Public Sub setPing(str As String, time As Integer)
        ping_msg = str
        ping_time = time
    End Sub

    Private Sub sendPing()
        While ping_time > 0
            send(ping_msg)
            Thread.Sleep(ping_time)
        End While
    End Sub

End Class
