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
    Private monitor As MonitorController
    'Private timeOut = 3000
    Private isUDP_ON = False
    Private monitorList As List(Of MonitorController)
    Private msg As Message = Nothing
    Private ReadOnly lock As New Object
    Private state As String = "UDP"
    



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
        monitorList = New List(Of MonitorController)
    End Sub


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
        addNewMonitor(ip)
        'str_ip = "192.168.1.35"
    End Sub

    Public Sub addNewMonitor(ip As String)
                     monitor = Nothing
                     'sph.WaitOne()
                     monitor = New MonitorController(ip, _portr, _portw)
                     SyncLock lock
                         If (Not (monitorList.Contains(monitor))) Then
                             AddHandler monitor.OnGotNewMSG, AddressOf gotNewMSG
                             AddHandler monitor.OnClose, AddressOf monitorDisconnect
                             monitor.start()
                             monitorList.Add(monitor)
                         End If
                     End SyncLock
    End Sub

    Private Sub gotNewMSG(msg As Message)
        RaiseEvent OnReceiveMSG(msg)
    End Sub

    Public Sub Connect()

        Select Case state
            Case "UDP"
                udp = New UDP(4600)
                AddHandler udp.OnReceiveDataUDP, AddressOf receiveBedIP
                udp.start()
                isUDP_ON = True
            Case "P2P"
                If (str_ip <> "") Then
                    addNewMonitor(str_ip)
                End If
        End Select

    End Sub

    Public Sub setState(state As String)
        Me.state = state
    End Sub

    Public Sub Disconnect()
        SyncLock lock
            For i = 0 To monitorList.Count - 1
                monitorList.Item(i).close()
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
            If (monitorList.Count = 0) Then
                Disconnect()
            End If
        End SyncLock
    End Sub

    Private Sub monitorDisconnect(ip As String)
        Thread.Sleep(1)
        Dim monitorOFF As New MonitorController(ip, 0, 0)
        SyncLock lock
            If (monitorList.Contains(monitorOFF)) Then
                monitorList.Remove(monitorOFF)
            End If
            If (monitorList.Count = 0) Then
                Task.Run(AddressOf timeout)
            End If
        End SyncLock
    End Sub



End Class
