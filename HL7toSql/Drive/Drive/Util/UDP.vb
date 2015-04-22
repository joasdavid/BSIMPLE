Imports System.Threading
Imports System.Net.Sockets
Imports System.Net
Imports System.Text

Public Class UDP

#Region "EVENTS"
    Public Event OnReceiveDataUDP(ByVal dataReceived As String)
#End Region

    Private cudp As UdpClient
    Private receivePoint As IPEndPoint
    Private _port As Integer
    Public timeOut As Integer
    Private workDone As Boolean = False

    Private debug As Boolean


    Public Sub New(port As Integer)
        _port = port
        cudp = New UdpClient(_port)
        receivePoint = New IPEndPoint(IPAddress.Any, _port)
        debug = System.Configuration.ConfigurationManager.AppSettings("Debug")
    End Sub
    Public Sub close()
        If (workDone = False) Then
            workDone = True
            cudp.Close()
        End If
    End Sub
    Public Function start()
        Try
            If (debug = "true") Then
                Logger.Instance.log("state.log", "UDP/start", _port)
            End If

            Dim t = New Thread(AddressOf getData)
            t.Name = "Thread getData UDP"
            t.Start()
            Return t.IsAlive

        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function
    Private Sub getData()
        Dim strData As String = ""
        Try
            While Not (workDone)
                Dim bt As Byte()
                bt = cudp.Receive(receivePoint)
                strData = Encoding.Unicode.GetString(bt)
                RaiseEvent OnReceiveDataUDP(strData)
            End While
        Catch ex As Exception
            Logger.Instance.log("err.log", "UDP/getData", ex.Message)
        End Try
        close()
    End Sub
End Class
