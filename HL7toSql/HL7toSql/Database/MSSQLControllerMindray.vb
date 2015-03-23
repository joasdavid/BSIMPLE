Imports System.Configuration

Public Class MSSQLControllerMindray
    Private Shared ReadOnly _instance As New Lazy(Of MSSQLController)(Function() New MSSQLController(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication)

    Dim strConn As String
    Dim idPaciente As String

    Private Sub New()
        strConn = ConfigurationManager.AppSettings("StrgConn").ToString
    End Sub

    'Patient Information Change Message
    Private Sub PIC(msg As Message)
        'valida

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
            'r()
        ElseIf id = 203 Then
            'r()
        ElseIf id = 503 Then
            'r()
        Else
            'r()
        End If
    End Sub

    Public Shared ReadOnly Property Instance() As MSSQLController
        Get
            Return _instance.Value
        End Get
    End Property
End Class
