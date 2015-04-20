Imports System.Net.Sockets
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
    'Private timeOut = 3000
    Private pingTime As Integer = 7000
    Private buffer As String
    Private isUDP_ON = False
    Private ipList As List(Of TCP)
    Private msg As Message = Nothing
    Private ReadOnly lock As New Object

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
        ipList = New List(Of TCP)
    End Sub

    Public Function getBedIP() As String
        Dim copy = str_ip
        Return copy
    End Function

    Private Sub receiveBedIP(data As String)
        Dim mensagem As New MessageADT(data)
        Dim aux As String = mensagem.getSegmentField("PV1", 2)
        Dim array_ip As String() = aux.Split(New Char() {"&"c})

        Dim dec As Int64 = array_ip(2)
        Dim bytes As Byte() = BitConverter.GetBytes(dec)
        Array.Reverse(bytes)
        Dim ip As String = ""
        For Each ipval In bytes
            If ipval <> 0 Then
                ip += ipval & "."
            End If
        Next
        ip = ip.Substring(0, ip.Length - 1)
        tcp = Nothing
        'sph.WaitOne()
        SyncLock lock

            
            tcp = New TCP(ip, _portr, _portw)
            If (Not (ipList.Contains(tcp)) And tcp.connectionIP <> Nothing) Then
                Task.Run(Sub()
                             MsgBox("ip:" & ip)
                         End Sub)
                ipList.Add(tcp)
                AddHandler tcp.OnReceiveDataTCP, AddressOf stratReadingStream
                AddHandler tcp.OnDisconnectTCP, AddressOf monitorDisconnect
                tcp.setPing(packingLLP("MSH|^~\&|||||||ORU^R01|106|P|2.3.1|"), pingTime)
                tcp.start()

                tcp.send(packingLLP("MSH|^~\&|||||||QRY^R02|1203|P|2.3.1" & cr & _
                                    "QRD|20060731145557|R|I|Q895211|||||RES" & cr & _
                                    "QRF|MON||||0&0^1^1^1^" & cr & _
                                    "QRF|MON||||0&0^3^1^1^" & cr))
            End If
        End SyncLock
        'sph.Release()
        'Thread.Sleep(10000)
        'str_ip = "192.168.1.35"
    End Sub

    Public Sub Connect()
        'Dim sw As New Stopwatch
        'sw.Start()
        udp = New UDP(4600)
        AddHandler udp.OnReceiveDataUDP, AddressOf receiveBedIP
        udp.start()
        isUDP_ON = True
        'udp.close()
        'While str_ip = ""
        '    str_ip = getBedIP()
        '    If (str_ip = "") Then
        '        'If (sw.ElapsedMilliseconds > timeOut) Then
        '        '    Throw New Exception
        '        'End If
        '    Else
        '        Exit While
        '    End If
        'End While
        'udp.close()
        'tcp = New TCP(str_ip, _portr, _portw)
        'AddHandler tcp.OnReceiveDataTCP, AddressOf stratReadingStream
        'tcp.setPing(packingLLP("MSH|^~\&|||||||ORU^R01|106|P|2.3.1|"), 7000)
        'tcp.start()

        'tcp.send(packingLLP("MSH|^~\&|||||||QRY^R02|1203|P|2.3.1" & cr & _
        '                    "QRD|20060731145557|R|I|Q895211|||||RES" & cr & _
        '                    "QRF|MON||||0&0^1^1^1^" & cr & _
        '                    "QRF|MON||||0&0^3^1^1^" & cr))


    End Sub

    Public Sub Disconnect()
        SyncLock lock
            For i = 0 To ipList.Count - 1
                ipList.Item(i).Disconnect()
            Next
        End SyncLock
        If (isUDP_ON = True) Then
            udp.close()
            isUDP_ON = False
        End If
    End Sub

    Private Sub timeout()
        Thread.Sleep(5000)
        SyncLock lock
            If (ipList.Count = 0) Then
                Disconnect()
            End If
        End SyncLock
    End Sub

    Private Sub monitorDisconnect(ip As TCP)
        'sph.WaitOne()
        SyncLock lock
            If (ipList.Contains(ip)) Then
                ipList.Remove(ip)
            End If
            If (ipList.Count = 0) Then
                Task.Run(AddressOf timeout)
            End If
        End SyncLock
        'sph.Release()
    End Sub

    Public Sub stratReadingStream(data As Char)
        Try
            If (data = vt) Then 'head
                msg = New Message()
                buffer = data
            ElseIf (data = fs) Then 'trailer
                msg.parseData(buffer & data)
                RaiseEvent OnReceiveMSG(msg)
            End If
            buffer += data
        Catch ex As Exception
            Logger.Instance.log("err.log", "TCP/IP getData", ex.Message)
        End Try
    End Sub

    Private Function packingLLP(msg As String) As String
        Dim copy = vt & msg & fs & cr
        Return copy
    End Function


End Class
