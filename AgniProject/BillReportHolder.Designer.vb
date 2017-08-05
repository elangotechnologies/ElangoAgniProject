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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
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
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.Transparent
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button1.Location = New System.Drawing.Point(656, 0)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(203, 38)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Export to PDF"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.Transparent
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button2.Location = New System.Drawing.Point(867, 0)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(328, 38)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "PDF file Destination"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'BillReportForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1371, 911)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.reportViewerBillReport)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "BillReportForm"
        Me.Text = "Agni Designs - Bill Generation"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents reportViewerBillReport As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
End Class
