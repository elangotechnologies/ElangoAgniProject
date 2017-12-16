<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImagePopup
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
        Me.pbImageBox = New System.Windows.Forms.PictureBox()
        CType(Me.pbImageBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pbImageBox
        '
        Me.pbImageBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbImageBox.Location = New System.Drawing.Point(0, 0)
        Me.pbImageBox.Name = "pbImageBox"
        Me.pbImageBox.Size = New System.Drawing.Size(532, 393)
        Me.pbImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbImageBox.TabIndex = 0
        Me.pbImageBox.TabStop = False
        '
        'ImagePopup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(532, 393)
        Me.Controls.Add(Me.pbImageBox)
        Me.Cursor = System.Windows.Forms.Cursors.SizeAll
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ImagePopup"
        Me.Text = "Preview - Design Image"
        CType(Me.pbImageBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pbImageBox As PictureBox
End Class
