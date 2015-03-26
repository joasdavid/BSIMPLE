Imports System.Configuration

Public Class MSSQLControllerMindray
    Private Shared ReadOnly _instance As New Lazy(Of MSSQLControllerMindray)(Function() New MSSQLControllerMindray(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication)

    Dim strConn As String
    Dim idPaciente As String

#Region "Construtor"
    Private Sub New()
        strConn = ConfigurationManager.AppSettings("StrgConn").ToString
        Dim bd = New MSSQLConnection(strConn)
    End Sub
#End Region
#Region "Gravar para Base de Dados"
    'Patient Information Change Message
    Private Sub PIC(msg As Message)
        Dim bd = New MSSQLConnection(strConn)
        'campos
        Dim _idPaciente = msg.getSegmentField("PID", 2)
        idPaciente = _idPaciente
        Dim fullName = msg.getSegmentField("PID", 4).Split("^")
        Dim fName = fullName(0)
        Dim lName = fullName(1)
        Dim dataNascim = msg.getSegmentField("PID", 6)
        Dim sexo = msg.getSegmentField("PID", 7)
        Dim tipoPaciente = msg.getSegmentField("PV1", 17)
        Dim _fullOBX
        Dim _valor
        Dim idOBX
        Dim tipoSangue As Integer = Nothing
        Dim paceSwitch As Integer = Nothing
        Dim _date
        For i = 0 To msg.getSegmentCont("OBX") - 1 Step 1
            _fullOBX = msg.getSegmentField("OBX", i, 2).Split("^")
            idOBX = CInt(_fullOBX(0))
            _valor = msg.getSegmentField("OBX", i, 4).Split("^")
            If (idOBX = 2302) Then
                tipoSangue = CInt(_valor(0))
            ElseIf (idOBX = 2303) Then
                paceSwitch = CInt(_valor(0))
            End If
        Next

        If (dataNascim = "" Or dataNascim = Nothing) Then 'valida se temos a data de nascimento
            _date = "NULL"
        Else ' se ouver data, foramata esta para ser convertida para DATE pela BD
            _date = "CONVERT(DATETIME, '" & dataNascim & "')"
        End If



        'ver se o paciente ja existe
        Dim tb As DataTable = bd.sendQuery("select Count(*) from Paciente where IdPaciente like '" & _idPaciente & "'")
        Dim isNew As Integer = tb.Rows(0).Item(0) 'primeira linha, primeira coluna ... valor do Count(*)

        If (isNew = 0) Then 'caso não exista
            bd.execQuery("insert into Paciente VALUES('" & idPaciente & "', '" &
                                                           fName & "', '" &
                                                           lName & "'," &
                                                           _date & ", '" &
                                                           sexo & "','asd', '" &
                                                           tipoSangue & "','" &
                                                           tipoPaciente & "', '" &
                                                           paceSwitch & "')")
        Else 'caso exista faz update
            bd.execQuery("UPDATE Paciente SET Frist_Name_Paciente = '" & fName & "'" &
                                             ",Last_Name_Paciente = '" & lName & "'" &
                                                       ", DataNas = " & _date &
                                                          ", Sexo = '" & sexo & "'" &
                                                         ",Morada = '" & "0asd" & "'" &
                                                         ",Sangue = '" & tipoSangue & "'" &
                                                   ",TipoPaciente = '" & tipoPaciente & "'" &
                                                    ",Pace_Switch = '" & paceSwitch & "'" &
                                                    " WHERE IdPaciente like '" & idPaciente & "'")
        End If

    End Sub

    'Unsolicited Observation Reporting Message
    'Periodic parameters
    Private Sub Parameters(msg As Message)
        'para cada obx na mensagem
        For i = 0 To msg.getSegmentCont("OBX") - 1 Step 1
            'get data
            Dim _idAndDesc = msg.getSegmentField("OBX", i, 2).Split("^") '<id>^<descrição>
            Dim idOBX = _idAndDesc(0) '<id>
            Dim subidOBX = msg.getSegmentField("OBX", i, 3) + "" 'add aspas para eviar ser "Nothing"
            Dim valor = msg.getSegmentField("OBX", i, 4)

            Dim di = "CONVERT(DATETIME, '" & msg.getTime().Replace("Z", "") & "')"
            Dim df = di

            'guarda informação da Monitorização
            saveMonitorizacao(idOBX)


            'guarda os valores da Monitorização
            saveValor(idOBX, subidOBX, valor, di, df)
        Next

    End Sub

    'NIBP Parameter Message
    Private Sub NIBP(msg As Message)
        'como o formato do segmento OBX e identico
        Parameters(msg)
    End Sub

    'gurada a monitorizacao caso esta ainda não exista
    Private Sub saveMonitorizacao(id As Integer)
        Dim bd = New MSSQLConnection(strConn)
        Dim tb As DataTable

        'confirma se ja existe
        tb = bd.sendQuery("select Count(*) from Monitorizacao as m where  m.IdOBX = " & id & " and m.IdPaciente like '" & idPaciente & "'")
        Dim isNew As Integer = tb.Rows(0).Item(0) 'primeira linha, primeira coluna ... valor do Count(*)
        tb.Dispose()

        'casso nao exista
        If (isNew = 0) Then 'se for 0 e porque ainda nao existe
            bd.execQuery("insert into Monitorizacao VALUES(" & id & ",'" & idPaciente & "')")
        End If

    End Sub

    Private Sub saveValor(id As Integer, subid As Integer, valor As String, di As String, df As String)
        Dim bd = New MSSQLConnection(strConn)
        Dim _valor = CDbl(valor.Replace(".", ",")).ToString()
        'ultimo valor para uma monitorização
        Dim valor_idvalor = needUpdateValor(id)
        If (_valor = valor_idvalor(0)) Then 'se o valor é identico entao só faz update à data
            bd.execQuery("UPDATE [HL7Mindray].[dbo].[Valores] SET DataFinal = " & di & " WHERE IdValores = " & valor_idvalor(1))
        Else 'caso exista alteração ao valor
            bd.execQuery("insert into Valores(IdPaciente, IdOBX, Sub_id, Valor, DataInicio, DataFinal) " &
                                      "VALUES('" & idPaciente & "', " & id & ", " & subid & ", " & valor & "," & di & "," & df & ")")
        End If

    End Sub

    Private Function needUpdateValor(id As String) As Object()
        Dim bd = New MSSQLConnection(strConn)
        Dim tb As DataTable
        Dim toReturn(1) As String
        'Caso já existam valores obtém-se o ultimo valor inserido _
        'para uma dada monitorização de um dado paciente
        tb = bd.sendQuery("SELECT * FROM Valores as v WHERE v.IdOBX = " & id & "and v.IdPaciente like '" & idPaciente & "' order by v.IdValores desc")
        If (tb.Rows.Count <> Nothing) Then 'se ouver algum valor na tabela
            toReturn(1) = tb.Rows(0).Item(0).ToString()
            toReturn(0) = CDbl(tb.Rows(0).Item(6))
        End If
        tb.Dispose()
        Return toReturn
    End Function

#End Region
#Region "Public"
    Public Sub addMSGtoDB(msg As Message)
        Dim id = CInt(msg.getSegmentField("MSH", 8))
        If id = 103 Then
            PIC(msg)
        ElseIf id = 204 Then
            Parameters(msg)
        ElseIf id = 503 Then
            NIBP(msg)
        Else
            'r()
        End If
    End Sub

    Public Shared ReadOnly Property Instance() As MSSQLControllerMindray
        Get
            Return _instance.Value
        End Get
    End Property
#End Region
End Class
