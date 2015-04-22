Imports System.Configuration
Imports System.IO
Imports System.Text.RegularExpressions
Public Class MessageORU
    Inherits MessageHL7

    Private MSH(19) As String
    Private OBX(40, 17) As String
    Private OBR(40, 43) As String
    Private PV1(52) As String
    Private PID(30) As String

    Private haveMSH As Integer = 0
    Private havePV1 As Integer = 0
    Private havePID As Integer = 0
    Private haveOBX As Integer = 0
    Private haveOBR As Integer = 0

    Public Sub New()
    End Sub

    Public Sub New(data As String)
        parseData(data)
    End Sub
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
        ElseIf (header = "PID") Then
            PID(pos - 1) = buffer
        ElseIf (header = "PV1") Then
            PV1(pos - 1) = buffer
        ElseIf (header = "OBX") Then
            Dim counter1, counter2 As Integer
            Try
                counter1 = haveOBX
                counter2 = pos - 1
                OBX(counter1, counter2) = buffer
            Catch ex As Exception
                counter1 = 0
                counter2 = 0
            End Try


        ElseIf (header = "OBR") Then
            Dim counter1, counter2 As Integer
            Try
                counter1 = haveOBR
                counter2 = pos - 1
                OBR(counter1, counter2) = buffer
            Catch ex As Exception
                counter1 = 0
                counter2 = 0
            End Try
        End If

        Return header
    End Function

    Public Overrides Function getSegmentCont(seg As String) As Integer
        If (seg = "MSH" & haveMSH > 0) Then
            Return haveMSH
        ElseIf (seg = "OBX" And haveMSH > 0) Then
            Return haveOBX
        ElseIf (seg = "OBR" And haveOBR > 0) Then
            Return haveOBR
        ElseIf (seg = "PV1" And havePV1 > 0) Then
            Return havePV1
        ElseIf (seg = "PID" And havePID > 0) Then
            Return havePID
        End If
        Return 0
    End Function

    Public Overloads Overrides Function getSegmentField(seg As String, pos As Integer) As String
        If (seg = "PID") Then
            Dim strReturn = "" & PID(pos)
            Return strReturn
        ElseIf seg = "MSH" Then
            Dim strReturn = "" & MSH(pos)
            Return strReturn
        ElseIf seg = "PV1" Then
            Dim strReturn = "" & PV1(pos)
            Return strReturn
        End If
        Return Nothing
    End Function

    Public Overloads Overrides Function getSegmentField(seg As String, segN As Integer, pos As Integer) As String
        If (seg = "OBX") Then
            Dim strReturn = "" & OBX(segN, pos)
            Return strReturn
        ElseIf seg = "OBR" Then
            Dim strReturn = "" & OBR(segN, pos)
            Return strReturn
        End If
        Return Nothing
    End Function

    Protected Overrides Sub haveOneMore(seg As String)
        If (seg = "MSH") Then
            haveMSH += 1
        ElseIf (seg = "OBX") Then
            haveOBX += 1
        ElseIf (seg = "OBR") Then
            haveOBR += 1
        ElseIf (seg = "PV1") Then
            havePV1 += 1
        ElseIf (seg = "PID") Then
            havePID += 1
        End If
    End Sub

    Public Overrides Function Valide() As Boolean
        Return ValideMSH() And _
        ValideOBX() And _
        ValidePID() And _
        ValidePV1() And _
        ValideOBR()
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

    Private Function ValidePID() As Boolean
        If (havePID) Then
            If (PID(2) = Nothing Or PID(4) = Nothing Or ValidePID_Nome() = False) Then
                '2 - patient id , 4 -name(<firstName>^<LastName>)
                Return False
            End If
        End If
        Return True
    End Function

    Private Function ValidePV1() As Boolean
        If (havePV1) Then
            If (PV1(1) = Nothing) Then
                '1-Patient Class
                Return False
            End If
        End If
        Return True
    End Function

    Private Function ValideOBR() As Boolean
        For i = 0 To haveOBR - 1
            If (OBR(i, 3) = Nothing) Then
                '3- universal service id(Monitor MindRay)
                Return False
            End If
        Next
        Return True
    End Function

    Private Function ValideOBX() As Boolean
        For i = 0 To haveOBX - 1
            If (OBX(i, 1) = Nothing Or _
                OBX(i, 2) = Nothing Or _
                ValideOBX_Identifier(i) = False Or _
                OBX(i, 4) = Nothing Or _
                OBX(i, 10) <> "F") Then
                '1-value type ,2-Observation Identifier(<id>^descrição), 4-Observation Results ,10-Observation Results status
                Return False
            End If
        Next
        Return True
    End Function

    Private Function ValideOBX_Identifier(pos As Integer) As Boolean
        If MSH(8) = "54" Then
            Dim match = Regex.Match(OBX(pos, 2), "[0-4]")
            If (match.Value <> OBX(pos, 2)) Then
                Return False
            End If
        Else
            Dim match = Regex.Match(OBX(pos, 2), "[0-9][0-9]*\^.*")
            If (match.Value <> OBX(pos, 2)) Then
                Return False
            End If
        End If
        Return True
    End Function

    Private Function ValidePID_Nome() As Boolean
        Dim match = Regex.Match(PID(4), "[aA-zZ]+[aA-zZ]*\.{0,1}[aA-zZ]*\^[aA-zZ]+[aA-zZ]*\.{0,1}[aA-zZ]*")
        If (match.Value <> PID(4)) Then
            Return False
        End If
        Return True
    End Function
End Class
