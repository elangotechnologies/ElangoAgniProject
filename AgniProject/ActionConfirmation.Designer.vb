<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ActionConfirmation
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
        Me.txtActionConfirmPassword = New System.Windows.Forms.TextBox()
        Me.btnActionConfirmationCancel = New CButtonLib.CButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnActionConfirmationConfirm = New CButtonLib.CButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.cmbActionConfirmUserName = New System.Windows.Forms.ComboBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtActionConfirmPassword
        '
        Me.txtActionConfirmPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtActionConfirmPassword.Location = New System.Drawing.Point(345, 75)
        Me.txtActionConfirmPassword.Name = "txtActionConfirmPassword"
        Me.txtActionConfirmPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtActionConfirmPassword.Size = New System.Drawing.Size(336, 24)
        Me.txtActionConfirmPassword.TabIndex = 1
        '
        'btnActionConfirmationCancel
        '
        Me.btnActionConfirmationCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnActionConfirmationCancel.Location = New System.Drawing.Point(523, 119)
        Me.btnActionConfirmationCancel.Name = "btnActionConfirmationCancel"
        Me.btnActionConfirmationCancel.Size = New System.Drawing.Size(101, 31)
        Me.btnActionConfirmationCancel.TabIndex = 3
        Me.btnActionConfirmationCancel.Text = "Cancel"
        Me.btnActionConfirmationCancel.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(252, 78)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 18)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Password"
        '
        'btnActionConfirmationConfirm
        '
        Me.btnActionConfirmationConfirm.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnActionConfirmationConfirm.AutoSize = True
        Me.btnActionConfirmationConfirm.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnActionConfirmationConfirm.Location = New System.Drawing.Point(388, 119)
        Me.btnActionConfirmationConfirm.Name = "btnActionConfirmationConfirm"
        Me.btnActionConfirmationConfirm.Size = New System.Drawing.Size(101, 31)
        Me.btnActionConfirmationConfirm.TabIndex = 2
        Me.btnActionConfirmationConfirm.Text = "Confirm"
        Me.btnActionConfirmationConfirm.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(252, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 18)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "User Name"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.AgniProject.My.Resources.Resources.login
        Me.PictureBox1.Location = New System.Drawing.Point(0, -1)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(246, 171)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 14
        Me.PictureBox1.TabStop = False
        '
        'cmbActionConfirmUserName
        '
        Me.cmbActionConfirmUserName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbActionConfirmUserName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbActionConfirmUserName.DisplayMember = "username"
        Me.cmbActionConfirmUserName.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.cmbActionConfirmUserName.FormattingEnabled = True
        Me.cmbActionConfirmUserName.Location = New System.Drawing.Point(345, 33)
        Me.cmbActionConfirmUserName.Name = "cmbActionConfirmUserName"
        Me.cmbActionConfirmUserName.Size = New System.Drawing.Size(336, 26)
        Me.cmbActionConfirmUserName.TabIndex = 0
        Me.cmbActionConfirmUserName.ValueMember = "id"
        '
        'ActionConfirmation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(57, Byte), Integer), CType(CType(156, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(691, 172)
        Me.Controls.Add(Me.cmbActionConfirmUserName)
        Me.Controls.Add(Me.btnActionConfirmationConfirm)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.txtActionConfirmPassword)
        Me.Controls.Add(Me.btnActionConfirmationCancel)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "ActionConfirmation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "VerifyingDelete"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents txtActionConfirmPassword As System.Windows.Forms.TextBox
    Friend WithEvents btnActionConfirmationCancel As CButtonLib.CButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnActionConfirmationConfirm As CButtonLib.CButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbActionConfirmUserName As ComboBox
End Class
