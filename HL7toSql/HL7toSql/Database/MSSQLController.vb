Imports System.Configuration

Public NotInheritable Class MSSQLController
    Private Shared ReadOnly _instance As New Lazy(Of MSSQLController)(Function() New MSSQLController(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication)

    Dim strConn As String
    Dim idPaciente As String
    Sub New()
        strConn = ConfigurationManager.AppSettings("StrgConn").ToString
    End Sub

    Public Sub addMSGtoDB(m As Message)
        Dim bd = New MSSQLConnection(strConn)
        Dim sqlQuery As String = ""
        'paciente
        If m.getSegmentCont("PID") = 1 Then
            idPaciente = m.getSegmentField("PID", 2)
        End If
        Dim name() As String = m.getSegmentField("PID", 4).Split("^")
        Dim isPacienteOnDB = bd.sendQuery("select Count(*) from Paciente where IdPaciente like '" & idPaciente & "'")
        If isPacienteOnDB(1, 0) = "0" Then
            sqlQuery = "INSERT INTO Paciente  VALUES('" & idPaciente & "','" & name(1) & "','" & name(0) & "')"
            bd.execQuery(sqlQuery)
        End If
        'Monitorização
        For i = 0 To m.getSegmentCont("OBX") - 1 Step 1
            Dim obxComp2 = m.getSegmentField("OBX", i, 2)
            Dim idAndDesc = obxComp2.Split("^")
            If CInt(idAndDesc(0)) > 4 And CInt(idAndDesc(0)) < 1024 Then
                Dim isIDOBXOnDB = bd.sendQuery("select Count(*) from Monitorizacao where IdOBX=" & idAndDesc(0) & " and IdPaciente like '" & idPaciente & "'")
                If isIDOBXOnDB(1, 0) = "0" Then
                    Dim sqlQuery2 = "INSERT INTO Monitorizacao VALUES(" & idAndDesc(0) & ",'" & idPaciente & "','" & idAndDesc(1) & "')"
                    bd.execQuery(sqlQuery2)
                End If
                'valores
                Dim subid = m.getSegmentField("OBX", i, 3)
                If subid = "" Then
                    subid = "0"
                End If
                Dim parametro = m.getSegmentField("OBX", i, 12)
                Dim timeStamp = m.getSegmentField("OBX", i, 13)
                Dim sqlQuery3 = "INSERT INTO Valores VALUES('" & idPaciente & "'," & idAndDesc(0) & "," & subid & ",'" & parametro & "','" & timeStamp & "')"
                bd.execQuery(sqlQuery3)
                'multi-valores aka componentes
                Dim obxComp4 = m.getSegmentField("OBX", i, 4)
                Dim valores() = obxComp4.Split("^")
                Dim idValor = bd.sendQuery("select Max(IdValores) from Valores")
                For Each valor In valores
                    Dim sqlQuery4 = "INSERT INTO Componentes VALUES(" & idValor(1, 0) & ",'" & idPaciente & "'," & idAndDesc(0) & ",'" & valor & "')"
                    bd.execQuery(sqlQuery4)
                Next
            End If
        Next
    End Sub

    Public Shared ReadOnly Property Instance() As MSSQLController
        Get
            Return _instance.Value
        End Get
    End Property

End Class
