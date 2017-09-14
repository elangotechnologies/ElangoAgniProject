<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OutstandingCrystalReportHolder
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
        Me.reportOutstandingBalanceReportViewer = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.SuspendLayout()
        '
        'reportOutstandingBalanceReportViewer
        '
        Me.reportOutstandingBalanceReportViewer.ActiveViewIndex = -1
        Me.reportOutstandingBalanceReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.reportOutstandingBalanceReportViewer.Cursor = System.Windows.Forms.Cursors.Default
        Me.reportOutstandingBalanceReportViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.reportOutstandingBalanceReportViewer.EnableDrillDown = False
        Me.reportOutstandingBalanceReportViewer.Location = New System.Drawing.Point(0, 0)
        Me.reportOutstandingBalanceReportViewer.Name = "reportOutstandingBalanceReportViewer"
        Me.reportOutstandingBalanceReportViewer.SelectionFormula = ""
        Me.reportOutstandingBalanceReportViewer.Size = New System.Drawing.Size(1279, 615)
        Me.reportOutstandingBalanceReportViewer.TabIndex = 1
        Me.reportOutstandingBalanceReportViewer.ViewTimeSelectionFormula = ""
        '
        'OutstandingBalanceReportForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1279, 615)
        Me.Controls.Add(Me.reportOutstandingBalanceReportViewer)
        Me.Name = "OutstandingBalanceReportForm"
        Me.Text = "Form1"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents reportOutstandingBalanceReportViewer As CrystalDecisions.Windows.Forms.CrystalReportViewer
End Class
