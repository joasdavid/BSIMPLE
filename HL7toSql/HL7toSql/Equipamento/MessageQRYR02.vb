Public Class MessageQRYR02
    Inherits MessageHL7

    Private MSH(19) As String
    Private QRD(12) As String
    Private QRF(40, 9) As String

    Private haveMSH As Integer = 0
    Private haveQRD As Integer = 0
    Private haveQRF As Integer = 0

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
    End Function
End Class
