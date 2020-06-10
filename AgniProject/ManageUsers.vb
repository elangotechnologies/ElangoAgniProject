Imports System.IO

Imports System.Data.SqlClient
Imports System.Threading

Public Class ManageUsers

    Dim dbConnection As SqlConnection
    Dim gUserType As Integer = -1
    Public gSelectedUserId As Integer = -1

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

        gSelectedUserId = -1
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
        btnManageUsersUpdateUser.Visible = Not createUserMode
        btnManageUsersCancel.Visible = Not createUserMode
    End Sub

    Sub setAdministratorMode(adminMode As Boolean)
        btnManageUsersCreateUser.Enabled = adminMode
        btnManageUsersDeleteUser.Enabled = adminMode
        btnManageUsersUpdateUser.Enabled = adminMode
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
        gSelectedUserId = userId

        Dim UsersTable As DataTable = dgManageUsersUsersGrid.DataSource
        Dim dataRow As DataRow = UsersTable.Rows.Find(userId)
        Dim userType As Integer = dataRow.Item("typeId")
        gUserType = userType

        If userType = USER_TYPE_ADMINISTRATOR Then
            rbManageUsersAdmin.Checked = True
        ElseIf userType = USER_TYPE_GUEST Then
            rbManageUsersGuest.Checked = True
        End If
    End Sub

    Private Function getAdministratorsCount() As Integer
        Dim adminCountQuery = New SqlCommand("select 1 as adminCount from Users where typeId=1", dbConnection)
        Dim adminCountAdapter = New SqlDataAdapter()
        adminCountAdapter.SelectCommand = adminCountQuery
        Dim adminCountDataSet = New DataSet
        adminCountAdapter.Fill(adminCountDataSet, "users")
        Dim adminTable As DataTable = adminCountDataSet.Tables(0)
        Return adminTable.Rows.Count
    End Function


    Private Sub btnManageUsersDeleteUser_Click(sender As Object, e As EventArgs) Handles btnManageUsersDeleteUser.ClickButtonArea

        If cmbManageUsersUserName.SelectedValue = -1 Or cmbManageUsersUserName.SelectedIndex = -1 Then
            MsgBox("Plesae enter an user name")
            cmbManageUsersUserName.Focus()
            Return
        End If

        Dim userId As Integer = gSelectedUserId

        Dim typeId As Integer = -1
        If gUserType = USER_TYPE_ADMINISTRATOR Then
            If getAdministratorsCount() <= 1 Then
                MsgBox("Sorry, You cannot delete the last administrator user. There must be atleast one administrator user to manage the users.")
                Return
            End If
        End If

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

    Private Sub btnManageUsersUpdateUser_ClickButtonArea(Sender As Object, e As MouseEventArgs) Handles btnManageUsersUpdateUser.ClickButtonArea
        If gSelectedUserId = -1 Then
            MsgBox("Plesae enter an user name")
            cmbManageUsersUserName.Focus()
            Return
        End If

        Dim userName As String = cmbManageUsersUserName.Text.Trim
        Dim userId As Integer = gSelectedUserId
        Dim typeId As Integer = -1
        If rbManageUsersAdmin.Checked Then
            typeId = USER_TYPE_ADMINISTRATOR
        ElseIf rbManageUsersGuest.Checked Then
            typeId = USER_TYPE_GUEST
        End If

        If gUserType = USER_TYPE_ADMINISTRATOR And typeId = USER_TYPE_GUEST Then
            If getAdministratorsCount() <= 1 Then
                MsgBox("Sorry, You cannot convert the last administrator user into a Guest user. There must be atleast one administrator user to manage the users.")
                Return
            End If
        End If

        Dim isUpdateSuccess As Boolean = updateUserDetails(userId, userName, typeId)

        If (isUpdateSuccess = False) Then
            MsgBox("Update user details operation failed. Please try again")
        Else
            MsgBox("User details updated successfully")
            loadUserNameListAndGrid()
            Login.loadUserNameList()
        End If
    End Sub

    Private Function updateUserDetails(userId As Integer, username As String, typeId As String) As Boolean

        Try
            Dim updateQuery As String = "update users set username=@newUsername, typeId=@newTypeId where id=@userId"

            Using comm As New SqlCommand()
                With comm
                    .Connection = dbConnection
                    .CommandType = CommandType.Text
                    .CommandText = updateQuery
                    .Parameters.AddWithValue("@newUsername", username)
                    .Parameters.AddWithValue("@newTypeId", typeId)
                    .Parameters.AddWithValue("@userId", userId)
                End With
                comm.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    Private Sub ManageUsers_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
End Class