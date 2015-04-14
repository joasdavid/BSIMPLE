Public MustInherit Class MessageHL7

    Protected tempoChegada As String
    Protected strdata As String = ""
    Protected ReadOnly versao As String = "2.3.1"

    Private fs As Char = Chr(28)
    Private cr As Char = Chr(13)
    Private nl As Char = Chr(10)
    Private vt As Char = Chr(11)


    Sub New()
        Me.New("")
    End Sub

    Sub New(msg As String)
        parseData(msg)
        tempoChegada = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")
    End Sub

    Public Function parseData(ByVal data As String) As String
        If (data = "") Then
            Return ""
        End If
        'strdata += data

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
                    strdata += buffer & "|"
                    buffer = ""
                    j = j + 1
                ElseIf (cc = vt) Then  'nao faz nada
                    j = 0
                    buffer = ""
                ElseIf (cc = nl OrElse cc = cr) Then  'fim de segmento
                    If (buffer <> "") Then
                        header = addToHeader(header, buffer, j)
                        strdata += buffer & nl
                    End If
                    haveOneMore(header)
                    j = 0
                    buffer = ""
                ElseIf (cc = fs) Then  'fim de mensagem
                    If (buffer <> "") Then
                        header = addToHeader(header, buffer, j)
                        strdata += buffer & nl
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
            strdata += buffer & nl
        End If

        ' debug()
        '  Console.WriteLine(returnField("H", 12))

        'devolve tipo de mensagem parsed
        Return getSegmentField("MSH", 7)
    End Function

    Public Function getTime() As String
        Return tempoChegada
    End Function

    Public Overloads Function toString() As String
        Return strdata
    End Function

    Protected MustOverride Function addToHeader(ByVal header As String, ByVal buffer As String, ByVal pos As Integer) As String

    Protected MustOverride Sub haveOneMore(seg As String)

    Public MustOverride Function getSegmentField(seg As String, pos As Integer) As String

    Public MustOverride Function getSegmentField(seg As String, segN As Integer, pos As Integer) As String

    Public MustOverride Function getSegmentCont(seg As String) As Integer

    Public MustOverride Function Valide() As Boolean

End Class
