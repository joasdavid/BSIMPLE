Imports System.Net.Sockets
Imports System.IO
Imports System.Threading

Public Class TCP

    Public Event OnReceiveDataTCP(ByVal dataReceived As String)
    Public Event OnConnectTCP()
    Public Event OnDisconnectTCP()

    Private itcp As TcpClient
    Private etcp As TcpClient
    Private read As StreamReader
    Private write As StreamWriter
    Private ping_msg As String
    Private ping_time As Integer = -1
    Private thrd_ping As Thread
    Private thrd_read As Thread
    Private isRunning As Boolean = False
    Public connectionIP As String

    Public Sub New(ip As String, read_port As Integer, write_port As Integer)
        Try
            'Thread.Sleep(1)
            itcp = New TcpClient(ip, read_port)
            etcp = New TcpClient(ip, write_port)
            read = New StreamReader(itcp.GetStream)
            write = New StreamWriter(etcp.GetStream)
            connectionIP = ip
        Catch ex As Exception
            Logger.Instance.log("err.log", "TCP/New", ex.Message)
        End Try
    End Sub

    Public Sub New()

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
                isRunning = True
                thrd_read = New Thread(AddressOf readStream)
                thrd_read.Name = "ReadStream_" & connectionIP
                thrd_read.Start()
                thrd_ping = New Thread(AddressOf sendPing)
                thrd_ping.Name = "sendPing_" & connectionIP
                thrd_ping.Start()
                Logger.Instance.log("state.log", "TCP/start", connectionIP)
                RaiseEvent OnConnectTCP()
            Catch ex As Exception
                Logger.Instance.log("err.log", "TCP/start", ex.Message)
                Return False
            End Try
            Return True
        End If
        Return False
    End Function

    Private Sub readStream()
        Thread.Sleep(1)
        Dim buffer As String = ""
        While isRunning
            Try
                Dim data = read.Read
                buffer += Chr(data)
                If (data = 28) Then
                    RaiseEvent OnReceiveDataTCP(buffer)
                    buffer = ""
                End If
                Console.WriteLine("")
                'RaiseEvent OnReceiveDataTCP(Chr(data))
            Catch ex As IOException
                Logger.Instance.log("err.log", "TCP/readStream", ex.ToString)
                RaiseEvent OnDisconnectTCP()
                Disconnect()
            End Try
        End While
    End Sub

    Public Sub Disconnect()
        If (isRunning) Then
            thrd_ping.Abort()
            read.Close()
            write.Close()
            isRunning = False
            etcp.Close()
            itcp.Close()
            Logger.Instance.log("state.log", "TCP/Disconnect", connectionIP)
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

    Public Function isAnyConnectionOpen() As Boolean
        Return itcp.Connected Or etcp.Connected Or isRunning
    End Function

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If
        Dim p As TCP = CType(obj, TCP)
        Return Me.connectionIP = p.connectionIP
    End Function

End Class
