<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HL7toDB
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Export2Sql = New System.Windows.Forms.Button()
        Me.Select_file = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ShowDatehl7 = New System.Windows.Forms.ListView()
        Me.Load2DB = New System.Windows.Forms.ProgressBar()
        Me.DataFromDB = New System.Windows.Forms.ListView()
        Me.SuspendLayout()
        '
        'Export2Sql
        '
        Me.Export2Sql.Location = New System.Drawing.Point(578, 37)
        Me.Export2Sql.Name = "Export2Sql"
        Me.Export2Sql.Size = New System.Drawing.Size(93, 27)
        Me.Export2Sql.TabIndex = 0
        Me.Export2Sql.Text = "Export to Sql"
        Me.Export2Sql.UseVisualStyleBackColor = True
        '
        'Select_file
        '
        Me.Select_file.Location = New System.Drawing.Point(456, 37)
        Me.Select_file.Name = "Select_file"
        Me.Select_file.Size = New System.Drawing.Size(93, 27)
        Me.Select_file.TabIndex = 1
        Me.Select_file.Text = "Select file"
        Me.Select_file.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(21, 41)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(413, 20)
        Me.TextBox1.TabIndex = 2
        '
        'ShowDatehl7
        '
        Me.ShowDatehl7.Location = New System.Drawing.Point(21, 89)
        Me.ShowDatehl7.Margin = New System.Windows.Forms.Padding(0)
        Me.ShowDatehl7.Name = "ShowDatehl7"
        Me.ShowDatehl7.Size = New System.Drawing.Size(413, 377)
        Me.ShowDatehl7.TabIndex = 3
        Me.ShowDatehl7.UseCompatibleStateImageBehavior = False
        '
        'Load2DB
        '
        Me.Load2DB.Location = New System.Drawing.Point(693, 41)
        Me.Load2DB.Name = "Load2DB"
        Me.Load2DB.Size = New System.Drawing.Size(278, 20)
        Me.Load2DB.TabIndex = 4
        '
        'DataFromDB
        '
        Me.DataFromDB.Location = New System.Drawing.Point(456, 89)
        Me.DataFromDB.Margin = New System.Windows.Forms.Padding(0)
        Me.DataFromDB.Name = "DataFromDB"
        Me.DataFromDB.Size = New System.Drawing.Size(515, 377)
        Me.DataFromDB.TabIndex = 5
        Me.DataFromDB.UseCompatibleStateImageBehavior = False
        '
        'HL7toDB
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(980, 493)
        Me.Controls.Add(Me.DataFromDB)
        Me.Controls.Add(Me.Load2DB)
        Me.Controls.Add(Me.ShowDatehl7)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Select_file)
        Me.Controls.Add(Me.Export2Sql)
        Me.Name = "HL7toDB"
        Me.Text = "Hl7 to DB"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Export2Sql As System.Windows.Forms.Button
    Friend WithEvents Select_file As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ShowDatehl7 As System.Windows.Forms.ListView
    Friend WithEvents Load2DB As System.Windows.Forms.ProgressBar
    Friend WithEvents DataFromDB As System.Windows.Forms.ListView

End Class
