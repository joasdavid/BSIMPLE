﻿Imports System.Threading
Imports System.Net.Sockets
Imports System.Net
Imports System.Text

Public Class UDP

    Public Event OnReceiveDataUDP(ByVal dataReceived As String)

    Dim cudp As UdpClient
    Dim receivePoint As IPEndPoint
    Dim _port As Integer
    Public timeOut As Integer
    Dim workDone As Boolean = False

    Public Sub New(port As Integer)
        _port = port
        cudp = New UdpClient(_port)
        receivePoint = New IPEndPoint(IPAddress.Any, _port)
    End Sub

    Private Sub getData()
        Dim strData As String = ""
        Try
            While Not (workDone)
                Dim bt As Byte()
                bt = cudp.Receive(receivePoint)
                strData = Encoding.Unicode.GetString(bt)
                Logger.Instance.log("DB.log", "UDP/getData", strData)
                RaiseEvent OnReceiveDataUDP(strData)
            End While
        Catch ex As Exception
            Logger.Instance.log("err.log", "UDP/getData", ex.Message)
        End Try
        close()
    End Sub

    Public Sub close()
        If (workDone = False) Then
            workDone = True
            cudp.Close()
        End If
    End Sub

    Public Function start()
        Try
            Dim t = New Thread(AddressOf getData)
            t.Name = "Thread getData UDP"
            t.Start()
            Logger.Instance.log("state.log", "UDP/start", _port)
            Return t.IsAlive
        Catch ex As Exception
            Logger.Instance.log("err.log", "UDP/start", ex.Message)
            Return False
        End Try
        Return False
    End Function
End Class
