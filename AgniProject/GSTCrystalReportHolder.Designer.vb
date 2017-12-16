<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GSTCrystalReportHolder
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
        Me.reportGSTReportViewer = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.SuspendLayout()
        '
        'reportGSTReportViewer
        '
        Me.reportGSTReportViewer.ActiveViewIndex = -1
        Me.reportGSTReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.reportGSTReportViewer.Cursor = System.Windows.Forms.Cursors.Default
        Me.reportGSTReportViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.reportGSTReportViewer.Location = New System.Drawing.Point(0, 0)
        Me.reportGSTReportViewer.Name = "reportGSTReportViewer"
        Me.reportGSTReportViewer.Size = New System.Drawing.Size(1251, 580)
        Me.reportGSTReportViewer.TabIndex = 0
        '
        'GSTReportHolder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1251, 580)
        Me.Controls.Add(Me.reportGSTReportViewer)
        Me.Name = "GSTReportHolder"
        Me.Text = "GSTReportHolder"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents reportGSTReportViewer As CrystalDecisions.Windows.Forms.CrystalReportViewer
End Class
