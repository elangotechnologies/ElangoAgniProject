<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BillSearchCrystalReportHolder
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
        Me.reportBillSearchReportViewer = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.SuspendLayout()
        '
        'reportBillSearchReportViewer
        '
        Me.reportBillSearchReportViewer.ActiveViewIndex = -1
        Me.reportBillSearchReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.reportBillSearchReportViewer.Cursor = System.Windows.Forms.Cursors.Default
        Me.reportBillSearchReportViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.reportBillSearchReportViewer.EnableDrillDown = False
        Me.reportBillSearchReportViewer.Location = New System.Drawing.Point(0, 0)
        Me.reportBillSearchReportViewer.Name = "reportBillSearchReportViewer"
        Me.reportBillSearchReportViewer.SelectionFormula = ""
        Me.reportBillSearchReportViewer.Size = New System.Drawing.Size(987, 607)
        Me.reportBillSearchReportViewer.TabIndex = 2
        Me.reportBillSearchReportViewer.ViewTimeSelectionFormula = ""
        '
        'BillSearchCrystalReportHolder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(987, 607)
        Me.Controls.Add(Me.reportBillSearchReportViewer)
        Me.Name = "BillSearchCrystalReportHolder"
        Me.Text = "BillSearchReport"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents reportBillSearchReportViewer As CrystalDecisions.Windows.Forms.CrystalReportViewer
End Class
