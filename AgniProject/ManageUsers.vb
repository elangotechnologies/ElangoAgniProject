Imports System.IO

Imports System.Data.SqlClient
Imports System.Threading

Public Class ManageUsers

    Dim dbConnection As SqlConnection

    Private Sub manageuser_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dbConnection = getDBConnection()

        loadUserNameListAndGrid()

        Me.AcceptButton = btnManageUsersCreateUser

        If Login.gIsCurrentUserAdministrator Then
            setAdministratorMode(True)
        Else
            setAdministratorMode(False)
        End If

    End Sub

    Sub loadUserNameListAndGrid()
        Dim thread As Thread = New Thread(AddressOf loadUserNameListAndGridInThread)
        thread.IsBackground = True
        thread.Start()
    End Sub

    Sub loadUserNameListAndGridInThread()
        Dim usersTable As DataTable = getUsersTable()
        Dim setUserNameListInvoker As New setUserNameListDelegate(AddressOf Me.setUserNameList)
        Me.BeginInvoke(setUserNameListInvoker, usersTable)

        Dim setUserNameGridInvoker As New setUserNameGridDelegate(AddressOf Me.setUserNameGrid)
        Me.BeginInvoke(setUserNameGridInvoker, usersTable)
    End Sub

    Delegate Sub setUserNameListDelegate(usersTable As DataTable)

    Sub setUserNameList(usersTable As DataTable)
        Dim userNameListTable As DataTable = usersTable.Copy
        Dim dummyFirstRow As DataRow = userNameListTable.NewRow()
        dummyFirstRow("id") = -1
        dummyFirstRow("username") = "Please select or type an user name..."
        userNameListTable.Rows.InsertAt(dummyFirstRow, 0)

        cmbManageUsersUserName.BindingContext = New BindingContext()
        cmbManageUsersUserName.DataSource = userNameListTable
    End Sub

    Delegate Sub setUserNameGridDelegate(usersTable As DataTable)

    Sub setUserNameGrid(usersTable As DataTable)
        Dim primaryKey(0) As DataColumn
        primaryKey(0) = usersTable.Columns("id")
        usersTable.PrimaryKey = primaryKey

        dgManageUsersUsersGrid.DataSource = usersTable
        If usersTable.Rows.Count > 0 Then
            dgManageUsersUsersGrid.FirstDisplayedScrollingRowIndex = usersTable.Rows.Count - 1
        End If
    End Sub

    Private Function getUsersTable() As DataTable
        Dim userNameQuery = New SqlCommand("select u.*,ut.description as usertype, REPLICATE('*', LEN(password)) as encrPassword from Users u, UsersType ut where u.typeId=ut.typeId order by u.username", dbConnection)
        Dim userNameAdapter = New SqlDataAdapter()
        userNameAdapter.SelectCommand = userNameQuery
        Dim userNameDataSet = New DataSet
        userNameAdapter.Fill(userNameDataSet, "users")
        Return userNameDataSet.Tables(0)
    End Function

    Private Sub ManageUsers_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Login.Show()

    End Sub

    Private Sub btnManageUsersCreateUser_Click(sender As Object, e As EventArgs) Handles btnManageUsersCreateUser.ClickButtonArea
        setInUserCreationMode(True)

        cmbManageUsersUserName.Focus()
    End Sub

    Sub setInUserCreationMode(createUserMode As Boolean)
        If createUserMode Then
            Me.AcceptButton = btnManageUsersCreateUserConfirm
        End If

        txtManageUsersPassword.Visible = createUserMode
        txtManageUsersRetypePassword.Visible = createUserMode
        lblManageUsersPassword.Visible = createUserMode
        lblManageUsersRetypePassword.Visible = createUserMode
        btnManageUsersCreateUserConfirm.Visible = createUserMode
        btnManageUsersCreateUserCancel.Visible = createUserMode
        btnManageUsersCreateUser.Visible = Not createUserMode
        btnManageUsersDeleteUser.Visible = Not createUserMode
        btnManageUsersCancel.Visible = Not createUserMode
    End Sub

    Sub setAdministratorMode(adminMode As Boolean)
        btnManageUsersCreateUser.Enabled = adminMode
        btnManageUsersDeleteUser.Enabled = adminMode
    End Sub

    Private Sub btnManageUsersCreateUserCancel_Click(sender As Object, e As EventArgs) Handles btnManageUsersCreateUserCancel.ClickButtonArea
        setInUserCreationMode(False)
    End Sub

    Private Sub dgManageUsersUsersGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgManageUsersUsersGrid.CellClick
        If e.RowIndex < 0 Then
            Return
        End If

        Dim userId As Integer = dgManageUsersUsersGrid.Item("UserGridUserId", e.RowIndex).Value
        cmbManageUsersUserName.SelectedValue = userId
    End Sub

    Private Sub cmbManageUsersUserName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbManageUsersUserName.SelectedIndexChanged

        If (cmbManageUsersUserName.SelectedIndex = -1 Or cmbManageUsersUserName.SelectedValue = -1) Then
            Return
        End If

        Dim userId As Integer = cmbManageUsersUserName.SelectedValue

        Dim UsersTable As DataTable = dgManageUsersUsersGrid.DataSource
        Dim dataRow As DataRow = UsersTable.Rows.Find(userId)
        Dim userType As Integer = dataRow.Item("typeId")

        If userType = USER_TYPE_ADMINISTRATOR Then
            rbManageUsersAdmin.Checked = True
        ElseIf userType = USER_TYPE_GUEST Then
            rbManageUsersGuest.Checked = True
        End If
    End Sub

    Private Sub btnManageUsersDeleteUser_Click(sender As Object, e As EventArgs) Handles btnManageUsersDeleteUser.ClickButtonArea
        Dim userId As Integer = cmbManageUsersUserName.SelectedValue

        Dim query As String = "delete from users where id=@userid"

        Using comm As New SqlCommand()
            With comm
                .Connection = dbConnection
                .CommandType = CommandType.Text
                .CommandText = query
                .Parameters.AddWithValue("@userid", userId)
            End With
            comm.ExecuteNonQuery()
        End Using

        MessageBox.Show("User successfully deleted")

        loadUserNameListAndGrid()
        Login.loadUserNameList()
    End Sub

    Private Sub btnManageUsersCreateUserConfirm_Click(sender As Object, e As EventArgs) Handles btnManageUsersCreateUserConfirm.ClickButtonArea

        Try

            Dim password As String = txtManageUsersPassword.Text
            Dim retypePassword As String = txtManageUsersRetypePassword.Text

            If password.Trim Is String.Empty Then
                MsgBox("The password cannot be empty.")
                txtManageUsersPassword.Focus()
                Return
            End If

            If (password.Length <= 5) Then
                MsgBox("The password must contain more than 5 letters. Any trailing and leading 'space' will be removed automatically")
                txtManageUsersPassword.Focus()
                Return
            End If

            If password <> retypePassword Then
                MsgBox("The password and re-typed password must be same.")
                txtManageUsersRetypePassword.Focus()
                Return
            End If

            Dim typeId As Integer = -1
            If rbManageUsersAdmin.Checked Then
                typeId = USER_TYPE_ADMINISTRATOR
            ElseIf rbManageUsersGuest.Checked Then
                typeId = USER_TYPE_GUEST
            End If

            If typeId = -1 Then
                MsgBox("Plese select the user type")
                gbManageUsersUserType.Focus()
                Return
            End If

            Dim query As String = String.Empty
            query &= "INSERT INTO users (username,password,typeId) "
            query &= "VALUES (@username,@password,@typeId)"

            Using comm As New SqlCommand()
                With comm
                    .Connection = dbConnection
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@username", cmbManageUsersUserName.Text)
                    .Parameters.AddWithValue("@password", password)
                    .Parameters.AddWithValue("@typeId", typeId)
                End With
                comm.ExecuteNonQuery()
            End Using
            MessageBox.Show("User successfully addedd")

            loadUserNameListAndGrid()
            setInUserCreationMode(False)

            Login.loadUserNameList()

        Catch sqlEx As SqlException
            MsgBox("Username is already taken. Please try with other username.")
        Catch ex As Exception
            MessageBox.Show("Username is already taken. Exception:   " & ex.Message)
        End Try

    End Sub

    Private Sub btnManageUsersCancel_Click(sender As Object, e As EventArgs) Handles btnManageUsersCancel.ClickButtonArea
        Me.Close()
        Login.Show()
    End Sub
End Class