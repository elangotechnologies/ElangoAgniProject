<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ChangePassword
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
        Me.txtChangePwdNewPassword = New System.Windows.Forms.TextBox()
        Me.btnChangePwdCancel = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnChangePwdConfim = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtChangePwdRetypeNewPassword = New System.Windows.Forms.TextBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.cmbChangePwdUserName = New System.Windows.Forms.ComboBox()
        Me.txtChangePwdOldPassword = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtChangePwdNewPassword
        '
        Me.txtChangePwdNewPassword.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.txtChangePwdNewPassword.Location = New System.Drawing.Point(422, 117)
        Me.txtChangePwdNewPassword.Name = "txtChangePwdNewPassword"
        Me.txtChangePwdNewPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtChangePwdNewPassword.Size = New System.Drawing.Size(306, 26)
        Me.txtChangePwdNewPassword.TabIndex = 2
        '
        'btnChangePwdCancel
        '
        Me.btnChangePwdCancel.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnChangePwdCancel.Location = New System.Drawing.Point(576, 224)
        Me.btnChangePwdCancel.Name = "btnChangePwdCancel"
        Me.btnChangePwdCancel.Size = New System.Drawing.Size(136, 31)
        Me.btnChangePwdCancel.TabIndex = 5
        Me.btnChangePwdCancel.Text = "Cancel"
        Me.btnChangePwdCancel.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(265, 164)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(140, 18)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Re-Type Password"
        '
        'btnChangePwdConfim
        '
        Me.btnChangePwdConfim.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnChangePwdConfim.Location = New System.Drawing.Point(422, 224)
        Me.btnChangePwdConfim.Name = "btnChangePwdConfim"
        Me.btnChangePwdConfim.Size = New System.Drawing.Size(136, 31)
        Me.btnChangePwdConfim.TabIndex = 4
        Me.btnChangePwdConfim.Text = "Confirm"
        Me.btnChangePwdConfim.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(265, 118)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(113, 18)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "New Password"
        '
        'txtChangePwdRetypeNewPassword
        '
        Me.txtChangePwdRetypeNewPassword.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.txtChangePwdRetypeNewPassword.Location = New System.Drawing.Point(422, 162)
        Me.txtChangePwdRetypeNewPassword.Name = "txtChangePwdRetypeNewPassword"
        Me.txtChangePwdRetypeNewPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtChangePwdRetypeNewPassword.Size = New System.Drawing.Size(306, 26)
        Me.txtChangePwdRetypeNewPassword.TabIndex = 3
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(57, Byte), Integer), CType(CType(156, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.PictureBox1.Image = Global.AgniProject.My.Resources.Resources.login
        Me.PictureBox1.Location = New System.Drawing.Point(-2, -1)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(262, 269)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 11
        Me.PictureBox1.TabStop = False
        '
        'cmbChangePwdUserName
        '
        Me.cmbChangePwdUserName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbChangePwdUserName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbChangePwdUserName.DisplayMember = "username"
        Me.cmbChangePwdUserName.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.cmbChangePwdUserName.FormattingEnabled = True
        Me.cmbChangePwdUserName.Location = New System.Drawing.Point(422, 23)
        Me.cmbChangePwdUserName.Name = "cmbChangePwdUserName"
        Me.cmbChangePwdUserName.Size = New System.Drawing.Size(306, 26)
        Me.cmbChangePwdUserName.TabIndex = 0
        Me.cmbChangePwdUserName.ValueMember = "id"
        '
        'txtChangePwdOldPassword
        '
        Me.txtChangePwdOldPassword.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.txtChangePwdOldPassword.Location = New System.Drawing.Point(422, 70)
        Me.txtChangePwdOldPassword.Name = "txtChangePwdOldPassword"
        Me.txtChangePwdOldPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtChangePwdOldPassword.Size = New System.Drawing.Size(306, 26)
        Me.txtChangePwdOldPassword.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(265, 72)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(106, 18)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "Old Password"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(265, 26)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(87, 18)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "User Name"
        '
        'ChangePassword
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(57, Byte), Integer), CType(CType(156, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(739, 267)
        Me.Controls.Add(Me.cmbChangePwdUserName)
        Me.Controls.Add(Me.txtChangePwdOldPassword)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.txtChangePwdRetypeNewPassword)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtChangePwdNewPassword)
        Me.Controls.Add(Me.btnChangePwdCancel)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnChangePwdConfim)
        Me.Name = "ChangePassword"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Agni Designs -Change Password"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtChangePwdNewPassword As System.Windows.Forms.TextBox
    Friend WithEvents btnChangePwdCancel As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnChangePwdConfim As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtChangePwdRetypeNewPassword As System.Windows.Forms.TextBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents cmbChangePwdUserName As ComboBox
    Friend WithEvents txtChangePwdOldPassword As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
End Class
