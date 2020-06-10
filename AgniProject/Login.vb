Imports System.IO
Imports System.Data.SqlClient
Imports System.Threading

'Imports NLog

Public Class Login

    Dim dbConnection As SqlConnection
    'Dim log As Logger = LogManager.GetCurrentClassLogger()
    Public gIsCurrentUserAdministrator As Boolean = True

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dbConnection = getDBConnection()

        loadUserNameList()
        Me.AcceptButton = btnLoginLogin

    End Sub

    Public Sub loadUserNameList()
        Dim thread As Thread = New Thread(AddressOf getUserNameList)
        thread.IsBackground = True
        thread.Start()
    End Sub

    Sub getUserNameList()
        Dim userNameQuery = New SqlCommand("select id, username from users order by username", dbConnection)
        Dim userNameAdapter = New SqlDataAdapter()
        userNameAdapter.SelectCommand = userNameQuery
        Dim userNameDataSet = New DataSet
        userNameAdapter.Fill(userNameDataSet, "users")
        Dim userNameTable As DataTable = userNameDataSet.Tables(0)

        Dim setUserNameListInvoker As New setUserNameListDelegate(AddressOf Me.setUserNameList)
        Me.BeginInvoke(setUserNameListInvoker, userNameTable)
    End Sub

    Delegate Sub setUserNameListDelegate(userNameTable As DataTable)

    Sub setUserNameList(userNameTable As DataTable)
        Dim dummyFirstRow As DataRow = userNameTable.NewRow()
        dummyFirstRow("id") = -1
        dummyFirstRow("username") = "Please select or type an user name..."
        userNameTable.Rows.InsertAt(dummyFirstRow, 0)

        cmbLoginUserName.BindingContext = New BindingContext()
        cmbLoginUserName.DataSource = userNameTable
    End Sub

    Private Function verifyCredential(userName As String, password As String) As Boolean
        'log.Debug("login query: " + "select id from users where username='" + userName + "' and password='" + password + "'")

        Dim userNameQuery = New SqlCommand("select * from users where username='" + userName + "' and password='" + password + "'", dbConnection)
        Dim userNameAdapter = New SqlDataAdapter()
        userNameAdapter.SelectCommand = userNameQuery
        Dim userNameDataSet = New DataSet
        userNameAdapter.Fill(userNameDataSet, "users")
        Dim userNameTable As DataTable = userNameDataSet.Tables(0)

        'log.Debug("login found count: " + userNameTable.Rows.Count.ToString)

        If userNameTable.Rows.Count > 0 Then

            Dim dataRow = userNameTable.Rows(0)
            Dim userType As Integer = dataRow.Item("typeId")

            If USER_TYPE_ADMINISTRATOR = userType Then
                gIsCurrentUserAdministrator = True
            Else
                gIsCurrentUserAdministrator = False
            End If

            Return True
        End If

        Return False
    End Function

    Private Sub btnLoginCancel_Click(sender As Object, e As EventArgs) Handles btnLoginCancel.ClickButtonArea
        Me.Close()
    End Sub

    Private Sub btnLoginChangePassword_Click(sender As Object, e As EventArgs) Handles btnLoginChangePassword.ClickButtonArea
        ChangePassword.Show()
        Me.Hide()
    End Sub

    Sub showLoadingScreen()
        Dim thread As Thread = New Thread(AddressOf showLoadingScreenThread)
        thread.IsBackground = True
        thread.Start()
    End Sub

    Sub showLoadingScreenThread()
        Loading.gImplicitHandling = True
        Loading.Show()
    End Sub

    Private Sub btnLoginLogin_ClickButtonArea(Sender As Object, e As MouseEventArgs) Handles btnLoginLogin.ClickButtonArea

        If cmbLoginUserName.SelectedValue = -1 Or cmbLoginUserName.SelectedIndex = -1 Then
            MsgBox("Plesae enter an user name")
            cmbLoginUserName.Focus()
            Return
        ElseIf txtLoginPassword.Text.Trim Is String.Empty Then
            MsgBox("Plesae enter the password")
            txtLoginPassword.Focus()
            Return
        End If

        Dim userName As String = cmbLoginUserName.Text.Trim
        Dim password As String = txtLoginPassword.Text.Trim

        Dim isLoginSuccess As Boolean = verifyCredential(userName, password)

        If isLoginSuccess = True Then
            Loading.Show()
        Else
            MsgBox("Invalid login credentails. Please check the user name and password.")
            Return
        End If

    End Sub

    Private Sub btnLoginManageUsers_Click(sender As Object, e As EventArgs) Handles btnLoginManageUsers.ClickButtonArea

        If cmbLoginUserName.SelectedValue = -1 Or cmbLoginUserName.SelectedIndex = -1 Then
            MsgBox("Plesae enter an user name")
            cmbLoginUserName.Focus()
            Return
        ElseIf txtLoginPassword.Text.Trim Is String.Empty Then
            MsgBox("Plesae enter the password")
            txtLoginPassword.Focus()
            Return
        End If

        Dim userName As String = cmbLoginUserName.Text.Trim
        Dim password As String = txtLoginPassword.Text.Trim

        Dim isLoginSuccess As Boolean = verifyCredential(userName, password)
        If isLoginSuccess = True Then
            ManageUsers.Show()
            Me.Hide()
        Else
            MsgBox("Invalid user credentails. Please check the user name and password.")
            Return
        End If

    End Sub

    Private Sub Login_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Login_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        closeDBConnection()
    End Sub
End Class