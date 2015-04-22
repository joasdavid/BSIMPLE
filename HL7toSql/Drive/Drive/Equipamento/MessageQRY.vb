Public Class MessageQRY
    Inherits MessageHL7

    Private MSH(19) As String
    Private QRD(12) As String
    Private QRF(40, 9) As String

    Private haveMSH As Integer = 0
    Private haveQRD As Integer = 0
    Private haveQRF As Integer = 0

    Private internalID
    Private Shared sharedID = 0

    Sub New(data As String)
        Me.New()
        parseData(data)
    End Sub
    Sub New()
        sharedID += 1
        internalID = sharedID
    End Sub
    Public Shared Function CreadNewQRY() As MessageQRY
        Dim qry As New MessageQRY
        qry.parseData("MSH|^~\&|||||||QRY^R02|1203|P|2.3.1" & Chr(13))
        Dim time = DateTime.Now.ToString("yyyyMMddHHmmss")
        Dim seqID = "Q" & sharedID
        qry.parseData("QRD|" & time & "|R|I| " & seqID & "|||||RES" & Chr(13))
        qry.parseData("QRF|MON||||0&0^1^1^1^" & Chr(13))
        qry.parseData("QRF|MON||||0&0^3^1^1^" & Chr(13))

        Return qry
    End Function

    Protected Overrides Function addToHeader(header As String, buffer As String, pos As Integer) As String
        If (pos = 0) Then
            header = buffer  'header = buffer para posição 0
            Return header
        End If

        If (buffer = "") Then
            Return header
        End If

        If (header = "MSH") Then
            MSH(pos - 1) = buffer
        ElseIf (header = "QRD") Then
            QRD(pos - 1) = buffer
        ElseIf (header = "QRF") Then
            Dim counter1, counter2 As Integer
            Try
                counter1 = haveQRF
                counter2 = pos - 1
                QRF(counter1, counter2) = buffer
            Catch ex As Exception
                counter1 = 0
                counter2 = 0
            End Try
        End If

        Return header
    End Function
    Public Overrides Function getSegmentCont(seg As String) As Integer
        If (seg = "MSH") Then
            Return haveMSH
        ElseIf (seg = "QRD") Then
            Return haveQRD
        ElseIf (seg = "QRF") Then
            Return haveQRF
        End If
        Return 0
    End Function
    Public Overloads Overrides Function getSegmentField(seg As String, pos As Integer) As String
        If (seg = "MSH") Then
            Return MSH(pos)
        ElseIf (seg = "QRD") Then
            Return QRD(pos)
        End If
        Return ""
    End Function
    Public Overloads Overrides Function getSegmentField(seg As String, segN As Integer, pos As Integer) As String
        If (seg = "QRF") Then
            Return QRF(segN, pos)
        End If
        Return ""
    End Function
    Protected Overrides Sub haveOneMore(seg As String)
        If (seg = "MSH") Then
            haveMSH += 1
        ElseIf (seg = "QRD") Then
            haveQRD += 1
        ElseIf (seg = "QRF") Then
            haveQRF += 1
        End If
    End Sub
    Public Overrides Function Valide() As Boolean
        Return True
        'Return ValideMSH() And _
        '    ValideQRD() And _
        '    ValideQRF()
    End Function
    Private Function ValideMSH() As Boolean
        If MSH(0) <> Nothing Then
            Dim count = 0
            For Each encodChar In MSH(0)
                For Each encodChar2 In MSH(0)
                    If (encodChar = encodChar2) Then
                        count += 1
                    End If
                Next
                If count > 1 Then
                    Return False
                End If
                count = 0
            Next
        Else
            Return False
        End If
        If (MSH(0).Length <> 4 Or MSH(7) = Nothing Or MSH(8) = Nothing Or MSH(9) = Nothing Or MSH(10) <> versao) Then
            ' 1- enconding chars = 4 , 8- Message type, 9- message control id ,10 -processing id 11- versao hl7
            Return False
        End If
        Return True
    End Function
    Private Function ValideQRD() As Boolean
        If (haveQRD) Then
            If (QRD(0) = Nothing Or QRD(1) = Nothing Or QRD(2) = Nothing Or QRD(3) = Nothing Or QRD(8) = Nothing) Then
                '0 - Date/Time of Query , 1 -Query format mode, 2-Query Priority, 3-Query ID:Some unique identifier 8-What subject filter
                Return False
            End If
        End If
        Return True
    End Function
    Private Function ValideQRF() As Boolean
        For i = 0 To haveQRF - 1
            If (QRF(i, 0) = Nothing) Then
                '0- Where subject filter - always MON
                Return False
            End If
        Next
        Return True
    End Function
End Class
