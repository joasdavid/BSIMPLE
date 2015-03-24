Imports System.Configuration

Public Class MSSQLControllerMindray
    Private Shared ReadOnly _instance As New Lazy(Of MSSQLControllerMindray)(Function() New MSSQLControllerMindray(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication)

    Dim strConn As String
    Dim idPaciente As String

    Private Sub New()
        strConn = ConfigurationManager.AppSettings("StrgConn").ToString
    End Sub

    'Patient Information Change Message
    Private Sub PIC(msg As Message)
        Dim _idPaciente = msg.getSegmentField("PID", 2)
        Dim bd = New MSSQLConnection(strConn)
        'ver se o paciente ja existe
        Dim tb As DataTable = bd.sendQuery2("select Count(*) from Paciente where IdPaciente like '" & _idPaciente & "'")
        Dim isNew As Integer = tb.Rows(0).Item(0)

        'campos
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
        'caso nao exista cria
        If (isNew = 0) Then
            Dim _date
            If (dataNascim = "" Or dataNascim = Nothing) Then
                _date = "NULL"
            Else
                _date = "CONVERT(DATETIME, '" & dataNascim & "')"
            End If
            bd.execQuery("insert into Paciente VALUES('" & idPaciente & "', '" &
                                                           fName & "', '" &
                                                           lName & "'," &
                                                           _date & ", '" &
                                                           sexo & "','asd', '" &
                                                           tipoSangue & "','" &
                                                           tipoPaciente & "', '" &
                                                           paceSwitch & "')")
        Else 'caso exista faz update
            Dim _date
            If (dataNascim = "" Or dataNascim = Nothing) Then
                _date = "NULL"
            Else
                _date = "CONVERT(DATETIME, '" & dataNascim & "')"
            End If
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
    Private Sub PP(msg As Message)
        Dim bd = New MSSQLConnection(strConn)
        'para cada obx na mensagem
        For i = 0 To msg.getSegmentCont("OBX") - 1 Step 1
            'get data
            Dim _idAndDesc = msg.getSegmentField("OBX", i, 2).Split("^")
            Dim idOBX = _idAndDesc(0)
            Dim subidOBX = msg.getSegmentField("OBX", i, 3) + "" 'add aspas casso seja vazio
            Dim valor = CDbl(msg.getSegmentField("OBX", i, 4).Replace(".", ","))

            'Monitorização
            Dim tb As DataTable
            tb = bd.sendQuery2("select Count(*) from Monitorizacao as m where  m.IdOBX = " & idOBX & " and m.IdPaciente like '" & idPaciente & "'")
            Dim isNew As Integer = tb.Rows(0).Item(0)
            'casso nao exista
            If (isNew = 0) Then
                bd.execQuery("insert into Monitorizacao VALUES(" & idOBX & ",'" & idPaciente & "')")
            End If
            'valores
            Dim di = "CONVERT(DATETIME, '" & msg.getTime().Replace("Z", "") & "')"
            Dim df = di
            tb = bd.sendQuery2("SELECT TOP 1 * FROM Valores as v WHERE v.IdOBX = " & idOBX & "and v.IdPaciente like '" & idPaciente & "' order by v.DataFinal desc")
            Dim ultimoValor As Double = Nothing
            If (tb.Rows.Count <> Nothing) Then
                ultimoValor = CDbl(tb.Rows(0).Item(6))
            End If
            If (valor = ultimoValor) Then
                bd.execQuery("UPDATE [HL7Mindray].[dbo].[Valores] SET DataFinal = " & di & " WHERE IdPaciente like '" & idPaciente & "' and IdOBX =" & idOBX)
            Else 'caso exista alteração
                bd.execQuery("insert into Valores(IdPaciente, IdOBX, Sub_id, Valor, DataInicio, DataFinal) " &
                                          "VALUES('" & idPaciente & "', " & idOBX & ", " & subidOBX & ", " & valor.ToString().Replace(",", ".") & "," & di & "," & df & ")")
            End If
        Next

    End Sub

    'NIBP Parameter Message
    Private Sub NIBP(msg As Message)
        'valida

    End Sub


    Public Sub addMSGtoDB(msg As Message)
        Dim id = CInt(msg.getSegmentField("MSH", 8))
        If id = 103 Then
            PIC(msg)
        ElseIf id = 204 Then
            PP(msg)
        ElseIf id = 503 Then
            'r()
        Else
            'r()
        End If
    End Sub

    Public Shared ReadOnly Property Instance() As MSSQLControllerMindray
        Get
            Return _instance.Value
        End Get
    End Property
End Class
