﻿Imports System.Configuration

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
        'caso nao exista

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
        Console.WriteLine("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", idPaciente, fName, lName, dataNascim, sexo, tipoPaciente, tipoSangue, paceSwitch)
        If (isNew = 0) Then
            Dim _date
            If (dataNascim = "" Or dataNascim = Nothing) Then
                _date = Nothing
            Else
                _date = "CONVERT(DATETIME, " & dataNascim & ")"
            End If
            bd.execQuery("insert into Paciente VALUES('" & idPaciente & "', '" & fName & "', '" & lName & "'," & _date & ", '" & sexo & "','asd', '" & tipoSangue & "','" & tipoPaciente & "', '" & paceSwitch & "')")
        End If

    End Sub

    'Unsolicited Observation Reporting Message
    'Periodic parameters
    Private Sub PP(msg As Message)
        'valida

    End Sub

    'NIBP Parameter Message
    Private Sub NIBP(msg As Message)
        'valida

    End Sub


    Public Sub addMSGtoDB(msg As Message)
        Dim id = CInt(msg.getSegmentField("MSH", 8))
        If id = 103 Then
            PIC(msg)
        ElseIf id = 203 Then
            'r()
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
