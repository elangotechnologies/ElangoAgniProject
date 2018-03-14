<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Login
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnLoginLogin = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnLoginCancel = New System.Windows.Forms.Button()
        Me.txtLoginPassword = New System.Windows.Forms.TextBox()
        Me.btnLoginChangePassword = New System.Windows.Forms.Button()
        Me.btnLoginManageUsers = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.cmbLoginUserName = New System.Windows.Forms.ComboBox()
        Me.DataSet11 = New AgniProject.AgniDataSet()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataSet11, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(287, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 18)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "User Name"
        '
        'btnLoginLogin
        '
        Me.btnLoginLogin.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.btnLoginLogin.Location = New System.Drawing.Point(402, 116)
        Me.btnLoginLogin.Name = "btnLoginLogin"
        Me.btnLoginLogin.Size = New System.Drawing.Size(149, 31)
        Me.btnLoginLogin.TabIndex = 2
        Me.btnLoginLogin.Text = "Login"
        Me.btnLoginLogin.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(286, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 18)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Password"
        '
        'btnLoginCancel
        '
        Me.btnLoginCancel.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.btnLoginCancel.Location = New System.Drawing.Point(566, 116)
        Me.btnLoginCancel.Name = "btnLoginCancel"
        Me.btnLoginCancel.Size = New System.Drawing.Size(142, 31)
        Me.btnLoginCancel.TabIndex = 3
        Me.btnLoginCancel.Text = "Cancel"
        Me.btnLoginCancel.UseVisualStyleBackColor = True
        '
        'txtLoginPassword
        '
        Me.txtLoginPassword.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.txtLoginPassword.Location = New System.Drawing.Point(391, 71)
        Me.txtLoginPassword.Name = "txtLoginPassword"
        Me.txtLoginPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtLoginPassword.Size = New System.Drawing.Size(336, 26)
        Me.txtLoginPassword.TabIndex = 1
        '
        'btnLoginChangePassword
        '
        Me.btnLoginChangePassword.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.btnLoginChangePassword.Location = New System.Drawing.Point(402, 165)
        Me.btnLoginChangePassword.Name = "btnLoginChangePassword"
        Me.btnLoginChangePassword.Size = New System.Drawing.Size(306, 31)
        Me.btnLoginChangePassword.TabIndex = 4
        Me.btnLoginChangePassword.Text = "Change Password"
        Me.btnLoginChangePassword.UseVisualStyleBackColor = True
        '
        'btnLoginManageUsers
        '
        Me.btnLoginManageUsers.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.btnLoginManageUsers.Location = New System.Drawing.Point(402, 212)
        Me.btnLoginManageUsers.Name = "btnLoginManageUsers"
        Me.btnLoginManageUsers.Size = New System.Drawing.Size(306, 31)
        Me.btnLoginManageUsers.TabIndex = 5
        Me.btnLoginManageUsers.Text = "Manage Users"
        Me.btnLoginManageUsers.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.AgniProject.My.Resources.Resources.login
        Me.PictureBox1.Location = New System.Drawing.Point(1, -1)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(262, 269)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 7
        Me.PictureBox1.TabStop = False
        '
        'cmbLoginUserName
        '
        Me.cmbLoginUserName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbLoginUserName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbLoginUserName.DisplayMember = "username"
        Me.cmbLoginUserName.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.cmbLoginUserName.FormattingEnabled = True
        Me.cmbLoginUserName.Location = New System.Drawing.Point(391, 26)
        Me.cmbLoginUserName.Name = "cmbLoginUserName"
        Me.cmbLoginUserName.Size = New System.Drawing.Size(336, 26)
        Me.cmbLoginUserName.TabIndex = 0
        Me.cmbLoginUserName.ValueMember = "id"
        '
        'DataSet11
        '
        '
        'Login
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(57, Byte), Integer), CType(CType(156, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(739, 267)
        Me.Controls.Add(Me.cmbLoginUserName)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnLoginManageUsers)
        Me.Controls.Add(Me.btnLoginChangePassword)
        Me.Controls.Add(Me.txtLoginPassword)
        Me.Controls.Add(Me.btnLoginCancel)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnLoginLogin)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Login"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Agni Designs - Login"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataSet11, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnLoginLogin As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnLoginCancel As System.Windows.Forms.Button
    Friend WithEvents txtLoginPassword As System.Windows.Forms.TextBox
    Friend WithEvents btnLoginChangePassword As System.Windows.Forms.Button
    Friend WithEvents btnLoginManageUsers As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents cmbLoginUserName As System.Windows.Forms.ComboBox
    Friend WithEvents DataSet11 As AgniDataSet
End Class
