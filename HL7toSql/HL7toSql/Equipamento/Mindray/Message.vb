Public Class Message

    Private idMessage As Integer
    Private MSH(19) As String
    Private OBX(40, 17) As String
    Private OBR(40, 43) As String
    Private PV1(52) As String
    Private PID(30) As String

    Private c28 As Char = Chr(28)
    Private c13 As Char = Chr(13)
    Private c10 As Char = Chr(10)
    Private c11 As Char = Chr(11)
    Private c33 As Char = Chr(161)

    Private sizeOBX As Integer = 0
    Private sizeOBR As Integer = 0

    Private strdata As String = ""
   
    Sub New()

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
                        If (header = "OBX") Then
                            sizeOBX += 1
                        ElseIf (header = "OBR") Then
                            sizeOBR += 1
                        End If
                    End If
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
                counter1 = sizeOBX
                counter2 = pos - 1
                OBX(counter1, counter2) = buffer
            Catch ex As Exception
                counter1 = 0
                counter2 = 0
            End Try


        ElseIf (header = "OBR") Then
            Dim counter1, counter2 As Integer
            Try
                counter1 = sizeOBR
                counter2 = pos - 1
                OBR(counter1, counter2) = buffer
            Catch ex As Exception
                counter1 = 0
                counter2 = 0
            End Try
        End If

        Return header
    End Function

    Private Sub debug(ByVal list As String(), ByVal nome As String)
        Dim i As Integer
        Try

            For i = 0 To list.Length - 1 Step 1
                If (Not list(i) Is Nothing) Then
                    Console.WriteLine(nome & " " & i & "     " & list(i))
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
                        Console.WriteLine("{0}({1},{2}) {3}", nome, i, j, list(i, j))
                    End If
                Next
            Next
        Catch ex As Exception
        End Try
    End Sub




    Public Sub debug()
        debug(MSH, "MSH")
        debug(PID, "PID")
        debug(OBR, "OBR")
        debug(OBX, "OBX")
        debug(PV1, "PV1")
    End Sub



End Class
