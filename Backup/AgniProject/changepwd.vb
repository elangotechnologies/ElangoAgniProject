Imports System.Data.SqlClient
Imports System.IO
Public Class changepwd
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
    Private Sub changepwd_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            Login.Show()
            Login.ComboBox1.Focus()
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub changepwd_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Con = New SqlConnection("server=agni\SQLEXPRESS;Database=agnidatabase;Integrated Security=true")
            Con.Open()
            Cmd5 = New SqlCommand("select * from login", Con)
            Sda5 = New SqlDataAdapter()
            Sda5.SelectCommand = Cmd5
            Ds5 = New DataSet
            Sda5.Fill(Ds5, "login")
            Dt5 = Ds5.Tables(0)
            TextBox1.Text = Login.uname
            TextBox2.Focus()
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

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

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

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            Me.Close()
            Login.Show()
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            If TextBox2.Text.Trim.ToString.Equals("") Then
                MsgBox("Enter the New Password ")
                TextBox2.Focus()
                'End If
            ElseIf TextBox2.Text.Equals(TextBox3.Text) Then
                uname = TextBox1.Text
                pwd = TextBox2.Text
                a = Dt5.Rows.Count
                inc = 0
                flag = False
                While (a)
                    Dr5 = Dt5.Rows(inc)
                    If Dr5.Item(0).Equals(uname) Then
                        Dr5.BeginEdit()
                        Dr5.Item(1) = pwd
                        Dr5.EndEdit()
                        Sda5.SelectCommand = Cmd5
                        Scb5 = New SqlCommandBuilder(Sda5)
                        Sda5.UpdateCommand = Scb5.GetUpdateCommand
                        If Ds5.HasChanges Then
                            Sda5.Update(Ds5, "login")
                            MessageBox.Show(" Password successfully Changed")
                            Login.RefreshList1()
                        End If
                        Me.Close()
                        Login.Show()
                        flag = True
                        Exit While
                    End If
                    a -= 1
                    inc += 1
                End While
            Else
                MsgBox("Password mismatch.. Try Again")
                TextBox2.Text = ""
                TextBox3.Text = ""
                TextBox2.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
            Login.RefreshList1()
        End Try
    End Sub
End Class