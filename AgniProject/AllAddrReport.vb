Imports System.Data.SqlClient
Imports System.IO
Imports System.Math
Public Class AllAddrReport
    Dim Con As SqlConnection
    Dim cmd1 As SqlCommand
    Dim Sda1 As SqlDataAdapter
    Dim Ds1, ds8 As DataSet
    Dim Dt1, dt8 As DataTable
    Dim Dr1, dr8, dr4, dr5, dr6 As DataRow
    Dim Dc1, dc8(3) As DataColumn
    Dim Scb1 As SqlCommandBuilder
    Dim i As Int16
    Dim objrpt As New AllAddress

    Private Sub AllAddrReport_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        objrpt.Dispose()
    End Sub
    Private Sub AllAddr_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Con = New SqlConnection("server=agni\SQLEXPRESS;Database=agnidatabase;Integrated Security=true")
            Con.Open()

            cmd1 = New SqlCommand("select * from customer", Con)
            Sda1 = New SqlDataAdapter()
            Sda1.SelectCommand = cmd1
            Ds1 = New DataSet
            Sda1.Fill(Ds1, "customer")
            Dt1 = Ds1.Tables(0)

            Dim a As Int32 = Dt1.Rows.Count
            Dim key As String
            Dim inc As Int32 = 0
            Dim tempcust As New DataTable
            Dim tempdr As DataRow
            tempcust = Dt1.Clone()

            Dim listobj As ListBox = AgnimainForm.ListBox1
            If AgnimainForm.addrselected = True Then
                For i = 0 To listobj.Items.Count - 1
                    If listobj.GetSelected(i) Then
                        key = listobj.Items(i)
                        a = Dt1.Rows.Count
                        inc = 0
                        While (a)
                            Dr1 = Dt1.Rows(inc)
                            If key.Equals(Dr1.Item(1).ToString) Then
                                tempdr = tempcust.NewRow
                                tempdr.Item(0) = Dr1.Item(0)
                                tempdr.Item(1) = Dr1.Item(1)
                                tempdr.Item(2) = Dr1.Item(2)
                                tempdr.Item(3) = Dr1.Item(3)
                                tempdr.Item(4) = Dr1.Item(4)
                                tempdr.Item(5) = Dr1.Item(5)
                                tempdr.Item(6) = Dr1.Item(6)
                                tempdr.Item(7) = Dr1.Item(7)
                                tempdr.Item(8) = Dr1.Item(8)
                                tempdr.Item(9) = Dr1.Item(9)
                                tempdr.Item(10) = Dr1.Item(10)
                                tempdr.Item(11) = Dr1.Item(11)
                                tempcust.Rows.Add(tempdr)
                                Exit While
                            End If
                            a -= 1
                            inc += 1
                        End While
                    End If
                Next
                Dt1 = tempcust
            End If


            Dim three As Integer
            ds8 = New DataSet
            dt8 = New DataTable
            Dim dbaddr As String
            ds8.Tables.Add(dt8)
            dc8(0) = New DataColumn("Address1", Type.GetType("System.String"))
            dt8.Columns.Add(dc8(0))
            dc8(1) = New DataColumn("Address2", Type.GetType("System.String"))
            dt8.Columns.Add(dc8(1))
            dc8(2) = New DataColumn("Address3", Type.GetType("System.String"))
            dt8.Columns.Add(dc8(2))
            a = Dt1.Rows.Count - 1
            inc = 0
            dr8 = dt8.NewRow
            Dim rcount As Int16 = a + 1
            Dim rrow As Int16 = Fix(rcount / 3)
            Dim rrem As Int32 = rcount Mod 3
            While (a >= 0)
                Dr1 = Dt1.Rows(inc)
                dbaddr = Dr1.Item(1).ToString + vbNewLine + Dr1.Item(3).ToString + vbNewLine + "Mobile : " + Dr1.Item(4).ToString
                dr8.Item(three) = dbaddr
                a -= 1
                inc += 1
                three += 1
                If rrow <> dt8.Rows.Count Then
                    If three Mod 3 = 0 And three <> 0 Then
                        dt8.Rows.Add(dr8)
                        dr8 = dt8.NewRow
                        three = 0
                    End If
                ElseIf three = rrem Then
                    dt8.Rows.Add(dr8)
                End If
            End While


            objrpt.SetDataSource(ds8.Tables(0))
            CrystalReportViewer1.ReportSource = objrpt
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
End Class