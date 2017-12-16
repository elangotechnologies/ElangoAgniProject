Imports System.Data.SqlClient
Public Class DeleteConfirmation
    Public uname1, pwd1, type1 As String
    Dim a, inc As Integer
    Dim flag As Boolean = False
    Dim Con As SqlConnection
    Dim Cmd5 As SqlCommand
    Dim Sda5 As SqlDataAdapter
    Dim Ds5 As DataSet
    Dim Dt5 As DataTable
    Dim Dr5, MyRow As DataRow
    Dim Dc5 As DataColumn
    Dim Scb5 As SqlCommandBuilder
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            uname1 = TextBox1.Text
            pwd1 = TextBox2.Text
            a = Dt5.Rows.Count
            inc = 0
            flag = False
            type1 = ""
            While (a)
                Dr5 = Dt5.Rows(inc)
                If Dr5.Item(0).ToString.ToUpper.Equals(uname1.ToUpper) And Dr5.Item(1).Equals(pwd1) Then
                    'If Not uname1.Equals(Login.uname) Then
                    '    MsgBox("Sorry.. You are Logged in as '" + Login.uname + "'. So Please enter User Name as '" + Login.uname + "'")
                    '    TextBox1.Text = ""
                    '    TextBox2.Text = ""
                    '    TextBox1.Focus()
                    '    Exit Sub
                    'End If
                    'type1 = Dr5.Item(2)
                    AgniMainForm.deleteSeletectedCustomer()
                    Me.Close()
                    'If Dr5.Item(2).Equals("Others") Then
                    '    MsgBox("You are not an administrator. You cannot Delete an Customer")
                    '    Exit Sub
                    'ElseIf Dr5.Item(2).Equals("Admin") Then
                    '    
                    'End If
                    flag = True
                    Exit While
                End If
                a -= 1
                inc += 1
            End While
            If flag = False Then
                MsgBox("User Name and Password did not match." + vbNewLine + "Check whether caps lock is on or you entered correct User Name and Password." + vbNewLine + " Please Try again.", MsgBoxStyle.Critical)
                TextBox1.Text = ""
                TextBox2.Text = ""
                TextBox1.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try

    End Sub

    Private Sub VerifyingDelete_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        AgniMainForm.Enabled = True
    End Sub

    Private Sub VerifyingDelete_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Con = New SqlConnection("server=DESKTOP-EHEMD7K\ELASQLEXPRESS;Database=agnidatabase;Integrated Security=true")
            'Con = New SqlConnection("Data Source=ELAPC;Initial Catalog=agnidatabase;Integrated Security=True")
            Con.Open()
            Cmd5 = New SqlCommand("select * from login", Con)
            Sda5 = New SqlDataAdapter()
            Sda5.SelectCommand = Cmd5
            Ds5 = New DataSet
            Sda5.Fill(Ds5, "login")
            Dt5 = Ds5.Tables(0)
            TextBox1.Text = ""
            TextBox2.Text = ""
            TextBox1.Focus()
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
        AgniMainForm.Enabled = False
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button1.PerformClick()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox2.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button1.PerformClick()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub
End Class