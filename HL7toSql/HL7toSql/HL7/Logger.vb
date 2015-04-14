Imports System.IO

Public Class Logger
    Private Shared ReadOnly _instance As New Lazy(Of Logger)(Function() New Logger(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication)


    Dim logDirectory As String
    Dim logFiles As New List(Of String)
    ReadOnly list_lock

    Private Sub New()
        logDirectory = Directory.GetCurrentDirectory()
        logFiles.Add("default.log")
    End Sub

    Public Sub setDirectory(dir As String)
        logDirectory = dir
    End Sub

    Public Sub log(text As String)
        log("default.log", "", text)
    End Sub

    Public Sub log(fileName As String, text As String)
        log(fileName, "", text)
    End Sub

    Public Sub log(fileName As String, tag As String, text As String)

        Dim index = logFiles.IndexOf(fileName)
        If (index = -1) Then
            logFiles.Add(fileName)
            index = logFiles.IndexOf(fileName)
        End If
        Dim lock = logFiles.Item(index)
        SyncLock (lock)
            writeLine(fileName, tag, text)
        End SyncLock
    End Sub

    Private Sub writeLine(filename As String, tag As String, text As String)
        Dim presentDate = DateTime.Now.ToString("dd-MM-yyyy")
        Dim trueFilename As String = presentDate & "_" & filename
        Dim objWriter As New System.IO.StreamWriter(trueFilename, True)
        Dim hour = DateTime.Now.ToString("HH:mm:ss")
        If (tag <> "") Then
            tag = "<" & tag & ">"
        End If
        objWriter.WriteLine("{0} - {1} {2}", hour, tag, text)
        objWriter.Close()
    End Sub

    Public Shared ReadOnly Property Instance() As Logger
        Get
            Return _instance.Value
        End Get
    End Property


End Class
