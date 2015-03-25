Imports System.Configuration
Imports System.IO
Imports System.Text.RegularExpressions

Public Class Message

    Private MSH(19) As String
    Private OBX(40, 17) As String
    Private OBR(40, 43) As String
    Private PV1(52) As String
    Private PID(30) As String
    Private ZSG(30, 30) As String 'guarda a informação de um qualquer segmentoZ
    Private ZHD(30) As String     'gurada os 3 caracteres que identifiquem o segmentoZ

    Private haveMSH As Integer = 0
    Private havePV1 As Integer = 0
    Private havePID As Integer = 0
    Private haveOBX As Integer = 0
    Private haveOBR As Integer = 0
    Private haveZSG As Integer = 0

    Private c28 As Char = Chr(28)
    Private c13 As Char = Chr(13)
    Private c10 As Char = Chr(10)
    Private c11 As Char = Chr(11)
    Private c33 As Char = Chr(161)

    Private tempoChegada As String


    Private strdata As String = ""

    Sub New()

    End Sub
    Sub New(msg As String)
        parseData(msg)
        tempoChegada = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")
    End Sub

    Public Function parseData(ByVal data As String) As String
        strdata = data

        Dim j, i As Integer
        Dim header, buffer As String
        buffer = ""
        header = ""

        Dim cc, separator As Char
        separator = data.Chars(4)
        separator = "|"
        j = 0
        i = 0

        While (i <= data.Length - 1)
            Try
                cc = data.Chars(i)

                If (cc = separator) Then
                    header = addToHeader(header, buffer, j)
                    buffer = ""
                    j = j + 1
                ElseIf (cc = c11) Then  'nao faz nada
                    j = 0
                    buffer = ""
                ElseIf (cc = c10 OrElse cc = c13) Then  'fim de segmento
                    If (buffer <> "") Then
                        header = addToHeader(header, buffer, j)
                    End If
                    haveOneMore(header)
                    j = 0
                    buffer = ""
                ElseIf (cc = c28) Then  'fim de mensagem
                    If (buffer <> "") Then
                        header = addToHeader(header, buffer, j)
                    End If
                    j = 0
                    Exit While
                Else
                    buffer = buffer + cc
                End If

            Catch ex As Exception
                Console.WriteLine("Erro em parser: " & ex.Message & " header  " & header & "    " & buffer)
                buffer = ""
            End Try

            i = i + 1
        End While

        'restos
        If (buffer <> "") Then
            header = addToHeader(header, buffer, j)
        End If

        ' debug()
        '  Console.WriteLine(returnField("H", 12))

        'devolve tipo de mensagem parsed
        Return MSH(7)
    End Function


    Private Function addToHeader(ByVal header As String, ByVal buffer As String, ByVal pos As Integer) As String

        If (pos = 0) Then
            header = buffer  'header = buffer para posição 0
            Return header
        End If

        If (buffer = "") Then
            Return header
        End If

        'Console.WriteLine(header & " data added " & buffer)

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
        ElseIf (header.Chars(0) = "Z") Then
            Dim counter1, counter2 As Integer
            Try
                counter1 = haveZSG
                counter2 = pos - 1
                ZSG(counter1, counter2) = buffer
                ZHD(counter1) = header
            Catch ex As Exception
                counter1 = 0
                counter2 = 0
            End Try
        End If

        Return header
    End Function

    Private Sub haveOneMore(seg As String)
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
        ElseIf (seg = "ZSG") Then
            haveZSG += 1
        End If
    End Sub

    Private Sub debug(ByVal list As String(), ByVal nome As String)
        Dim i As Integer
        Try

            For i = 0 To list.Length - 1 Step 1
                If (Not list(i) Is Nothing) Then
                    Console.WriteLine(nome & " " & (i + 1) & "     " & list(i))
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub


    Private Sub debug(ByVal list As String(,), ByVal nome As String)
        Dim i, j As Integer
        Try
            For i = 0 To list.GetUpperBound(0) - 1 Step 1
                For j = 0 To list.GetUpperBound(1) - 1 Step 1
                    If (Not list(i, j) Is Nothing) Then
                        If (nome = "ZXX") Then
                            Console.WriteLine("{0}({1},{2}) {3}", ZHD(i), (i + 1), (j + 1), list(i, j))
                        Else
                            Console.WriteLine("{0}({1},{2}) {3}", nome, (i + 1), (j + 1), list(i, j))
                        End If
                    End If
                Next
            Next
        Catch ex As Exception
        End Try
    End Sub


    Public Function toString() As String
        Return strdata
    End Function
    Public Function getTime() As String
        Return tempoChegada
    End Function

    Public Function getSegmentField(seg As String, pos As Integer) As String
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
    Public Function getSegmentField(seg As String, segN As Integer, pos As Integer) As String
        If (seg = "OBX") Then
            Dim strReturn = "" & OBX(segN, pos)
            Return strReturn
        ElseIf seg = "OBR" Then
            Dim strReturn = "" & OBR(segN, pos)
            Return strReturn
        End If
        Return Nothing
    End Function
    Public Function getSegmentCont(seg As String) As Integer
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
        ElseIf (seg = "ZSG" And haveZSG > 0) Then
            Return haveZSG
        End If
        Return 0
    End Function
    Public Sub debug()
        debug(MSH, "MSH")
        debug(PID, "PID")
        debug(OBR, "OBR")
        debug(OBX, "OBX")
        debug(PV1, "PV1")
        debug(ZSG, "ZXX")
    End Sub

    Public Function Valide() As Boolean

        If (MSH(0) = Nothing Or MSH(7) = Nothing Or MSH(8) = Nothing Or MSH(9) = Nothing Or MSH(10) <> "2.3.1") Then
            ' 1- enconding chars , 8- Message type, 9- message control id ,10 -processing id 11- versao hl7
            Return False
        End If
        Dim regex = New Regex("[0-9][0-9]*\^.*")

        For i = 0 To haveOBX - 1

            Dim match = regex.Match(OBX(i, 2), "[0-9][0-9]*\^.*")
            If (match.Value <> OBX(i, 2)) Then
                Return False
            End If
        Next
        Dim msgValidate As Integer = CInt(MSH(8))
        If msgValidate = 103 Then
            If (PID(2) = Nothing Or PID(4) = Nothing) Then
                '2 - patient id , 4 -name
                Return False
            End If
            If (PV1(1) = Nothing) Then
                '1-Patient Class
                Return False
            End If
            For i = 0 To haveOBR
                If (OBR(i, 3) = Nothing) Then
                    '3- universal service id(Monitor MindRay)
                    Return False
                End If
            Next
            For i = 0 To haveOBX
                If (OBX(i, 1) = Nothing Or OBX(i, 2) = Nothing Or OBX(i, 4) = Nothing Or OBX(i, 10) = Nothing) Then
                    '1-value type ,2-Observation Identifier, 4-Observation Results ,10-Observation Results
                    Return False
                End If
            Next
        ElseIf msgValidate = 204 Or msgValidate = 503 Then
            For i = 0 To haveOBX
                If (OBX(i, 1) = Nothing Or OBX(i, 2) = Nothing Or OBX(i, 4) = Nothing Or OBX(i, 10) = Nothing) Then
                    '1-value type ,2-Observation Identifier, 4-Observation Results ,10-Observation Results
                    Return False
                End If
            Next

        End If
        Return True

    End Function

End Class

