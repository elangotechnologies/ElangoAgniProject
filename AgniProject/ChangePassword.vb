Imports System.IO
Imports System.Data.SqlClient
Imports System.Threading

'Imports NLog

Public Class ChangePassword

    Dim dbConnection As SqlConnection
    'Dim log As Logger = LogManager.GetCurrentClassLogger()
    Public gIsCurrentUserAdministrator As Boolean = False

    Private Sub ChangePassword_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dbConnection = getDBConnection()

        loadUserNameList()

        Me.AcceptButton = btnChangePwdConfim
    End Sub

    Sub loadUserNameList()
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

        cmbChangePwdUserName.BindingContext = New BindingContext()
        cmbChangePwdUserName.DataSource = userNameTable
    End Sub

    Private Sub btnChangePwdConfim_Click(sender As Object, e As EventArgs) Handles btnChangePwdConfim.ClickButtonArea
        If cmbChangePwdUserName.SelectedValue = -1 Or cmbChangePwdUserName.SelectedIndex = -1 Then
            MsgBox("Please enter an user name")
            cmbChangePwdUserName.Focus()
            Return
        ElseIf txtChangePwdOldPassword.Text.Trim Is String.Empty Then
            MsgBox("Please enter the old password")
            txtChangePwdOldPassword.Focus()
            Return
        ElseIf txtChangePwdNewPassword.Text.Trim Is String.Empty Then
            MsgBox("Please enter the new password")
            txtChangePwdNewPassword.Focus()
            Return
        ElseIf txtChangePwdRetypeNewPassword.Text.Trim Is String.Empty Then
            MsgBox("Please re-type the new password")
            txtChangePwdRetypeNewPassword.Focus()
            Return
        End If

        Dim userName As String = cmbChangePwdUserName.Text.Trim
        Dim oldPassword As String = txtChangePwdOldPassword.Text.Trim
        Dim newPassword As String = txtChangePwdNewPassword.Text.Trim
        Dim retypeNewPassword As String = txtChangePwdRetypeNewPassword.Text.Trim

        If (newPassword.Length <= 5) Then
            MsgBox("The new password must contain more than 5 letters")
            txtChangePwdNewPassword.Focus()
            Return
        End If

        If newPassword <> retypeNewPassword Then
            MsgBox("The new password and re-typed new password must be same.")
            txtChangePwdRetypeNewPassword.Focus()
            Return
        End If

        Dim isOldPasswordValid As Boolean = verifyCredential(userName, oldPassword)
        If isOldPasswordValid = True Then
            Dim isUpdateSuccess As Boolean = updateNewPassword(userName, newPassword)
            If (isUpdateSuccess = False) Then
                MsgBox("Change password operation failed. Please try again")
            Else
                MsgBox("Password is changed successfully")
                Me.Close()
                Login.Show()
            End If
        Else
            MsgBox("Invalid user name or old password. Please check the user name and old password.")
            Return
        End If

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

    Private Function updateNewPassword(username As String, newPassword As String) As Boolean

        Try
            Dim updatePwdQuery As String = "update users set password=@newPassword where username=@username"

            Using comm As New SqlCommand()
                With comm
                    .Connection = dbConnection
                    .CommandType = CommandType.Text
                    .CommandText = updatePwdQuery
                    .Parameters.AddWithValue("@username", username)
                    .Parameters.AddWithValue("@newPassword", newPassword)
                End With
                comm.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    Private Sub ChangePassword_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Login.Show()
    End Sub

    Private Sub btnChangePwdCancel_Click(sender As Object, e As EventArgs) Handles btnChangePwdCancel.ClickButtonArea
        Me.Close()
        Login.Show()
    End Sub

End Class