Imports System.Configuration

Public Class MSSQLController

    Dim strConn As String
    Dim idPaciente As String

    Private firstOfToday As List(Of Integer)


#Region "Construtor"
    Public Sub New()
        strConn = System.Configuration.ConfigurationManager.AppSettings("strgConn").ToString
        Dim bd = New MSSQLConnection(strConn)
        firstOfToday = New List(Of Integer)
    End Sub
#End Region
#Region "Public"
    Public Sub addMSGtoDB(msg As MessageORU)
        Dim id = CInt(msg.getSegmentField("MSH", 8))
        If id = 103 Then
            PIC(msg)
        ElseIf id = 204 Then
            Parameters(msg)
        ElseIf id = 503 Then
            NIBP(msg)
        ElseIf id = 54 Then
            Alarm(msg)
        Else
            'r()
        End If
    End Sub

    Public Function getTable(name As String) As DataSet
        Dim dt As New DataSet
        Dim bd = New MSSQLConnection(strConn)
        Dim tb = bd.sendQuery("select * from " & name)
        Dim r = tb.Rows.Count
        dt.Tables.Add(tb)
        Return dt
    End Function

    Public Function getTableGraph(id As String, DataIn As String, DataFim As String) As DataSet
        Dim dt As New DataSet
        Dim bd = New MSSQLConnection(strConn)
        Dim tb = bd.sendQuery("select  Valor,DataInicio, DataFinal from Monitorizacao as g where g.IdPaciente like'" & idPaciente & "'and g.IdSV = " & id & " and g.DataInicio >= '" & DataIn & " 00:00:01' and g.DataFinal <= '" & DataFim & " 23:59:00'")
       Dim r = tb.Rows.Count
        dt.Tables.Add(tb)
        Return dt
    End Function

    Public Function getTableNumAlarm(DataIn As String, DataFim As String) As DataSet
        Return getTableNumAlarm(0, DataIn, DataFim)
    End Function

    Public Function getTableNumAlarm(idAlarm As Integer, DataIn As String, DataFim As String) As DataSet
        Dim dt As New DataSet
        Dim bd = New MSSQLConnection(strConn)
        Dim tb
        If idAlarm = 0 Then
            tb = bd.sendQuery("SELECT COUNT(DataInicio) as Num, NivelAlarme, DataInicio as dataAlarme FROM Monitorizacao where IdAlarme is not NULL and idpaciente like '" & idPaciente & "' and DataInicio >= '" & DataIn & " 00:00:01' and DataFinal <= '" & DataFim & " 23:59:00' group by NivelAlarme, DataInicio")
        Else
            tb = bd.sendQuery("SELECT COUNT(DataInicio) as Num, NivelAlarme, DataInicio as dataAlarme FROM Monitorizacao where IdAlarme is not NULL and idpaciente like '" & idPaciente & "' and DataInicio >= '" & DataIn & " 00:00:01' and DataFinal <= '" & DataFim & " 23:59:00' and idAlarme = '" & idAlarm & "' group by NivelAlarme, DataInicio")
        End If
        Dim r = tb.Rows.Count
        dt.Tables.Add(tb)
        Return dt
    End Function

    Public Function getSVidFromName(name As String) As DataSet
        Dim dt As New DataSet
        Dim bd = New MSSQLConnection(strConn)
        Dim tb = bd.sendQuery("select  IdSV from SinaisVitais where Descricao like '" & name & "'")
        Dim r = tb.Rows.Count
        dt.Tables.Add(tb)
        Return dt
    End Function

    Public Sub setidPaciente(stg As String)
        idPaciente = stg
    End Sub

    Public Function getAlarmeidFromName(name As String) As DataSet
        Dim dt As New DataSet
        Dim bd = New MSSQLConnection(strConn)
        Dim tb = bd.sendQuery("select  IdAlarme from Alarme where Descricao like '" & name & "'")
        Dim r = tb.Rows.Count
        dt.Tables.Add(tb)
        Return dt
    End Function

