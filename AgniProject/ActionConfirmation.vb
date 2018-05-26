Imports System.IO
Imports System.Data.SqlClient
Imports System.Threading

'Imports NLog

Public Class ActionConfirmation

    Dim dbConnection As SqlConnection
    'Dim log As Logger = LogManager.GetCurrentClassLogger()
    Public gIsCurrentUserAdministrator As Boolean = False

    Private Sub ActionConfirmation_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dbConnection = getDBConnection()

        loadUserNameList()

        Me.AcceptButton = btnActionConfirmationConfirm
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

        cmbActionConfirmUserName.BindingContext = New BindingContext()
        cmbActionConfirmUserName.DataSource = userNameTable
    End Sub

    Private Sub btnbtnActionConfirmationConfirm_Click(sender As Object, e As EventArgs) Handles btnActionConfirmationConfirm.ClickButtonArea
        If cmbActionConfirmUserName.SelectedValue = -1 Or cmbActionConfirmUserName.SelectedIndex = -1 Then
            MsgBox("Plesae enter an user name")
            cmbActionConfirmUserName.Focus()
            Return
        ElseIf txtActionConfirmPassword.Text.Trim Is String.Empty Then
            MsgBox("Plesae enter the password")
            txtActionConfirmPassword.Focus()
            Return
        End If

        Dim userName As String = cmbActionConfirmUserName.Text.Trim
        Dim password As String = txtActionConfirmPassword.Text.Trim

        Dim isLoginSuccess As Boolean = verifyCredential(userName, password)
        If isLoginSuccess = True Then
            'Already this dialog is closed in verifyCredential by setting the DialogResult
        Else
            MsgBox("Invalid login credentails. Please check the user name and password.")
        End If

    End Sub

    Private Function verifyCredential(userName As String, password As String) As Boolean

        Dim userNameQuery = New SqlCommand("select * from users where username='" + userName + "' and password='" + password + "'", dbConnection)
        Dim userNameAdapter = New SqlDataAdapter()
        userNameAdapter.SelectCommand = userNameQuery
        Dim userNameDataSet = New DataSet
        userNameAdapter.Fill(userNameDataSet, "users")
        Dim userNameTable As DataTable = userNameDataSet.Tables(0)

        If userNameTable.Rows.Count > 0 Then

            Dim dataRow = userNameTable.Rows(0)
            Dim userType As Integer = dataRow.Item("typeId")

            If USER_TYPE_ADMINISTRATOR = userType Then
                DialogResult = Windows.Forms.DialogResult.Yes
                Return True
            Else
                DialogResult = Windows.Forms.DialogResult.No
                Return True
            End If
        End If

        Return False
    End Function

    Private Sub btnActionConfirmationCancel_Click(sender As Object, e As EventArgs) Handles btnActionConfirmationCancel.ClickButtonArea
        DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

End Class