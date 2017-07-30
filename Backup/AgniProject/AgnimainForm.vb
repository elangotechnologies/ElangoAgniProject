Imports System.Data.SqlClient
Imports System.IO
Imports System.Math

Public Class AgnimainForm
    Dim Con As SqlConnection
    Dim Cmd1, cmd2, cmd3, cmd10 As SqlCommand
    Dim Sda1, Sda2, sda3, sda10 As SqlDataAdapter
    Dim Ds1, Ds2, ds3, ds11 As DataSet
    Dim Dt1, Dt2, dt3, dt11 As DataTable
    Dim Dr1, Dr2, dr3, dr11, MyRow As DataRow
    Dim Dc1, Dc2, dc3 As DataColumn
    Dim Scb1, Scb2, scb3, scb10 As SqlCommandBuilder
    Dim rwindex As Integer = 0
    Public a, b, c, inc As Integer
    Dim CustId As Integer = 100
    Dim DesId As Integer = 10000
    Dim flag As Integer = 0
    Dim filepath As String
    Dim t12, t13, t14, t15 As Decimal
    Dim newitem As MyComboitem
    Dim mycurrentitem As MyComboitem
    Dim indx, key1 As Integer
    Public ds4 As New DataSet
    Public dt4 As New DataTable
    Public dr4 As DataRow
    Public dc4(10) As DataColumn

    Dim i As Int32
    Dim key As String
    Dim ds6 As New DataSet
    Dim dt6 As New DataTable
    Dim dc6(14), dc11(14) As DataColumn
    Dim dr6 As DataRow

    Dim ds7 As New DataSet
    Dim dt7 As New DataTable
    Dim dc7(13) As DataColumn
    Dim dr7 As DataRow

    Public ds9, ds10 As New DataSet
    Public dt9, dt10 As New DataTable
    Public dc9(13) As DataColumn
    Public dr9, dr10 As DataRow
    Public outbal, unbilled As Decimal
    Public taxDeduction, tottaxDeduction As Decimal
    Public Deduction, totDeduction As Decimal

    Dim total, actpaid As Decimal
    Public addrcount As Int16 = 5

    Public billtable As DataTable
    Public billkey As String
    Public billcust, billdatestring As String
    Public T17, T20, T21, t26, t27, t25, t31, t24 As Decimal
    Public PrevBillNo As String = "NIL"

    Public lastbillno As Int32
    Public addrselected As Boolean = False
    Public fromdatestr1, todatestr1 As String
    Public cmprfromdate, cmprtodate As Date
    Public cmprcomp As String

    Dim datebased As Boolean = False
    Dim searchboth As Boolean = False
    Dim compbased As Boolean = False
    Dim desbased As Boolean = False
    Dim billbased As Boolean = False

    Public compname As String = "Not Specified"
    Public pdfdesfolder As String = "E:"
    Public pdfdesfolder1 As String = "E:"
    Public pdfdesfolder2 As String = "E:"

    Dim desIdOpr As Integer



    Private Sub AgnimainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If MessageBox.Show("This will close the program." + vbNewLine + "Are you really want to close?", "Application Closing", System.Windows.Forms.MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                e.Cancel = True
            Else
                Login.Close()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            TabControl1.Width = Me.Width
            TabControl1.Height = Me.Height
            DateTimePicker1.Value = DateTime.Today
            ComboBox1.Focus()

            Con = New SqlConnection("server=agni\SQLEXPRESS;Database=agnidatabase;Integrated Security=true")
            Con.Open()
            Cmd1 = New SqlCommand("select * from customer", Con)
            Sda1 = New SqlDataAdapter()
            Sda1.SelectCommand = Cmd1
            Ds1 = New DataSet
            Sda1.Fill(Ds1, "customer")
            Dt1 = Ds1.Tables(0)

            cmd2 = New SqlCommand("select * from design", Con)
            Sda2 = New SqlDataAdapter()
            Sda2.SelectCommand = cmd2
            Ds2 = New DataSet
            Sda2.Fill(Ds2, "design")
            Dt2 = Ds2.Tables(0)

            cmd3 = New SqlCommand("select * from bill", Con)
            sda3 = New SqlDataAdapter()
            sda3.SelectCommand = cmd3
            ds3 = New DataSet
            sda3.Fill(ds3, "bill")
            dt3 = ds3.Tables(0)

            cmd10 = New SqlCommand("select * from payment", Con)
            sda10 = New SqlDataAdapter()
            sda10.SelectCommand = cmd10
            ds10 = New DataSet
            sda10.Fill(ds10, "payment")
            dt10 = ds10.Tables(0)

            a = Dt1.Rows.Count
            inc = 0
            ComboBox1.Items.Clear()
            ComboBox2.Items.Clear()
            ComboBox4.Items.Clear()
            ComboBox6.Items.Clear()
            ComboBox7.Items.Clear()
            ListBox1.Items.Clear()
            While (a)
                Dr1 = Dt1.Rows(inc)
                ComboBox1.Items.Add(Dr1.Item(1).ToString)
                ComboBox2.Items.Add(Dr1.Item(1).ToString)
                ComboBox4.Items.Add(Dr1.Item(1).ToString)
                ComboBox6.Items.Add(Dr1.Item(1).ToString)
                ComboBox7.Items.Add(Dr1.Item(1).ToString)
                ListBox1.Items.Add(Dr1.Item(1).ToString)
                inc = inc + 1
                a = a - 1
            End While

            DataGrid1.DataSource = Ds2.Tables(0)
            Dim gridstyle1 As New DataGridTableStyle
            gridstyle1.MappingName = "design"
            Dim colStyle(12) As DataGridTextBoxColumn
            Dim i As Integer
            For i = 0 To 12
                colStyle(i) = New DataGridTextBoxColumn
            Next
            With colStyle(0)
                .MappingName = "CompName"
                .HeaderText = "Company Name"
                .Width = 220
            End With
            With colStyle(1)
                .MappingName = "DesnID"
                .HeaderText = "Design No."
                .Width = 80
            End With
            With colStyle(2)
                .MappingName = "DesnName"
                .HeaderText = "Design Name"
                .Width = 180
            End With
            With colStyle(3)
                .MappingName = "Height"
                .HeaderText = "Height"
                .Width = 95
            End With
            With colStyle(4)
                .MappingName = "Width"
                .HeaderText = "Width"
                .Width = 95
            End With
            With colStyle(5)
                .MappingName = "Colors"
                .HeaderText = "Colours"
            End With
            With colStyle(6)
                .MappingName = "UnitCost"
                .HeaderText = "Cost/inch"
                .Alignment = HorizontalAlignment.Right
            End With
            With colStyle(9)
                .MappingName = "Price"
                .HeaderText = "Price"
                .Alignment = HorizontalAlignment.Right
                .Width = 120
            End With
            With colStyle(7)
                .MappingName = "Type"
                .HeaderText = "Type"
            End With
            With colStyle(10)
                .MappingName = "DesignDate"
                .HeaderText = "Design Date"
                .Format = "MMM dd, yyyy"
                .Width = 130
            End With
            With colStyle(11)
                .MappingName = "isPaid"
                .HeaderText = "Status"
            End With
            With colStyle(12)
                .MappingName = "ForBill"
                .HeaderText = "For Bill No."
            End With

            With gridstyle1.GridColumnStyles
                .Add(colStyle(0))
                .Add(colStyle(1))
                .Add(colStyle(2))
                .Add(colStyle(3))
                .Add(colStyle(4))
                .Add(colStyle(5))
                .Add(colStyle(6))
                .Add(colStyle(9))
                .Add(colStyle(7))
                .Add(colStyle(10))
                '.Add(colStyle(11))
                .Add(colStyle(12))
            End With
            DataGrid1.TableStyles.Add(gridstyle1)
            gridstyle1.HeaderBackColor = Color.MidnightBlue
            gridstyle1.HeaderForeColor = Color.White
            gridstyle1.GridLineColor = Color.RoyalBlue
            gridstyle1.AllowSorting = False

            DataGrid2.DataSource = ds3.Tables(0)
            Dim gridstyle2 As New DataGridTableStyle
            gridstyle2.MappingName = "bill"
            Dim colStyle1(10) As DataGridTextBoxColumn
            For i = 0 To 8
                colStyle1(i) = New DataGridTextBoxColumn
            Next
            With colStyle1(0)
                .MappingName = "CompName"
                .HeaderText = "Company Name"
                .Width = 250
            End With
            With colStyle1(8)
                .MappingName = "Year"
                .HeaderText = "Bill"
                .Width = 30
                .Alignment = HorizontalAlignment.Right
            End With
            With colStyle1(1)
                .MappingName = "BillNo"
                .HeaderText = "No. "
                .Alignment = HorizontalAlignment.Left
                .Width = 90
            End With
            With colStyle1(2)
                .MappingName = "BillDate"
                .Format = "MMM dd, yyyy"
                .HeaderText = "Bill Date"
                .Width = 130
            End With
            With colStyle1(3)
                .MappingName = "prebalance"
                .HeaderText = "Previous Balance"
                .Alignment = HorizontalAlignment.Right
                .Width = 180
            End With
            With colStyle1(4)
                .MappingName = "descost"
                .HeaderText = "Design Cost"
                .Alignment = HorizontalAlignment.Right
                .Width = 180
            End With
            With colStyle1(5)
                .MappingName = "TotAmount"
                .HeaderText = "Design + Balance"
                .Alignment = HorizontalAlignment.Right
                .Width = 200
            End With
            With colStyle1(6)
                .MappingName = "Paid"
                .HeaderText = "Paid"
                .Alignment = HorizontalAlignment.Right
                .Width = 140
            End With
            With colStyle1(7)
                .MappingName = "curBalance"
                .HeaderText = "Current Balance"
                .Alignment = HorizontalAlignment.Right
                .Width = 160
            End With
            With gridstyle2.GridColumnStyles
                .Add(colStyle1(0))
                .Add(colStyle1(8))
                .Add(colStyle1(1))
                .Add(colStyle1(2))
                .Add(colStyle1(3))
                .Add(colStyle1(4))
                .Add(colStyle1(5))
                '.Add(colStyle1(6))
                .Add(colStyle1(7))
            End With
            DataGrid2.TableStyles.Add(gridstyle2)
            gridstyle2.HeaderBackColor = Color.MidnightBlue
            gridstyle2.HeaderForeColor = Color.White
            gridstyle2.GridLineColor = Color.RoyalBlue
            gridstyle2.AllowSorting = False

            ds6.Tables.Add(dt6)
            dc6(0) = New DataColumn("CompName", Type.GetType("System.String"))
            dc6(1) = New DataColumn("DesnID", Type.GetType("System.Int32"))
            dc6(2) = New DataColumn("DesnName", Type.GetType("System.String"))
            dc6(3) = New DataColumn("Height", Type.GetType("System.Decimal"))
            dc6(4) = New DataColumn("Width", Type.GetType("System.Decimal"))
            dc6(5) = New DataColumn("Colors", Type.GetType("System.Decimal"))
            dc6(6) = New DataColumn("UnitCost", Type.GetType("System.Decimal"))
            dc6(7) = New DataColumn("Type", Type.GetType("System.String"))
            dc6(8) = New DataColumn("Image", Type.GetType("System.Byte[]"))
            dc6(9) = New DataColumn("Price", Type.GetType("System.Decimal"))
            dc6(10) = New DataColumn("DesignDate", Type.GetType("System.DateTime"))
            dc6(11) = New DataColumn("isPaid", Type.GetType("System.String"))
            dc6(12) = New DataColumn("ForBill", Type.GetType("System.Int32"))
            For i = 0 To 12
                dt6.Columns.Add(dc6(i))
            Next
            DataGrid3.DataSource = ds6.Tables(0)
            ds6.Tables(0).TableName = "design"
            Dim gridstyle3 As New DataGridTableStyle
            gridstyle3.MappingName = "design"
            Dim colStyle2(12) As DataGridTextBoxColumn
            For i = 0 To 12
                colStyle2(i) = New DataGridTextBoxColumn
            Next
            With colStyle2(0)
                .MappingName = "CompName"
                .HeaderText = "Company Name"
                .Width = 180
            End With
            With colStyle2(1)
                .MappingName = "DesnID"
                .HeaderText = "Des. ID "
                .Width = 60
            End With
            With colStyle2(2)
                .MappingName = "DesnName"
                .HeaderText = "Design Name"
                .Width = 120
            End With
            With colStyle2(3)
                .MappingName = "Height"
                .HeaderText = "Height"
                .Width = 70
            End With
            With colStyle2(4)
                .MappingName = "Width"
                .HeaderText = "Width"
                .Width = 70
            End With
            With colStyle2(5)
                .MappingName = "Colors"
                .HeaderText = "Colours"
                .Width = 70
            End With
            With colStyle2(6)
                .MappingName = "UnitCost"
                .HeaderText = "Cost/inch"
                .Alignment = HorizontalAlignment.Right
                .Width = 70
            End With
            With colStyle2(9)
                .MappingName = "Price"
                .HeaderText = "Price"
                .Alignment = HorizontalAlignment.Right
            End With
            With colStyle2(7)
                .MappingName = "Type"
                .HeaderText = "Type"
            End With
            With colStyle2(10)
                .MappingName = "DesignDate"
                .HeaderText = "Design Date"
                .Format = "MMM dd, yyyy"
                .Width = 100
            End With
            'With colStyle(11)
            '    .MappingName = "isPaid"
            '    .HeaderText = "Status"
            'End With
            With colStyle2(12)
                .MappingName = "ForBill"
                .HeaderText = "For Bill No."
            End With

            With gridstyle3.GridColumnStyles
                .Add(colStyle2(0))
                .Add(colStyle2(1))
                .Add(colStyle2(2))
                .Add(colStyle2(10))
                .Add(colStyle2(3))
                .Add(colStyle2(4))
                .Add(colStyle2(5))
                .Add(colStyle2(6))
                .Add(colStyle2(9))
                .Add(colStyle2(7))
                '.Add(colStyle(11))
                .Add(colStyle2(12))
            End With
            DataGrid3.TableStyles.Clear()
            DataGrid3.TableStyles.Add(gridstyle3)
            gridstyle3.HeaderBackColor = Color.MidnightBlue
            gridstyle3.HeaderForeColor = Color.White
            gridstyle3.GridLineColor = Color.RoyalBlue
            gridstyle3.AllowSorting = False

            ds7.Tables.Add(dt7)
            dc7(0) = New DataColumn("CompName", Type.GetType("System.String"))
            dc7(1) = New DataColumn("BillNo", Type.GetType("System.Int32"))
            dc7(2) = New DataColumn("BillDate", Type.GetType("System.DateTime"))
            dc7(3) = New DataColumn("prebalance", Type.GetType("System.Decimal"))
            dc7(4) = New DataColumn("descost", Type.GetType("System.Decimal"))
            dc7(5) = New DataColumn("TotAmount", Type.GetType("System.Decimal"))
            dc7(6) = New DataColumn("Paid", Type.GetType("System.String"))
            dc7(7) = New DataColumn("curBalance", Type.GetType("System.Decimal"))
            dc7(8) = New DataColumn("Year", Type.GetType("System.Int32"))
            For i = 0 To 8
                dt7.Columns.Add(dc7(i))
            Next

            DataGrid4.DataSource = ds7.Tables(0)
            ds7.Tables(0).TableName = "bill"
            Dim gridstyle4 As New DataGridTableStyle
            gridstyle4.MappingName = "bill"
            Dim colStyle3(9) As DataGridTextBoxColumn
            For i = 0 To 8
                colStyle3(i) = New DataGridTextBoxColumn
            Next
            With colStyle3(0)
                .MappingName = "CompName"
                .HeaderText = "Company Name"
                .Width = 250
            End With
            With colStyle3(8)
                .MappingName = "Year"
                .HeaderText = "Bill"
                .Width = 30
                .Alignment = HorizontalAlignment.Right
            End With
            With colStyle3(1)
                .MappingName = "BillNo"
                .HeaderText = " No."
                .Width = 100
            End With
            With colStyle3(2)
                .MappingName = "BillDate"
                .Format = "MMM dd, yyyy"
                .HeaderText = "Bill Date"
                .Width = 150
            End With
            With colStyle3(3)
                .MappingName = "prebalance"
                .HeaderText = "Previous Balance"
                .Alignment = HorizontalAlignment.Right
                .Width = 180
            End With
            With colStyle3(4)
                .MappingName = "descost"
                .HeaderText = "Design Cost"
                .Alignment = HorizontalAlignment.Right
                .Width = 160
            End With
            With colStyle3(5)
                .MappingName = "TotAmount"
                .HeaderText = "Design + Balance"
                .Alignment = HorizontalAlignment.Right
                .Width = 190
            End With
            With colStyle3(6)
                .MappingName = "Paid"
                .HeaderText = "Paid"
                .Alignment = HorizontalAlignment.Right
                .Width = 150
            End With
            With colStyle3(7)
                .MappingName = "curBalance"
                .HeaderText = "Current Balance"
                .Alignment = HorizontalAlignment.Right
                .Width = 160
            End With
            With gridstyle4.GridColumnStyles
                .Add(colStyle3(0))
                .Add(colStyle3(8))
                .Add(colStyle3(1))
                .Add(colStyle3(2))
                .Add(colStyle3(3))
                .Add(colStyle3(4))
                .Add(colStyle3(5))
                '.Add(colStyle3(6))
                .Add(colStyle3(7))
            End With
            DataGrid4.TableStyles.Clear()
            DataGrid4.TableStyles.Add(gridstyle4)
            gridstyle4.HeaderBackColor = Color.MidnightBlue
            gridstyle4.HeaderForeColor = Color.White
            gridstyle4.GridLineColor = Color.RoyalBlue
            gridstyle4.AllowSorting = False

            dt11 = New DataTable
            ds11 = New DataSet
            ds11.Tables.Add(dt11)
            dc11(0) = New DataColumn("PayNo", Type.GetType("System.Int32"))
            dc11(1) = New DataColumn("CompName", Type.GetType("System.String"))
            dc11(2) = New DataColumn("BillNo", Type.GetType("System.String"))
            dc11(3) = New DataColumn("Balance", Type.GetType("System.Decimal"))
            dc11(4) = New DataColumn("Date", Type.GetType("System.DateTime"))
            dc11(5) = New DataColumn("PayMode", Type.GetType("System.String"))
            dc11(6) = New DataColumn("Paid", Type.GetType("System.Decimal"))
            dc11(7) = New DataColumn("Deduct", Type.GetType("System.Decimal"))
            dc11(8) = New DataColumn("TaxDeduct", Type.GetType("System.Decimal"))
            dc11(9) = New DataColumn("Cheque", Type.GetType("System.String"))
            dc11(10) = New DataColumn("Bank", Type.GetType("System.String"))
            dc11(11) = New DataColumn("ChequeDate", Type.GetType("System.DateTime"))
            dc11(12) = New DataColumn("Detail", Type.GetType("System.String"))
            dc11(13) = New DataColumn("TotPaid", Type.GetType("System.Decimal"))
            For i = 0 To 13
                dt11.Columns.Add(dc11(i))
            Next
            DataGrid5.DataSource = ds11.Tables(0)
            ds11.Tables(0).TableName = "payment"
            Dim gridstyle11 As New DataGridTableStyle
            gridstyle11.MappingName = "payment"
            Dim colstyle11(15) As DataGridTextBoxColumn
            For i = 0 To 13
                colstyle11(i) = New DataGridTextBoxColumn
            Next
            With colstyle11(1)
                .MappingName = "CompName"
                .HeaderText = "Company Name"
                .Width = 100
            End With
            With colstyle11(2)
                .MappingName = "BillNo"
                .HeaderText = "Bill No."
                .Width = 60
            End With
            With colstyle11(3)
                .MappingName = "Balance"
                .HeaderText = "Balance"
                .Alignment = HorizontalAlignment.Right
                .Width = 80
            End With
            With colstyle11(4)
                .MappingName = "Date"
                .HeaderText = "Paid Date"
                .Format = "MMM dd, yyyy"
                .Width = 100
            End With
            With colstyle11(5)
                .MappingName = "PayMode"
                .HeaderText = "Payment Mode"
                .Width = 70
            End With
            With colstyle11(6)
                .MappingName = "Paid"
                .HeaderText = "Amount Paid"
                .Alignment = HorizontalAlignment.Right
                .Width = 80
            End With
            With colstyle11(7)
                .MappingName = "Deduct"
                .HeaderText = "Deduction"
                .Alignment = HorizontalAlignment.Right
                .Width = 80
            End With
            With colstyle11(8)
                .MappingName = "TaxDeduct"
                .HeaderText = "Tax Deduct."
                .Alignment = HorizontalAlignment.Right
                .Width = 80
            End With
            With colstyle11(9)
                .MappingName = "Cheque"
                .HeaderText = "Cheque Number"
                .Width = 130
            End With
            With colstyle11(10)
                .MappingName = "Bank"
                .HeaderText = "Bank Name"
                .Width = 130
            End With
            With colstyle11(11)
                .MappingName = "ChequeDate"
                .HeaderText = "Cheque Date"
                .Format = "MMM dd, yyyy"
                .Width = 100
            End With
            With colstyle11(12)
                .MappingName = "Detail"
                .HeaderText = "Detail"
                .Width = 130
            End With
            With colstyle11(13)
                .MappingName = "TotPaid"
                .HeaderText = "Total Paid"
                .Alignment = HorizontalAlignment.Right
                .Width = 80
            End With
            With gridstyle11.GridColumnStyles
                .Add(colstyle11(0))
                .Add(colstyle11(1))
                .Add(colstyle11(2))
                .Add(colstyle11(3))
                .Add(colstyle11(4))
                .Add(colstyle11(5))
                .Add(colstyle11(6))
                .Add(colstyle11(7))
                .Add(colstyle11(8))
                .Add(colstyle11(13))
                .Add(colstyle11(9))
                .Add(colstyle11(10))
                .Add(colstyle11(11))
                .Add(colstyle11(12))
            End With
            DataGrid5.TableStyles.Clear()
            DataGrid5.TableStyles.Add(gridstyle11)
            gridstyle11.HeaderBackColor = Color.MidnightBlue
            gridstyle11.HeaderForeColor = Color.White
            gridstyle11.GridLineColor = Color.RoyalBlue
            gridstyle11.AllowSorting = False


            ds9.Tables.Add(dt9)
            dc9(0) = New DataColumn("CompName", Type.GetType("System.String"))
            dc9(1) = New DataColumn("BillNo", Type.GetType("System.Int32"))
            dc9(2) = New DataColumn("BillDate", Type.GetType("System.DateTime"))
            dc9(3) = New DataColumn("prebalance", Type.GetType("System.Decimal"))
            dc9(4) = New DataColumn("Deduction", Type.GetType("System.Decimal"))
            dc9(5) = New DataColumn("TaxDeduction", Type.GetType("System.Decimal"))
            dc9(6) = New DataColumn("curBalance", Type.GetType("System.Decimal"))
            dc9(7) = New DataColumn("TotUnPaid", Type.GetType("System.Decimal"))
            For i = 0 To 7
                dt9.Columns.Add(dc9(i))
            Next
            'With Me.TabControl1
            '    '.Alignment = TabAlignment.Right
            '    .Padding = New Point(20, 5)
            '    '.Margin = New Padding(100)
            '    .Appearance = TabAppearance.FlatButtons
            '    .Multiline = True
            '    .DrawMode = TabDrawMode.OwnerDrawFixed
            'End With

        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
        RadioButton7.Checked = True

    End Sub
    Private Sub AgnimainForm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        'Button38.Text += " " + Login.type
        'If Login.type.Equals("Others") Then
        '    Button1.Enabled = False
        '    Button2.Enabled = False
        '    Button3.Enabled = False
        '    Button32.Enabled = False
        'End If

    End Sub
    Sub RefreshList1()
        Try
            Ds1.Dispose()
            Ds1 = New DataSet
            Sda1.SelectCommand = Cmd1
            Sda1.Fill(Ds1, "customer")
            Dt1 = Ds1.Tables(0)
            a = Dt1.Rows.Count
            inc = 0
            ComboBox1.Items.Clear()
            ComboBox2.Items.Clear()
            ComboBox4.Items.Clear()
            ComboBox6.Items.Clear()
            ComboBox7.Items.Clear()
            ListBox1.Items.Clear()
            While (a)
                Dr1 = Dt1.Rows(inc)
                ComboBox1.Items.Add(Dr1.Item(1).ToString)
                ComboBox2.Items.Add(Dr1.Item(1).ToString)
                ComboBox4.Items.Add(Dr1.Item(1).ToString)
                ComboBox6.Items.Add(Dr1.Item(1).ToString)
                ComboBox7.Items.Add(Dr1.Item(1).ToString)
                ListBox1.Items.Add(Dr1.Item(1).ToString)
                inc = inc + 1
                a = a - 1
            End While
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Sub RefreshList2()
        Try
            Ds2.Dispose()
            Ds2 = New DataSet
            Sda2.SelectCommand = cmd2
            Sda2.Fill(Ds2, "design")
            Dt2 = Ds2.Tables(0)
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
    Sub RefreshList4()
        Try
            cmd10 = New SqlCommand("select * from payment", Con)
            sda10 = New SqlDataAdapter()
            sda10.SelectCommand = cmd10
            ds10 = New DataSet
            sda10.Fill(ds10, "payment")
            dt10 = ds10.Tables(0)
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
    Sub RefreshList3()
        Try
            ds3.Dispose()
            ds3 = New DataSet
            sda3.SelectCommand = cmd3
            sda3.Fill(ds3, "bill")
            dt3 = ds3.Tables(0)
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
    Sub refreshgrid1()
        Try
            RefreshList2()
            DataGrid1.DataSource = Ds2.Tables(0)
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
    Sub refreshgrid2()
        Try
            RefreshList3()
            DataGrid2.DataSource = ds3.Tables(0)
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
    Sub refreshgrid3()
        Try
            RefreshList4()
            DataGrid5.DataSource = ds10.Tables(0)
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub ComboBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboBox1.KeyDown
        Try
            'If (e.KeyCode = Keys.Enter) Then
            '    Button1.PerformClick()
            'Else
            If e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Try
            Dim key As String
            key = ComboBox1.Text.Trim
            a = Dt1.Rows.Count
            inc = 0
            While (a)
                Dr1 = Dt1.Rows(inc)
                If (key.Equals(Dr1.Item(1).ToString())) Then
                    TextBox1.Text = Dr1.Item(2)
                    TextBox2.Text = Dr1.Item(3)
                    TextBox3.Text = Dr1.Item(4)
                    TextBox4.Text = Dr1.Item(5).ToString
                    TextBox5.Text = Dr1.Item(6).ToString
                    TextBox6.Text = Dr1.Item(7).ToString
                    TextBox7.Text = Dr1.Item(8).ToString
                    TextBox8.Text = Dr1.Item(9).ToString
                    TextBox9.Text = Dr1.Item(10).ToString
                    TextBox10.Text = Dr1.Item(11).ToString
                    TextBox11.Text = Dr1.Item(0).ToString
                    Exit While
                End If
                inc = inc + 1
                a = a - 1
            End While
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Try
            Dr1 = Dt1.Rows(0)
            TextBox1.Text = Dr1.Item(2)
            TextBox2.Text = Dr1.Item(3)
            TextBox3.Text = Dr1.Item(4)
            TextBox4.Text = Dr1.Item(5).ToString
            TextBox5.Text = Dr1.Item(6).ToString
            TextBox6.Text = Dr1.Item(7).ToString
            TextBox7.Text = Dr1.Item(8).ToString
            TextBox8.Text = Dr1.Item(9).ToString
            TextBox9.Text = Dr1.Item(10).ToString
            TextBox10.Text = Dr1.Item(11).ToString
            TextBox11.Text = Dr1.Item(0).ToString
            ComboBox1.Text = Dr1.Item(1)
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        Try
            Dr1 = Dt1.Rows(Dt1.Rows.Count - 1)
            TextBox1.Text = Dr1.Item(2)
            TextBox2.Text = Dr1.Item(3)
            TextBox3.Text = Dr1.Item(4)
            TextBox4.Text = Dr1.Item(5).ToString
            TextBox5.Text = Dr1.Item(6).ToString
            TextBox6.Text = Dr1.Item(7).ToString
            TextBox7.Text = Dr1.Item(8).ToString
            TextBox8.Text = Dr1.Item(9).ToString
            TextBox9.Text = Dr1.Item(10).ToString
            TextBox10.Text = Dr1.Item(11).ToString
            TextBox11.Text = Dr1.Item(0).ToString
            ComboBox1.Text = Dr1.Item(1)
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Try
            If ComboBox1.Text.Trim.Equals("") Then
                MessageBox.Show("select Company Name")
            Else

                rwindex = Dt1.Rows.IndexOf(Dr1)
                rwindex += 1
                Dr1 = Dt1.Rows(rwindex)
                TextBox1.Text = Dr1.Item(2)
                TextBox2.Text = Dr1.Item(3)
                TextBox3.Text = Dr1.Item(4)
                TextBox4.Text = Dr1.Item(5).ToString
                TextBox5.Text = Dr1.Item(6).ToString
                TextBox6.Text = Dr1.Item(7).ToString
                TextBox7.Text = Dr1.Item(8).ToString
                TextBox8.Text = Dr1.Item(9).ToString
                TextBox9.Text = Dr1.Item(10).ToString
                TextBox10.Text = Dr1.Item(11).ToString
                TextBox11.Text = Dr1.Item(0).ToString
                ComboBox1.Text = Dr1.Item(1)
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User: This is the last record (or) " & ex.Message)
        End Try
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Try
            If ComboBox1.Text.Trim.Equals("") Then
                MessageBox.Show("select Company Name")
            Else
                rwindex = Dt1.Rows.IndexOf(Dr1)
                rwindex -= 1
                Dr1 = Dt1.Rows(rwindex)
                TextBox1.Text = Dr1.Item(2)
                TextBox2.Text = Dr1.Item(3)
                TextBox3.Text = Dr1.Item(4)
                TextBox4.Text = Dr1.Item(5).ToString
                TextBox5.Text = Dr1.Item(6).ToString
                TextBox6.Text = Dr1.Item(7).ToString
                TextBox7.Text = Dr1.Item(8).ToString
                TextBox8.Text = Dr1.Item(9).ToString
                TextBox9.Text = Dr1.Item(10).ToString
                TextBox10.Text = Dr1.Item(11).ToString
                TextBox11.Text = Dr1.Item(0).ToString
                ComboBox1.Text = Dr1.Item(1)
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User: This is the first record (or) " & ex.Message)
        End Try
    End Sub

    Public Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            If ComboBox1.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Valid company Name")
                ComboBox1.Focus()
            ElseIf TextBox1.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Proprietor Name")
                TextBox1.Focus()
            ElseIf TextBox2.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Valid Address")
                TextBox2.Focus()
            ElseIf TextBox3.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Mobile Number")
                TextBox3.Focus()
            Else
                MyRow = Dt1.NewRow()
                MyRow(2) = TextBox1.Text.Trim
                MyRow(3) = TextBox2.Text.Trim
                MyRow(4) = TextBox3.Text.Trim
                MyRow(5) = TextBox4.Text.Trim
                MyRow(6) = TextBox5.Text.Trim
                MyRow(7) = TextBox6.Text.Trim
                MyRow(8) = TextBox7.Text.Trim
                MyRow(9) = TextBox8.Text.Trim
                MyRow(10) = TextBox9.Text.Trim
                MyRow(11) = TextBox10.Text.Trim
                MyRow(1) = ComboBox1.Text.Trim
                MyRow(0) = TextBox11.Text.Trim
                Dt1.Rows.Add(MyRow)
                Sda1.SelectCommand = Cmd1
                Scb1 = New SqlCommandBuilder(Sda1)
                Sda1.InsertCommand = Scb1.GetInsertCommand()
                If Ds1.HasChanges Then
                    Sda1.Update(Ds1, "customer")
                    MessageBox.Show("Company successfully added")
                    TextBox1.Text = ""
                    TextBox2.Text = ""
                    TextBox3.Text = ""
                    TextBox4.Text = ""
                    TextBox5.Text = ""
                    TextBox6.Text = ""
                    TextBox7.Text = ""
                    TextBox8.Text = ""
                    TextBox9.Text = ""
                    TextBox10.Text = ""
                    ComboBox1.Text = ""
                    TextBox11.Text = ""
                    ComboBox1.Focus()
                    RefreshList1()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message & " (Or) This Customer Record may already exist")
            RefreshList1()
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            flag = 0
            If ComboBox1.Text.Trim.Equals("") Then
                MessageBox.Show("select Company Name")
                ComboBox1.Focus()
            ElseIf MessageBox.Show("All Designs, Bills and Payments will be deleted belongs to this customer." + vbNewLine + vbNewLine + "  Do you want to delete this Customer - " & ComboBox1.Text & " ?", "WARNING", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                VerifyingDelete.Button1.Text = "Delete "
                VerifyingDelete.Button1.Text += ComboBox1.Text
                VerifyingDelete.Show()
                'deleteCompanyEntire()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message & " (Or) This Customer Record may not exist")
            refreshgrid1()
            refreshgrid2()
            refreshgrid3()
             RefreshList1()
        End Try
    End Sub
    Public Sub DeleteCompanyEntire()
        Dim key As String
        Dim a1, a2 As Int32
        key = ComboBox1.Text
        a1 = Dt2.Rows.Count
        inc = 0
        While (a1)
            Dr2 = Dt2.Rows(inc)
            If (key.Equals(Dr2.Item(0).ToString())) Then
                Dr2.Delete()
                Sda2.SelectCommand = cmd2
                Scb2 = New SqlCommandBuilder(Sda2)
                Sda2.DeleteCommand = Scb2.GetDeleteCommand()
            End If
            inc = inc + 1
            a1 = a1 - 1
        End While

        If Ds2.HasChanges Then
            Sda2.Update(Ds2, "design")
            refreshgrid1()
        End If

        a2 = dt3.Rows.Count
        inc = 0
        While (a2)
            dr3 = dt3.Rows(inc)
            If (key.Equals(dr3.Item(0).ToString())) Then
                dr3.Delete()
                sda3.SelectCommand = cmd3
                scb3 = New SqlCommandBuilder(sda3)
                sda3.DeleteCommand = scb3.GetDeleteCommand()
            End If
            inc = inc + 1
            a2 = a2 - 1
        End While

        If ds3.HasChanges Then
            sda3.Update(ds3, "bill")
            refreshgrid2()
        End If
        Dim a3 As Integer
        a3 = dt10.Rows.Count
        inc = 0
        While (a3)
            dr10 = dt10.Rows(inc)
            If (key.Equals(dr10.Item(1).ToString())) Then
                dr10.Delete()
                sda10.SelectCommand = cmd10
                scb10 = New SqlCommandBuilder(sda10)
                sda10.DeleteCommand = scb10.GetDeleteCommand()
            End If
            inc = inc + 1
            a3 = a3 - 1
        End While

        If ds10.HasChanges Then
            sda10.Update(ds10, "payment")
            refreshgrid3()
        End If

        a = Dt1.Rows.Count
        inc = 0
        While (a)
            Dr1 = Dt1.Rows(inc)
            If (key.Equals(Dr1.Item(1).ToString())) Then
                Dr1.Delete()
                Sda1.SelectCommand = Cmd1
                Scb1 = New SqlCommandBuilder(Sda1)
                Sda1.DeleteCommand = Scb1.GetDeleteCommand()
            End If
            If Ds1.HasChanges Then
                Sda1.Update(Ds1, "customer")
                MessageBox.Show(" Company successfully deleted")
                RefreshList1()
                TextBox1.Text = ""
                TextBox2.Text = ""
                TextBox3.Text = ""
                TextBox4.Text = ""
                TextBox5.Text = ""
                TextBox6.Text = ""
                TextBox7.Text = ""
                TextBox8.Text = ""
                TextBox9.Text = ""
                TextBox10.Text = ""
                TextBox11.Text = ""
                ComboBox1.Text = ""
                flag = 1
                Exit While
            End If
            inc = inc + 1
            a = a - 1
        End While
        If flag = 0 Then
            MessageBox.Show(" Cannot Delete.. There is no such company Record")
        End If
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try
            flag = 0
            If ComboBox1.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Valid company Name")
                ComboBox1.Focus()
            ElseIf TextBox1.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Proprietor Name")
                TextBox1.Focus()
            ElseIf TextBox2.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Valid Address")
                TextBox2.Focus()
            ElseIf TextBox3.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Mobile Number")
                TextBox3.Focus()
            Else
                Dim key As String
                key = ComboBox1.Text.Trim
                a = Dt1.Rows.Count
                inc = 0
                While (a)
                    Dr1 = Dt1.Rows(inc)
                    If (key.Equals(Dr1.Item(1).ToString())) Then
                        Dr1.BeginEdit()
                        Dr1.Item(2) = TextBox1.Text.Trim
                        Dr1.Item(3) = TextBox2.Text.Trim
                        Dr1.Item(4) = TextBox3.Text.Trim
                        Dr1.Item(5) = TextBox4.Text.Trim
                        Dr1.Item(6) = TextBox5.Text.Trim
                        Dr1.Item(7) = TextBox6.Text.Trim
                        Dr1.Item(8) = TextBox7.Text.Trim
                        Dr1.Item(9) = TextBox8.Text.Trim
                        Dr1.Item(10) = TextBox9.Text.Trim
                        Dr1.Item(11) = TextBox10.Text.Trim
                        Dr1.Item(0) = TextBox11.Text.Trim
                        Dr1.EndEdit()
                        Sda1.SelectCommand = Cmd1
                        Scb1 = New SqlCommandBuilder(Sda1)
                        Sda1.UpdateCommand = Scb1.GetUpdateCommand
                        flag = 1
                        Exit While
                    End If
                    inc = inc + 1
                    a = a - 1
                End While
                If flag = 0 Then
                    MessageBox.Show(" Cannot Update.. There is no such company Record")
                    ComboBox1.Focus()
                End If
                If Ds1.HasChanges Then
                    Sda1.Update(Ds1, "customer")
                    MessageBox.Show(" Company successfully updated")
                    ComboBox1.Focus()
                    RefreshList1()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message & " Or this Customer Record may not exist")
            RefreshList1()
        End Try
    End Sub


    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Try
            If ComboBox3.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Valid Design Name")
                ComboBox3.Focus()
            ElseIf DateTimePicker2.Text.Trim.Equals("") Then
                MessageBox.Show("Choose Valid date")
                DateTimePicker2.Focus()
            ElseIf ComboBox2.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Valid Company Name")
                ComboBox2.Focus()
                'ElseIf TextBox12.Text.Trim.Equals("") Then
                '    MessageBox.Show("Enter Valid width")
                'ElseIf TextBox13.Text.Trim.Equals("") Then
                '    MessageBox.Show("Enter Valid Height")
            ElseIf TextBox14.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Valid No.of colors")
                TextBox14.Focus()
            ElseIf TextBox15.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Valid Unit cost")
                TextBox15.Focus()
            ElseIf TextBox16.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Valid Total cost")
                TextBox16.Focus()
                'ElseIf PictureBox2.Image Is Nothing Then
                '    MessageBox.Show("select the design picture")
            Else
                Dim ms As New MemoryStream()
                Dim arrImage() As Byte
                If PictureBox2.Image Is Nothing Then
                    arrImage = Nothing
                Else
                    'PictureBox2.Image.Save(ms, PictureBox2.Image.RawFormat)
                    PictureBox2.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
                    arrImage = ms.GetBuffer
                End If
                ms.Close()
                MyRow = Dt2.NewRow()
                MyRow(0) = ComboBox2.Text.Trim
                MyRow(2) = ComboBox3.Text.Trim
                If Val(TextBox12.Text) = 0 Then
                    t12 = 0
                Else
                    t12 = Decimal.Parse(TextBox12.Text.Trim)
                End If
                MyRow(3) = t12
                If Val(TextBox13.Text) = 0 Then
                    t13 = 0
                Else
                    t13 = Decimal.Parse(TextBox13.Text.Trim)
                End If
                MyRow(4) = t13
                MyRow(5) = Decimal.Parse(TextBox14.Text.Trim)
                MyRow(6) = Decimal.Parse(TextBox15.Text.Trim)
                MyRow(10) = DateTimePicker2.Value
                MyRow(11) = "notpaid"
                If RadioButton1.Checked Then
                    MyRow(7) = "Design"
                Else
                    MyRow(7) = "Print"
                End If
                MyRow(8) = arrImage
                MyRow(9) = Decimal.Parse(TextBox16.Text.Trim)
                Dt2.Rows.Add(MyRow)
                Scb2 = New SqlCommandBuilder(Sda2)
                Sda2.InsertCommand = Scb2.GetInsertCommand()
                If Ds2.HasChanges Then
                    Sda2.Update(Ds2, "design")
                    MessageBox.Show("Design successfully added")
                    ComboBox3.Text = ""
                    TextBox12.Text = ""
                    TextBox13.Text = ""
                    TextBox14.Text = ""
                    TextBox16.Text = ""
                    RadioButton1.Checked = True
                    PictureBox2.Image = Nothing
                    'DateTimePicker2.Text = ""
                    ComboBox3.Focus()
                End If
            End If
            refreshgrid1()
            Dim key As String
            key = ComboBox2.Text.Trim
            a = Dt2.Rows.Count
            inc = 0
            ComboBox3.Items.Clear()
            While (a)
                Dr2 = Dt2.Rows(inc)
                If (key.Equals(Dr2.Item(0).ToString())) Then
                    newitem = New MyComboitem(Dr2.Item(1), Dr2.Item(2).ToString)
                    ComboBox3.Items.Add(newitem)
                End If
                inc = inc + 1
                a = a - 1
            End While
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message & " Or this design Record may already exist")
            refreshgrid1()
        End Try

    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        Try
            If ComboBox2.Text.Trim.Equals("") Then
                MessageBox.Show("select Company Name")
                ComboBox2.Focus()
            Else
                Dim key As String
                key = ComboBox2.Text
                a = Dt2.Rows.Count
                inc = 0
                ComboBox3.Items.Clear()
                While (a)
                    Dr2 = Dt2.Rows(inc)
                    If (key.Equals(Dr2.Item(0).ToString())) Then
                        newitem = New MyComboitem(Dr2.Item(1), Dr2.Item(2).ToString)
                        ComboBox3.Items.Add(newitem)
                    End If
                    inc = inc + 1
                    a = a - 1
                End While
                If ComboBox3.Items.Count = 0 Then
                    MsgBox("Message to Agni User: There is no design for " + key)
                    ComboBox3.Focus()
                Else
                    ComboBox3.SelectedIndex = 0
                End If

            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox15_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox15.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button8.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub


    Private Sub TextBox15_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox15.TextChanged
        Try
            If Val(TextBox12.Text) = 0 Then
                t12 = 1
            Else
                t12 = Decimal.Parse(TextBox12.Text)

            End If
            If Val(TextBox13.Text) = 0 Then
                t13 = 1
            Else
                t13 = Decimal.Parse(TextBox13.Text)
            End If
            If Val(TextBox14.Text) = 0 Then
                t14 = 1
            Else
                t14 = Decimal.Parse(TextBox14.Text)
            End If
            If Val(TextBox15.Text) = 0 Then
                t15 = 1
            Else
                t15 = Decimal.Parse(TextBox15.Text)
            End If
            TextBox16.Text = Round(t12 * t13 * t14 * t15)
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click
        pictureload()
    End Sub
    Private Sub pictureload()
        Try
            'Dim file As New OpenFileDialog
            'With file
            '    '.InitialDirectory = "E:\Bill Images"
            '    .Filter = "All Files|*.*|Bitmaps|*.bmp|GIFs|*.gif|JPEGs|*.jpg"
            '    .FilterIndex = 7
            'End With
            'If file.ShowDialog() = Windows.Forms.DialogResult.OK Then
            '    With PictureBox2
            '        .Image = Image.FromFile(file.FileName)
            '    End With
            'End If
            Dim file As New OpenFileDialog
            With file
                .Filter = "All Files|*.*|Bitmaps|*.bmp|GIFs|*.gif|JPEGs|*.jpg"
                .FilterIndex = 7
            End With
            If file.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Dim Data() As Byte
                Dim fs As New FileStream(Trim(file.FileName), FileMode.Open)
                Data = New [Byte](fs.Length) {}
                fs.Read(Data, 0, fs.Length)
                Dim memorybits As New MemoryStream(Data)
                Dim img As System.Drawing.Image
                img = System.Drawing.Image.FromStream(memorybits)
                Dim inp As New IntPtr
                Dim thumb As System.Drawing.Image
                thumb = img.GetThumbnailImage(PictureBox2.Width, PictureBox2.Height, Nothing, inp)
                PictureBox2.Image = thumb
                With PictureBox2
                    .Image = thumb
                End With
                fs.Close()
            End If
            filepath = file.FileName.ToString
            Dim desname As String
            Dim strFilename As String = filepath.Substring(filepath.LastIndexOf("\") + 1)
            If strFilename.Equals("") Then
            Else
                desname = strFilename.Substring(0, strFilename.LastIndexOf("."))
                ComboBox3.Text = ""
                ComboBox3.SelectedText = desname
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try

    End Sub
    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        'Try
        flag = 0
        If ComboBox3.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Design Name")
            ComboBox3.Focus()
        ElseIf DateTimePicker2.Text.Trim.Equals("") Then
            MessageBox.Show("Choose Valid date")
            DateTimePicker2.Focus()
        ElseIf ComboBox2.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Company Name")
            ComboBox2.Focus()
            'ElseIf TextBox12.Text.Trim.Equals("") Then
            '    MessageBox.Show("Enter Valid width")
            'ElseIf TextBox13.Text.Trim.Equals("") Then
            '    MessageBox.Show("Enter Valid Height")
        ElseIf TextBox14.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid No.of colors")
            TextBox14.Focus()
        ElseIf TextBox15.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Unit cost")
            TextBox15.Focus()
        ElseIf TextBox16.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Total cost")
            TextBox16.Focus()
            'ElseIf PictureBox2.Image Is Nothing Then
            '    MessageBox.Show("select the design picture")
        Else
            Dim ms As New MemoryStream()
            Dim arrImage() As Byte
            If PictureBox2.Image Is Nothing Then
                arrImage = Nothing
            Else
                PictureBox2.Image.Save(ms, PictureBox2.Image.RawFormat)
                arrImage = ms.GetBuffer
            End If

            ms.Close()
            Dim lastprice As Decimal
            Dim key As Integer
            'Dim item As MyComboitem
            'item = DirectCast(ComboBox3.SelectedItem, MyComboitem)
            key = desIdOpr
            b = Dt2.Rows.Count
            inc = 0
            While (b)
                Dr2 = Dt2.Rows(inc)
                If key = Dr2.Item(1) And Dr2.Item(11).ToString.Equals("notpaid") Then
                    Dr2.BeginEdit()
                    Dr2.Item(0) = ComboBox2.Text.Trim
                    Dr2.Item(2) = ComboBox3.Text.Trim
                    If Val(TextBox12.Text) = 0 Then
                        t12 = 0
                    Else
                        t12 = Decimal.Parse(TextBox12.Text.Trim)
                    End If
                    Dr2.Item(3) = t12
                    If Val(TextBox13.Text.Trim) = 0 Then
                        t13 = 0
                    Else
                        t13 = Decimal.Parse(TextBox13.Text.Trim)
                    End If
                    Dr2.Item(4) = t13
                    Dr2.Item(5) = Decimal.Parse(TextBox14.Text.Trim)
                    Dr2.Item(6) = Decimal.Parse(TextBox15.Text.Trim)
                    If RadioButton1.Checked Then
                        Dr2.Item(7) = "Design"
                    Else
                        Dr2.Item(7) = "Print"
                    End If
                    Dr2.Item(8) = arrImage
                    lastprice = Dr2.Item(9)
                    Dr2.Item(9) = Decimal.Parse(TextBox16.Text.Trim)
                    Dr2.Item(10) = DateTimePicker2.Value
                    Dr2.EndEdit()
                    Sda2.SelectCommand = cmd2
                    Scb2 = New SqlCommandBuilder(Sda2)
                    Sda2.UpdateCommand = Scb2.GetUpdateCommand
                    If Ds2.HasChanges Then
                        Sda2.Update(Ds2, "design")
                        MessageBox.Show("Design successfully Updated")
                        flag = 1
                        refreshgrid1()
                        ComboBox3.Text = ""
                        TextBox12.Text = ""
                        TextBox13.Text = ""
                        TextBox14.Text = ""
                        'TextBox15.Text = ""
                        RadioButton1.Checked = True
                        PictureBox2.Image = Nothing
                        TextBox16.Text = ""
                        ComboBox2.Text = ""
                        DateTimePicker2.Text = ""
                        ComboBox2.Focus()
                    End If
                    Exit While
                End If
                inc = inc + 1
                b = b - 1
            End While
            If flag = 0 Then
                MessageBox.Show(" Cannot Update.. There is no such design Record or it might be billed")
                ComboBox2.Focus()
                refreshgrid1()
            End If
        End If

        'Catch ex As Exception
        '    MessageBox.Show("Message to Agni User:   " & ex.Message)
        '    refreshgrid1()
        'End Try

    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        Try
            If ComboBox3.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Valid design Name")
                ComboBox2.Focus()
            Else
                If MessageBox.Show("Do you want to delete this design " & ComboBox3.Text, "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    flag = 0
                    Dim key As String
                    'mycurrentitem = DirectCast(ComboBox3.SelectedItem, MyComboitem)
                    'key = mycurrentitem.ID
                    key = desIdOpr
                    b = Dt2.Rows.Count
                    inc = 0
                    While (b)
                        Dr2 = Dt2.Rows(inc)
                        If key = Dr2.Item(1) And Dr2.Item(11).ToString.Equals("notpaid") Then
                            Dim lastprice As Integer
                            lastprice = Dr2.Item(9)
                            Dr2.Delete()
                            Scb2 = New SqlCommandBuilder(Sda2)
                            Sda2.DeleteCommand = Scb2.GetDeleteCommand()
                            If Ds2.HasChanges Then
                                Sda2.Update(Ds2, "design")
                                MessageBox.Show("Design successfully deleted")
                                flag = 1
                                ComboBox2.Text = ""
                                ComboBox3.Text = ""
                                TextBox12.Text = ""
                                TextBox13.Text = ""
                                TextBox14.Text = ""
                                'TextBox15.Text = ""
                                RadioButton1.Checked = True
                                PictureBox2.Image = Nothing
                                TextBox16.Text = ""
                                ComboBox3.Items.Clear()
                                DateTimePicker2.Text = ""
                                ComboBox2.Focus()
                                Exit While
                            End If
                        End If
                        inc = inc + 1
                        b = b - 1
                    End While
                    If flag = 0 Then
                        MessageBox.Show("Cannot Delete.. There is no such design Record or it might be billed")
                    End If
                End If
            End If
            refreshgrid1()
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
            refreshgrid1()
        End Try
    End Sub

    Private Sub TextBox12_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox12.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button8.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox12_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox12.TextChanged
        Try
            If Val(TextBox12.Text) = 0 Then
                t12 = 1
            Else
                t12 = Decimal.Parse(TextBox12.Text)
            End If
            If Val(TextBox13.Text) = 0 Then
                t13 = 1
            Else
                t13 = Decimal.Parse(TextBox13.Text)
            End If
            If Val(TextBox14.Text) = 0 Then
                t14 = 1
            Else
                t14 = Decimal.Parse(TextBox14.Text)
            End If
            If Val(TextBox15.Text) = 0 Then
                t15 = 1
            Else
                t15 = Decimal.Parse(TextBox15.Text)
            End If
            TextBox16.Text = Round(t12 * t13 * t14 * t15, 0)
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox13_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox13.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button8.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox13_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox13.TextChanged
        Try
            If Val(TextBox12.Text) = 0 Then
                t12 = 1
            Else
                t12 = Decimal.Parse(TextBox12.Text)
            End If
            If Val(TextBox13.Text) = 0 Then
                t13 = 1
            Else
                t13 = Decimal.Parse(TextBox13.Text)
            End If
            If Val(TextBox14.Text) = 0 Then
                t14 = 1
            Else
                t14 = Decimal.Parse(TextBox14.Text)
            End If
            If Val(TextBox15.Text) = 0 Then
                t15 = 1
            Else
                t15 = Decimal.Parse(TextBox15.Text)
            End If
            TextBox16.Text = Round(t12 * t13 * t14 * t15, 0)
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox14_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox14.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button8.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox14_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox14.TextChanged
        Try
            If Val(TextBox12.Text) = 0 Then
                t12 = 1
            Else
                t12 = Decimal.Parse(TextBox12.Text)
            End If
            If Val(TextBox13.Text) = 0 Then
                t13 = 1
            Else
                t13 = Decimal.Parse(TextBox13.Text)
            End If
            If Val(TextBox14.Text) = 0 Then
                t14 = 1
            Else
                t14 = Decimal.Parse(TextBox14.Text)
            End If
            If Val(TextBox15.Text) = 0 Then
                t15 = 1
            Else
                t15 = Decimal.Parse(TextBox15.Text)
            End If
            TextBox16.Text = Round(t12 * t13 * t14 * t15, 0)
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        Try
            Dim myindx As Int16 = 0
            If ComboBox3.Text.Trim.Equals("") Then
                MessageBox.Show("select design Name")
                ComboBox3.Focus()
            Else
                Dim key, key1 As String
                key = ComboBox2.Text
                key1 = ComboBox3.Text
                a = Dt2.Rows.Count
                inc = 0
                ComboBox3.Items.Clear()
                While (a)
                    Dr2 = Dt2.Rows(inc)
                    If (key.Equals(Dr2.Item(0).ToString())) Then
                        newitem = New MyComboitem(Dr2.Item(1), Dr2.Item(2).ToString)
                        ComboBox3.Items.Add(newitem)
                    End If
                    inc = inc + 1
                    a = a - 1
                End While
                myindx = ComboBox3.FindString(key1)
                If (myindx + 1) >= ComboBox3.Items.Count Then
                    ComboBox3.SelectedIndex = myindx
                    MsgBox("Message to Agni User: This is the last design for " + key)
                Else
                    ComboBox3.SelectedIndex = myindx + 1
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User: This is the last record (or) " & ex.Message)
        End Try
    End Sub

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        Try
            Dim myindx As Int16 = 0
            If ComboBox3.Text.Trim.Equals("") Then
                MessageBox.Show("select design Name")
                ComboBox3.Focus()
            Else

                Dim key, key1 As String
                key = ComboBox2.Text
                key1 = ComboBox3.Text
                a = Dt2.Rows.Count
                inc = 0
                ComboBox3.Items.Clear()
                While (a)
                    Dr2 = Dt2.Rows(inc)
                    If (key.Equals(Dr2.Item(0).ToString())) Then
                        newitem = New MyComboitem(Dr2.Item(1), Dr2.Item(2).ToString)
                        ComboBox3.Items.Add(newitem)
                    End If
                    inc = inc + 1
                    a = a - 1
                End While
                myindx = ComboBox3.FindString(key1)
                If (myindx - 1) < 0 Then
                    ComboBox3.SelectedIndex = myindx
                    MsgBox("Message to Agni User: This is the first design for " + key)
                Else
                    ComboBox3.SelectedIndex = myindx - 1
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User: This is the last record (or) " & ex.Message)
        End Try
    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        Try
            If ComboBox2.Text.Trim.Equals("") Then
                MessageBox.Show("select Company Name")
            Else
                Dim key As String
                key = ComboBox2.Text
                a = Dt2.Rows.Count
                inc = 0
                ComboBox3.Items.Clear()
                While (a)
                    Dr2 = Dt2.Rows(inc)
                    If (key.Equals(Dr2.Item(0).ToString())) Then
                        newitem = New MyComboitem(Dr2.Item(1), Dr2.Item(2).ToString)
                        ComboBox3.Items.Add(newitem)
                    End If
                    inc = inc + 1
                    a = a - 1
                End While
                If ComboBox3.Items.Count = 0 Then
                    MsgBox("Message to Agni User: There is no design for " + key)
                Else
                    ComboBox3.SelectedIndex = ComboBox3.Items.Count - 1
                End If

            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        Try
            If ComboBox4.Text.Trim.Equals("") Then
                MessageBox.Show("select Company Name")
                ComboBox4.Focus()
            Else
                Dim key As String
                key = ComboBox4.Text
                a = dt3.Rows.Count
                inc = 0
                ComboBox5.Items.Clear()
                While (a)
                    dr3 = dt3.Rows(inc)
                    If (key.Equals(dr3.Item(0).ToString())) Then
                        ComboBox5.Items.Add(dr3.Item(8).ToString + "/" + dr3.Item(1).ToString)
                    End If
                    inc = inc + 1
                    a = a - 1
                End While
                If ComboBox5.Items.Count = 0 Then
                    MsgBox("Message to Agni User: There is no bill for " + key)
                    ComboBox5.Focus()
                Else
                    ComboBox5.SelectedIndex = 0
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click
        Try
            dr3 = dt3.Rows(dt3.Rows.Count - 1)
            ComboBox4.Text = ""
            ComboBox4.SelectedText = dr3.Item(0).ToString()
            ComboBox5.Text = ""
            ComboBox5.SelectedText = dr3.Item(8).ToString + "/" + dr3.Item(1).ToString
            DateTimePicker1.Text = dr3.Item(2)
            TextBox20.Text = dr3.Item(3).ToString
            TextBox21.Text = dr3.Item(4).ToString
            TextBox17.Text = dr3.Item(5).ToString
            'TextBox18.Text = dr3.Item(6).ToString
            TextBox19.Text = dr3.Item(7).ToString
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        Try
            Dim myindx As Int16 = 0
            If ComboBox5.Text.Trim.Equals("") Then
                MessageBox.Show("select Bill Number")
                ComboBox5.Focus()
            Else

                Dim key, key1 As String
                key = ComboBox4.Text
                key1 = ComboBox5.Text
                a = dt3.Rows.Count
                inc = 0
                ComboBox5.Items.Clear()
                While (a)
                    dr3 = dt3.Rows(inc)
                    If (key.Equals(dr3.Item(0).ToString())) Then
                        ComboBox5.Items.Add(dr3.Item(8).ToString + "/" + dr3.Item(1).ToString)
                    End If
                    inc = inc + 1
                    a = a - 1
                End While
                myindx = ComboBox5.FindString(key1)
                If (myindx + 1) >= ComboBox5.Items.Count Then
                    ComboBox5.SelectedIndex = myindx
                    MsgBox("Message to Agni User: This is the last bill for " + key)
                    ComboBox5.Focus()
                Else
                    ComboBox5.SelectedIndex = myindx + 1
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User: This is the last record (or) " & ex.Message)
        End Try
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        Try
            Dim myindx As Int16 = 0
            If ComboBox5.Text.Trim.Equals("") Then
                MessageBox.Show("select Bill Number")
                ComboBox5.Focus()
            Else

                Dim key, key1 As String
                key = ComboBox4.Text
                key1 = ComboBox5.Text.ToString
                a = dt3.Rows.Count
                inc = 0
                ComboBox5.Items.Clear()
                While (a)
                    dr3 = dt3.Rows(inc)
                    If (key.Equals(dr3.Item(0).ToString())) Then
                        ComboBox5.Items.Add(dr3.Item(8).ToString + "/" + dr3.Item(1).ToString)
                    End If
                    inc = inc + 1
                    a = a - 1
                End While
                myindx = ComboBox5.FindString(key1)
                If (myindx - 1) < 0 Then
                    ComboBox5.SelectedIndex = myindx
                    MsgBox("Message to Agni User: This is the first bill for " + key)
                    ComboBox5.Focus()
                Else
                    ComboBox5.SelectedIndex = myindx - 1
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User: This is the first record (or) " & ex.Message)
        End Try
    End Sub

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
        Try
            If ComboBox4.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Valid Customer Name")
                ComboBox4.Focus()
            ElseIf Not ComboBox5.Text.Trim.Equals("") Then
                MessageBox.Show("This is a Duplicate Bill. Select the Company Name again from dropdown list.")
                ComboBox5.Focus()
                'ComboBox5.Text = ""
            ElseIf DateTimePicker1.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Valid Date")
                DateTimePicker1.Focus()
            ElseIf TextBox20.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Previous Balance amount")
                TextBox20.Focus()
            ElseIf TextBox21.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Design Amount")
                TextBox21.Focus()
            ElseIf TextBox17.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Total Amount")
                TextBox17.Focus()
            ElseIf TextBox19.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Balance Amount")
                TextBox19.Focus()
            ElseIf TextBox21.Text = 0 Then
                MessageBox.Show("This is a Duplicate Bill. You do not have any designs to bill for this Company.")
                TextBox21.Focus()
            Else
                Dim desdate, seldate As Date
                Dim desdatestr, seldatestr As String
                Dim lastbildate As DateTime
                Dim lastbildatestr As String = ""
                Dim StoredIndex(10000) As Integer
                Dim lastbalance As Decimal = 0
                Dim count As Integer = 0
                seldatestr = DateTimePicker1.Value.ToString("MM dd yyyy")
                seldate = DateTime.Parse(seldatestr)
                Dim custname = ComboBox4.Text
                flag = 0
                If dt3.Rows.Count = 0 Then
                    lastbildate = DateTime.Parse("01 01 1900")
                Else
                    a = dt3.Rows.Count - 1
                    While (a >= 0)
                        dr3 = dt3.Rows(a)
                        If custname.Equals(dr3.Item(0)) Then
                            lastbalance = dr3.Item(7)
                            lastbildate = DateTime.Parse(dr3.Item(2))
                            lastbildatestr = lastbildate.ToString("MM dd yyyy")
                            lastbildate = DateTime.Parse(lastbildatestr)
                            flag = 1
                            Exit While
                        End If
                        a -= 1
                    End While
                End If
                If flag = 0 Then
                    lastbildate = DateTime.Parse("01 01 1900")
                End If
                If lastbildate > seldate Then
                    MsgBox("Sorry.. You cannot create a Bill on '" + seldate.ToString("MMMM dd, yyyy") + "'. Because you have a Bill on '" + lastbildate.ToString("MMMM dd, yyyy") + "' for '" + custname + "'." + vbNewLine + "So you cannot create Bill in prior date. Please select Bill Date as on or after '" + lastbildate.ToString("MMMM dd, yyyy") + "'")
                    Exit Sub
                End If

                If lastbalance <> Decimal.Parse(TextBox20.Text) Then
                    MsgBox("Please select the company name again in drop down list. Because some Payments are changed now")
                    Exit Sub
                End If
             
                If Dt2.Rows.Count > 0 Then
                    a = Dt2.Rows.Count - 1
                    inc = 0
                    While (a >= 0)
                        Dr2 = Dt2.Rows(a)
                        If custname.Equals(Dr2.Item(0).ToString) Then
                            desdate = DateTime.Parse(Dr2.Item(10))
                            desdatestr = desdate.ToString("MM dd yyyy")
                            desdate = DateTime.Parse(desdatestr)
                            'If lastbildate > seldate Then
                            'Exit While
                            'ElseIf desdate <= seldate And lastbildate <= seldate And desdate <= seldate Then
                            'If lastbildate <= desdate And Dr2.Item(11).ToString.Equals("notpaid") Then
                            '    Dr2.BeginEdit()
                            '    Dr2.Item(11) = "paid"
                            '    StoredIndex(inc) = a
                            '    inc += 1
                            '    count += 1
                            '    Dr2.EndEdit()
                            'End If
                            'End If
                            If desdate <= seldate And Dr2.Item(11).ToString.Equals("notpaid") Then
                                Dr2.BeginEdit()
                                Dr2.Item(11) = "paid"
                                StoredIndex(inc) = a
                                inc += 1
                                count += 1
                                Dr2.EndEdit()
                            End If
                        End If
                        a -= 1
                    End While
                End If
                MyRow = dt3.NewRow()
                MyRow(0) = ComboBox4.Text.Trim
                Dim nowyear = DateTimePicker1.Value.Year.ToString.Substring(2, 2)
                MyRow(2) = DateTimePicker1.Value
                MyRow(3) = Decimal.Parse(TextBox20.Text.Trim)
                MyRow(4) = Decimal.Parse(TextBox21.Text.Trim)
                MyRow(5) = Decimal.Parse(TextBox17.Text.Trim)
                MyRow(6) = "notpaid"
                MyRow(7) = Decimal.Parse(TextBox19.Text.Trim)
                MyRow(8) = nowyear
                dt3.Rows.Add(MyRow)
                scb3 = New SqlCommandBuilder(sda3)
                sda3.InsertCommand = scb3.GetInsertCommand()
                If ds3.HasChanges Then
                    sda3.Update(ds3, "bill")
                    refreshgrid2()
                    Dim key As String
                    indx = 0
                    key = ComboBox4.Text
                    a = dt3.Rows.Count - 1
                    inc = 0
                    ComboBox5.Items.Clear()
                    While (a >= 0)
                        dr3 = dt3.Rows(a)
                        If (key.Equals(dr3.Item(0).ToString())) Then
                            ComboBox5.Text = dr3.Item(8).ToString + "/" + dr3.Item(1).ToString
                            MessageBox.Show(" Bill successfully added")
                            ComboBox4.Focus()
                            Exit While
                        End If
                        a = a - 1
                    End While

                    inc = 0
                    dr3 = dt3.Rows(dt3.Rows.Count - 1)
                    While count
                        Dr2 = Dt2.Rows(StoredIndex(inc))
                        Dr2.BeginEdit()
                        Dr2.Item(12) = dr3.Item(1)
                        Dr2.EndEdit()
                        inc += 1
                        count -= 1
                    End While

                    If Ds2.HasChanges Then
                        Scb2 = New SqlCommandBuilder(Sda2)
                        Sda2.UpdateCommand = Scb2.GetUpdateCommand
                        Sda2.Update(Ds2, "design")
                        refreshgrid1()
                    End If
                End If
                End If
                refreshgrid2()
                refreshgrid1()
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
            refreshgrid2()
            refreshgrid1()
        End Try
    End Sub

    Private Sub ComboBox5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox5.Click
        Try
            If ComboBox4.Text.Trim.Equals("") Then
                MsgBox("Please select Company name")
                ComboBox4.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub ComboBox5_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboBox5.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button21.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub ComboBox5_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox5.SelectedIndexChanged
        Try
            Dim key As String
            key = ComboBox5.Text
            key = key.Substring(key.IndexOf("/") + 1, key.Length - key.IndexOf("/") - 1)
            a = dt3.Rows.Count
            inc = 0
            While (a)
                dr3 = dt3.Rows(inc)
                If key.Equals(dr3.Item(1).ToString()) Then
                    ComboBox4.Text = ""
                    ComboBox4.SelectedText = dr3.Item(0).ToString()
                    DateTimePicker1.Text = dr3.Item(2)
                    TextBox20.Text = dr3.Item(3).ToString
                    TextBox21.Text = dr3.Item(4).ToString
                    TextBox17.Text = dr3.Item(5).ToString
                    'TextBox18.Text = dr3.Item(6).ToString
                    TextBox19.Text = dr3.Item(7).ToString
                    ds4.Dispose()
                    Exit While
                End If
                inc = inc + 1
                a = a - 1
            End While
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox17_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox17.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button21.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox18_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button21.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox18_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim T17, T18 As Decimal
            If Val(TextBox17.Text) = 0 Then
                T17 = 0
            Else
                T17 = Decimal.Parse(TextBox17.Text)
            End If
            'If Val(TextBox18.Text) = 0 Then
            '    T18 = 0
            'Else
            '    T18 = Decimal.Parse(TextBox18.Text)
            'End If
            TextBox19.Text = T17 - T18
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message & " (Check the input as a valid number) ")
        End Try
    End Sub

    Private Sub TextBox19_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox19.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button21.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Class MyComboitem
        Public ReadOnly ID As Integer
        Public ReadOnly Text As String
        Public Sub New(ByVal ID As Integer, ByVal Text As String)
            Try
                Me.ID = ID
                Me.Text = Text
            Catch ex As Exception
                MessageBox.Show("Message to Agni User:   " & ex.Message)
            End Try
        End Sub
        Public Overrides Function ToString() As String
            Return Text
        End Function
    End Class

    Private Sub ComboBox4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboBox4.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button21.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub



    Private Sub ComboBox4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox4.SelectedIndexChanged
        Try
            Dim key As String
            indx = 0
            key = ComboBox4.Text.Trim
            a = dt3.Rows.Count
            inc = 0
            ComboBox5.Items.Clear()
            While (a)
                dr3 = dt3.Rows(inc)
                If (key.Equals(dr3.Item(0).ToString())) Then
                    ComboBox5.Items.Add(dr3.Item(8).ToString + "/" + dr3.Item(1).ToString)
                End If
                inc = inc + 1
                a = a - 1
            End While
            ComboBox5.Text = ""
            TextBox17.Text = ""
            'TextBox18.Text = ""
            TextBox19.Text = ""
            TextBox20.Text = ""
            TextBox21.Text = ""
            DateTimePicker1.Value = DateTime.Today
            Dim desdate, seldate As Date
            Dim desdatestr, seldatestr As String
            Dim lastbildate As DateTime
            Dim lastbildatestr As String = ""
            Dim desamount As Decimal = 0
            Dim balamount As Decimal = 0
            seldatestr = DateTimePicker1.Value.ToString("MM dd yyyy")
            seldate = DateTime.Parse(seldatestr)
            Dim custname = ComboBox4.Text
            flag = 0
            If dt3.Rows.Count = 0 Then
                lastbildate = DateTime.Parse("01 01 1900")
                balamount = 0
            Else
                a = dt3.Rows.Count - 1
                While (a >= 0)
                    dr3 = dt3.Rows(a)
                    If custname.Equals(dr3.Item(0)) Then
                        lastbildate = DateTime.Parse(dr3.Item(2))
                        lastbildatestr = lastbildate.ToString("MM dd yyyy")
                        lastbildate = DateTime.Parse(lastbildatestr)
                        balamount = dr3.Item(7)
                        flag = 1
                        Exit While
                    End If
                    a -= 1
                End While
            End If
            If flag = 0 Then
                lastbildate = DateTime.Parse("01 01 1900")
                balamount = 0
            End If
            If lastbildate > seldate Then
                MsgBox("Sorry.. You cannot create a Bill on '" + seldate.ToString("MMMM dd, yyyy") + "'. Because you have a Bill on '" + lastbildate.ToString("MMMM dd, yyyy") + "' for '" + custname + "'." + vbNewLine + "So you cannot create Bill in prior date. Please select Bill Date as on or after '" + lastbildate.ToString("MMMM dd, yyyy") + "'")
                Exit Sub
            End If

            If Dt2.Rows.Count > 0 Then
                a = Dt2.Rows.Count - 1
                While (a >= 0)
                    Dr2 = Dt2.Rows(a)
                    If custname.Equals(Dr2.Item(0)) Then
                        desdate = DateTime.Parse(Dr2.Item(10))
                        desdatestr = desdate.ToString("MM dd yyyy")
                        desdate = DateTime.Parse(desdatestr)
                        'If lastbildate > seldate Then
                        'Exit While
                        'ElseIf desdate <= seldate And lastbildate <= seldate And desdate <= seldate Then
                        'If lastbildate <= desdate And Dr2.Item(11).ToString.Equals("notpaid") Then
                        '    desamount += Dr2.Item(9)
                        'End If
                        'End If
                        If desdate <= seldate And Dr2.Item(11).ToString.Equals("notpaid") Then
                            desamount += Dr2.Item(9)
                        End If
                    End If
                    a -= 1
                End While
            End If
            TextBox20.Text = balamount
            TextBox21.Text = desamount
            TextBox17.Text = desamount + balamount
            TextBox19.Text = TextBox17.Text
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub DateTimePicker1_CloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateTimePicker1.CloseUp
        Try
            Dim desdate, seldate As Date
            Dim desdatestr, seldatestr As String
            Dim desamount As Decimal = 0
            Dim balamount As Decimal = 0
            Dim lastbildate As DateTime
            Dim lastbildatestr As String = ""
            seldatestr = DateTimePicker1.Value.ToString("MM dd yyyy")
            seldate = DateTime.Parse(seldatestr)
            Dim custname = ComboBox4.Text
            flag = 0
            If dt3.Rows.Count = 0 Then
                lastbildate = DateTime.Parse("01 01 1900")
                balamount = 0
            Else
                a = dt3.Rows.Count - 1
                While (a >= 0)
                    dr3 = dt3.Rows(a)
                    If custname.Equals(dr3.Item(0)) Then
                        lastbildate = DateTime.Parse(dr3.Item(2))
                        lastbildatestr = lastbildate.ToString("MM dd yyyy")
                        lastbildate = DateTime.Parse(lastbildatestr)
                        balamount = dr3.Item(7)
                        flag = 1
                        Exit While
                    End If
                    a -= 1
                End While
            End If
            If flag = 0 Then
                lastbildate = DateTime.Parse("01 01 1900")
                balamount = 0
            End If
            If lastbildate > seldate Then
                MsgBox("Sorry.. You cannot create a Bill on '" + seldate.ToString("MMMM dd, yyyy") + "'. Because you have a Bill on '" + lastbildate.ToString("MMMM dd, yyyy") + "' for '" + custname + "'." + vbNewLine + "So you cannot create Bill in prior date. Please select Bill Date as on or after '" + lastbildate.ToString("MMMM dd, yyyy") + "'")
                Exit Sub
            End If

            If Dt2.Rows.Count > 0 Then
                a = Dt2.Rows.Count - 1
                While (a >= 0)
                    Dr2 = Dt2.Rows(a)
                    If custname.Equals(Dr2.Item(0)) Then
                        desdate = DateTime.Parse(Dr2.Item(10))
                        desdatestr = desdate.ToString("MM dd yyyy")
                        desdate = DateTime.Parse(desdatestr)
                        'If lastbildate > seldate Then
                        '    ' Exit While
                        'ElseIf desdate <= seldate And lastbildate <= seldate And desdate <= seldate Then
                        '    If lastbildate <= desdate And Dr2.Item(11).ToString.Equals("notpaid") Then
                        '        desamount += Dr2.Item(9)
                        '    End If
                        'End If
                        If desdate <= seldate And Dr2.Item(11).ToString.Equals("notpaid") Then
                            desamount += Dr2.Item(9)
                        End If
                    End If
                    a -= 1
                End While
            End If
            TextBox20.Text = balamount
            TextBox21.Text = desamount
            TextBox17.Text = desamount + balamount
            TextBox19.Text = TextBox17.Text
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button22.Click
        Try
            If ComboBox5.Text.Trim.Equals("") Then
                MsgBox("Please Select Bill Number")
                ComboBox5.Focus()
            Else

                billkey = ComboBox5.Text
                billcust = ComboBox4.Text

                Dim billkey1 As String = billkey
                billkey1 = billkey1.Substring(billkey1.IndexOf("/") + 1, billkey1.Length - billkey1.IndexOf("/") - 1)
                a = dt3.Rows.Count - 1
                Dim countcust As Int32 = 0
                While (a >= 0)
                    dr3 = dt3.Rows(a)
                    If billcust.Equals(dr3.Item(0).ToString) And billkey1 >= dr3.Item(1) Then
                        countcust += 1
                        PrevBillNo = "NIL"
                        If countcust = 2 Then
                            PrevBillNo = dr3.Item(8).ToString + "/" + dr3.Item(1).ToString
                            Exit While
                        End If
                    End If
                    a = a - 1
                End While

                billdatestring = DateTimePicker1.Value.ToString("dd/MM/yyyy")
                T17 = TextBox17.Text
                T20 = TextBox20.Text
                T21 = TextBox21.Text
                Agnireport.Show()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub ComboBox2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboBox2.KeyDown
        Try
            'If (e.KeyCode = Keys.Enter) Then
            '    Button8.PerformClick()
            'Else
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub



    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        Try
            Dim key As String
            key = ComboBox2.Text.Trim
            a = Dt2.Rows.Count
            inc = 0
            ComboBox3.Items.Clear()
            While (a)
                Dr2 = Dt2.Rows(inc)
                If (key.Equals(Dr2.Item(0).ToString())) Then
                    newitem = New MyComboitem(Dr2.Item(1), Dr2.Item(2).ToString)
                    ComboBox3.Items.Add(newitem)
                End If
                inc = inc + 1
                a = a - 1
            End While
            'ComboBox3.Text = ""
            'TextBox12.Text = ""
            'TextBox13.Text = ""
            'TextBox14.Text = ""
            'TextBox15.Text = ""
            'RadioButton1.Checked = True
            'PictureBox2.Image = Nothing
            'TextBox16.Text = ""
            'ComboBox2.Text = ""
            'DateTimePicker2.Value = DateTime.Today
            'Button33.Focus()
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub DataGrid1_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid1.CurrentCellChanged
        Try
            Dim key As String
            Dim pos As Int32
            Dim comboindx As Int32
            Dim descount As Int16 = 0
            pos = DataGrid1.CurrentRowIndex
            Dim destable As DataTable = DataGrid1.DataSource
            key = destable.Rows(pos).Item(0).ToString
            DesId = destable.Rows(pos).Item(1).ToString
            a = Dt2.Rows.Count
            inc = 0
            i = 0
            ComboBox3.Items.Clear()
            While (a)
                Dr2 = Dt2.Rows(inc)
                If key.Equals(Dr2.Item(0).ToString) Then
                    newitem = New MyComboitem(Dr2.Item(1), Dr2.Item(2).ToString)
                    ComboBox3.Items.Add(newitem)
                    i += 1
                    If Dr2.Item(1) = DesId Then
                        comboindx = i
                    End If
                End If
                inc = inc + 1
                a = a - 1
            End While
            Dr2 = Dt2.Rows(pos)
            ComboBox3.SelectedIndex = comboindx - 1
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub DataGrid1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DataGrid1.MouseUp
        Try
            Dim key As String
            Dim pos As Int32
            Dim comboindx As Int32
            Dim descount As Int16 = 0
            pos = DataGrid1.CurrentRowIndex
            Dim destable As DataTable = DataGrid1.DataSource
            key = destable.Rows(pos).Item(0).ToString
            DesId = destable.Rows(pos).Item(1).ToString
            desIdOpr = DesId
            a = Dt2.Rows.Count
            inc = 0
            i = 0
            ComboBox3.Items.Clear()
            While (a)
                Dr2 = Dt2.Rows(inc)
                If key.Equals(Dr2.Item(0).ToString) Then
                    newitem = New MyComboitem(Dr2.Item(1), Dr2.Item(2).ToString)
                    ComboBox3.Items.Add(newitem)
                    i += 1
                    If Dr2.Item(1) = DesId Then
                        comboindx = i
                    End If
                End If
                inc = inc + 1
                a = a - 1
            End While
            Dr2 = Dt2.Rows(pos)
            TextBox12.Text = Dr2.Item(3)
            TextBox13.Text = Dr2.Item(4)
            TextBox14.Text = Dr2.Item(5)
            TextBox15.Text = Dr2.Item(6)
            If Dr2.Item(7).ToString.Equals("design") Then
                RadioButton1.Checked = True
            Else
                RadioButton2.Checked = True
            End If
            If Dr2.Item(11).ToString.Equals("paid") Then
                RadioButton3.Checked = True
            Else
                RadioButton4.Checked = True
            End If
            If Dr2(8) Is DBNull.Value Then
                PictureBox2.Image = Nothing
            Else
                Dim arrayImage() As Byte = CType(Dr2(8), Byte())
                Dim ms As New MemoryStream(arrayImage)
                PictureBox2.Image = Image.FromStream(ms)
            End If

            TextBox16.Text = Dr2.Item(9)
            DateTimePicker2.Value = Dr2.Item(10)
            ComboBox2.Text = ""
            ComboBox2.SelectedText = Dr2.Item(0)
            ComboBox3.SelectedIndex = comboindx - 1
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub ComboBox3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If ComboBox2.Text.Trim.Equals("") Then
                MsgBox("Please select Company name")
                ComboBox2.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub ComboBox3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button8.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub DataGrid2_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DataGrid2.MouseUp
        Try
            Dim key As String
            Dim pos As Int32
            Dim BillId As Int32
            Dim comboindx As Int32 = 0
            pos = DataGrid2.CurrentRowIndex
            key = dt3.Rows(pos).Item(0).ToString
            BillId = dt3.Rows(pos).Item(1).ToString
            a = dt3.Rows.Count
            inc = 0
            i = 0
            ComboBox5.Items.Clear()
            While (a)
                dr3 = dt3.Rows(inc)
                If (key.Equals(dr3.Item(0).ToString())) Then
                    ComboBox5.Items.Add(dr3.Item(8).ToString + "/" + dr3.Item(1).ToString)
                    i += 1
                    If dr3.Item(1) = BillId Then
                        comboindx = i
                    End If
                End If
                inc = inc + 1
                a = a - 1
            End While
            ComboBox5.SelectedIndex = comboindx - 1

        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

   

    
   

    Private Sub Button27_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button27.Click
        Try
            If ComboBox1.Text.Trim.Equals("") Then
                MsgBox("Please Select Company Name")
                ComboBox1.Focus()
            ElseIf TextBox2.Text.Trim.Equals("") Then
                MsgBox("Please fill Address field")
                TextBox2.Focus()
            Else
                AddressReport.Show()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button28_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button28.Click
        Try
            TextBox22.Text = 5
            Label45.Visible = True
            Button28.Visible = False
            Button29.Visible = True
            Button30.Visible = True
            TextBox22.Visible = True
            Button46.PerformClick()
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button29_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button29.Click
        Try
            If TextBox22.Text.Trim.Equals("") Then
                MsgBox("Please Enter valid Count (between 1 to 100)")
                TextBox22.Focus()
            Else
                addrcount = Int16.Parse(TextBox22.Text)
                Label45.Visible = False
                Button28.Visible = True
                Button29.Visible = False
                Button30.Visible = False
                TextBox22.Visible = False
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button30_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button30.Click
        Try
            addrcount = 1
            TextBox22.Text = 5
            Label45.Visible = False
            Button28.Visible = True
            Button29.Visible = False
            Button30.Visible = False
            TextBox22.Visible = False
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub


    Private Sub Button31_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button31.Click
        Try
            addrselected = False
            AllAddrReport.Show()
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub


    Private Sub TextBox11_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox11.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button1.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button1.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox2.KeyDown
        Try
            '    If (e.KeyCode = Keys.Enter) Then
            '        Button1.PerformClick()
            If e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox3.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button1.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged

    End Sub

    Private Sub TextBox4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox4.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button1.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged

    End Sub

    Private Sub TextBox5_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox5.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button1.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged

    End Sub

    Private Sub TextBox6_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox6.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button1.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox6.TextChanged

    End Sub

    Private Sub TextBox7_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox7.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button1.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox7_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox7.TextChanged

    End Sub

    Private Sub TextBox8_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox8.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button1.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox8_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox8.TextChanged

    End Sub

    Private Sub TextBox9_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox9.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button1.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox9_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox9.TextChanged

    End Sub

    Private Sub TextBox10_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox10.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button1.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox10_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox10.TextChanged

    End Sub

    Private Sub Button32_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button32.Click
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
        TextBox7.Text = ""
        TextBox8.Text = ""
        TextBox9.Text = ""
        TextBox10.Text = ""
        TextBox11.Text = ""
        ComboBox1.Text = ""
        ComboBox1.Focus()
    End Sub

    Private Sub DataGrid1_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs) Handles DataGrid1.Navigate

    End Sub

    Private Sub DataGrid3_ControlRemoved(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs) Handles DataGrid3.ControlRemoved

    End Sub

    Private Sub DataGrid3_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid3.CurrentCellChanged
        Try
            Dim pos As Int32
            pos = DataGrid3.CurrentRowIndex
            Dim destable As DataTable = DataGrid3.DataSource
            DesId = destable.Rows(pos).Item(1).ToString
            Dr2 = destable.Rows(pos)
            If Dr2(8) Is DBNull.Value Then
                PictureBox1.Image = Nothing
            Else
                Dim arrayImage() As Byte = CType(Dr2(8), Byte())
                Dim ms As New MemoryStream(arrayImage)
                PictureBox1.Image = Image.FromStream(ms)
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub DataGrid3_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DataGrid3.MouseUp
        Try
            Dim pos As Int32
            pos = DataGrid3.CurrentRowIndex
            Dim destable As DataTable = DataGrid3.DataSource
            DesId = destable.Rows(pos).Item(1).ToString
            Dr2 = destable.Rows(pos)
            If Dr2(8) Is DBNull.Value Then
                PictureBox1.Image = Nothing
            Else
                Dim arrayImage() As Byte = CType(Dr2(8), Byte())
                Dim ms As New MemoryStream(arrayImage)
                PictureBox1.Image = Image.FromStream(ms)
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub DataGrid3_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs) Handles DataGrid3.Navigate

    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        Try
            If RadioButton1.Checked Then
                TextBox15.Text = "0.30"
            Else
                TextBox15.Text = "0.15"
            End If

        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        Try
            If RadioButton1.Checked Then
                TextBox15.Text = "0.30"
            Else
                TextBox15.Text = "0.15"
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try

    End Sub

    Private Sub TextBox16_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox16.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button8.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try

    End Sub

    Private Sub TextBox16_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox16.LostFocus
        TextBox16.ReadOnly = True
    End Sub

    Private Sub TextBox16_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox16.TextChanged

    End Sub

    Private Sub RadioButton1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles RadioButton1.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button8.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub RadioButton2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles RadioButton2.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button8.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub DateTimePicker2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DateTimePicker2.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button8.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub DateTimePicker2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker2.ValueChanged

    End Sub

    Private Sub DateTimePicker1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DateTimePicker1.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button21.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker1.ValueChanged

    End Sub

    Private Sub TextBox20_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox20.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button21.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox20_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox20.TextChanged

    End Sub

    Private Sub TextBox21_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox21.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button21.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox21_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox21.TextChanged

    End Sub

    Private Sub ComboBox6_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboBox6.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button26.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub ComboBox6_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox6.SelectedIndexChanged
        Try
            ComboBox9.Text = ""
            PictureBox1.Image = Nothing
            Dim key As String
            key = ComboBox6.Text.Trim
            a = Dt2.Rows.Count
            inc = 0
            ComboBox9.Items.Clear()
            While (a)
                Dr2 = Dt2.Rows(inc)
                If (key.Equals(Dr2.Item(0).ToString())) Then
                    newitem = New MyComboitem(Dr2.Item(1), Dr2.Item(2).ToString)
                    ComboBox9.Items.Add(newitem)
                End If
                inc = inc + 1
                a = a - 1
            End While


        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub DateTimePicker3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DateTimePicker3.KeyDown
        
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button26.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try

    End Sub

    Private Sub DateTimePicker3_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker3.ValueChanged
        PictureBox1.Image = Nothing
    End Sub

    Private Sub DateTimePicker4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DateTimePicker4.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button26.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub


    Private Sub Label22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label22.Click

    End Sub

    Private Sub Button33_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button33.Click

    End Sub

    Private Sub Button33_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button33.KeyDown
        Try
            If (e.KeyCode = Keys.Space) Then
                pictureload()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button34_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button34.Click
        TextBox16.ReadOnly = False
        TextBox16.Focus()
    End Sub

    Private Sub TabPage2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage2.Click

    End Sub

    Private Sub Button35_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button35.Click
        ComboBox2.Text = ""
        ComboBox3.Text = ""
        TextBox12.Text = ""
        TextBox13.Text = ""
        TextBox14.Text = ""
        RadioButton1.Checked = True
        PictureBox2.Image = Nothing
        TextBox16.Text = ""
        DateTimePicker2.Text = ""
        ComboBox2.Focus()
    End Sub

    Private Sub Button36_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button36.Click
        ComboBox4.Text = ""
        ComboBox5.Text = ""
        TextBox20.Text = ""
        TextBox21.Text = ""
        'TextBox18.Text = ""
        TextBox17.Text = ""
        TextBox19.Text = ""
        DateTimePicker1.Text = ""
        ComboBox4.Focus()
    End Sub

    Private Sub Button38_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button38.Click
        Try
            If MessageBox.Show("Are you sure want to log off?", "Log off", System.Windows.Forms.MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Me.Hide()
                Login.ComboBox1.Text = ""
                Login.TextBox2.Text = ""
                Login.Show()

                AddressReport.Close()
                Agnireport.Close()
                AllAddrReport.Close()
                AllBillReport.Close()
                Balance.Close()
                OutBalance.Close()
                Login.ComboBox1.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button37_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button37.Click
        Try
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub


    Private Sub Button39_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button39.Click
        Try
            If compbased = True And billbased = False And desbased = False And datebased = False And searchboth = False Then
                fromdatestr1 = "Not Specified"
                todatestr1 = "Not Specified"
                compname = cmprcomp
            ElseIf compbased = False And billbased = True And desbased = False And datebased = False And searchboth = False Then
                fromdatestr1 = "Not Specified"
                todatestr1 = "Not Specified"
                compname = cmprcomp
            ElseIf compbased = False And billbased = False And desbased = True And datebased = False And searchboth = False Then
                fromdatestr1 = "Not Specified"
                todatestr1 = "Not Specified"
                compname = cmprcomp
            ElseIf compbased = False And billbased = False And desbased = False And datebased = True And searchboth = False Then
                fromdatestr1 = cmprfromdate.ToString("MMMM dd, yyyy")
                todatestr1 = cmprtodate.ToString("MMMM dd, yyyy")
                compname = "Not Specified"
            ElseIf compbased = False And billbased = False And desbased = False And datebased = False And searchboth = True Then
                fromdatestr1 = cmprfromdate.ToString("MMMM dd, yyyy")
                todatestr1 = cmprtodate.ToString("MMMM dd, yyyy")
                compname = cmprcomp
            End If

            billtable = DataGrid4.DataSource

            If billtable.Rows.Count = 0 Then
                MsgBox("No Bills to print")
            Else
                AllBillReport.Show()
            End If

        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox11_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox11.TextChanged

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub Button1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button1.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button1.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button2.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button3.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button32_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button32.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button4.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button5_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button5.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button6_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button6.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button7_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button7.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TabPage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage1.Click
        ComboBox1.Focus()
    End Sub

    Private Sub Button27_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button27.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button31_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button31.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button29_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button29.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button30_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button30.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button28_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button28.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox22_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox22.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox22_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox22.TextChanged

    End Sub

    Private Sub TabPage1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage1.Enter
        ComboBox1.Focus()
    End Sub

    Private Sub TabPage1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage1.GotFocus
        ComboBox1.Focus()
    End Sub

    Private Sub TabPage1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TabPage1.MouseDown
        ComboBox1.Focus()
    End Sub

    Private Sub Button34_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button34.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button8_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button8.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button8.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button9_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button9.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button10_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button10.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button35_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button35.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub


    Private Sub Button13_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button13.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button14_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button14.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button15_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button15.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button12_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button12.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button22_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button22.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button24_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button23_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button36_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button36.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button17_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button17.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button16_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button16.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button11_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button11.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button18_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button18.KeyDown
        Try
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button25_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            'If (e.KeyCode = Keys.Enter) Then
            'Button20.PerformClick()
            'Else
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub DateTimePicker4_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker4.ValueChanged
        PictureBox1.Image = Nothing
    End Sub

    Private Sub Button20_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            'If (e.KeyCode = Keys.Enter) Then
            'Button20.PerformClick()
            'Else
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button26_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button26.KeyDown
        Try
            'If (e.KeyCode = Keys.Enter) Then
            'Button20.PerformClick()
            'Else
            If e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub DataGrid4_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DataGrid4.MouseDoubleClick
        Try
            Dim bilr As DataTable
            Dim billdate As Date
            bilr = DataGrid4.DataSource
            billkey = bilr.Rows(DataGrid4.CurrentRowIndex).Item(8).ToString + "/" + bilr.Rows(DataGrid4.CurrentRowIndex).Item(1).ToString
            billcust = bilr.Rows(DataGrid4.CurrentRowIndex).Item(0)

            Dim billkey1 As String = billkey
            billkey1 = billkey1.Substring(billkey1.IndexOf("/") + 1, billkey1.Length - billkey1.IndexOf("/") - 1)
            a = dt3.Rows.Count - 1
            Dim countcust As Int32 = 0
            While (a >= 0)
                dr3 = dt3.Rows(a)
                If billcust.Equals(dr3.Item(0).ToString) And billkey1 >= dr3.Item(1) Then
                    countcust += 1
                    PrevBillNo = "NIL"
                    If countcust = 2 Then
                        PrevBillNo = dr3.Item(8).ToString + "/" + dr3.Item(1).ToString
                        Exit While
                    End If
                End If
                a = a - 1
            End While

            billdate = bilr.Rows(DataGrid4.CurrentRowIndex).Item(2)
            billdatestring = billdate.ToString("dd/MM/yyyy")
            T17 = bilr.Rows(DataGrid4.CurrentRowIndex).Item(5)
            T20 = bilr.Rows(DataGrid4.CurrentRowIndex).Item(3)
            T21 = bilr.Rows(DataGrid4.CurrentRowIndex).Item(4)
            Agnireport.Show()
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

   

    Private Sub DataGrid4_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DataGrid4.MouseUp
        'Try
        '    Dim bilr As DataTable
        '    Dim billdate As Date
        '    bilr = DataGrid4.DataSource
        '    billkey = bilr.Rows(DataGrid4.CurrentRowIndex).Item(8).ToString + "/" + bilr.Rows(DataGrid4.CurrentRowIndex).Item(1).ToString
        '    billcust = bilr.Rows(DataGrid4.CurrentRowIndex).Item(0)

        '    Dim billkey1 As String = billkey
        '    billkey1 = billkey1.Substring(billkey1.IndexOf("/") + 1, billkey1.Length - billkey1.IndexOf("/") - 1)
        '    a = dt3.Rows.Count - 1
        '    Dim countcust As Int32 = 0
        '    While (a >= 0)
        '        dr3 = dt3.Rows(a)
        '        If billcust.Equals(dr3.Item(0).ToString) And billkey1 >= dr3.Item(1) Then
        '            countcust += 1
        '            PrevBillNo = "NIL"
        '            If countcust = 2 Then
        '                PrevBillNo = dr3.Item(8).ToString + "/" + dr3.Item(1).ToString
        '                Exit While
        '            End If
        '        End If
        '        a = a - 1
        '    End While

        '    billdate = bilr.Rows(DataGrid4.CurrentRowIndex).Item(2)
        '    billdatestring = billdate.ToString("dd/MM/yyyy")
        '    T17 = bilr.Rows(DataGrid4.CurrentRowIndex).Item(5)
        '    T20 = bilr.Rows(DataGrid4.CurrentRowIndex).Item(3)
        '    T21 = bilr.Rows(DataGrid4.CurrentRowIndex).Item(4)
        '    Agnireport.Show()
        'Catch ex As Exception
        '    MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End Try
    End Sub

    Private Sub Label64_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label64.Click

    End Sub

    Private Sub DataGrid2_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs) Handles DataGrid2.Navigate

    End Sub

    Private Sub TabPage3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage3.Click

    End Sub

    Private Sub Button40_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button40.Click
        Try
            'Dim ds10 As DataSet

            Dim dt17, dt12 As DataTable
            Dim dr17, dr12 As DataRow
            Dim notpaidbal As Decimal = 0
           
            dt17 = DataGrid2.DataSource
            dt12 = DataGrid1.DataSource
            outbal = 0
            unbilled = 0
            Dim b As Int16
            Dim a, a1 As Integer
            b = Dt1.Rows.Count
            inc = 0
            dt9.Clear()
            While (b)
                Dr1 = Dt1.Rows(inc)
                a1 = dt12.Rows.Count - 1
                notpaidbal = 0
                While (a1 >= 0)
                    dr12 = dt12.Rows(a1)
                    If dr12.Item(0).Equals(Dr1.Item(1)) And dr12.Item(11).ToString.Equals("notpaid") Then
                        notpaidbal += Decimal.Parse(dr12.Item(9))
                    ElseIf dr12.Item(0).Equals(Dr1.Item(1)) And dr12.Item(11).ToString.Equals("paid") Then
                        Exit While
                    End If
                    a1 -= 1
                End While

                a1 = dt10.Rows.Count - 1
                Deduction = 0
                taxDeduction = 0
                While (a1 >= 0)
                    dr10 = dt10.Rows(a1)
                    If dr10.Item(1).Equals(Dr1.Item(1)) Then
                        Deduction += Decimal.Parse(dr10.Item(7))
                        taxDeduction += Decimal.Parse(dr10.Item(8))
                    End If
                    a1 -= 1
                End While

                a = dt17.Rows.Count - 1
                flag = 0
                While (a >= 0)
                    dr17 = dt17.Rows(a)
                    If dr17.Item(0).Equals(Dr1.Item(1)) Then
                        flag = 1
                        dr9 = dt9.NewRow
                        dr9.Item(0) = dr17.Item(0)
                        'dr9.Item(1) = dr17.Item(1)
                        'dr9.Item(2) = dr17.Item(2)
                        'dr9.Item(3) = dr17.Item(3)
                        dr9.Item(4) = Deduction
                        dr9.Item(5) = taxDeduction
                        dr9.Item(7) = notpaidbal
                        dr9.Item(6) = dr17.Item(7)

                        totDeduction += dr9.Item(4)
                        tottaxDeduction += dr9.Item(5)

                        outbal += dr9.Item(6)
                        unbilled += notpaidbal
                        dt9.Rows.Add(dr9)

                        Exit While
                    End If
                    a -= 1
                End While
                If flag = 0 Then
                    dr9 = dt9.NewRow
                    dr9.Item(0) = Dr1.Item(1).ToString
                    dr9.Item(1) = 0
                    dr9.Item(2) = DateTime.Today
                    dr9.Item(3) = 0
                    dr9.Item(4) = 0
                    dr9.Item(5) = 0
                    dr9.Item(7) = notpaidbal
                    dr9.Item(6) = 0
                    unbilled += notpaidbal
                    dt9.Rows.Add(dr9)
                End If
                b -= 1
                inc += 1
            End While
            OutBalance.Show()
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button41_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button41.Click
        Try
            If ComboBox5.Text.Trim.Equals("") Then
                MessageBox.Show("Select Bill Number from below bill details")
                ComboBox5.Focus()
            Else
                If MessageBox.Show("Do you want to delete this bill " & ComboBox3.Text, "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    Dim flag1, flag2, flag3 As Boolean
                    Dim a1, inc1 As Int32
                    key = ComboBox5.Text
                    key = key.Substring(key.IndexOf("/") + 1, key.Length - key.IndexOf("/") - 1)

                    a = dt3.Rows.Count - 1
                    Dim custname As String = ""
                    Dim updbill As Decimal = 0
                    flag2 = False
                    While (a >= 0)
                        dr3 = dt3.Rows(a)
                        If key.ToString.Equals(dr3.Item(1).ToString) Then
                            custname = dr3.Item(0)
                            flag2 = True
                            Exit While
                        End If
                        a -= 1
                    End While
                    If flag2 = False Then
                        MsgBox("Sorry. There is no customer corresponding to the particular bill")
                        Exit Sub
                    End If

                    a = dt3.Rows.Count - 1
                    flag3 = False
                    While (a >= 0)
                        dr3 = dt3.Rows(a)
                        If custname.ToString.Equals(dr3.Item(0).ToString) Then
                            updbill = dr3.Item(7) - (dr3.Item(3) + dr3.Item(4))
                            lastbillno = dr3.Item(1)
                            flag3 = True
                            Exit While
                        End If
                        a -= 1
                    End While
                    If flag3 = False Then
                        MsgBox("Sorry. There is no bills for the particular Customer")
                        Exit Sub
                    End If

                    If Not lastbillno.ToString.Equals(key.ToString) Then
                        MsgBox("Sorry.. This is not last bill. And some of the bills are depending this bill. So you cannot delete this bill")
                    Else
                        a = dt3.Rows.Count - 1
                        inc = 0
                        flag = 0
                        While (a >= 0)
                            dr3 = dt3.Rows(a)
                            If key.ToString.Equals(dr3.Item(1).ToString) And dr3.Item(6).ToString.Equals("notpaid") Then
                                a1 = Dt2.Rows.Count - 1
                                inc1 = 0
                                flag1 = 0
                                While (a1 >= 0)
                                    Dr2 = Dt2.Rows(a1)
                                    If key.ToString.Equals(Dr2.Item(12).ToString) Then
                                        Dr2.BeginEdit()
                                        Dr2.Item(11) = "notpaid"
                                        Dr2.Item(12) = DBNull.Value
                                        Dr2.EndEdit()
                                        Sda2.SelectCommand = cmd2
                                        Scb2 = New SqlCommandBuilder(Sda2)
                                        Sda2.UpdateCommand = Scb2.GetUpdateCommand
                                        If Ds2.HasChanges Then
                                            Sda2.Update(Ds2, "design")
                                            flag1 = 1
                                        End If
                                    End If
                                    a1 -= 1
                                End While
                                dr3.Delete()
                                sda3.SelectCommand = cmd3
                                scb3 = New SqlCommandBuilder(sda3)
                                sda3.DeleteCommand = scb3.GetDeleteCommand()
                                If ds3.HasChanges Then
                                    sda3.Update(ds3, "bill")
                                    MessageBox.Show("Bill successfully deleted and Designs details are updated")

                                    ComboBox4.Text = ""
                                    ComboBox5.Text = ""
                                    TextBox20.Text = ""
                                    TextBox21.Text = ""
                                    'TextBox18.Text = ""
                                    TextBox17.Text = ""
                                    TextBox19.Text = ""
                                    DateTimePicker1.Text = ""
                                    ComboBox4.Focus()
                                    flag = 1
                                    refreshgrid2()

                                    a1 = dt3.Rows.Count - 1
                                    flag3 = False
                                    While (a1 >= 0)
                                        dr3 = dt3.Rows(a1)
                                        If custname.ToString.Equals(dr3.Item(0).ToString) Then
                                            lastbillno = dr3.Item(1)
                                            flag3 = True
                                            Exit While
                                        End If
                                        a1 -= 1
                                    End While
                                    If flag3 = True Then
                                        dr3.BeginEdit()
                                        dr3.Item(7) += updbill
                                        dr3.EndEdit()
                                        sda3.SelectCommand = cmd3
                                        scb3 = New SqlCommandBuilder(sda3)
                                        sda3.UpdateCommand = scb3.GetUpdateCommand
                                        If ds3.HasChanges Then
                                            sda3.Update(ds3, "bill")
                                        End If
                                        refreshgrid2()
                                    End If

                                End If
                                Exit While
                            End If
                            a = a - 1
                        End While

                        If flag = 0 Then
                            MessageBox.Show(" Cannot Delete.. There is no such Bill Record or this bill might be paid")
                        End If
                    End If
                End If
                End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
            refreshgrid2()
        End Try
    End Sub



    Private Sub RadioButton5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton5.CheckedChanged
        GroupBox5.Visible = False
    End Sub

    Private Sub RadioButton6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton6.CheckedChanged
        GroupBox5.Visible = True
    End Sub

    Private Sub ComboBox7_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboBox7.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button43.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub ComboBox7_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox7.SelectedIndexChanged
        Try
            'refreshgrid3()
            'refreshgrid2()
            Dim lastbildate, seldate As DateTime
            Dim lastbildatestr, seldatestr As String
            TextBox18.Text = ""
            TextBox25.Text = ""
            TextBox26.Text = ""
            TextBox28.Text = ""
            TextBox29.Text = ""
            TextBox27.Text = ""
            RadioButton5.Checked = True
            DateTimePicker6.Value = DateTime.Today
            actpaid = 0
            dt11.Clear()
            key = ComboBox7.Text.Trim
            seldatestr = DateTimePicker6.Value.ToString("MM dd yyyy")
            seldate = DateTime.Parse(seldatestr)
            If dt3.Rows.Count <> 0 Then
                a = dt3.Rows.Count - 1
                inc = 0
                While (a >= 0)
                    dr3 = dt3.Rows(a)
                    If key.ToString.Equals(dr3.Item(0).ToString) Then
                        lastbildate = DateTime.Parse(dr3.Item(2))
                        lastbildatestr = lastbildate.ToString("MM dd yyyy")
                        lastbildate = DateTime.Parse(lastbildatestr)
                        If lastbildate > seldate Then
                            MsgBox("Sorry.. You cannot Pay a Bill on '" + seldate.ToString("MMMM dd, yyyy") + "'. Because you have a Bill on '" + lastbildate.ToString("MMMM dd, yyyy") + "' for '" + key + "'." + vbNewLine + "So you cannot pay Bill in prior date. Please select Amount Paying Date as on or after '" + lastbildate.ToString("MMMM dd, yyyy") + "'")
                            Exit Sub
                        End If
                        TextBox23.Text = dr3.Item(8).ToString + "/" + dr3.Item(1).ToString
                        TextBox24.Text = dr3.Item(7)
                        Exit While
                    End If
                    a -= 1
                End While
            End If
            a = dt10.Rows.Count - 1
            inc = 0
            While (a >= 0)
                dr10 = dt10.Rows(inc)
                If key.ToString.Equals(dr10.Item(1).ToString) Then
                    dr11 = dt11.NewRow
                    dr11.Item(0) = dr10.Item(0)
                    dr11.Item(1) = dr10.Item(1)
                    dr11.Item(2) = dr10.Item(2)
                    dr11.Item(3) = dr10.Item(3)
                    dr11.Item(4) = dr10.Item(4)
                    dr11.Item(5) = dr10.Item(5)
                    dr11.Item(6) = dr10.Item(6)
                    actpaid += dr10.Item(6)
                    dr11.Item(7) = dr10.Item(7)
                    dr11.Item(8) = dr10.Item(8)
                    dr11.Item(9) = dr10.Item(9)
                    dr11.Item(10) = dr10.Item(10)
                    dr11.Item(11) = dr10.Item(11)
                    dr11.Item(12) = dr10.Item(12)
                    dr11.Item(13) = dr10.Item(13)
                    dt11.Rows.Add(dr11)
                End If
                a -= 1
                inc += 1
            End While

            DataGrid5.DataSource = ds11.Tables(0)
            Label83.Text = ds11.Tables(0).Rows.Count.ToString
            Label82.Text = actpaid
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
            refreshgrid3()
            refreshgrid2()
        End Try
    End Sub

    Private Sub DataGrid5_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DataGrid5.MouseUp
        Try
            Dim pos As Int32
            Dim paytable As DataTable
            paytable = DataGrid5.DataSource
            pos = DataGrid5.CurrentRowIndex
            dr10 = paytable.Rows(pos)
            TextBox18.Text = dr10.Item(0)
            ComboBox7.Text = ""
            ComboBox7.SelectedText = dr10.Item(1).ToString
            TextBox23.Text = dr10.Item(2)
            TextBox24.Text = dr10.Item(3)
            TextBox25.Text = dr10.Item(6)
            If dr10.Item(7) Is Nothing Then
                TextBox26.Text = ""
            Else
                TextBox26.Text = dr10.Item(7)
            End If
            If dr10.Item(8) Is Nothing Then
                TextBox27.Text = ""
            Else
                TextBox27.Text = dr10.Item(8)
            End If
            If dr10.Item(9) Is Nothing Then
                TextBox28.Text = ""
            Else
                TextBox28.Text = dr10.Item(9)
            End If
            If dr10.Item(10) Is Nothing Then
                TextBox29.Text = ""
            Else
                TextBox29.Text = dr10.Item(10)
            End If
            If dr10.Item(11) Is DBNull.Value Then
                DateTimePicker7.Text = ""
            Else
                DateTimePicker7.Value = dr10.Item(11)
            End If
            If dr10.Item(12) Is Nothing Then
                TextBox30.Text = ""
            Else
                TextBox30.Text = dr10.Item(12)
            End If

            TextBox31.Text = dr10.Item(13)
            DateTimePicker6.Value = dr10.Item(4)

            If dr10.Item(5).ToString.Equals("Cash") Then
                RadioButton5.Checked = True
            Else
                RadioButton6.Checked = True
            End If
            'Button43.Enabled = False
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub DataGrid5_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs) Handles DataGrid5.Navigate

    End Sub


    Private Sub Button43_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button43.Click
        Try
            If ComboBox7.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Valid company Name")
                ComboBox7.Focus()
            ElseIf DateTimePicker6.Text.Trim.Equals("") Then
                MessageBox.Show("Choose Valid Paid date")
                DateTimePicker6.Focus()
            ElseIf Val(TextBox24.Text) = 0 Then
                MessageBox.Show("There is no Balence Amount to pay")
                TextBox24.Focus()
            ElseIf TextBox25.Text.Trim.Equals("") And TextBox26.Text.Trim.Equals("") And TextBox27.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Paid Amount or Deduction or Tax Deduction")
                TextBox25.Focus()
            ElseIf TextBox31.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Amount to be credited")
                TextBox31.Focus()
            ElseIf RadioButton6.Checked And TextBox28.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Cheque Number")
                TextBox28.Focus()
            ElseIf RadioButton6.Checked And TextBox29.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Bank Name")
                TextBox29.Focus()
            ElseIf RadioButton6.Checked And DateTimePicker7.Text.Trim.Equals("") Then
                MessageBox.Show("Choose Valid Cheque date")
                DateTimePicker7.Focus()
            ElseIf Not TextBox18.Text.Trim.Equals("") Then
                MessageBox.Show("This is a Duplicate Bill Paying Transaction. Please make valid transaction by selecting Company Name from dropdown list")
                TextBox18.Focus()
            Else
                Dim lastbildate, seldate As DateTime
                Dim lastbildatestr, seldatestr As String
                seldatestr = DateTimePicker6.Value.ToString("MM dd yyyy")
                seldate = DateTime.Parse(seldatestr)
                key = ComboBox7.Text
                If dt3.Rows.Count <> 0 Then
                    a = dt3.Rows.Count - 1
                    inc = 0
                    While (a >= 0)
                        dr3 = dt3.Rows(a)
                        If key.ToString.Equals(dr3.Item(0).ToString) Then
                            lastbildate = DateTime.Parse(dr3.Item(2))
                            lastbildatestr = lastbildate.ToString("MM dd yyyy")
                            lastbildate = DateTime.Parse(lastbildatestr)
                            If lastbildate > seldate Then
                                MsgBox("Sorry.. You cannot Pay a Bill on '" + seldate.ToString("MMMM dd, yyyy") + "'. Because you have a Bill on '" + lastbildate.ToString("MMMM dd, yyyy") + "' for '" + key + "'." + vbNewLine + "So you cannot pay Bill in prior date. Please select Amount Paying Date as on or after '" + lastbildate.ToString("MMMM dd, yyyy") + "'")
                                Exit Sub
                            End If

                            If Not TextBox24.Text.ToString.Equals(dr3.Item(7).ToString) Then
                                MessageBox.Show("Select Company Name again in dropdown list. Because the Balance is changed now for this Compnay")
                                ComboBox7.Focus()
                                Exit Sub
                            End If

                            Exit While
                        End If
                        a -= 1
                    End While
                End If

                MyRow = dt10.NewRow()
                MyRow(1) = ComboBox7.Text.Trim
                MyRow(2) = TextBox23.Text.Trim
                MyRow(3) = TextBox24.Text.Trim
                MyRow(4) = DateTimePicker6.Value
                If RadioButton5.Checked Then
                    MyRow(5) = "Cash"
                    MyRow(11) = DBNull.Value
                Else
                    MyRow(5) = "Cheque"
                    MyRow(11) = DateTimePicker7.Value
                End If
                If Val(TextBox25.Text.Trim) = 0 Then
                    t25 = 0
                Else
                    t25 = Decimal.Parse(TextBox25.Text.Trim)
                End If
                MyRow(6) = t25
                If Val(TextBox26.Text.Trim) = 0 Then
                    t26 = 0
                Else
                    t26 = Decimal.Parse(TextBox26.Text.Trim)
                End If
                If Val(TextBox27.Text.Trim) = 0 Then
                    t27 = 0
                Else
                    t27 = Decimal.Parse(TextBox27.Text.Trim)
                End If
                MyRow(7) = t26
                MyRow(8) = t27
                MyRow(9) = TextBox28.Text.Trim
                MyRow(10) = TextBox29.Text.Trim
                MyRow(12) = TextBox30.Text.Trim
                MyRow(13) = TextBox31.Text.Trim
                dt10.Rows.Add(MyRow)
                sda10.SelectCommand = cmd10
                scb10 = New SqlCommandBuilder(sda10)
                sda10.InsertCommand = scb10.GetInsertCommand()
                If ds10.HasChanges Then
                    ' sda10.Update(ds10, "payment")
                    key = TextBox23.Text.Trim
                    key = key.Substring(key.IndexOf("/") + 1, key.Length - key.IndexOf("/") - 1)
                    a = dt3.Rows.Count - 1
                    inc = 0
                    While (a >= 0)
                        dr3 = dt3.Rows(a)
                        If key.ToString.Equals(dr3.Item(1).ToString) Then
                            'If TextBox32.Text.ToString.Equals(dr3.Item(7).ToString) Then
                            '    MsgBox("Sorry. This is Duplicate Bill Paying Transaction. Please make a valid transaction by selecting the Company Name")
                            '    Exit Sub
                            'End If
                            dr3.BeginEdit()
                            If Val(TextBox32.Text.Trim) = 0 Then
                                dr3.Item(7) = 0
                            Else
                                dr3.Item(7) = Decimal.Parse(TextBox32.Text.Trim)
                                dr3.Item(6) = "paid"
                            End If
                            dr3.EndEdit()
                            sda3.SelectCommand = cmd3
                            scb3 = New SqlCommandBuilder(sda3)
                            sda3.UpdateCommand = scb3.GetUpdateCommand
                            Exit While
                        End If
                        a -= 1
                    End While
                    sda10.Update(ds10, "payment")
                    sda3.Update(ds3, "bill")
                    MessageBox.Show("Payment successfully added and Accounts updated")
                    ComboBox7.Text = ""
                    TextBox23.Text = ""
                    TextBox24.Text = ""
                    TextBox25.Text = ""
                    TextBox26.Text = ""
                    TextBox27.Text = ""
                    TextBox28.Text = ""
                    TextBox29.Text = ""
                    TextBox30.Text = ""
                    refreshgrid3()
                    Label83.Text = ds10.Tables(0).Rows.Count.ToString
                    ComboBox7.Focus()
                End If
                End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
            refreshgrid3()
        End Try
    End Sub

    Private Sub TextBox25_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox25.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button43.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox25_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox25.TextChanged
        Try
            If Val(TextBox25.Text) = 0 Then
                t25 = 0
            Else
                t25 = Decimal.Parse(TextBox25.Text)
            End If
            If Val(TextBox26.Text) = 0 Then
                t26 = 0
            Else
                t26 = Decimal.Parse(TextBox26.Text)
            End If
            If Val(TextBox27.Text) = 0 Then
                t27 = 0
            Else
                t27 = Decimal.Parse(TextBox27.Text)
            End If
            If Val(TextBox24.Text) = 0 Then
                t24 = 0
            Else
                t24 = Decimal.Parse(TextBox24.Text)
            End If
            If (t25 + t26 + t27) > t24 Then
                'MsgBox("Amount to be credited cannot be greater than balance amount. Please try again")
                TextBox25.Text = ""
                TextBox26.Text = ""
                TextBox27.Text = ""
                TextBox31.Text = ""
            Else
                TextBox31.Text = t25 + t26 + t27
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox26_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox26.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button43.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox26_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox26.TextChanged
        Try
            If Val(TextBox25.Text) = 0 Then
                t25 = 0
            Else
                t25 = Decimal.Parse(TextBox25.Text)
            End If
            If Val(TextBox26.Text) = 0 Then
                t26 = 0
            Else
                t26 = Decimal.Parse(TextBox26.Text)
            End If
            If Val(TextBox27.Text) = 0 Then
                t27 = 0
            Else
                t27 = Decimal.Parse(TextBox27.Text)
            End If
            If Val(TextBox24.Text) = 0 Then
                t24 = 0
            Else
                t24 = Decimal.Parse(TextBox24.Text)
            End If
            If (t25 + t26 + t27) > t24 Then
                'MsgBox("Amount to be credited cannot be greater than balance amount. Please try again")
                TextBox25.Text = ""
                TextBox26.Text = ""
                TextBox27.Text = ""
                TextBox31.Text = ""
            Else
                TextBox31.Text = t25 + t26 + t27
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox27_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox27.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button43.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox27_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox27.TextChanged
        Try
            If Val(TextBox25.Text) = 0 Then
                t25 = 0
            Else
                t25 = Decimal.Parse(TextBox25.Text)
            End If
            If Val(TextBox26.Text) = 0 Then
                t26 = 0
            Else
                t26 = Decimal.Parse(TextBox26.Text)
            End If
            If Val(TextBox27.Text) = 0 Then
                t27 = 0
            Else
                t27 = Decimal.Parse(TextBox27.Text)
            End If
            If Val(TextBox24.Text) = 0 Then
                t24 = 0
            Else
                t24 = Decimal.Parse(TextBox24.Text)
            End If
            If (t25 + t26 + t27) > t24 Then
                TextBox25.Text = ""
                TextBox26.Text = ""
                TextBox27.Text = ""
                TextBox31.Text = ""
            Else
                TextBox31.Text = t25 + t26 + t27
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox31_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox31.TextChanged
        Try
            If TextBox31.Text = "" And TextBox24.Text <> "" Then
                MsgBox("Amount to be credited cannot be greater than balance amount. Please try again")
                TextBox25.Focus()
            End If
            If Val(TextBox24.Text) = 0 Then
                t24 = 0
            Else
                t24 = Decimal.Parse(TextBox24.Text)
            End If
            If Val(TextBox31.Text) = 0 Then
                t31 = 0
            Else
                t31 = Decimal.Parse(TextBox31.Text)
            End If
            TextBox32.Text = t24 - t31
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox24_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox24.TextChanged
        Try
            If Val(TextBox24.Text) = 0 Then
                t24 = 0
            Else
                t24 = Decimal.Parse(TextBox24.Text)
            End If
            If Val(TextBox31.Text) = 0 Then
                t31 = 0
            Else
                t31 = Decimal.Parse(TextBox31.Text)
            End If
            TextBox32.Text = t24 - t31
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub DataGrid4_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs) Handles DataGrid4.Navigate

    End Sub

    Private Sub TextBox28_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox28.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button43.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox28_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox28.TextChanged

    End Sub

    Private Sub TextBox29_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox29.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button43.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox29_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox29.TextChanged

    End Sub

    Private Sub TextBox30_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox30.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button43.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox30_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox30.TextChanged

    End Sub

    Private Sub RadioButton5_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles RadioButton5.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button43.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub RadioButton6_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles RadioButton6.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button43.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub DateTimePicker6_CloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateTimePicker6.CloseUp
        Try
            'refreshgrid3()
            'refreshgrid2()
            Dim lastbildate, seldate As DateTime
            Dim lastbildatestr, seldatestr As String
            actpaid = 0
            dt11.Clear()
            key = ComboBox7.Text.Trim
            seldatestr = DateTimePicker6.Value.ToString("MM dd yyyy")
            seldate = DateTime.Parse(seldatestr)
            If dt3.Rows.Count <> 0 Then
                a = dt3.Rows.Count - 1
                inc = 0
                While (a >= 0)
                    dr3 = dt3.Rows(a)
                    If key.ToString.Equals(dr3.Item(0).ToString) Then
                        lastbildate = DateTime.Parse(dr3.Item(2))
                        lastbildatestr = lastbildate.ToString("MM dd yyyy")
                        lastbildate = DateTime.Parse(lastbildatestr)
                        If lastbildate > seldate Then
                            MsgBox("Sorry.. You cannot Pay a Bill on '" + seldate.ToString("MMMM dd, yyyy") + "'. Because you have a Bill on '" + lastbildate.ToString("MMMM dd, yyyy") + "' for '" + key + "'." + vbNewLine + "So you cannot pay Bill in prior date. Please select Amount Paying Date as on or after '" + lastbildate.ToString("MMMM dd, yyyy") + "'")
                            Exit Sub
                        End If
                        TextBox23.Text = dr3.Item(8).ToString + "/" + dr3.Item(1).ToString
                        TextBox24.Text = dr3.Item(7)
                        Exit While
                    End If
                    a -= 1
                End While
            End If
            a = dt10.Rows.Count - 1
            inc = 0
            While (a >= 0)
                dr10 = dt10.Rows(inc)
                If key.ToString.Equals(dr10.Item(1).ToString) Then
                    dr11 = dt11.NewRow
                    dr11.Item(0) = dr10.Item(0)
                    dr11.Item(1) = dr10.Item(1)
                    dr11.Item(2) = dr10.Item(2)
                    dr11.Item(3) = dr10.Item(3)
                    dr11.Item(4) = dr10.Item(4)
                    dr11.Item(5) = dr10.Item(5)
                    dr11.Item(6) = dr10.Item(6)
                    actpaid += dr10.Item(6)
                    dr11.Item(7) = dr10.Item(7)
                    dr11.Item(8) = dr10.Item(8)
                    dr11.Item(9) = dr10.Item(9)
                    dr11.Item(10) = dr10.Item(10)
                    dr11.Item(11) = dr10.Item(11)
                    dr11.Item(12) = dr10.Item(12)
                    dr11.Item(13) = dr10.Item(13)
                    dt11.Rows.Add(dr11)
                End If
                a -= 1
                inc += 1
            End While

            DataGrid5.DataSource = ds11.Tables(0)
            Label83.Text = ds11.Tables(0).Rows.Count.ToString
            Label82.Text = actpaid
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
            refreshgrid3()
            refreshgrid2()
        End Try
    End Sub

    Private Sub DateTimePicker6_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DateTimePicker6.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button43.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub DateTimePicker6_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker6.ValueChanged
      
    End Sub

    Private Sub DateTimePicker7_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DateTimePicker7.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button43.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.R Then
                    TabControl1.SelectTab(4)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub DateTimePicker7_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker7.ValueChanged

    End Sub

    Private Sub TabPage6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage6.Click

    End Sub

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click
        ListBox1.Visible = True
        Button23.Visible = True
        Button24.Visible = True
        Button27.Visible = False
        Button31.Visible = False
        Button44.Visible = False
        Button19.Visible = False
        Button28.Visible = False
        Button30.PerformClick()

        Button46.PerformClick()
        Button44.Visible = False
        Button28.Visible = False
    End Sub

    Private Sub Button24_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button24.Click

        ListBox1.Visible = False
        Button23.Visible = False
        Button24.Visible = False
        Button44.Visible = True
        Button27.Visible = True
        Button31.Visible = True
        Button19.Visible = True
        Button28.Visible = True
        Button44.Visible = True
        Button28.Visible = True
    End Sub

    Private Sub Button23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button23.Click
        Try
            addrselected = True
            'listbox1.GetSelected(
            AllAddrReport.Show()
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub ListBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListBox1.KeyDown
        Try
            '    If (e.KeyCode = Keys.Enter) Then
            '        Button1.PerformClick()
            If e.Alt Then
                If e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(4)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub



    Private Sub Button42_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button42.Click
        Try
            If TextBox18.Text.Trim.Equals("") Then
                MessageBox.Show("Select paid bill transaction to delete from payment details")
                TextBox18.Focus()
            Else
                If MessageBox.Show("Do you want to delete this payment transaction ", "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    Dim flag1, flag2, flag3 As Boolean
                    Dim a1 As Int32
                    key = Integer.Parse(TextBox18.Text)

                    cmd10 = New SqlCommand("select * from payment", Con)
                    sda10 = New SqlDataAdapter()
                    sda10.SelectCommand = cmd10
                    ds10 = New DataSet
                    sda10.Fill(ds10, "payment")
                    dt10 = ds10.Tables(0)

                    cmd3 = New SqlCommand("select * from bill", Con)
                    sda3 = New SqlDataAdapter()
                    sda3.SelectCommand = cmd3
                    ds3 = New DataSet
                    sda3.Fill(ds3, "bill")
                    dt3 = ds3.Tables(0)

                    a = dt10.Rows.Count - 1
                    Dim custname As String = ""
                    Dim lastpayno As Integer
                    flag2 = False
                    While (a >= 0)
                        dr10 = dt10.Rows(a)
                        If key.ToString.Equals(dr10.Item(0).ToString) Then
                            custname = dr10.Item(1)
                            flag2 = True
                            Exit While
                        End If
                        a -= 1
                    End While
                    If flag2 = False Then
                        MsgBox("Sorry. There is no payment as you selected")
                        Exit Sub
                    End If
                    Dim lastbillno1 As String = ""
                    a = dt10.Rows.Count - 1
                    flag3 = False
                    While (a >= 0)
                        dr10 = dt10.Rows(a)
                        If custname.ToString.Equals(dr10.Item(1).ToString) Then
                            lastpayno = dr10.Item(0)
                            lastbillno1 = dr10.Item(2)
                            flag3 = True
                            Exit While
                        End If
                        a -= 1
                    End While
                    If flag3 = False Then
                        MsgBox("Sorry. The Customer has no bills")
                        Exit Sub
                    End If

                    a = dt10.Rows.Count - 1
                    Dim count As Integer = 0
                    While (a >= 0)
                        dr10 = dt10.Rows(a)
                        If custname.ToString.Equals(dr10.Item(1).ToString) And lastbillno1.ToString.Equals(dr10.Item(2).ToString) Then
                            count += 1
                        End If
                        a -= 1
                    End While

                    lastbillno1 = lastbillno1.Substring(lastbillno1.IndexOf("/") + 1, lastbillno1.Length - lastbillno1.IndexOf("/") - 1)
                    Dim key1 As String
                    If Not lastpayno.ToString.Equals(key.ToString) Then
                        MsgBox("Sorry.. This is not last Payment Transaction. And some of the payment transactions are depending this payment. You are allowed to delete last payment transaction only")
                    Else
                        a = dt10.Rows.Count - 1
                        flag = 0
                        While (a >= 0)
                            dr10 = dt10.Rows(a)
                            If key.ToString.Equals(dr10.Item(0).ToString) Then
                                flag1 = 0
                                key1 = dr10.Item(2).ToString
                                key1 = key1.Substring(key1.IndexOf("/") + 1, key1.Length - key1.IndexOf("/") - 1)
                                a1 = dt3.Rows.Count - 1
                                Dim custname1 As String = ""
                                flag2 = False
                                While (a1 >= 0)
                                    dr3 = dt3.Rows(a1)
                                    If key1.ToString.Equals(dr3.Item(1).ToString) Then
                                        custname1 = dr3.Item(0)
                                        flag2 = True
                                        Exit While
                                    End If
                                    a1 -= 1
                                End While
                                If flag2 = False Then
                                    MsgBox("Sorry. There is no bill corresponding to the particular payment transaction")
                                    Exit Sub
                                End If

                                a1 = dt3.Rows.Count - 1
                                flag3 = False
                                While (a1 >= 0)
                                    dr3 = dt3.Rows(a1)
                                    If custname1.ToString.Equals(dr3.Item(0).ToString) Then
                                        lastbillno = dr3.Item(1)
                                        flag3 = True
                                        Exit While
                                    End If
                                    a1 -= 1
                                End While
                                If flag3 = False Then
                                    MsgBox("Sorry. There is no customer corresponding to the particular bill")
                                    Exit Sub
                                End If

                                a1 = dt3.Rows.Count - 1
                                While (a1 >= 0)
                                    dr3 = dt3.Rows(a1)
                                    If lastbillno1.ToString.Equals(dr3.Item(1).ToString) Then
                                        If count <= 1 Then
                                            dr3.BeginEdit()
                                            dr3.Item(6) = "notpaid"
                                            dr3.EndEdit()
                                            sda3.SelectCommand = cmd3
                                            scb3 = New SqlCommandBuilder(sda3)
                                            sda3.UpdateCommand = scb3.GetUpdateCommand
                                            If ds3.HasChanges Then
                                                sda3.Update(ds3, "bill")
                                                flag1 = 1
                                            End If
                                        End If
                                    End If
                                    If lastbillno.ToString.Equals(dr3.Item(1).ToString) Then
                                        dr3.BeginEdit()
                                        dr3.Item(7) += Decimal.Parse(TextBox31.Text)
                                        dr3.EndEdit()
                                        sda3.SelectCommand = cmd3
                                        scb3 = New SqlCommandBuilder(sda3)
                                        sda3.UpdateCommand = scb3.GetUpdateCommand
                                        If ds3.HasChanges Then
                                            sda3.Update(ds3, "bill")
                                            flag1 = 1
                                        End If
                                    End If
                                    a1 -= 1
                                End While
                                If flag1 = 0 Then
                                    MsgBox("No Bills Updated")
                                    Exit Sub
                                End If
                                dr10.Delete()
                                sda10.SelectCommand = cmd10
                                scb10 = New SqlCommandBuilder(sda10)
                                sda10.DeleteCommand = scb10.GetDeleteCommand()
                                If ds10.HasChanges Then
                                    sda10.Update(ds10, "payment")
                                    MessageBox.Show("Payment transaction successfully deleted and Bill details are updated")
                                    TextBox18.Text = ""
                                    refreshgrid3()
                                    refreshgrid2()
                                    TextBox23.Text = ""
                                    TextBox24.Text = ""
                                    TextBox25.Text = ""
                                    TextBox26.Text = ""
                                    TextBox27.Text = ""
                                    TextBox28.Text = ""
                                    TextBox29.Text = ""
                                    TextBox30.Text = ""
                                    TextBox31.Text = ""
                                    TextBox32.Text = ""
                                    ComboBox7.Text = ""
                                    ComboBox5.Text = ""
                                    RadioButton5.Checked = True
                                    flag = 1
                                    ComboBox7.Focus()
                                End If
                                Exit While
                            End If
                            a = a - 1
                        End While
                        If flag = 0 Then
                            MessageBox.Show(" Cannot Delete.. There is no such Paid Bill transaction")
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
            refreshgrid3()
            refreshgrid2()
        End Try
    End Sub

    
    Private Sub Button44_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button44.Click
        Try
            Label101.Visible = True
            Label102.Visible = True
            TextBox33.Visible = True
            Button45.Visible = True
            Button46.Visible = True
            Button44.Visible = False
            Button30.PerformClick()
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try

    End Sub

    Private Sub Button45_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button45.Click
        Try
            If ComboBox1.Text.Trim.ToString.Equals("") Then
                MsgBox("Please Select Old Name of the Customer")
                ComboBox1.Focus()
            ElseIf TextBox33.Text.Trim.ToString.Equals("") Then
                MsgBox("Please Enter New Name of the Customer")
                TextBox33.Focus()
            ElseIf MessageBox.Show("All Designs, Bills and Payments will be updated to new name belongs to this customer." + vbNewLine + vbNewLine + vbTab + "  Do you want to Change this Customer - " & ComboBox1.Text & " To - " & TextBox33.Text & " ?", "WARNING", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Dim key As String
                Dim a1, a2 As Int32
                key = ComboBox1.Text
                a1 = Dt2.Rows.Count
                inc = 0
                flag = 0
                While (a1)
                    Dr2 = Dt2.Rows(inc)
                    If (key.Equals(Dr2.Item(0).ToString())) Then
                        Dr2.BeginEdit()
                        Dr2.Item(0) = TextBox33.Text
                        Dr2.EndEdit()
                        Sda2.SelectCommand = cmd2
                        Scb2 = New SqlCommandBuilder(Sda2)
                        Sda2.UpdateCommand = Scb2.GetUpdateCommand
                    End If
                    inc = inc + 1
                    a1 = a1 - 1
                End While

                If Ds2.HasChanges Then
                    Sda2.Update(Ds2, "design")
                    refreshgrid1()
                End If

                a2 = dt3.Rows.Count
                inc = 0
                While (a2)
                    dr3 = dt3.Rows(inc)
                    If (key.Equals(dr3.Item(0).ToString())) Then
                        dr3.BeginEdit()
                        dr3.Item(0) = TextBox33.Text
                        dr3.EndEdit()
                        sda3.SelectCommand = cmd3
                        scb3 = New SqlCommandBuilder(sda3)
                        sda3.UpdateCommand = scb3.GetUpdateCommand()
                    End If
                    inc = inc + 1
                    a2 = a2 - 1
                End While

                If ds3.HasChanges Then
                    sda3.Update(ds3, "bill")
                    refreshgrid2()
                End If

                Dim a3 As Integer
                a3 = dt10.Rows.Count
                inc = 0
                While (a3)
                    dr10 = dt10.Rows(inc)
                    If (key.Equals(dr10.Item(1).ToString())) Then
                        dr10.BeginEdit()
                        dr10.Item(1) = TextBox33.Text
                        dr10.EndEdit()
                        sda10.SelectCommand = cmd10
                        scb10 = New SqlCommandBuilder(sda10)
                        sda10.UpdateCommand = scb10.GetUpdateCommand()
                    End If
                    
                    inc = inc + 1
                    a3 = a3 - 1
                End While

                If ds10.HasChanges Then
                    sda10.Update(ds10, "payment")
                    refreshgrid3()
                End If

                a = Dt1.Rows.Count
                inc = 0
                While (a)
                    Dr1 = Dt1.Rows(inc)
                    If (key.Equals(Dr1.Item(1).ToString())) Then
                        Dr1.BeginEdit()
                        Dr1.Item(1) = TextBox33.Text
                        Dr1.EndEdit()
                        Sda1.SelectCommand = Cmd1
                        Scb1 = New SqlCommandBuilder(Sda1)
                        Sda1.UpdateCommand = Scb1.GetUpdateCommand()
                    End If
                    
                    If Ds1.HasChanges Then
                        Sda1.Update(Ds1, "customer")
                        MessageBox.Show("The Customer Name is successfully changed")
                        RefreshList1()
                        Button46.PerformClick()
                        TextBox1.Text = ""
                        TextBox2.Text = ""
                        TextBox3.Text = ""
                        TextBox4.Text = ""
                        TextBox5.Text = ""
                        TextBox6.Text = ""
                        TextBox7.Text = ""
                        TextBox8.Text = ""
                        TextBox9.Text = ""
                        TextBox10.Text = ""
                        TextBox33.Text = ""
                        TextBox11.Text = ""
                        ComboBox1.Text = ""
                        flag = 1
                        ComboBox1.Focus()
                        Exit While
                    End If
                    inc = inc + 1
                    a = a - 1
                End While
                If flag = 0 Then
                    MessageBox.Show(" Cannot Change.. There is no such company Name")
                    ComboBox1.Focus()
                    RefreshList1()
                    refreshgrid1()
                    refreshgrid2()
                    refreshgrid3()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
            RefreshList1()
            refreshgrid1()
            refreshgrid2()
            refreshgrid3()
        End Try
    End Sub

    Private Sub Button46_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button46.Click
        Try
            Label101.Visible = False
            Label102.Visible = False
            TextBox33.Text = ""
            TextBox33.Visible = False
            Button45.Visible = False
            Button46.Visible = False
            Button44.Visible = True
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        Try
            Dim i As Integer
            i = TabControl1.SelectedIndex

            If i = 0 Then
                Button32.PerformClick()
                Button30.PerformClick()
                Button46.PerformClick()
                Button24.PerformClick()
            ElseIf i = 1 Then
                Button35.PerformClick()
            ElseIf i = 2 Then
                Button36.PerformClick()
            ElseIf i = 3 Then
                Button47.PerformClick()
            ElseIf i = 4 Then
                RadioButton7.Checked = True
                ComboBox6.Text = ""
                ComboBox6.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button47_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button47.Click
        ComboBox7.Text = ""
        TextBox25.Text = ""
        TextBox26.Text = ""
        TextBox27.Text = ""
        TextBox28.Text = ""
        TextBox29.Text = ""
        TextBox30.Text = ""
        TextBox23.Text = ""
        TextBox24.Text = ""
        TextBox31.Text = ""
        TextBox32.Text = ""
        TextBox18.Text = ""
        ComboBox7.Focus()
        RadioButton5.Checked = True
    End Sub

    Private Sub TextBox19_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox19.TextChanged

    End Sub

    Private Sub TextBox17_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox17.TextChanged

    End Sub

    Private Sub TextBox33_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox33.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button45.PerformClick()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox33_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox33.TextChanged

    End Sub

    Private Sub ComboBox8_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button48_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub RadioButton8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton8.CheckedChanged
        Try
            If RadioButton8.Checked Then
                PictureBox1.Image = Nothing
                Label53.Text = RadioButton8.Text
                ComboBox6.Visible = False
                ComboBox10.Visible = True
                ComboBox10.Text = ""
                ComboBox10.Focus()
                If CheckBox4.Checked Then
                    Button26.Text = "Search by Bill Number and Date"
                Else
                    Button26.Text = "Search by Bill Number"
                End If

                a = dt3.Rows.Count
                inc = 0
                ComboBox10.Items.Clear()
                While (a)
                    dr3 = dt3.Rows(inc)
                    ComboBox10.Items.Add(dr3.Item(8).ToString + "/" + dr3.Item(1).ToString)
                    inc = inc + 1
                    a = a - 1
                End While
            Else
                ComboBox10.Visible = False
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub RadioButton7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton7.CheckedChanged
        Try
            If RadioButton7.Checked Then
                PictureBox1.Image = Nothing
                Label53.Text = RadioButton7.Text
                ComboBox10.Visible = False
                ComboBox6.Visible = True
                ComboBox6.Text = ""
                ComboBox6.Focus()
                CheckBox1.Enabled = True
                If CheckBox4.Checked Then
                    Button26.Text = "Search by Company Name and Date"
                Else
                    Button26.Text = "Search by Company Name"
                End If
                a = Dt1.Rows.Count
                inc = 0
                ComboBox6.Items.Clear()
                While (a)
                    Dr1 = Dt1.Rows(inc)
                    ComboBox6.Items.Add(Dr1.Item(1).ToString)
                    inc = inc + 1
                    a = a - 1
                End While
            Else
                CheckBox1.Enabled = False
                CheckBox1.Checked = False
                ComboBox6.Visible = False
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub



    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Try
            If CheckBox1.Checked Then
                PictureBox1.Image = Nothing
                ComboBox9.Visible = True
                ComboBox9.Focus()
                If CheckBox4.Checked Then
                    Button26.Text = "Search by Company Name, Design Name and Date"
                Else
                    Button26.Text = "Search by Company Name and Design Name"
                End If

                ComboBox9.Text = ""
                Dim key As String
                key = ComboBox6.Text.Trim
                a = Dt2.Rows.Count
                inc = 0
                ComboBox9.Items.Clear()
                While (a)
                    Dr2 = Dt2.Rows(inc)
                    If (key.Equals(Dr2.Item(0).ToString())) Then
                        newitem = New MyComboitem(Dr2.Item(1), Dr2.Item(2).ToString)
                        ComboBox9.Items.Add(newitem)
                    End If
                    inc = inc + 1
                    a = a - 1
                End While
            Else
                ComboBox9.Visible = False
                If CheckBox4.Checked Then
                    Button26.Text = "Search by Company Name and Date"
                Else
                    Button26.Text = "Search by Company Name"
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub ComboBox9_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboBox9.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button26.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub ComboBox9_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox9.SelectedIndexChanged
        PictureBox1.Image = Nothing
    End Sub

    Private Sub ComboBox9_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox9.VisibleChanged
        If ComboBox9.Visible = False Then
            ComboBox9.Items.Clear()
        End If
    End Sub

    Private Sub Button49_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

   

    Private Sub CheckBox4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked = True And CheckBox1.Checked = False And RadioButton7.Checked = False And RadioButton8.Checked = False Then
            Button26.Text = "Search by Date"
        ElseIf CheckBox4.Checked = True And CheckBox1.Checked = False And RadioButton7.Checked = True And RadioButton8.Checked = False Then
            Button26.Text = "Search by Company Name and Date"
        ElseIf CheckBox4.Checked = True And CheckBox1.Checked = True And RadioButton7.Checked = True And RadioButton8.Checked = False Then
            Button26.Text = "Search by Company Name, Design Name and Date"
        ElseIf CheckBox4.Checked = True And CheckBox1.Checked = False And RadioButton7.Checked = False And RadioButton8.Checked = True Then
            Button26.Text = "Search by Bill Number and Date"
        ElseIf CheckBox4.Checked = False And CheckBox1.Checked = False And RadioButton7.Checked = True And RadioButton8.Checked = False Then
            Button26.Text = "Search by Company Name"
        ElseIf CheckBox4.Checked = False And CheckBox1.Checked = False And RadioButton7.Checked = False And RadioButton8.Checked = True Then
            Button26.Text = "Search by Bill Number"
        ElseIf CheckBox4.Checked = False And CheckBox1.Checked = True And RadioButton7.Checked = True And RadioButton8.Checked = False Then
            Button26.Text = "Search by Company Name and Design Name"
        End If
        If CheckBox4.Checked = True Or CheckBox5.Checked = True Then
            Label54.Visible = True
            Label55.Visible = True
            DateTimePicker3.Visible = True
            DateTimePicker4.Visible = True
        ElseIf CheckBox4.Checked = False And CheckBox5.Checked = False Then
            Label54.Visible = False
            Label55.Visible = False
            DateTimePicker3.Visible = False
            DateTimePicker4.Visible = False
        End If
    End Sub
    Private Sub searchbycompany()
        Try
            compbased = True
            billbased = False
            desbased = False
            datebased = False
            searchboth = False
            If ComboBox6.Text.Trim.Equals("") Then
                MsgBox("Please Select Company ")
                ComboBox6.Focus()
            Else
                Dim totdesamount As Decimal = 0
                Dim countunbilled As Integer = 0
                Dim sumunbilled As Decimal = 0
                Dim countbilled As Integer = 0
                Dim sumbilled As Decimal = 0
                Dim countbill As Integer = 0
                dt6.Clear()
                key = ComboBox6.Text.Trim
                cmprcomp = key
                a = Dt2.Rows.Count - 1
                inc = 0
                While (a >= 0)
                    Dr2 = Dt2.Rows(inc)
                    If key.ToString.Equals(Dr2.Item(0).ToString) Then
                        dr6 = dt6.NewRow
                        dr6.Item(0) = Dr2.Item(0)
                        dr6.Item(1) = Dr2.Item(1)
                        dr6.Item(2) = Dr2.Item(2)
                        dr6.Item(3) = Dr2.Item(3)
                        dr6.Item(4) = Dr2.Item(4)
                        dr6.Item(5) = Dr2.Item(5)
                        dr6.Item(6) = Dr2.Item(6)
                        dr6.Item(7) = Dr2.Item(7)
                        dr6.Item(8) = Dr2.Item(8)
                        dr6.Item(9) = Dr2.Item(9)
                        totdesamount += Decimal.Parse(dr6.Item(9))
                        dr6.Item(10) = Dr2.Item(10)
                        dr6.Item(11) = Dr2.Item(11)
                        dr6.Item(12) = Dr2.Item(12)
                        If Dr2.Item(12) Is DBNull.Value Then
                            countunbilled += 1
                            sumunbilled += Decimal.Parse(Dr2.Item(9))
                        Else
                            countbilled += 1
                            sumbilled += Decimal.Parse(Dr2.Item(9))
                        End If
                        dt6.Rows.Add(dr6)
                    End If
                    a -= 1
                    inc += 1
                End While
                DataGrid3.DataSource = ds6.Tables(0)
                Label48.Text = ds6.Tables(0).Rows.Count.ToString
                Label51.Text = totdesamount.ToString
                Label157.Text = countunbilled.ToString
                Label153.Text = sumunbilled.ToString
                Label151.Text = countbilled.ToString
                Label155.Text = sumbilled.ToString


                Dim tottransamount As Decimal = 0
                dt7.Clear()
                key = ComboBox6.Text.Trim
                a = dt3.Rows.Count - 1
                inc = 0
                While (a >= 0)
                    dr3 = dt3.Rows(inc)
                    If key.ToString.Equals(dr3.Item(0).ToString) Then
                        dr7 = dt7.NewRow
                        dr7.Item(0) = dr3.Item(0)
                        dr7.Item(1) = dr3.Item(1)
                        dr7.Item(2) = dr3.Item(2)
                        dr7.Item(3) = dr3.Item(3)
                        dr7.Item(4) = dr3.Item(4)
                        dr7.Item(5) = dr3.Item(5)
                        dr7.Item(6) = dr3.Item(6)
                        If Not dr7.Item(4) Is DBNull.Value Then
                            tottransamount += Decimal.Parse(dr7.Item(4))
                        End If
                        dr7.Item(7) = dr3.Item(7)
                        dr7.Item(8) = dr3.Item(8)
                        dt7.Rows.Add(dr7)
                        countbill += 1
                    End If
                    a -= 1
                    inc += 1
                End While
                Label113.Text = key
                DataGrid4.DataSource = ds7.Tables(0)
                Label57.Text = ds7.Tables(0).Rows.Count.ToString
                Label61.Text = tottransamount
                Label161.Text = tottransamount
                If Not dt7.Rows.Count = 0 Then
                    Label104.Text = tottransamount - Decimal.Parse(dt7.Rows(dt7.Rows.Count - 1).Item(7))
                End If
                If Not dt7.Rows.Count = 0 Then
                    Label110.Text = dt7.Rows(dt7.Rows.Count - 1).Item(7)
                End If
                Label163.Text = countbill
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
    Private Sub searchbyBillno()
        Try
            compbased = False
            billbased = True
            desbased = False
            datebased = False
            searchboth = False
            If ComboBox10.Text.Trim.Equals("") Then
                MsgBox("Please Select Bill Number")
                ComboBox10.Focus()
            Else
                Dim key As String
                Dim tottransamount As Decimal = 0
                Dim tottransamount1 As Decimal = 0
                key = ComboBox10.Text.Trim
                key = key.Substring(key.IndexOf("/") + 1, key.Length - key.IndexOf("/") - 1)

                Dim totdesamount As Decimal = 0
                Dim custname As String = ""
                Dim countunbilled As Integer = 0
                Dim sumunbilled As Decimal = 0
                Dim countbilled As Integer = 0
                Dim sumbilled As Decimal = 0
                Dim countbill As Integer = 0
                dt6.Clear()
                a = Dt2.Rows.Count - 1
                inc = 0
                While (a >= 0)
                    Dr2 = Dt2.Rows(a)
                    If key.ToString.Equals(Dr2.Item(12).ToString) Then
                        dr6 = dt6.NewRow
                        dr6.Item(0) = Dr2.Item(0)
                        dr6.Item(1) = Dr2.Item(1)
                        dr6.Item(2) = Dr2.Item(2)
                        dr6.Item(3) = Dr2.Item(3)
                        dr6.Item(4) = Dr2.Item(4)
                        dr6.Item(5) = Dr2.Item(5)
                        dr6.Item(6) = Dr2.Item(6)
                        dr6.Item(7) = Dr2.Item(7)
                        dr6.Item(8) = Dr2.Item(8)
                        dr6.Item(9) = Dr2.Item(9)
                        totdesamount += Decimal.Parse(Dr2.Item(9))
                        dr6.Item(10) = Dr2.Item(10)
                        dr6.Item(11) = Dr2.Item(11)
                        dr6.Item(12) = Dr2.Item(12)
                        If Dr2.Item(12) Is DBNull.Value Then
                            countunbilled += 1
                            sumunbilled += Decimal.Parse(Dr2.Item(9))
                        Else
                            countbilled += 1
                            sumbilled += Decimal.Parse(Dr2.Item(9))
                        End If
                        dt6.Rows.Add(dr6)
                    End If
                    a -= 1
                End While
                DataGrid3.DataSource = ds6.Tables(0)
                Label48.Text = ds6.Tables(0).Rows.Count.ToString
                Label51.Text = totdesamount.ToString
                Label157.Text = countunbilled.ToString
                Label153.Text = sumunbilled.ToString
                Label151.Text = countbilled.ToString
                Label155.Text = sumbilled.ToString

                a = dt3.Rows.Count - 1
                dt7.Clear()
                While (a >= 0)
                    dr3 = dt3.Rows(a)
                    If key.Equals(dr3.Item(1).ToString()) Then
                        custname = dr3.Item(0)
                        dr7 = dt7.NewRow
                        dr7.Item(0) = dr3.Item(0)
                        dr7.Item(1) = dr3.Item(1)
                        dr7.Item(2) = dr3.Item(2)
                        dr7.Item(3) = dr3.Item(3)
                        dr7.Item(4) = dr3.Item(4)
                        dr7.Item(5) = dr3.Item(5)
                        dr7.Item(6) = dr3.Item(6)
                        If Not dr7.Item(4) Is DBNull.Value Then
                            tottransamount1 += Decimal.Parse(dr7.Item(4))
                        End If
                        dr7.Item(7) = dr3.Item(7)
                        dr7.Item(8) = dr3.Item(8)
                        dt7.Rows.Add(dr7)
                        Exit While
                    End If
                    a = a - 1
                End While

                Dim flag As Boolean = False
                Dim bal As Decimal = 0
                a = dt3.Rows.Count - 1
                While (a >= 0)
                    dr3 = dt3.Rows(a)
                    If custname.Trim.ToString.Equals(dr3.Item(0).ToString) Then
                        countbill += 1
                        tottransamount += Decimal.Parse(dr3.Item(4))
                        If flag = False Then
                            bal = dr3.Item(7)
                            flag = True
                        End If
                    End If
                    a -= 1
                End While
                cmprcomp = custname
                DataGrid4.DataSource = ds7.Tables(0)
                Label113.Text = custname
                Label57.Text = ds7.Tables(0).Rows.Count.ToString
                Label61.Text = tottransamount
                Label161.Text = tottransamount1
                If Not dt7.Rows.Count = 0 Then
                    Label104.Text = tottransamount - bal
                End If
                If Not dt7.Rows.Count = 0 Then
                    Label110.Text = bal
                End If
                Label163.Text = countbill
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
    Private Sub searchbyDesno()
        Try
            compbased = False
            billbased = False
            desbased = True
            datebased = False
            searchboth = False
            PictureBox1.Image = Nothing
            If ComboBox6.Text.Trim.Equals("") Then
                MsgBox("Please Select Company Name")
                ComboBox6.Focus()
            ElseIf ComboBox9.Text.Trim.Equals("") Then
                MsgBox("Please Select Design Name")
                ComboBox9.Focus()
            Else
                Dim key As Integer
                Dim custname As String = ""
                Dim item As MyComboitem
                Dim totdesamount As Decimal = 0
                Dim tottransamount As Decimal = 0
                Dim tottransamount1 As Decimal = 0
                Dim billnum As Integer = 0
                Dim countunbilled As Integer = 0
                Dim sumunbilled As Decimal = 0
                Dim countbilled As Integer = 0
                Dim sumbilled As Decimal = 0
                Dim countbill As Integer = 0
                item = DirectCast(ComboBox9.SelectedItem, MyComboitem)
                key = item.ID
                b = Dt2.Rows.Count - 1
                inc = 0
                flag = 0
                dt6.Clear()
                While (b >= 0)
                    Dr2 = Dt2.Rows(b)
                    If key = Dr2.Item(1) Then
                        dr6 = dt6.NewRow
                        custname = Dr2.Item(0)
                        dr6.Item(0) = Dr2.Item(0)
                        dr6.Item(1) = Dr2.Item(1)
                        dr6.Item(2) = Dr2.Item(2)
                        dr6.Item(3) = Dr2.Item(3)
                        dr6.Item(4) = Dr2.Item(4)
                        dr6.Item(5) = Dr2.Item(5)
                        dr6.Item(6) = Dr2.Item(6)
                        dr6.Item(7) = Dr2.Item(7)
                        dr6.Item(8) = Dr2.Item(8)
                        dr6.Item(9) = Dr2.Item(9)
                        totdesamount += Decimal.Parse(dr6.Item(9))
                        dr6.Item(10) = Dr2.Item(10)
                        dr6.Item(11) = Dr2.Item(11)
                        dr6.Item(12) = Dr2.Item(12)
                        billnum = Dr2.Item(12)
                        If Dr2.Item(12) Is DBNull.Value Then
                            countunbilled += 1
                            sumunbilled += Decimal.Parse(Dr2.Item(9))
                        Else
                            countbilled += 1
                            sumbilled += Decimal.Parse(Dr2.Item(9))
                        End If
                        dt6.Rows.Add(dr6)
                        Exit While
                    End If
                    b = b - 1
                End While
                DataGrid3.DataSource = ds6.Tables(0)
                Label48.Text = ds6.Tables(0).Rows.Count.ToString
                Label51.Text = totdesamount.ToString
                Label157.Text = countunbilled.ToString
                Label153.Text = sumunbilled.ToString
                Label151.Text = countbilled.ToString
                Label155.Text = sumbilled.ToString

                a = dt3.Rows.Count - 1
                dt7.Clear()
                While (a >= 0)
                    dr3 = dt3.Rows(a)
                    If billnum.ToString.Equals(dr3.Item(1).ToString()) Then
                        dr7 = dt7.NewRow
                        custname = dr3.Item(0)
                        dr7.Item(0) = dr3.Item(0)
                        dr7.Item(1) = dr3.Item(1)
                        dr7.Item(2) = dr3.Item(2)
                        dr7.Item(3) = dr3.Item(3)
                        dr7.Item(4) = dr3.Item(4)
                        dr7.Item(5) = dr3.Item(5)
                        dr7.Item(6) = dr3.Item(6)
                        If Not dr7.Item(4) Is DBNull.Value Then
                            tottransamount1 += Decimal.Parse(dr7.Item(4))
                        End If
                        dr7.Item(7) = dr3.Item(7)
                        dr7.Item(8) = dr3.Item(8)
                        dt7.Rows.Add(dr7)
                        Exit While
                    End If
                    a = a - 1
                End While

                Dim flag1 As Boolean = False
                Dim bal As Decimal = 0
                a = dt3.Rows.Count - 1
                While (a >= 0)
                    dr3 = dt3.Rows(a)
                    If custname.Trim.ToString.Equals(dr3.Item(0).ToString) Then
                        tottransamount += Decimal.Parse(dr3.Item(4))
                        countbill += 1
                        If flag1 = False Then
                            bal = dr3.Item(7)
                            flag1 = True
                        End If
                    End If
                    a -= 1
                End While
                cmprcomp = custname

                DataGrid4.DataSource = ds7.Tables(0)
                Label57.Text = ds7.Tables(0).Rows.Count.ToString
                Label61.Text = tottransamount
                Label161.Text = tottransamount1
                If Not dt7.Rows.Count = 0 Then
                    Label104.Text = tottransamount - bal
                End If
                If Not dt7.Rows.Count = 0 Then
                    Label110.Text = bal
                End If
                Label163.Text = countbill
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
    Private Sub searchbycompdate()

    End Sub
    Private Sub searchbyBilldate()

    End Sub
    Private Sub searchbydesdate()

    End Sub
    Private Sub searchbydate()
        Try
            compbased = False
            billbased = False
            desbased = False
            datebased = True
            searchboth = False
            PictureBox1.Image = Nothing
            If DateTimePicker3.Text.Trim.Equals("") Then
                MsgBox("Please Select From Date ")
                DateTimePicker3.Focus()
            ElseIf DateTimePicker4.Text.Trim.Equals("") Then
                MsgBox("Please Select To Date ")
                DateTimePicker4.Focus()
            Else
                Dim countunbilled As Integer = 0
                Dim sumunbilled As Decimal = 0
                Dim countbilled As Integer = 0
                Dim sumbilled As Decimal = 0
                Dim countbill As Integer = 0
                dt6.Clear()
                Dim totdesamount As Decimal = 0
                Dim fromdate, todate, billdate, desdate As Date
                Dim fromdatestr, todatestr, billdatestr As String
                fromdatestr = DateTimePicker3.Value.ToString("MM dd yyyy")
                fromdate = DateTime.Parse(fromdatestr)
                todatestr = DateTimePicker4.Value.ToString("MM dd yyyy")
                todate = DateTime.Parse(todatestr)
                a = Dt2.Rows.Count - 1
                inc = 0
                While (a >= 0)
                    Dr2 = Dt2.Rows(inc)
                    desdate = DateTime.Parse(Dr2.Item(10))
                    billdatestr = desdate.ToString("MM dd yyyy")
                    desdate = DateTime.Parse(billdatestr)
                    If desdate >= fromdate And desdate <= todate Then
                        dr6 = dt6.NewRow
                        dr6.Item(0) = Dr2.Item(0)
                        dr6.Item(1) = Dr2.Item(1)
                        dr6.Item(2) = Dr2.Item(2)
                        dr6.Item(3) = Dr2.Item(3)
                        dr6.Item(4) = Dr2.Item(4)
                        dr6.Item(5) = Dr2.Item(5)
                        dr6.Item(6) = Dr2.Item(6)
                        dr6.Item(7) = Dr2.Item(7)
                        dr6.Item(8) = Dr2.Item(8)
                        dr6.Item(9) = Dr2.Item(9)
                        totdesamount += Decimal.Parse(dr6.Item(9))
                        dr6.Item(10) = Dr2.Item(10)
                        dr6.Item(11) = Dr2.Item(11)
                        dr6.Item(12) = Dr2.Item(12)
                        If Dr2.Item(12) Is DBNull.Value Then
                            countunbilled += 1
                            sumunbilled += Decimal.Parse(Dr2.Item(9))
                        Else
                            countbilled += 1
                            sumbilled += Decimal.Parse(Dr2.Item(9))
                        End If
                        dt6.Rows.Add(dr6)
                    End If
                    a -= 1
                    inc += 1
                End While

                DataGrid3.DataSource = ds6.Tables(0)
                Label48.Text = ds6.Tables(0).Rows.Count.ToString
                Label51.Text = totdesamount.ToString
                Label157.Text = countunbilled.ToString
                Label153.Text = sumunbilled.ToString
                Label151.Text = countbilled.ToString
                Label155.Text = sumbilled.ToString

                Dim custname As String = ""
                dt7.Clear()
                Dim tottransamount As Decimal = 0
                Dim tottransamount1 As Decimal = 0
                fromdatestr = DateTimePicker3.Value.ToString("MM dd yyyy")
                fromdate = DateTime.Parse(fromdatestr)
                cmprfromdate = fromdate
                todatestr = DateTimePicker4.Value.ToString("MM dd yyyy")
                todate = DateTime.Parse(todatestr)
                cmprtodate = todate
                a = dt3.Rows.Count - 1
                inc = 0
                While (a >= 0)
                    dr3 = dt3.Rows(inc)
                    billdate = DateTime.Parse(dr3.Item(2))
                    billdatestr = billdate.ToString("MM dd yyyy")
                    billdate = DateTime.Parse(billdatestr)
                    If billdate >= fromdate And billdate <= todate Then
                        dr7 = dt7.NewRow
                        custname = dr3.Item(0)
                        dr7.Item(0) = dr3.Item(0)
                        dr7.Item(1) = dr3.Item(1)
                        dr7.Item(2) = dr3.Item(2)
                        dr7.Item(3) = dr3.Item(3)
                        dr7.Item(4) = dr3.Item(4)
                        dr7.Item(5) = dr3.Item(5)
                        dr7.Item(6) = dr3.Item(6)
                        If Not dr7.Item(4) Is DBNull.Value Then
                            tottransamount1 += Decimal.Parse(dr7.Item(4))
                        End If
                        dr7.Item(7) = dr3.Item(7)
                        dr7.Item(8) = dr3.Item(8)
                        dt7.Rows.Add(dr7)
                    End If
                    a -= 1
                    inc += 1
                End While

                Label113.Text = "Mixed (Not Specific)"
                DataGrid4.DataSource = ds7.Tables(0)
                Label57.Text = ds7.Tables(0).Rows.Count.ToString
                Label61.Text = "No"
                Label161.Text = tottransamount1
                Label163.Text = "No"
                Label104.Text = "No"
                Label110.Text = "No"
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
    Private Sub Button26_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button26.Click
        Try
            PictureBox1.Image = Nothing
            If Button26.Text.Trim.Equals("Search by Company Name") Then
                searchbycompany()
            ElseIf Button26.Text.Trim.Equals("Search by Company Name and Design Name") Then
                searchbyDesno()
            ElseIf Button26.Text.Trim.Equals("Search by Bill Number") Then
                searchbyBillno()
            ElseIf Button26.Text.Trim.Equals("Search by Date") Then
                searchbydate()
            ElseIf Button26.Text.Trim.Equals("Search by Company Name and Date") Then
                compbased = False
                billbased = False
                desbased = False
                datebased = False
                searchboth = True
                If ComboBox6.Text.Trim.Equals("") Then
                    MsgBox("Please Select Company ")
                    ComboBox6.Focus()
                ElseIf DateTimePicker3.Text.Trim.Equals("") Then
                    MsgBox("Please Select From Date ")
                    DateTimePicker3.Focus()
                ElseIf DateTimePicker4.Text.Trim.Equals("") Then
                    MsgBox("Please Select To Date ")
                    DateTimePicker4.Focus()
                Else
                    searchmore(ComboBox6.Text.Trim, 0, 0)
                End If
            ElseIf Button26.Text.Trim.Equals("Search by Bill Number and Date") Then
                compbased = False
                billbased = False
                desbased = False
                datebased = False
                searchboth = True
                If ComboBox10.Text.Trim.Equals("") Then
                    MsgBox("Please Select Bill Number")
                    ComboBox10.Focus()
                ElseIf DateTimePicker3.Text.Trim.Equals("") Then
                    MsgBox("Please Select From Date ")
                    DateTimePicker3.Focus()
                ElseIf DateTimePicker4.Text.Trim.Equals("") Then
                    MsgBox("Please Select To Date ")
                    DateTimePicker4.Focus()
                Else
                    key = ComboBox10.Text.Trim
                    key = key.Substring(key.IndexOf("/") + 1, key.Length - key.IndexOf("/") - 1)
                    searchmore(key, 12, 1)
                End If
            ElseIf Button26.Text.Trim.Equals("Search by Company Name, Design Name and Date") Then
                compbased = False
                billbased = False
                desbased = False
                datebased = False
                searchboth = True
                If ComboBox6.Text.Trim.Equals("") Then
                    MsgBox("Please Select Company Name")
                    ComboBox6.Focus()
                ElseIf ComboBox9.Text.Trim.Equals("") Then
                    MsgBox("Please Select Design Name")
                    ComboBox9.Focus()
                ElseIf DateTimePicker3.Text.Trim.Equals("") Then
                    MsgBox("Please Select From Date ")
                    DateTimePicker3.Focus()
                ElseIf DateTimePicker4.Text.Trim.Equals("") Then
                    MsgBox("Please Select To Date ")
                    DateTimePicker4.Focus()
                Else
                    Dim item As MyComboitem
                    item = DirectCast(ComboBox9.SelectedItem, MyComboitem)
                    key = item.ID
                    searchmoredes(key.ToString, 1, 1)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
    Sub searchmore(ByVal key As String, ByVal descol As Int16, ByVal billcol As Int16)
        dt6.Clear()
        Dim totdesamount As Decimal = 0
        Dim custname As String = ""
        Dim countunbilled As Integer = 0
        Dim sumunbilled As Decimal = 0
        Dim countbilled As Integer = 0
        Dim sumbilled As Decimal = 0
        Dim countbill As Integer = 0
        Dim fromdate, todate, billdate, desdate As Date
        Dim fromdatestr, todatestr, billdatestr As String
        fromdatestr = DateTimePicker3.Value.ToString("MM dd yyyy")
        fromdate = DateTime.Parse(fromdatestr)
        todatestr = DateTimePicker4.Value.ToString("MM dd yyyy")
        todate = DateTime.Parse(todatestr)
        a = Dt2.Rows.Count - 1
        inc = 0
        cmprcomp = key
        While (a >= 0)
            Dr2 = Dt2.Rows(inc)
            desdate = DateTime.Parse(Dr2.Item(10))
            billdatestr = desdate.ToString("MM dd yyyy")
            desdate = DateTime.Parse(billdatestr)
            If key.ToString.Equals(Dr2.Item(descol).ToString) Then
                custname = Dr2.Item(0)
                If desdate >= fromdate And desdate <= todate Then
                    dr6 = dt6.NewRow
                    dr6.Item(0) = Dr2.Item(0)
                    dr6.Item(1) = Dr2.Item(1)
                    dr6.Item(2) = Dr2.Item(2)
                    dr6.Item(3) = Dr2.Item(3)
                    dr6.Item(4) = Dr2.Item(4)
                    dr6.Item(5) = Dr2.Item(5)
                    dr6.Item(6) = Dr2.Item(6)
                    dr6.Item(8) = Dr2.Item(8)
                    dr6.Item(9) = Dr2.Item(9)
                    totdesamount += Decimal.Parse(dr6.Item(9))
                    dr6.Item(7) = Dr2.Item(7)
                    dr6.Item(10) = Dr2.Item(10)
                    dr6.Item(11) = Dr2.Item(11)
                    dr6.Item(12) = Dr2.Item(12)
                    If Dr2.Item(12) Is DBNull.Value Then
                        countunbilled += 1
                        sumunbilled += Decimal.Parse(Dr2.Item(9))
                    Else
                        countbilled += 1
                        sumbilled += Decimal.Parse(Dr2.Item(9))
                    End If
                    dt6.Rows.Add(dr6)
                End If
            End If
            a -= 1
            inc += 1
        End While
        DataGrid3.DataSource = ds6.Tables(0)
        Label48.Text = ds6.Tables(0).Rows.Count.ToString
        Label51.Text = totdesamount.ToString
        Label157.Text = countunbilled.ToString
        Label153.Text = sumunbilled.ToString
        Label151.Text = countbilled.ToString
        Label155.Text = sumbilled.ToString

        dt7.Clear()
        Dim tottransamount As Decimal = 0
        Dim tottransamount1 As Decimal = 0

        fromdatestr = DateTimePicker3.Value.ToString("MM dd yyyy")
        fromdate = DateTime.Parse(fromdatestr)
        cmprfromdate = fromdate
        todatestr = DateTimePicker4.Value.ToString("MM dd yyyy")
        todate = DateTime.Parse(todatestr)
        cmprtodate = todate
        a = dt3.Rows.Count - 1
        inc = 0
        While (a >= 0)
            dr3 = dt3.Rows(inc)
            billdate = DateTime.Parse(dr3.Item(2))
            billdatestr = billdate.ToString("MM dd yyyy")
            billdate = DateTime.Parse(billdatestr)
            If key.ToString.Equals(dr3.Item(billcol).ToString) Then
                custname = dr3.Item(0)
                If billdate >= fromdate And billdate <= todate Then
                    dr7 = dt7.NewRow
                    dr7.Item(0) = dr3.Item(0)
                    dr7.Item(1) = dr3.Item(1)
                    dr7.Item(2) = dr3.Item(2)
                    dr7.Item(3) = dr3.Item(3)
                    dr7.Item(4) = dr3.Item(4)
                    dr7.Item(5) = dr3.Item(5)
                    dr7.Item(6) = dr3.Item(6)
                    If Not dr7.Item(4) Is DBNull.Value Then
                        tottransamount1 += Decimal.Parse(dr7.Item(4))
                    End If
                    dr7.Item(7) = dr3.Item(7)
                    dr7.Item(8) = dr3.Item(8)
                    dt7.Rows.Add(dr7)
                    Exit While
                End If
            End If
            a -= 1
            inc += 1
        End While

        Dim flag As Boolean = False
        Dim bal As Decimal = 0
        a = dt3.Rows.Count - 1
        While (a >= 0)
            dr3 = dt3.Rows(a)
            If custname.Trim.ToString.Equals(dr3.Item(0).ToString) Then
                tottransamount += Decimal.Parse(dr3.Item(4))
                countbill += 1
                If flag = False Then
                    bal = dr3.Item(7)
                    flag = True
                End If
            End If
            a -= 1
        End While
        cmprcomp = custname
        Label113.Text = custname
        DataGrid4.DataSource = Nothing
        DataGrid4.DataSource = ds7.Tables(0)
        Label57.Text = ds7.Tables(0).Rows.Count.ToString
        Label61.Text = tottransamount
        Label161.Text = tottransamount1
        Label163.Text = countbill
        If Not dt7.Rows.Count = 0 Then
            Label104.Text = tottransamount - bal
        End If
        If Not dt7.Rows.Count = 0 Then
            Label110.Text = bal
        End If
    End Sub
    Sub searchmoredes(ByVal key As String, ByVal descol As Int16, ByVal billcol As Int16)
        dt6.Clear()
        Dim totdesamount As Decimal = 0
        Dim fromdate, todate, billdate, desdate As Date
        Dim fromdatestr, todatestr, billdatestr As String
        Dim countunbilled As Integer = 0
        Dim sumunbilled As Decimal = 0
        Dim countbilled As Integer = 0
        Dim sumbilled As Decimal = 0
        Dim countbill As Integer = 0
        Dim billnum As Integer = 0
        Dim custname As String = ""
        fromdatestr = DateTimePicker3.Value.ToString("MM dd yyyy")
        fromdate = DateTime.Parse(fromdatestr)
        todatestr = DateTimePicker4.Value.ToString("MM dd yyyy")
        todate = DateTime.Parse(todatestr)
        a = Dt2.Rows.Count - 1
        inc = 0
        cmprcomp = key
        While (a >= 0)
            Dr2 = Dt2.Rows(inc)
            desdate = DateTime.Parse(Dr2.Item(10))
            billdatestr = desdate.ToString("MM dd yyyy")
            desdate = DateTime.Parse(billdatestr)
            If key.ToString.Equals(Dr2.Item(descol).ToString) Then
                custname = Dr2.Item(0)
                If desdate >= fromdate And desdate <= todate Then
                    dr6 = dt6.NewRow
                    dr6.Item(0) = Dr2.Item(0)
                    dr6.Item(1) = Dr2.Item(1)
                    dr6.Item(2) = Dr2.Item(2)
                    dr6.Item(3) = Dr2.Item(3)
                    dr6.Item(4) = Dr2.Item(4)
                    dr6.Item(5) = Dr2.Item(5)
                    dr6.Item(6) = Dr2.Item(6)
                    dr6.Item(8) = Dr2.Item(8)
                    dr6.Item(9) = Dr2.Item(9)
                    totdesamount += Decimal.Parse(dr6.Item(9))
                    dr6.Item(7) = Dr2.Item(7)
                    dr6.Item(10) = Dr2.Item(10)
                    dr6.Item(11) = Dr2.Item(11)
                    dr6.Item(12) = Dr2.Item(12)
                    If Dr2.Item(12) Is DBNull.Value Then
                        countunbilled += 1
                        sumunbilled += Decimal.Parse(Dr2.Item(9))
                    Else
                        countbilled += 1
                        sumbilled += Decimal.Parse(Dr2.Item(9))
                    End If
                    billnum = Dr2.Item(12)
                    dt6.Rows.Add(dr6)
                End If
            End If
            a -= 1
            inc += 1
        End While
        DataGrid3.DataSource = ds6.Tables(0)
        Label48.Text = ds6.Tables(0).Rows.Count.ToString
        Label51.Text = totdesamount.ToString

        dt7.Clear()
        Dim tottransamount As Decimal = 0
        Dim tottransamount1 As Decimal = 0

        fromdatestr = DateTimePicker3.Value.ToString("MM dd yyyy")
        fromdate = DateTime.Parse(fromdatestr)
        cmprfromdate = fromdate
        todatestr = DateTimePicker4.Value.ToString("MM dd yyyy")
        todate = DateTime.Parse(todatestr)
        cmprtodate = todate
        a = dt3.Rows.Count - 1
        inc = 0
        While (a >= 0)
            dr3 = dt3.Rows(inc)
            billdate = DateTime.Parse(dr3.Item(2))
            billdatestr = billdate.ToString("MM dd yyyy")
            billdate = DateTime.Parse(billdatestr)
            If billnum.ToString.Equals(dr3.Item(billcol).ToString) Then
                custname = dr3.Item(0)
                If billdate >= fromdate And billdate <= todate Then
                    dr7 = dt7.NewRow
                    dr7.Item(0) = dr3.Item(0)
                    dr7.Item(1) = dr3.Item(1)
                    dr7.Item(2) = dr3.Item(2)
                    dr7.Item(3) = dr3.Item(3)
                    dr7.Item(4) = dr3.Item(4)
                    dr7.Item(5) = dr3.Item(5)
                    dr7.Item(6) = dr3.Item(6)
                    If Not dr7.Item(4) Is DBNull.Value Then
                        tottransamount1 += Decimal.Parse(dr7.Item(4))
                    End If
                    dr7.Item(7) = dr3.Item(7)
                    dr7.Item(8) = dr3.Item(8)
                    dt7.Rows.Add(dr7)
                    Exit While
                End If
            End If
            a -= 1
            inc += 1
        End While

        Dim flag As Boolean = False
        Dim bal As Decimal = 0
        a = dt3.Rows.Count - 1
        While (a >= 0)
            dr3 = dt3.Rows(a)
            If custname.ToString.Equals(dr3.Item(0).ToString) Then
                tottransamount += Decimal.Parse(dr3.Item(4))
                countbill += 1
                If flag = False Then
                    bal = dr3.Item(7)
                    flag = True
                End If
            End If
            a -= 1
        End While
        cmprcomp = custname
        DataGrid4.DataSource = Nothing
        DataGrid4.DataSource = ds7.Tables(0)
        Label113.Text = custname
        Label57.Text = ds7.Tables(0).Rows.Count.ToString
        Label61.Text = tottransamount
        Label161.Text = tottransamount1
        Label163.Text = countbill
        If Not dt7.Rows.Count = 0 Then
            Label104.Text = tottransamount - bal
        End If
        If Not dt7.Rows.Count = 0 Then
            Label110.Text = bal
        End If
    End Sub

    Private Sub ComboBox10_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboBox10.KeyDown
        Try
            If (e.KeyCode = Keys.Enter) Then
                Button26.PerformClick()
            ElseIf e.Alt Then
                If e.KeyCode = Keys.C Then
                    TabControl1.SelectTab(0)
                ElseIf e.KeyCode = Keys.D Then
                    TabControl1.SelectTab(1)
                ElseIf e.KeyCode = Keys.B Then
                    TabControl1.SelectTab(2)
                ElseIf e.KeyCode = Keys.P Then
                    TabControl1.SelectTab(3)
                ElseIf e.KeyCode = Keys.H Then
                    TabControl1.SelectTab(5)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub TabControl1_DrawItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles TabControl1.DrawItem
        Dim BackBrush As Brush = New SolidBrush(System.Drawing.Color.BurlyWood)
        Dim ForeBrush As Brush = New SolidBrush(System.Drawing.Color.Black)
        Dim tabBackgroundRect As Rectangle = e.Bounds
        e.Graphics.FillRectangle(BackBrush, tabBackgroundRect)
        e.Graphics.DrawString(TabControl1.TabPages(e.Index).Text, e.Font, ForeBrush, tabBackgroundRect)

        Dim tabstripEndRect As Rectangle = TabControl1.GetTabRect(TabControl1.TabPages.Count - 1)
        Dim tabstripEndRectF As RectangleF = New RectangleF(tabstripEndRect.X + tabstripEndRect.Width, tabstripEndRect.Y - 5, TabControl1.Width - (tabstripEndRect.X + tabstripEndRect.Width), tabstripEndRect.Height + 5)
        'Using backBrush1 As Brush = New SolidBrush(System.Drawing.Color.BlueViolet)
        e.Graphics.FillRectangle(BackBrush, tabstripEndRectF)
        ' End Using
    End Sub

    Private Sub ComboBox10_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox10.SelectedIndexChanged

    End Sub

    Private Sub CheckBox5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox5.CheckedChanged
        Try
            If CheckBox5.Checked = True Then
                RadioButton7.Checked = False
                RadioButton8.Checked = False
                RadioButton7.Enabled = False
                RadioButton8.Enabled = False
                Label53.Visible = False
                CheckBox4.Checked = False
                CheckBox4.Enabled = False
                Label54.Visible = True
                Label55.Visible = True
                DateTimePicker3.Visible = True
                DateTimePicker4.Visible = True
                Button26.Text = "Search by Date"
            Else
                RadioButton7.Enabled = True
                RadioButton8.Enabled = True
                RadioButton7.Checked = True
                Label53.Visible = True
                CheckBox4.Enabled = True
                Label54.Visible = False
                Label55.Visible = False
                DateTimePicker3.Visible = False
                DateTimePicker4.Visible = False
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

   
    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        'Try
        '    Dim key As Integer
        '    Dim item As MyComboitem
        '    item = DirectCast(ComboBox3.SelectedItem, MyComboitem)
        '    key = item.ID
        '    desIdOpr = key
        '    MsgBox(desIdOpr.ToString)
        '    b = Dt2.Rows.Count
        '    inc = 0
        '    flag = 0
        '    While (b)
        '        Dr2 = Dt2.Rows(inc)
        '        If key = Dr2.Item(1) Then
        '            flag = 1
        '            TextBox12.Text = Dr2.Item(3)
        '            TextBox13.Text = Dr2.Item(4)
        '            TextBox14.Text = Dr2.Item(5)
        '            TextBox15.Text = Dr2.Item(6)
        '            If Dr2.Item(7).ToString.Equals("Design") Then
        '                RadioButton1.Checked = True
        '            Else
        '                RadioButton2.Checked = True
        '            End If
        '            If Dr2.Item(11).ToString.Equals("Paid") Then
        '                RadioButton3.Checked = True
        '            Else
        '                RadioButton4.Checked = True
        '            End If
        '            If Dr2("image") Is DBNull.Value Then
        '                PictureBox2.Image = Nothing
        '            Else
        '                Dim arrayImage() As Byte = CType(Dr2("image"), Byte())
        '                Dim ms As New MemoryStream(arrayImage)
        '                PictureBox2.Image = Image.FromStream(ms)
        '            End If

        '            TextBox16.Text = Dr2.Item(9)
        '            ComboBox2.Text = ""
        '            ComboBox2.SelectedText = Dr2.Item(0).ToString
        '            DateTimePicker2.Text = Dr2.Item(10)
        '            TextBox12.Focus()
        '            Exit While
        '        End If
        '        inc = inc + 1
        '        b = b - 1
        '    End While
        '    If flag = 0 Then
        '        ComboBox3.Text = ""
        '        TextBox12.Text = ""
        '        TextBox13.Text = ""
        '        TextBox14.Text = ""
        '        'TextBox15.Text = ""
        '        RadioButton1.Checked = True
        '        PictureBox2.Image = Nothing
        '        TextBox16.Text = ""
        '        ComboBox2.Text = ""
        '    End If
        'Catch ex As Exception
        '    MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End Try
    End Sub

    Private Sub ComboBox3_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectionChangeCommitted
        Try
            Dim key As Integer
            Dim item As MyComboitem
            item = DirectCast(ComboBox3.SelectedItem, MyComboitem)
            key = item.ID
            desIdOpr = key
            b = Dt2.Rows.Count
            inc = 0
            flag = 0
            While (b)
                Dr2 = Dt2.Rows(inc)
                If key = Dr2.Item(1) Then
                    flag = 1
                    TextBox12.Text = Dr2.Item(3)
                    TextBox13.Text = Dr2.Item(4)
                    TextBox14.Text = Dr2.Item(5)
                    TextBox15.Text = Dr2.Item(6)
                    If Dr2.Item(7).ToString.Equals("Design") Then
                        RadioButton1.Checked = True
                    Else
                        RadioButton2.Checked = True
                    End If
                    If Dr2.Item(11).ToString.Equals("Paid") Then
                        RadioButton3.Checked = True
                    Else
                        RadioButton4.Checked = True
                    End If
                    If Dr2("image") Is DBNull.Value Then
                        PictureBox2.Image = Nothing
                    Else
                        Dim arrayImage() As Byte = CType(Dr2("image"), Byte())
                        Dim ms As New MemoryStream(arrayImage)
                        PictureBox2.Image = Image.FromStream(ms)
                    End If

                    TextBox16.Text = Dr2.Item(9)
                    ComboBox2.Text = ""
                    ComboBox2.SelectedText = Dr2.Item(0).ToString
                    DateTimePicker2.Text = Dr2.Item(10)
                    TextBox12.Focus()
                    Exit While
                End If
                inc = inc + 1
                b = b - 1
            End While
            If flag = 0 Then
                ComboBox3.Text = ""
                TextBox12.Text = ""
                TextBox13.Text = ""
                TextBox14.Text = ""
                'TextBox15.Text = ""
                RadioButton1.Checked = True
                PictureBox2.Image = Nothing
                TextBox16.Text = ""
                ComboBox2.Text = ""
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub
End Class


