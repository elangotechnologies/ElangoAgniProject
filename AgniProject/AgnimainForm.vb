Imports System.Data.SqlClient
Imports System.IO
Imports System.Threading
'Imports NLog

Public Class AgnimainForm
    Dim dbConnection As SqlConnection

    'Dim log As Logger = LogManager.GetCurrentClassLogger()

    Dim BILL_TYPE_UNBILLED As Int16 = 0
    Dim BILL_TYPE_BILLED As Int16 = 1
    Dim BILL_TYPE_ALL As Int16 = 2

    Public gSelectedCustNoIndex As Integer = -1
    Public gSelectedBillNo As Integer = -1
    Public gSelectedCustName As String = String.Empty
    Public gDisplayBillNo As Integer = 0

    Private SEARCH_BY_COMPANY As Integer = 1
    Private SEARCH_BY_DESIGN As Integer = 2
    Private SEARCH_BY_BILL As Integer = 4
    Private SEARCH_BY_DATE As Integer = 8


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
            dbConnection.Close()
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'log.Debug("Form is loading")

        dbConnection = New SqlConnection("server=agni\SQLEXPRESS;Database=agnidatabase;Integrated Security=true; MultipleActiveResultSets=True;")
        dbConnection.Open()

        tabAllTabsHolder.Width = Me.Width
        tabAllTabsHolder.Height = Me.Height
        dpBillingBillDate.Value = DateTime.Today

        Dim lastBillRow As DataRow = getLastBillRow()
        If (lastBillRow IsNot Nothing) Then
            gDisplayBillNo = lastBillRow.Item("DisplayBillNo")
        End If

        'loadCustomerList()
        loadCustomerList()
        loadBillListInReport()

        resetAllScreens()

    End Sub

    Sub loadCustomerList()
        Dim thread As Thread = New Thread(AddressOf getCustomerListTable)
        thread.IsBackground = True
        thread.Start()
    End Sub

    Sub getCustomerListTable()
        'log.Debug("getCustomerListTable: entry")
        Dim customerQuery = New SqlCommand("select CustNo,CompName from customer", dbConnection)
        Dim customerAdapter = New SqlDataAdapter()
        customerAdapter.SelectCommand = customerQuery
        Dim customerDataSet = New DataSet
        customerAdapter.Fill(customerDataSet, "customer")
        Dim customerTable As DataTable = customerDataSet.Tables(0)

        Dim setCustomerListInvoker As New setCustomerListDelegate(AddressOf Me.setCustomerList)
        Me.BeginInvoke(setCustomerListInvoker, customerTable, Nothing)

    End Sub

    Delegate Sub setCustomerListDelegate(customerTable As DataTable, cmbCompanyList As ComboBox)

    Sub setCustomerList(customerTable As DataTable, Optional cmbCompanyList As ComboBox = Nothing)
        'log.Debug("setCustomerList: entry")

        Dim dummyFirstRow As DataRow = customerTable.NewRow()
        dummyFirstRow("CustNo") = -1
        dummyFirstRow("CompName") = "Please select a customer..."
        customerTable.Rows.InsertAt(dummyFirstRow, 0)

        If cmbCompanyList IsNot Nothing Then
            cmbCompanyList.BindingContext = New BindingContext()
            cmbCompanyList.DataSource = customerTable
        Else
            cmbCustCustomerList.BindingContext = New BindingContext()
            cmbCustCustomerList.DataSource = customerTable
            cmbDesCustomerList.BindingContext = New BindingContext()
            cmbDesCustomerList.DataSource = customerTable
            cmbBillingCustomerList.BindingContext = New BindingContext()
            cmbBillingCustomerList.DataSource = customerTable
            cmbPaymentCustomerList.BindingContext = New BindingContext()
            cmbPaymentCustomerList.DataSource = customerTable
            cmbReportCustomerList.BindingContext = New BindingContext()
            cmbReportCustomerList.DataSource = customerTable
        End If

    End Sub

    Sub loadDesignList(custNo As Integer)
        Dim thread As Thread = New Thread(AddressOf getDesignListTable)
        thread.IsBackground = True
        thread.Start(custNo)
    End Sub

    Sub getDesignListTable(ByVal custNoObj As Object)

        Dim custNo = CType(custNoObj, Integer)

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

        Dim setDesignListInvoker As New setDesignListDelegate(AddressOf Me.setDesignList)
        Me.BeginInvoke(setDesignListInvoker, designTable)
    End Sub

    Delegate Sub setDesignListDelegate(designTable As DataTable)

    Sub setDesignList(designTable As DataTable)
        Dim dummyFirstRow As DataRow = designTable.NewRow()
        dummyFirstRow("DesignNo") = -1
        dummyFirstRow("DesignName") = "Please select a design..."
        designTable.Rows.InsertAt(dummyFirstRow, 0)

        cmbDesDesignList.BindingContext = New BindingContext()
        cmbDesDesignList.DataSource = designTable

        resetDesignScreen()

    End Sub

    Sub loadDesignChargePerUnit(custNo As Integer)
        Dim thread As Thread = New Thread(AddressOf getDesignChargePerUnit)
        thread.IsBackground = True
        thread.Start(custNo)
    End Sub

    Sub getDesignChargePerUnit(ByVal custNoObj As Object)
        Dim custNo = CType(custNoObj, Integer)

        Dim customerQuery = New SqlCommand("select WorkingPrintSqrInch, WorkingColor, PrintColor from customer where CustNo=" + custNo.ToString, dbConnection)
        Dim customerAdapter = New SqlDataAdapter()
        customerAdapter.SelectCommand = customerQuery
        Dim customerDataSet = New DataSet
        customerAdapter.Fill(customerDataSet, "customer")
        Dim designChargeCostPerUnitTable As DataTable = customerDataSet.Tables(0)

        Dim setDesignChargePerUnitInvoker As New setDesignChargePerUnitDelegate(AddressOf Me.setDesignChargePerUnit)
        Me.BeginInvoke(setDesignChargePerUnitInvoker, designChargeCostPerUnitTable)
    End Sub

    Delegate Sub setDesignChargePerUnitDelegate(chargingDetailsTable As DataTable)

    Sub setDesignChargePerUnit(chargingDetailsTable As DataTable)

        If (chargingDetailsTable.Rows.Count = 0) Then
            Return
        End If

        If radioDesWP.Checked Then
            lblDesCostPerUnit.Text = "Cost Per Inch"
            txtDesCostPerUnit.Text = chargingDetailsTable.Rows(0).Item("WorkingPrintSqrInch").ToString
        ElseIf radioDesWorking.Checked Then
            lblDesCostPerUnit.Text = "Cost Per Color"
            txtDesCostPerUnit.Text = chargingDetailsTable.Rows(0).Item("WorkingColor").ToString
        ElseIf radioDesPrint.Checked Then
            lblDesCostPerUnit.Text = "Cost Per Inch"
            txtDesCostPerUnit.Text = chargingDetailsTable.Rows(0).Item("PrintColor").ToString
        End If

    End Sub

    Sub loadDesignGrid(custNo As Integer)
        Dim thread As Thread = New Thread(AddressOf getDesignGridTable)
        thread.IsBackground = True
        thread.Start(custNo)
    End Sub

    Sub getDesignGridTable(ByVal custNoObj As Object)

        Dim custNo = CType(custNoObj, Integer)
        Dim designTable As DataTable = fetchDesignTable(custNo)

        Dim setDesignGridInvoker As New setDesignGridDelegate(AddressOf Me.setDesignGrid)
        Me.BeginInvoke(setDesignGridInvoker, designTable)
    End Sub

    Function fetchDesignTable(Optional custNo As Integer = Nothing) As DataTable
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
        Return designDataSet.Tables(0)
    End Function

    Delegate Sub setDesignGridDelegate(designTable As DataTable)

    Sub setDesignGrid(designTable As DataTable)
        dgDesDesignDetails.DataSource = designTable
    End Sub

    Sub loadBillList(custNo As Integer)
        Dim thread As Thread = New Thread(AddressOf getBillListTable)
        thread.IsBackground = True
        thread.Start(custNo)
    End Sub

    Sub loadBillListInReport()
        Dim thread As Thread = New Thread(AddressOf getBillListTable)
        thread.IsBackground = True
        thread.Start(Nothing)
    End Sub

    Sub getBillListTable(ByVal custNoObj As Object)

        Dim custNo = CType(custNoObj, Integer)

        Dim billQuery As SqlCommand
        If (custNo <> Nothing) Then
            billQuery = New SqlCommand("select BillNo, DisplayBillNo from bill where custNo=" + custNo.ToString, dbConnection)
        Else
            billQuery = New SqlCommand("select BillNo, DisplayBillNo from bill", dbConnection)
        End If

        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "bill")
        Dim billTable As DataTable = billDataSet.Tables(0)

        Dim setBillingListInvoker As New setBillingListDelegate(AddressOf Me.setBillingList)
        Me.BeginInvoke(setBillingListInvoker, billTable, Nothing)

    End Sub

    Delegate Sub setBillingListDelegate(billTable As DataTable, cmbBillList As ComboBox)

    Sub setBillingList(billTable As DataTable, Optional cmbBillList As ComboBox = Nothing)

        Dim dummyFirstRow As DataRow = billTable.NewRow()
        dummyFirstRow("BillNo") = -1
        dummyFirstRow("DisplayBillNo") = "Select Bill..."
        billTable.Rows.InsertAt(dummyFirstRow, 0)

        If cmbBillList IsNot Nothing Then
            cmbBillingBillNoList.BindingContext = New BindingContext()
            cmbBillList.DataSource = billTable
        Else
            cmbBillingBillNoList.BindingContext = New BindingContext()
            cmbBillingBillNoList.DataSource = billTable
            cmbReportBillNoList.BindingContext = New BindingContext()
            cmbReportBillNoList.DataSource = billTable
        End If
        resetBillingScreen()
    End Sub

    Sub loadBillGrid(custNo As Integer)
        Dim thread As Thread = New Thread(AddressOf getBillGridTable)
        thread.IsBackground = True
        thread.Start(custNo)
    End Sub

    Sub getBillGridTable(ByVal custNoObj As Object)

        Dim custNo = CType(custNoObj, Integer)
        Dim billTable As DataTable = fetchBillTable(custNo)
        Dim setBillingGridInvoker As New setBillingGridDelegate(AddressOf Me.setBillingGrid)
        Me.BeginInvoke(setBillingGridInvoker, billTable)

    End Sub

    Function fetchBillTable(custNo As Integer) As DataTable

        Dim billQuery As SqlCommand
        If (custNo <> Nothing) Then
            billQuery = New SqlCommand("select BillNo, DisplayBillNo, BillDate, DesignCost, UnPaidAmountTillNow, CGST, CGST*DesignCost/100 as CGSTAmount, 
                            SGST, SGST*DesignCost/100 as SGSTAmount, IGST, IGST*DesignCost/100 as IGSTAmount, (CGST+ SGST+ IGST)*DesignCost/100 as GSTAmount, ((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost as DesignAmountGST, PaidAmount, 
                            ((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost+UnPaidAmountTillNow as TotalAmount, 
                            (((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost+UnPaidAmountTillNow)-PaidAmount as RemainingBalance, Cancelled  from bill where custNo=" + custNo.ToString, dbConnection)
        Else
            billQuery = New SqlCommand("select BillNo, DisplayBillNo, BillDate, DesignCost, UnPaidAmountTillNow, CGST, CGST*DesignCost/100 as CGSTAmount, 
                            SGST, SGST*DesignCost/100 as SGSTAmount, IGST, IGST*DesignCost/100 as IGSTAmount, (CGST+ SGST+ IGST)*DesignCost/100 as GSTAmount, ((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost as DesignAmountGST, PaidAmount, 
                            ((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost+UnPaidAmountTillNow as TotalAmount, 
                            (((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost+UnPaidAmountTillNow)-PaidAmount as RemainingBalance, Cancelled  from bill", dbConnection)
        End If

        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "bill")
        Return billDataSet.Tables(0)
    End Function

    Delegate Sub setBillingGridDelegate(billTable As DataTable)

    Sub setBillingGrid(billTable As DataTable)
        dgBIllingBillDetails.DataSource = billTable
    End Sub

    Sub loadPaymentList(custNo As Integer)
        Dim thread As Thread = New Thread(AddressOf getPaymentListTable)
        thread.IsBackground = True
        thread.Start(custNo)
    End Sub

    Sub getPaymentListTable(ByVal custNoObj As Object)
        'log.Debug("getPaymentListTable: entry")
        Dim custNo = CType(custNoObj, Integer)

        Dim paymentQuery As SqlCommand
        If (custNo <> Nothing) Then
            paymentQuery = New SqlCommand("select PaymentNo, CONVERT(varchar(11), PaymentNo) as DisplayPaymentNo  from payment where custNo=" + custNo.ToString, dbConnection)
        Else
            paymentQuery = New SqlCommand("select PaymentNo, CONVERT(varchar(11), PaymentNo) as DisplayPaymentNo  from payment", dbConnection)
        End If

        Dim paymentAdapter = New SqlDataAdapter()
        paymentAdapter.SelectCommand = paymentQuery
        Dim paymentDataSet = New DataSet
        paymentAdapter.Fill(paymentDataSet, "payment")
        Dim paymentTable As DataTable = paymentDataSet.Tables(0)

        Dim setPaymentListInvoker As New setPaymentListDelegate(AddressOf Me.setPaymentList)
        Me.BeginInvoke(setPaymentListInvoker, paymentTable, Nothing)

    End Sub

    Delegate Sub setPaymentListDelegate(paymentTable As DataTable, cmbPaymentList As ComboBox)

    Sub setPaymentList(paymentTable As DataTable, Optional cmbPaymentList As ComboBox = Nothing)

        Dim dummyFirstRow As DataRow = paymentTable.NewRow()
        dummyFirstRow("PaymentNo") = -1
        dummyFirstRow("DisplayPaymentNo") = "Select Payment..."
        paymentTable.Rows.InsertAt(dummyFirstRow, 0)

        If cmbPaymentList IsNot Nothing Then
            cmbPaymentList.BindingContext = New BindingContext()
            cmbPaymentList.DataSource = paymentTable
        Else
            cmbPaymentPaymentNoList.BindingContext = New BindingContext()
            cmbPaymentPaymentNoList.DataSource = paymentTable
        End If

        resetPaymentScreen()

    End Sub

    Sub loadPaymentGrid(custNo As Integer)
        Dim thread As Thread = New Thread(AddressOf getPaymentGridTable)
        thread.IsBackground = True
        thread.Start(custNo)
    End Sub

    Sub getPaymentGridTable(ByVal custNoObj As Object)

        Dim custNo = CType(custNoObj, Integer)

        'log.Debug("getPaymentGridTable: entry")
        Dim paymentQuery As SqlCommand
        If (custNo <> Nothing) Then
            paymentQuery = New SqlCommand("select p.*, p.UnPaidBilledAmount - p.FinalPaidAmount as NetBalance, b.DisplayBillNo  from payment p, bill b where p.BillNo = b.BillNo and p.custNo=" + custNo.ToString, dbConnection)
        Else
            paymentQuery = New SqlCommand("select p.*, p.UnPaidBilledAmount - p.FinalPaidAmount as NetBalance, b.DisplayBillNo  from payment p, bill b where p.BillNo = b.BillNo", dbConnection)
        End If

        Dim paymentAdapter = New SqlDataAdapter()
        paymentAdapter.SelectCommand = paymentQuery
        Dim paymentDataSet = New DataSet
        paymentAdapter.Fill(paymentDataSet, "payment")
        Dim paymentTable As DataTable = paymentDataSet.Tables(0)

        Dim setPaymentGridInvoker As New setPaymentGridDelegate(AddressOf Me.setPaymentGrid)
        Me.BeginInvoke(setPaymentGridInvoker, paymentTable)
    End Sub

    Delegate Sub setPaymentGridDelegate(paymentTable As DataTable)

    Sub setPaymentGrid(paymentTable As DataTable)
        dgPaymentDetails.DataSource = paymentTable
    End Sub


    Function getDesignAmountWithGSTTax(billedType As Int16, Optional custNo As Integer = Nothing) As Decimal

        Dim designQueryString = "select sum(designAmountWithTax) from (
                        select d.designNo, d.CustNo, c.CGST,c.SGST,c.IGST,d.Price,d.Price+((isnull(c.CGST,0)+isnull(c.SGST,0)+isnull(c.IGST,0))*d.Price/100) 
                        as designAmountWithTax from design d, Customer c where d.CustNo=1 and d.Billed=0 and d.custno = c.custno) as custDesignTable"

        If (billedType = BILL_TYPE_BILLED Or billedType = BILL_TYPE_UNBILLED) Then
            If (custNo <> Nothing) Then
                designQueryString = "select sum(designAmountWithTax) from (
                        select d.designNo, d.CustNo, c.CGST,c.SGST,c.IGST,d.Price,d.Price+((isnull(c.CGST,0)+isnull(c.SGST,0)+isnull(c.IGST,0))*d.Price/100) 
                        as designAmountWithTax from design d, Customer c where d.CustNo=" + custNo.ToString + " and d.Billed=" + billedType.ToString +
                        " and d.custno = c.custno) as custDesignTable"
            Else
                designQueryString = "select sum(designAmountWithTax) from (
                        select d.designNo, d.CustNo, c.CGST,c.SGST,c.IGST,d.Price,d.Price+((isnull(c.CGST,0)+isnull(c.SGST,0)+isnull(c.IGST,0))*d.Price/100) 
                        as designAmountWithTax from design d, Customer c where d.Billed=" + billedType.ToString +
                        " and d.custno = c.custno) as custDesignTable"
            End If
        ElseIf billedType = BILL_TYPE_ALL And custNo <> Nothing Then
            designQueryString = "select sum(designAmountWithTax) from (
                        select d.designNo, d.CustNo, c.CGST,c.SGST,c.IGST,d.Price,d.Price+((isnull(c.CGST,0)+isnull(c.SGST,0)+isnull(c.IGST,0))*d.Price/100) 
                        as designAmountWithTax from design d, Customer c where d.CustNo=" + custNo.ToString + " and d.custno = c.custno) as custDesignTable"
        End If

        Dim designQuery As SqlCommand = New SqlCommand(designQueryString, dbConnection)

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

    Function getDesignAmountWithoutGSTTax(billedType As Int16, Optional custNo As Integer = Nothing) As Decimal

        Dim designQueryString = "select sum(price) from design"

        If (billedType = BILL_TYPE_BILLED Or billedType = BILL_TYPE_UNBILLED) Then
            If (custNo <> Nothing) Then
                designQueryString = "select sum(price) from design where CustNo=" + custNo.ToString + " and Billed=" + billedType.ToString
            Else
                designQueryString = "select sum(price) from design where Billed=" + billedType.ToString
            End If
        ElseIf billedType = BILL_TYPE_ALL And custNo <> Nothing Then
            designQueryString = "select sum(price) from design where CustNo=" + custNo.ToString
        End If

        Dim designQuery As SqlCommand = New SqlCommand(designQueryString, dbConnection)

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

    Sub loadGSTForCustomerInBilling(custNo As Integer)
        'Try

        Dim customerQuery = New SqlCommand("select ISNULL(CGST, 0) AS CGST, ISNULL(SGST, 0) AS SGST, ISNULL(IGST, 0) AS IGST from customer where CustNo=" + custNo.ToString, dbConnection)
        Dim customerAdapter = New SqlDataAdapter()
        customerAdapter.SelectCommand = customerQuery
        Dim customerDataSet = New DataSet
        customerAdapter.Fill(customerDataSet, "customer")
        Dim customerTable As DataTable = customerDataSet.Tables(0)

        If (customerTable.Rows.Count = 0) Then
            Return
        End If

        Dim CGST As Decimal = customerTable.Rows(0).Item("CGST")
        Dim SGST As Decimal = customerTable.Rows(0).Item("SGST")
        Dim IGST As Decimal = customerTable.Rows(0).Item("IGST")

        txtBillingCGSTPercent.Text = CGST.ToString
        txtBillingSGSTPercent.Text = SGST.ToString
        txtBillingIGSTPercent.Text = IGST.ToString

        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub


    Private Sub cmbCustCompanyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCustCustomerList.SelectedIndexChanged
        'Try
        gSelectedCustNoIndex = cmbCustCustomerList.SelectedIndex

        If (cmbCustCustomerList.SelectedIndex = -1 Or cmbCustCustomerList.SelectedValue = -1) Then
            resetCustomerScreen()
            Return
        End If

        Dim custNo As Integer = cmbCustCustomerList.SelectedValue

        Dim custSelectQuery = New SqlCommand("select * from customer where custno=" + custNo.ToString, dbConnection)
        Dim customerDataAdapter = New SqlDataAdapter()
        customerDataAdapter.SelectCommand = custSelectQuery
        Dim customerDataSet = New DataSet
        customerDataAdapter.Fill(customerDataSet, "customer")
        Dim customerTable = customerDataSet.Tables(0)

        If (customerTable.Rows.Count > 0) Then
            Dim dataRow = customerTable.Rows(0)
            txtGstIn.Text = dataRow.Item("GSTIN")
            txtOwnerName.Text = dataRow.Item("OwnerName")
            txtAddress.Text = dataRow.Item("Address")
            txtMobile.Text = dataRow.Item("Mobile")
            txtLandline.Text = dataRow.Item("Landline")
            txtEmail.Text = dataRow.Item("Email")
            txtWebsite.Text = dataRow.Item("Website")
            txtCustCGST.Text = dataRow.Item("CGST").ToString
            txtCustSGST.Text = dataRow.Item("SGST").ToString
            txtCustIGST.Text = dataRow.Item("IGST").ToString
            txtCustWPCharge.Text = dataRow.Item("WorkingPrintSqrInch").ToString
            txtCustWorkingCharge.Text = dataRow.Item("WorkingColor").ToString
            txtCustPrintCharge.Text = dataRow.Item("PrintColor").ToString
        Else
            MessageBox.Show("No data found for customer: " + custNo + "-" + cmbCustCustomerList.Text)
        End If


        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try

    End Sub

    Sub resetAllScreens()
        cmbCustCustomerList.SelectedIndex = -1
        cmbDesCustomerList.SelectedIndex = -1
        cmbBillingCustomerList.SelectedIndex = -1
        cmbPaymentCustomerList.SelectedIndex = -1
        cmbReportCustomerList.SelectedIndex = -1
    End Sub

    Sub resetCustomerScreen()
        txtGstIn.Text = ""
        txtOwnerName.Text = ""
        txtAddress.Text = ""
        txtMobile.Text = ""
        txtEmail.Text = ""
        txtLandline.Text = ""
        txtWebsite.Text = ""
        txtCustCGST.Text = ""
        txtCustSGST.Text = ""
        txtCustIGST.Text = ""
        txtCustWPCharge.Text = ""
        txtCustWorkingCharge.Text = ""
        txtCustPrintCharge.Text = ""
        cmbCustCustomerList.Focus()
    End Sub

    Sub resetDesignScreen()
        radioDesWP.Checked = True
        txtDesWidth.Text = ""
        txtDesHeight.Text = ""
        txtDesNoOfColors.Text = ""
        If cmbDesCustomerList.SelectedIndex = -1 Or cmbDesCustomerList.SelectedValue = -1 Then
            txtDesCostPerUnit.Text = ""
        Else
            loadDesignChargePerUnit(cmbDesCustomerList.SelectedValue)
        End If
        txtDesCalculatedPrice.Text = ""
        pbDesDesignImage.Image = Nothing
        cmbDesDesignList.Focus()
    End Sub

    Sub resetBillingControlsVisibilities()
        cmbBillingBillNoList.Enabled = True
        btnBillingCreateBill.Visible = True
        btnBillingConfirmCreateBill.Visible = False
        btnBillingCancelCreateBill.Visible = False
        btnBillingCancelBill.Text = "Mark Cancelled"
        lblCancelledBillIndicator.Visible = False
    End Sub

    Sub setBillingControlsVisibilitiesForCreateBill()
        cmbBillingBillNoList.Enabled = False
        btnBillingCreateBill.Visible = False
        btnBillingConfirmCreateBill.Visible = True
        btnBillingCancelCreateBill.Visible = True
    End Sub

    Sub resetBillingScreen()
        resetBillingControlsVisibilities()

        dpBillingBillDate.Text = ""
        txtBillingActualBillNo.Text = ""
        txtBillingPrevBalance.Text = ""
        txtBillingDesignAmoutBeforeGST.Text = ""
        txtBillingCGSTPercent.Text = ""
        txtBillingSGSTPercent.Text = ""
        txtBillingIGSTPercent.Text = ""
        txtBillingCGSTAmount.Text = ""
        txtBillingSGSTAmount.Text = ""
        txtBillingIGSTAmount.Text = ""
        txtBillingTotalGSTAmount.Text = ""
        txtBillingDesignAmoutAfterGST.Text = ""
        txtBillingTotalAmount.Text = ""
        txtBillingPaidAmount.Text = ""
        txtBillingRemainingBalance.Text = ""
        cmbBillingBillNoList.Focus()
    End Sub

    Sub resetPaymentControlsVisibilities()
        btnPaymentCreatePayment.Visible = True
        btnPaymentConfirmCreatePayment.Visible = False
        btnPaymentCancelCreatePayment.Visible = False
        cmbPaymentPaymentNoList.Enabled = True
        txtPaymentActualPaidAmount.ReadOnly = True
        txtPaymentDiscountAmount.ReadOnly = True
        txtPaymentTaxDeductionAmount.ReadOnly = True
        dpPaymentDate.Enabled = False
        btnPaymentDelete.Visible = True
        btnPaymentClear.Visible = True
    End Sub

    Sub setPaymentControlsVisibilitiesForCreatePayment()
        cmbPaymentPaymentNoList.Enabled = False
        btnPaymentCreatePayment.Visible = False
        btnPaymentConfirmCreatePayment.Visible = True
        btnPaymentCancelCreatePayment.Visible = True
        txtPaymentActualPaidAmount.ReadOnly = False
        txtPaymentDiscountAmount.ReadOnly = False
        txtPaymentTaxDeductionAmount.ReadOnly = False
        dpPaymentDate.Enabled = True
        btnPaymentDelete.Visible = False
        btnPaymentClear.Visible = False
    End Sub

    Sub resetPaymentScreen()
        'log.Debug("resetPaymentScreen: resetting payment screen")

        resetPaymentControlsVisibilities()

        txtPaymentBillNo.Text = ""
        txtPaymentDisplayBillNo.Text = ""
        dpPaymentDate.Text = ""
        radioPaymentByCash.Checked = True
        txtPaymentActualPaidAmount.Text = ""
        txtPaymentDiscountAmount.Text = ""
        txtPaymentTaxDeductionAmount.Text = ""
        txtPaymentFinalPaidAmount.Text = ""
        txtPaymentChequeNo.Text = ""
        txtPaymentBankName.Text = ""
        dpPaymentChequeDate.Text = ""
        txtPaymentNetBalance.Text = ""
        txtPaymentRemarks.Text = ""
        txtPaymentUnPaidBilledAmount.Text = ""
    End Sub

    Public Sub btnCustAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustAdd.Click
        Try
            If cmbCustCustomerList.Text.Trim.Equals("") Or cmbCustCustomerList.Text.Trim.Equals("Please select a customer...") Then
                MessageBox.Show("Enter Valid company Name")
                cmbCustCustomerList.Focus()
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
                query &= "CGST, SGST, IGST, WorkingPrintSqrInch, WorkingColor, PrintColor) "
                query &= "VALUES ( @CompName, @GSTIN, @OwnerName, @Address, @Mobile, @Landline, @Email, "
                query &= "@Website, @CGST, @SGST, @IGST, @WorkingPrintSqrInch, @WorkingColor, @PrintColor)"

                Using comm As New SqlCommand()
                    With comm
                        .Connection = dbConnection
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@CompName", cmbCustCustomerList.Text)
                        .Parameters.AddWithValue("@GSTIN", txtGstIn.Text)
                        .Parameters.AddWithValue("@OwnerName", txtOwnerName.Text)
                        .Parameters.AddWithValue("@Address", txtAddress.Text)
                        .Parameters.AddWithValue("@Mobile", txtMobile.Text)
                        .Parameters.AddWithValue("@Landline", txtLandline.Text)
                        .Parameters.AddWithValue("@Email", txtEmail.Text)
                        .Parameters.AddWithValue("@Website", txtWebsite.Text)
                        .Parameters.AddWithValue("@CGST", If(String.IsNullOrEmpty(txtCustCGST.Text), DBNull.Value, txtCustCGST.Text))
                        .Parameters.AddWithValue("@SGST", If(String.IsNullOrEmpty(txtCustSGST.Text), DBNull.Value, txtCustSGST.Text))
                        .Parameters.AddWithValue("@IGST", If(String.IsNullOrEmpty(txtCustIGST.Text), DBNull.Value, txtCustIGST.Text))
                        .Parameters.AddWithValue("@WorkingPrintSqrInch", If(String.IsNullOrEmpty(txtCustWPCharge.Text), DBNull.Value, txtCustWPCharge.Text))
                        .Parameters.AddWithValue("@WorkingColor", If(String.IsNullOrEmpty(txtCustWorkingCharge.Text), DBNull.Value, txtCustWorkingCharge.Text))
                        .Parameters.AddWithValue("@PrintColor", If(String.IsNullOrEmpty(txtCustPrintCharge.Text), DBNull.Value, txtCustPrintCharge.Text))
                    End With
                    comm.ExecuteNonQuery()
                End Using
                MessageBox.Show("Company successfully added")
                loadCustomerList()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message & " (Or) This Customer Record may already exist")
        End Try
    End Sub

    Private Sub btnCustDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustDelete.Click
        'Try

        If (cmbCustCustomerList.SelectedIndex = -1 Or cmbCustCustomerList.SelectedValue = -1) Then
            MessageBox.Show("Please select a customer")
            cmbCustCustomerList.Focus()
            Return
        End If

        If MessageBox.Show("All Designs, Bills and Payments will be deleted belongs to this customer." + vbNewLine + vbNewLine + "  Do you want to delete this Customer - " & cmbCustCustomerList.Text & " ?", "WARNING", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            VerifyingDelete.Button1.Text = "Delete "
            VerifyingDelete.Button1.Text += cmbCustCustomerList.Text
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
    Public Sub deleteSeletectedCustomer()

        If cmbCustCustomerList.SelectedIndex = -1 Or cmbCustCustomerList.SelectedValue = -1 Then
            MessageBox.Show("Please select a customer")
            cmbCustCustomerList.Focus()
            Return
        End If

        Dim query As String = String.Empty
        query &= "DELETE FROM customer where CustNo=@CustNo"

        Using comm As New SqlCommand()
            With comm
                .Connection = dbConnection
                .CommandType = CommandType.Text
                .CommandText = query
                .Parameters.AddWithValue("@CustNo", cmbCustCustomerList.SelectedValue)
            End With
            comm.ExecuteNonQuery()
        End Using
        MessageBox.Show("Company successfully deleted")
        loadCustomerList()
        resetAllScreens()
    End Sub

    Public Function deleteSelectedPayment() As Boolean

        If cmbPaymentPaymentNoList.SelectedIndex = -1 Then
            MessageBox.Show("Please select a payment from payment list")
            cmbPaymentPaymentNoList.Focus()
            Return False
        End If

        Dim paymentNo As Integer = cmbPaymentPaymentNoList.SelectedValue

        Dim query As String = String.Empty
        query &= "DELETE FROM payment where PaymentNo=@paymentNo"

        Using comm As New SqlCommand()
            With comm
                .Connection = dbConnection
                .CommandType = CommandType.Text
                .CommandText = query
                .Parameters.AddWithValue("@paymentNo", paymentNo)
            End With
            comm.ExecuteNonQuery()
        End Using

        Return True
    End Function

    Private Sub btnCustUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustUpdate.Click
        'Try
        If (cmbCustCustomerList.SelectedIndex = -1 Or cmbCustCustomerList.SelectedValue = -1) Then
            MessageBox.Show("Please select a customer")
            cmbCustCustomerList.Focus()
            Return
        End If

        If cmbCustCustomerList.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid company Name")
            cmbCustCustomerList.Focus()
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
            Dim custNo As Integer = cmbCustCustomerList.SelectedValue
            Dim query As String = String.Empty
            query &= "UPDATE customer SET CompName=@CompName, GSTIN=@GSTIN, OwnerName=@OwnerName, Address=@Address,"
            query &= "Mobile=@Mobile, Landline=@Landline, Email=@Email, Website=@Website, CGST=@CGST, SGST=@SGST, "
            query &= "IGST=@IGST, WorkingPrintSqrInch=@WorkingPrintSqrInch, WorkingColor=@WorkingColor, PrintColor=@PrintColor where CustNo=@CustNo"

            Using comm As New SqlCommand()
                With comm
                    .Connection = dbConnection
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@CustNo", custNo)
                    .Parameters.AddWithValue("@CompName", cmbCustCustomerList.Text)
                    .Parameters.AddWithValue("@GSTIN", txtGstIn.Text)
                    .Parameters.AddWithValue("@OwnerName", txtOwnerName.Text)
                    .Parameters.AddWithValue("@Address", txtAddress.Text)
                    .Parameters.AddWithValue("@Mobile", txtMobile.Text)
                    .Parameters.AddWithValue("@Landline", txtLandline.Text)
                    .Parameters.AddWithValue("@Email", txtEmail.Text)
                    .Parameters.AddWithValue("@Website", txtWebsite.Text)
                    .Parameters.AddWithValue("@CGST", If(String.IsNullOrEmpty(txtCustCGST.Text), DBNull.Value, txtCustCGST.Text))
                    .Parameters.AddWithValue("@SGST", If(String.IsNullOrEmpty(txtCustSGST.Text), DBNull.Value, txtCustSGST.Text))
                    .Parameters.AddWithValue("@IGST", If(String.IsNullOrEmpty(txtCustIGST.Text), DBNull.Value, txtCustIGST.Text))
                    .Parameters.AddWithValue("@WorkingPrintSqrInch", If(String.IsNullOrEmpty(txtCustWPCharge.Text), DBNull.Value, txtCustWPCharge.Text))
                    .Parameters.AddWithValue("@WorkingColor", If(String.IsNullOrEmpty(txtCustWorkingCharge.Text), DBNull.Value, txtCustWorkingCharge.Text))
                    .Parameters.AddWithValue("@PrintColor", If(String.IsNullOrEmpty(txtCustPrintCharge.Text), DBNull.Value, txtCustPrintCharge.Text))
                End With
                comm.ExecuteNonQuery()
            End Using
            MessageBox.Show("Company successfully updated")
            resetCustomerScreen()
            loadCustomerList()
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message & " Or this Customer Record may not exist")
        'End 'Try
    End Sub


    Private Sub btnDesAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesAdd.Click
        'Try
        If (cmbDesCustomerList.SelectedIndex = -1 Or cmbDesCustomerList.SelectedValue = -1) Then
            MessageBox.Show("Please select a customer")
            cmbDesCustomerList.Focus()
        ElseIf cmbDesDesignList.Text.Trim.Equals("") Or cmbDesDesignList.Text.Trim.Equals("Please select a design...") Then
            MessageBox.Show("Enter Valid Design Name")
            cmbDesDesignList.Focus()
        ElseIf dpDesDesignDate.Text.Trim.Equals("") Then
            MessageBox.Show("Choose Valid date")
            dpDesDesignDate.Focus()
        ElseIf cmbDesCustomerList.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Company Name")
            cmbDesCustomerList.Focus()
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

            Dim custNo As Integer = cmbDesCustomerList.SelectedValue

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
                    .Parameters.AddWithValue("@CustNo", custNo)
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
            loadDesignList(custNo)
            loadDesignGrid(custNo)

            'Catch ex As Exception
            'MessageBox.Show("Message to Agni User:   " & ex.Message & " Or this design Record may already exist")
        End If
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

        If (cmbDesDesignList.Items.Count > 0) Then
            cmbDesDesignList.SelectedValue = -1
        Else
            cmbDesDesignList.SelectedIndex = -1
        End If

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
            RemoveHandler cmbDesDesignList.SelectedIndexChanged, AddressOf cmbDesDesignList_SelectedIndexChanged
            desname = strFilename.Substring(0, strFilename.LastIndexOf("."))
            cmbDesDesignList.Text = desname
            AddHandler cmbDesDesignList.SelectedIndexChanged, AddressOf cmbDesDesignList_SelectedIndexChanged
            'cmbDesDesignName.SelectedText = desname
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try

    End Sub
    Private Sub btnDesUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesUpdate.Click
        ''Try

        If cmbDesDesignList.SelectedIndex = -1 Or cmbDesDesignList.SelectedValue = -1 Then
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
        ElseIf cmbDesCustomerList.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Company Name")
            cmbDesCustomerList.Focus()
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

            Dim custNo As Integer = cmbDesCustomerList.SelectedValue

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
            loadDesignList(custNo)
            loadDesignGrid(custNo)
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
        query &= "update design set Billed=@Billed, BillNo=@BillNo where CustNo=@CustNo and BillNo IS NULL"

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
        loadDesignList(custNo)
        loadDesignGrid(custNo)
    End Sub

    Sub updateDesignsAsUnBilled(BillNo As Integer)

        If (BillNo = -1) Then
            Return
        End If

        Dim custNo As Integer = cmbDesCustomerList.SelectedValue

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
        loadDesignList(custNo)
        loadDesignGrid(custNo)
    End Sub

    Sub addPaidAmountInBill(BillNo As Integer, paidAmount As Decimal)

        If (BillNo = -1) Then
            Return
        End If

        Dim custNo As Integer = cmbDesCustomerList.SelectedValue

        Dim query As String = String.Empty
        query &= "update bill set PaidAmount=PaidAmount+@paidAmount where BillNo=@BillNo"

        Using comm As New SqlCommand()
            With comm
                .Connection = dbConnection
                .CommandType = CommandType.Text
                .CommandText = query
                .Parameters.AddWithValue("@BillNo", BillNo)
                .Parameters.AddWithValue("@paidAmount", paidAmount)
            End With
            comm.ExecuteNonQuery()
        End Using
        loadDesignList(custNo)
        loadDesignGrid(custNo)
    End Sub

    Private Sub btnDesDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesDelete.Click
        'Try
        If cmbDesDesignList.SelectedIndex = -1 Or cmbDesDesignList.SelectedValue = -1 Then
            MessageBox.Show("Please select a design")
            cmbDesDesignList.Focus()
            Return
        End If

        If MessageBox.Show("Do you want to delete the design " & cmbDesDesignList.Text, "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Dim custNo As Integer = cmbDesCustomerList.SelectedValue

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

            loadDesignList(custNo)
            loadDesignGrid(custNo)
        End If

        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub cmbBillingBillNoList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBillingBillNoList.SelectedIndexChanged
        'Try
        'log.Debug("cmbBillingBillNoList_SelectedIndexChanged: entry")

        'log.Debug("cmbBillingBillNoList_SelectedIndexChanged: cmbBillingBillNoList.SelectedIndex: " + cmbBillingBillNoList.SelectedIndex.ToString)

        If (cmbBillingBillNoList.SelectedIndex = -1 Or cmbBillingBillNoList.SelectedValue = -1) Then
            resetBillingScreen()
            Return
        End If

        Dim billNo As Integer = cmbBillingBillNoList.SelectedValue

        Dim billSelectQuery = New SqlCommand("select * from bill where BillNo=" + billNo.ToString, dbConnection)
        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billSelectQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "bill")
        Dim billTable As DataTable = billDataSet.Tables(0)

        If (billTable.Rows.Count > 0) Then
            Dim dataRow As DataRow = billTable.Rows(0)
            txtBillingActualBillNo.Text = dataRow.Item("BillNo")
            dpBillingBillDate.Text = dataRow.Item("BillDate")
            txtBillingPrevBalance.Text = dataRow.Item("UnPaidAmountTillNow")
            txtBillingDesignAmoutBeforeGST.Text = dataRow.Item("DesignCost")
            txtBillingCGSTPercent.Text = dataRow.Item("CGST")
            txtBillingSGSTPercent.Text = dataRow.Item("SGST")
            txtBillingIGSTPercent.Text = dataRow.Item("IGST")
            calculateGSTAmountInBilling()
            Dim billingTotalAmount = dataRow.Item("UnPaidAmountTillNow") + Decimal.Parse(txtBillingDesignAmoutAfterGST.Text)
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
            ''log.Debug("cmbBillingBillNoList_SelectedIndexChanged: No data found for Bill: " + billNo.ToString)
        End If

        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub cmbBillingCompanyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBillingCustomerList.SelectedIndexChanged
        'Try
        If (cmbBillingCustomerList.SelectedIndex = -1 Or cmbBillingCustomerList.SelectedValue = -1) Then
            If (cmbBillingBillNoList.Items.Count > 0) Then
                cmbBillingBillNoList.SelectedValue = -1
            Else
                cmbBillingBillNoList.SelectedIndex = -1
            End If
            Return
        End If

        Dim custNo As Integer = cmbBillingCustomerList.SelectedValue
        loadBillList(custNo)
        loadBillGrid(custNo)
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub btnBillingPrintBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBillingPrintBill.Click
        'Try
        If cmbBillingBillNoList.SelectedIndex = -1 Or cmbBillingBillNoList.SelectedValue = -1 Then
            MsgBox("Please select a bill to print")
            cmbBillingBillNoList.Focus()
            Return
        End If

        gSelectedBillNo = cmbBillingBillNoList.SelectedValue
        gSelectedCustName = cmbBillingCustomerList.Text

        BillReportForm.Show()
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub cmbDesCompanyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDesCustomerList.SelectedIndexChanged
        'Try
        If (cmbDesCustomerList.SelectedIndex = -1 Or cmbDesCustomerList.SelectedValue = -1) Then

            'Dim dummyDesignListTable As DataTable = New DataTable
            'dummyDesignListTable.TableName = "designName"
            'Dim designNoColumn As DataColumn = New DataColumn("designNo", Type.GetType("System.Int32"))
            'Dim designNameColumn As DataColumn = New DataColumn("designName", Type.GetType("System.String"))
            'dummyDesignListTable.Columns.Add(designNoColumn)
            'dummyDesignListTable.Columns.Add(designNameColumn)
            'cmbDesDesignList.BindingContext = New BindingContext
            'cmbDesDesignList.DataSource = dummyDesignListTable

            If (cmbDesDesignList.Items.Count > 0) Then
                cmbDesDesignList.SelectedValue = -1
            Else
                cmbDesDesignList.SelectedIndex = -1
            End If

            Return
        End If

        Dim custNo As Integer = cmbDesCustomerList.SelectedValue

        loadDesignChargePerUnit(custNo)
        loadDesignList(custNo)
        loadDesignGrid(custNo)
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub dgDesDesignDetails_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgDesDesignDetails.CurrentCellChanged
        cmbDesDesignList.SelectedIndex = dgDesDesignDetails.CurrentRowIndex + 1
    End Sub

    Private Sub btnCustClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustClear.Click
        If (cmbCustCustomerList.Items.Count > 0) Then
            cmbCustCustomerList.SelectedValue = -1
        Else
            cmbCustCustomerList.SelectedIndex = -1
        End If
    End Sub

    Private Sub btnDesEditPrice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesEditPrice.Click
        txtDesCalculatedPrice.ReadOnly = Not txtDesCalculatedPrice.ReadOnly
        txtDesCalculatedPrice.Focus()
    End Sub

    Private Sub btnDesClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesClear.Click
        If (cmbDesDesignList.SelectedIndex = -1 Or cmbDesDesignList.SelectedValue = -1) Then
            resetDesignScreen()
            cmbDesDesignList.Text = ""
        End If

        If (cmbDesDesignList.Items.Count > 0) Then
            cmbDesDesignList.SelectedValue = -1
        Else
            cmbDesDesignList.SelectedIndex = -1
        End If
    End Sub

    Private Sub chargeTypeCheckedChanged(sender As Object, e As EventArgs) Handles radioDesWP.CheckedChanged, radioDesWorking.CheckedChanged, radioDesPrint.CheckedChanged
        'bwtDesChargeTypeLoadThread.RunWorkerAsync(cmbDesCompanyList.SelectedValue)
        loadDesignChargePerUnit(cmbDesCustomerList.SelectedValue)
    End Sub

    Private Sub btnBillingClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBillingClear.Click
        If (cmbBillingBillNoList.Items.Count > 0) Then
            cmbBillingBillNoList.SelectedValue = -1
        Else
            cmbBillingBillNoList.SelectedIndex = -1
        End If
    End Sub

    Private Sub Button38_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button38.Click
        'Try
        If MessageBox.Show("Are you sure want to log off?", "Log off", System.Windows.Forms.MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            Me.Hide()
            Login.ComboBox1.Text = ""
            Login.TextBox2.Text = ""
            Login.Show()

            AddressReport.Close()
            BillReportForm.Close()
            AllAddrReport.Close()
            AllBillReport.Close()
            Balance.Close()
            CustomerBillSummary.Close()
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


    Private Sub TabPage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabCustomer.Click
        cmbCustCustomerList.Focus()
    End Sub

    Private Sub DataGrid4_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        'Try
        Dim bilr As DataTable
        Dim billdate As Date
        bilr = dgReportBillGrid.DataSource
        billkey = bilr.Rows(dgReportBillGrid.CurrentRowIndex).Item(8).ToString + "/" + bilr.Rows(dgReportBillGrid.CurrentRowIndex).Item(1).ToString
        billcust = bilr.Rows(dgReportBillGrid.CurrentRowIndex).Item(0)

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

        billdate = bilr.Rows(dgReportBillGrid.CurrentRowIndex).Item(2)
        billdatestring = billdate.ToString("dd/MM/yyyy")
        T17 = bilr.Rows(dgReportBillGrid.CurrentRowIndex).Item(5)
        T20 = bilr.Rows(dgReportBillGrid.CurrentRowIndex).Item(3)
        T21 = bilr.Rows(dgReportBillGrid.CurrentRowIndex).Item(4)
        BillReportForm.Show()
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub btnBilingOutstandingBalance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBilingOutstandingBalance.Click
        'Try
        'Dim ds10 As DataSet



        CustomerBillSummary.Show()
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub radioPaymentByCash_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioPaymentByCash.CheckedChanged
        gbBankDetails.Visible = False
    End Sub

    Private Sub radioPaymentByCheque_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioPaymentByCheque.CheckedChanged
        gbBankDetails.Visible = True
    End Sub

    Private Sub cmbPaymentCompanyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPaymentCustomerList.SelectedIndexChanged

        If (cmbPaymentCustomerList.SelectedIndex = -1 Or cmbPaymentCustomerList.SelectedValue = -1) Then
            If (cmbPaymentPaymentNoList.Items.Count > 0) Then
                cmbPaymentPaymentNoList.SelectedValue = -1
            Else
                cmbPaymentPaymentNoList.SelectedIndex = -1
            End If
            Return
        End If
        Dim custNo As Integer = cmbPaymentCustomerList.SelectedValue

        loadPaymentList(custNo)
        loadPaymentGrid(custNo)

    End Sub

    Sub calculatePaymentFinalPaidAmount() Handles txtPaymentActualPaidAmount.TextChanged, txtPaymentDiscountAmount.TextChanged, txtPaymentTaxDeductionAmount.TextChanged

        Dim actualPaidAmount As Decimal = 0
        Dim discountAmount As Decimal = 0
        Dim taxDeductionAmount As Decimal = 0
        Dim unPaidBillAmount As Decimal = 0

        Decimal.TryParse(txtPaymentActualPaidAmount.Text, actualPaidAmount)
        Decimal.TryParse(txtPaymentDiscountAmount.Text, discountAmount)
        Decimal.TryParse(txtPaymentTaxDeductionAmount.Text, taxDeductionAmount)
        Decimal.TryParse(txtPaymentUnPaidBilledAmount.Text, unPaidBillAmount)

        Dim finalPaidAmount As Decimal = actualPaidAmount + discountAmount + taxDeductionAmount

        If unPaidBillAmount < finalPaidAmount Then
            MsgBox("The payment amount cannot be greater than the unpaid billed amount. Please correct the payment amount")
            Return
        End If

        txtPaymentFinalPaidAmount.Text = Format(finalPaidAmount, "0.00")
        txtPaymentNetBalance.Text = Format((unPaidBillAmount - finalPaidAmount), "0.00")

    End Sub
    Private Sub btnBillingCreateBill_Click(sender As Object, e As EventArgs) Handles btnBillingCreateBill.Click

        ''log.Debug("btnBillingCreateBill_Click: entry")

        If (cmbBillingCustomerList.SelectedIndex = -1 Or cmbBillingCustomerList.SelectedValue = -1) Then
            MessageBox.Show("Please select a customer")
            cmbBillingCustomerList.Focus()
            Return
        End If

        cmbBillingBillNoList.SelectedIndex = -1
        Dim custNo = cmbBillingCustomerList.SelectedValue

        Dim unBilledDesignAmount As Decimal = getDesignAmountWithoutGSTTax(BILL_TYPE_UNBILLED, custNo)
        Dim billedDesignAmount As Decimal = getDesignAmountWithGSTTax(BILL_TYPE_BILLED, custNo)
        Dim totalBillsPaidAmount As Decimal = getAllBillsPaidAmount(custNo)
        Dim unPaidBalance As Decimal = billedDesignAmount - totalBillsPaidAmount

        If (unBilledDesignAmount = 0) Then
            MessageBox.Show("There are no designs to bill or all the designs are billed already for this customer")
            Return
        End If

        loadGSTForCustomerInBilling(custNo)

        dpBillingBillDate.Text = ""

        txtBillingDesignAmoutBeforeGST.Text = Format(unBilledDesignAmount, "0.00")
        txtBillingPrevBalance.Text = Format(unPaidBalance, "0.00")

        calculateGSTAmountInBilling()
        Dim totalAmountToPay As Decimal = Format(unPaidBalance + Decimal.Parse(txtBillingDesignAmoutAfterGST.Text), "0.00")

        txtBillingTotalAmount.Text = Format(Math.Round(totalAmountToPay), "0.00")
        txtBillingPaidAmount.Text = "0.00"
        txtBillingRemainingBalance.Text = Format(Math.Round(totalAmountToPay), "0.00")

        setBillingControlsVisibilitiesForCreateBill()

    End Sub

    Private Sub cmbDesDesignList_TextChanged(sender As Object, e As EventArgs) Handles cmbDesDesignList.TextChanged

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

    Function getLastPaymentRow(Optional custNo As Integer = Nothing) As DataRow

        Dim paymentSelectQuery As SqlCommand
        If (custNo <> Nothing) Then
            paymentSelectQuery = New SqlCommand("select * from payment where PaymentNo=(select max(PaymentNo) from payment where custno=" + custNo.ToString + ")", dbConnection)
        Else
            paymentSelectQuery = New SqlCommand("select * from payment where PaymentNo=(select max(PaymentNo) from payment)", dbConnection)
        End If

        Dim paymentAdapter = New SqlDataAdapter()
        paymentAdapter.SelectCommand = paymentSelectQuery
        Dim paymentDataSet = New DataSet
        paymentAdapter.Fill(paymentDataSet, "payment")
        Dim paymentTable = paymentDataSet.Tables(0)

        If (paymentTable.Rows.Count > 0) Then
            Return paymentTable.Rows(0)
        End If

        Return Nothing
    End Function

    Function getAllBillsPaidAmount(Optional custNo As Integer = Nothing) As Decimal

        Dim billSelectQuery As SqlCommand
        If (custNo <> Nothing) Then
            billSelectQuery = New SqlCommand("select sum(PaidAmount) from bill where custno=" + custNo.ToString, dbConnection)
        Else
            billSelectQuery = New SqlCommand("select sum(PaidAmount) from bill", dbConnection)
        End If

        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billSelectQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "bill")
        Dim billTable = billDataSet.Tables(0)

        If billTable.Rows.Count = 0 Or billTable.Rows(0).Item(0) Is DBNull.Value Then
            Return 0
        End If

        Return billTable.Rows(0).Item(0)

    End Function

    Private Sub btnBillingConfirmCreateBill_Click(sender As Object, e As EventArgs) Handles btnBillingConfirmCreateBill.Click
        If cmbBillingCustomerList.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Company Name")
            cmbDesCustomerList.Focus()
        ElseIf Not cmbBillingBillNoList.Text.Trim.Equals("") Then
            MessageBox.Show("Bill Number will be auto generated. Please clear the Bill Number box.")
            cmbBillingBillNoList.Focus()
        ElseIf dpBillingBillDate.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid Date")
            dpBillingBillDate.Focus()
        ElseIf txtBillingPrevBalance.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Previous Balance amount")
            txtBillingPrevBalance.Focus()
        ElseIf txtBillingDesignAmoutBeforeGST.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Design Amount")
            txtBillingDesignAmoutBeforeGST.Focus()
        ElseIf txtBillingTotalAmount.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Total Amount")
            txtBillingTotalAmount.Focus()
        ElseIf txtBillingRemainingBalance.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Remaining Balance Amount")
            txtBillingRemainingBalance.Focus()
        Else

            Dim newBillNo As Integer = -1

            Dim query As String = String.Empty
            query &= "INSERT INTO bill (DisplayBillNo, CustNo, BillDate, DesignCost, CGST, SGST, IGST, UnPaidAmountTillNow, PaidAmount, Cancelled) "
            query &= "VALUES (@DisplayBillNo, @CustNo, @BillDate, @DesignCost, @CGST, @SGST, @IGST, @UnPaidAmountTillNow, @PaidAmount, @Cancelled); SELECT SCOPE_IDENTITY()"

            gDisplayBillNo += 1

            Using comm As New SqlCommand()
                With comm
                    .Connection = dbConnection
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@DisplayBillNo", gDisplayBillNo)
                    .Parameters.AddWithValue("@CustNo", cmbBillingCustomerList.SelectedValue)
                    .Parameters.AddWithValue("@BillDate", dpBillingBillDate.Value)
                    .Parameters.AddWithValue("@DesignCost", Decimal.Parse(txtBillingDesignAmoutBeforeGST.Text))
                    .Parameters.AddWithValue("@CGST", Decimal.Parse(txtBillingCGSTPercent.Text))
                    .Parameters.AddWithValue("@SGST", Decimal.Parse(txtBillingSGSTPercent.Text))
                    .Parameters.AddWithValue("@IGST", Decimal.Parse(txtBillingIGSTPercent.Text))
                    .Parameters.AddWithValue("@UnPaidAmountTillNow", Decimal.Parse(txtBillingPrevBalance.Text))
                    .Parameters.AddWithValue("@PaidAmount", Decimal.Parse(txtBillingPaidAmount.Text))
                    .Parameters.AddWithValue("@Cancelled", 0)
                End With
                'comm.ExecuteNonQuery()
                newBillNo = CInt(comm.ExecuteScalar())
            End Using

            updateRecentDesignsAsBilled(cmbBillingCustomerList.SelectedValue, newBillNo)

            MessageBox.Show("Bill successfully added")

            Dim custNo As Integer = cmbBillingCustomerList.SelectedValue

            loadBillList(custNo)
            loadBillGrid(custNo)

            'Catch ex As Exception
            'MessageBox.Show("Message To Agni User:   " & ex.Message & " Or this design Record may already exist")
        End If
    End Sub

    Private Sub btnBillingCancelCreateBill_Click(sender As Object, e As EventArgs) Handles btnBillingCancelCreateBill.Click
        If (cmbBillingBillNoList.Items.Count > 0) Then
            cmbBillingBillNoList.SelectedValue = -1
        Else
            cmbBillingBillNoList.SelectedIndex = -1
        End If

    End Sub

    Private Sub btnBillingCancelBill_Click(sender As Object, e As EventArgs) Handles btnBillingCancelBill.Click

        If (cmbBillingBillNoList.SelectedIndex = -1 Or cmbBillingBillNoList.SelectedValue = -1) Then
            MessageBox.Show("Please select a bill")
            cmbBillingBillNoList.Focus()
            Return
        End If

        Dim billNo As Integer = cmbBillingBillNoList.SelectedValue
        Dim custNo As Integer = cmbBillingCustomerList.SelectedValue

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
        'updateDesignsAsUnBilled(billNo)

        MessageBox.Show("Bill " + billNo.ToString + " is marked as cancelled bill. You need to create a new bill for the designs which were billed in this bill")

        loadBillList(custNo)
        loadBillGrid(custNo)

    End Sub

    Private Sub dgBIllingBillDetails_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgBIllingBillDetails.CurrentCellChanged
        cmbBillingBillNoList.SelectedIndex = dgBIllingBillDetails.CurrentRowIndex + 1

        If btnBillingConfirmCreateBill.Visible = True Then
            resetBillingControlsVisibilities()
        End If
    End Sub

    Private Sub btnPaymentCreatePayment_Click(sender As Object, e As EventArgs) Handles btnPaymentCreatePayment.Click
        ''log.Debug("btnBillingCreateBill_Click: entry")

        If (cmbPaymentCustomerList.SelectedIndex = -1 Or cmbPaymentCustomerList.SelectedValue = -1) Then
            MessageBox.Show("Please select a customer")
            cmbPaymentCustomerList.Focus()
            Return
        End If

        Dim custNo = cmbPaymentCustomerList.SelectedValue

        Dim billedDesignAmount As Decimal = getDesignAmountWithGSTTax(BILL_TYPE_BILLED, custNo)
        Dim totalBillsPaidAmount As Decimal = getAllBillsPaidAmount(custNo)
        Dim unPaidBalance As Decimal = billedDesignAmount - totalBillsPaidAmount

        Dim lastBillRow As DataRow = getLastBillRow(custNo)

        If (lastBillRow Is Nothing Or unPaidBalance = 0) Then
            MessageBox.Show("This customer has no due amount to make the payment. All bills are paid by this customer")
            Return
        End If

        resetPaymentScreen()

        txtPaymentDisplayBillNo.Text = lastBillRow.Item("DisplayBillNo")
        txtPaymentBillNo.Text = lastBillRow.Item("BillNo")
        txtPaymentUnPaidBilledAmount.Text = Format(unPaidBalance, "0.00")

        setPaymentControlsVisibilitiesForCreatePayment()
    End Sub

    Private Sub btnPaymentConfirmCreatePayment_Click(sender As Object, e As EventArgs) Handles btnPaymentConfirmCreatePayment.Click

        If cmbPaymentCustomerList.Text.Trim.Equals("") Then
            MessageBox.Show("Choose a company from company list")
            cmbPaymentCustomerList.Focus()
        ElseIf dpPaymentDate.Text.Trim.Equals("") Then
            MessageBox.Show("Choose valid payment date")
            dpPaymentDate.Focus()
        ElseIf Val(txtPaymentUnPaidBilledAmount.Text) = 0 Then
            MessageBox.Show("There is no balance anmount to pay")
            txtPaymentUnPaidBilledAmount.Focus()
        ElseIf txtPaymentActualPaidAmount.Text.Trim.Equals("") And txtPaymentDiscountAmount.Text.Trim.Equals("") And txtPaymentTaxDeductionAmount.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Payment Amount or Deduction or Tax Deduction")
            txtPaymentActualPaidAmount.Focus()
        ElseIf txtPaymentFinalPaidAmount.Text.Trim.Equals("") Then
            MessageBox.Show("Enter amount to be credited")
            txtPaymentFinalPaidAmount.Focus()
        ElseIf radioPaymentByCheque.Checked And txtPaymentChequeNo.Text.Trim.Equals("") Then
            MessageBox.Show("Enter cheque Number")
            txtPaymentChequeNo.Focus()
        ElseIf radioPaymentByCheque.Checked And txtPaymentBankName.Text.Trim.Equals("") Then
            MessageBox.Show("Enter bank Name")
            txtPaymentBankName.Focus()
        ElseIf radioPaymentByCheque.Checked And dpPaymentChequeDate.Text.Trim.Equals("") Then
            MessageBox.Show("Choose valid cheque date")
            dpPaymentChequeDate.Focus()
        Else

            Dim actualPaidAmount As Decimal = 0
            Dim discountAmount As Decimal = 0
            Dim taxDeductionAmount As Decimal = 0
            Dim unPaidBillAmount As Decimal = 0

            Decimal.TryParse(txtPaymentActualPaidAmount.Text, actualPaidAmount)
            Decimal.TryParse(txtPaymentDiscountAmount.Text, discountAmount)
            Decimal.TryParse(txtPaymentTaxDeductionAmount.Text, taxDeductionAmount)
            Decimal.TryParse(txtPaymentUnPaidBilledAmount.Text, unPaidBillAmount)

            Dim finalPaidAmount As Decimal = actualPaidAmount + discountAmount + taxDeductionAmount

            If (unPaidBillAmount < (actualPaidAmount + discountAmount + taxDeductionAmount)) Then
                MessageBox.Show("You cannot pay more amount than the the unpaid bill amount")
                Return
            End If

            Dim newPaymentNo As Integer = -1

            Dim paymentType As String = "Cash"
            If radioPaymentByCheque.Checked Then
                paymentType = "Cheque"
            End If

            Dim paymentChequeNo = txtPaymentChequeNo.Text
            Dim paymentBankName = txtPaymentBankName.Text
            Dim paymentChequeDate = dpPaymentChequeDate.Text

            Dim query As String = String.Empty
            query &= "INSERT INTO payment (CustNo, BillNo, UnPaidBilledAmount, PaymentDate, PaymentMode, ActualPaidAmount, "
            query &= "Discount, TaxDeduction, ChequeNo, BankName, ChequeDate, Remarks, FinalPaidAmount) "
            query &= "VALUES (@CustNo, @BillNo, @UnPaidBilledAmount, @PaymentDate, @PaymentMode, @ActualPaidAmount, "
            query &= "@Discount, @TaxDeduction, @ChequeNo, @BankName, @ChequeDate, @Remarks, @FinalPaidAmount); SELECT SCOPE_IDENTITY()"

            Using comm As New SqlCommand()
                With comm
                    .Connection = dbConnection
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@CustNo", cmbPaymentCustomerList.SelectedValue)
                    .Parameters.AddWithValue("@BillNo", txtPaymentBillNo.Text)
                    .Parameters.AddWithValue("@UnPaidBilledAmount", txtPaymentUnPaidBilledAmount.Text)
                    .Parameters.AddWithValue("@PaymentDate", dpPaymentDate.Value)
                    .Parameters.AddWithValue("@PaymentMode", paymentType)
                    .Parameters.AddWithValue("@ActualPaidAmount", actualPaidAmount)
                    .Parameters.AddWithValue("@Discount", discountAmount)
                    .Parameters.AddWithValue("@TaxDeduction", taxDeductionAmount)
                    .Parameters.AddWithValue("@Remarks", txtPaymentRemarks.Text)
                    .Parameters.AddWithValue("@FinalPaidAmount", txtPaymentFinalPaidAmount.Text)
                End With

                If radioPaymentByCheque.Checked Then
                    comm.Parameters.AddWithValue("@ChequeNo", txtPaymentChequeNo.Text)
                    comm.Parameters.AddWithValue("@BankName", txtPaymentBankName.Text)
                    comm.Parameters.AddWithValue("@ChequeDate", dpPaymentChequeDate.Value)
                Else
                    comm.Parameters.AddWithValue("@ChequeNo", DBNull.Value)
                    comm.Parameters.AddWithValue("@BankName", DBNull.Value)
                    comm.Parameters.AddWithValue("@ChequeDate", DBNull.Value)
                End If

                newPaymentNo = CInt(comm.ExecuteScalar())
            End Using

            cmbPaymentPaymentNoList.Text = newPaymentNo

            addPaidAmountInBill(txtPaymentBillNo.Text, txtPaymentFinalPaidAmount.Text)

            MessageBox.Show("Payment successfully added")
            Dim custNo As Integer = cmbBillingCustomerList.SelectedValue
            loadPaymentList(custNo)
            loadPaymentGrid(custNo)
        End If
    End Sub

    Private Sub btnPaymentCancelCreatePayment_Click(sender As Object, e As EventArgs) Handles btnPaymentCancelCreatePayment.Click
        If (cmbPaymentPaymentNoList.SelectedIndex = -1 Or cmbPaymentPaymentNoList.SelectedValue = -1) Then
            resetPaymentScreen()
            cmbPaymentPaymentNoList.Text = ""
        End If

        If (cmbPaymentPaymentNoList.Items.Count > 0) Then
            cmbPaymentPaymentNoList.SelectedValue = -1
        Else
            cmbPaymentPaymentNoList.SelectedIndex = -1
        End If

    End Sub

    Sub calculateGSTAmountInBilling() Handles txtBillingCGSTPercent.TextChanged, txtBillingSGSTPercent.TextChanged, txtBillingIGSTPercent.TextChanged

        Dim CGSTPercent As Decimal = 0
        Dim SGSTPercent As Decimal = 0
        Dim IGSTPercent As Decimal = 0
        Dim unBilledDesignAmount As Decimal = 0

        Decimal.TryParse(txtBillingCGSTPercent.Text, CGSTPercent)
        Decimal.TryParse(txtBillingSGSTPercent.Text, SGSTPercent)
        Decimal.TryParse(txtBillingIGSTPercent.Text, IGSTPercent)
        Decimal.TryParse(txtBillingDesignAmoutBeforeGST.Text, unBilledDesignAmount)

        Dim CGSTAmount As Decimal = CGSTPercent * unBilledDesignAmount / 100
        Dim SGSTAmount As Decimal = SGSTPercent * unBilledDesignAmount / 100
        Dim IGSTAmount As Decimal = IGSTPercent * unBilledDesignAmount / 100
        Dim totalGSTAmount As Decimal = CGSTAmount + SGSTAmount + IGSTAmount

        txtBillingCGSTAmount.Text = Format(CGSTAmount, "0.00")
        txtBillingSGSTAmount.Text = Format(SGSTAmount, "0.00")
        txtBillingIGSTAmount.Text = Format(IGSTAmount, "0.00")
        txtBillingTotalGSTAmount.Text = Format(totalGSTAmount, "0.00")

        txtBillingDesignAmoutAfterGST.Text = Format(Math.Round(unBilledDesignAmount + totalGSTAmount), "0.00")

    End Sub

    Sub calculateDesignCostOnChange(sender As Object, e As EventArgs) Handles txtDesWidth.TextChanged, txtDesHeight.TextChanged,
        txtDesNoOfColors.TextChanged, txtDesCostPerUnit.TextChanged

        Dim designWidth As Decimal = 0
        Dim designHeight As Decimal = 0
        Dim designNoOfColors As Decimal = 0
        Dim designCostPerUnit As Decimal = 0

        Decimal.TryParse(txtDesWidth.Text, designWidth)
        Decimal.TryParse(txtDesHeight.Text, designHeight)
        Decimal.TryParse(txtDesNoOfColors.Text, designNoOfColors)
        Decimal.TryParse(txtDesCostPerUnit.Text, designCostPerUnit)

        Dim designCost = If(designWidth = 0, 1, designWidth) * If(designHeight = 0, 1, designHeight) *
                          If(designNoOfColors = 0, 1, designNoOfColors) * If(designCostPerUnit = 0, 1, designCostPerUnit)

        txtDesCalculatedPrice.Text = Math.Round(designCost)
    End Sub


    Private Sub dgPaymentDetails_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgPaymentDetails.CurrentCellChanged

        cmbPaymentPaymentNoList.SelectedIndex = dgPaymentDetails.CurrentRowIndex + 1

        If btnPaymentConfirmCreatePayment.Visible = True Then
            resetPaymentControlsVisibilities()
        End If
    End Sub

    Private Sub cmbPaymentPaymentNoList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPaymentPaymentNoList.SelectedIndexChanged

        'log.Debug("cmbPaymentPaymentNoList_SelectedIndexChanged: cmbPaymentPaymentNoList.SelectedIndex: " + cmbPaymentPaymentNoList.SelectedIndex.ToString)

        If (cmbPaymentPaymentNoList.SelectedIndex = -1 Or cmbPaymentPaymentNoList.SelectedValue = -1) Then
            resetPaymentScreen()
            Return
        End If

        Dim paymentNo As Integer = cmbPaymentPaymentNoList.SelectedValue
        ''log.Debug("cmbPaymentPaymentNoList_SelectedIndexChanged: cmbPaymentPaymentNoList.SelectedValue  : " + paymentNo.ToString)
        Dim paymentSelectQuery = New SqlCommand("select p.*,b.DisplayBillNo from payment p, bill b where p.BillNo = b.BillNo and p.PaymentNo=" + paymentNo.ToString, dbConnection)
        Dim paymentAdapter = New SqlDataAdapter()
        paymentAdapter.SelectCommand = paymentSelectQuery
        Dim paymentDataSet = New DataSet
        paymentAdapter.Fill(paymentDataSet, "payment")
        Dim paymentTable = paymentDataSet.Tables(0)

        If (paymentTable.Rows.Count > 0) Then
            Dim dataRow = paymentTable.Rows(0)

            txtPaymentDisplayBillNo.Text = dataRow.Item("DisplayBillNo")
            txtPaymentBillNo.Text = dataRow.Item("BillNo")
            txtPaymentUnPaidBilledAmount.Text = dataRow.Item("UnPaidBilledAmount")
            dpPaymentDate.Text = dataRow.Item("PaymentDate")

            If dataRow.Item("PaymentMode").ToString.Equals("Cash") Then
                radioPaymentByCash.Checked = True
            Else
                radioPaymentByCheque.Checked = True
            End If

            txtPaymentActualPaidAmount.Text = dataRow.Item("ActualPaidAmount")
            txtPaymentDiscountAmount.Text = dataRow.Item("Discount")
            txtPaymentTaxDeductionAmount.Text = dataRow.Item("TaxDeduction")
            txtPaymentFinalPaidAmount.Text = dataRow.Item("FinalPaidAmount")
            txtPaymentChequeNo.Text = If(dataRow.Item("ChequeNo") Is DBNull.Value, String.Empty, dataRow.Item("ChequeNo"))
            txtPaymentBankName.Text = If(dataRow.Item("BankName") Is DBNull.Value, String.Empty, dataRow.Item("BankName"))
            dpPaymentChequeDate.Text = If(dataRow.Item("ChequeDate") Is DBNull.Value, String.Empty, dataRow.Item("ChequeDate"))
            txtPaymentNetBalance.Text = dataRow.Item("UnPaidBilledAmount") - (dataRow.Item("ActualPaidAmount") + dataRow.Item("Discount") + dataRow.Item("TaxDeduction"))
            txtPaymentRemarks.Text = dataRow.Item("Remarks")
        Else
            MessageBox.Show("No data found for payment: " + paymentNo.ToString)
            ''log.Debug("cmbPaymentPaymentNoList_SelectedIndexChanged: No data found for Bill: " + paymentNo.ToString)
        End If
    End Sub

    Private Sub DateTimePicker6_CloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles dpPaymentDate.CloseUp

    End Sub


    Sub reduceBillPaidAmount(billNo As Decimal, amountToBeDeducted As Decimal)

        Dim updateQuery As String = String.Empty
        updateQuery &= "update bill set PaidAmount = PaidAmount - @amountToBeDeducted where BillNo=@BillNo"

        Using comm As New SqlCommand()
            With comm
                .Connection = dbConnection
                .CommandType = CommandType.Text
                .CommandText = updateQuery
                .Parameters.AddWithValue("@amountToBeDeducted", amountToBeDeducted)
                .Parameters.AddWithValue("@BillNo", billNo)
            End With
            comm.ExecuteNonQuery()
        End Using

    End Sub

    Private Sub btnPaymentDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPaymentDelete.Click
        'Try
        If cmbPaymentPaymentNoList.SelectedIndex = -1 Or cmbPaymentPaymentNoList.SelectedValue = -1 Then
            MessageBox.Show("select the payment which you want to delete")
            cmbPaymentPaymentNoList.Focus()
        Else
            Dim custNo As Integer = cmbPaymentCustomerList.SelectedValue
            Dim selectedPaymentNo As Integer = cmbPaymentPaymentNoList.SelectedValue

            Dim lastPaymentRow As DataRow = getLastPaymentRow(custNo)

            ''log.Debug("btnPaymentDelete_Click: selectedPaymentNo: " + selectedPaymentNo.ToString + " lastPaymentRow.Item(PaymentNo): " + lastPaymentRow.Item("PaymentNo").ToString)

            If (lastPaymentRow IsNot Nothing AndAlso selectedPaymentNo <> lastPaymentRow.Item("PaymentNo")) Then
                MessageBox.Show("This is not the last payment. You can only delete the last payment")
                Return
            End If

            If MessageBox.Show("Do you want to delete this payment transaction ", "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then

                Dim billNoForPayment As Integer = txtPaymentBillNo.Text
                Dim amountPaidForPayment As Decimal = txtPaymentFinalPaidAmount.Text

                If deleteSelectedPayment() = True Then
                    reduceBillPaidAmount(billNoForPayment, amountPaidForPayment)
                    MessageBox.Show("payment " + selectedPaymentNo.ToString + " is deleted successfully")

                    loadPaymentList(custNo)
                    loadPaymentGrid(custNo)
                    loadBillGrid(custNo)
                    loadBillList(custNo)
                End If

            End If
        End If

        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub tabAllTabsHolder_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabAllTabsHolder.SelectedIndexChanged

    End Sub

    Private Sub btnPaymentClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPaymentClear.Click
        If (cmbPaymentPaymentNoList.Items.Count > 0) Then
            cmbPaymentPaymentNoList.SelectedValue = -1
        Else
            cmbPaymentPaymentNoList.SelectedIndex = -1
        End If
    End Sub


    Private Sub reportFilterOptions_changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioReportCustName.CheckedChanged,
                                    radioReportBillNo.CheckedChanged,
                                    radioReportDate.CheckedChanged,
                                    cbReportExtraDateFilter.CheckedChanged,
                                    radioReportDesignName.CheckedChanged
        'Try

        lblReportCompName.Visible = False
        lblReportBillNo.Visible = False
        lblReportDesignNo.Visible = False
        lblReportFromDate.Visible = False
        lblReportToDate.Visible = False
        lblReportDesignNameHint.Visible = False
        cbReportExtraDateFilter.Visible = False
        cmbReportCustomerList.Visible = False
        cmbReportBillNoList.Visible = False
        txtReportDesignName.Visible = False
        dpReportFromDate.Visible = False
        dpReportToDate.Visible = False

        If radioReportCustName.Checked Then
            lblReportCompName.Visible = True
            cmbReportCustomerList.Visible = True
            cbReportExtraDateFilter.Visible = True
        ElseIf radioReportBillNo.Checked Then
            lblReportBillNo.Visible = True
            cmbReportBillNoList.Visible = True
            cbReportExtraDateFilter.Visible = True
        ElseIf radioReportDesignName.Checked Then
            lblReportDesignNo.Visible = True
            txtReportDesignName.Visible = True
            lblReportDesignNameHint.Visible = True
            cbReportExtraDateFilter.Visible = True
        ElseIf radioReportDate.Checked Then
            lblReportFromDate.Visible = False
            lblReportToDate.Visible = True
            dpReportFromDate.Visible = True
            dpReportToDate.Visible = True
        End If

        If cbReportExtraDateFilter.Checked = True Then
            lblReportFromDate.Visible = False
            lblReportToDate.Visible = True
            dpReportFromDate.Visible = True
            dpReportToDate.Visible = True
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub cmbReportDesignList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        pbReportDesignImage.Image = Nothing
    End Sub

    Function getSearchFilter() As Integer

        Dim searchFilter As Integer = 0

        If radioReportCustName.Checked = True Then
            searchFilter = searchFilter Or SEARCH_BY_COMPANY
        ElseIf radioReportBillNo.Checked = True Then
            searchFilter = searchFilter Or SEARCH_BY_BILL
        ElseIf radioReportDesignName.Checked = True Then
            searchFilter = searchFilter Or SEARCH_BY_DESIGN
        ElseIf radioReportDate.Checked = True Then
            searchFilter = searchFilter Or SEARCH_BY_DATE
        End If

        If cbReportExtraDateFilter.Checked = True Then
            searchFilter = searchFilter Or SEARCH_BY_DATE
        End If

        Return searchFilter
    End Function

    Private Sub dgReportDesignGrid_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgReportDesignGrid.CurrentCellChanged
        Dim rowIndex As Integer = dgReportDesignGrid.CurrentRowIndex
        Dim designtable As DataTable = dgReportDesignGrid.DataSource

        If designtable.Rows.Count = 0 Then
            Return
        End If

        Dim designNo = designtable.Rows(rowIndex).Item("DesignNo")
        pbReportDesignImage.Image = getDesignImage(designNo)
    End Sub

    Private Function getDesignImage(designNo As Integer) As Image

        Dim designSelectQuery = New SqlCommand("select Image from design where DesignNo=" + designNo.ToString, dbConnection)
        Dim designDataAdapter = New SqlDataAdapter()
        designDataAdapter.SelectCommand = designSelectQuery
        Dim designDataSet = New DataSet
        designDataAdapter.Fill(designDataSet, "design")
        Dim designTable = designDataSet.Tables(0)

        If (designTable.Rows.Count = 0) Then
            Return Nothing
        End If

        Dim dataRow = designTable.Rows(0)
        If Not dataRow.Item("Image") Is DBNull.Value Then
            Dim designImage() As Byte = CType(dataRow.Item("Image"), Byte())
            Dim designImageBuffer As New MemoryStream(designImage)
            Return Image.FromStream(designImageBuffer)
        End If

        Return Nothing

        ''Catch ex As Exception
        '    'MessageBox.Show("Message to Agni User:   " & ex.Message)
        ''End 'Try
    End Function

    Private Sub btnReportSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReportSearch.Click
        'Try
        pbReportDesignImage.Image = Nothing
        dgReportDesignGrid.DataSource = Nothing
        dgReportBillGrid.DataSource = Nothing

        Dim searchFilter As Integer = getSearchFilter()

        If (searchFilter And SEARCH_BY_COMPANY) <> 0 Then
            If (cmbReportCustomerList.SelectedIndex = -1 Or cmbReportCustomerList.SelectedValue = -1) Then
                MsgBox("Please select a customer to search")
                Return
            End If
            Dim custNo As Integer = cmbReportCustomerList.SelectedValue
            If (searchFilter And SEARCH_BY_DATE) <> 0 Then
                searchDesignByCustNo(custNo, dpReportFromDate.Value, dpReportToDate.Value)
                searchBillByCustNo(custNo, dpReportFromDate.Value, dpReportToDate.Value)
                searchPaymentByCustNo(custNo, dpReportFromDate.Value, dpReportToDate.Value)
            Else
                searchDesignByCustNo(custNo)
                searchBillByCustNo(custNo)
                searchPaymentByCustNo(custNo)
            End If

        ElseIf (searchFilter And SEARCH_BY_BILL) <> 0 Then
            If (cmbReportBillNoList.SelectedIndex = -1 Or cmbReportBillNoList.SelectedValue = -1) Then
                MsgBox("Please select a bill number to search")
                Return
            End If
            Dim billNo As Integer = cmbReportBillNoList.SelectedValue
            If (searchFilter And SEARCH_BY_DATE) <> 0 Then
                searchDesignByBillNo(billNo, dpReportFromDate.Value, dpReportToDate.Value)
                searchBillByBillNo(billNo, dpReportFromDate.Value, dpReportToDate.Value)
                searchPaymentByBillNo(billNo, dpReportFromDate.Value, dpReportToDate.Value)
            Else
                searchDesignByBillNo(billNo)
                searchBillByBillNo(billNo)
                searchPaymentByBillNo(billNo)
            End If

        ElseIf (searchFilter And SEARCH_BY_DESIGN) <> 0 Then
            If (txtReportDesignName.Text.Trim().Equals(String.Empty)) Then
                MsgBox("Please enter design name or any part of design name to search")
                Return
            End If
            Dim designName As String = txtReportDesignName.Text
            If (searchFilter And SEARCH_BY_DATE) <> 0 Then
                searchDesignByDesignName(designName, dpReportFromDate.Value, dpReportToDate.Value)
                searchBillByDesignName(designName, dpReportFromDate.Value, dpReportToDate.Value)
                searchPaymentByDesignName(designName, dpReportFromDate.Value, dpReportToDate.Value)
            Else
                searchDesignByDesignName(designName)
                searchBillByDesignName(designName)
                searchPaymentByDesignName(designName)
            End If
        ElseIf (searchFilter And SEARCH_BY_DATE) <> 0 Then
            searchDesignByDateRange(dpReportFromDate.Value, dpReportToDate.Value)
            searchBillByDateRange(dpReportFromDate.Value, dpReportToDate.Value)
            searchPaymentByDateRange(dpReportFromDate.Value, dpReportToDate.Value)
        End If

        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Class SearchData

        Public custNo, billNo As Integer
        Public designName As String
        Public fromDate, toDate As Date

        Sub New()
            custNo = -1
            billNo = -1
            designName = String.Empty
            fromDate = Date.Today
            toDate = Date.Today
        End Sub

    End Class

    Sub searchDesignByCustNo(custNo As Integer, Optional fromDate As Date = Nothing, Optional toDate As Date = Nothing)
        Dim thread As Thread = New Thread(AddressOf fetchDesignByCustNo)
        thread.IsBackground = True

        Dim searchData As SearchData = New SearchData
        searchData.custNo = custNo
        searchData.fromDate = fromDate
        searchData.toDate = toDate

        thread.Start(searchData)
    End Sub

    Sub fetchDesignByCustNo(ByVal searchDataParam As Object)
        Dim searchData As SearchData = CType(searchDataParam, SearchData)

        Dim designTable As DataTable = fetchDesignTableByCustNo(searchData)
        Dim showDesignSearchResultInvoker As New showDesignSearchResultDelegate(AddressOf Me.showDesignSearchResult)
        Me.BeginInvoke(showDesignSearchResultInvoker, designTable)

        Dim designSummaryTable As DataTable = fetchDesignSummaryTableByCustNo(searchData)
        Dim showDesignSummarySearchResultInvoker As New showDesignSummarySearchResultDelegate(AddressOf Me.showDesignSummarySearchResult)
        Me.BeginInvoke(showDesignSummarySearchResultInvoker, designSummaryTable)
    End Sub

    Function fetchDesignTableByCustNo(searchData As SearchData) As DataTable

        Dim custNo As Integer = searchData.custNo
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate

        Dim designQuery As SqlCommand

        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            designQuery = New SqlCommand("select * from design where custNo=" + custNo.ToString + " and cast(designDate as date)>='" + fromDateStr + "' and cast(designDate as date)<='" + toDateStr + "'", dbConnection)
        Else
            designQuery = New SqlCommand("select * from design where custNo=" + custNo.ToString, dbConnection)
        End If

        Dim designAdapter = New SqlDataAdapter()
        designAdapter.SelectCommand = designQuery
        Dim designDataSet = New DataSet
        designAdapter.Fill(designDataSet, "design")
        Return designDataSet.Tables(0)

    End Function

    Function fetchDesignSummaryTableByCustNo(searchData As SearchData) As DataTable

        Dim custNo As Integer = searchData.custNo
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate

        Dim designQuery As SqlCommand

        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            designQuery = New SqlCommand("select count(1) as designCount, isnull(sum(CASE WHEN Billed = 1 THEN Price ELSE 0 END),0) AS billedDesignAmount, isnull(sum(CASE WHEN Billed = 0 THEN Price ELSE 0 END),0) AS unbilledDesignAmount, isnull(sum(Price),0) as TotalDesignAmount from design where custNo=" + custNo.ToString + " and cast(designDate as date)>='" + fromDateStr + "' and cast(designDate as date)<='" + toDateStr + "'", dbConnection)
        Else
            designQuery = New SqlCommand("select count(1) as designCount, isnull(sum(CASE WHEN Billed = 1 THEN Price ELSE 0 END),0) AS billedDesignAmount, isnull(sum(CASE WHEN Billed = 0 THEN Price ELSE 0 END),0) AS unbilledDesignAmount, isnull(sum(Price),0) as TotalDesignAmount from design where custNo=" + custNo.ToString, dbConnection)
        End If

        Dim designAdapter = New SqlDataAdapter()
        designAdapter.SelectCommand = designQuery
        Dim designDataSet = New DataSet
        designAdapter.Fill(designDataSet, "designSummary")
        Return designDataSet.Tables(0)

    End Function


    Delegate Sub showDesignSearchResultDelegate(designTable As DataTable)

    Sub showDesignSearchResult(designTable As DataTable)
        dgReportDesignGrid.DataSource = designTable
    End Sub

    Delegate Sub showDesignSummarySearchResultDelegate(designTable As DataTable)

    Sub showDesignSummarySearchResult(designSummaryTable As DataTable)
        If designSummaryTable.Rows.Count = 0 Then
            Return
        End If

        Dim dataRow As DataRow = designSummaryTable.Rows(0)
        lblReportNoOfDesigns.Text = dataRow.Item("designCount")
        lblReportBilledDesignAmount.Text = dataRow.Item("billedDesignAmount")
        lblReportUnBilledDesignAmount.Text = dataRow.Item("unbilledDesignAmount")
        lblReportTotDesignAmount.Text = dataRow.Item("TotalDesignAmount")

    End Sub

    Sub searchBillByCustNo(custNo As Integer, Optional fromDate As Date = Nothing, Optional toDate As Date = Nothing)
        Dim thread As Thread = New Thread(AddressOf fetchBillByCustNo)
        thread.IsBackground = True

        Dim searchData As SearchData = New SearchData
        searchData.custNo = custNo
        searchData.fromDate = fromDate
        searchData.toDate = toDate

        thread.Start(searchData)
    End Sub

    Sub fetchBillByCustNo(ByVal searchDataParam As Object)
        Dim searchData As SearchData = CType(searchDataParam, SearchData)
        Dim billTable As DataTable = fetchBillTableByCustNo(searchData)
        Dim showBillSearchResultInvoker As New showBillSearchResultDelegate(AddressOf Me.showBillSearchResult)
        Me.BeginInvoke(showBillSearchResultInvoker, billTable)

        Dim billSummaryTable As DataTable = fetchBillSummaryTableByCustNo(searchData)
        Dim showBillSummarySearchResultInvoker As New showBillSummarySearchResultDelegate(AddressOf Me.showBillSummarySearchResult)
        Me.BeginInvoke(showBillSummarySearchResultInvoker, billSummaryTable)

    End Sub

    Function fetchBillTableByCustNo(searchData As SearchData) As DataTable
        Dim custNo As Integer = searchData.custNo
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate
        Dim billQuery As SqlCommand
        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            billQuery = New SqlCommand("select BillNo, DisplayBillNo, BillDate, DesignCost, UnPaidAmountTillNow, CGST, CGST*DesignCost/100 as CGSTAmount, 
                            SGST, SGST*DesignCost/100 as SGSTAmount, IGST, IGST*DesignCost/100 as IGSTAmount, (CGST+ SGST+ IGST)*DesignCost/100 as GSTAmount, ((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost as DesignAmountGST, PaidAmount, 
                            ((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost+UnPaidAmountTillNow as TotalAmount, 
                            (((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost+UnPaidAmountTillNow)-PaidAmount as RemainingBalance, Cancelled  from bill where custNo=" + custNo.ToString + " and cast(BillDate as date)>='" + fromDateStr + "' and cast(BillDate as date)<='" + toDateStr + "'", dbConnection)
        Else
            billQuery = New SqlCommand("select BillNo, DisplayBillNo, BillDate, DesignCost, UnPaidAmountTillNow, CGST, CGST*DesignCost/100 as CGSTAmount, 
                            SGST, SGST*DesignCost/100 as SGSTAmount, IGST, IGST*DesignCost/100 as IGSTAmount, (CGST+ SGST+ IGST)*DesignCost/100 as GSTAmount, ((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost as DesignAmountGST, PaidAmount, 
                            ((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost+UnPaidAmountTillNow as TotalAmount, 
                            (((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost+UnPaidAmountTillNow)-PaidAmount as RemainingBalance, Cancelled  from bill where custNo=" + custNo.ToString, dbConnection)
        End If

        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "bill")
        Return billDataSet.Tables(0)
    End Function

    Delegate Sub showBillSearchResultDelegate(billTable As DataTable)

    Sub showBillSearchResult(billTable As DataTable)
        dgReportBillGrid.DataSource = billTable
    End Sub

    Function fetchBillSummaryTableByCustNo(searchData As SearchData) As DataTable
        Dim custNo As Integer = searchData.custNo
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate
        Dim billQuery As SqlCommand
        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            billQuery = New SqlCommand("select count(1) as BillsCount, isnull(sum(DesignAmountGST),0) as TotBilledAmount from(
                            select ((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost as DesignAmountGST from bill where custNo=" + custNo.ToString + " and cast(BillDate as date)>='" + fromDateStr + "' and cast(BillDate as date)<='" + toDateStr + "') as billTable", dbConnection)
        Else
            billQuery = New SqlCommand("select count(1) as BillsCount, isnull(sum(DesignAmountGST),0) as TotBilledAmount from(
                            select ((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost as DesignAmountGST from bill where custNo=" + custNo.ToString + ")  as billTable", dbConnection)
        End If

        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "billSummary")
        Return billDataSet.Tables(0)
    End Function

    Delegate Sub showBillSummarySearchResultDelegate(billSummaryTable As DataTable)

    Sub showBillSummarySearchResult(billSummaryTable As DataTable)
        If billSummaryTable.Rows.Count = 0 Then
            Return
        End If

        Dim dataRow As DataRow = billSummaryTable.Rows(0)
        lblReportNoOfBills.Text = dataRow.Item("BillsCount")
        lblReportBilledAmount.Text = Format(dataRow.Item("TotBilledAmount"), "0.00")
    End Sub

    Sub searchPaymentByCustNo(custNo As Integer, Optional fromDate As Date = Nothing, Optional toDate As Date = Nothing)
        Dim thread As Thread = New Thread(AddressOf fetchPaymentByCustNo)
        thread.IsBackground = True

        Dim searchData As SearchData = New SearchData
        searchData.custNo = custNo
        searchData.fromDate = fromDate
        searchData.toDate = toDate

        thread.Start(searchData)
    End Sub

    Sub fetchPaymentByCustNo(ByVal searchDataParam As Object)
        Dim searchData As SearchData = CType(searchDataParam, SearchData)
        Dim paymentTable As DataTable = fetchPaymentTableByCustNo(searchData)
        Dim showPaymentSearchResultInvoker As New showPaymentSearchResultDelegate(AddressOf Me.showPaymentSearchResult)
        Me.BeginInvoke(showPaymentSearchResultInvoker, paymentTable)
        Dim paymentSummaryTable As DataTable = fetchPaymentSummaryTableByCustNo(searchData)
        Dim showPaymentSummarySearchResultInvoker As New showPaymentSummarySearchResultDelegate(AddressOf Me.showPaymentSummarySearchResult)
        Me.BeginInvoke(showPaymentSummarySearchResultInvoker, paymentSummaryTable)
    End Sub

    Function fetchPaymentTableByCustNo(searchData As SearchData) As DataTable
        Dim custNo As Integer = searchData.custNo
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate
        Dim paymentQuery As SqlCommand
        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            paymentQuery = New SqlCommand("select c.CompName,p.*, p.UnPaidBilledAmount - p.FinalPaidAmount as NetBalance, b.DisplayBillNo  from payment p, bill b, customer c where p.BillNo = b.BillNo and p.CustNo = c.CustNo and p.custNo=" + custNo.ToString + "  and cast(PaymentDate as date)>='" + fromDateStr + "' and  cast(PaymentDate as date)<='" + toDateStr + "'", dbConnection)
        Else
            paymentQuery = New SqlCommand("select c.CompName,p.*, p.UnPaidBilledAmount - p.FinalPaidAmount as NetBalance, b.DisplayBillNo  from payment p, bill b, customer c where p.BillNo = b.BillNo and p.CustNo = c.CustNo and p.custNo=" + custNo.ToString, dbConnection)
        End If

        Dim paymentAdapter = New SqlDataAdapter()
        paymentAdapter.SelectCommand = paymentQuery
        Dim paymentDataSet = New DataSet
        paymentAdapter.Fill(paymentDataSet, "payment")
        Return paymentDataSet.Tables(0)
    End Function

    Delegate Sub showPaymentSearchResultDelegate(paymentTable As DataTable)

    Sub showPaymentSearchResult(paymentTable As DataTable)
        dgReportPaymentGrid.DataSource = paymentTable
    End Sub

    Function fetchPaymentSummaryTableByCustNo(searchData As SearchData) As DataTable
        Dim custNo As Integer = searchData.custNo
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate
        Dim paymentQuery As SqlCommand
        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            paymentQuery = New SqlCommand("select count(1) as PaymentCount, isnull(sum(p.ActualPaidAmount),0) as TotPaidAmountActual, isnull(sum(p.FinalPaidAmount),0) as TotPaidAmountAfterAdjustment from payment p, bill b, customer c where p.BillNo = b.BillNo and p.CustNo = c.CustNo and p.custNo=" + custNo.ToString + "  and cast(PaymentDate as date)>='" + fromDateStr + "' and  cast(PaymentDate as date)<='" + toDateStr + "'", dbConnection)
        Else
            paymentQuery = New SqlCommand("select count(1) as PaymentCount, isnull(sum(p.ActualPaidAmount),0) as TotPaidAmountActual, isnull(sum(p.FinalPaidAmount),0) as TotPaidAmountAfterAdjustment from payment p, bill b, customer c where p.BillNo = b.BillNo and p.CustNo = c.CustNo and p.custNo=" + custNo.ToString, dbConnection)
        End If

        Dim paymentAdapter = New SqlDataAdapter()
        paymentAdapter.SelectCommand = paymentQuery
        Dim paymentDataSet = New DataSet
        paymentAdapter.Fill(paymentDataSet, "payment")
        Return paymentDataSet.Tables(0)
    End Function

    Delegate Sub showPaymentSummarySearchResultDelegate(billSummaryTable As DataTable)

    Sub showPaymentSummarySearchResult(paymentSummaryTable As DataTable)
        If paymentSummaryTable.Rows.Count = 0 Then
            Return
        End If

        Dim dataRow As DataRow = paymentSummaryTable.Rows(0)
        lblReportNoOfPayment.Text = dataRow.Item("PaymentCount")
        lblReportPaidAmountActual.Text = Format(dataRow.Item("TotPaidAmountActual"), "0.00")
        lblReportPaidAmountWithDeduction.Text = Format(dataRow.Item("TotPaidAmountAfterAdjustment"), "0.00")
    End Sub

    Sub searchDesignByBillNo(billNo As Integer, Optional fromDate As Date = Nothing, Optional toDate As Date = Nothing)
        Dim thread As Thread = New Thread(AddressOf fetchDesignByBillNo)
        thread.IsBackground = True
        Dim searchData As SearchData = New SearchData
        searchData.billNo = billNo
        searchData.fromDate = fromDate
        searchData.toDate = toDate
        thread.Start(searchData)
    End Sub

    Sub fetchDesignByBillNo(ByVal searchDataParam As Object)
        Dim searchData As SearchData = CType(searchDataParam, SearchData)
        Dim designTable As DataTable = fetchDesignTableByBillNo(searchData)
        Dim showDesignSearchResultInvoker As New showDesignSearchResultDelegate(AddressOf Me.showDesignSearchResult)
        Me.BeginInvoke(showDesignSearchResultInvoker, designTable)
        Dim designSummaryTable As DataTable = fetchDesignSummaryTableByBillNo(searchData)
        Dim showDesignSummarySearchResultInvoker As New showDesignSummarySearchResultDelegate(AddressOf Me.showDesignSummarySearchResult)
        Me.BeginInvoke(showDesignSummarySearchResultInvoker, designSummaryTable)
    End Sub

    Function fetchDesignTableByBillNo(searchData As SearchData) As DataTable
        Dim billNo As Integer = searchData.billNo
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate

        Dim designQuery As SqlCommand

        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            designQuery = New SqlCommand("select c.CompName,d.* from design d,customer c where d.billNo=" + billNo.ToString + " and d.CustNo=c.CustNo and cast(d.designDate as date)>='" + fromDateStr + "' and cast(d.designDate as date)<='" + toDateStr + "'", dbConnection)
        Else
            designQuery = New SqlCommand("select c.CompName,d.* from design d,customer c where d.billNo=" + billNo.ToString + " and d.CustNo=c.CustNo", dbConnection)
        End If

        Dim designAdapter = New SqlDataAdapter()
        designAdapter.SelectCommand = designQuery
        Dim designDataSet = New DataSet
        designAdapter.Fill(designDataSet, "design")
        Return designDataSet.Tables(0)
    End Function

    Function fetchDesignSummaryTableByBillNo(searchData As SearchData) As DataTable
        Dim billNo As Integer = searchData.billNo
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate

        Dim designQuery As SqlCommand

        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            designQuery = New SqlCommand("select count(1) as designCount, isnull(sum(CASE WHEN d.Billed = 1 THEN d.Price ELSE 0 END),0) AS billedDesignAmount, isnull(sum(CASE WHEN d.Billed = 0 THEN d.Price ELSE 0 END),0) AS unbilledDesignAmount, isnull(sum(d.Price),0) as TotalDesignAmount from design d,customer c where d.billNo=" + billNo.ToString + " and d.CustNo=c.CustNo and cast(d.designDate as date)>='" + fromDateStr + "' and cast(d.designDate as date)<='" + toDateStr + "'", dbConnection)
        Else
            designQuery = New SqlCommand("select count(1) as designCount, isnull(sum(CASE WHEN d.Billed = 1 THEN d.Price ELSE 0 END),0) AS billedDesignAmount, isnull(sum(CASE WHEN d.Billed = 0 THEN d.Price ELSE 0 END),0) AS unbilledDesignAmount, isnull(sum(d.Price),0) as TotalDesignAmount from design d,customer c where d.billNo=" + billNo.ToString + " and d.CustNo=c.CustNo", dbConnection)
        End If

        Dim designAdapter = New SqlDataAdapter()
        designAdapter.SelectCommand = designQuery
        Dim designDataSet = New DataSet
        designAdapter.Fill(designDataSet, "designSummary")
        Return designDataSet.Tables(0)
    End Function

    Sub searchBillByBillNo(billNo As Integer, Optional fromDate As Date = Nothing, Optional toDate As Date = Nothing)
        Dim thread As Thread = New Thread(AddressOf fetchBillByBillNo)
        thread.IsBackground = True

        Dim searchData As SearchData = New SearchData
        searchData.billNo = billNo
        searchData.fromDate = fromDate
        searchData.toDate = toDate

        thread.Start(searchData)
    End Sub

    Sub fetchBillByBillNo(ByVal searchDataParam As Object)
        Dim searchData As SearchData = CType(searchDataParam, SearchData)
        Dim billTable As DataTable = fetchBillTableByBillNo(searchData)
        Dim showBillSearchResultInvoker As New showBillSearchResultDelegate(AddressOf Me.showBillSearchResult)
        Me.BeginInvoke(showBillSearchResultInvoker, billTable)
        Dim billSummaryTable As DataTable = fetchBillSummaryTableByBillNo(searchData)
        Dim showBillSummarySearchResultInvoker As New showBillSummarySearchResultDelegate(AddressOf Me.showBillSummarySearchResult)
        Me.BeginInvoke(showBillSummarySearchResultInvoker, billSummaryTable)
    End Sub

    Function fetchBillTableByBillNo(searchData As SearchData) As DataTable
        Dim billNo As Integer = searchData.billNo
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate

        Dim billQuery As SqlCommand

        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            billQuery = New SqlCommand("select c.CompName, b.BillNo, b.DisplayBillNo, b.BillDate, b.DesignCost, b.UnPaidAmountTillNow, b.CGST, b.CGST*b.DesignCost/100 as CGSTAmount, 
                            b.SGST, b.SGST*b.DesignCost/100 as SGSTAmount, b.IGST, b.IGST*b.DesignCost/100 as IGSTAmount, (b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100 as GSTAmount, ((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost as DesignAmountGST, b.PaidAmount, 
                            ((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost+b.UnPaidAmountTillNow as TotalAmount, 
                            (((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost+b.UnPaidAmountTillNow)-b.PaidAmount as RemainingBalance, b.Cancelled from bill b, customer c where b.billNo=" + billNo.ToString + " and b.CustNo=c.CustNo and cast(b.BillDate as date)>='" + fromDateStr + "' and  cast(b.BillDate as date)<='" + toDateStr + "'", dbConnection)

        Else
            billQuery = New SqlCommand("select c.CompName, b.BillNo, b.DisplayBillNo, b.BillDate, b.DesignCost, b.UnPaidAmountTillNow, b.CGST, b.CGST*b.DesignCost/100 as CGSTAmount, 
                            b.SGST, b.SGST*b.DesignCost/100 as SGSTAmount, b.IGST, b.IGST*b.DesignCost/100 as IGSTAmount, (b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100 as GSTAmount, ((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost as DesignAmountGST, b.PaidAmount, 
                            ((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost+b.UnPaidAmountTillNow as TotalAmount, 
                            (((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost+b.UnPaidAmountTillNow)-b.PaidAmount as RemainingBalance, b.Cancelled from bill b, customer c where b.billNo=" + billNo.ToString + " and b.CustNo=c.CustNo", dbConnection)

        End If

        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "bill")
        Return billDataSet.Tables(0)
    End Function

    Function fetchBillSummaryTableByBillNo(searchData As SearchData) As DataTable
        Dim billNo As Integer = searchData.billNo
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate

        Dim billQuery As SqlCommand

        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            billQuery = New SqlCommand("select count(1) as BillsCount, isnull(sum(DesignAmountGST),0) as TotBilledAmount from(
                            select ((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost as DesignAmountGST from bill b, customer c where b.billNo=" + billNo.ToString + " and b.CustNo=c.CustNo and cast(b.BillDate as date)>='" + fromDateStr + "' and  cast(b.BillDate as date)<='" + toDateStr + "') as billTable", dbConnection)

        Else
            billQuery = New SqlCommand("select count(1) as BillsCount, isnull(sum(DesignAmountGST),0) as TotBilledAmount from(
                            select ((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost as DesignAmountGST from bill b, customer c where b.billNo=" + billNo.ToString + " and b.CustNo=c.CustNo) as billTable", dbConnection)

        End If

        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "billSummary")
        Return billDataSet.Tables(0)
    End Function

    Sub searchPaymentByBillNo(billNo As Integer, Optional fromDate As Date = Nothing, Optional toDate As Date = Nothing)
        Dim thread As Thread = New Thread(AddressOf fetchPaymentByBillNo)
        thread.IsBackground = True

        Dim searchData As SearchData = New SearchData
        searchData.billNo = billNo
        searchData.fromDate = fromDate
        searchData.toDate = toDate

        thread.Start(searchData)
    End Sub

    Sub fetchPaymentByBillNo(ByVal searchDataParam As Object)
        Dim searchData As SearchData = CType(searchDataParam, SearchData)
        Dim paymentTable As DataTable = fetchPaymentTableByBillNo(searchData)
        Dim showPaymentSearchResultInvoker As New showPaymentSearchResultDelegate(AddressOf Me.showPaymentSearchResult)
        Me.BeginInvoke(showPaymentSearchResultInvoker, paymentTable)
        Dim paymentSummaryTable As DataTable = fetchPaymentSummaryTableByBillNo(searchData)
        Dim showPaymentSummarySearchResultInvoker As New showPaymentSummarySearchResultDelegate(AddressOf Me.showPaymentSummarySearchResult)
        Me.BeginInvoke(showPaymentSummarySearchResultInvoker, paymentSummaryTable)
    End Sub

    Function fetchPaymentTableByBillNo(searchData As SearchData) As DataTable
        Dim billNo As Integer = searchData.billNo
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate

        Dim paymentQuery As SqlCommand

        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            paymentQuery = New SqlCommand("select c.CompName,p.*, p.UnPaidBilledAmount - p.FinalPaidAmount as NetBalance, b.DisplayBillNo  from payment p, bill b, customer c where p.BillNo = b.BillNo and p.CustNo = c.CustNo and p.BillNo=" + billNo.ToString + "  and cast(PaymentDate as date)>='" + fromDateStr + "' and  cast(PaymentDate as date)<='" + toDateStr + "'", dbConnection)
        Else
            paymentQuery = New SqlCommand("select c.CompName,p.*, p.UnPaidBilledAmount - p.FinalPaidAmount as NetBalance, b.DisplayBillNo  from payment p, bill b, customer c where p.BillNo = b.BillNo and p.CustNo = c.CustNo and p.BillNo=" + billNo.ToString, dbConnection)

        End If

        Dim paymentAdapter = New SqlDataAdapter()
        paymentAdapter.SelectCommand = paymentQuery
        Dim paymentDataSet = New DataSet
        paymentAdapter.Fill(paymentDataSet, "payment")
        Return paymentDataSet.Tables(0)
    End Function

    Function fetchPaymentSummaryTableByBillNo(searchData As SearchData) As DataTable
        Dim billNo As Integer = searchData.billNo
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate

        Dim paymentQuery As SqlCommand

        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            paymentQuery = New SqlCommand("select count(1) as PaymentCount, isnull(sum(p.ActualPaidAmount),0) as TotPaidAmountActual, isnull(sum(p.FinalPaidAmount),0) as TotPaidAmountAfterAdjustment from payment p, bill b, customer c where p.BillNo = b.BillNo and p.CustNo = c.CustNo and p.BillNo=" + billNo.ToString + "  and cast(PaymentDate as date)>='" + fromDateStr + "' and  cast(PaymentDate as date)<='" + toDateStr + "'", dbConnection)
        Else
            paymentQuery = New SqlCommand("select count(1) as PaymentCount, isnull(sum(p.ActualPaidAmount),0) as TotPaidAmountActual, isnull(sum(p.FinalPaidAmount),0) as TotPaidAmountAfterAdjustment from payment p, bill b, customer c where p.BillNo = b.BillNo and p.CustNo = c.CustNo and p.BillNo=" + billNo.ToString, dbConnection)

        End If

        Dim paymentAdapter = New SqlDataAdapter()
        paymentAdapter.SelectCommand = paymentQuery
        Dim paymentDataSet = New DataSet
        paymentAdapter.Fill(paymentDataSet, "paymentSummary")
        Return paymentDataSet.Tables(0)
    End Function

    Sub searchDesignByDesignName(designName As String, Optional fromDate As Date = Nothing, Optional toDate As Date = Nothing)
        Dim thread As Thread = New Thread(AddressOf fetchDesignByDesignName)
        thread.IsBackground = True

        Dim searchData As SearchData = New SearchData
        searchData.designName = designName
        searchData.fromDate = fromDate
        searchData.toDate = toDate

        thread.Start(searchData)
    End Sub

    Sub fetchDesignByDesignName(ByVal searchDataParam As Object)
        Dim searchData As SearchData = CType(searchDataParam, SearchData)
        Dim designTable As DataTable = fetchDesignTableByDesignName(searchData)
        Dim showDesignSearchResultInvoker As New showDesignSearchResultDelegate(AddressOf Me.showDesignSearchResult)
        Me.BeginInvoke(showDesignSearchResultInvoker, designTable)
        Dim designSummaryTable As DataTable = fetchDesignSummaryTableByDesignName(searchData)
        Dim showDesignSummarySearchResultInvoker As New showDesignSummarySearchResultDelegate(AddressOf Me.showDesignSummarySearchResult)
        Me.BeginInvoke(showDesignSummarySearchResultInvoker, designSummaryTable)
    End Sub

    Function fetchDesignTableByDesignName(searchData As SearchData) As DataTable
        Dim designName As String = searchData.designName
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate

        Dim designQuery As SqlCommand

        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            designQuery = New SqlCommand("select c.CompName,d.* from design d,customer c where d.DesignName like '%" + designName + "%' and d.CustNo=c.CustNo and cast(d.designDate as date)>='" + fromDateStr + "' and cast(d.designDate as date)<='" + toDateStr + "'", dbConnection)
        Else
            designQuery = New SqlCommand("Select c.CompName, d.* From design d, customer c Where d.DesignName Like '%" + designName + "%' and d.CustNo=c.CustNo", dbConnection)
        End If

        Dim designAdapter = New SqlDataAdapter()
        designAdapter.SelectCommand = designQuery
        Dim designDataSet = New DataSet
        designAdapter.Fill(designDataSet, "design")
        Return designDataSet.Tables(0)
    End Function

    Function fetchDesignSummaryTableByDesignName(searchData As SearchData) As DataTable
        Dim designName As String = searchData.designName
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate

        Dim designQuery As SqlCommand

        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            designQuery = New SqlCommand("select count(1) as designCount, isnull(sum(CASE WHEN Billed = 1 THEN Price ELSE 0 END),0) AS billedDesignAmount, isnull(sum(CASE WHEN Billed = 0 THEN Price ELSE 0 END),0) AS unbilledDesignAmount, isnull(sum(Price),0) as TotalDesignAmount from design d,customer c where d.DesignName like '%" + designName + "%' and d.CustNo=c.CustNo and cast(d.designDate as date)>='" + fromDateStr + "' and cast(d.designDate as date)<='" + toDateStr + "'", dbConnection)
        Else
            designQuery = New SqlCommand("select count(1) as designCount, isnull(sum(CASE WHEN Billed = 1 THEN Price ELSE 0 END),0) AS billedDesignAmount, isnull(sum(CASE WHEN Billed = 0 THEN Price ELSE 0 END),0) AS unbilledDesignAmount, isnull(sum(Price),0) as TotalDesignAmount from design d, customer c Where d.DesignName Like '%" + designName + "%' and d.CustNo=c.CustNo", dbConnection)
        End If

        Dim designAdapter = New SqlDataAdapter()
        designAdapter.SelectCommand = designQuery
        Dim designDataSet = New DataSet
        designAdapter.Fill(designDataSet, "designSummary")
        Return designDataSet.Tables(0)
    End Function


    Sub searchBillByDesignName(designName As String, Optional fromDate As Date = Nothing, Optional toDate As Date = Nothing)
        Dim thread As Thread = New Thread(AddressOf fetchBillByDesignName)
        thread.IsBackground = True

        Dim searchData As SearchData = New SearchData
        searchData.designName = designName
        searchData.fromDate = fromDate
        searchData.toDate = toDate

        thread.Start(searchData)
    End Sub

    Sub fetchBillByDesignName(ByVal searchDataParam As Object)
        Dim searchData As SearchData = CType(searchDataParam, SearchData)
        Dim billTable As DataTable = fetchBillTableByDesignName(searchData)
        Dim showBillSearchResultInvoker As New showBillSearchResultDelegate(AddressOf Me.showBillSearchResult)
        Me.BeginInvoke(showBillSearchResultInvoker, billTable)
        Dim billSummaryTable As DataTable = fetchBillSummaryTableByDesignName(searchData)
        Dim showBillSummarySearchResultInvoker As New showBillSummarySearchResultDelegate(AddressOf Me.showBillSummarySearchResult)
        Me.BeginInvoke(showBillSummarySearchResultInvoker, billSummaryTable)
    End Sub

    Function fetchBillTableByDesignName(searchData As SearchData) As DataTable
        Dim designName As String = searchData.designName
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate

        Dim billQuery As SqlCommand

        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            billQuery = New SqlCommand("select c.CompName, b.BillNo, b.DisplayBillNo, b.BillDate, b.DesignCost, b.UnPaidAmountTillNow, b.CGST, b.CGST*b.DesignCost/100 as CGSTAmount, 
                            b.SGST, b.SGST*b.DesignCost/100 as SGSTAmount, b.IGST, b.IGST*b.DesignCost/100 as IGSTAmount, (b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100 as GSTAmount, ((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost as DesignAmountGST, b.PaidAmount, 
                            ((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost+b.UnPaidAmountTillNow as TotalAmount, 
                            (((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost+b.UnPaidAmountTillNow)-b.PaidAmount as RemainingBalance, b.Cancelled from bill b, customer c where b.billNo in " +
                            "(select BillNo from Design where DesignName like '%" + designName + "%') and b.CustNo=c.CustNo and cast(b.BillDate as date)>='" + fromDateStr + "' and cast(b.BillDate as date)<='" + toDateStr + "'", dbConnection)

        Else
            billQuery = New SqlCommand("select c.CompName, b.BillNo, b.DisplayBillNo, b.BillDate, b.DesignCost, b.UnPaidAmountTillNow, b.CGST, b.CGST*b.DesignCost/100 as CGSTAmount, 
                            b.SGST, b.SGST*b.DesignCost/100 as SGSTAmount, b.IGST, b.IGST*b.DesignCost/100 as IGSTAmount, (b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100 as GSTAmount, ((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost as DesignAmountGST, b.PaidAmount, 
                            ((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost+b.UnPaidAmountTillNow as TotalAmount, 
                            (((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost+b.UnPaidAmountTillNow)-b.PaidAmount as RemainingBalance, b.Cancelled from bill b, customer c where b.billNo in " +
                            "(select BillNo from Design where DesignName like '%" + designName + "%') and b.CustNo=c.CustNo", dbConnection)

        End If

        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "bill")
        Return billDataSet.Tables(0)
    End Function

    Function fetchBillSummaryTableByDesignName(searchData As SearchData) As DataTable
        Dim designName As String = searchData.designName
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate

        Dim billQuery As SqlCommand

        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            billQuery = New SqlCommand("select count(1) as BillsCount, isnull(sum(DesignAmountGST),0) as TotBilledAmount from(
                            select ((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost as DesignAmountGST from bill b, customer c where b.billNo in " +
                            "(select BillNo from Design where DesignName like '%" + designName + "%') and b.CustNo=c.CustNo and cast(b.BillDate as date)>='" + fromDateStr + "' and cast(b.BillDate as date)<='" + toDateStr + "') as billTable", dbConnection)

        Else
            billQuery = New SqlCommand("select count(1) as BillsCount, isnull(sum(DesignAmountGST),0) as TotBilledAmount from(
                            select ((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost as DesignAmountGST   from bill b, customer c where b.billNo in " +
                            "(select BillNo from Design where DesignName like '%" + designName + "%') and b.CustNo=c.CustNo) as billTable", dbConnection)

        End If

        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "billSummary")
        Return billDataSet.Tables(0)
    End Function

    Sub searchPaymentByDesignName(designName As String, Optional fromDate As Date = Nothing, Optional toDate As Date = Nothing)
        Dim thread As Thread = New Thread(AddressOf fetchPaymentByDesignName)
        thread.IsBackground = True

        Dim searchData As SearchData = New SearchData
        searchData.designName = designName
        searchData.fromDate = fromDate
        searchData.toDate = toDate

        thread.Start(searchData)
    End Sub

    Sub fetchPaymentByDesignName(ByVal searchDataParam As Object)
        Dim searchData As SearchData = CType(searchDataParam, SearchData)
        Dim paymentTable As DataTable = fetchPaymentTableByDesignName(searchData)
        Dim showPaymentSearchResultInvoker As New showPaymentSearchResultDelegate(AddressOf Me.showPaymentSearchResult)
        Me.BeginInvoke(showPaymentSearchResultInvoker, paymentTable)
        Dim paymentSummaryTable As DataTable = fetchPaymentSummaryTableByDesignName(searchData)
        Dim showPaymentSummarySearchResultInvoker As New showPaymentSummarySearchResultDelegate(AddressOf Me.showPaymentSummarySearchResult)
        Me.BeginInvoke(showPaymentSummarySearchResultInvoker, paymentSummaryTable)
    End Sub

    Function fetchPaymentTableByDesignName(searchData As SearchData) As DataTable
        Dim designName As String = searchData.designName
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate

        Dim paymentQuery As SqlCommand

        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            paymentQuery = New SqlCommand("select c.CompName, p.* from payment p, design d, customer c where d.DesignName like '%" + designName + "%' and d.CustNo=c.CustNo and p.custNo=c.custNo and d.BillNo = p.BillNo and cast(d.designDate as date)>='" + fromDateStr + "' and  cast(d.designDate as date)<='" + toDateStr + "'", dbConnection)
        Else
            paymentQuery = New SqlCommand("Select c.CompName, p.* from payment p, design d, customer c Where d.DesignName Like '%" + designName + "%' and d.CustNo=c.CustNo and p.custNo=c.custNo and d.BillNo = p.BillNo", dbConnection)
        End If

        Dim paymentAdapter = New SqlDataAdapter()
        paymentAdapter.SelectCommand = paymentQuery
        Dim paymentDataSet = New DataSet
        paymentAdapter.Fill(paymentDataSet, "payment")
        Return paymentDataSet.Tables(0)
    End Function

    Function fetchPaymentSummaryTableByDesignName(searchData As SearchData) As DataTable
        Dim designName As String = searchData.designName
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate

        Dim paymentQuery As SqlCommand

        If (fromDate <> Nothing) Then
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            paymentQuery = New SqlCommand("select count(1) as PaymentCount, isnull(sum(p.ActualPaidAmount),0) as TotPaidAmountActual, isnull(sum(p.FinalPaidAmount),0) as TotPaidAmountAfterAdjustment from payment p, design d, customer c where d.DesignName like '%" + designName + "%' and d.CustNo=c.CustNo and p.custNo=c.custNo and d.BillNo = p.BillNo and cast(d.designDate as date)>='" + fromDateStr + "' and  cast(d.designDate as date)<='" + toDateStr + "'", dbConnection)
        Else
            paymentQuery = New SqlCommand("select count(1) as PaymentCount, isnull(sum(p.ActualPaidAmount),0) as TotPaidAmountActual, isnull(sum(p.FinalPaidAmount),0) as TotPaidAmountAfterAdjustment from payment p, design d, customer c Where d.DesignName Like '%" + designName + "%' and d.CustNo=c.CustNo and p.custNo=c.custNo and d.BillNo = p.BillNo", dbConnection)
        End If

        Dim paymentAdapter = New SqlDataAdapter()
        paymentAdapter.SelectCommand = paymentQuery
        Dim paymentDataSet = New DataSet
        paymentAdapter.Fill(paymentDataSet, "paymentSummary")
        Return paymentDataSet.Tables(0)
    End Function


    Sub searchDesignByDateRange(fromDate As Date, toDate As Date)
        Dim thread As Thread = New Thread(AddressOf fetchDesignByDateRange)
        thread.IsBackground = True

        Dim searchData As SearchData = New SearchData
        searchData.fromDate = fromDate
        searchData.toDate = toDate

        thread.Start(searchData)
    End Sub

    Sub fetchDesignByDateRange(ByVal searchDataParam As Object)
        Dim searchData As SearchData = CType(searchDataParam, SearchData)
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate
        Dim designTable As DataTable = fetchDesignTableByDateRange(fromDate, toDate)
        Dim showDesignSearchResultInvoker As New showDesignSearchResultDelegate(AddressOf Me.showDesignSearchResult)
        Me.BeginInvoke(showDesignSearchResultInvoker, designTable)
        Dim designSummaryTable As DataTable = fetchDesignSummaryTableByDateRange(fromDate, toDate)
        Dim showDesignSummarySearchResultInvoker As New showDesignSummarySearchResultDelegate(AddressOf Me.showDesignSummarySearchResult)
        Me.BeginInvoke(showDesignSummarySearchResultInvoker, designSummaryTable)
    End Sub

    Function fetchDesignTableByDateRange(fromDate As Date, toDate As Date) As DataTable

        Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
        Dim toDateStr As String = toDate.ToString("yyyyMMdd")

        Dim designQuery As SqlCommand
        designQuery = New SqlCommand("select c.CompName,d.* from design d,customer c where cast(d.designDate as date)>='" + fromDateStr + "' and cast(d.designDate as date)<='" + toDateStr + "' and d.CustNo=c.CustNo", dbConnection)

        Dim designAdapter = New SqlDataAdapter()
        designAdapter.SelectCommand = designQuery
        Dim designDataSet = New DataSet
        designAdapter.Fill(designDataSet, "design")
        Return designDataSet.Tables(0)
    End Function

    Function fetchDesignSummaryTableByDateRange(fromDate As Date, toDate As Date) As DataTable

        Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
        Dim toDateStr As String = toDate.ToString("yyyyMMdd")

        Dim designQuery As SqlCommand
        designQuery = New SqlCommand("select count(1) as designCount, isnull(sum(CASE WHEN d.Billed = 1 THEN d.Price ELSE 0 END),0) AS billedDesignAmount, isnull(sum(CASE WHEN d.Billed = 0 THEN d.Price ELSE 0 END),0) AS unbilledDesignAmount, isnull(sum(d.Price),0) as TotalDesignAmount from design d,customer c where cast(d.designDate as date)>='" + fromDateStr + "' and cast(d.designDate as date)<='" + toDateStr + "' and d.CustNo=c.CustNo", dbConnection)

        Dim designAdapter = New SqlDataAdapter()
        designAdapter.SelectCommand = designQuery
        Dim designDataSet = New DataSet
        designAdapter.Fill(designDataSet, "designSummary")
        Return designDataSet.Tables(0)
    End Function

    Sub searchBillByDateRange(fromDate As Date, toDate As Date)
        Dim thread As Thread = New Thread(AddressOf fetchBillByDateRange)
        thread.IsBackground = True

        Dim searchData As SearchData = New SearchData
        searchData.fromDate = fromDate
        searchData.toDate = toDate

        thread.Start(searchData)
    End Sub

    Sub fetchBillByDateRange(ByVal searchDataParam As Object)
        Dim searchData As SearchData = CType(searchDataParam, SearchData)
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate
        Dim billTable As DataTable = fetchBillTableByDateRange(fromDate, toDate)
        Dim showBillSearchResultInvoker As New showBillSearchResultDelegate(AddressOf Me.showBillSearchResult)
        Me.BeginInvoke(showBillSearchResultInvoker, billTable)
        Dim billSummaryTable As DataTable = fetchBillSummaryTableByDateRange(fromDate, toDate)
        Dim showBillSummarySearchResultInvoker As New showBillSummarySearchResultDelegate(AddressOf Me.showBillSummarySearchResult)
        Me.BeginInvoke(showBillSummarySearchResultInvoker, billSummaryTable)
    End Sub

    Function fetchBillTableByDateRange(fromDate As Date, toDate As Date) As DataTable

        Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
        Dim toDateStr As String = toDate.ToString("yyyyMMdd")

        Dim billQuery As SqlCommand

        billQuery = New SqlCommand("select c.CompName, b.BillNo, b.DisplayBillNo, b.BillDate, b.DesignCost, b.UnPaidAmountTillNow, b.CGST, b.CGST*b.DesignCost/100 as CGSTAmount, 
                            b.SGST, b.SGST*b.DesignCost/100 as SGSTAmount, b.IGST, b.IGST*b.DesignCost/100 as IGSTAmount, (b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100 as GSTAmount, ((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost as DesignAmountGST, b.PaidAmount, 
                            ((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost+b.UnPaidAmountTillNow as TotalAmount, 
                            (((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost+b.UnPaidAmountTillNow)-b.PaidAmount as RemainingBalance, b.Cancelled from bill b, customer c where cast(b.BillDate as date)>='" + fromDateStr + "' and  cast(b.BillDate as date)<='" + toDateStr + "' and b.CustNo=c.CustNo", dbConnection)

        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "bill")
        Return billDataSet.Tables(0)
    End Function

    Function fetchBillSummaryTableByDateRange(fromDate As Date, toDate As Date) As DataTable

        Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
        Dim toDateStr As String = toDate.ToString("yyyyMMdd")

        Dim billQuery As SqlCommand

        billQuery = New SqlCommand("select count(1) as BillsCount, isnull(sum(DesignAmountGST),0) as TotBilledAmount from(
                            select ((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100)+b.DesignCost as DesignAmountGST from bill b, customer c where cast(b.BillDate as date)>='" + fromDateStr + "' and  cast(b.BillDate as date)<='" + toDateStr + "' and b.CustNo=c.CustNo) as billTable", dbConnection)

        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "bill")
        Return billDataSet.Tables(0)
    End Function

    Sub searchPaymentByDateRange(fromDate As Date, toDate As Date)
        Dim thread As Thread = New Thread(AddressOf fetchPaymentByDateRange)
        thread.IsBackground = True

        Dim searchData As SearchData = New SearchData
        searchData.fromDate = fromDate
        searchData.toDate = toDate

        thread.Start(searchData)
    End Sub

    Sub fetchPaymentByDateRange(ByVal searchDataParam As Object)
        Dim searchData As SearchData = CType(searchDataParam, SearchData)
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate
        Dim paymentTable As DataTable = fetchPaymentTableByDateRange(fromDate, toDate)
        Dim showPaymentSearchResultInvoker As New showPaymentSearchResultDelegate(AddressOf Me.showPaymentSearchResult)
        Me.BeginInvoke(showPaymentSearchResultInvoker, paymentTable)
        Dim paymentSummaryTable As DataTable = fetchPaymentSummaryTableByDateRange(fromDate, toDate)
        Dim showPaymentSummarySearchResultInvoker As New showPaymentSummarySearchResultDelegate(AddressOf Me.showPaymentSummarySearchResult)
        Me.BeginInvoke(showPaymentSummarySearchResultInvoker, paymentSummaryTable)
    End Sub

    Function fetchPaymentTableByDateRange(fromDate As Date, toDate As Date) As DataTable

        Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
        Dim toDateStr As String = toDate.ToString("yyyyMMdd")

        Dim paymentQuery As SqlCommand
        paymentQuery = New SqlCommand("select c.CompName, p.*, p.UnPaidBilledAmount - p.FinalPaidAmount as NetBalance, b.DisplayBillNo  from payment p, bill b, customer c where p.BillNo = b.BillNo and p.custno = c.custno and cast(p.PaymentDate as date)>='" + fromDateStr + "' and cast(p.PaymentDate as date)<='" + toDateStr + "'", dbConnection)

        Dim paymentAdapter = New SqlDataAdapter()
        paymentAdapter.SelectCommand = paymentQuery
        Dim paymentDataSet = New DataSet
        paymentAdapter.Fill(paymentDataSet, "payment")
        Return paymentDataSet.Tables(0)
    End Function

    Function fetchPaymentSummaryTableByDateRange(fromDate As Date, toDate As Date) As DataTable

        Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
        Dim toDateStr As String = toDate.ToString("yyyyMMdd")

        Dim paymentQuery As SqlCommand
        paymentQuery = New SqlCommand("select count(1) as PaymentCount, isnull(sum(p.ActualPaidAmount),0) as TotPaidAmountActual, isnull(sum(p.FinalPaidAmount),0) as TotPaidAmountAfterAdjustment from payment p, bill b, customer c where p.BillNo = b.BillNo and p.custno = c.custno and cast(p.PaymentDate as date)>='" + fromDateStr + "' and cast(p.PaymentDate as date)<='" + toDateStr + "'", dbConnection)

        Dim paymentAdapter = New SqlDataAdapter()
        paymentAdapter.SelectCommand = paymentQuery
        Dim paymentDataSet = New DataSet
        paymentAdapter.Fill(paymentDataSet, "paymentSummary")
        Return paymentDataSet.Tables(0)
    End Function

    Private Sub cmbDesDesignList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDesDesignList.SelectedIndexChanged
         
        If (cmbDesDesignList.SelectedIndex = -1 Or cmbDesDesignList.SelectedValue = -1) Then
            resetDesignScreen()
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
            txtDesHeight.Text = dataRow.Item("Height")
            txtDesWidth.Text = dataRow.Item("Width")
            txtDesNoOfColors.Text = dataRow.Item("Colors")
            txtDesCostPerUnit.Text = dataRow.Item("UnitCost")
            If dataRow.Item("Type").ToString.Equals("WP/Inch") Then
                radioDesWP.Checked = True
            ElseIf dataRow.Item("Type").ToString.Equals("Working/Color") Then
                radioDesWorking.Checked = True
            Else
                radioDesPrint.Checked = True
            End If
            If dataRow.Item("Image") Is DBNull.Value Then
                pbDesDesignImage.Image = Nothing
            Else
                Dim designImage() As Byte = CType(dataRow.Item("Image"), Byte())
                Dim designImageBuffer As New MemoryStream(designImage)
                pbDesDesignImage.Image = Image.FromStream(designImageBuffer)
            End If
            txtDesCalculatedPrice.Text = dataRow.Item("Price")
            dpDesDesignDate.Text = dataRow.Item("DesignDate")
        Else
            MessageBox.Show("No data found for design: " + cmbDesDesignList.Text)
        End If


        ''Catch ex As Exception
        '    'MessageBox.Show("Message to Agni User:   " & ex.Message)
        ''End 'Try
    End Sub

End Class


