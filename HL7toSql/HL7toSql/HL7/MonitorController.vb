Public Class MonitorController

    Public Event OnClose(ByVal ip As String)
    Public Event OnGotNewMSG(ByVal msg As Message)

    Public ReadOnly monitorIP As String
    Private idPaciente As String
    Private databaseController As MSSQLControllerMindray

    Private readPort As Integer
    Private writePort As Integer

    Private tcp As TCP
    Private pingTime As Integer = 7000

    Private fs As Char = Chr(28)
    Private cr As Char = Chr(13)
    Private nl As Char = Chr(10)
    Private vt As Char = Chr(11)


    Public Sub New(ip As String, read_port As Integer, write_port As Integer)
        monitorIP = ip
        readPort = read_port
        writePort = write_port
        databaseController = New MSSQLControllerMindray
    End Sub

    Public Sub start()
        tcp = New TCP(monitorIP, readPort, writePort)
        AddHandler tcp.OnReceiveDataTCP, AddressOf tcpGetData
        AddHandler tcp.OnDisconnectTCP, AddressOf tcpDisconnect
        tcp.setPing(packingLLP("MSH|^~\&|||||||ORU^R01|106|P|2.3.1|"), pingTime)
        tcp.start()

        tcp.send(packingLLP("MSH|^~\&|||||||QRY^R02|1203|P|2.3.1" & cr & _
                            "QRD|20060731145557|R|I|Q895211|||||RES" & cr & _
                            "QRF|MON||||0&0^1^1^1^" & cr & _
                            "QRF|MON||||0&0^3^1^1^" & cr))
    End Sub

    Public Sub close()
        If (tcp IsNot Nothing) Then
            If (tcp.isAnyConnectionOpen) Then
                tcp.Disconnect()
            End If
        End If
        RaiseEvent OnClose(monitorIP)
    End Sub

    Public Sub setPingTimeOut(ms As Integer)
        pingTime = ms
    End Sub

    Private Function packingLLP(msg As String) As String
        Dim copy = vt & msg & fs & cr
        Return copy
    End Function

    Private Sub tcpDisconnect()
        close()
    End Sub

    Private Sub tcpGetData(data As String)
        Dim msg As New Message(data)
        If (msg.Valide) Then
            databaseController.addMSGtoDB(msg)
            RaiseEvent OnGotNewMSG(msg)
        End If
    End Sub

    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If
        Dim p As MonitorController = CType(obj, MonitorController)
        Return Me.monitorIP = p.monitorIP
    End Function

End Class
