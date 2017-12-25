<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PaymentSearchCrystalReportHolder
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
        Me.paymentSearchReportViewer = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.SuspendLayout()
        '
        'paymentSearchReportViewer
        '
        Me.paymentSearchReportViewer.ActiveViewIndex = -1
        Me.paymentSearchReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.paymentSearchReportViewer.Cursor = System.Windows.Forms.Cursors.Default
        Me.paymentSearchReportViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.paymentSearchReportViewer.Location = New System.Drawing.Point(0, 0)
        Me.paymentSearchReportViewer.Name = "paymentSearchReportViewer"
        Me.paymentSearchReportViewer.Size = New System.Drawing.Size(1296, 801)
        Me.paymentSearchReportViewer.TabIndex = 0
        '
        'PaymentSearchCrystalReportHolder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1296, 801)
        Me.Controls.Add(Me.paymentSearchReportViewer)
        Me.Name = "PaymentSearchCrystalReportHolder"
        Me.Text = "PaymentSearchCrystalReportHolder"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents paymentSearchReportViewer As CrystalDecisions.Windows.Forms.CrystalReportViewer
End Class
