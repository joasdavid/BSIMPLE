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

    Private Sub showBDcontentSearch()
        Dim name2sv As DataSet = controller.getSVidFromName(cbGraph.SelectedItem.ToString)
        Me.DataGridView1.DataSource = New MSSQLControllerMindray().getTableDGShearch("Monitorizacao", name2sv.Tables(0).Rows(0).Item(0)).Tables(0)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        controller.setidPaciente(tbGraph.Text)
        showBDcontentSearch()
        tbGraph.Text = ""
        Dim name2sv As DataSet = controller.getSVidFromName(cbGraph.SelectedItem.ToString)
        Dim id = name2sv.Tables(0).Rows(0).Item(0)
        Dim table As DataSet = controller.getTableGraph(id)
        Chart1.Series(0).Points.Clear()
        'Chart1.Series.Dispose()
        For Each value As DataRow In table.Tables(0).Rows
            Dim i = Chart1.Series(0).Points.AddXY(value.Item(1).ToString, value.Item(0))
            i = Chart1.Series(0).Points.AddXY(value.Item(2).ToString, value.Item(0))
        Next
        Chart1.DataBind()
        Chart1.Update()




    End Sub

End Class