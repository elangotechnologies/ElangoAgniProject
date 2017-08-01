Imports System.Data.SqlClient
Imports System.IO
Public Class Login
    Dim Con As SqlConnection
    Dim Cmd5 As SqlCommand
    Dim Sda5 As SqlDataAdapter
    Dim Ds5 As DataSet
    Dim Dt5 As DataTable
    Dim Dr5, MyRow As DataRow
    Dim Dc5 As DataColumn
    Dim Scb5 As SqlCommandBuilder
    Public uname, pwd, type As String
    Dim a, inc As Integer
    Dim flag As Boolean = False

    Private Sub Login_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'Con = New SqlConnection("Data Source=ELAPC;Initial Catalog=agnidatabase;Integrated Security=True")
            Con = New SqlConnection("Data Source=ELAPC;Initial Catalog=agnidatabase;Integrated Security=True")
            Con.Open()
            Cmd5 = New SqlCommand("select * from login", Con)
            Sda5 = New SqlDataAdapter()
            Sda5.SelectCommand = Cmd5
            Ds5 = New DataSet
            Sda5.Fill(Ds5, "login")
            Dt5 = Ds5.Tables(0)

            a = Dt5.Rows.Count
            inc = 0
            ComboBox1.Items.Clear()
            While (a)
                Dr5 = Dt5.Rows(inc)
                ComboBox1.Items.Add(Dr5.Item(0).ToString)
                inc = inc + 1
                a = a - 1
            End While

            ComboBox1.Text = ""
            ComboBox1.Focus()

        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
    Sub RefreshList1()
        Try
            Ds5.Dispose()
            Cmd5 = New SqlCommand("select * from login", Con)
            Sda5 = New SqlDataAdapter()
            Sda5.SelectCommand = Cmd5
            Ds5 = New DataSet
            Sda5.Fill(Ds5, "login")
            Dt5 = Ds5.Tables(0)
            a = Dt5.Rows.Count
            inc = 0
            ComboBox1.Items.Clear()
            While (a)
                Dr5 = Dt5.Rows(inc)
                ComboBox1.Items.Add(Dr5.Item(0).ToString)
                inc = inc + 1
                a = a - 1
            End While

            ComboBox1.Text = ""
            ComboBox1.Focus()

        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            uname = ComboBox1.Text
            pwd = TextBox2.Text
            a = Dt5.Rows.Count
            inc = 0
            flag = False
            type = ""
            While (a)
                Dr5 = Dt5.Rows(inc)
                If Dr5.Item(0).ToString.ToUpper.Equals(uname.ToUpper) And Dr5.Item(1).Equals(pwd) Then
                    type = Dr5.Item(2)
                    Me.Hide()
                    AgnimainForm.Show()
                    AgnimainForm.tabAllTabsHolder.SelectedIndex = 0
                    AgnimainForm.btnCustClear.PerformClick()
                    flag = True
                    Exit While
                End If
                a -= 1
                inc += 1
            End While
            If flag = False Then

                MsgBox("User Name and Password did not match." + vbNewLine + "Check whether caps lock is on or you entered correct User Name and Password." + vbNewLine + " Please Try again.", MsgBoxStyle.Critical)
                ComboBox1.Text = ""
                TextBox2.Text = ""
                ComboBox1.Focus()
            End If

            AgnimainForm.Button38.Text = "Log Off " + uname
            If type.Equals("Others") Then
                AgnimainForm.btnCustAdd.Enabled = False
                AgnimainForm.btnCustDelete.Enabled = False
                AgnimainForm.btnCustUpdate.Enabled = False
                AgnimainForm.btnCustClear.Enabled = False
                AgnimainForm.btnDesAdd.Enabled = False
                AgnimainForm.btnDesDelete.Enabled = False
                AgnimainForm.btnDesUpdate.Enabled = False
                AgnimainForm.btnDesEditPrice.Enabled = False
                AgnimainForm.btnDesClear.Enabled = False
                AgnimainForm.btnBillingCreateBill.Enabled = False
                AgnimainForm.btnBillingClear.Enabled = False
                AgnimainForm.btnPaymentDelete.Enabled = False
                AgnimainForm.btnPaymentCreatePayment.Enabled = False
                AgnimainForm.btnBillingCancelBill.Enabled = False
            Else
                AgnimainForm.btnCustAdd.Enabled = True
                AgnimainForm.btnCustDelete.Enabled = True
                AgnimainForm.btnCustUpdate.Enabled = True
                AgnimainForm.btnCustClear.Enabled = True
                AgnimainForm.btnDesAdd.Enabled = True
                AgnimainForm.btnDesDelete.Enabled = True
                AgnimainForm.btnDesUpdate.Enabled = True
                AgnimainForm.btnDesEditPrice.Enabled = True
                AgnimainForm.btnDesClear.Enabled = True
                AgnimainForm.btnBillingCreateBill.Enabled = True
                AgnimainForm.btnBillingClear.Enabled = True
                AgnimainForm.btnPaymentDelete.Enabled = True
                AgnimainForm.btnPaymentCreatePayment.Enabled = True
                AgnimainForm.btnBillingCancelBill.Enabled = True
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
    Private Sub submituser()
        Try
            uname = ComboBox1.Text
            pwd = TextBox2.Text
            a = Dt5.Rows.Count
            inc = 0
            flag = False
            While (a)
                Dr5 = Dt5.Rows(inc)
                If Dr5.Item(0).ToString.ToUpper.Equals(uname.ToUpper) And Dr5.Item(1).Equals(pwd) Then
                    Me.Hide()
                    AgnimainForm.Show()
                    flag = True
                    Exit While
                End If
                a -= 1
                inc += 1
            End While
            If flag = False Then
                MsgBox("User Name and Password did not match." + vbNewLine + "Check whether caps lock is on or you entered correct User Name and Password." + vbNewLine + " Please Try again.", MsgBoxStyle.Critical)
                ComboBox1.Text = ""
                TextBox2.Text = ""
                ComboBox1.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            If e.KeyCode = Keys.Enter Then
                Button1.PerformClick()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox2.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                Button1.PerformClick()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try
            uname = ComboBox1.Text
            pwd = TextBox2.Text
            a = Dt5.Rows.Count
            inc = 0
            flag = False
            While (a)
                Dr5 = Dt5.Rows(inc)
                If Dr5.Item(0).ToString.ToUpper.Equals(uname.ToUpper) And Dr5.Item(1).Equals(pwd) Then
                    Me.Hide()
                    ComboBox1.Text = ""
                    TextBox2.Text = ""
                    changepwd.Show()
                    flag = True
                    Exit While
                End If
                a -= 1
                inc += 1
            End While
            If flag = False Then
                MsgBox("User Name and Password did not match." + vbNewLine + "Check whether caps lock is on or you entered correct User Name and Password." + vbNewLine + " Please Try again.", MsgBoxStyle.Critical)
                ComboBox1.Text = ""
                TextBox2.Text = ""
                ComboBox1.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Try
            uname = ComboBox1.Text
            pwd = TextBox2.Text
            a = Dt5.Rows.Count
            inc = 0
            flag = False
            While (a)
                Dr5 = Dt5.Rows(inc)
                If Dr5.Item(0).ToString.ToUpper.Equals(uname.ToUpper) And Dr5.Item(1).Equals(pwd) Then
                    flag = True
                    If Dr5.Item(2).Equals("Others") Then
                        MsgBox("You are not an administrator. You cannot manage user accounts")
                        ComboBox1.Focus()
                        Exit While
                    End If
                    Me.Hide()
                    ComboBox1.Text = ""
                    TextBox2.Text = ""
                    manageuser.Show()
                    Exit While
                End If
                a -= 1
                inc += 1
            End While
            If flag = False Then
                MsgBox("User Name and Password did not match." + vbNewLine + "Check whether caps lock is on or you entered correct User Name and Password." + vbNewLine + " Please Try again.", MsgBoxStyle.Critical)
                ComboBox1.Text = ""
                TextBox2.Text = ""
                ComboBox1.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Login_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        ComboBox1.Focus()
    End Sub
End Class