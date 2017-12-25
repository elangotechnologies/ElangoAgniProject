<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BillAndPaymentHistoryCrystalReportHolder
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
        Me.BillAndPaymentHistoryReportViewer = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.SuspendLayout()
        '
        'BillAndPaymentHistoryReportViewer
        '
        Me.BillAndPaymentHistoryReportViewer.ActiveViewIndex = -1
        Me.BillAndPaymentHistoryReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.BillAndPaymentHistoryReportViewer.Cursor = System.Windows.Forms.Cursors.Default
        Me.BillAndPaymentHistoryReportViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BillAndPaymentHistoryReportViewer.Location = New System.Drawing.Point(0, 0)
        Me.BillAndPaymentHistoryReportViewer.Name = "BillAndPaymentHistoryReportViewer"
        Me.BillAndPaymentHistoryReportViewer.Size = New System.Drawing.Size(1195, 737)
        Me.BillAndPaymentHistoryReportViewer.TabIndex = 0
        '
        'BillAndPaymentHistoryCrystalReportHolder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1195, 737)
        Me.Controls.Add(Me.BillAndPaymentHistoryReportViewer)
        Me.Name = "BillAndPaymentHistoryCrystalReportHolder"
        Me.Text = "BillAndPaymentHistoryCrystalReportHolder"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents BillAndPaymentHistoryReportViewer As CrystalDecisions.Windows.Forms.CrystalReportViewer
End Class
