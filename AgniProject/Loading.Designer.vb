<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Loading
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Loading))
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.pbLoadingImage = New System.Windows.Forms.PictureBox()
        CType(Me.pbLoadingImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'pbLoadingImage
        '
        Me.pbLoadingImage.Image = CType(resources.GetObject("pbLoadingImage.Image"), System.Drawing.Image)
        Me.pbLoadingImage.Location = New System.Drawing.Point(12, 12)
        Me.pbLoadingImage.Name = "pbLoadingImage"
        Me.pbLoadingImage.Size = New System.Drawing.Size(271, 247)
        Me.pbLoadingImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbLoadingImage.TabIndex = 4
        Me.pbLoadingImage.TabStop = False
        '
        'Loading
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightBlue
        Me.ClientSize = New System.Drawing.Size(303, 277)
        Me.Controls.Add(Me.pbLoadingImage)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Loading"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Loading"
        Me.TransparencyKey = System.Drawing.Color.LightBlue
        CType(Me.pbLoadingImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pbLoadingImage As PictureBox
    Friend WithEvents Timer1 As Timer
End Class
