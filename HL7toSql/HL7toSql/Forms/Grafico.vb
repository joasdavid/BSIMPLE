Imports System.Data.SqlClient
Imports System.Windows.Forms.DataVisualization.Charting

Public Class Grafico
    Private controller As MSSQLControllerMindray
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        controller = New MSSQLControllerMindray
        controller.setidPaciente(tbGraph.Text)
        fillCBox()

        


    End Sub
    Private Sub fillCBox()

        ' table As DataSet = controller.getTableCB("SinaisVitais").Tables(0)
        For Each fill As DataRow In controller.getTableSV("SinaisVitais").Tables(0).Rows
            cbGraph.Items.Add(fill("Descricao"))
        Next

    End Sub

    Private Sub showBDcontentSearch(id As String)
        Dim name2sv As DataSet = controller.getSVidFromName(cbGraph.SelectedItem.ToString)
        Me.DataGridView1.DataSource = controller.getTableGraph(id, DataInicio.Value.Date.ToString("yyyy-MM-dd"), DataFim.Value.Date.ToString("yyyy-MM-dd")).Tables(0)
        Me.DataGridView1.Update()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        controller.setidPaciente(tbGraph.Text)

        Dim name2sv As DataSet = controller.getSVidFromName(cbGraph.SelectedItem.ToString)
        Dim id = name2sv.Tables(0).Rows(0).Item(0)
        showBDcontentSearch(id)
        Dim table As DataSet = controller.getTableGraph(id, DataInicio.Value.Date.ToString("yyyy-MM-dd"), DataFim.Value.Date.ToString("yyyy-MM-dd"))
        Chart1.Series(0).Points.Clear()
        Chart1.Series(1).Points.Clear()
        'Chart1.Series.Dispose()
        Chart1.Series(1).ToolTip = cbGraph.SelectedItem.ToString & " = #VALY" & vbCrLf & "#AXISLABEL"
        For Each value As DataRow In table.Tables(0).Rows
            Dim i = Chart1.Series(0).Points.AddXY(value.Item(1).ToString, value.Item(0))
            i = Chart1.Series(0).Points.AddXY(value.Item(2).ToString, value.Item(0))
            Chart1.Series(1).Points.AddXY(value.Item(1).ToString, value.Item(0))
            Chart1.Series(1).Points.AddXY(value.Item(2).ToString, value.Item(0))

        Next
        Chart1.DataBind()
        Chart1.Update()




    End Sub

End Class