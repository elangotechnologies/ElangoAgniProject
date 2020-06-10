<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Login
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnLoginLogin = New CButtonLib.CButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnLoginCancel = New CButtonLib.CButton()
        Me.txtLoginPassword = New ElaCustomTextBoxControl.ElaCustomTextBox()
        Me.btnLoginChangePassword = New CButtonLib.CButton()
        Me.btnLoginManageUsers = New CButtonLib.CButton()
        Me.cmbLoginUserName = New ElaCustomComboBoxControl.ElaCustomComboBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(311, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(93, 19)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "User Name"
        '
        'btnLoginLogin
        '
        Me.btnLoginLogin.AutoEllipsis = False
        Me.btnLoginLogin.AutoSizeMode = False
        Me.btnLoginLogin.BackColor = System.Drawing.Color.Transparent
        Me.btnLoginLogin.Corners.All = 10
        Me.btnLoginLogin.Corners.LowerLeft = 10
        Me.btnLoginLogin.Corners.LowerRight = 10
        Me.btnLoginLogin.Corners.UpperLeft = 10
        Me.btnLoginLogin.Corners.UpperRight = 10
        Me.btnLoginLogin.DesignerSelected = False
        Me.btnLoginLogin.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoginLogin.ImageIndex = 0
        Me.btnLoginLogin.ImageKey = 0
        Me.btnLoginLogin.Location = New System.Drawing.Point(446, 112)
        Me.btnLoginLogin.Name = "btnLoginLogin"
        Me.btnLoginLogin.Size = New System.Drawing.Size(149, 31)
        Me.btnLoginLogin.TabIndex = 2
        Me.btnLoginLogin.Text = "Login"
        Me.btnLoginLogin.TextShadow = System.Drawing.Color.Black
        Me.btnLoginLogin.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(311, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 19)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Password"
        '
        'btnLoginCancel
        '
        Me.btnLoginCancel.AutoEllipsis = False
        Me.btnLoginCancel.AutoSizeMode = False
        Me.btnLoginCancel.BackColor = System.Drawing.Color.Transparent
        Me.btnLoginCancel.Corners.All = 10
        Me.btnLoginCancel.Corners.LowerLeft = 10
        Me.btnLoginCancel.Corners.LowerRight = 10
        Me.btnLoginCancel.Corners.UpperLeft = 10
        Me.btnLoginCancel.Corners.UpperRight = 10
        Me.btnLoginCancel.DesignerSelected = False
        Me.btnLoginCancel.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoginCancel.ImageIndex = 0
        Me.btnLoginCancel.ImageKey = 0
        Me.btnLoginCancel.Location = New System.Drawing.Point(610, 112)
        Me.btnLoginCancel.Name = "btnLoginCancel"
        Me.btnLoginCancel.Size = New System.Drawing.Size(142, 31)
        Me.btnLoginCancel.TabIndex = 3
        Me.btnLoginCancel.Text = "Cancel"
        Me.btnLoginCancel.UseVisualStyleBackColor = False
        '
        'txtLoginPassword
        '
        Me.txtLoginPassword.BorderColor = System.Drawing.Color.DeepSkyBlue
        Me.txtLoginPassword.BorderColorFocus = System.Drawing.Color.Orange
        Me.txtLoginPassword.BorderColorMouseEnter = System.Drawing.Color.Green
        Me.txtLoginPassword.BorderThickness = ElaCustomTextBoxControl.ElaCustomTextBox.BorderThicknessEnum.Thick
        Me.txtLoginPassword.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.txtLoginPassword.Location = New System.Drawing.Point(435, 67)
        Me.txtLoginPassword.Name = "txtLoginPassword"
        Me.txtLoginPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtLoginPassword.Size = New System.Drawing.Size(336, 26)
        Me.txtLoginPassword.TabIndex = 1
        '
        'btnLoginChangePassword
        '
        Me.btnLoginChangePassword.AutoEllipsis = False
        Me.btnLoginChangePassword.AutoSizeMode = False
        Me.btnLoginChangePassword.BackColor = System.Drawing.Color.Transparent
        Me.btnLoginChangePassword.Corners.All = 10
        Me.btnLoginChangePassword.Corners.LowerLeft = 10
        Me.btnLoginChangePassword.Corners.LowerRight = 10
        Me.btnLoginChangePassword.Corners.UpperLeft = 10
        Me.btnLoginChangePassword.Corners.UpperRight = 10
        Me.btnLoginChangePassword.DesignerSelected = False
        Me.btnLoginChangePassword.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoginChangePassword.ImageIndex = 0
        Me.btnLoginChangePassword.ImageKey = 0
        Me.btnLoginChangePassword.Location = New System.Drawing.Point(446, 161)
        Me.btnLoginChangePassword.Name = "btnLoginChangePassword"
        Me.btnLoginChangePassword.Size = New System.Drawing.Size(306, 31)
        Me.btnLoginChangePassword.TabIndex = 4
        Me.btnLoginChangePassword.Text = "Change Password"
        Me.btnLoginChangePassword.UseVisualStyleBackColor = False
        '
        'btnLoginManageUsers
        '
        Me.btnLoginManageUsers.AutoEllipsis = False
        Me.btnLoginManageUsers.AutoSizeMode = False
        Me.btnLoginManageUsers.BackColor = System.Drawing.Color.Transparent
        Me.btnLoginManageUsers.Corners.All = 10
        Me.btnLoginManageUsers.Corners.LowerLeft = 10
        Me.btnLoginManageUsers.Corners.LowerRight = 10
        Me.btnLoginManageUsers.Corners.UpperLeft = 10
        Me.btnLoginManageUsers.Corners.UpperRight = 10
        Me.btnLoginManageUsers.DesignerSelected = False
        Me.btnLoginManageUsers.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoginManageUsers.ImageIndex = 0
        Me.btnLoginManageUsers.ImageKey = 0
        Me.btnLoginManageUsers.Location = New System.Drawing.Point(446, 208)
        Me.btnLoginManageUsers.Name = "btnLoginManageUsers"
        Me.btnLoginManageUsers.Size = New System.Drawing.Size(306, 31)
        Me.btnLoginManageUsers.TabIndex = 5
        Me.btnLoginManageUsers.Text = "Manage Users"
        Me.btnLoginManageUsers.UseVisualStyleBackColor = False
        '
        'cmbLoginUserName
        '
        Me.cmbLoginUserName.ArrowSquareColor = System.Drawing.Color.DeepSkyBlue
        Me.cmbLoginUserName.ArrowSquareColorFocus = System.Drawing.Color.Orange
        Me.cmbLoginUserName.ArrowSquareColorMouseEnter = System.Drawing.Color.Green
        Me.cmbLoginUserName.ArrowTriangleColor = System.Drawing.Color.Gray
        Me.cmbLoginUserName.ArrowTriangleColorFocus = System.Drawing.Color.Gray
        Me.cmbLoginUserName.ArrowTriangleColorMouseEnter = System.Drawing.Color.White
        Me.cmbLoginUserName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbLoginUserName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbLoginUserName.BorderColor = System.Drawing.Color.DeepSkyBlue
        Me.cmbLoginUserName.BorderColorFocus = System.Drawing.Color.Orange
        Me.cmbLoginUserName.BorderColorMouseEnter = System.Drawing.Color.Green
        Me.cmbLoginUserName.BorderThickness = ElaCustomComboBoxControl.ElaCustomComboBox.BorderThicknessEnum.Thick
        Me.cmbLoginUserName.DisplayMember = "username"
        Me.cmbLoginUserName.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.cmbLoginUserName.FormattingEnabled = True
        Me.cmbLoginUserName.Location = New System.Drawing.Point(435, 22)
        Me.cmbLoginUserName.Name = "cmbLoginUserName"
        Me.cmbLoginUserName.Size = New System.Drawing.Size(336, 26)
        Me.cmbLoginUserName.TabIndex = 0
        Me.cmbLoginUserName.ValueMember = "id"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = Global.AgniProject.My.Resources.Resources.AgniLogo
        Me.PictureBox1.Location = New System.Drawing.Point(0, -1)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(288, 278)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 6
        Me.PictureBox1.TabStop = False
        '
        'Login
        '
        Me.AcceptButton = Me.btnLoginLogin
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.GhostWhite
        Me.BackgroundImage = Global.AgniProject.My.Resources.Resources.bg1
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(848, 275)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.cmbLoginUserName)
        Me.Controls.Add(Me.btnLoginManageUsers)
        Me.Controls.Add(Me.btnLoginChangePassword)
        Me.Controls.Add(Me.txtLoginPassword)
        Me.Controls.Add(Me.btnLoginCancel)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnLoginLogin)
        Me.Controls.Add(Me.Label1)
        Me.KeyPreview = True
        Me.Name = "Login"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " "
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnLoginLogin As CButtonLib.CButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnLoginCancel As CButtonLib.CButton
    Friend WithEvents txtLoginPassword As ElaCustomTextBoxControl.ElaCustomTextBox
    Friend WithEvents btnLoginChangePassword As CButtonLib.CButton
    Friend WithEvents btnLoginManageUsers As CButtonLib.CButton
    Friend WithEvents cmbLoginUserName As ElaCustomComboBoxControl.ElaCustomComboBox
    Friend WithEvents PictureBox1 As PictureBox
End Class
