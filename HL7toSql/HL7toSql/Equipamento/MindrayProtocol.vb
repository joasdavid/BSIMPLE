﻿Imports System.Net.Sockets
Imports System.IO
Imports System.Threading

Public Class MindrayProtocol

    Public Event OnReceiveMSG(ByVal dataReceived As Message)

    Public str_ip As String
    Private _portw As Integer
    Private _portr As Integer
    Private _thread As Thread
    Private udp As UDP
    Private tcp As TCP
    Private timeOut = 3000
    Private buffer As String
    Dim msg As Message = Nothing

    Private fs As Char = Chr(28)
    Private cr As Char = Chr(13)
    Private nl As Char = Chr(10)
    Private vt As Char = Chr(11)



    Public Sub New()
        Me.New("", 4601, 4678)
    End Sub

    Public Sub New(portr As Integer, portw As Integer)
        Me.New("", portr, portw)
    End Sub

    Public Sub New(ip As String, portr As Integer, portw As Integer)
        str_ip = ip
        _portr = portr
        _portw = portw
    End Sub

    Public Function getBedIP() As String
        Return str_ip
    End Function

    Private Sub receiveBedIP(data As String)
        Dim mensagem As New MessageADT(data)
        Dim aux As String = mensagem.getSegmentField("PV1", 2)
        Dim array_ip As String() = aux.Split(New Char() {"&"c})

        Dim dec As Int64 = array_ip(1)
        Dim bytes As Byte() = BitConverter.GetBytes(dec)
        Array.Reverse(bytes)
        Dim ip As String
        For Each ipval In bytes
            If ipval <> 0 Then
                ip += ipval & "."
            End If
        Next
        ip = ip.Substring(0, ip.Length - 1)
        str_ip = ip
        'str_ip = "192.168.1.35"
    End Sub

    Public Sub Connect()
        Dim sw As New Stopwatch
        sw.Start()
        udp = New UDP(4600)
        AddHandler udp.OnReceiveDataUDP, AddressOf receiveBedIP
        udp.start()
        While str_ip = ""
            str_ip = getBedIP()
            If (str_ip = "") Then
                If (sw.ElapsedMilliseconds > timeOut) Then
                    Throw New Exception
                End If
            Else
                Exit While
            End If
        End While
            udp.close()

            tcp = New TCP(str_ip, _portr, _portw)
            AddHandler tcp.OnReceiveDataTCP, AddressOf stratReadingStream
            tcp.setPing("", 1000)
            tcp.send("")
            tcp.start()


    End Sub

    Public Sub stratReadingStream(data As Char)
        Try
            If (data = vt) Then
                msg = New Message
            ElseIf (data = fs) Then
                RaiseEvent OnReceiveMSG(msg)
                Logger.Instance.log("connections.log", "MSG", "get one new message")
                Logger.Instance.log("msgUpload.log", msg.getSegmentField("MSH", 8), msg.toString)

            ElseIf (data = cr Or data = nl) Then
                buffer += "" + data
                msg.parseData(buffer)
                buffer = ""
            End If
            buffer += "" + data
        Catch ex As Exception
            Logger.Instance.log("err.log", "TCP/IP getData", ex.Message)
        End Try
    End Sub


End Class
