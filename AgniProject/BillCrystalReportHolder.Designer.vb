<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BillReportForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.reportViewerBillReport = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.btnBillReportExportPdf = New CButtonLib.CButton()
        Me.btnBillReportPDFPath = New CButtonLib.CButton()
        Me.CachedBillReport1 = New AgniProject.CachedBillReport()
        Me.CachedBillReport2 = New AgniProject.CachedBillReport()
        Me.SuspendLayout()
        '
        'reportViewerBillReport
        '
        Me.reportViewerBillReport.ActiveViewIndex = -1
        Me.reportViewerBillReport.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.reportViewerBillReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.reportViewerBillReport.Dock = System.Windows.Forms.DockStyle.Fill
        Me.reportViewerBillReport.EnableDrillDown = False
        Me.reportViewerBillReport.Location = New System.Drawing.Point(0, 0)
        Me.reportViewerBillReport.Name = "reportViewerBillReport"
        Me.reportViewerBillReport.SelectionFormula = ""
        Me.reportViewerBillReport.Size = New System.Drawing.Size(1028, 740)
        Me.reportViewerBillReport.TabIndex = 0
        Me.reportViewerBillReport.ViewTimeSelectionFormula = ""
        '
        'btnBillReportExportPdf
        '
        Me.btnBillReportExportPdf.AutoEllipsis = False
        Me.btnBillReportExportPdf.AutoSizeMode = False
        Me.btnBillReportExportPdf.BackColor = System.Drawing.Color.Transparent
        Me.btnBillReportExportPdf.DesignerSelected = False
        Me.btnBillReportExportPdf.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnBillReportExportPdf.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnBillReportExportPdf.ImageIndex = 0
        Me.btnBillReportExportPdf.ImageKey = 0
        Me.btnBillReportExportPdf.Location = New System.Drawing.Point(529, 5)
        Me.btnBillReportExportPdf.Name = "btnBillReportExportPdf"
        Me.btnBillReportExportPdf.Size = New System.Drawing.Size(120, 23)
        Me.btnBillReportExportPdf.TabIndex = 1
        Me.btnBillReportExportPdf.Text = "Export to PDF"
        Me.btnBillReportExportPdf.UseVisualStyleBackColor = False
        '
        'btnBillReportPDFPath
        '
        Me.btnBillReportPDFPath.AutoEllipsis = False
        Me.btnBillReportPDFPath.AutoSizeMode = False
        Me.btnBillReportPDFPath.BackColor = System.Drawing.Color.Transparent
        Me.btnBillReportPDFPath.DesignerSelected = True
        Me.btnBillReportPDFPath.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnBillReportPDFPath.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnBillReportPDFPath.ImageIndex = 0
        Me.btnBillReportPDFPath.ImageKey = 0
        Me.btnBillReportPDFPath.Location = New System.Drawing.Point(693, 5)
        Me.btnBillReportPDFPath.Name = "btnBillReportPDFPath"
        Me.btnBillReportPDFPath.Size = New System.Drawing.Size(133, 23)
        Me.btnBillReportPDFPath.TabIndex = 2
        Me.btnBillReportPDFPath.Text = "Set PDF Directory"
        Me.btnBillReportPDFPath.UseVisualStyleBackColor = False
        '
        'BillReportForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 740)
        Me.Controls.Add(Me.btnBillReportPDFPath)
        Me.Controls.Add(Me.btnBillReportExportPdf)
        Me.Controls.Add(Me.reportViewerBillReport)
        Me.Name = "BillReportForm"
        Me.Text = "Agni Designs - Bill Generation"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents reportViewerBillReport As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents btnBillReportExportPdf As CButtonLib.CButton
    Friend WithEvents btnBillReportPDFPath As CButtonLib.CButton
    Friend WithEvents CachedBillReport1 As CachedBillReport
    Friend WithEvents CachedBillReport2 As CachedBillReport
End Class
