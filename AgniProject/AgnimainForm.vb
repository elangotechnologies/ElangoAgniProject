Imports System.Data.SqlClient
Imports System.IO
Imports System.Math
Imports NLog

Public Class AgnimainForm
    Dim dbConnection As SqlConnection

    Dim log = LogManager.GetCurrentClassLogger()

    Dim Cmd1, cmd2, cmd3, cmd10 As SqlCommand
    Dim Sda1, Sda2, sda3, sda10 As SqlDataAdapter
    Dim Ds1, Ds2, ds3, ds11 As DataSet
    Dim customerTable, Dt2, dt3, dt11 As DataTable
    Dim DataRow, Dr2, dr3, dr11, MyRow As DataRow
    Dim Dc1, Dc2, dc3 As DataColumn
    Dim Scb1, Scb2, scb3, scb10 As SqlCommandBuilder
    Dim rwindex As Integer = 0
    Public rowCount, b, c, inc As Integer
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
        'Try
        If MessageBox.Show("This will close the program." + vbNewLine + "Are you really want to close?", "Application Closing", System.Windows.Forms.MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
            e.Cancel = True
        Else
            Login.Close()
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ''Try
        log.Debug("Form is loading")

        tabAllTabsHolder.Width = Me.Width
        tabAllTabsHolder.Height = Me.Height
        dpBillingBillDate.Value = DateTime.Today
        cmbCustCompanyList.Focus()

        dbConnection = New SqlConnection("Data Source=agni;Initial Catalog=agnidatabase;Integrated Security=True")
        dbConnection.Open()

        loadOrReloadCustomerList()

        'cmd3 = New SqlCommand("select * from bill", dbConnection)
        'sda3 = New SqlDataAdapter()
        'sda3.SelectCommand = cmd3
        'ds3 = New DataSet
        'sda3.Fill(ds3, "bill")
        'dt3 = ds3.Tables(0)

        'cmd10 = New SqlCommand("select * from payment", dbConnection)
        'sda10 = New SqlDataAdapter()
        'sda10.SelectCommand = cmd10
        'ds10 = New DataSet
        'sda10.Fill(ds10, "payment")
        'dt10 = ds10.Tables(0)

        'DataGrid2.DataSource = ds3.Tables(0)
        'Dim gridstyle2 As New DataGridTableStyle
        'gridstyle2.MappingName = "bill"
        'Dim colStyle1(10) As DataGridTextBoxColumn
        'For i = 0 To 8
        '    colStyle1(i) = New DataGridTextBoxColumn
        'Next
        'With colStyle1(0)
        '    .MappingName = "CompName"
        '    .HeaderText = "Company Name"
        '    .Width = 250
        'End With
        'With colStyle1(8)
        '    .MappingName = "Year"
        '    .HeaderText = "Bill"
        '    .Width = 30
        '    .Alignment = HorizontalAlignment.Right
        'End With
        'With colStyle1(1)
        '    .MappingName = "BillNo"
        '    .HeaderText = "No. "
        '    .Alignment = HorizontalAlignment.Left
        '    .Width = 90
        'End With
        'With colStyle1(2)
        '    .MappingName = "BillDate"
        '    .Format = "MMM dd, yyyy"
        '    .HeaderText = "Bill Date"
        '    .Width = 130
        'End With
        'With colStyle1(3)
        '    .MappingName = "prebalance"
        '    .HeaderText = "Previous Balance"
        '    .Alignment = HorizontalAlignment.Right
        '    .Width = 180
        'End With
        'With colStyle1(4)
        '    .MappingName = "descost"
        '    .HeaderText = "Design Cost"
        '    .Alignment = HorizontalAlignment.Right
        '    .Width = 180
        'End With
        'With colStyle1(5)
        '    .MappingName = "TotAmount"
        '    .HeaderText = "Design + Balance"
        '    .Alignment = HorizontalAlignment.Right
        '    .Width = 200
        'End With
        'With colStyle1(6)
        '    .MappingName = "Paid"
        '    .HeaderText = "Paid"
        '    .Alignment = HorizontalAlignment.Right
        '    .Width = 140
        'End With
        'With colStyle1(7)
        '    .MappingName = "curBalance"
        '    .HeaderText = "Current Balance"
        '    .Alignment = HorizontalAlignment.Right
        '    .Width = 160
        'End With
        'With gridstyle2.GridColumnStyles
        '    .Add(colStyle1(0))
        '    .Add(colStyle1(8))
        '    .Add(colStyle1(1))
        '    .Add(colStyle1(2))
        '    .Add(colStyle1(3))
        '    .Add(colStyle1(4))
        '    .Add(colStyle1(5))
        '    '.Add(colStyle1(6))
        '    .Add(colStyle1(7))
        'End With
        'DataGrid2.TableStyles.Add(gridstyle2)
        'gridstyle2.HeaderBackColor = Color.MidnightBlue
        'gridstyle2.HeaderForeColor = Color.White
        'gridstyle2.GridLineColor = Color.RoyalBlue
        'gridstyle2.AllowSorting = False

        'ds6.Tables.Add(dt6)
        'dc6(0) = New DataColumn("CompName", Type.GetType("System.String"))
        'dc6(1) = New DataColumn("DesnID", Type.GetType("System.Int32"))
        'dc6(2) = New DataColumn("DesnName", Type.GetType("System.String"))
        'dc6(3) = New DataColumn("Height", Type.GetType("System.Decimal"))
        'dc6(4) = New DataColumn("Width", Type.GetType("System.Decimal"))
        'dc6(5) = New DataColumn("Colors", Type.GetType("System.Decimal"))
        'dc6(6) = New DataColumn("UnitCost", Type.GetType("System.Decimal"))
        'dc6(7) = New DataColumn("Type", Type.GetType("System.String"))
        'dc6(8) = New DataColumn("Image", Type.GetType("System.Byte[]"))
        'dc6(9) = New DataColumn("Price", Type.GetType("System.Decimal"))
        'dc6(10) = New DataColumn("DesignDate", Type.GetType("System.DateTime"))
        'dc6(11) = New DataColumn("isPaid", Type.GetType("System.String"))
        'dc6(12) = New DataColumn("ForBill", Type.GetType("System.Int32"))
        'For i = 0 To 12
        '    dt6.Columns.Add(dc6(i))
        'Next
        'DataGrid3.DataSource = ds6.Tables(0)
        'ds6.Tables(0).TableName = "design"
        'Dim gridstyle3 As New DataGridTableStyle
        'gridstyle3.MappingName = "design"
        'Dim colStyle2(12) As DataGridTextBoxColumn
        'For i = 0 To 12
        '    colStyle2(i) = New DataGridTextBoxColumn
        'Next
        'With colStyle2(0)
        '    .MappingName = "CompName"
        '    .HeaderText = "Company Name"
        '    .Width = 180
        'End With
        'With colStyle2(1)
        '    .MappingName = "DesnID"
        '    .HeaderText = "Des. ID "
        '    .Width = 60
        'End With
        'With colStyle2(2)
        '    .MappingName = "DesnName"
        '    .HeaderText = "Design Name"
        '    .Width = 120
        'End With
        'With colStyle2(3)
        '    .MappingName = "Height"
        '    .HeaderText = "Height"
        '    .Width = 70
        'End With
        'With colStyle2(4)
        '    .MappingName = "Width"
        '    .HeaderText = "Width"
        '    .Width = 70
        'End With
        'With colStyle2(5)
        '    .MappingName = "Colors"
        '    .HeaderText = "Colours"
        '    .Width = 70
        'End With
        'With colStyle2(6)
        '    .MappingName = "UnitCost"
        '    .HeaderText = "Cost/inch"
        '    .Alignment = HorizontalAlignment.Right
        '    .Width = 70
        'End With
        'With colStyle2(9)
        '    .MappingName = "Price"
        '    .HeaderText = "Price"
        '    .Alignment = HorizontalAlignment.Right
        'End With
        'With colStyle2(7)
        '    .MappingName = "Type"
        '    .HeaderText = "Type"
        'End With
        'With colStyle2(10)
        '    .MappingName = "DesignDate"
        '    .HeaderText = "Design Date"
        '    .Format = "MMM dd, yyyy"
        '    .Width = 100
        'End With

        'With colStyle2(12)
        '    .MappingName = "ForBill"
        '    .HeaderText = "For Bill No."
        'End With

        'With gridstyle3.GridColumnStyles
        '    .Add(colStyle2(0))
        '    .Add(colStyle2(1))
        '    .Add(colStyle2(2))
        '    .Add(colStyle2(10))
        '    .Add(colStyle2(3))
        '    .Add(colStyle2(4))
        '    .Add(colStyle2(5))
        '    .Add(colStyle2(6))
        '    .Add(colStyle2(9))
        '    .Add(colStyle2(7))
        '    '.Add(colStyle(11))
        '    .Add(colStyle2(12))
        'End With
        'DataGrid3.TableStyles.Clear()
        'DataGrid3.TableStyles.Add(gridstyle3)
        'gridstyle3.HeaderBackColor = Color.MidnightBlue
        'gridstyle3.HeaderForeColor = Color.White
        'gridstyle3.GridLineColor = Color.RoyalBlue
        'gridstyle3.AllowSorting = False

        'ds7.Tables.Add(dt7)
        'dc7(0) = New DataColumn("CompName", Type.GetType("System.String"))
        'dc7(1) = New DataColumn("BillNo", Type.GetType("System.Int32"))
        'dc7(2) = New DataColumn("BillDate", Type.GetType("System.DateTime"))
        'dc7(3) = New DataColumn("prebalance", Type.GetType("System.Decimal"))
        'dc7(4) = New DataColumn("descost", Type.GetType("System.Decimal"))
        'dc7(5) = New DataColumn("TotAmount", Type.GetType("System.Decimal"))
        'dc7(6) = New DataColumn("Paid", Type.GetType("System.String"))
        'dc7(7) = New DataColumn("curBalance", Type.GetType("System.Decimal"))
        'dc7(8) = New DataColumn("Year", Type.GetType("System.Int32"))
        'For i = 0 To 8
        '    dt7.Columns.Add(dc7(i))
        'Next

        'DataGrid4.DataSource = ds7.Tables(0)
        'ds7.Tables(0).TableName = "bill"
        'Dim gridstyle4 As New DataGridTableStyle
        'gridstyle4.MappingName = "bill"
        'Dim colStyle3(9) As DataGridTextBoxColumn
        'For i = 0 To 8
        '    colStyle3(i) = New DataGridTextBoxColumn
        'Next
        'With colStyle3(0)
        '    .MappingName = "CompName"
        '    .HeaderText = "Company Name"
        '    .Width = 250
        'End With
        'With colStyle3(8)
        '    .MappingName = "Year"
        '    .HeaderText = "Bill"
        '    .Width = 30
        '    .Alignment = HorizontalAlignment.Right
        'End With
        'With colStyle3(1)
        '    .MappingName = "BillNo"
        '    .HeaderText = " No."
        '    .Width = 100
        'End With
        'With colStyle3(2)
        '    .MappingName = "BillDate"
        '    .Format = "MMM dd, yyyy"
        '    .HeaderText = "Bill Date"
        '    .Width = 150
        'End With
        'With colStyle3(3)
        '    .MappingName = "prebalance"
        '    .HeaderText = "Previous Balance"
        '    .Alignment = HorizontalAlignment.Right
        '    .Width = 180
        'End With
        'With colStyle3(4)
        '    .MappingName = "descost"
        '    .HeaderText = "Design Cost"
        '    .Alignment = HorizontalAlignment.Right
        '    .Width = 160
        'End With
        'With colStyle3(5)
        '    .MappingName = "TotAmount"
        '    .HeaderText = "Design + Balance"
        '    .Alignment = HorizontalAlignment.Right
        '    .Width = 190
        'End With
        'With colStyle3(6)
        '    .MappingName = "Paid"
        '    .HeaderText = "Paid"
        '    .Alignment = HorizontalAlignment.Right
        '    .Width = 150
        'End With
        'With colStyle3(7)
        '    .MappingName = "curBalance"
        '    .HeaderText = "Current Balance"
        '    .Alignment = HorizontalAlignment.Right
        '    .Width = 160
        'End With
        'With gridstyle4.GridColumnStyles
        '    .Add(colStyle3(0))
        '    .Add(colStyle3(8))
        '    .Add(colStyle3(1))
        '    .Add(colStyle3(2))
        '    .Add(colStyle3(3))
        '    .Add(colStyle3(4))
        '    .Add(colStyle3(5))
        '    '.Add(colStyle3(6))
        '    .Add(colStyle3(7))
        'End With
        'DataGrid4.TableStyles.Clear()
        'DataGrid4.TableStyles.Add(gridstyle4)
        'gridstyle4.HeaderBackColor = Color.MidnightBlue
        'gridstyle4.HeaderForeColor = Color.White
        'gridstyle4.GridLineColor = Color.RoyalBlue
        'gridstyle4.AllowSorting = False

        'dt11 = New DataTable
        'ds11 = New DataSet
        'ds11.Tables.Add(dt11)
        'dc11(0) = New DataColumn("PayNo", Type.GetType("System.Int32"))
        'dc11(1) = New DataColumn("CompName", Type.GetType("System.String"))
        'dc11(2) = New DataColumn("BillNo", Type.GetType("System.String"))
        'dc11(3) = New DataColumn("Balance", Type.GetType("System.Decimal"))
        'dc11(4) = New DataColumn("Date", Type.GetType("System.DateTime"))
        'dc11(5) = New DataColumn("PayMode", Type.GetType("System.String"))
        'dc11(6) = New DataColumn("Paid", Type.GetType("System.Decimal"))
        'dc11(7) = New DataColumn("Deduct", Type.GetType("System.Decimal"))
        'dc11(8) = New DataColumn("TaxDeduct", Type.GetType("System.Decimal"))
        'dc11(9) = New DataColumn("Cheque", Type.GetType("System.String"))
        'dc11(10) = New DataColumn("Bank", Type.GetType("System.String"))
        'dc11(11) = New DataColumn("ChequeDate", Type.GetType("System.DateTime"))
        'dc11(12) = New DataColumn("Detail", Type.GetType("System.String"))
        'dc11(13) = New DataColumn("TotPaid", Type.GetType("System.Decimal"))
        'For i = 0 To 13
        '    dt11.Columns.Add(dc11(i))
        'Next
        'DataGrid5.DataSource = ds11.Tables(0)
        'ds11.Tables(0).TableName = "payment"
        'Dim gridstyle11 As New DataGridTableStyle
        'gridstyle11.MappingName = "payment"
        'Dim colstyle11(15) As DataGridTextBoxColumn
        'For i = 0 To 13
        '    colstyle11(i) = New DataGridTextBoxColumn
        'Next
        'With colstyle11(1)
        '    .MappingName = "CompName"
        '    .HeaderText = "Company Name"
        '    .Width = 100
        'End With
        'With colstyle11(2)
        '    .MappingName = "BillNo"
        '    .HeaderText = "Bill No."
        '    .Width = 60
        'End With
        'With colstyle11(3)
        '    .MappingName = "Balance"
        '    .HeaderText = "Balance"
        '    .Alignment = HorizontalAlignment.Right
        '    .Width = 80
        'End With
        'With colstyle11(4)
        '    .MappingName = "Date"
        '    .HeaderText = "Paid Date"
        '    .Format = "MMM dd, yyyy"
        '    .Width = 100
        'End With
        'With colstyle11(5)
        '    .MappingName = "PayMode"
        '    .HeaderText = "Payment Mode"
        '    .Width = 70
        'End With
        'With colstyle11(6)
        '    .MappingName = "Paid"
        '    .HeaderText = "Amount Paid"
        '    .Alignment = HorizontalAlignment.Right
        '    .Width = 80
        'End With
        'With colstyle11(7)
        '    .MappingName = "Deduct"
        '    .HeaderText = "Deduction"
        '    .Alignment = HorizontalAlignment.Right
        '    .Width = 80
        'End With
        'With colstyle11(8)
        '    .MappingName = "TaxDeduct"
        '    .HeaderText = "Tax Deduct."
        '    .Alignment = HorizontalAlignment.Right
        '    .Width = 80
        'End With
        'With colstyle11(9)
        '    .MappingName = "Cheque"
        '    .HeaderText = "Cheque Number"
        '    .Width = 130
        'End With
        'With colstyle11(10)
        '    .MappingName = "Bank"
        '    .HeaderText = "Bank Name"
        '    .Width = 130
        'End With
        'With colstyle11(11)
        '    .MappingName = "ChequeDate"
        '    .HeaderText = "Cheque Date"
        '    .Format = "MMM dd, yyyy"
        '    .Width = 100
        'End With
        'With colstyle11(12)
        '    .MappingName = "Detail"
        '    .HeaderText = "Detail"
        '    .Width = 130
        'End With
        'With colstyle11(13)
        '    .MappingName = "TotPaid"
        '    .HeaderText = "Total Paid"
        '    .Alignment = HorizontalAlignment.Right
        '    .Width = 80
        'End With
        'With gridstyle11.GridColumnStyles
        '    .Add(colstyle11(0))
        '    .Add(colstyle11(1))
        '    .Add(colstyle11(2))
        '    .Add(colstyle11(3))
        '    .Add(colstyle11(4))
        '    .Add(colstyle11(5))
        '    .Add(colstyle11(6))
        '    .Add(colstyle11(7))
        '    .Add(colstyle11(8))
        '    .Add(colstyle11(13))
        '    .Add(colstyle11(9))
        '    .Add(colstyle11(10))
        '    .Add(colstyle11(11))
        '    .Add(colstyle11(12))
        'End With
        'DataGrid5.TableStyles.Clear()
        'DataGrid5.TableStyles.Add(gridstyle11)
        'gridstyle11.HeaderBackColor = Color.MidnightBlue
        'gridstyle11.HeaderForeColor = Color.White
        'gridstyle11.GridLineColor = Color.RoyalBlue
        'gridstyle11.AllowSorting = False


        'ds9.Tables.Add(dt9)
        'dc9(0) = New DataColumn("CompName", Type.GetType("System.String"))
        'dc9(1) = New DataColumn("BillNo", Type.GetType("System.Int32"))
        'dc9(2) = New DataColumn("BillDate", Type.GetType("System.DateTime"))
        'dc9(3) = New DataColumn("prebalance", Type.GetType("System.Decimal"))
        'dc9(4) = New DataColumn("Deduction", Type.GetType("System.Decimal"))
        'dc9(5) = New DataColumn("TaxDeduction", Type.GetType("System.Decimal"))
        'dc9(6) = New DataColumn("curBalance", Type.GetType("System.Decimal"))
        'dc9(7) = New DataColumn("TotUnPaid", Type.GetType("System.Decimal"))
        'For i = 0 To 7
        '    dt9.Columns.Add(dc9(i))
        'Next

        'radioReportsCompName.Checked = True

    End Sub

    Sub loadOrReloadCustomerList(Optional cmbCompanyList As ComboBox = Nothing)

        Dim customerQuery = New SqlCommand("select CustNo,CompName from customer", dbConnection)
        Dim customerAdapter = New SqlDataAdapter()
        customerAdapter.SelectCommand = customerQuery
        Dim customerDataSet = New DataSet
        customerAdapter.Fill(customerDataSet, "customer")
        Dim customerTable As DataTable = customerDataSet.Tables(0)

        If cmbCompanyList IsNot Nothing Then
            cmbCompanyList.BindingContext = New BindingContext()
            cmbCompanyList.DataSource = customerTable
        Else
            cmbCustCompanyList.BindingContext = New BindingContext()
            cmbCustCompanyList.DataSource = customerTable
            cmbDesCompanyList.BindingContext = New BindingContext()
            cmbDesCompanyList.DataSource = customerTable
            cmbBillingCompanyList.BindingContext = New BindingContext()
            cmbBillingCompanyList.DataSource = customerTable
            cmbPaymentCompanyList.BindingContext = New BindingContext()
            cmbPaymentCompanyList.DataSource = customerTable
            cmbReportCompanyList.BindingContext = New BindingContext()
            cmbReportCompanyList.DataSource = customerTable
        End If


    End Sub

    Sub loadOrReloadDesignList(Optional cmbDesignList As ComboBox = Nothing, Optional custNo As Integer = Nothing)
        Dim designQuery As SqlCommand
        If (custNo <> Nothing) Then
            designQuery = New SqlCommand("select DesignNo, DesignName from design where custNo=" + custNo.ToString, dbConnection)
        Else
            designQuery = New SqlCommand("select DesignNo, DesignName from design", dbConnection)
        End If

        Dim designAdapter = New SqlDataAdapter()
        designAdapter.SelectCommand = designQuery
        Dim designDataSet = New DataSet
        designAdapter.Fill(designDataSet, "design")
        Dim designTable As DataTable = designDataSet.Tables(0)

        If cmbDesignList IsNot Nothing Then
            cmbDesignList.BindingContext = New BindingContext()
            cmbDesignList.DataSource = designTable
        Else
            cmbDesDesignList.BindingContext = New BindingContext()
            cmbDesDesignList.DataSource = designTable
        End If
    End Sub

    Function getUnBilledDesignAmount(Optional custNo As Integer = Nothing) As Decimal
        Dim designQuery As SqlCommand
        If (custNo <> Nothing) Then
            designQuery = New SqlCommand("select sum(Price) from design where custNo=" + custNo.ToString + " and Billed = 0", dbConnection)
        Else
            designQuery = New SqlCommand("select sum(Price) from design where Billed = 0", dbConnection)
        End If

        Dim designAdapter = New SqlDataAdapter()
        designAdapter.SelectCommand = designQuery
        Dim designDataSet = New DataSet
        designAdapter.Fill(designDataSet, "design")
        Dim designTable As DataTable = designDataSet.Tables(0)

        If designTable.Rows.Count = 0 Or designTable.Rows(0).Item(0) Is DBNull.Value Then
            Return 0
        End If

        Return designTable.Rows(0).Item(0)


    End Function

    Sub loadOrReloadBillList(Optional cmbBillList As ComboBox = Nothing, Optional custNo As Integer = Nothing)
        Dim billQuery As SqlCommand
        If (custNo <> Nothing) Then
            billQuery = New SqlCommand("select BillNo from bill where custNo=" + custNo.ToString, dbConnection)
        Else
            billQuery = New SqlCommand("select BillNo from bill", dbConnection)
        End If

        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "bill")
        Dim billTable As DataTable = billDataSet.Tables(0)

        If cmbBillList IsNot Nothing Then
            cmbBillingBillNoList.BindingContext = New BindingContext()
            cmbBillList.DataSource = billTable
        Else
            cmbBillingBillNoList.BindingContext = New BindingContext()
            cmbBillingBillNoList.DataSource = billTable
        End If
    End Sub

    Sub RefreshBillNoList(Optional custNo As Integer = -1)
        Dim billQuery As SqlCommand
        If (custNo <> -1) Then
            billQuery = New SqlCommand("select billno from bill where custNo=" + custNo.ToString, dbConnection)
        Else
            billQuery = New SqlCommand("select billno from bill", dbConnection)
        End If

        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "bill")
        Dim billTable As DataTable = billDataSet.Tables(0)
        cmbBillingBillNoList.DisplayMember = "billno"
        cmbBillingBillNoList.DataSource = billTable
    End Sub

    Sub RefreshList1()
    End Sub

    Sub RefreshList2()
        'Try
        Ds2.Dispose()
        Ds2 = New DataSet
        Sda2.SelectCommand = cmd2
        Sda2.Fill(Ds2, "design")
        Dt2 = Ds2.Tables(0)
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub
    Sub RefreshList4()
        'Try
        cmd10 = New SqlCommand("select * from payment", dbConnection)
        sda10 = New SqlDataAdapter()
        sda10.SelectCommand = cmd10
        ds10 = New DataSet
        sda10.Fill(ds10, "payment")
        dt10 = ds10.Tables(0)
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub
    Sub RefreshList3()
        'Try
        ds3.Dispose()
        ds3 = New DataSet
        sda3.SelectCommand = cmd3
        sda3.Fill(ds3, "bill")
        dt3 = ds3.Tables(0)
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub
    Sub loadDesignGrid(Optional custNo As Integer = Nothing)
        'Try
        Dim designQuery As SqlCommand
        If (custNo <> Nothing) Then
            designQuery = New SqlCommand("select * from design where custNo=" + custNo.ToString, dbConnection)
        Else
            designQuery = New SqlCommand("select * from design", dbConnection)
        End If

        Dim designAdapter = New SqlDataAdapter()
        designAdapter.SelectCommand = designQuery
        Dim designDataSet = New DataSet
        designAdapter.Fill(designDataSet, "design")
        Dim designTable = designDataSet.Tables(0)
        dgDesDesignDetails.DataSource = designTable
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Sub loadBillingGrid(Optional custNo As Integer = Nothing)
        'Try
        Dim billQuery As SqlCommand
        If (custNo <> Nothing) Then
            billQuery = New SqlCommand("select BillNo, BillDate, DesignCost, UnPaidAmountTillNow, PaidAmount, 
                            DesignCost+UnPaidAmountTillNow as TotalAmount, DesignCost+UnPaidAmountTillNow-PaidAmount as RemainingBalance, Cancelled  from bill where custNo=" + custNo.ToString, dbConnection)
        Else
            billQuery = New SqlCommand("select BillNo, BillDate, DesignCost, UnPaidAmountTillNow, PaidAmount, 
                            DesignCost+UnPaidAmountTillNow as TotalAmount, DesignCost+UnPaidAmountTillNow-PaidAmount as RemainingBalance, Cancelled from bill", dbConnection)
        End If

        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "bill")
        Dim billTable = billDataSet.Tables(0)
        dgBIllingBillDetails.DataSource = billTable
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Sub refreshgrid2()
        'Try
        RefreshList3()
        dgBIllingBillDetails.DataSource = ds3.Tables(0)
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub
    Sub refreshgrid3()
        'Try
        RefreshList4()
        DataGrid5.DataSource = ds10.Tables(0)
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub ComboBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbCustCompanyList.KeyDown
        'Try
        'If (e.KeyCode = Keys.Enter) Then
        '    Button1.PerformClick()
        'Else
        'If e.Alt Then
        '    If e.KeyCode = Keys.D Then
        '        tabAllTabsHolder.SelectTab(1)
        '    ElseIf e.KeyCode = Keys.B Then
        '        tabAllTabsHolder.SelectTab(2)
        '    ElseIf e.KeyCode = Keys.P Then
        '        tabAllTabsHolder.SelectTab(3)
        '    ElseIf e.KeyCode = Keys.R Then
        '        tabAllTabsHolder.SelectTab(4)
        '    ElseIf e.KeyCode = Keys.H Then
        '        tabAllTabsHolder.SelectTab(5)
        '    End If
        'End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub
    Private Sub cmbCustCompanyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCustCompanyList.SelectedIndexChanged
        'Try
        If (cmbCustCompanyList.SelectedIndex = -1) Then
            Return
        End If

        Dim custNo As Integer = cmbCustCompanyList.SelectedValue
        Dim custSelectQuery = New SqlCommand("select * from customer where custno=" + custNo.ToString, dbConnection)
        Dim customerDataAdapter = New SqlDataAdapter()
        customerDataAdapter.SelectCommand = custSelectQuery
        Dim customerDataSet = New DataSet
        customerDataAdapter.Fill(customerDataSet, "customer")
        Dim customerTable = customerDataSet.Tables(0)

        If (customerTable.Rows.Count > 0) Then
            Dim dataRow = customerTable.Rows(0)
            txtGstIn.Text = dataRow.Item(2)
            txtOwnerName.Text = dataRow.Item(3)
            txtAddress.Text = dataRow.Item(4)
            txtMobile.Text = dataRow.Item(5)
            txtLandline.Text = dataRow.Item(6)
            txtEmail.Text = dataRow.Item(7)
            txtWebsite.Text = dataRow.Item(8)
            txtCGST.Text = dataRow.Item(9).ToString
            txtSGST.Text = dataRow.Item(10).ToString
            txtIGST.Text = dataRow.Item(11).ToString
            txtWPCharge.Text = dataRow.Item(12).ToString
            txtWorkingCharge.Text = dataRow.Item(13).ToString
            txtPrintCharge.Text = dataRow.Item(14).ToString
        Else
            MessageBox.Show("No data found for customer: " + custNo + "-" + cmbCustCompanyList.Text)
        End If


        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try

    End Sub

    Sub resetCustomerScreen()
        cmbCustCompanyList.SelectedIndex = -1
        txtGstIn.Text = ""
        txtOwnerName.Text = ""
        txtAddress.Text = ""
        txtMobile.Text = ""
        txtEmail.Text = ""
        txtLandline.Text = ""
        txtWebsite.Text = ""
        txtCGST.Text = ""
        txtSGST.Text = ""
        txtIGST.Text = ""
        txtWPCharge.Text = ""
        txtWorkingCharge.Text = ""
        txtPrintCharge.Text = ""
        cmbCustCompanyList.Focus()
    End Sub

    Sub resetDesignScreen()
        cmbDesDesignList.SelectedIndex = -1
        radioDesWP.Checked = True
        txtDesWidth.Text = ""
        txtDesHeight.Text = ""
        txtDesNoOfColors.Text = ""
        txtDesCostPerUnit.Text = ""
        txtDesCalculatedPrice.Text = ""
        pbDesDesignImage.Image = Nothing
        dgDesDesignDetails.DataSource = Nothing
        cmbDesDesignList.Focus()
    End Sub

    Sub resetBillingScreen()
        cmbBillingBillNoList.Enabled = True
        btnBillingCreateBill.Visible = True
        btnBillingConfirmCreateBill.Visible = False
        btnBillingCancelCreateBill.Visible = False
        btnBillingCancelBill.Text = "Mark Cancelled"
        lblCancelledBillIndicator.Visible = False
        cmbBillingBillNoList.SelectedIndex = -1
        dpBillingBillDate.Text = ""
        txtBillingPrevBalance.Text = "0.00"
        txtBillingDesignAmount.Text = "0.00"
        txtBillingTotalAmount.Text = "0.00"
        txtBillingPaidAmount.Text = "0.00"
        txtBillingRemainingBalance.Text = "0.00"
        cmbBillingBillNoList.Focus()
    End Sub

    Public Sub btnCustAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustAdd.Click
        'Try
        If cmbCustCompanyList.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid company Name")
            cmbCustCompanyList.Focus()
        ElseIf txtGstIn.Text.Trim.Equals("") Then
            MessageBox.Show("Enter GSTIN number")
            txtGstIn.Focus()
        ElseIf txtOwnerName.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Proprietor Name")
            txtOwnerName.Focus()
        ElseIf txtAddress.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Address")
            txtAddress.Focus()
        ElseIf txtMobile.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Mobile Number")
            txtMobile.Focus()
        Else
            Dim query As String = String.Empty
            query &= "INSERT INTO customer (CompName, GSTIN, OwnerName, Address, Mobile, Landline, Email, Website, "
            query &= "CGST, SGST, IGST, WPsqrinch, Wcolor, Pcolor) "
            query &= "VALUES ( @CompName, @GSTIN, @OwnerName, @Address, @Mobile, @Landline, @Email, "
            query &= "@Website, @CGST, @SGST, @IGST, @WPsqrinch, @Wcolor, @Pcolor)"

            Using comm As New SqlCommand()
                With comm
                    .Connection = dbConnection
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@CompName", cmbCustCompanyList.Text)
                    .Parameters.AddWithValue("@GSTIN", txtGstIn.Text)
                    .Parameters.AddWithValue("@OwnerName", txtOwnerName.Text)
                    .Parameters.AddWithValue("@Address", txtAddress.Text)
                    .Parameters.AddWithValue("@Mobile", txtMobile.Text)
                    .Parameters.AddWithValue("@Landline", txtLandline.Text)
                    .Parameters.AddWithValue("@Email", txtEmail.Text)
                    .Parameters.AddWithValue("@Website", txtWebsite.Text)
                    .Parameters.AddWithValue("@CGST", txtCGST.Text)
                    .Parameters.AddWithValue("@SGST", txtSGST.Text)
                    .Parameters.AddWithValue("@IGST", txtIGST.Text)
                    .Parameters.AddWithValue("@WPsqrinch", txtWPCharge.Text)
                    .Parameters.AddWithValue("@Wcolor", txtWorkingCharge.Text)
                    .Parameters.AddWithValue("@Pcolor", txtPrintCharge.Text)
                End With
                comm.ExecuteNonQuery()
            End Using
            MessageBox.Show("Company successfully added")
            resetCustomerScreen()
            loadOrReloadCustomerList()
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message & " (Or) This Customer Record may already exist")
        'End 'Try
    End Sub

    Private Sub btnCustDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustDelete.Click
        'Try

        If (cmbCustCompanyList.SelectedIndex = -1) Then
            MessageBox.Show("Please select a Company from Company List")
            cmbCustCompanyList.Focus()
            Return
        End If

        flag = 0
        If cmbCustCompanyList.Text.Trim.Equals("") Then
            MessageBox.Show("select Company Name")
            cmbCustCompanyList.Focus()
        ElseIf MessageBox.Show("All Designs, Bills and Payments will be deleted belongs to this customer." + vbNewLine + vbNewLine + "  Do you want to delete this Customer - " & cmbCustCompanyList.Text & " ?", "WARNING", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            VerifyingDelete.Button1.Text = "Delete "
            VerifyingDelete.Button1.Text += cmbCustCompanyList.Text
            VerifyingDelete.Show()
            'deleteCompanyEntire()
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message & " (Or) This Customer Record may not exist")
        'loadDesignGrid()
        'refreshgrid2()
        'refreshgrid3()
        'RefreshList1()
        'End 'Try
    End Sub
    Public Sub DeleteCompanyEntire()
        If cmbCustCompanyList.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid company Name")
            cmbCustCompanyList.Focus()
        Else
            Dim query As String = String.Empty
            query &= "DELETE FROM customer where CustNo=@CustNo"

            Using comm As New SqlCommand()
                With comm
                    .Connection = dbConnection
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@CustNo", cmbCustCompanyList.SelectedValue)
                End With
                comm.ExecuteNonQuery()
            End Using
            MessageBox.Show("Company successfully deleted")
            resetCustomerScreen()
            loadOrReloadCustomerList()
        End If
    End Sub
    Private Sub btnCustUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustUpdate.Click
        'Try
        If (cmbCustCompanyList.SelectedIndex = -1) Then
            MessageBox.Show("Please select a Company from Company List")
            cmbCustCompanyList.Focus()
            Return
        End If

        If cmbCustCompanyList.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid company Name")
            cmbCustCompanyList.Focus()
        ElseIf txtGstIn.Text.Trim.Equals("") Then
            MessageBox.Show("Enter GSTIN number")
            txtGstIn.Focus()
        ElseIf txtOwnerName.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Proprietor Name")
            txtOwnerName.Focus()
        ElseIf txtAddress.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Address")
            txtAddress.Focus()
        ElseIf txtMobile.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Mobile Number")
            txtMobile.Focus()
        Else
            Dim custNo As Integer = cmbCustCompanyList.SelectedValue
            Dim query As String = String.Empty
            query &= "UPDATE customer SET CompName=@CompName, GSTIN=@GSTIN, OwnerName=@OwnerName, Address=@Address,"
            query &= "Mobile=@Mobile, Landline=@Landline, Email=@Email, Website=@Website, CGST=@CGST, SGST=@SGST "
            query &= "IGST=@IGST, WPsqrinch=@WPsqrinch, Wcolor=@Wcolor, Pcolor=@Pcolor where CustNo=@CustNo"

            Using comm As New SqlCommand()
                With comm
                    .Connection = dbConnection
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@CustNo", custNo)
                    .Parameters.AddWithValue("@CompName", cmbCustCompanyList.Text)
                    .Parameters.AddWithValue("@GSTIN", txtGstIn.Text)
                    .Parameters.AddWithValue("@OwnerName", txtOwnerName.Text)
                    .Parameters.AddWithValue("@Address", txtAddress.Text)
                    .Parameters.AddWithValue("@Mobile", txtMobile.Text)
                    .Parameters.AddWithValue("@Landline", txtLandline.Text)
                    .Parameters.AddWithValue("@Email", txtEmail.Text)
                    .Parameters.AddWithValue("@Website", txtWebsite.Text)
                    .Parameters.AddWithValue("@CGST", txtCGST.Text)
                    .Parameters.AddWithValue("@SGST", txtSGST.Text)
                    .Parameters.AddWithValue("@IGST", txtIGST.Text)
                    .Parameters.AddWithValue("@WPsqrinch", txtWPCharge.Text)
                    .Parameters.AddWithValue("@Wcolor", txtWorkingCharge.Text)
                    .Parameters.AddWithValue("@Pcolor", txtPrintCharge.Text)
                End With
                comm.ExecuteNonQuery()
            End Using
            MessageBox.Show("Company successfully updated")
            resetCustomerScreen()
            loadOrReloadCustomerList()
            cmbCustCompanyList.SelectedIndex = cmbCustCompanyList.FindString(custNo.ToString)
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message & " Or this Customer Record may not exist")
        'End 'Try
    End Sub


    Private Sub btnDesAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesAdd.Click
        'Try
        If cmbDesDesignList.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Design Name")
            cmbDesDesignList.Focus()
        ElseIf dpDesDesignDate.Text.Trim.Equals("") Then
            MessageBox.Show("Choose Valid date")
            dpDesDesignDate.Focus()
        ElseIf cmbDesCompanyList.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Company Name")
            cmbDesCompanyList.Focus()
        ElseIf txtDesNoOfColors.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid No.of colors")
            txtDesNoOfColors.Focus()
        ElseIf txtDesCostPerUnit.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Unit cost")
            txtDesCostPerUnit.Focus()
        ElseIf txtDesCalculatedPrice.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Total cost")
            txtDesCalculatedPrice.Focus()
        Else
            Dim memoryStream As New MemoryStream()
            Dim imageBuffer() As Byte
            If pbDesDesignImage.Image Is Nothing Then
                imageBuffer = Nothing
            Else
                'PictureBox2.Image.Save(ms, PictureBox2.Image.RawFormat)
                pbDesDesignImage.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg)
                imageBuffer = memoryStream.GetBuffer
            End If
            memoryStream.Close()

            Dim designType As String
            If radioDesWP.Checked Then
                designType = "WP/Inch"
            ElseIf radioDesWorking.Checked Then
                designType = "Working/Color"
            Else
                designType = "Print/Color"
            End If

            Dim designWidth As Decimal
            If Val(txtDesWidth.Text) = 0 Then
                designWidth = 0
            Else
                designWidth = Decimal.Parse(txtDesWidth.Text.Trim)
            End If

            Dim designHeight As Decimal
            If Val(txtDesHeight.Text) = 0 Then
                designHeight = 0
            Else
                designHeight = Decimal.Parse(txtDesHeight.Text.Trim)
            End If

            Dim query As String = String.Empty
            query &= "INSERT INTO design (CustNo, DesignName, Height, Width, Colors, UnitCost,"
            query &= "Type, Image, Price, DesignDate, Billed) "
            query &= "VALUES (@CustNo, @DesignName, @Height, @Width, @Colors, @UnitCost,"
            query &= "@Type, @Image, @Price, @DesignDate, @Billed )"

            Using comm As New SqlCommand()
                With comm
                    .Connection = dbConnection
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@CustNo", cmbDesCompanyList.SelectedValue)
                    .Parameters.AddWithValue("@DesignName", cmbDesDesignList.Text)
                    .Parameters.AddWithValue("@Height", designHeight)
                    .Parameters.AddWithValue("@Width", designWidth)
                    .Parameters.AddWithValue("@Colors", Integer.Parse(txtDesNoOfColors.Text.Trim))
                    .Parameters.AddWithValue("@UnitCost", Decimal.Parse(txtDesCostPerUnit.Text.Trim))
                    .Parameters.AddWithValue("@Type", designType)
                    .Parameters.AddWithValue("@Price", Decimal.Parse(txtDesCalculatedPrice.Text.Trim))
                    .Parameters.AddWithValue("@DesignDate", dpDesDesignDate.Value)
                    .Parameters.AddWithValue("@Billed", 0)
                End With
                Dim designImage As New SqlParameter("@Image", SqlDbType.Image)
                designImage.Value = If(imageBuffer Is Nothing, DBNull.Value, imageBuffer)
                comm.Parameters.Add(designImage)
                comm.ExecuteNonQuery()
            End Using
            MessageBox.Show("Design successfully added")
            resetDesignScreen()
            loadOrReloadDesignList(cmbDesDesignList, cmbDesCompanyList.SelectedValue)
            loadDesignGrid(cmbDesCompanyList.SelectedValue)

            'Catch ex As Exception
            'MessageBox.Show("Message to Agni User:   " & ex.Message & " Or this design Record may already exist")
        End If
        'End 'Try

    End Sub

    Private Sub TextBox15_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDesCostPerUnit.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnDesAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub


    Private Sub txtDesCostPerUnit_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDesCostPerUnit.TextChanged
        'Try
        Dim designWidth As Decimal
        If Val(txtDesWidth.Text) = 0 Then
            designWidth = 1
        Else
            designWidth = Decimal.Parse(txtDesWidth.Text)
        End If

        Dim designHeight As Decimal
        If Val(txtDesHeight.Text) = 0 Then
            designHeight = 1
        Else
            designHeight = Decimal.Parse(txtDesHeight.Text)
        End If

        Dim noOfColors As Decimal
        If Val(txtDesNoOfColors.Text) = 0 Then
            noOfColors = 1
        Else
            noOfColors = Decimal.Parse(txtDesNoOfColors.Text)
        End If

        Dim costPerUnit As Decimal
        If Val(txtDesCostPerUnit.Text) = 0 Then
            costPerUnit = 1
        Else
            costPerUnit = Decimal.Parse(txtDesCostPerUnit.Text)
        End If

        txtDesCalculatedPrice.Text = Round(designWidth * designHeight * noOfColors * costPerUnit)
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbDesDesignImage.Click
        pictureload()
    End Sub
    Private Sub pictureload()
        'Try
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
            thumb = img.GetThumbnailImage(pbDesDesignImage.Width, pbDesDesignImage.Height, Nothing, inp)
            pbDesDesignImage.Image = thumb
            With pbDesDesignImage
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
            cmbDesDesignList.Text = desname
            'cmbDesDesignName.SelectedText = desname
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try

    End Sub
    Private Sub btnDesUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesUpdate.Click
        ''Try

        If cmbDesDesignList.SelectedIndex = -1 Then
            MessageBox.Show("Please select a design")
            cmbDesDesignList.Focus()
            Return
        End If

        If cmbDesDesignList.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Design Name")
            cmbDesDesignList.Focus()
        ElseIf dpDesDesignDate.Text.Trim.Equals("") Then
            MessageBox.Show("Choose Valid date")
            dpDesDesignDate.Focus()
        ElseIf cmbDesCompanyList.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Company Name")
            cmbDesCompanyList.Focus()
        ElseIf txtDesNoOfColors.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid No.of colors")
            txtDesNoOfColors.Focus()
        ElseIf txtDesCostPerUnit.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Unit cost")
            txtDesCostPerUnit.Focus()
        ElseIf txtDesCalculatedPrice.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Total cost")
            txtDesCalculatedPrice.Focus()
        Else
            Dim memoryStream As New MemoryStream()
            Dim imageBuffer() As Byte
            If pbDesDesignImage.Image Is Nothing Then
                imageBuffer = Nothing
            Else
                pbDesDesignImage.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg)
                imageBuffer = memoryStream.GetBuffer
            End If
            memoryStream.Close()

            Dim designType As String
            If radioDesWP.Checked Then
                designType = "WP/Inch"
            ElseIf radioDesWorking.Checked Then
                designType = "Working/Color"
            Else
                designType = "Print/Color"
            End If

            Dim designWidth As Decimal
            If Val(txtDesWidth.Text) = 0 Then
                designWidth = 0
            Else
                designWidth = Decimal.Parse(txtDesWidth.Text.Trim)
            End If

            Dim designHeight As Decimal
            If Val(txtDesHeight.Text) = 0 Then
                designHeight = 0
            Else
                designHeight = Decimal.Parse(txtDesHeight.Text.Trim)
            End If

            Dim query As String = String.Empty
            query &= "update design set DesignName=@DesignName, Height=@Height, Width=@Width, Colors=@Colors, UnitCost=@UnitCost,"
            query &= "Type=@Type, Image=@Image, Price=@Price, DesignDate=@DesignDate "
            query &= "where DesignNo=@DesignNo"

            Using comm As New SqlCommand()
                With comm
                    .Connection = dbConnection
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@DesignNo", cmbDesDesignList.SelectedValue)
                    .Parameters.AddWithValue("@DesignName", cmbDesDesignList.Text)
                    .Parameters.AddWithValue("@Height", designHeight)
                    .Parameters.AddWithValue("@Width", designWidth)
                    .Parameters.AddWithValue("@Colors", Integer.Parse(txtDesNoOfColors.Text.Trim))
                    .Parameters.AddWithValue("@UnitCost", Decimal.Parse(txtDesCostPerUnit.Text.Trim))
                    .Parameters.AddWithValue("@Type", designType)
                    .Parameters.AddWithValue("@Price", Decimal.Parse(txtDesCalculatedPrice.Text.Trim))
                    .Parameters.AddWithValue("@DesignDate", dpDesDesignDate.Value)
                End With
                Dim designImage As New SqlParameter("@Image", SqlDbType.Image)
                designImage.Value = If(imageBuffer Is Nothing, DBNull.Value, imageBuffer)
                comm.Parameters.Add(designImage)
                comm.ExecuteNonQuery()
            End Using
            MessageBox.Show("Design successfully updated")
            resetDesignScreen()
            loadDesignGrid(cmbDesCompanyList.SelectedValue)
        End If
        ''Catch ex As Exception
        '    'MessageBox.Show("Message to Agni User:   " & ex.Message)
        '    loadDesignGrid()
        ''End 'Try

    End Sub

    Sub updateRecentDesignsAsBilled(custNo As Integer, BillNo As Integer)

        If (BillNo = -1) Then
            Return
        End If

        Dim query As String = String.Empty
        query &= "update design set Billed=@Billed, BillNo=@BillNo where CustNo=@CustNo"

        Using comm As New SqlCommand()
            With comm
                .Connection = dbConnection
                .CommandType = CommandType.Text
                .CommandText = query
                .Parameters.AddWithValue("@CustNo", custNo.ToString)
                .Parameters.AddWithValue("@Billed", True)
                .Parameters.AddWithValue("@BillNo", BillNo)
            End With
            comm.ExecuteNonQuery()
        End Using
        resetDesignScreen()
        loadDesignGrid(cmbDesCompanyList.SelectedValue)
    End Sub

    Sub updateDesignsAsUnBilled(BillNo As Integer)

        If (BillNo = -1) Then
            Return
        End If

        Dim query As String = String.Empty
        query &= "update design set Billed=0, BillNo=null where BillNo=@BillNo"

        Using comm As New SqlCommand()
            With comm
                .Connection = dbConnection
                .CommandType = CommandType.Text
                .CommandText = query
                .Parameters.AddWithValue("@BillNo", BillNo)
            End With
            comm.ExecuteNonQuery()
        End Using
        resetDesignScreen()
        loadDesignGrid(cmbDesCompanyList.SelectedValue)
    End Sub

    Private Sub btnDesDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesDelete.Click
        'Try
        If cmbDesDesignList.SelectedIndex = -1 Then
            MessageBox.Show("Please select a design")
            cmbDesDesignList.Focus()
            Return
        End If

        If MessageBox.Show("Do you want to delete the design " & cmbDesDesignList.Text, "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            flag = 0
            Dim query As String = String.Empty
            query &= "DELETE FROM design where DesignNo=@designNo"

            Using comm As New SqlCommand()
                With comm
                    .Connection = dbConnection
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@designNo", cmbDesDesignList.SelectedValue)
                End With
                comm.ExecuteNonQuery()
            End Using
            MessageBox.Show("Design successfully deleted")
            resetDesignScreen()
            loadOrReloadDesignList(Nothing, cmbDesCompanyList.SelectedValue)
            loadDesignGrid(cmbDesCompanyList.SelectedValue)
            'If flag = 0 Then
            'MessageBox.Show("Cannot Delete.. There is no such design Record or it might be billed")
            'End If
        End If

        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox12_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDesWidth.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnDesAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox12_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDesWidth.TextChanged
        'Try
        If Val(txtDesWidth.Text) = 0 Then
            t12 = 1
        Else
            t12 = Decimal.Parse(txtDesWidth.Text)
        End If
        If Val(txtDesHeight.Text) = 0 Then
            t13 = 1
        Else
            t13 = Decimal.Parse(txtDesHeight.Text)
        End If
        If Val(txtDesNoOfColors.Text) = 0 Then
            t14 = 1
        Else
            t14 = Decimal.Parse(txtDesNoOfColors.Text)
        End If
        If Val(txtDesCostPerUnit.Text) = 0 Then
            t15 = 1
        Else
            t15 = Decimal.Parse(txtDesCostPerUnit.Text)
        End If
        txtDesCalculatedPrice.Text = Round(t12 * t13 * t14 * t15, 0)
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox13_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDesHeight.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnDesAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox13_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDesHeight.TextChanged
        'Try
        If Val(txtDesWidth.Text) = 0 Then
            t12 = 1
        Else
            t12 = Decimal.Parse(txtDesWidth.Text)
        End If
        If Val(txtDesHeight.Text) = 0 Then
            t13 = 1
        Else
            t13 = Decimal.Parse(txtDesHeight.Text)
        End If
        If Val(txtDesNoOfColors.Text) = 0 Then
            t14 = 1
        Else
            t14 = Decimal.Parse(txtDesNoOfColors.Text)
        End If
        If Val(txtDesCostPerUnit.Text) = 0 Then
            t15 = 1
        Else
            t15 = Decimal.Parse(txtDesCostPerUnit.Text)
        End If
        txtDesCalculatedPrice.Text = Round(t12 * t13 * t14 * t15, 0)
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox14_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDesNoOfColors.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnDesAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox14_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDesNoOfColors.TextChanged
        'Try
        If Val(txtDesWidth.Text) = 0 Then
            t12 = 1
        Else
            t12 = Decimal.Parse(txtDesWidth.Text)
        End If
        If Val(txtDesHeight.Text) = 0 Then
            t13 = 1
        Else
            t13 = Decimal.Parse(txtDesHeight.Text)
        End If
        If Val(txtDesNoOfColors.Text) = 0 Then
            t14 = 1
        Else
            t14 = Decimal.Parse(txtDesNoOfColors.Text)
        End If
        If Val(txtDesCostPerUnit.Text) = 0 Then
            t15 = 1
        Else
            t15 = Decimal.Parse(txtDesCostPerUnit.Text)
        End If
        txtDesCalculatedPrice.Text = Round(t12 * t13 * t14 * t15, 0)
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub
    Private Sub ComboBox5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbBillingBillNoList.Click
        'Try
        If cmbBillingCompanyList.Text.Trim.Equals("") Then
            MsgBox("Please select Company name")
            cmbBillingCompanyList.Focus()
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub ComboBox5_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbBillingBillNoList.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnBillingCreateBill.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub cmbBillingBillNoList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBillingBillNoList.SelectedIndexChanged
        'Try

        If (cmbBillingBillNoList.SelectedIndex = -1) Then
            Return
        End If

        log.debug("cmbBillingBillNoList_SelectedIndexChanged: entry")
        Dim billNo As Integer = cmbBillingBillNoList.SelectedValue
        log.debug("cmbBillingBillNoList_SelectedIndexChanged: cmbBillingBillNoList.SelectedValue  : " + billNo.ToString)
        Dim billSelectQuery = New SqlCommand("select * from bill where BillNo=" + billNo.ToString, dbConnection)
        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billSelectQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "bill")
        Dim billTable = billDataSet.Tables(0)

        If (billTable.Rows.Count > 0) Then
            Dim dataRow = billTable.Rows(0)
            dpBillingBillDate.Text = dataRow.Item("BillDate")
            txtBillingPrevBalance.Text = dataRow.Item("UnPaidAmountTillNow")
            txtBillingDesignAmount.Text = dataRow.Item("DesignCost")
            Dim billingTotalAmount = dataRow.Item("UnPaidAmountTillNow") + dataRow.Item("DesignCost")
            txtBillingTotalAmount.Text = billingTotalAmount
            txtBillingPaidAmount.Text = dataRow.Item("PaidAmount")
            txtBillingRemainingBalance.Text = billingTotalAmount - dataRow.Item("PaidAmount")
            If (Not IsDBNull(dataRow.Item("Cancelled")) AndAlso dataRow.Item("Cancelled") = True) Then
                lblCancelledBillIndicator.Visible = True
                btnBillingCancelBill.Enabled = False
            Else
                lblCancelledBillIndicator.Visible = False
                btnBillingCancelBill.Enabled = True
            End If

        Else
            MessageBox.Show("No data found for Bill: " + billNo.ToString)
            log.debug("cmbBillingBillNoList_SelectedIndexChanged: No data found for Bill: " + billNo.ToString)
        End If

        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox17_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBillingTotalAmount.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnBillingCreateBill.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox18_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnBillingCreateBill.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox18_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Try
        Dim T17, T18 As Decimal
        If Val(txtBillingTotalAmount.Text) = 0 Then
            T17 = 0
        Else
            T17 = Decimal.Parse(txtBillingTotalAmount.Text)
        End If
        'If Val(TextBox18.Text) = 0 Then
        '    T18 = 0
        'Else
        '    T18 = Decimal.Parse(TextBox18.Text)
        'End If
        txtBillingRemainingBalance.Text = T17 - T18
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message & " (Check the input as a valid number) ")
        'End 'Try
    End Sub

    Private Sub TextBox19_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBillingRemainingBalance.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnBillingCreateBill.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Class MyComboitem
        Public ReadOnly ID As Integer
        Public ReadOnly Text As String
        Public Sub New(ByVal ID As Integer, ByVal Text As String)
            'Try
            Me.ID = ID
            Me.Text = Text
            'Catch ex As Exception
            'MessageBox.Show("Message to Agni User:   " & ex.Message)
            'End 'Try
        End Sub
        Public Overrides Function ToString() As String
            Return Text
        End Function
    End Class

    Private Sub ComboBox4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbBillingCompanyList.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnBillingCreateBill.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub



    Private Sub cmbBillingCompanyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBillingCompanyList.SelectedIndexChanged
        'Try

        If (cmbBillingCompanyList.SelectedIndex <> -1) Then
            resetBillingScreen()
            loadOrReloadBillList(cmbBillingBillNoList, cmbBillingCompanyList.SelectedValue)
            loadBillingGrid(cmbBillingCompanyList.SelectedValue)
        End If

        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub DateTimePicker1_CloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles dpBillingBillDate.CloseUp
        'Try
        Dim desdate, seldate As Date
        Dim desdatestr, seldatestr As String
        Dim desamount As Decimal = 0
        Dim balamount As Decimal = 0
        Dim lastbildate As DateTime
        Dim lastbildatestr As String = ""
        seldatestr = dpBillingBillDate.Value.ToString("MM dd yyyy")
        seldate = DateTime.Parse(seldatestr)
        Dim custname = cmbBillingCompanyList.Text
        flag = 0
        If dt3.Rows.Count = 0 Then
            lastbildate = DateTime.Parse("01 01 1900")
            balamount = 0
        Else
            rowCount = dt3.Rows.Count - 1
            While (rowCount >= 0)
                dr3 = dt3.Rows(rowCount)
                If custname.Equals(dr3.Item(0)) Then
                    lastbildate = DateTime.Parse(dr3.Item(2))
                    lastbildatestr = lastbildate.ToString("MM dd yyyy")
                    lastbildate = DateTime.Parse(lastbildatestr)
                    balamount = dr3.Item(7)
                    flag = 1
                    Exit While
                End If
                rowCount -= 1
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
            rowCount = Dt2.Rows.Count - 1
            While (rowCount >= 0)
                Dr2 = Dt2.Rows(rowCount)
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
                rowCount -= 1
            End While
        End If
        txtBillingPrevBalance.Text = balamount
        txtBillingDesignAmount.Text = desamount
        txtBillingTotalAmount.Text = desamount + balamount
        txtBillingRemainingBalance.Text = txtBillingTotalAmount.Text
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBillingPrintBill.Click
        'Try
        If cmbBillingBillNoList.Text.Trim.Equals("") Then
            MsgBox("Please Select Bill Number")
            cmbBillingBillNoList.Focus()
        Else

            billkey = cmbBillingBillNoList.Text
            billcust = cmbBillingCompanyList.Text

            Dim billkey1 As String = billkey
            billkey1 = billkey1.Substring(billkey1.IndexOf("/") + 1, billkey1.Length - billkey1.IndexOf("/") - 1)
            rowCount = dt3.Rows.Count - 1
            Dim countcust As Int32 = 0
            While (rowCount >= 0)
                dr3 = dt3.Rows(rowCount)
                If billcust.Equals(dr3.Item(0).ToString) And billkey1 >= dr3.Item(1) Then
                    countcust += 1
                    PrevBillNo = "NIL"
                    If countcust = 2 Then
                        PrevBillNo = dr3.Item(8).ToString + "/" + dr3.Item(1).ToString
                        Exit While
                    End If
                End If
                rowCount = rowCount - 1
            End While

            billdatestring = dpBillingBillDate.Value.ToString("dd/MM/yyyy")
            T17 = txtBillingTotalAmount.Text
            T20 = txtBillingPrevBalance.Text
            T21 = txtBillingDesignAmount.Text
            Agnireport.Show()
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub ComboBox2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbDesCompanyList.KeyDown
        'Try
        'If (e.KeyCode = Keys.Enter) Then
        '    Button8.PerformClick()
        'Else
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub



    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDesCompanyList.SelectedIndexChanged
        'Try
        resetDesignScreen()
        If (cmbDesCompanyList.SelectedIndex <> -1) Then
            loadOrReloadDesignList(cmbDesDesignList, cmbDesCompanyList.SelectedValue)
            loadDesignGrid(cmbDesCompanyList.SelectedValue)
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub dgDesDesignDetails_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgDesDesignDetails.CurrentCellChanged
        'Try

        cmbDesDesignList.SelectedIndex = dgDesDesignDetails.CurrentRowIndex


        'Dim designNo As Integer = dgDesDesignDetails.DataSource.Rows(dgDesDesignDetails.CurrentRowIndex).Item(0)
        'Dim designSelectQuery = New SqlCommand("select * from design where DesignNo=" + designNo.ToString, dbConnection)
        'Dim designDataAdapter = New SqlDataAdapter()
        'designDataAdapter.SelectCommand = designSelectQuery
        'Dim designDataSet = New DataSet
        'designDataAdapter.Fill(designDataSet, "design")
        'Dim designTable = designDataSet.Tables(0)

        'If (designTable.Rows.Count > 0) Then
        '    Dim dataRow = designTable.Rows(0)
        '    cmbDesDesignName.SelectedValue = designNo
        '    txtDesHeight.Text = dataRow.Item(3)
        '    txtDesWidth.Text = dataRow.Item(4)
        '    txtDesNoOfColors.Text = dataRow.Item(5)
        '    txtDesCostPerUnit.Text = dataRow.Item(6)
        '    If dataRow.Item(7).ToString.Equals("WP/Inch") Then
        '        radioDesWP.Checked = True
        '    ElseIf dataRow.Item(7).ToString.Equals("Working/Color") Then
        '        radioDesWorking.Checked = True
        '    Else
        '        radioDesPrint.Checked = True
        '    End If
        '    If dataRow.Item(8) Is DBNull.Value Then
        '        pbDesDesignImage.Image = Nothing
        '    Else
        '        Dim designImage() As Byte = CType(dataRow.Item(8), Byte())
        '        Dim designImageBuffer As New MemoryStream(designImage)
        '        pbDesDesignImage.Image = Image.FromStream(designImageBuffer)
        '    End If
        '    txtDesCalculatedPrice.Text = dataRow.Item(9)
        '    dpDesDesignDate.Text = dataRow.Item(10)
        'Else
        '    MessageBox.Show("No data found for design: " + dgDesDesignDetails.DataSource.Rows(dgDesDesignDetails.CurrentRowIndex).Item(1))
        'End If

    End Sub

    Private Sub ComboBox3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Try
        If cmbDesCompanyList.Text.Trim.Equals("") Then
            MsgBox("Please select Company name")
            cmbDesCompanyList.Focus()
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub ComboBox3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnDesAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub


    Private Sub Button25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub






    Private Sub Button27_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Try
        If cmbCustCompanyList.Text.Trim.Equals("") Then
            MsgBox("Please Select Company Name")
            cmbCustCompanyList.Focus()
        ElseIf txtAddress.Text.Trim.Equals("") Then
            MsgBox("Please fill Address field")
            txtAddress.Focus()
        Else
            AddressReport.Show()
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub


    Private Sub TextBox11_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGstIn.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnCustAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOwnerName.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnCustAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAddress.KeyDown
        'Try
        '    If (e.KeyCode = Keys.Enter) Then
        '        Button1.PerformClick()
        If e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMobile.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnCustAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMobile.TextChanged

    End Sub

    Private Sub TextBox4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnCustAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub TextBox5_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLandline.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnCustAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLandline.TextChanged

    End Sub

    Private Sub TextBox6_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnCustAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub TextBox7_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtEmail.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnCustAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox7_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEmail.TextChanged

    End Sub

    Private Sub TextBox8_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnCustAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox8_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub TextBox9_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnCustAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox9_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub TextBox10_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtWebsite.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnCustAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox10_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWebsite.TextChanged

    End Sub

    Private Sub Button32_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustClear.Click
        resetCustomerScreen()
    End Sub

    Private Sub DataGrid1_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs)

    End Sub

    Private Sub DataGrid3_ControlRemoved(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs) Handles DataGrid3.ControlRemoved

    End Sub

    Private Sub DataGrid3_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid3.CurrentCellChanged
        'Try
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
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub DataGrid3_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DataGrid3.MouseUp
        'Try
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
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub DataGrid3_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs) Handles DataGrid3.Navigate

    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioDesWP.CheckedChanged
        'Try
        If radioDesWP.Checked Then
            lblDesCostPerUnit.Text = "Cost Per Inch"
        End If

        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioDesWorking.CheckedChanged
        'Try
        If radioDesWorking.Checked Then
            lblDesCostPerUnit.Text = "Cost Per Color"
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try

    End Sub

    Private Sub TextBox16_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDesCalculatedPrice.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnDesAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try

    End Sub

    Private Sub TextBox16_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDesCalculatedPrice.LostFocus
        txtDesCalculatedPrice.ReadOnly = True
    End Sub

    Private Sub TextBox16_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDesCalculatedPrice.TextChanged

    End Sub

    Private Sub RadioButton1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles radioDesWP.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnDesAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub RadioButton2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles radioDesWorking.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnDesAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub DateTimePicker2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dpDesDesignDate.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnDesAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub DateTimePicker2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dpDesDesignDate.ValueChanged

    End Sub

    Private Sub DateTimePicker1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dpBillingBillDate.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnBillingCreateBill.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dpBillingBillDate.ValueChanged

    End Sub

    Private Sub TextBox20_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBillingPrevBalance.KeyDown
        'Try
        'If (e.KeyCode = Keys.Enter) Then
        '    btnBillingCreateBill.PerformClick()
        'ElseIf e.Alt Then
        '    If e.KeyCode = Keys.C Then
        '        tabAllTabsHolder.SelectTab(0)
        '    ElseIf e.KeyCode = Keys.D Then
        '        tabAllTabsHolder.SelectTab(1)
        '    ElseIf e.KeyCode = Keys.P Then
        '        tabAllTabsHolder.SelectTab(3)
        '    ElseIf e.KeyCode = Keys.R Then
        '        tabAllTabsHolder.SelectTab(4)
        '    ElseIf e.KeyCode = Keys.H Then
        '        tabAllTabsHolder.SelectTab(5)
        '    End If
        'End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox20_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBillingPrevBalance.TextChanged

    End Sub

    Private Sub TextBox21_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBillingDesignAmount.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnBillingCreateBill.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox21_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBillingDesignAmount.TextChanged

    End Sub

    Private Sub ComboBox6_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbReportCompanyList.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            Button26.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub ComboBox6_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbReportCompanyList.SelectedIndexChanged
        'Try
        If (cmbReportCompanyList.SelectedIndex <> -1) Then
            loadOrReloadDesignList(cmbReportDesignList, cmbDesCompanyList.SelectedValue)
        End If

        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub DateTimePicker3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DateTimePicker3.KeyDown

        'Try
        If (e.KeyCode = Keys.Enter) Then
            Button26.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try

    End Sub

    Private Sub DateTimePicker3_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker3.ValueChanged
        PictureBox1.Image = Nothing
    End Sub

    Private Sub DateTimePicker4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DateTimePicker4.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            Button26.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub


    Private Sub Label22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label22.Click

    End Sub

    Private Sub Button33_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button33.Click

    End Sub

    Private Sub Button33_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button33.KeyDown
        'Try
        If (e.KeyCode = Keys.Space) Then
            pictureload()
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button34_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesEditPrice.Click
        txtDesCalculatedPrice.ReadOnly = Not txtDesCalculatedPrice.ReadOnly
        txtDesCalculatedPrice.Focus()
    End Sub

    Private Sub TabPage2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabDesign.Click

    End Sub

    Private Sub btnDesClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesClear.Click
        resetDesignScreen()
    End Sub

    Private Sub btnBillingClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBillingClear.Click
        resetBillingScreen()
    End Sub

    Private Sub Button38_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button38.Click
        'Try
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
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button37_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button37.Click
        'Try
        Me.Close()
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub


    Private Sub Button39_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button39.Click
        'Try
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

        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox11_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGstIn.TextChanged

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOwnerName.TextChanged

    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAddress.TextChanged

    End Sub

    Private Sub Button1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnCustAdd.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnCustAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnCustDelete.KeyDown
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnCustUpdate.KeyDown
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button32_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnCustClear.KeyDown
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button4_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button5_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button6_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button7_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TabPage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabCustomer.Click
        cmbCustCompanyList.Focus()
    End Sub

    Private Sub Button27_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button31_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button29_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button30_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button28_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox22_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox22_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub TabPage1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabCustomer.Enter
        cmbCustCompanyList.Focus()
    End Sub

    Private Sub TabPage1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabCustomer.GotFocus
        cmbCustCompanyList.Focus()
    End Sub

    Private Sub TabPage1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tabCustomer.MouseDown
        cmbCustCompanyList.Focus()
    End Sub

    Private Sub Button34_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnDesEditPrice.KeyDown
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button8_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnDesAdd.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnDesAdd.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button9_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnDesDelete.KeyDown
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button10_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnDesUpdate.KeyDown
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button35_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnDesClear.KeyDown
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub


    Private Sub Button13_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button14_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button15_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button12_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button22_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnBillingPrintBill.KeyDown
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button24_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button23_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button36_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnBillingClear.KeyDown
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button17_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button16_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button11_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button18_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button25_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        'If (e.KeyCode = Keys.Enter) Then
        'Button20.PerformClick()
        'Else
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub DateTimePicker4_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker4.ValueChanged
        PictureBox1.Image = Nothing
    End Sub

    Private Sub Button20_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        'If (e.KeyCode = Keys.Enter) Then
        'Button20.PerformClick()
        'Else
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button26_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Button26.KeyDown
        'Try
        'If (e.KeyCode = Keys.Enter) Then
        'Button20.PerformClick()
        'Else
        If e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub DataGrid4_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DataGrid4.MouseDoubleClick
        'Try
        Dim bilr As DataTable
        Dim billdate As Date
        bilr = DataGrid4.DataSource
        billkey = bilr.Rows(DataGrid4.CurrentRowIndex).Item(8).ToString + "/" + bilr.Rows(DataGrid4.CurrentRowIndex).Item(1).ToString
        billcust = bilr.Rows(DataGrid4.CurrentRowIndex).Item(0)

        Dim billkey1 As String = billkey
        billkey1 = billkey1.Substring(billkey1.IndexOf("/") + 1, billkey1.Length - billkey1.IndexOf("/") - 1)
        rowCount = dt3.Rows.Count - 1
        Dim countcust As Int32 = 0
        While (rowCount >= 0)
            dr3 = dt3.Rows(rowCount)
            If billcust.Equals(dr3.Item(0).ToString) And billkey1 >= dr3.Item(1) Then
                countcust += 1
                PrevBillNo = "NIL"
                If countcust = 2 Then
                    PrevBillNo = dr3.Item(8).ToString + "/" + dr3.Item(1).ToString
                    Exit While
                End If
            End If
            rowCount = rowCount - 1
        End While

        billdate = bilr.Rows(DataGrid4.CurrentRowIndex).Item(2)
        billdatestring = billdate.ToString("dd/MM/yyyy")
        T17 = bilr.Rows(DataGrid4.CurrentRowIndex).Item(5)
        T20 = bilr.Rows(DataGrid4.CurrentRowIndex).Item(3)
        T21 = bilr.Rows(DataGrid4.CurrentRowIndex).Item(4)
        Agnireport.Show()
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub



    Private Sub DataGrid4_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DataGrid4.MouseUp
        ''Try
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
        ''Catch ex As Exception
        '    'MessageBox.Show("Message to Agni User:   " & ex.Message)
        ''End 'Try
    End Sub

    Private Sub Label64_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label64.Click

    End Sub

    Private Sub DataGrid2_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs) Handles dgBIllingBillDetails.Navigate

    End Sub

    Private Sub tabBilling_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabBilling.Click
        'resetBillingScreen()
    End Sub

    Private Sub Button40_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBilingOutstandingBalance.Click
        'Try
        'Dim ds10 As DataSet

        Dim dt17, dt12 As DataTable
        Dim dr17, dr12 As DataRow
        Dim notpaidbal As Decimal = 0

        dt17 = dgBIllingBillDetails.DataSource
        dt12 = dgDesDesignDetails.DataSource
        outbal = 0
        unbilled = 0
        Dim b As Int16
        Dim a, a1 As Integer
        b = customerTable.Rows.Count
        inc = 0
        dt9.Clear()
        While (b)
            DataRow = customerTable.Rows(inc)
            a1 = dt12.Rows.Count - 1
            notpaidbal = 0
            While (a1 >= 0)
                dr12 = dt12.Rows(a1)
                If dr12.Item(0).Equals(DataRow.Item(1)) And dr12.Item(11).ToString.Equals("notpaid") Then
                    notpaidbal += Decimal.Parse(dr12.Item(9))
                ElseIf dr12.Item(0).Equals(DataRow.Item(1)) And dr12.Item(11).ToString.Equals("paid") Then
                    Exit While
                End If
                a1 -= 1
            End While

            a1 = dt10.Rows.Count - 1
            Deduction = 0
            taxDeduction = 0
            While (a1 >= 0)
                dr10 = dt10.Rows(a1)
                If dr10.Item(1).Equals(DataRow.Item(1)) Then
                    Deduction += Decimal.Parse(dr10.Item(7))
                    taxDeduction += Decimal.Parse(dr10.Item(8))
                End If
                a1 -= 1
            End While

            a = dt17.Rows.Count - 1
            flag = 0
            While (a >= 0)
                dr17 = dt17.Rows(a)
                If dr17.Item(0).Equals(DataRow.Item(1)) Then
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
                dr9.Item(0) = DataRow.Item(1).ToString
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
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub RadioButton5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton5.CheckedChanged
        GroupBox5.Visible = False
    End Sub

    Private Sub RadioButton6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton6.CheckedChanged
        GroupBox5.Visible = True
    End Sub

    Private Sub ComboBox7_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbPaymentCompanyList.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            Button43.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub ComboBox7_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPaymentCompanyList.SelectedIndexChanged

    End Sub

    Private Sub DataGrid5_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DataGrid5.MouseUp
        'Try
        Dim pos As Int32
        Dim paytable As DataTable
        paytable = DataGrid5.DataSource
        pos = DataGrid5.CurrentRowIndex
        dr10 = paytable.Rows(pos)
        TextBox18.Text = dr10.Item(0)
        cmbPaymentCompanyList.Text = ""
        cmbPaymentCompanyList.SelectedText = dr10.Item(1).ToString
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
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub DataGrid5_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs) Handles DataGrid5.Navigate

    End Sub


    Private Sub Button43_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button43.Click
        'Try
        If cmbPaymentCompanyList.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid company Name")
            cmbPaymentCompanyList.Focus()
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
            key = cmbPaymentCompanyList.Text
            If dt3.Rows.Count <> 0 Then
                rowCount = dt3.Rows.Count - 1
                inc = 0
                While (rowCount >= 0)
                    dr3 = dt3.Rows(rowCount)
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
                            cmbPaymentCompanyList.Focus()
                            Exit Sub
                        End If

                        Exit While
                    End If
                    rowCount -= 1
                End While
            End If

            MyRow = dt10.NewRow()
            MyRow(1) = cmbPaymentCompanyList.Text.Trim
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
                rowCount = dt3.Rows.Count - 1
                inc = 0
                While (rowCount >= 0)
                    dr3 = dt3.Rows(rowCount)
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
                    rowCount -= 1
                End While
                sda10.Update(ds10, "payment")
                sda3.Update(ds3, "bill")
                MessageBox.Show("Payment successfully added and Accounts updated")
                cmbPaymentCompanyList.Text = ""
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
                cmbPaymentCompanyList.Focus()
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        refreshgrid3()
        'End 'Try
    End Sub

    Private Sub TextBox25_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox25.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            Button43.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox25_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox25.TextChanged
        'Try
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
            'MsgBox("Amount to be credited cannot be greater than balance amount. Please 'Try again")
            TextBox25.Text = ""
            TextBox26.Text = ""
            TextBox27.Text = ""
            TextBox31.Text = ""
        Else
            TextBox31.Text = t25 + t26 + t27
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox26_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox26.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            Button43.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox26_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox26.TextChanged
        'Try
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
            'MsgBox("Amount to be credited cannot be greater than balance amount. Please 'Try again")
            TextBox25.Text = ""
            TextBox26.Text = ""
            TextBox27.Text = ""
            TextBox31.Text = ""
        Else
            TextBox31.Text = t25 + t26 + t27
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox27_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox27.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            Button43.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox27_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox27.TextChanged
        'Try
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
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox31_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox31.TextChanged
        'Try
        If TextBox31.Text = "" And TextBox24.Text <> "" Then
            MsgBox("Amount to be credited cannot be greater than balance amount. Please 'Try again")
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
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox24_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox24.TextChanged
        'Try
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
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub DataGrid4_Navigate(ByVal sender As System.Object, ByVal ne As System.Windows.Forms.NavigateEventArgs) Handles DataGrid4.Navigate

    End Sub

    Private Sub TextBox28_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox28.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            Button43.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox28_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox28.TextChanged

    End Sub

    Private Sub TextBox29_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox29.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            Button43.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox29_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox29.TextChanged

    End Sub

    Private Sub TextBox30_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox30.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            Button43.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox30_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox30.TextChanged

    End Sub

    Private Sub RadioButton5_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles RadioButton5.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            Button43.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub RadioButton6_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles RadioButton6.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            Button43.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Label47_Click(sender As Object, e As EventArgs) Handles Label47.Click

    End Sub

    Private Sub Label87_Click(sender As Object, e As EventArgs) Handles Label87.Click

    End Sub

    Private Sub Label60_Click(sender As Object, e As EventArgs) Handles Label60.Click

    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles lblIGST.Click

    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles lblSGST.Click

    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles lblCGST.Click

    End Sub

    Private Sub TextBox6_TextChanged_1(sender As Object, e As EventArgs) Handles txtIGST.TextChanged

    End Sub

    Private Sub TextBox4_TextChanged_1(sender As Object, e As EventArgs) Handles txtPrintCharge.TextChanged

    End Sub

    Private Sub Label91_Click(sender As Object, e As EventArgs) Handles lblWorkingCharge.Click

    End Sub

    Private Sub Label90_Click(sender As Object, e As EventArgs) Handles lblPrintCharge.Click

    End Sub

    Private Sub btnBillingCreateBill_Click(sender As Object, e As EventArgs) Handles btnBillingCreateBill.Click

        log.Debug("btnBillingCreateBill_Click: entry")

        cmbBillingBillNoList.Text = ""
        Dim custNo = cmbBillingCompanyList.SelectedValue

        dpBillingBillDate.Text = ""
        Dim unPaidDesginAmount As Decimal = getUnBilledDesignAmount(custNo)

        If (unPaidDesginAmount = 0) Then
            MessageBox.Show("There are no designs to bill or all the designs are billed already for this customer")
            Return
        End If

        txtBillingDesignAmount.Text = unPaidDesginAmount

        Dim lastBillRow = getLastBillRow(custNo)

        If lastBillRow IsNot Nothing Then
            Dim lastBillTotalAmount As Decimal = lastBillRow.Item("DesignCost") + lastBillRow.Item("UnPaidAmountTillNow")
            Dim lastBillRemainingBalance As Decimal = lastBillTotalAmount - lastBillRow.Item("PaidAmount")
            txtBillingPrevBalance.Text = lastBillRemainingBalance
            Dim amountToBePaid As Decimal = unPaidDesginAmount + lastBillRemainingBalance
            txtBillingTotalAmount.Text = amountToBePaid
            txtBillingPaidAmount.Text = "0.00"
            txtBillingRemainingBalance.Text = amountToBePaid
        Else
            txtBillingPrevBalance.Text = "0.00"
            txtBillingTotalAmount.Text = unPaidDesginAmount
            txtBillingPaidAmount.Text = "0.00"
            txtBillingRemainingBalance.Text = unPaidDesginAmount
        End If

        cmbBillingBillNoList.Enabled = False
        btnBillingCreateBill.Visible = False
        btnBillingConfirmCreateBill.Visible = True
        btnBillingCancelCreateBill.Visible = True


    End Sub

    Function getLastBillRow(Optional custNo As Integer = Nothing) As DataRow

        Dim billSelectQuery As SqlCommand
        If (custNo <> Nothing) Then
            billSelectQuery = New SqlCommand("select * from bill where billno=(select max(billno) from bill where custno=" + custNo.ToString + ")", dbConnection)
        Else
            billSelectQuery = New SqlCommand("select * from bill where billno=(select max(billno) from bill)", dbConnection)
        End If

        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billSelectQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "bill")
        Dim billTable = billDataSet.Tables(0)

        If (billTable.Rows.Count > 0) Then
            Return billTable.Rows(0)
        End If

        Return Nothing
    End Function

    Private Sub Button3_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnBillingConfirmCreateBill_Click(sender As Object, e As EventArgs) Handles btnBillingConfirmCreateBill.Click
        If cmbBillingCompanyList.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Company Name")
            cmbDesCompanyList.Focus()
        ElseIf Not cmbBillingBillNoList.Text.Trim.Equals("") Then
            MessageBox.Show("Bill Number will be auto generated. Please clear the Bill Number box.")
            cmbBillingBillNoList.Focus()
        ElseIf dpBillingBillDate.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Date")
            dpBillingBillDate.Focus()
        ElseIf txtBillingPrevBalance.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Previous Balance amount")
            txtBillingPrevBalance.Focus()
        ElseIf txtBillingDesignAmount.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Design Amount")
            txtBillingDesignAmount.Focus()
        ElseIf txtBillingTotalAmount.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Total Amount")
            txtBillingTotalAmount.Focus()
        ElseIf txtBillingRemainingBalance.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Remaining Balance Amount")
            txtBillingRemainingBalance.Focus()
        Else

            Dim newBillNo As Integer = -1

            Dim query As String = String.Empty
            query &= "INSERT INTO bill (CustNo, BillDate, DesignCost, UnPaidAmountTillNow, PaidAmount, Cancelled) "
            query &= "VALUES (@CustNo, @BillDate, @DesignCost, @UnPaidAmountTillNow, @PaidAmount, @Cancelled); SELECT SCOPE_IDENTITY()"

            Using comm As New SqlCommand()
                With comm
                    .Connection = dbConnection
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@CustNo", cmbBillingCompanyList.SelectedValue)
                    .Parameters.AddWithValue("@BillDate", dpBillingBillDate.Text)
                    .Parameters.AddWithValue("@DesignCost", Decimal.Parse(txtBillingDesignAmount.Text))
                    .Parameters.AddWithValue("@UnPaidAmountTillNow", Decimal.Parse(txtBillingPrevBalance.Text))
                    .Parameters.AddWithValue("@PaidAmount", Decimal.Parse(txtBillingPaidAmount.Text))
                    .Parameters.AddWithValue("@Cancelled", 0)
                End With
                'comm.ExecuteNonQuery()
                newBillNo = CInt(comm.ExecuteScalar())
            End Using

            updateRecentDesignsAsBilled(cmbBillingCompanyList.SelectedValue, newBillNo)

            MessageBox.Show("Bill successfully added")
            resetBillingScreen()
            loadOrReloadBillList(cmbBillingBillNoList, cmbBillingCompanyList.SelectedValue)
            loadBillingGrid(cmbDesCompanyList.SelectedValue)

            btnBillingCreateBill.Visible = True
            btnBillingConfirmCreateBill.Visible = False
            btnBillingCancelCreateBill.Visible = False
            cmbBillingBillNoList.Enabled = True

            log.debug("btnBillingConfirmCreateBill_Click: Setting cmbBillingBillNoList index to:  " + cmbBillingBillNoList.FindString(newBillNo.ToString).ToString)

            cmbBillingBillNoList.SelectedIndex = cmbBillingBillNoList.FindString(newBillNo.ToString)

            log.debug("btnBillingConfirmCreateBill_Click: cmbBillingBillNoList.SelectedIndex is set to index: " + cmbBillingBillNoList.SelectedIndex.ToString)

            'Catch ex As Exception
            'MessageBox.Show("Message To Agni User:   " & ex.Message & " Or this design Record may already exist")
        End If
    End Sub

    Private Sub btnBillingCancelCreateBill_Click(sender As Object, e As EventArgs) Handles btnBillingCancelCreateBill.Click
        btnBillingCreateBill.Visible = True
        btnBillingConfirmCreateBill.Visible = False
        btnBillingCancelCreateBill.Visible = False
        cmbBillingBillNoList.Enabled = True

        resetBillingScreen()

        Dim lastBillRow = getLastBillRow(cmbBillingCompanyList.SelectedValue)
        Dim lastBillNumber = lastBillRow.Item("BillNo")
        cmbBillingBillNoList.SelectedIndex = cmbBillingBillNoList.FindString(lastBillNumber.ToString)
    End Sub

    Private Sub btnBillingCancelBill_Click(sender As Object, e As EventArgs) Handles btnBillingCancelBill.Click

        If (cmbBillingBillNoList.SelectedIndex = -1) Then
            MessageBox.Show("Please select a bill")
            cmbBillingBillNoList.Focus()
            Return
        End If

        Dim billNo As Integer = cmbBillingBillNoList.SelectedValue

        Dim updateQuery As String = String.Empty
        updateQuery &= "update bill set Cancelled=@Cancelled where BillNo=@BillNo"

        Using comm As New SqlCommand()
            With comm
                .Connection = dbConnection
                .CommandType = CommandType.Text
                .CommandText = updateQuery
                .Parameters.AddWithValue("@Cancelled", True)
                .Parameters.AddWithValue("@BillNo", billNo)
            End With
            comm.ExecuteNonQuery()
        End Using
        updateDesignsAsUnBilled(billNo)

        MessageBox.Show("Bill " + billNo.ToString + " is marked as cancelled bill. You need to create a new bill for the designs which were billed in this bill")

        resetBillingScreen()
        loadOrReloadBillList()
        log.debug("btnBillingCancelBill_Click: setting cmbBillingBillNoList.SelectedIndex to " + cmbBillingBillNoList.FindString(billNo.ToString).ToString)
        cmbBillingBillNoList.SelectedIndex = cmbBillingBillNoList.FindString(billNo.ToString)
        loadBillingGrid(cmbDesCompanyList.SelectedValue)
    End Sub

    Private Sub dgDesDesignDetails_Navigate(sender As Object, ne As NavigateEventArgs)

    End Sub

    Private Sub dgDesDesignDetails_MouseUp(sender As Object, e As MouseEventArgs)

    End Sub

    Private Sub dgBIllingBillDetails_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgBIllingBillDetails.CurrentCellChanged
        cmbBillingBillNoList.SelectedIndex = dgBIllingBillDetails.CurrentRowIndex
    End Sub

    Private Sub radioDesPrint_CheckedChanged(sender As Object, e As EventArgs) Handles radioDesPrint.CheckedChanged
        If radioDesPrint.Checked Then
            lblDesCostPerUnit.Text = "Cost Per Inch"
        End If
    End Sub

    Private Sub TextBox8_TextChanged_1(sender As Object, e As EventArgs) Handles txtSGST.TextChanged

    End Sub

    Private Sub TextBox9_TextChanged_1(sender As Object, e As EventArgs) Handles txtCGST.TextChanged

    End Sub

    Private Sub DateTimePicker6_CloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateTimePicker6.CloseUp
        'Try
        'refreshgrid3()
        'refreshgrid2()
        Dim lastbildate, seldate As DateTime
        Dim lastbildatestr, seldatestr As String
        actpaid = 0
        dt11.Clear()
        key = cmbPaymentCompanyList.Text.Trim
        seldatestr = DateTimePicker6.Value.ToString("MM dd yyyy")
        seldate = DateTime.Parse(seldatestr)
        If dt3.Rows.Count <> 0 Then
            rowCount = dt3.Rows.Count - 1
            inc = 0
            While (rowCount >= 0)
                dr3 = dt3.Rows(rowCount)
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
                rowCount -= 1
            End While
        End If
        rowCount = dt10.Rows.Count - 1
        inc = 0
        While (rowCount >= 0)
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
            rowCount -= 1
            inc += 1
        End While

        DataGrid5.DataSource = ds11.Tables(0)
        Label83.Text = ds11.Tables(0).Rows.Count.ToString
        Label82.Text = actpaid
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        refreshgrid3()
        refreshgrid2()
        'End 'Try
    End Sub

    Private Sub DateTimePicker6_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DateTimePicker6.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            Button43.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub DateTimePicker6_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker6.ValueChanged

    End Sub

    Private Sub DateTimePicker7_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DateTimePicker7.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            Button43.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.R Then
                tabAllTabsHolder.SelectTab(4)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub DateTimePicker7_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker7.ValueChanged

    End Sub

    Private Sub TabPage6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabPayment.Click

    End Sub

    Private Sub Button23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Try
        addrselected = True
        'listbox1.GetSelected(
        AllAddrReport.Show()
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub ListBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Try
        '    If (e.KeyCode = Keys.Enter) Then
        '        Button1.PerformClick()
        If e.Alt Then
            If e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(4)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub



    Private Sub Button42_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button42.Click
        'Try
        If TextBox18.Text.Trim.Equals("") Then
            MessageBox.Show("Select paid bill transaction to delete from payment details")
            TextBox18.Focus()
        Else
            If MessageBox.Show("Do you want to delete this payment transaction ", "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Dim flag1, flag2, flag3 As Boolean
                Dim a1 As Int32
                key = Integer.Parse(TextBox18.Text)

                cmd10 = New SqlCommand("select * from payment", dbConnection)
                sda10 = New SqlDataAdapter()
                sda10.SelectCommand = cmd10
                ds10 = New DataSet
                sda10.Fill(ds10, "payment")
                dt10 = ds10.Tables(0)

                cmd3 = New SqlCommand("select * from bill", dbConnection)
                sda3 = New SqlDataAdapter()
                sda3.SelectCommand = cmd3
                ds3 = New DataSet
                sda3.Fill(ds3, "bill")
                dt3 = ds3.Tables(0)

                rowCount = dt10.Rows.Count - 1
                Dim custname As String = ""
                Dim lastpayno As Integer
                flag2 = False
                While (rowCount >= 0)
                    dr10 = dt10.Rows(rowCount)
                    If key.ToString.Equals(dr10.Item(0).ToString) Then
                        custname = dr10.Item(1)
                        flag2 = True
                        Exit While
                    End If
                    rowCount -= 1
                End While
                If flag2 = False Then
                    MsgBox("Sorry. There is no payment as you selected")
                    Exit Sub
                End If
                Dim lastbillno1 As String = ""
                rowCount = dt10.Rows.Count - 1
                flag3 = False
                While (rowCount >= 0)
                    dr10 = dt10.Rows(rowCount)
                    If custname.ToString.Equals(dr10.Item(1).ToString) Then
                        lastpayno = dr10.Item(0)
                        lastbillno1 = dr10.Item(2)
                        flag3 = True
                        Exit While
                    End If
                    rowCount -= 1
                End While
                If flag3 = False Then
                    MsgBox("Sorry. The Customer has no bills")
                    Exit Sub
                End If

                rowCount = dt10.Rows.Count - 1
                Dim count As Integer = 0
                While (rowCount >= 0)
                    dr10 = dt10.Rows(rowCount)
                    If custname.ToString.Equals(dr10.Item(1).ToString) And lastbillno1.ToString.Equals(dr10.Item(2).ToString) Then
                        count += 1
                    End If
                    rowCount -= 1
                End While

                lastbillno1 = lastbillno1.Substring(lastbillno1.IndexOf("/") + 1, lastbillno1.Length - lastbillno1.IndexOf("/") - 1)
                Dim key1 As String
                If Not lastpayno.ToString.Equals(key.ToString) Then
                    MsgBox("Sorry.. This is not last Payment Transaction. And some of the payment transactions are depending this payment. You are allowed to delete last payment transaction only")
                Else
                    rowCount = dt10.Rows.Count - 1
                    flag = 0
                    While (rowCount >= 0)
                        dr10 = dt10.Rows(rowCount)
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
                                cmbPaymentCompanyList.Text = ""
                                cmbBillingBillNoList.Text = ""
                                RadioButton5.Checked = True
                                flag = 1
                                cmbPaymentCompanyList.Focus()
                            End If
                            Exit While
                        End If
                        rowCount = rowCount - 1
                    End While
                    If flag = 0 Then
                        MessageBox.Show(" Cannot Delete.. There is no such Paid Bill transaction")
                    End If
                End If
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        refreshgrid3()
        refreshgrid2()
        'End 'Try
    End Sub


    Private Sub Button44_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeCustomerName.Click
        'Try
        Label101.Visible = True
        Label102.Visible = True
        txtCustNewName.Visible = True
        btnUpdateName.Visible = True
        btnCancelUpdateName.Visible = True
        btnChangeCustomerName.Visible = False
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try

    End Sub

    Private Sub Button45_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateName.Click
        'Try
        If cmbCustCompanyList.Text.Trim.ToString.Equals("") Then
            MsgBox("Please Select Old Name of the Customer")
            cmbCustCompanyList.Focus()
        ElseIf txtCustNewName.Text.Trim.ToString.Equals("") Then
            MsgBox("Please Enter New Name of the Customer")
            txtCustNewName.Focus()
        ElseIf MessageBox.Show("All Designs, Bills and Payments will be updated to new name belongs to this customer." + vbNewLine + vbNewLine + vbTab + "  Do you want to Change this Customer - " & cmbCustCompanyList.Text & " To - " & txtCustNewName.Text & " ?", "WARNING", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Dim key As String
            Dim a1, a2 As Int32
            key = cmbCustCompanyList.Text
            a1 = Dt2.Rows.Count
            inc = 0
            flag = 0
            While (a1)
                Dr2 = Dt2.Rows(inc)
                If (key.Equals(Dr2.Item(0).ToString())) Then
                    Dr2.BeginEdit()
                    Dr2.Item(0) = txtCustNewName.Text
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
                loadDesignGrid()
            End If

            a2 = dt3.Rows.Count
            inc = 0
            While (a2)
                dr3 = dt3.Rows(inc)
                If (key.Equals(dr3.Item(0).ToString())) Then
                    dr3.BeginEdit()
                    dr3.Item(0) = txtCustNewName.Text
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
                    dr10.Item(1) = txtCustNewName.Text
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

            rowCount = customerTable.Rows.Count
            inc = 0
            While (rowCount)
                DataRow = customerTable.Rows(inc)
                If (key.Equals(DataRow.Item(1).ToString())) Then
                    DataRow.BeginEdit()
                    DataRow.Item(1) = txtCustNewName.Text
                    DataRow.EndEdit()
                    Sda1.SelectCommand = Cmd1
                    Scb1 = New SqlCommandBuilder(Sda1)
                    Sda1.UpdateCommand = Scb1.GetUpdateCommand()
                End If

                If Ds1.HasChanges Then
                    Sda1.Update(Ds1, "customer")
                    MessageBox.Show("The Customer Name is successfully changed")
                    RefreshList1()
                    btnCancelUpdateName.PerformClick()
                    txtOwnerName.Text = ""
                    txtAddress.Text = ""
                    txtMobile.Text = ""
                    txtPrintCharge.Text = ""
                    txtLandline.Text = ""
                    txtIGST.Text = ""
                    txtEmail.Text = ""
                    txtSGST.Text = ""
                    txtCGST.Text = ""
                    txtWebsite.Text = ""
                    txtCustNewName.Text = ""
                    txtGstIn.Text = ""
                    cmbCustCompanyList.Text = ""
                    flag = 1
                    cmbCustCompanyList.Focus()
                    Exit While
                End If
                inc = inc + 1
                rowCount = rowCount - 1
            End While
            If flag = 0 Then
                MessageBox.Show(" Cannot Change.. There is no such company Name")
                cmbCustCompanyList.Focus()
                RefreshList1()
                loadDesignGrid()
                refreshgrid2()
                refreshgrid3()
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        RefreshList1()
        loadDesignGrid()
        refreshgrid2()
        refreshgrid3()
        'End 'Try
    End Sub

    Private Sub Button46_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelUpdateName.Click
        'Try
        Label101.Visible = False
        Label102.Visible = False
        txtCustNewName.Text = ""
        txtCustNewName.Visible = False
        btnUpdateName.Visible = False
        btnCancelUpdateName.Visible = False
        btnChangeCustomerName.Visible = True
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub tabAllTabsHolder_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabAllTabsHolder.SelectedIndexChanged
        'Try
        'Dim i As Integer
        'i = tabAllTabsHolder.SelectedIndex

        'If i = 0 Then
        '    btnCustClear.PerformClick()
        '    btnCancelUpdateName.PerformClick()
        'ElseIf i = 1 Then
        '    btnDesClear.PerformClick()
        'ElseIf i = 2 Then
        '    Button36.PerformClick()
        'ElseIf i = 3 Then
        '    Button47.PerformClick()
        'ElseIf i = 4 Then
        '    radioReportsCompName.Checked = True
        '    cmbReportCompanyList.Text = ""
        '    cmbReportCompanyList.Focus()
        'End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub Button47_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button47.Click
        cmbPaymentCompanyList.Text = ""
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
        cmbPaymentCompanyList.Focus()
        RadioButton5.Checked = True
    End Sub

    Private Sub TextBox19_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBillingRemainingBalance.TextChanged

    End Sub

    Private Sub TextBox17_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBillingTotalAmount.TextChanged

    End Sub

    Private Sub TextBox33_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCustNewName.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            btnUpdateName.PerformClick()
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TextBox33_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCustNewName.TextChanged

    End Sub

    Private Sub ComboBox8_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button48_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub RadioButton8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioReportsBillNo.CheckedChanged
        'Try
        If radioReportsBillNo.Checked Then
            PictureBox1.Image = Nothing
            Label53.Text = radioReportsBillNo.Text
            cmbReportCompanyList.Visible = False
            ComboBox10.Visible = True
            ComboBox10.Text = ""
            ComboBox10.Focus()
            If CheckBox4.Checked Then
                Button26.Text = "Search by Bill Number and Date"
            Else
                Button26.Text = "Search by Bill Number"
            End If

            rowCount = dt3.Rows.Count
            inc = 0
            ComboBox10.Items.Clear()
            While (rowCount)
                dr3 = dt3.Rows(inc)
                ComboBox10.Items.Add(dr3.Item(8).ToString + "/" + dr3.Item(1).ToString)
                inc = inc + 1
                rowCount = rowCount - 1
            End While
        Else
            ComboBox10.Visible = False
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub RadioButton7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioReportsCompName.CheckedChanged
        'Try
        If radioReportsCompName.Checked Then
            PictureBox1.Image = Nothing
            Label53.Text = radioReportsCompName.Text
            ComboBox10.Visible = False
            cmbReportCompanyList.Visible = True
            cmbReportCompanyList.Text = ""
            cmbReportCompanyList.Focus()
            CheckBox1.Enabled = True
            If CheckBox4.Checked Then
                Button26.Text = "Search by Company Name and Date"
            Else
                Button26.Text = "Search by Company Name"
            End If
            inc = 0
            Dim customerQuery = New SqlCommand("select custno,compname from customer", dbConnection)
            Dim customerAdapter = New SqlDataAdapter()
            customerAdapter.SelectCommand = customerQuery
            Dim customerDataSet = New DataSet
            customerAdapter.Fill(customerDataSet, "customer")
            cmbCustCompanyList.ValueMember = "custno"
            cmbCustCompanyList.DisplayMember = "compname"
            cmbCustCompanyList.DataSource = customerDataSet.Tables(0)
        Else
            CheckBox1.Enabled = False
            CheckBox1.Checked = False
            cmbReportCompanyList.Visible = False
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub



    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        'Try
        If CheckBox1.Checked Then
            PictureBox1.Image = Nothing
            cmbReportDesignList.Visible = True
            cmbReportDesignList.Focus()
            If CheckBox4.Checked Then
                Button26.Text = "Search by Company Name, Design Name and Date"
            Else
                Button26.Text = "Search by Company Name and Design Name"
            End If

            cmbReportDesignList.Text = ""
            Dim key As String
            key = cmbReportCompanyList.Text.Trim
            rowCount = Dt2.Rows.Count
            inc = 0
            cmbReportDesignList.Items.Clear()
            While (rowCount)
                Dr2 = Dt2.Rows(inc)
                If (key.Equals(Dr2.Item(0).ToString())) Then
                    newitem = New MyComboitem(Dr2.Item(1), Dr2.Item(2).ToString)
                    cmbReportDesignList.Items.Add(newitem)
                End If
                inc = inc + 1
                rowCount = rowCount - 1
            End While
        Else
            cmbReportDesignList.Visible = False
            If CheckBox4.Checked Then
                Button26.Text = "Search by Company Name and Date"
            Else
                Button26.Text = "Search by Company Name"
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub ComboBox9_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbReportDesignList.KeyDown
        'Try
        If (e.KeyCode = Keys.Enter) Then
            Button26.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub ComboBox9_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbReportDesignList.SelectedIndexChanged
        PictureBox1.Image = Nothing
    End Sub

    Private Sub ComboBox9_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReportDesignList.VisibleChanged
        If cmbReportDesignList.Visible = False Then
            cmbReportDesignList.Items.Clear()
        End If
    End Sub

    Private Sub Button49_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub



    Private Sub CheckBox4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked = True And CheckBox1.Checked = False And radioReportsCompName.Checked = False And radioReportsBillNo.Checked = False Then
            Button26.Text = "Search by Date"
        ElseIf CheckBox4.Checked = True And CheckBox1.Checked = False And radioReportsCompName.Checked = True And radioReportsBillNo.Checked = False Then
            Button26.Text = "Search by Company Name and Date"
        ElseIf CheckBox4.Checked = True And CheckBox1.Checked = True And radioReportsCompName.Checked = True And radioReportsBillNo.Checked = False Then
            Button26.Text = "Search by Company Name, Design Name and Date"
        ElseIf CheckBox4.Checked = True And CheckBox1.Checked = False And radioReportsCompName.Checked = False And radioReportsBillNo.Checked = True Then
            Button26.Text = "Search by Bill Number and Date"
        ElseIf CheckBox4.Checked = False And CheckBox1.Checked = False And radioReportsCompName.Checked = True And radioReportsBillNo.Checked = False Then
            Button26.Text = "Search by Company Name"
        ElseIf CheckBox4.Checked = False And CheckBox1.Checked = False And radioReportsCompName.Checked = False And radioReportsBillNo.Checked = True Then
            Button26.Text = "Search by Bill Number"
        ElseIf CheckBox4.Checked = False And CheckBox1.Checked = True And radioReportsCompName.Checked = True And radioReportsBillNo.Checked = False Then
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
        'Try
        compbased = True
        billbased = False
        desbased = False
        datebased = False
        searchboth = False
        If cmbReportCompanyList.Text.Trim.Equals("") Then
            MsgBox("Please Select Company ")
            cmbReportCompanyList.Focus()
        Else
            Dim totdesamount As Decimal = 0
            Dim countunbilled As Integer = 0
            Dim sumunbilled As Decimal = 0
            Dim countbilled As Integer = 0
            Dim sumbilled As Decimal = 0
            Dim countbill As Integer = 0
            dt6.Clear()
            key = cmbReportCompanyList.Text.Trim
            cmprcomp = key
            rowCount = Dt2.Rows.Count - 1
            inc = 0
            While (rowCount >= 0)
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
                rowCount -= 1
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
            key = cmbReportCompanyList.Text.Trim
            rowCount = dt3.Rows.Count - 1
            inc = 0
            While (rowCount >= 0)
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
                rowCount -= 1
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
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub
    Private Sub searchbyBillno()
        'Try
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
            rowCount = Dt2.Rows.Count - 1
            inc = 0
            While (rowCount >= 0)
                Dr2 = Dt2.Rows(rowCount)
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
                rowCount -= 1
            End While
            DataGrid3.DataSource = ds6.Tables(0)
            Label48.Text = ds6.Tables(0).Rows.Count.ToString
            Label51.Text = totdesamount.ToString
            Label157.Text = countunbilled.ToString
            Label153.Text = sumunbilled.ToString
            Label151.Text = countbilled.ToString
            Label155.Text = sumbilled.ToString

            rowCount = dt3.Rows.Count - 1
            dt7.Clear()
            While (rowCount >= 0)
                dr3 = dt3.Rows(rowCount)
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
                rowCount = rowCount - 1
            End While

            Dim flag As Boolean = False
            Dim bal As Decimal = 0
            rowCount = dt3.Rows.Count - 1
            While (rowCount >= 0)
                dr3 = dt3.Rows(rowCount)
                If custname.Trim.ToString.Equals(dr3.Item(0).ToString) Then
                    countbill += 1
                    tottransamount += Decimal.Parse(dr3.Item(4))
                    If flag = False Then
                        bal = dr3.Item(7)
                        flag = True
                    End If
                End If
                rowCount -= 1
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
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub
    Private Sub searchbyDesno()
        'Try
        compbased = False
        billbased = False
        desbased = True
        datebased = False
        searchboth = False
        PictureBox1.Image = Nothing
        If cmbReportCompanyList.Text.Trim.Equals("") Then
            MsgBox("Please Select Company Name")
            cmbReportCompanyList.Focus()
        ElseIf cmbReportDesignList.Text.Trim.Equals("") Then
            MsgBox("Please Select Design Name")
            cmbReportDesignList.Focus()
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
            item = DirectCast(cmbReportDesignList.SelectedItem, MyComboitem)
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

            rowCount = dt3.Rows.Count - 1
            dt7.Clear()
            While (rowCount >= 0)
                dr3 = dt3.Rows(rowCount)
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
                rowCount = rowCount - 1
            End While

            Dim flag1 As Boolean = False
            Dim bal As Decimal = 0
            rowCount = dt3.Rows.Count - 1
            While (rowCount >= 0)
                dr3 = dt3.Rows(rowCount)
                If custname.Trim.ToString.Equals(dr3.Item(0).ToString) Then
                    tottransamount += Decimal.Parse(dr3.Item(4))
                    countbill += 1
                    If flag1 = False Then
                        bal = dr3.Item(7)
                        flag1 = True
                    End If
                End If
                rowCount -= 1
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
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub
    Private Sub searchbycompdate()

    End Sub
    Private Sub searchbyBilldate()

    End Sub
    Private Sub searchbydesdate()

    End Sub
    Private Sub searchbydate()
        'Try
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
            rowCount = Dt2.Rows.Count - 1
            inc = 0
            While (rowCount >= 0)
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
                rowCount -= 1
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
            rowCount = dt3.Rows.Count - 1
            inc = 0
            While (rowCount >= 0)
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
                rowCount -= 1
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
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub
    Private Sub Button26_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button26.Click
        'Try
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
            If cmbReportCompanyList.Text.Trim.Equals("") Then
                MsgBox("Please Select Company ")
                cmbReportCompanyList.Focus()
            ElseIf DateTimePicker3.Text.Trim.Equals("") Then
                MsgBox("Please Select From Date ")
                DateTimePicker3.Focus()
            ElseIf DateTimePicker4.Text.Trim.Equals("") Then
                MsgBox("Please Select To Date ")
                DateTimePicker4.Focus()
            Else
                searchmore(cmbReportCompanyList.Text.Trim, 0, 0)
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
            If cmbReportCompanyList.Text.Trim.Equals("") Then
                MsgBox("Please Select Company Name")
                cmbReportCompanyList.Focus()
            ElseIf cmbReportDesignList.Text.Trim.Equals("") Then
                MsgBox("Please Select Design Name")
                cmbReportDesignList.Focus()
            ElseIf DateTimePicker3.Text.Trim.Equals("") Then
                MsgBox("Please Select From Date ")
                DateTimePicker3.Focus()
            ElseIf DateTimePicker4.Text.Trim.Equals("") Then
                MsgBox("Please Select To Date ")
                DateTimePicker4.Focus()
            Else
                Dim item As MyComboitem
                item = DirectCast(cmbReportDesignList.SelectedItem, MyComboitem)
                key = item.ID
                searchmoredes(key.ToString, 1, 1)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
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
        rowCount = Dt2.Rows.Count - 1
        inc = 0
        cmprcomp = key
        While (rowCount >= 0)
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
            rowCount -= 1
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
        rowCount = dt3.Rows.Count - 1
        inc = 0
        While (rowCount >= 0)
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
            rowCount -= 1
            inc += 1
        End While

        Dim flag As Boolean = False
        Dim bal As Decimal = 0
        rowCount = dt3.Rows.Count - 1
        While (rowCount >= 0)
            dr3 = dt3.Rows(rowCount)
            If custname.Trim.ToString.Equals(dr3.Item(0).ToString) Then
                tottransamount += Decimal.Parse(dr3.Item(4))
                countbill += 1
                If flag = False Then
                    bal = dr3.Item(7)
                    flag = True
                End If
            End If
            rowCount -= 1
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
        rowCount = Dt2.Rows.Count - 1
        inc = 0
        cmprcomp = key
        While (rowCount >= 0)
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
            rowCount -= 1
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
        rowCount = dt3.Rows.Count - 1
        inc = 0
        While (rowCount >= 0)
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
            rowCount -= 1
            inc += 1
        End While

        Dim flag As Boolean = False
        Dim bal As Decimal = 0
        rowCount = dt3.Rows.Count - 1
        While (rowCount >= 0)
            dr3 = dt3.Rows(rowCount)
            If custname.ToString.Equals(dr3.Item(0).ToString) Then
                tottransamount += Decimal.Parse(dr3.Item(4))
                countbill += 1
                If flag = False Then
                    bal = dr3.Item(7)
                    flag = True
                End If
            End If
            rowCount -= 1
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
        'Try
        If (e.KeyCode = Keys.Enter) Then
            Button26.PerformClick()
        ElseIf e.Alt Then
            If e.KeyCode = Keys.C Then
                tabAllTabsHolder.SelectTab(0)
            ElseIf e.KeyCode = Keys.D Then
                tabAllTabsHolder.SelectTab(1)
            ElseIf e.KeyCode = Keys.B Then
                tabAllTabsHolder.SelectTab(2)
            ElseIf e.KeyCode = Keys.P Then
                tabAllTabsHolder.SelectTab(3)
            ElseIf e.KeyCode = Keys.H Then
                tabAllTabsHolder.SelectTab(5)
            End If
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub TabControl1_DrawItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles tabAllTabsHolder.DrawItem
        Dim BackBrush As Brush = New SolidBrush(System.Drawing.Color.BurlyWood)
        Dim ForeBrush As Brush = New SolidBrush(System.Drawing.Color.Black)
        Dim tabBackgroundRect As Rectangle = e.Bounds
        e.Graphics.FillRectangle(BackBrush, tabBackgroundRect)
        e.Graphics.DrawString(tabAllTabsHolder.TabPages(e.Index).Text, e.Font, ForeBrush, tabBackgroundRect)

        Dim tabstripEndRect As Rectangle = tabAllTabsHolder.GetTabRect(tabAllTabsHolder.TabPages.Count - 1)
        Dim tabstripEndRectF As RectangleF = New RectangleF(tabstripEndRect.X + tabstripEndRect.Width, tabstripEndRect.Y - 5, tabAllTabsHolder.Width - (tabstripEndRect.X + tabstripEndRect.Width), tabstripEndRect.Height + 5)
        'Using backBrush1 As Brush = New SolidBrush(System.Drawing.Color.BlueViolet)
        e.Graphics.FillRectangle(BackBrush, tabstripEndRectF)
        ' End Using
    End Sub

    Private Sub ComboBox10_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox10.SelectedIndexChanged

    End Sub

    Private Sub CheckBox5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox5.CheckedChanged
        'Try
        If CheckBox5.Checked = True Then
            radioReportsCompName.Checked = False
            radioReportsBillNo.Checked = False
            radioReportsCompName.Enabled = False
            radioReportsBillNo.Enabled = False
            Label53.Visible = False
            CheckBox4.Checked = False
            CheckBox4.Enabled = False
            Label54.Visible = True
            Label55.Visible = True
            DateTimePicker3.Visible = True
            DateTimePicker4.Visible = True
            Button26.Text = "Search by Date"
        Else
            radioReportsCompName.Enabled = True
            radioReportsBillNo.Enabled = True
            radioReportsCompName.Checked = True
            Label53.Visible = True
            CheckBox4.Enabled = True
            Label54.Visible = False
            Label55.Visible = False
            DateTimePicker3.Visible = False
            DateTimePicker4.Visible = False
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub


    Private Sub cmbDesDesignList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDesDesignList.SelectedIndexChanged

        If (cmbDesDesignList.SelectedIndex = -1) Then
            Return
        End If

        Dim designNo As Integer = cmbDesDesignList.SelectedValue
        Dim designSelectQuery = New SqlCommand("select * from design where DesignNo=" + designNo.ToString, dbConnection)
        Dim designDataAdapter = New SqlDataAdapter()
        designDataAdapter.SelectCommand = designSelectQuery
        Dim designDataSet = New DataSet
        designDataAdapter.Fill(designDataSet, "design")
        Dim designTable = designDataSet.Tables(0)

        If (designTable.Rows.Count > 0) Then
            Dim dataRow = designTable.Rows(0)
            txtDesHeight.Text = dataRow.Item(3)
            txtDesWidth.Text = dataRow.Item(4)
            txtDesNoOfColors.Text = dataRow.Item(5)
            txtDesCostPerUnit.Text = dataRow.Item(6)
            If dataRow.Item(7).ToString.Equals("WP/Inch") Then
                radioDesWP.Checked = True
            ElseIf dataRow.Item(7).ToString.Equals("Working/Color") Then
                radioDesWorking.Checked = True
            Else
                radioDesPrint.Checked = True
            End If
            If dataRow.Item(8) Is DBNull.Value Then
                pbDesDesignImage.Image = Nothing
            Else
                Dim designImage() As Byte = CType(dataRow.Item(8), Byte())
                Dim designImageBuffer As New MemoryStream(designImage)
                pbDesDesignImage.Image = Image.FromStream(designImageBuffer)
            End If
            txtDesCalculatedPrice.Text = dataRow.Item(9)
            dpDesDesignDate.Text = dataRow.Item(10)
        Else
            MessageBox.Show("No data found for design: " + cmbDesDesignList.Text)
        End If


        ''Catch ex As Exception
        '    'MessageBox.Show("Message to Agni User:   " & ex.Message)
        ''End 'Try
    End Sub

End Class


