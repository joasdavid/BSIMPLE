Imports System.Net.Sockets
Imports System.IO
Imports System.Threading

Public Class TCP

    Public Event OnReceiveDataTCP(ByVal dataReceived As Char)
    Public Event OnConnectTCP(ByVal ip As String)
    Public Event OnDisconnectTCP(ByVal ip As TCP)

    Private itcp As TcpClient
    Private etcp As TcpClient
    Private read As StreamReader
    Private write As StreamWriter
    Private ping_msg As String
    Private ping_time As Integer = -1
    Private thrd_ping As Thread
    Private thrd_read As Thread
    Private isRunning As Boolean = False
    Public ReadOnly connectionIP As String

    Public Sub New(ip As String, read_port As Integer, write_port As Integer)
        Try
            itcp = New TcpClient(ip, read_port)
            etcp = New TcpClient(ip, write_port)
            read = New StreamReader(itcp.GetStream)
            write = New StreamWriter(etcp.GetStream)
            connectionIP = ip
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
                thrd_read = New Thread(AddressOf readStream)
                thrd_read.Name = "Thread readStream TCP"
                thrd_read.Start()
                thrd_ping = New Thread(AddressOf sendPing)
                thrd_ping.Name = "Thread sendPing TCP"
                thrd_ping.Start()
                isRunning = True
                Logger.Instance.log("state.log", "TCP/start", connectionIP)
                RaiseEvent OnConnectTCP(connectionIP)
            Catch ex As Exception
                Logger.Instance.log("err.log", "TCP/start", ex.Message)
                Return False
            End Try
            Return True
        End If
        Return False
    End Function

    Private Sub readStream()
        While True
            Try
                Dim data = read.Read
                RaiseEvent OnReceiveDataTCP(Chr(data))
            Catch ex As IOException
                Logger.Instance.log("err.log", "TCP/readStream", ex.Message)
                Disconnect()
            End Try
        End While
    End Sub

    Public Sub Disconnect()
        If (isRunning) Then
            thrd_ping.Abort()
            read.Close()
            write.Close()
            etcp.Close()
            itcp.Close()
            isRunning = False
            Logger.Instance.log("state.log", "TCP/Disconnect", connectionIP)
            RaiseEvent OnDisconnectTCP(Me)
            thrd_read.Abort()
        End If
    End Sub

    Public Sub setPing(str As String, time As Integer)
        ping_msg = str
        ping_time = time
    End Sub

    Private Sub sendPing()
        While ping_time >= 0
            Thread.Sleep(ping_time)
            send(ping_msg)
            Logger.Instance.log("ping.log", "", ping_msg)
        End While
    End Sub

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If
        Dim p As TCP = CType(obj, TCP)
        Return Me.connectionIP = p.connectionIP
    End Function

End Class