#End Region
#Region "Gravar para Base de Dados"
    'Patient Information Change Message
    Private Sub PIC(msg As MessageORU)
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
            Dim sql As String = "insert into Paciente VALUES('" & idPaciente & "', '" &
                                                           fName & "', '" &
                                                           lName & "'," &
                                                           _date & ", '" &
                                                           sexo & "','asd', '" &
                                                           tipoSangue & "','" &
                                                           tipoPaciente & "', '" &
                                                           paceSwitch & "')"
            bd.execQuery(sql)

        Else 'caso exista faz update
            Dim sql As String = "UPDATE Paciente SET Frist_Name_Paciente = '" & fName & "'" &
                                             ",Last_Name_Paciente = '" & lName & "'" &
                                                       ", DataNas = " & _date &
                                                          ", Sexo = '" & sexo & "'" &
                                                         ",Morada = '" & "0asd" & "'" &
                                                         ",Sangue = '" & tipoSangue & "'" &
                                                   ",TipoPaciente = '" & tipoPaciente & "'" &
                                                    ",Pace_Switch = '" & paceSwitch & "'" &
                                                    " WHERE IdPaciente like '" & idPaciente & "'"
            bd.execQuery(sql)
        End If

    End Sub

    'Unsolicited Observation Reporting Message
    'Periodic parameters
    Private Sub Parameters(msg As MessageORU)
        'para cada obx na mensagem
        For i = 0 To msg.getSegmentCont("OBX") - 1 Step 1
            'get data
            Dim _idAndDesc = msg.getSegmentField("OBX", i, 2).Split("^") '<id>^<descrição>
            Dim idOBX = _idAndDesc(0) '<id>
            Dim valor = msg.getSegmentField("OBX", i, 4)

            Dim di = "CONVERT(DATETIME, '" & msg.getTime().Replace("Z", "") & "')"
            Dim df = di

            'guarda os valores da Monitorização
            saveMonitorizacao(idOBX, valor, di, df)

        Next

    End Sub

    'NIBP Parameter Message
    Private Sub NIBP(msg As MessageORU)
        'como o formato do segmento OBX e identico
        Parameters(msg)
    End Sub

    'Physiological Alarm Message
    Private Sub Alarm(msg As MessageORU)
        'para cada obx na mensagem
        For i = 0 To msg.getSegmentCont("OBX") - 1 Step 1
            'get data
            Dim nivel = msg.getSegmentField("OBX", i, 2) 'nivel
            Dim idSV_desc = msg.getSegmentField("OBX", i, 4).Split("^") '<id>^<desc>
            Dim idSV = idSV_desc(0)

            Dim di = "CONVERT(DATETIME, '" & msg.getTime().Replace("Z", "") & "')"
            Dim df = di

            'guarda os valores da Monitorização
            saveAlarm(idSV, nivel, di, df)

        Next
    End Sub

    'gurada a monitorizacao
    Private Sub saveMonitorizacao(id As Integer, valor As String, di As String, df As String)
        Dim bd = New MSSQLConnection(strConn)
        Dim _valor = CDbl(valor.Replace(".", ",")).ToString()
        'ultimo valor para uma monitorização
        Dim valor_idvalor = needUpdateValor(id)
        If (_valor = valor_idvalor(0) And firstOfToday.Contains(id)) Then 'se o valor é identico entao só faz update à data
            bd.execQuery("UPDATE Monitorizacao SET DataFinal = " & di & " WHERE Id = " & valor_idvalor(1))
        Else 'caso exista alteração ao valor
            bd.execQuery("insert into Monitorizacao(IdPaciente, IdSV, Valor, DataInicio, DataFinal) " & _
                                      "VALUES('" & idPaciente & "', " & id & ", " & valor & "," & di & "," & df & ")")
            If (Not (firstOfToday.Contains(id))) Then
                firstOfToday.Add(id)
            End If
        End If
    End Sub

    'gurada a alarme
    Private Sub saveAlarm(id As String, nivel As String, di As String, df As String)
        Dim bd = New MSSQLConnection(strConn)

        bd.execQuery("insert into Monitorizacao(IdPaciente, IdAlarme, nivelAlarme, DataInicio, DataFinal) " & _
                                      "VALUES('" & idPaciente & "', " & id & ", " & nivel & "," & di & "," & df & ")")
    End Sub

    Private Function needUpdateValor(id As String) As Object()
        Dim bd = New MSSQLConnection(strConn)
        Dim tb As DataTable
        Dim toReturn(1) As String
        'Caso já existam valores obtém-se o ultimo valor inserido _
        'para uma dada monitorização de um dado paciente
        tb = bd.sendQuery("SELECT * FROM Monitorizacao as v WHERE v.IdSV = " & id & "and v.IdPaciente like '" & idPaciente & "' order by v.Id desc")
        If (tb.Rows.Count <> Nothing) Then 'se ouver algum valor na tabela
            toReturn(1) = tb.Rows(0).Item(0).ToString()
            toReturn(0) = CDbl(tb.Rows(0).Item(2))
        End If
        tb.Dispose()
        Return toReturn
    End Function



#End Region
End Class
