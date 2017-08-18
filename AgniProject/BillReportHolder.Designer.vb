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
        Me.reportViewerBillReport = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.btnBillReportExportPdf = New System.Windows.Forms.Button()
        Me.btnBillReportPDFPath = New System.Windows.Forms.Button()
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
        Me.reportViewerBillReport.Margin = New System.Windows.Forms.Padding(4)
        Me.reportViewerBillReport.Name = "reportViewerBillReport"
        Me.reportViewerBillReport.SelectionFormula = ""
        Me.reportViewerBillReport.Size = New System.Drawing.Size(1371, 911)
        Me.reportViewerBillReport.TabIndex = 0
        Me.reportViewerBillReport.ToolPanelWidth = 267
        Me.reportViewerBillReport.ViewTimeSelectionFormula = ""
        '
        'btnBillReportExportPdf
        '
        Me.btnBillReportExportPdf.BackColor = System.Drawing.Color.Transparent
        Me.btnBillReportExportPdf.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBillReportExportPdf.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnBillReportExportPdf.Location = New System.Drawing.Point(705, 6)
        Me.btnBillReportExportPdf.Margin = New System.Windows.Forms.Padding(4)
        Me.btnBillReportExportPdf.Name = "btnBillReportExportPdf"
        Me.btnBillReportExportPdf.Size = New System.Drawing.Size(154, 28)
        Me.btnBillReportExportPdf.TabIndex = 1
        Me.btnBillReportExportPdf.Text = "Export to PDF"
        Me.btnBillReportExportPdf.UseVisualStyleBackColor = False
        '
        'btnBillReportPDFPath
        '
        Me.btnBillReportPDFPath.BackColor = System.Drawing.Color.Transparent
        Me.btnBillReportPDFPath.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.btnBillReportPDFPath.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnBillReportPDFPath.Location = New System.Drawing.Point(924, 6)
        Me.btnBillReportPDFPath.Margin = New System.Windows.Forms.Padding(4)
        Me.btnBillReportPDFPath.Name = "btnBillReportPDFPath"
        Me.btnBillReportPDFPath.Size = New System.Drawing.Size(154, 28)
        Me.btnBillReportPDFPath.TabIndex = 2
        Me.btnBillReportPDFPath.Text = "Set PDF Directory"
        Me.btnBillReportPDFPath.UseVisualStyleBackColor = False
        '
        'BillReportForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1371, 911)
        Me.Controls.Add(Me.btnBillReportPDFPath)
        Me.Controls.Add(Me.btnBillReportExportPdf)
        Me.Controls.Add(Me.reportViewerBillReport)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "BillReportForm"
        Me.Text = "Agni Designs - Bill Generation"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents reportViewerBillReport As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents btnBillReportExportPdf As System.Windows.Forms.Button
    Friend WithEvents btnBillReportPDFPath As System.Windows.Forms.Button
End Class
