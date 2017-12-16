Imports System.Data.SqlClient
Imports System.IO
Public Class manageuser
    Dim Con As SqlConnection
    Dim Cmd5 As SqlCommand
    Dim Sda5 As SqlDataAdapter
    Dim Ds5 As DataSet
    Dim Dt5 As DataTable
    Dim Dr5, MyRow As DataRow
    Dim Dc5 As DataColumn
    Dim Scb5 As SqlCommandBuilder
    Dim uname, pwd As String
    Dim a, inc As Integer
    Dim flag As Boolean = False
    Private Sub createuser_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            Login.Show()
            Login.ComboBox1.Focus()
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub createuser_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Con = New SqlConnection("Data Source=ELAPC;Initial Catalog=agnidatabase;Integrated Security=True")
            Con.Open()
            Cmd5 = New SqlCommand("select * from login", Con)
            Sda5 = New SqlDataAdapter()
            Sda5.SelectCommand = Cmd5
            Ds5 = New DataSet
            Sda5.Fill(Ds5, "login")
            Dt5 = Ds5.Tables(0)
            TextBox1.Focus()
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

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
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

    Private Sub TextBox3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox3.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                Button1.PerformClick()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub RadioButton1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles RadioButton1.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                Button1.PerformClick()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
    Private Sub RadioButton2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles RadioButton2.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                Button1.PerformClick()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            If TextBox1.Text.Trim.ToString.Equals("") Then
                MsgBox("Enter the User Name")
                TextBox1.Focus()
            ElseIf TextBox2.Text.Trim.ToString.Equals("") Then
                MsgBox("Enter the Password")
                TextBox2.Focus()
            ElseIf TextBox2.Text.Equals(TextBox3.Text) Then
                uname = TextBox1.Text
                pwd = TextBox2.Text
                MyRow = Dt5.NewRow
                MyRow.Item(0) = uname
                MyRow.Item(1) = pwd
                If RadioButton1.Checked Then
                    MyRow.Item(2) = "Admin"
                Else
                    MyRow.Item(2) = "Others"
                End If
                Dt5.Rows.Add(MyRow)
                Sda5.SelectCommand = Cmd5
                Scb5 = New SqlCommandBuilder(Sda5)
                Sda5.InsertCommand = Scb5.GetInsertCommand()
            Else
                MsgBox("Password mismatch.. Try Again")
                TextBox2.Text = ""
                TextBox3.Text = ""
                TextBox2.Focus()
            End If
            If Ds5.HasChanges Then
                Sda5.Update(Ds5, "login")
                MessageBox.Show(" User successfully Created")
                Login.RefreshList1()
                If MessageBox.Show("Do you want to create another user?", "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    Me.Close()
                Else
                    TextBox1.Text = ""
                    TextBox2.Text = ""
                    TextBox3.Text = ""
                    TextBox1.Focus()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
            Login.RefreshList1()
        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try
            TextBox2.Enabled = False
            TextBox3.Enabled = False
            If TextBox1.Text.Trim.Equals("") Then
                MessageBox.Show("Please enter User Name")
                TextBox1.Focus()
            ElseIf TextBox1.Text.Trim.Equals(Login.uname) Then
                MessageBox.Show(vbTab + "You cannot delete your account yourself." + vbNewLine + vbNewLine + "If you want to delete this user you must login as another admin user")
                TextBox1.Text = ""
                TextBox1.Focus()
            Else
                uname = TextBox1.Text.ToString
                a = Dt5.Rows.Count
                inc = 0
                flag = False
                While (a)
                    Dr5 = Dt5.Rows(inc)
                    If (uname.Equals(Dr5.Item(0).ToString())) Then
                        Dr5.Delete()
                    End If
                    Sda5.SelectCommand = Cmd5
                    Scb5 = New SqlCommandBuilder(Sda5)
                    Sda5.DeleteCommand = Scb5.GetDeleteCommand()
                    If Ds5.HasChanges Then
                        Sda5.Update(Ds5, "login")
                        MessageBox.Show("User successfully deleted")
                        TextBox1.Focus()
                        flag = True
                        Exit While
                    End If
                    a -= 1
                    inc += 1
                End While
                Login.RefreshList1()
                If flag = False Then
                    MessageBox.Show("Wrong User Name")
                    TextBox1.Focus()
                End If
                If MessageBox.Show("Do you want to delete another user?", "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    Me.Close()
                Else
                    TextBox1.Text = ""
                    TextBox2.Text = ""
                    TextBox3.Text = ""
                    TextBox1.Focus()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
            Login.RefreshList1()
        End Try

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Try
            TextBox4.Visible = True
            Button5.Visible = True
            Button1.Visible = False
            Button2.Visible = False
            Button3.Visible = False
            Button4.Visible = False
            Dim unames As String = "User Type" + vbTab + vbTab + "User Name" + vbNewLine + "----------------------------------------------" + vbNewLine
            a = Dt5.Rows.Count
            inc = 0
            flag = False
            While (a)
                Dr5 = Dt5.Rows(inc)
                unames += Dr5.Item(2) + vbTab + vbTab + Dr5.Item(0) + vbNewLine
                a -= 1
                inc += 1
            End While
            TextBox4.Text = unames
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Try
            TextBox4.Visible = False
            Button5.Visible = False
            Button1.Visible = True
            Button2.Visible = True
            Button3.Visible = True
            Button4.Visible = True
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
End Class