<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ManageUsers
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
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.txtManageUsersRetypePassword = New ElaCustomTextBoxControl.ElaCustomTextBox()
        Me.lblManageUsersPassword = New System.Windows.Forms.Label()
        Me.txtManageUsersPassword = New ElaCustomTextBoxControl.ElaCustomTextBox()
        Me.btnManageUsersCancel = New CButtonLib.CButton()
        Me.lblManageUsersRetypePassword = New System.Windows.Forms.Label()
        Me.btnManageUsersCreateUser = New CButtonLib.CButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.rbManageUsersAdmin = New System.Windows.Forms.RadioButton()
        Me.rbManageUsersGuest = New System.Windows.Forms.RadioButton()
        Me.gbManageUsersUserType = New ElaCustomGroupBoxControl.ElaCustomGroupBox()
        Me.btnManageUsersDeleteUser = New CButtonLib.CButton()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.gbUsersList = New ElaCustomGroupBoxControl.ElaCustomGroupBox()
        Me.dgManageUsersUsersGrid = New System.Windows.Forms.DataGridView()
        Me.UserGridUserId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UserGridUserName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UserGridPassword = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UserGridType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UserGridPasswordOrig = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UserGridTypeId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnManageUsersCreateUserConfirm = New CButtonLib.CButton()
        Me.btnManageUsersCreateUserCancel = New CButtonLib.CButton()
        Me.cmbManageUsersUserName = New ElaCustomComboBoxControl.ElaCustomComboBox()
        Me.gbManageUsersUserType.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbUsersList.SuspendLayout()
        CType(Me.dgManageUsersUsersGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtManageUsersRetypePassword
        '
        Me.txtManageUsersRetypePassword.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtManageUsersRetypePassword.Location = New System.Drawing.Point(419, 157)
        Me.txtManageUsersRetypePassword.Name = "txtManageUsersRetypePassword"
        Me.txtManageUsersRetypePassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtManageUsersRetypePassword.Size = New System.Drawing.Size(308, 26)
        Me.txtManageUsersRetypePassword.TabIndex = 3
        Me.txtManageUsersRetypePassword.Visible = False
        '
        'lblManageUsersPassword
        '
        Me.lblManageUsersPassword.AutoSize = True
        Me.lblManageUsersPassword.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblManageUsersPassword.ForeColor = System.Drawing.Color.White
        Me.lblManageUsersPassword.Location = New System.Drawing.Point(268, 119)
        Me.lblManageUsersPassword.Name = "lblManageUsersPassword"
        Me.lblManageUsersPassword.Size = New System.Drawing.Size(78, 18)
        Me.lblManageUsersPassword.TabIndex = 18
        Me.lblManageUsersPassword.Text = "Password"
        Me.lblManageUsersPassword.Visible = False
        '
        'txtManageUsersPassword
        '
        Me.txtManageUsersPassword.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtManageUsersPassword.Location = New System.Drawing.Point(419, 113)
        Me.txtManageUsersPassword.Name = "txtManageUsersPassword"
        Me.txtManageUsersPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtManageUsersPassword.Size = New System.Drawing.Size(308, 26)
        Me.txtManageUsersPassword.TabIndex = 2
        Me.txtManageUsersPassword.Visible = False
        '
        'btnManageUsersCancel
        '
        Me.btnManageUsersCancel.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnManageUsersCancel.Location = New System.Drawing.Point(609, 207)
        Me.btnManageUsersCancel.Name = "btnManageUsersCancel"
        Me.btnManageUsersCancel.Size = New System.Drawing.Size(118, 31)
        Me.btnManageUsersCancel.TabIndex = 6
        Me.btnManageUsersCancel.Text = "Cancel"
        Me.btnManageUsersCancel.UseVisualStyleBackColor = True
        '
        'lblManageUsersRetypePassword
        '
        Me.lblManageUsersRetypePassword.AutoSize = True
        Me.lblManageUsersRetypePassword.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblManageUsersRetypePassword.ForeColor = System.Drawing.Color.White
        Me.lblManageUsersRetypePassword.Location = New System.Drawing.Point(268, 163)
        Me.lblManageUsersRetypePassword.Name = "lblManageUsersRetypePassword"
        Me.lblManageUsersRetypePassword.Size = New System.Drawing.Size(140, 18)
        Me.lblManageUsersRetypePassword.TabIndex = 17
        Me.lblManageUsersRetypePassword.Text = "Re-Type Password"
        Me.lblManageUsersRetypePassword.Visible = False
        '
        'btnManageUsersCreateUser
        '
        Me.btnManageUsersCreateUser.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnManageUsersCreateUser.Location = New System.Drawing.Point(361, 207)
        Me.btnManageUsersCreateUser.Name = "btnManageUsersCreateUser"
        Me.btnManageUsersCreateUser.Size = New System.Drawing.Size(118, 31)
        Me.btnManageUsersCreateUser.TabIndex = 4
        Me.btnManageUsersCreateUser.Text = "Create User"
        Me.btnManageUsersCreateUser.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(268, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 18)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "User Name"
        '
        'rbManageUsersAdmin
        '
        Me.rbManageUsersAdmin.AutoSize = True
        Me.rbManageUsersAdmin.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbManageUsersAdmin.ForeColor = System.Drawing.Color.White
        Me.rbManageUsersAdmin.Location = New System.Drawing.Point(24, 13)
        Me.rbManageUsersAdmin.Name = "rbManageUsersAdmin"
        Me.rbManageUsersAdmin.Size = New System.Drawing.Size(119, 22)
        Me.rbManageUsersAdmin.TabIndex = 0
        Me.rbManageUsersAdmin.Text = "Administrator"
        Me.rbManageUsersAdmin.UseVisualStyleBackColor = True
        '
        'rbManageUsersGuest
        '
        Me.rbManageUsersGuest.AutoSize = True
        Me.rbManageUsersGuest.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbManageUsersGuest.ForeColor = System.Drawing.Color.White
        Me.rbManageUsersGuest.Location = New System.Drawing.Point(196, 13)
        Me.rbManageUsersGuest.Name = "rbManageUsersGuest"
        Me.rbManageUsersGuest.Size = New System.Drawing.Size(67, 22)
        Me.rbManageUsersGuest.TabIndex = 1
        Me.rbManageUsersGuest.Text = "Guest"
        Me.rbManageUsersGuest.UseVisualStyleBackColor = True
        '
        'gbManageUsersUserType
        '
        Me.gbManageUsersUserType.Controls.Add(Me.rbManageUsersAdmin)
        Me.gbManageUsersUserType.Controls.Add(Me.rbManageUsersGuest)
        Me.gbManageUsersUserType.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbManageUsersUserType.ForeColor = System.Drawing.Color.Black
        Me.gbManageUsersUserType.Location = New System.Drawing.Point(419, 56)
        Me.gbManageUsersUserType.Name = "gbManageUsersUserType"
        Me.gbManageUsersUserType.Size = New System.Drawing.Size(308, 42)
        Me.gbManageUsersUserType.TabIndex = 1
        Me.gbManageUsersUserType.TabStop = False
        '
        'btnManageUsersDeleteUser
        '
        Me.btnManageUsersDeleteUser.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnManageUsersDeleteUser.Location = New System.Drawing.Point(485, 207)
        Me.btnManageUsersDeleteUser.Name = "btnManageUsersDeleteUser"
        Me.btnManageUsersDeleteUser.Size = New System.Drawing.Size(118, 31)
        Me.btnManageUsersDeleteUser.TabIndex = 5
        Me.btnManageUsersDeleteUser.Text = "Delete User"
        Me.btnManageUsersDeleteUser.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.AgniProject.My.Resources.Resources.login
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(262, 287)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 22
        Me.PictureBox1.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(268, 73)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(78, 18)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "User Type"
        '
        'gbUsersList
        '
        Me.gbUsersList.Controls.Add(Me.dgManageUsersUsersGrid)
        Me.gbUsersList.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbUsersList.ForeColor = System.Drawing.Color.White
        Me.gbUsersList.Location = New System.Drawing.Point(0, 293)
        Me.gbUsersList.Name = "gbUsersList"
        Me.gbUsersList.Size = New System.Drawing.Size(742, 322)
        Me.gbUsersList.TabIndex = 9
        Me.gbUsersList.TabStop = False
        Me.gbUsersList.Text = "User List"
        '
        'dgManageUsersUsersGrid
        '
        Me.dgManageUsersUsersGrid.AllowUserToAddRows = False
        Me.dgManageUsersUsersGrid.AllowUserToDeleteRows = False
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.LightSteelBlue
        Me.dgManageUsersUsersGrid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle5
        Me.dgManageUsersUsersGrid.BackgroundColor = System.Drawing.Color.Lavender
        Me.dgManageUsersUsersGrid.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgManageUsersUsersGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical
        Me.dgManageUsersUsersGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.RoyalBlue
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgManageUsersUsersGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.dgManageUsersUsersGrid.ColumnHeadersHeight = 35
        Me.dgManageUsersUsersGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.UserGridUserId, Me.UserGridUserName, Me.UserGridPassword, Me.UserGridType, Me.UserGridPasswordOrig, Me.UserGridTypeId})
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.LightSteelBlue
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgManageUsersUsersGrid.DefaultCellStyle = DataGridViewCellStyle7
        Me.dgManageUsersUsersGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgManageUsersUsersGrid.EnableHeadersVisualStyles = False
        Me.dgManageUsersUsersGrid.GridColor = System.Drawing.Color.Maroon
        Me.dgManageUsersUsersGrid.Location = New System.Drawing.Point(3, 22)
        Me.dgManageUsersUsersGrid.MultiSelect = False
        Me.dgManageUsersUsersGrid.Name = "dgManageUsersUsersGrid"
        Me.dgManageUsersUsersGrid.ReadOnly = True
        Me.dgManageUsersUsersGrid.RowHeadersVisible = False
        DataGridViewCellStyle8.BackColor = System.Drawing.Color.Lavender
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgManageUsersUsersGrid.RowsDefaultCellStyle = DataGridViewCellStyle8
        Me.dgManageUsersUsersGrid.RowTemplate.Height = 25
        Me.dgManageUsersUsersGrid.RowTemplate.ReadOnly = True
        Me.dgManageUsersUsersGrid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgManageUsersUsersGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgManageUsersUsersGrid.Size = New System.Drawing.Size(736, 297)
        Me.dgManageUsersUsersGrid.TabIndex = 0
        '
        'UserGridUserId
        '
        Me.UserGridUserId.DataPropertyName = "id"
        Me.UserGridUserId.HeaderText = "Id"
        Me.UserGridUserId.Name = "UserGridUserId"
        Me.UserGridUserId.ReadOnly = True
        Me.UserGridUserId.Width = 60
        '
        'UserGridUserName
        '
        Me.UserGridUserName.DataPropertyName = "username"
        Me.UserGridUserName.HeaderText = "User Name"
        Me.UserGridUserName.Name = "UserGridUserName"
        Me.UserGridUserName.ReadOnly = True
        Me.UserGridUserName.Width = 300
        '
        'UserGridPassword
        '
        Me.UserGridPassword.DataPropertyName = "encrPassword"
        Me.UserGridPassword.HeaderText = "Password"
        Me.UserGridPassword.Name = "UserGridPassword"
        Me.UserGridPassword.ReadOnly = True
        Me.UserGridPassword.Width = 140
        '
        'UserGridType
        '
        Me.UserGridType.DataPropertyName = "usertype"
        Me.UserGridType.HeaderText = "User Type"
        Me.UserGridType.Name = "UserGridType"
        Me.UserGridType.ReadOnly = True
        Me.UserGridType.Width = 236
        '
        'UserGridPasswordOrig
        '
        Me.UserGridPasswordOrig.DataPropertyName = "password"
        Me.UserGridPasswordOrig.HeaderText = "PwdOrig"
        Me.UserGridPasswordOrig.Name = "UserGridPasswordOrig"
        Me.UserGridPasswordOrig.ReadOnly = True
        Me.UserGridPasswordOrig.Visible = False
        '
        'UserGridTypeId
        '
        Me.UserGridTypeId.DataPropertyName = "typeid"
        Me.UserGridTypeId.HeaderText = "TypeId"
        Me.UserGridTypeId.Name = "UserGridTypeId"
        Me.UserGridTypeId.ReadOnly = True
        Me.UserGridTypeId.Visible = False
        '
        'btnManageUsersCreateUserConfirm
        '
        Me.btnManageUsersCreateUserConfirm.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnManageUsersCreateUserConfirm.Location = New System.Drawing.Point(431, 234)
        Me.btnManageUsersCreateUserConfirm.Name = "btnManageUsersCreateUserConfirm"
        Me.btnManageUsersCreateUserConfirm.Size = New System.Drawing.Size(118, 31)
        Me.btnManageUsersCreateUserConfirm.TabIndex = 7
        Me.btnManageUsersCreateUserConfirm.Text = "Confirm"
        Me.btnManageUsersCreateUserConfirm.UseVisualStyleBackColor = True
        Me.btnManageUsersCreateUserConfirm.Visible = False
        '
        'btnManageUsersCreateUserCancel
        '
        Me.btnManageUsersCreateUserCancel.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnManageUsersCreateUserCancel.Location = New System.Drawing.Point(555, 234)
        Me.btnManageUsersCreateUserCancel.Name = "btnManageUsersCreateUserCancel"
        Me.btnManageUsersCreateUserCancel.Size = New System.Drawing.Size(118, 31)
        Me.btnManageUsersCreateUserCancel.TabIndex = 8
        Me.btnManageUsersCreateUserCancel.Text = "Back"
        Me.btnManageUsersCreateUserCancel.UseVisualStyleBackColor = True
        Me.btnManageUsersCreateUserCancel.Visible = False
        '
        'cmbManageUsersUserName
        '
        Me.cmbManageUsersUserName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbManageUsersUserName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbManageUsersUserName.DisplayMember = "username"
        Me.cmbManageUsersUserName.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.cmbManageUsersUserName.FormattingEnabled = True
        Me.cmbManageUsersUserName.Location = New System.Drawing.Point(419, 26)
        Me.cmbManageUsersUserName.Name = "cmbManageUsersUserName"
        Me.cmbManageUsersUserName.Size = New System.Drawing.Size(308, 26)
        Me.cmbManageUsersUserName.TabIndex = 0
        Me.cmbManageUsersUserName.ValueMember = "id"
        '
        'ManageUsers
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(57, Byte), Integer), CType(CType(156, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(739, 613)
        Me.Controls.Add(Me.cmbManageUsersUserName)
        Me.Controls.Add(Me.btnManageUsersCreateUserCancel)
        Me.Controls.Add(Me.btnManageUsersCreateUserConfirm)
        Me.Controls.Add(Me.gbUsersList)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnManageUsersDeleteUser)
        Me.Controls.Add(Me.txtManageUsersRetypePassword)
        Me.Controls.Add(Me.lblManageUsersPassword)
        Me.Controls.Add(Me.txtManageUsersPassword)
        Me.Controls.Add(Me.btnManageUsersCancel)
        Me.Controls.Add(Me.lblManageUsersRetypePassword)
        Me.Controls.Add(Me.btnManageUsersCreateUser)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.gbManageUsersUserType)
        Me.Name = "ManageUsers"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Agni Designs - Manage user"
        Me.gbManageUsersUserType.ResumeLayout(False)
        Me.gbManageUsersUserType.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbUsersList.ResumeLayout(False)
        CType(Me.dgManageUsersUsersGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtManageUsersRetypePassword As ElaCustomTextBoxControl.ElaCustomTextBox
    Friend WithEvents lblManageUsersPassword As System.Windows.Forms.Label
    Friend WithEvents txtManageUsersPassword As ElaCustomTextBoxControl.ElaCustomTextBox
    Friend WithEvents btnManageUsersCancel As CButtonLib.CButton
    Friend WithEvents lblManageUsersRetypePassword As System.Windows.Forms.Label
    Friend WithEvents btnManageUsersCreateUser As CButtonLib.CButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents rbManageUsersAdmin As System.Windows.Forms.RadioButton
    Friend WithEvents rbManageUsersGuest As System.Windows.Forms.RadioButton
    Friend WithEvents gbManageUsersUserType As ElaCustomGroupBoxControl.ElaCustomGroupBox
    Friend WithEvents btnManageUsersDeleteUser As CButtonLib.CButton
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label4 As Label
    Friend WithEvents gbUsersList As ElaCustomGroupBoxControl.ElaCustomGroupBox
    Friend WithEvents dgManageUsersUsersGrid As DataGridView
    Friend WithEvents btnManageUsersCreateUserConfirm As CButtonLib.CButton
    Friend WithEvents btnManageUsersCreateUserCancel As CButtonLib.CButton
    Friend WithEvents cmbManageUsersUserName As ElaCustomComboBoxControl.ElaCustomComboBox
    Friend WithEvents UserGridUserId As DataGridViewTextBoxColumn
    Friend WithEvents UserGridUserName As DataGridViewTextBoxColumn
    Friend WithEvents UserGridPassword As DataGridViewTextBoxColumn
    Friend WithEvents UserGridType As DataGridViewTextBoxColumn
    Friend WithEvents UserGridPasswordOrig As DataGridViewTextBoxColumn
    Friend WithEvents UserGridTypeId As DataGridViewTextBoxColumn
End Class
