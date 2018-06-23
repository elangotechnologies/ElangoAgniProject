Imports System.Data.SqlClient
Imports System.IO
Imports System.Threading
Imports NLog

Public Class AgniMainForm
    Dim dbConnection As SqlConnection

    Dim log As Logger = LogManager.GetCurrentClassLogger()

    Dim BILL_TYPE_UNBILLED As Int16 = 0
    Dim BILL_TYPE_BILLED As Int16 = 1
    Dim BILL_TYPE_ALL As Int16 = 2

    Public gSelectedBillNo As Integer = -1
    Public gSelectedDisplayBillNo As String = String.Empty
    Public gSelectedCustNo As Integer = -1
    Public gSelectedCustName As String = String.Empty
    Public gSelectedDesignNo As Integer = -1

    Public gSearchFilter As Integer = -1
    Public gSearchFromDate As Date = Nothing
    Public gSearchToDate As Date = Nothing

    Private gDBConnInitialized As Boolean = False
    Private gFormLoadCompleted As Boolean = False

    Public gBillSearchResultDataSet As DataSet = Nothing
    Public gPaymentSearchResultDataSet As DataSet = Nothing
    Public gBillAndPaymentHistoryDataSet As DataSet = Nothing
    Public gReportSearchFilterText As String = Nothing
    Dim reportControlsPlaceHolders() As ElaCustomGroupBoxControl.ElaCustomGroupBox

    Private ATTRIBUTE_LAST_BILL_NO As String = "last_bill_no"

    Private Sub resetGlobalVairables()
        gSelectedBillNo = -1
        gSelectedDisplayBillNo = String.Empty
        gSelectedCustNo = -1
        gSelectedCustName = String.Empty
        gSelectedDesignNo = -1
        gSearchFilter = -1
        gSearchFromDate = Nothing
        gSearchToDate = Nothing
    End Sub


    Private Sub AgnimainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If MessageBox.Show("This will close the program." + vbNewLine + "Are you really want to close?", "Application Closing", System.Windows.Forms.MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
            e.Cancel = True
        Else
            Login.Close()
            closeDBConnection()
        End If
    End Sub


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dbConnection = getDBConnection()

        gDBConnInitialized = True

        dpBillingBillDate.Value = DateTime.Today

        loadCustomerList()
        loadCustomerGrid()

        resetAllScreens()
        Me.AcceptButton = btnReportSearch
        cmbReportCustomerList.Focus()

        alignReportControls()
        gFormLoadCompleted = True

        reportControlsPlaceHolders = {reportPlaceHolder1, reportPlaceHolder2, reportPlaceHolder3, reportPlaceHolder4, reportPlaceHolder5}
        cbReportSearchByFilterChanged(cbReportSearchByCustomer, Nothing)

        handleUserPermissions(Login.gIsCurrentUserAdministrator)

    End Sub

    Private Sub handleUserPermissions(isCurrentUserAdministrator As Boolean)
        If isCurrentUserAdministrator Then
            setEnabledStateForWriteOperations(True)
        Else
            setEnabledStateForWriteOperations(False)
        End If
    End Sub

    Private Sub setEnabledStateForWriteOperations(enabledState As Boolean)
        btnCustAdd.Enabled = enabledState
        btnCustUpdate.Enabled = enabledState
        btnCustDelete.Enabled = enabledState
        btnDesAdd.Enabled = enabledState
        btnDesUpdate.Enabled = enabledState
        btnDesDelete.Enabled = enabledState
        btnBillingCreateBill.Enabled = enabledState
        btnBillingDeleteBill.Enabled = enabledState
        btnBillingCancelBill.Enabled = enabledState
        btnPaymentCreatePayment.Enabled = enabledState
        btnPaymentDelete.Enabled = enabledState
        btnSettingsResetBilNo.Enabled = enabledState
        btnSettingsBackupDatabase.Enabled = enabledState
    End Sub

    Function getDummyDesignTable() As DataTable
        Dim dummyDesignTable As DataTable = New DataTable
        dummyDesignTable.Columns.Add(New DataColumn("DesignNo", GetType(Int32)))
        dummyDesignTable.Columns.Add(New DataColumn("DesignName", GetType(Int32)))
        Return dummyDesignTable
    End Function

    Sub loadCustomerList()
        Dim thread As Thread = New Thread(AddressOf getCustomerListTable)
        thread.IsBackground = True
        thread.Start()
    End Sub

    Sub alignReportControls()
        groupReportCustomerName.Location = reportPlaceHolder1.Location
        groupReportDesignList.Location = reportPlaceHolder2.Location
        groupReportBillNo.Location = reportPlaceHolder3.Location
        groupReportDesignName.Location = reportPlaceHolder4.Location
        groupReportDateRange.Location = reportPlaceHolder5.Location
    End Sub

    Sub getCustomerListTable()
        'log.Debug("getCustomerListTable: entry")
        Dim customerQuery = New SqlCommand("select CustNo,CompName from customer order by CompName", dbConnection)
        Dim customerAdapter = New SqlDataAdapter()
        customerAdapter.SelectCommand = customerQuery
        Dim customerDataSet = New DataSet
        customerAdapter.Fill(customerDataSet, "customer")
        Dim customerTable As DataTable = customerDataSet.Tables(0)

        Dim setCustomerListInvoker As New setCustomerListDelegate(AddressOf Me.setCustomerList)
        Me.BeginInvoke(setCustomerListInvoker, customerTable, Nothing)
    End Sub

    Delegate Sub setCustomerListDelegate(customerTable As DataTable, cmbCompanyList As ElaCustomComboBoxControl.ElaCustomComboBox)

    Sub setCustomerList(customerTable As DataTable, Optional cmbCompanyList As ElaCustomComboBoxControl.ElaCustomComboBox = Nothing)
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

    Sub loadDesignList(Optional custNo As Integer = Nothing, Optional cmbDesignListControl As ElaCustomComboBoxControl.ElaCustomComboBox = Nothing, Optional billNo As Integer = Nothing)
        Dim thread As Thread = New Thread(AddressOf getDesignListTable)
        thread.IsBackground = True

        Dim searchData As SearchData = New SearchData
        searchData.custNo = custNo
        searchData.billNo = billNo
        searchData.comboBoxControl = cmbDesignListControl
        thread.Start(searchData)
    End Sub

    Sub getDesignListTable(ByVal searchDataParam As Object)

        Dim searchData As SearchData = CType(searchDataParam, SearchData)

        Dim custNo = searchData.custNo
        Dim billNo = searchData.billNo

        Dim designQueryStr As String
        If (custNo <> Nothing And billNo <> Nothing) Then
            designQueryStr = "select DesignNo, DesignName from design where custNo=" + custNo.ToString + " and BillNo=" + billNo.ToString
        ElseIf custNo <> Nothing Then
            designQueryStr = "select DesignNo, DesignName from design where custNo=" + custNo.ToString
        ElseIf billNo <> Nothing Then
            designQueryStr = "select DesignNo, DesignName from design where BillNo=" + billNo.ToString
        Else
            designQueryStr = "select DesignNo, DesignName from design"
        End If

        Dim designQuery As SqlCommand = New SqlCommand(designQueryStr, dbConnection)

        Dim designAdapter = New SqlDataAdapter()
        designAdapter.SelectCommand = designQuery
        Dim designDataSet = New DataSet
        designAdapter.Fill(designDataSet, "design")
        Dim designTable As DataTable = designDataSet.Tables(0)

        Dim setDesignListInvoker As New setDesignListDelegate(AddressOf Me.setDesignList)
        Me.BeginInvoke(setDesignListInvoker, designTable, searchData.comboBoxControl)
    End Sub

    Delegate Sub setDesignListDelegate(designTable As DataTable, cmbDesignListControl As ElaCustomComboBoxControl.ElaCustomComboBox)

    Sub setDesignList(designTable As DataTable, Optional cmbDesignListControl As ElaCustomComboBoxControl.ElaCustomComboBox = Nothing)
        Dim dummyFirstRow As DataRow = designTable.NewRow()
        dummyFirstRow("DesignNo") = -1
        dummyFirstRow("DesignName") = "Please select a design..."
        designTable.Rows.InsertAt(dummyFirstRow, 0)

        If cmbDesignListControl IsNot Nothing Then
            cmbDesignListControl.BindingContext = New BindingContext()
            cmbDesignListControl.DataSource = designTable
        Else
            cmbDesDesignList.BindingContext = New BindingContext()
            cmbDesDesignList.DataSource = designTable
        End If

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

    Sub loadCustomerGrid()
        Dim thread As Thread = New Thread(AddressOf getCustomerGridTable)
        thread.IsBackground = True
        thread.Start()
    End Sub

    Sub getCustomerGridTable()

        Dim customerTable As DataTable = fetchCustomerTable()
        Dim setCustomerGridInvoker As New setCustomerGridDelegate(AddressOf Me.setCustomerGrid)
        Me.BeginInvoke(setCustomerGridInvoker, customerTable)
    End Sub

    Function fetchCustomerTable() As DataTable

        Dim customerQuery As SqlCommand = New SqlCommand("select * from customer order by CompName", dbConnection)

        Dim customerAdapter = New SqlDataAdapter()
        customerAdapter.SelectCommand = customerQuery
        Dim customerDataSet = New DataSet
        customerAdapter.Fill(customerDataSet, "customer")
        Return customerDataSet.Tables(0)
    End Function

    Delegate Sub setCustomerGridDelegate(customerTable As DataTable)

    Sub setCustomerGrid(customerTable As DataTable)
        dgCustCustomerDetails.DataSource = customerTable
        'log.Debug("setting customerTable count" + customerTable.Rows.Count.ToString)
        If customerTable.Rows.Count > 0 Then
            dgCustCustomerDetails.FirstDisplayedScrollingRowIndex = customerTable.Rows.Count - 1
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
            designQuery = New SqlCommand("select DesignNo, DesignDate, DesignName, Type,  Width, Height, Colors, UnitCost, Price, Billed, BillNo, Image, CustNo from design where custNo=" + custNo.ToString + " order by DesignNo ASC", dbConnection)
        Else
            designQuery = New SqlCommand("select DesignNo, DesignDate, DesignName, Type,  Width, Height, Colors, UnitCost, Price, Billed, BillNo, Image, CustNo from design order by DesignNo ASC", dbConnection)
        End If

        Dim designAdapter = New SqlDataAdapter()
        designAdapter.SelectCommand = designQuery
        Dim designDataSet = New DataSet
        designAdapter.Fill(designDataSet, "design")
        Return designDataSet.Tables(0)
    End Function

    Delegate Sub setDesignGridDelegate(designTable As DataTable)

    Sub setDesignGrid(designTable As DataTable)

        Dim primaryKey(0) As DataColumn
        primaryKey(0) = designTable.Columns("DesignNo")
        designTable.PrimaryKey = primaryKey

        dgDesDesignDetails.DataSource = designTable
        If designTable.Rows.Count > 0 Then
            dgDesDesignDetails.FirstDisplayedScrollingRowIndex = designTable.Rows.Count - 1
        End If
    End Sub

    Sub loadBillList(Optional custNo As Integer = Nothing, Optional cmbBillListControl As ElaCustomComboBoxControl.ElaCustomComboBox = Nothing)
        Dim thread As Thread = New Thread(AddressOf getBillListTable)
        thread.IsBackground = True

        Dim searchData As SearchData = New SearchData
        searchData.comboBoxControl = cmbBillListControl
        searchData.custNo = custNo
        thread.Start(searchData)

    End Sub

    Sub getBillListTable(ByVal searchDataParam As Object)

        Dim searchData As SearchData = CType(searchDataParam, SearchData)
        Dim custNo = searchData.custNo

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
        Me.BeginInvoke(setBillingListInvoker, billTable, searchData.comboBoxControl)

    End Sub

    Delegate Sub setBillingListDelegate(billTable As DataTable, cmbBillListControl As ElaCustomComboBoxControl.ElaCustomComboBox)

    Sub setBillingList(billTable As DataTable, Optional cmbBillListControl As ElaCustomComboBoxControl.ElaCustomComboBox = Nothing)

        Dim dummyFirstRow As DataRow = billTable.NewRow()
        dummyFirstRow("BillNo") = -1
        dummyFirstRow("DisplayBillNo") = "Select Bill..."
        billTable.Rows.InsertAt(dummyFirstRow, 0)

        If cmbBillListControl IsNot Nothing Then
            cmbBillListControl.BindingContext = New BindingContext()
            cmbBillListControl.DataSource = billTable
        Else
            cmbBillingBillNoList.BindingContext = New BindingContext()
            cmbBillingBillNoList.DataSource = billTable
            resetBillingScreen()
        End If

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
            billQuery = New SqlCommand("select BillNo, DisplayBillNo, BillDate, Round(DesignCost,0) as DesignCost, Round(UnPaidAmountTillNow,0) as UnPaidAmountTillNow, CGST, CGST*DesignCost/100 as CGSTAmount, 
                            SGST, SGST*DesignCost/100 as SGSTAmount, IGST, IGST*DesignCost/100 as IGSTAmount, (CGST+ SGST+ IGST)*DesignCost/100 as GSTAmount, Round(((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost,0) as BillAmount, Round(PaidAmount,0) as PaidAmount, 
                            Round(((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost,0)+Round(UnPaidAmountTillNow,0) as TotalAmount, 
                            (Round(((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost,0)+Round(UnPaidAmountTillNow,0))-Round(PaidAmount,0) as RemainingBalance, Cancelled  from bill where custNo=" + custNo.ToString, dbConnection)
        Else
            billQuery = New SqlCommand("select BillNo, DisplayBillNo, BillDate, Round(DesignCost,0) as DesignCost, Round(UnPaidAmountTillNow,0) as UnPaidAmountTillNow, CGST, CGST*DesignCost/100 as CGSTAmount, 
                            SGST, SGST*DesignCost/100 as SGSTAmount, IGST, IGST*DesignCost/100 as IGSTAmount, (CGST+ SGST+ IGST)*DesignCost/100 as GSTAmount, Round(((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost,0) as BillAmount, Round(PaidAmount,0) as PaidAmount, 
                            Round(((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost,0)+Round(UnPaidAmountTillNow,0) as TotalAmount, 
                            (Round(((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost,0)+Round(UnPaidAmountTillNow,0))-Round(PaidAmount,0) as RemainingBalance, Cancelled  from bill", dbConnection)
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

        If billTable.Rows.Count > 0 Then
            dgBIllingBillDetails.FirstDisplayedScrollingRowIndex = billTable.Rows.Count - 1
        End If
    End Sub

    Sub loadLastBill(Optional custNo As Integer = Nothing)
        Dim thread As Thread = New Thread(AddressOf getLastBillRowInThread)
        thread.IsBackground = True

        Dim searchData As SearchData = New SearchData
        searchData.custNo = custNo
        thread.Start(searchData)

    End Sub

    Sub getLastBillRowInThread(ByVal searchDataParam As Object)

        Dim searchData As SearchData = CType(searchDataParam, SearchData)
        Dim custNo = searchData.custNo

        Dim lastBillRow As DataRow = getLastBillRow(custNo)

        Dim setLastBillInScreenInvoker As New setLastBillInScreenDelegate(AddressOf Me.setLastBillInScreen)
        Me.BeginInvoke(setLastBillInScreenInvoker, lastBillRow)

    End Sub

    Delegate Sub setLastBillInScreenDelegate(billTable As DataRow)

    Sub setLastBillInScreen(billRow As DataRow)

        If billRow IsNot Nothing Then
            panelLastBillNo.Visible = True
            txtBillingLastBillNo.Text = billRow.Item("DisplayBillNo")
            txtBillingLastBillAmount.Text = Format(billRow.Item("BillAmount"), "0.00")
        Else
            panelLastBillNo.Visible = False
            txtBillingLastBillNo.Text = ""
            txtBillingLastBillAmount.Text = ""
        End If


    End Sub

    Sub loadPaymentList(custNo As Integer)
        Dim thread As Thread = New Thread(AddressOf getPaymentListTable)
        thread.IsBackground = True
        thread.Start(custNo)
    End Sub

    Sub getPaymentListTable(ByVal custNoObj As Object)
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

    Delegate Sub setPaymentListDelegate(paymentTable As DataTable, cmbPaymentList As ElaCustomComboBoxControl.ElaCustomComboBox)

    Sub setPaymentList(paymentTable As DataTable, Optional cmbPaymentList As ElaCustomComboBoxControl.ElaCustomComboBox = Nothing)

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

        Dim paymentQuery As SqlCommand
        If (custNo <> Nothing) Then
            paymentQuery = New SqlCommand("select p.PaymentNo, p.BillNo, p.PaymentDate, p.PaymentMode, round(p.UnPaidBilledAmount, 0) as UnPaidBilledAmount, round(p.ActualPaidAmount, 0) as ActualPaidAmount, round(p.Discount,0) as Discount, round(p.FinalPaidAmount,0) as FinalPaidAmount, round(p.UnPaidBilledAmount - p.FinalPaidAmount, 0) as NetBalance, p.ChequeNo, p.BankName, p.ChequeDate, p.Remarks, b.DisplayBillNo from payment p, bill b where p.BillNo = b.BillNo and p.custNo=" + custNo.ToString, dbConnection)
        Else
            paymentQuery = New SqlCommand("select p.PaymentNo, p.BillNo, p.PaymentDate, p.PaymentMode, round(p.UnPaidBilledAmount, 0) as UnPaidBilledAmount, round(p.ActualPaidAmount, 0) as ActualPaidAmount, round(p.Discount,0) as Discount, round(p.FinalPaidAmount,0) as FinalPaidAmount, round(p.UnPaidBilledAmount - p.FinalPaidAmount, 0) as NetBalance, p.ChequeNo, p.BankName, p.ChequeDate, p.Remarks, b.DisplayBillNo from payment p, bill b where p.BillNo = b.BillNo", dbConnection)
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

        If paymentTable.Rows.Count > 0 Then
            dgPaymentDetails.FirstDisplayedScrollingRowIndex = paymentTable.Rows.Count - 1
        End If
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
    End Sub


    Private Sub cmbCustCompanyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCustCustomerList.SelectedIndexChanged

        If (cmbCustCustomerList.SelectedIndex = -1 Or cmbCustCustomerList.SelectedValue = -1) Then
            resetCustomerScreen()
            gSelectedCustNo = -1
            Return
        End If

        Dim custNo As Integer = cmbCustCustomerList.SelectedValue
        gSelectedCustNo = custNo

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

            txtAddressLine1.Text = If(dataRow.Item("AddressLine1") Is DBNull.Value, String.Empty, dataRow.Item("AddressLine1"))
            txtAddressLine2.Text = If(dataRow.Item("AddressLine2") Is DBNull.Value, String.Empty, dataRow.Item("AddressLine2"))
            txtAddressLine3.Text = If(dataRow.Item("AddressLine3") Is DBNull.Value, String.Empty, dataRow.Item("AddressLine3"))
            txtAddressLine4.Text = If(dataRow.Item("AddressLine4") Is DBNull.Value, String.Empty, dataRow.Item("AddressLine4"))
            txtAddressLine5.Text = If(dataRow.Item("AddressLine5") Is DBNull.Value, String.Empty, dataRow.Item("AddressLine5"))

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
        txtAddressLine1.Text = ""
        txtAddressLine2.Text = ""
        txtAddressLine3.Text = ""
        txtAddressLine4.Text = ""
        txtAddressLine5.Text = ""
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
        cmbDesCustomerList.Focus()
    End Sub

    Sub resetBillingControlsVisibilities()
        Me.AcceptButton = btnBillingCreateBill
        cmbBillingBillNoList.Enabled = True
        btnBillingCreateBill.Visible = True
        btnBillingDeleteBill.Visible = True
        btnBillingClear.Visible = True
        btnBillingPrintBill.Visible = True
        btnBillingCancelBill.Visible = True
        btnBillingConfirmCreateBill.Visible = False
        btnBillingCancelCreateBill.Visible = False
        btnBillingCancelBill.Text = "Mark Cancelled"
        lblCancelledBillIndicator.Visible = False
    End Sub

    Sub setBillingControlsVisibilitiesForCreateBill()
        Me.AcceptButton = btnBillingConfirmCreateBill
        cmbBillingBillNoList.Enabled = False
        btnBillingCreateBill.Visible = False
        btnBillingConfirmCreateBill.Visible = True
        btnBillingCancelCreateBill.Visible = True
        btnBillingDeleteBill.Visible = False
        btnBillingClear.Visible = False
        btnBillingPrintBill.Visible = False
        btnBillingCancelBill.Visible = False
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
        cmbBillingCustomerList.Focus()
    End Sub

    Sub resetPaymentControlsVisibilities()
        Me.AcceptButton = btnPaymentCreatePayment
        btnPaymentCreatePayment.Visible = True
        btnPaymentConfirmCreatePayment.Visible = False
        btnPaymentCancelCreatePayment.Visible = False
        cmbPaymentPaymentNoList.Enabled = True
        txtPaymentActualPaidAmount.ReadOnly = True
        txtPaymentDiscountAmount.ReadOnly = True
        dpPaymentDate.Enabled = False
        btnPaymentDelete.Visible = True
        btnPaymentClear.Visible = True
    End Sub

    Sub setPaymentControlsVisibilitiesForCreatePayment()
        Me.AcceptButton = btnPaymentConfirmCreatePayment
        cmbPaymentPaymentNoList.Enabled = False
        btnPaymentCreatePayment.Visible = False
        btnPaymentConfirmCreatePayment.Visible = True
        btnPaymentCancelCreatePayment.Visible = True
        txtPaymentActualPaidAmount.ReadOnly = False
        txtPaymentDiscountAmount.ReadOnly = False
        dpPaymentDate.Enabled = True
        btnPaymentDelete.Visible = False
        btnPaymentClear.Visible = False
        'log.Debug("setPaymentControlsVisibilitiesForCreatePayment called and done")
    End Sub

    Sub resetPaymentScreen()
        resetPaymentControlsVisibilities()

        txtPaymentBillNo.Text = ""
        txtPaymentDisplayBillNo.Text = ""
        dpPaymentDate.Text = ""
        radioPaymentByCash.Checked = True
        txtPaymentActualPaidAmount.Text = ""
        txtPaymentDiscountAmount.Text = ""
        txtPaymentFinalPaidAmount.Text = ""
        txtPaymentChequeNo.Text = ""
        txtPaymentBankName.Text = ""
        dpPaymentChequeDate.Text = ""
        txtPaymentNetBalance.Text = ""
        txtPaymentRemarks.Text = ""
        txtPaymentUnPaidBilledAmount.Text = ""
        'log.Debug("resetPaymentScreen is called and done")
    End Sub

    Public Sub btnCustAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustAdd.ClickButtonArea
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
            ElseIf txtAddressLine1.Text.Trim.Equals("") Then
                MessageBox.Show("Enter at least one line address in Address Line1")
                txtAddressLine1.Focus()
            ElseIf txtMobile.Text.Trim.Equals("") Then
                MessageBox.Show("Enter Mobile Number")
                txtMobile.Focus()
            Else
                Dim query As String = String.Empty
                query &= "INSERT INTO customer (CompName, GSTIN, OwnerName, AddressLine1, AddressLine2, AddressLine3, AddressLine4, AddressLine5, Mobile, Landline, Email, Website, "
                query &= "CGST, SGST, IGST, WorkingPrintSqrInch, WorkingColor, PrintColor) "
                query &= "VALUES ( @CompName, @GSTIN, @OwnerName, @AddressLine1, @AddressLine2, @AddressLine3, @AddressLine4, @AddressLine5, @Mobile, @Landline, @Email, "
                query &= "@Website, @CGST, @SGST, @IGST, @WorkingPrintSqrInch, @WorkingColor, @PrintColor)"

                Using comm As New SqlCommand()
                    With comm
                        .Connection = dbConnection
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@CompName", cmbCustCustomerList.Text)
                        .Parameters.AddWithValue("@GSTIN", txtGstIn.Text)
                        .Parameters.AddWithValue("@OwnerName", txtOwnerName.Text)
                        .Parameters.AddWithValue("@AddressLine1", txtAddressLine1.Text)
                        .Parameters.AddWithValue("@AddressLine2", txtAddressLine2.Text)
                        .Parameters.AddWithValue("@AddressLine3", txtAddressLine3.Text)
                        .Parameters.AddWithValue("@AddressLine4", txtAddressLine4.Text)
                        .Parameters.AddWithValue("@AddressLine5", txtAddressLine5.Text)
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
                loadCustomerGrid()
            End If
        Catch sqlEx As SqlException
            MsgBox("Duplicate customer entry. Please check if any other customer exists with same customer name")
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub btnCustDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustDelete.ClickButtonArea
        If (cmbCustCustomerList.SelectedIndex = -1 Or cmbCustCustomerList.SelectedValue = -1) Then
            MessageBox.Show("Please select a customer")
            cmbCustCustomerList.Focus()
            Return
        End If

        If MessageBox.Show("Are you sure you want to delete this Customer? Please note that this action cannot be undone.", "WARNING", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Dim actionResult As DialogResult = ActionConfirmation.ShowDialog()
            ActionConfirmation.Dispose()

            If actionResult = System.Windows.Forms.DialogResult.Cancel Then
                'No need to anything as this result you get is when the user is canceled the Action Dialog
            ElseIf actionResult = System.Windows.Forms.DialogResult.Yes Then
                deleteSeletectedCustomer()
            ElseIf actionResult = System.Windows.Forms.DialogResult.No Then
                MsgBox("You do not have permission for this operation. Please try with Administrator user when prompted for confirmation")
            End If

        End If
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
        loadCustomerGrid()
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

    Private Sub btnCustUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustUpdate.ClickButtonArea

        If (gSelectedCustNo = -1) Then
            MessageBox.Show("Please select a customer")
            cmbCustCustomerList.Focus()
            Return
        End If

        If cmbCustCustomerList.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Valid customer Name")
            cmbCustCustomerList.Focus()
        ElseIf txtGstIn.Text.Trim.Equals("") Then
            MessageBox.Show("Enter GSTIN number")
            txtGstIn.Focus()
        ElseIf txtOwnerName.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Proprietor Name")
            txtOwnerName.Focus()
        ElseIf txtAddressLine1.Text.Trim.Equals("") Then
            MessageBox.Show("Enter at least one line address in Address Line1")
            txtAddressLine1.Focus()
        ElseIf txtMobile.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Mobile Number")
            txtMobile.Focus()
        Else
            Try
                Dim custNo As Integer = gSelectedCustNo
                Dim query As String = String.Empty
                query &= "UPDATE customer SET CompName=@CompName, GSTIN=@GSTIN, OwnerName=@OwnerName, AddressLine1=@AddressLine1, AddressLine2=@AddressLine2, AddressLine3=@AddressLine3, AddressLine4=@AddressLine4, AddressLine5=@AddressLine5,"
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
                        .Parameters.AddWithValue("@AddressLine1", txtAddressLine1.Text)
                        .Parameters.AddWithValue("@AddressLine2", txtAddressLine2.Text)
                        .Parameters.AddWithValue("@AddressLine3", txtAddressLine3.Text)
                        .Parameters.AddWithValue("@AddressLine4", txtAddressLine4.Text)
                        .Parameters.AddWithValue("@AddressLine5", txtAddressLine5.Text)
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
                loadCustomerGrid()

            Catch sqlEx As SqlException
                MsgBox("Duplicate customer entry. Please check if any other customer exists with same customer name")
            Catch ex As Exception
                MessageBox.Show("Message to Agni User:   " & ex.Message)
            End Try
        End If
    End Sub


    Private Sub btnDesAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesAdd.ClickButtonArea
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
            Try
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

            Catch sqlEx As SqlException
                MsgBox("Operation failed. DB error. Please try again or contact the software support if problem persists")
            Catch ex As Exception
                MessageBox.Show("Message to Agni User:   " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub pbDesDesignImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbDesDesignImage.Click
        loadPictureChooseDialog()
    End Sub

    Private Sub loadPictureChooseDialog()

        'log.Debug("pictureload: loading the picture")

        resetIndexOfComboBox(cmbDesDesignList)

        Dim fileDialog As New OpenFileDialog
        With fileDialog
            .Filter = "All Files|*.*|Bitmaps|*.bmp|GIFs|*.gif|JPEGs|*.jpg"
            .FilterIndex = 7
        End With
        If fileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim FileData() As Byte
            Dim fileStream As New FileStream(Trim(fileDialog.FileName), FileMode.Open)
            FileData = New [Byte](fileStream.Length) {}
            fileStream.Read(FileData, 0, fileStream.Length)
            Dim memoryStream As New MemoryStream(FileData)
            Dim fileImage As System.Drawing.Image
            fileImage = System.Drawing.Image.FromStream(memoryStream)
            Dim intPointer As New IntPtr
            Dim thumbnailImage As System.Drawing.Image
            thumbnailImage = fileImage.GetThumbnailImage(pbDesDesignImage.Width, pbDesDesignImage.Height, Nothing, intPointer)
            pbDesDesignImage.Image = thumbnailImage
            With pbDesDesignImage
                .Image = thumbnailImage
            End With
            fileStream.Close()
        End If
        Dim filepath As String = fileDialog.FileName.ToString
        Dim strFilename As String = filepath.Substring(filepath.LastIndexOf("\") + 1)
        If Not strFilename.Equals("") Then
            'removing and adding handler is to avoid the auto selection of a existing design with same name
            RemoveHandler cmbDesDesignList.SelectedIndexChanged, AddressOf cmbDesDesignList_SelectedIndexChanged
            Dim designName As String = strFilename.Substring(0, strFilename.LastIndexOf("."))
            cmbDesDesignList.Text = designName
            AddHandler cmbDesDesignList.SelectedIndexChanged, AddressOf cmbDesDesignList_SelectedIndexChanged
        End If
    End Sub
    Private Sub btnDesUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesUpdate.ClickButtonArea
        If gSelectedDesignNo = -1 Then
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
            Dim designTable As DataTable = dgDesDesignDetails.DataSource
            Dim dataRow As DataRow = designTable.Rows.Find(gSelectedDesignNo)
            Dim isBilled As Boolean = dataRow.Item("billed")
            Dim designTypeInDB As String = dataRow.Item("Type")
            Dim designWidthInDB As Decimal = dataRow.Item("Width")
            Dim designHeightInDB As Decimal = dataRow.Item("Height")
            Dim calculatedPriceInDB As Decimal = dataRow.Item("Price")
            Dim colorsInDB As Integer = dataRow.Item("Colors")
            Dim unitCostInDB As Decimal = dataRow.Item("UnitCost")

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

            Dim calculatedPrice As Decimal = Decimal.Parse(txtDesCalculatedPrice.Text.Trim)
            Dim colors As Integer = Integer.Parse(txtDesNoOfColors.Text.Trim)
            Dim unitCost As Decimal = Decimal.Parse(txtDesCostPerUnit.Text.Trim)

            If (isBilled) Then
                If designType <> designTypeInDB Then
                    MessageBox.Show("This design is already billed. Design's Type cannot be changed if the design is billed already. First delete the respective bill and try again this operation.")
                    gbDesignType.Focus()
                    Return
                End If
                If designWidth <> designWidthInDB Then
                    MessageBox.Show("This design is already billed. Design's Width cannot be changed if the design is billed already. First delete the respective bill and try again this operation.")
                    txtDesWidth.Focus()
                    Return
                End If
                If designHeight <> designHeightInDB Then
                    MessageBox.Show("This design is already billed. Design's Height cannot be changed if the design is billed already. First delete the respective bill and try again this operation.")
                    txtDesHeight.Focus()
                    Return
                End If
                If calculatedPrice <> calculatedPriceInDB Then
                    MessageBox.Show("This design is already billed. Design's price cannot be changed if the design is billed already. First delete the respective bill and try again this operation.")
                    txtDesCalculatedPrice.Focus()
                    Return
                End If
                If colors <> colorsInDB Then
                    MessageBox.Show("This design is already billed. Design's Colors Per Unit cannot be changed if the design is billed already. First delete the respective bill and try again this operation.")
                    txtDesNoOfColors.Focus()
                    Return
                End If
                If unitCost <> unitCostInDB Then
                    MessageBox.Show("This design is already billed. Design's Cost Per Unit cannot be changed if the design is billed already. First delete the respective bill and try again this operation.")
                    txtDesCostPerUnit.Focus()
                    Return
                End If
            End If

            Try
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
                        .Parameters.AddWithValue("@DesignNo", gSelectedDesignNo)
                        .Parameters.AddWithValue("@DesignName", cmbDesDesignList.Text)
                        .Parameters.AddWithValue("@Height", designHeight)
                        .Parameters.AddWithValue("@Width", designWidth)
                        .Parameters.AddWithValue("@Colors", colors)
                        .Parameters.AddWithValue("@UnitCost", unitCost)
                        .Parameters.AddWithValue("@Type", designType)
                        .Parameters.AddWithValue("@Price", calculatedPrice)
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
            Catch sqlEx As SqlException
                MsgBox("Operation failed. DB error. Please try again or contact the software support if problem persists")
            Catch ex As Exception
                MessageBox.Show("Message to Agni User:   " & ex.Message)
            End Try
        End If
    End Sub

    Sub updateRecentDesignsAsBilled(custNo As Integer, BillNo As Integer)

        If (BillNo = -1) Then
            Return
        End If
        Try
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
        Catch sqlEx As SqlException
            MsgBox("Operation failed. DB error. Please try again or contact the software support if problem persists")
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Sub updateDesignsAsUnBilled(BillNo As Integer)

        If (BillNo = -1) Then
            Return
        End If

        Try
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
        Catch sqlEx As SqlException
            MsgBox("Operation failed. DB error. Please try again or contact the software support if problem persists")
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Sub addPaidAmountInBill(BillNo As Integer, paidAmount As Decimal)

        If (BillNo = -1) Then
            Return
        End If

        Try
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
        Catch sqlEx As SqlException
            MsgBox("Operation failed. DB error. Please try again or contact the software support if problem persists")
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try
    End Sub

    Private Sub btnDesDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesDelete.ClickButtonArea
        If gSelectedDesignNo = -1 Then
            MessageBox.Show("Please select a design")
            cmbDesDesignList.Focus()
            Return
        End If

        Dim designTable As DataTable = dgDesDesignDetails.DataSource
        Dim dataRow As DataRow = designTable.Rows.Find(gSelectedDesignNo)
        Dim isBilled As Boolean = dataRow.Item("billed")
        If (isBilled) Then
            MessageBox.Show("This design is already billed. Billed designs cannot be deleted. First delete the respective bill and try again this operation.")
            cmbDesDesignList.Focus()
            Return
        End If

        If MessageBox.Show("Do you want to delete the design " & cmbDesDesignList.Text, "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Dim custNo As Integer = cmbDesCustomerList.SelectedValue

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
    End Sub

    Private Sub cmbBillingBillNoList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBillingBillNoList.SelectedIndexChanged

        gSelectedBillNo = cmbBillingBillNoList.SelectedValue
        gSelectedDisplayBillNo = cmbBillingBillNoList.Text

        If (cmbBillingBillNoList.SelectedIndex = -1 Or cmbBillingBillNoList.SelectedValue = -1) Then
            resetBillingScreen()
            gSelectedBillNo = -1
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
        End If
    End Sub

    Private Sub cmbBillingCustomerList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBillingCustomerList.SelectedIndexChanged
        If (cmbBillingCustomerList.SelectedIndex = -1 Or cmbBillingCustomerList.SelectedValue = -1) Then
            resetIndexOfComboBox(cmbBillingBillNoList)
            gSelectedCustNo = -1
            Return
        End If

        gSelectedCustName = cmbBillingCustomerList.Text
        gSelectedCustNo = cmbBillingCustomerList.SelectedValue
        panelLastBillNo.Visible = False

        Dim custNo As Integer = cmbBillingCustomerList.SelectedValue
        loadBillList(custNo)
        loadBillGrid(custNo)
        loadLastBill(custNo)
    End Sub

    Private Sub btnBillingPrintBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBillingPrintBill.ClickButtonArea
        If cmbBillingBillNoList.SelectedIndex = -1 Or cmbBillingBillNoList.SelectedValue = -1 Then
            MsgBox("Please select a bill to print")
            cmbBillingBillNoList.Focus()
            Return
        End If

        BillReportForm.ShowDialog()
    End Sub

    Private Sub cmbDesCompanyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDesCustomerList.SelectedIndexChanged
        If (cmbDesCustomerList.SelectedIndex = -1 Or cmbDesCustomerList.SelectedValue = -1) Then
            resetIndexOfComboBox(cmbDesDesignList)
            Return
        End If

        Dim custNo As Integer = cmbDesCustomerList.SelectedValue
        loadDesignChargePerUnit(custNo)
        loadDesignList(custNo)
        loadDesignGrid(custNo)

    End Sub

    Private Sub btnCustClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustClear.ClickButtonArea
        resetIndexOfComboBox(cmbCustCustomerList)
    End Sub

    Private Sub btnDesEditPrice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesEditPrice.ClickButtonArea
        txtDesCalculatedPrice.ReadOnly = Not txtDesCalculatedPrice.ReadOnly
        txtDesCalculatedPrice.Focus()
    End Sub

    Private Sub btnDesClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesClear.ClickButtonArea
        If (cmbDesDesignList.SelectedIndex = -1 Or cmbDesDesignList.SelectedValue = -1) Then
            resetDesignScreen()
            cmbDesDesignList.Text = ""
        End If

        resetIndexOfComboBox(cmbDesDesignList)
    End Sub

    Private Sub chargeTypeCheckedChanged(sender As Object, e As EventArgs) Handles radioDesWP.CheckedChanged, radioDesWorking.CheckedChanged, radioDesPrint.CheckedChanged
        If gDBConnInitialized = False Then
            Return
        End If
        loadDesignChargePerUnit(cmbDesCustomerList.SelectedValue)
    End Sub

    Private Sub btnBillingClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBillingClear.ClickButtonArea
        resetIndexOfComboBox(cmbBillingBillNoList)
    End Sub


    Private Sub btnBilingOutstandingBalance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBillingOutstandingBalance.ClickButtonArea
        CustomersOutstandingBalances.Show()
    End Sub

    Private Sub radioPaymentByCash_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioPaymentByCash.CheckedChanged
        gbBankDetails.Visible = False
    End Sub

    Private Sub radioPaymentByCheque_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioPaymentByCheque.CheckedChanged
        gbBankDetails.Visible = True
    End Sub

    Private Sub cmbPaymentCompanyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPaymentCustomerList.SelectedIndexChanged

        If (cmbPaymentCustomerList.SelectedIndex = -1 Or cmbPaymentCustomerList.SelectedValue = -1) Then
            resetIndexOfComboBox(cmbPaymentPaymentNoList)
            Return
        End If
        Dim custNo As Integer = cmbPaymentCustomerList.SelectedValue

        loadPaymentList(custNo)
        loadPaymentGrid(custNo)

    End Sub

    Sub calculatePaymentFinalPaidAmount() Handles txtPaymentActualPaidAmount.TextChanged, txtPaymentDiscountAmount.TextChanged

        Dim actualPaidAmount As Decimal = 0
        Dim discountAmount As Decimal = 0
        Dim unPaidBillAmount As Decimal = 0

        Decimal.TryParse(txtPaymentActualPaidAmount.Text, actualPaidAmount)
        Decimal.TryParse(txtPaymentDiscountAmount.Text, discountAmount)
        Decimal.TryParse(txtPaymentUnPaidBilledAmount.Text, unPaidBillAmount)

        Dim finalPaidAmount As Decimal = actualPaidAmount + discountAmount

        If unPaidBillAmount < finalPaidAmount Then
            MsgBox("The payment amount cannot be greater than the unpaid billed amount. Please correct the payment amount")
            Return
        End If

        txtPaymentFinalPaidAmount.Text = Format(Math.Round(finalPaidAmount), "0.00")
        txtPaymentNetBalance.Text = Format(Math.Round((unPaidBillAmount - finalPaidAmount)), "0.00")

    End Sub
    Private Sub btnBillingCreateBill_Click(sender As Object, e As EventArgs) Handles btnBillingCreateBill.ClickButtonArea

        If (cmbBillingCustomerList.SelectedIndex = -1 Or cmbBillingCustomerList.SelectedValue = -1) Then
            MessageBox.Show("Please select a customer")
            cmbBillingCustomerList.Focus()
            Return
        End If

        cmbBillingBillNoList.SelectedIndex = -1
        Dim custNo = cmbBillingCustomerList.SelectedValue

        Dim unBilledDesignAmount As Decimal = getDesignAmountWithoutGSTTax(BILL_TYPE_UNBILLED, custNo)
        Dim unPaidBalance As Decimal = getUnpaidBilledAmount(custNo)

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

    Function getLastBillRow(Optional custNo As Integer = Nothing) As DataRow

        Dim billSelectQuery As SqlCommand
        If (custNo <> Nothing) Then
            billSelectQuery = New SqlCommand("select *, Round(((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost,0) as BillAmount from bill where billno=(select max(billno) from bill where custno=" + custNo.ToString + ")", dbConnection)
        Else
            billSelectQuery = New SqlCommand("select *, Round(((CGST+ SGST+ IGST)*DesignCost/100)+DesignCost,0) as BillAmount from bill where billno=(select max(billno) from bill)", dbConnection)
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

    Function getLastPaymentRow(Optional custNo As Integer = Nothing, Optional withBankDetails As Boolean = False) As DataRow

        Dim paymentQueryStr As String = "select * from payment where PaymentNo=(select max(PaymentNo) from payment"
        Dim paymentQueryWhereClause As String = String.Empty

        If (custNo <> Nothing) Then
            paymentQueryWhereClause += " custNo = " + custNo.ToString
        End If

        If (withBankDetails = True) Then
            If paymentQueryWhereClause IsNot String.Empty Then
                paymentQueryWhereClause += " and "
            End If
            paymentQueryWhereClause += " BankName is not null"
        End If

        If paymentQueryWhereClause IsNot String.Empty Then
            paymentQueryStr += " where " + paymentQueryWhereClause
        End If

        paymentQueryStr += ")"

        Dim paymentSelectQuery As SqlCommand = New SqlCommand(paymentQueryStr, dbConnection)

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

    Function getUnpaidBilledAmount(Optional custNo As Integer = Nothing) As Decimal

        Dim billSelectQuery As SqlCommand
        If (custNo <> Nothing) Then
            billSelectQuery = New SqlCommand("Select sum(round(DesignCost+((isnull(CGST,0)+isnull(SGST,0)+isnull(IGST,0))*DesignCost/100),0))-sum(paidamount) as UnPaidBilledAmount from bill where custno=" + custNo.ToString, dbConnection)
        Else
            billSelectQuery = New SqlCommand("Select sum(round(DesignCost+((isnull(CGST,0)+isnull(SGST,0)+isnull(IGST,0))*DesignCost/100),0))-sum(paidamount) as UnPaidBilledAmount from bill", dbConnection)
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

    Private Sub btnBillingConfirmCreateBill_Click(sender As Object, e As EventArgs) Handles btnBillingConfirmCreateBill.ClickButtonArea
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
            Try
                Dim newBillNo As Integer = -1

                Dim query As String = String.Empty
                query &= "INSERT INTO bill (DisplayBillNo, CustNo, BillDate, DesignCost, CGST, SGST, IGST, UnPaidAmountTillNow, PaidAmount, Cancelled) "
                query &= "VALUES (@DisplayBillNo, @CustNo, @BillDate, @DesignCost, @CGST, @SGST, @IGST, @UnPaidAmountTillNow, @PaidAmount, @Cancelled); Select SCOPE_IDENTITY()"

                Dim displayBillNoStr As String = getAttribute(ATTRIBUTE_LAST_BILL_NO)
                Dim displayBillNo As Integer = 0
                If displayBillNoStr IsNot Nothing Then
                    displayBillNo = Integer.Parse(displayBillNoStr)
                End If
                displayBillNo = displayBillNo + 1


                Using comm As New SqlCommand()
                    With comm
                        .Connection = dbConnection
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@DisplayBillNo", displayBillNo)
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
                    newBillNo = CInt(comm.ExecuteScalar())
                End Using

                updateRecentDesignsAsBilled(cmbBillingCustomerList.SelectedValue, newBillNo)
                insertOrReplaceAttribute(ATTRIBUTE_LAST_BILL_NO, displayBillNo.ToString)
                MessageBox.Show("Bill successfully added")

                Dim custNo As Integer = cmbBillingCustomerList.SelectedValue

                loadBillList(custNo)
                loadBillGrid(custNo)
            Catch sqlEx As SqlException
                MsgBox("Duplicate customer entry. Please check if any other customer exists with same customer name")
            Catch ex As Exception
                MessageBox.Show("Message to Agni User:   " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnBillingCancelCreateBill_Click(sender As Object, e As EventArgs) Handles btnBillingCancelCreateBill.ClickButtonArea
        If (cmbBillingBillNoList.SelectedIndex = -1 Or cmbBillingBillNoList.SelectedValue = -1) Then
            resetBillingScreen()
            cmbBillingBillNoList.Text = ""
        End If

        resetIndexOfComboBox(cmbBillingBillNoList)

    End Sub

    Private Sub btnBillingCancelBill_Click(sender As Object, e As EventArgs) Handles btnBillingCancelBill.ClickButtonArea

        If (cmbBillingBillNoList.SelectedIndex = -1 Or cmbBillingBillNoList.SelectedValue = -1) Then
            MessageBox.Show("Please Select a bill")
            cmbBillingBillNoList.Focus()
            Return
        End If

        Try

            Dim billNo As Integer = cmbBillingBillNoList.SelectedValue
            Dim custNo As Integer = cmbBillingCustomerList.SelectedValue

            Dim updateQuery As String = String.Empty
            updateQuery &= "update bill Set Cancelled=@Cancelled where BillNo=@BillNo"

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

            MessageBox.Show("Bill " + billNo.ToString + " Is marked As cancelled bill. You need To create a New bill For the designs which were billed In this bill")

            loadBillList(custNo)
            loadBillGrid(custNo)

        Catch sqlEx As SqlException
            MsgBox("Duplicate customer entry. Please check if any other customer exists with same customer name")
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try

    End Sub

    Private Sub btnPaymentCreatePayment_Click(sender As Object, e As EventArgs) Handles btnPaymentCreatePayment.ClickButtonArea
        If (cmbPaymentCustomerList.SelectedIndex = -1 Or cmbPaymentCustomerList.SelectedValue = -1) Then
            MessageBox.Show("Please Select a customer")
            cmbPaymentCustomerList.Focus()
            Return
        End If

        Dim custNo = cmbPaymentCustomerList.SelectedValue
        Dim unPaidBalance As Decimal = getUnpaidBilledAmount(custNo)

        Dim lastBillRow As DataRow = getLastBillRow(custNo)

        If (lastBillRow Is Nothing Or unPaidBalance = 0) Then
            MessageBox.Show("This customer has no due amount To make the payment. All bills are paid by this customer")
            Return
        End If

        resetPaymentScreen()

        Dim lastPaymentRowWithBankInfo As DataRow = getLastPaymentRow(custNo, True)
        If lastPaymentRowWithBankInfo IsNot Nothing Then
            txtPaymentBankName.Text = lastPaymentRowWithBankInfo.Item("BankName")
            txtPaymentChequeNo.Text = lastPaymentRowWithBankInfo.Item("ChequeNo")
        End If

        txtPaymentDisplayBillNo.Text = lastBillRow.Item("DisplayBillNo")
        txtPaymentBillNo.Text = lastBillRow.Item("BillNo")
        txtPaymentUnPaidBilledAmount.Text = Format(Math.Round(unPaidBalance), "0.00")

        'log.Debug("btnPaymentCreatePayment_Click: before calling setPaymentControlsVisibilitiesForCreatePayment")
        setPaymentControlsVisibilitiesForCreatePayment()

        txtPaymentActualPaidAmount.Focus()
    End Sub

    Private Sub btnPaymentConfirmCreatePayment_Click(sender As Object, e As EventArgs) Handles btnPaymentConfirmCreatePayment.ClickButtonArea

        If cmbPaymentCustomerList.Text.Trim.Equals("") Then
            MessageBox.Show("Choose a company from company list")
            cmbPaymentCustomerList.Focus()
        ElseIf dpPaymentDate.Text.Trim.Equals("") Then
            MessageBox.Show("Choose valid payment date")
            dpPaymentDate.Focus()
        ElseIf Val(txtPaymentUnPaidBilledAmount.Text) = 0 Then
            MessageBox.Show("There is no balance anmount to pay")
            txtPaymentUnPaidBilledAmount.Focus()
        ElseIf txtPaymentActualPaidAmount.Text.Trim.Equals("") And txtPaymentDiscountAmount.Text.Trim.Equals("") Then
            MessageBox.Show("Enter Payment Amount or Discount")
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

            Try

                Dim actualPaidAmount As Decimal = 0
                Dim discountAmount As Decimal = 0
                Dim unPaidBillAmount As Decimal = 0

                Decimal.TryParse(txtPaymentActualPaidAmount.Text, actualPaidAmount)
                Decimal.TryParse(txtPaymentDiscountAmount.Text, discountAmount)
                Decimal.TryParse(txtPaymentUnPaidBilledAmount.Text, unPaidBillAmount)

                Dim finalPaidAmount As Decimal = actualPaidAmount + discountAmount

                If (unPaidBillAmount < (actualPaidAmount + discountAmount)) Then
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
                query &= "Discount, ChequeNo, BankName, ChequeDate, Remarks, FinalPaidAmount) "
                query &= "VALUES (@CustNo, @BillNo, @UnPaidBilledAmount, @PaymentDate, @PaymentMode, @ActualPaidAmount, "
                query &= "@Discount, @ChequeNo, @BankName, @ChequeDate, @Remarks, @FinalPaidAmount); SELECT SCOPE_IDENTITY()"

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
                Dim custNo As Integer = cmbPaymentCustomerList.SelectedValue
                loadPaymentList(custNo)
                loadPaymentGrid(custNo)

            Catch sqlEx As SqlException
                MsgBox("Operation failed. DB error. Please try again or contact the software support if problem persists")
            Catch ex As Exception
                MessageBox.Show("Message to Agni User:   " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnPaymentCancelCreatePayment_Click(sender As Object, e As EventArgs) Handles btnPaymentCancelCreatePayment.ClickButtonArea
        If (cmbPaymentPaymentNoList.SelectedIndex = -1 Or cmbPaymentPaymentNoList.SelectedValue = -1) Then
            resetPaymentScreen()
            cmbPaymentPaymentNoList.Text = ""
        End If

        resetIndexOfComboBox(cmbPaymentPaymentNoList)

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


    Private Sub cmbPaymentPaymentNoList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPaymentPaymentNoList.SelectedIndexChanged

        If (cmbPaymentPaymentNoList.SelectedIndex = -1 Or cmbPaymentPaymentNoList.SelectedValue = -1) Then
            resetPaymentScreen()
            Return
        End If

        Dim paymentNo As Integer = cmbPaymentPaymentNoList.SelectedValue
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
            txtPaymentFinalPaidAmount.Text = dataRow.Item("FinalPaidAmount")
            txtPaymentChequeNo.Text = If(dataRow.Item("ChequeNo") Is DBNull.Value, String.Empty, dataRow.Item("ChequeNo"))
            txtPaymentBankName.Text = If(dataRow.Item("BankName") Is DBNull.Value, String.Empty, dataRow.Item("BankName"))
            dpPaymentChequeDate.Text = If(dataRow.Item("ChequeDate") Is DBNull.Value, String.Empty, dataRow.Item("ChequeDate"))
            txtPaymentNetBalance.Text = Math.Round(dataRow.Item("UnPaidBilledAmount") - (dataRow.Item("ActualPaidAmount") + dataRow.Item("Discount")))
            txtPaymentRemarks.Text = dataRow.Item("Remarks")
        Else
            MessageBox.Show("No data found for payment: " + paymentNo.ToString)
        End If
    End Sub

    Private Sub DateTimePicker6_CloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles dpPaymentDate.CloseUp

    End Sub


    Sub reduceBillPaidAmount(billNo As Decimal, amountToBeDeducted As Decimal)

        Try
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

        Catch sqlEx As SqlException
            MsgBox("Operation failed. DB error. Please try again or contact the software support if problem persists")
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message)
        End Try

    End Sub

    Private Sub btnPaymentDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPaymentDelete.ClickButtonArea
        If cmbPaymentPaymentNoList.SelectedIndex = -1 Or cmbPaymentPaymentNoList.SelectedValue = -1 Then
            MessageBox.Show("select the payment which you want to delete")
            cmbPaymentPaymentNoList.Focus()
        Else
            Dim custNo As Integer = cmbPaymentCustomerList.SelectedValue
            Dim selectedPaymentNo As Integer = cmbPaymentPaymentNoList.SelectedValue

            Dim lastPaymentRow As DataRow = getLastPaymentRow(custNo)

            If (lastPaymentRow IsNot Nothing AndAlso selectedPaymentNo <> lastPaymentRow.Item("PaymentNo")) Then
                MessageBox.Show("This is not the last payment. You can only delete the last payment")
                Return
            End If

            If MessageBox.Show("Are you sure you want to delete this Payment? Please note that this action cannot be undone.", "WARNING", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Dim actionResult As DialogResult = ActionConfirmation.ShowDialog()
                ActionConfirmation.Dispose()

                If actionResult = System.Windows.Forms.DialogResult.Cancel Then
                    'No need to do anything as this result you get is when the user is canceled the Action Dialog
                ElseIf actionResult = System.Windows.Forms.DialogResult.Yes Then
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
                ElseIf actionResult = System.Windows.Forms.DialogResult.No Then
                    MsgBox("You do not have permission for this operation. Please try with Administrator user when prompted for confirmation")
                End If

            End If

        End If
    End Sub

    Private Sub tabAllTabsHolder_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabAllTabsHolder.SelectedIndexChanged
        resetGlobalVairables()

        Dim selectedTab As TabPage = tabAllTabsHolder.SelectedTab
        Dim selectedTabTag As String = selectedTab.Tag

        If selectedTabTag.Equals("tagReportsTab") Then
            Me.AcceptButton = btnReportSearch
            cmbReportCustomerList.Focus()
            Dim designReportTable As DataTable = dgReportDesignGrid.DataSource
            If (designReportTable IsNot Nothing) Then
                designReportTable.Rows.Clear()
            End If
            Dim billReportTable As DataTable = dgReportBillGrid.DataSource
            If (billReportTable IsNot Nothing) Then
                billReportTable.Rows.Clear()
            End If
            Dim paymentReportTable As DataTable = dgReportPaymentGrid.DataSource
            If (paymentReportTable IsNot Nothing) Then
                paymentReportTable.Rows.Clear()
            End If
        ElseIf selectedTabTag.Equals("tagCustomerTab") Then
            Me.AcceptButton = btnCustAdd
            cmbCustCustomerList.Focus()
            'Dim customerTable As DataTable = dgCustCustomerDetails.DataSource
            'If (customerTable IsNot Nothing) Then
            '    customerTable.Rows.Clear()
            'End If
        ElseIf selectedTabTag.Equals("tagDesignTab") Then
            Me.AcceptButton = btnDesAdd
            cmbDesCustomerList.Focus()
            resetIndexOfComboBox(cmbDesCustomerList)
            Dim designTable As DataTable = dgDesDesignDetails.DataSource
            If (designTable IsNot Nothing) Then
                designTable.Rows.Clear()
            End If
        ElseIf selectedTabTag.Equals("tagBillingTab") Then
            If btnBillingCreateBill.Visible = True Then
                Me.AcceptButton = btnBillingCreateBill
            Else
                Me.AcceptButton = btnBillingConfirmCreateBill
            End If
            cmbBillingCustomerList.Focus()
            resetIndexOfComboBox(cmbBillingCustomerList)
            Dim billingTable As DataTable = dgBIllingBillDetails.DataSource
            If (billingTable IsNot Nothing) Then
                billingTable.Rows.Clear()
            End If
        ElseIf selectedTabTag.Equals("tagPaymentTab") Then
            If btnPaymentCreatePayment.Visible = True Then
                Me.AcceptButton = btnPaymentCreatePayment
            Else
                Me.AcceptButton = btnPaymentConfirmCreatePayment
            End If
            cmbPaymentCustomerList.Focus()
            resetIndexOfComboBox(cmbPaymentCustomerList)
            Dim paymentTable As DataTable = dgPaymentDetails.DataSource
            If (paymentTable IsNot Nothing) Then
                paymentTable.Rows.Clear()
            End If
        End If
    End Sub

    Sub resetIndexOfComboBox(comboBox As ElaCustomComboBoxControl.ElaCustomComboBox)
        If (comboBox.Items.Count > 0) Then
            comboBox.SelectedValue = -1
        Else
            comboBox.SelectedIndex = -1
        End If
    End Sub

    Private Sub btnPaymentClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPaymentClear.ClickButtonArea
        resetIndexOfComboBox(cmbPaymentPaymentNoList)
    End Sub

    Dim gLastSearchByBillNoCheckedValue As Boolean = False
    Dim gLastSearchByCustomerCheckedValue As Boolean = False


    Private Sub cbReportSearchByFilterChanged(sender As Object, e As EventArgs) Handles cbReportSearchByCustomer.CheckedChanged,
                                                    cbReportSearchByDesignSelection.CheckedChanged,
                                                    cbReportSearchByDesignNo.CheckedChanged,
                                                    cbReportSearchByBillNo.CheckedChanged,
                                                    cbReportSearchByDateRange.CheckedChanged

        If gFormLoadCompleted = False Then
            Return
        End If

        If sender Is cbReportSearchByDesignSelection And cbReportSearchByDesignSelection.Checked = True Then
            cbReportSearchByDesignNo.Checked = False
        ElseIf sender Is cbReportSearchByDesignNo And cbReportSearchByDesignNo.Checked = True Then
            cbReportSearchByDesignSelection.Checked = False
        End If

        groupReportCustomerName.Visible = False
        groupReportDesignList.Visible = False
        groupReportDesignName.Visible = False
        groupReportBillNo.Visible = False
        groupReportDateRange.Visible = False

        Dim searchFilter As Integer = getSearchFilter()

        Dim custNo As Integer = Nothing
        Dim billNo As Integer = Nothing

        Dim customerColumnVisibility As Boolean = False
        If (searchFilter And SEARCH_BY_CUSTOMER) <> 0 Then
            custNo = cmbReportCustomerList.SelectedValue
            customerColumnVisibility = False
        Else
            customerColumnVisibility = True
        End If

        dgReportDesignGrid.Columns("CustName").Visible = customerColumnVisibility
        dgReportBillGrid.Columns("ReportBillCustName").Visible = customerColumnVisibility
        dgReportPaymentGrid.Columns("ReportPaymentCustName").Visible = customerColumnVisibility

        If (searchFilter And SEARCH_BY_BILL_NO) <> 0 Then
            billNo = cmbReportBillNoList.SelectedValue
        End If

        If (gLastSearchByCustomerCheckedValue <> cbReportSearchByCustomer.Checked) OrElse ((searchFilter Or SEARCH_BY_BILL_NO) = SEARCH_BY_BILL_NO) Then
            loadBillList(custNo, cmbReportBillNoList)
            If (searchFilter And SEARCH_BY_BILL_NO) <> 0 Then
                billNo = -1
            End If
        End If

        If (gLastSearchByCustomerCheckedValue <> cbReportSearchByCustomer.Checked) OrElse
                                    (gLastSearchByBillNoCheckedValue <> cbReportSearchByBillNo.Checked) OrElse
                                    ((searchFilter Or SEARCH_BY_DESIGN_SELECTION) = SEARCH_BY_DESIGN_SELECTION) Then
            loadDesignList(custNo, cmbReportDesignNoList, billNo)
        End If

        Dim placeHolderIndex As Integer = 0

        If (searchFilter And SEARCH_BY_CUSTOMER) <> 0 Then
            groupReportCustomerName.Visible = True
            groupReportCustomerName.Location = reportControlsPlaceHolders(placeHolderIndex).Location
            placeHolderIndex += 1
        End If

        If (searchFilter And SEARCH_BY_BILL_NO) <> 0 Then
            groupReportBillNo.Visible = True
            groupReportBillNo.Location = reportControlsPlaceHolders(placeHolderIndex).Location
            placeHolderIndex += 1
        End If

        If (searchFilter And SEARCH_BY_DESIGN_SELECTION) <> 0 Then
            groupReportDesignList.Visible = True
            groupReportDesignList.Location = reportControlsPlaceHolders(placeHolderIndex).Location
            placeHolderIndex += 1
        End If

        If (searchFilter And SEARCH_BY_DESIGN_NO) <> 0 Then
            groupReportDesignName.Visible = True
            groupReportDesignName.Location = reportControlsPlaceHolders(placeHolderIndex).Location
            placeHolderIndex += 1
        End If

        If (searchFilter And SEARCH_BY_DATE_RANGE) <> 0 Then
            groupReportDateRange.Visible = True
            groupReportDateRange.Location = reportControlsPlaceHolders(placeHolderIndex).Location
            placeHolderIndex += 1
        End If

        panelReportButtons.Location = New Point(reportControlsPlaceHolders(placeHolderIndex).Location.X + 40, reportControlsPlaceHolders(placeHolderIndex).Location.Y + 20)

        If sender Is cbReportSearchByCustomer Then
            gLastSearchByCustomerCheckedValue = cbReportSearchByCustomer.Checked
        ElseIf sender Is cbReportSearchByBillNo Then
            gLastSearchByBillNoCheckedValue = cbReportSearchByBillNo.Checked
        End If



    End Sub

    Private Sub cmbReportDesignList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        pbReportDesignImage.Image = Nothing
    End Sub

    Function getSearchFilter() As Integer

        Dim searchFilter As Integer = 0

        If cbReportSearchByCustomer.Checked = True Then
            searchFilter = searchFilter Or SEARCH_BY_CUSTOMER
        End If

        If cbReportSearchByBillNo.Checked = True Then
            searchFilter = searchFilter Or SEARCH_BY_BILL_NO
        End If

        If cbReportSearchByDesignSelection.Checked = True Then
            searchFilter = searchFilter Or SEARCH_BY_DESIGN_SELECTION
        End If

        If cbReportSearchByDesignNo.Checked = True Then
            searchFilter = searchFilter Or SEARCH_BY_DESIGN_NO
        End If

        If cbReportSearchByDateRange.Checked = True Then
            searchFilter = searchFilter Or SEARCH_BY_DATE_RANGE
        End If

        Return searchFilter
    End Function

    Function validateSearchEntries() As Boolean
        If groupReportCustomerName.Visible = True Then
            If cmbReportCustomerList.SelectedIndex = -1 Or cmbReportCustomerList.SelectedValue = -1 Then
                MsgBox("Please select a customer or Remove the customer filter")
                cmbReportCustomerList.Focus()
                Return False
            End If
        End If
        If groupReportBillNo.Visible = True Then
            If cmbReportBillNoList.SelectedIndex = -1 Or cmbReportBillNoList.SelectedValue = -1 Then
                MsgBox("Please select a bill number or Remove the bill number filter")
                cmbReportBillNoList.Focus()
                Return False
            End If
        End If
        If groupReportDesignList.Visible = True Then
            If cmbReportDesignNoList.SelectedIndex = -1 Or cmbReportDesignNoList.SelectedValue = -1 Then
                MsgBox("Please select a design or Remove the select design filter")
                cmbReportDesignNoList.Focus()
                Return False
            End If
        End If
        If groupReportDesignName.Visible = True Then
            If txtReportDesignNumber.Text.Trim = String.Empty Then
                MsgBox("Please enter the design name or Remove the enter design name filter")
                txtReportDesignNumber.Focus()
                Return False
            End If
        End If

        Return True
    End Function

    Function getDesignSearchQuery(searchData As SearchData, Optional isSummary As Boolean = False) As String

        Dim custNo As Integer = searchData.custNo
        Dim billNo As Integer = searchData.billNo
        Dim designNo As Integer = searchData.designNo
        Dim designName As String = searchData.designName
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate

        Dim designQuery As String

        If isSummary = False Then
            designQuery = "Select c.CompName, d.DesignNo, d.DesignDate, d.DesignName, d.Type,  d.Width, d.Height, d.Colors, d.UnitCost, d.Price, d.Billed, d.BillNo, d.Image, d.CustNo From design d, customer c"
        Else
            designQuery = "select count(1) as designCount, isnull(sum(CASE WHEN d.Billed = 1 THEN d.Price ELSE 0 END),0) AS billedDesignAmount, isnull(sum(CASE WHEN d.Billed = 0 THEN d.Price ELSE 0 END),0) AS unbilledDesignAmount, isnull(sum(d.Price),0) as TotalDesignAmount from design d,customer c"
        End If

        Dim designQueryWhereClause As String = String.Empty

        If custNo <> Nothing Then
            designQueryWhereClause += " d.CustNo=" + custNo.ToString
        End If

        If billNo <> Nothing Then
            If designQueryWhereClause IsNot String.Empty Then
                designQueryWhereClause += " and "
            End If
            designQueryWhereClause += " d.BillNo=" + billNo.ToString
        End If

        If designNo <> Nothing Then
            If designQueryWhereClause IsNot String.Empty Then
                designQueryWhereClause += " and "
            End If
            designQueryWhereClause += " d.DesignNo=" + designNo.ToString
        End If

        If designName <> Nothing Then
            If designQueryWhereClause IsNot String.Empty Then
                designQueryWhereClause += " and "
            End If
            designQueryWhereClause += " d.DesignName Like '%" + designName + "%'"
        End If

        If fromDate <> Nothing Or toDate <> Nothing Then
            If designQueryWhereClause IsNot String.Empty Then
                designQueryWhereClause += " and "
            End If
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            designQueryWhereClause += " cast(d.designDate as date)>='" + fromDateStr + "' and cast(d.designDate as date)<='" + toDateStr + "'"
        End If

        If designQueryWhereClause IsNot String.Empty Then
            designQueryWhereClause += " and "
        End If
        designQueryWhereClause += " d.CustNo = c.CustNo"

        If designQueryWhereClause IsNot String.Empty Then
            designQuery += " where " + designQueryWhereClause
        End If


        Return designQuery
    End Function

    Function getBillSearchQuery(searchData As SearchData, Optional isSummary As Boolean = False) As String

        Dim custNo As Integer = searchData.custNo
        Dim billNo As Integer = searchData.billNo
        Dim designNo As Integer = searchData.designNo
        Dim designName As String = searchData.designName
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate

        Dim billQuery As String

        If isSummary = False Then
            billQuery = "select c.CompName as CustomerName, c.GSTIN as GSTNo, b.BillNo, b.DisplayBillNo, b.BillDate, dbo.BankersRound(b.DesignCost,0) as DesignCost, dbo.BankersRound(b.UnPaidAmountTillNow,0) as UnPaidAmountTillNow, b.CGST, b.CGST*b.DesignCost/100 as CGSTAmount, 
                            b.SGST, b.SGST*b.DesignCost/100 as SGSTAmount, b.IGST, b.IGST*b.DesignCost/100 as IGSTAmount, dbo.BankersRound((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100,0) as GSTAmount, dbo.BankersRound(((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100),0)+dbo.BankersRound(b.DesignCost,0) as BillAmount, b.PaidAmount, 
                            dbo.BankersRound(((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100),0)+dbo.BankersRound(b.DesignCost,0)+dbo.BankersRound(b.UnPaidAmountTillNow,0) as TotalAmount, 
                            (dbo.BankersRound(((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100),0)+dbo.BankersRound(b.DesignCost,0)+dbo.BankersRound(b.UnPaidAmountTillNow,0))-dbo.BankersRound(b.PaidAmount,0) as RemainingBalance, b.Cancelled from bill b, customer c"
        Else
            billQuery = "select count(1) as BillsCount, sum(isnull(dbo.BankersRound(b.DesignCost,0),0)) as TotalDesignCost, sum(isnull(dbo.BankersRound(((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100),0)+dbo.BankersRound(b.DesignCost,0),0)) as TotalBillAmount, 
                            sum(isnull(dbo.BankersRound((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100,0),0)) as TotalGSTAmount, sum(isnull(b.PaidAmount,0)) as TotalPaidAmount,
                            sum(isnull(dbo.BankersRound(((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100),0)+dbo.BankersRound(b.DesignCost,0),0)) - sum(isnull(b.PaidAmount,0)) as TotalNetBalance from bill b, customer c"
        End If

        Dim billQueryWhereClause As String = String.Empty

        If custNo <> Nothing Then
            billQueryWhereClause += " b.CustNo=" + custNo.ToString
        End If

        If billNo <> Nothing Then
            If billQueryWhereClause IsNot String.Empty Then
                billQueryWhereClause += " and "
            End If
            billQueryWhereClause += " b.BillNo=" + billNo.ToString
        End If

        If designNo <> Nothing Then
            If billQueryWhereClause IsNot String.Empty Then
                billQueryWhereClause += " and "
            End If
            billQueryWhereClause += " b.billNo in (select BillNo from Design where designNo = " + designNo.ToString + ")"
        End If

        If designName <> Nothing Then
            If billQueryWhereClause IsNot String.Empty Then
                billQueryWhereClause += " and "
            End If
            billQueryWhereClause += " b.billNo in (select BillNo from Design where DesignName like '%" + designName + "%')"
        End If

        If fromDate <> Nothing Or toDate <> Nothing Then
            If billQueryWhereClause IsNot String.Empty Then
                billQueryWhereClause += " And "
            End If
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            billQueryWhereClause += " cast(b.BillDate as date)>='" + fromDateStr + "' and  cast(b.BillDate as date)<='" + toDateStr + "'"
        End If

        If billQueryWhereClause IsNot String.Empty Then
            billQueryWhereClause += " And "
        End If
        billQueryWhereClause += " b.CustNo = c.CustNo"

        If billQueryWhereClause IsNot String.Empty Then
            billQuery += " where " + billQueryWhereClause
        End If

        Return billQuery
    End Function

    Function getPaymentSearchQuery(searchData As SearchData, Optional isSummary As Boolean = False) As String

        Dim custNo As Integer = searchData.custNo
        Dim billNo As Integer = searchData.billNo
        Dim designNo As Integer = searchData.designNo
        Dim designName As String = searchData.designName
        Dim fromDate As Date = searchData.fromDate
        Dim toDate As Date = searchData.toDate

        Dim paymentQuery As String

        If isSummary = False Then
            paymentQuery = "select c.CompName as CustomerName, b.DisplayBillNo, 
                            b.BillDate, dbo.BankersRound(((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100),0)+dbo.BankersRound(b.DesignCost,0) as BillAmount,
                            p.UnPaidBilledAmount as NetBalanceBeforePayment,
                            p.UnPaidBilledAmount - p.FinalPaidAmount as NetBalanceAfterPayment, p.*
                            from payment p, bill b, customer c"
        Else
            paymentQuery = "select count(1) as PaymentCount, sum(isnull(dbo.BankersRound(((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100),0)+dbo.BankersRound(b.DesignCost,0),0)) as TotalBillAmount, 
                            isnull(sum(p.ActualPaidAmount),0) as TotPaidAmountActual, sum(isnull(p.FinalPaidAmount,0)) as TotalFinalPaidAmount, 
                            sum(isnull(dbo.BankersRound(((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100),0)+dbo.BankersRound(b.DesignCost,0),0)) - sum(isnull(p.FinalPaidAmount,0)) as TotalNetBalance from payment p, bill b, customer c"
        End If

        Dim paymentQueryWhereClause As String = String.Empty

        If custNo <> Nothing Then
            paymentQueryWhereClause += " p.CustNo=" + custNo.ToString
        End If

        If billNo <> Nothing Then
            If paymentQueryWhereClause IsNot String.Empty Then
                paymentQueryWhereClause += " and "
            End If
            paymentQueryWhereClause += " p.BillNo=" + billNo.ToString
        End If

        If designNo <> Nothing Then
            If paymentQueryWhereClause IsNot String.Empty Then
                paymentQueryWhereClause += " and "
            End If
            paymentQueryWhereClause += " p.BillNo in (select BillNo from Design where designNo = " + designNo.ToString + ")"
        End If

        If designName <> Nothing Then
            If paymentQueryWhereClause IsNot String.Empty Then
                paymentQueryWhereClause += " and "
            End If
            paymentQueryWhereClause += " p.BillNo in (select BillNo from Design where DesignName like '%" + designName + "%')"
        End If

        If fromDate <> Nothing Or toDate <> Nothing Then
            If paymentQueryWhereClause IsNot String.Empty Then
                paymentQueryWhereClause += " And "
            End If
            Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
            Dim toDateStr As String = toDate.ToString("yyyyMMdd")
            paymentQueryWhereClause += " cast(p.PaymentDate as date)>='" + fromDateStr + "' and cast(p.PaymentDate as date)<='" + toDateStr + "'"
        End If

        If paymentQueryWhereClause IsNot String.Empty Then
            paymentQueryWhereClause += " And "
        End If
        paymentQueryWhereClause += " p.CustNo = c.CustNo and b.CustNo = c.CustNo and p.BillNo = b.BillNo"

        If paymentQueryWhereClause IsNot String.Empty Then
            paymentQuery += " where " + paymentQueryWhereClause
        End If

        Return paymentQuery
    End Function

    Function getBillAndPaymentHistoryQuery(searchData As SearchData, billSearchQuery As String, paymentSearchQuery As String) As String

        Dim billAndPaymentHistoryQuery As String = ""

        If (gSearchFilter And SEARCH_BY_CUSTOMER) = 0 Then

            billAndPaymentHistoryQuery = "Select BR.CustomerName, BR.DisplayBillNo, BR.BillDate, PR.PaymentDate, 
            BR.BillAmount, isnull(PR.NetBalanceBeforePayment, Br.BillAmount + Br.UnPaidAmountTillNow) As NetBalanceBeforePayment, 
            isnull(PR.FinalPaidAmount,0) as FinalPaidAmount, isnull(PR.NetBalanceAfterPayment, Br.BillAmount + Br.UnPaidAmountTillNow) As NetBalanceAfterPayment from
            (" + billSearchQuery + ") As BR 
            Left Join
            (" + paymentSearchQuery + ") as PR 
            On BR.BillNo = PR.BillNo"

        Else

            Dim custNo As Integer = searchData.custNo
            Dim billNo As Integer = searchData.billNo
            Dim designNo As Integer = searchData.designNo
            Dim designName As String = searchData.designName
            Dim fromDate As Date = searchData.fromDate
            Dim toDate As Date = searchData.toDate

            Dim billQuery As String

            billQuery = "select c.CompName as CustomerName, b.BillDate as Date, b.BillNo, b.DisplayBillNo, dbo.BankersRound(((b.CGST+ b.SGST+ b.IGST)*b.DesignCost/100),0)+dbo.BankersRound(b.DesignCost,0) as BillAmount,
			NULL as FinalPaidAmount, NULL as BalanceBeforePayment, NULL as BalanceAfterPayment, b.UnPaidAmountTillNow from bill b, customer c "

            Dim billQueryWhereClause As String = String.Empty

            If custNo <> Nothing Then
                billQueryWhereClause += " b.CustNo=" + custNo.ToString
            End If

            If billNo <> Nothing Then
                If billQueryWhereClause IsNot String.Empty Then
                    billQueryWhereClause += " and "
                End If
                billQueryWhereClause += " b.BillNo=" + billNo.ToString
            End If

            If designNo <> Nothing Then
                If billQueryWhereClause IsNot String.Empty Then
                    billQueryWhereClause += " and "
                End If
                billQueryWhereClause += " b.billNo in (select BillNo from Design where designNo = " + designNo.ToString + ")"
            End If

            If designName <> Nothing Then
                If billQueryWhereClause IsNot String.Empty Then
                    billQueryWhereClause += " and "
                End If
                billQueryWhereClause += " b.billNo in (select BillNo from Design where DesignName like '%" + designName + "%')"
            End If

            If fromDate <> Nothing Or toDate <> Nothing Then
                If billQueryWhereClause IsNot String.Empty Then
                    billQueryWhereClause += " And "
                End If
                Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
                Dim toDateStr As String = toDate.ToString("yyyyMMdd")
                billQueryWhereClause += " cast(b.BillDate as date)>='" + fromDateStr + "' and  cast(b.BillDate as date)<='" + toDateStr + "'"
            End If

            If billQueryWhereClause IsNot String.Empty Then
                billQueryWhereClause += " And "
            End If
            billQueryWhereClause += " b.CustNo = c.CustNo"

            If billQueryWhereClause IsNot String.Empty Then
                billQuery += " where " + billQueryWhereClause
            End If


            Dim paymentQuery As String

            paymentQuery = "select c.CompName as CustomerName, p.PaymentDate as Date, NULL as BillNo, NULL as DisplayBillNo, NULL as BillAmount, 
		p.FinalPaidAmount, p.UnPaidBilledAmount as BalanceBeforePayment, p.UnPaidBilledAmount - p.FinalPaidAmount as BalanceAfterPayment, NULL as UnPaidAmountTillNow
		from payment p, bill b, customer c "

            Dim paymentQueryWhereClause As String = String.Empty

            If custNo <> Nothing Then
                paymentQueryWhereClause += " p.CustNo=" + custNo.ToString
            End If

            If billNo <> Nothing Then
                If paymentQueryWhereClause IsNot String.Empty Then
                    paymentQueryWhereClause += " and "
                End If
                paymentQueryWhereClause += " p.BillNo=" + billNo.ToString
            End If

            If designNo <> Nothing Then
                If paymentQueryWhereClause IsNot String.Empty Then
                    paymentQueryWhereClause += " and "
                End If
                paymentQueryWhereClause += " p.BillNo in (select BillNo from Design where designNo = " + designNo.ToString + ")"
            End If

            If designName <> Nothing Then
                If paymentQueryWhereClause IsNot String.Empty Then
                    paymentQueryWhereClause += " and "
                End If
                paymentQueryWhereClause += " p.BillNo in (select BillNo from Design where DesignName like '%" + designName + "%')"
            End If

            If fromDate <> Nothing Or toDate <> Nothing Then
                If paymentQueryWhereClause IsNot String.Empty Then
                    paymentQueryWhereClause += " And "
                End If
                Dim fromDateStr As String = fromDate.ToString("yyyyMMdd")
                Dim toDateStr As String = toDate.ToString("yyyyMMdd")
                paymentQueryWhereClause += " cast(p.PaymentDate as date)>='" + fromDateStr + "' and cast(p.PaymentDate as date)<='" + toDateStr + "'"
            End If

            If paymentQueryWhereClause IsNot String.Empty Then
                paymentQueryWhereClause += " And "
            End If
            paymentQueryWhereClause += " p.CustNo = c.CustNo and b.CustNo = c.CustNo and p.BillNo = b.BillNo"

            If paymentQueryWhereClause IsNot String.Empty Then
                paymentQuery += " where " + paymentQueryWhereClause
            End If

            billAndPaymentHistoryQuery = "select CustomerName, Date, BillNo, DisplayBillNo, BillAmount, FinalPaidAmount, 
                isnull(BalanceBeforePayment, BillAmount + UnPaidAmountTillNow) As NetBalanceBeforePayment, isnull(BalanceAfterPayment, BillAmount + UnPaidAmountTillNow) As NetBalanceAfterPayment  from
                (
                (Select * from
                (" + billQuery + ") As BR )
                union
                (Select * from
                (" + paymentQuery + ") as PR)) as billAndPayments"
        End If

        'log.Debug("getBillAndPaymentHistoryQuery: billAndPaymentHistoryQuery: " + billAndPaymentHistoryQuery)

        Return billAndPaymentHistoryQuery

    End Function

    Private Sub btnReportSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReportSearch.ClickButtonArea
        pbReportDesignImage.Image = Nothing

        gSearchFilter = getSearchFilter()
        Dim designQuery As String = String.Empty
        Dim designSummaryQuery As String = String.Empty
        Dim billQueryWhereClause As String = String.Empty
        Dim paymentQueryWhereClause As String = String.Empty

        If (validateSearchEntries() = False) Then
            Return
        End If

        Dim searchData As SearchData = New SearchData
        'To use in Bill Search report
        gReportSearchFilterText = ""

        If (gSearchFilter And SEARCH_BY_CUSTOMER) <> 0 Then
            searchData.custNo = cmbReportCustomerList.SelectedValue
            gReportSearchFilterText += "Customer Name   : " + cmbReportCustomerList.Text.ToString
        End If

        If (gSearchFilter And SEARCH_BY_BILL_NO) <> 0 Then
            searchData.billNo = cmbReportBillNoList.SelectedValue
            If gReportSearchFilterText IsNot String.Empty Then
                gReportSearchFilterText += Chr(10) + Chr(13)
            End If
            gReportSearchFilterText += "Bill Number          : " + cmbReportBillNoList.Text.ToString
        End If

        If (gSearchFilter And SEARCH_BY_DESIGN_SELECTION) <> 0 Then
            searchData.designNo = cmbReportDesignNoList.SelectedValue
            If gReportSearchFilterText IsNot String.Empty Then
                gReportSearchFilterText += Chr(10) + Chr(13)
            End If
            gReportSearchFilterText += "Design Name      : " + cmbReportDesignNoList.Text.ToString
        End If

        If (gSearchFilter And SEARCH_BY_DESIGN_NO) <> 0 Then
            searchData.designNo = txtReportDesignNumber.Text
            If gReportSearchFilterText IsNot String.Empty Then
                gReportSearchFilterText += Chr(10) + Chr(13)
            End If
            gReportSearchFilterText += "Design Number       : " + searchData.designNo.ToString
        End If

        If (gSearchFilter And SEARCH_BY_DATE_RANGE) <> 0 Then
            searchData.fromDate = dpReportFromDate.Value
            searchData.toDate = dpReportToDate.Value
            gSearchFromDate = searchData.fromDate
            gSearchToDate = searchData.toDate
            If gReportSearchFilterText IsNot String.Empty Then
                gReportSearchFilterText += Chr(10) + Chr(13)
            End If
            gReportSearchFilterText += "From Date            : " + searchData.fromDate.ToString("dd/MM/yyyy")
            gReportSearchFilterText += Chr(10) + Chr(13) + "To Date                : " + searchData.toDate.ToString("dd/MM/yyyy")
        End If

        Dim designSearchQuery As String = getDesignSearchQuery(searchData)
        Dim designSummarySearchQuery As String = getDesignSearchQuery(searchData, True)
        searchData.designQuery = designSearchQuery
        searchData.designSummaryQuery = designSummarySearchQuery
        searchDesign(searchData)

        Dim billSearchQuery As String = getBillSearchQuery(searchData)
        Dim billSummarySearchQuery As String = getBillSearchQuery(searchData, True)
        searchData.billQuery = billSearchQuery
        searchData.billSummaryQuery = billSummarySearchQuery
        searchBill(searchData)

        Dim paymentSearchQuery As String = getPaymentSearchQuery(searchData)
        Dim paymentSummarySearchQuery As String = getPaymentSearchQuery(searchData, True)
        searchData.paymentQuery = paymentSearchQuery
        searchData.paymentSummaryQuery = paymentSummarySearchQuery
        searchPayment(searchData)

        Dim billAndPaymentHistoryQuery As String = getBillAndPaymentHistoryQuery(searchData, billSearchQuery, paymentSearchQuery)
        searchBillAndPaymentHistory(billAndPaymentHistoryQuery)

    End Sub

    Class SearchData

        Public custNo As Integer
        Public billNo As Integer
        Public designNo As Integer
        Public designName As String
        Public fromDate As Date
        Public toDate As Date
        Public comboBoxControl As ElaCustomComboBoxControl.ElaCustomComboBox
        Public searchFilter As Integer
        Public designQuery As String
        Public designSummaryQuery As String
        Public billQuery As String
        Public billSummaryQuery As String
        Public paymentQuery As String
        Public paymentSummaryQuery As String

        Sub New()
            custNo = Nothing
            billNo = Nothing
            designNo = Nothing
            designName = Nothing
            fromDate = Nothing
            toDate = Nothing
            comboBoxControl = Nothing
            designQuery = Nothing
            designSummaryQuery = Nothing
            searchFilter = 0
        End Sub

    End Class

    Sub searchDesign(searchData As SearchData)
        Dim thread As Thread = New Thread(AddressOf fetchDesignForReport)
        thread.IsBackground = True
        thread.Start(searchData)
    End Sub

    Sub fetchDesignForReport(ByVal searchDataParam As Object)
        Dim searchData As SearchData = CType(searchDataParam, SearchData)

        Dim designTable As DataTable = fetchDesignTableForReport(searchData.designQuery)
        Dim showDesignSearchResultInvoker As New showDesignSearchResultDelegate(AddressOf Me.showDesignSearchResult)
        Me.BeginInvoke(showDesignSearchResultInvoker, designTable)

        Dim designSummaryTable As DataTable = fetchDesignTableForReport(searchData.designSummaryQuery)
        Dim showDesignSummarySearchResultInvoker As New showDesignSummarySearchResultDelegate(AddressOf Me.showDesignSummarySearchResult)
        Me.BeginInvoke(showDesignSummarySearchResultInvoker, designSummaryTable)
    End Sub

    Function fetchDesignTableForReport(searchQuery As String) As DataTable
        Dim designQueryCommand As SqlCommand = New SqlCommand(searchQuery, dbConnection)

        'log.Debug("fetchDesignTableForReport: searchQuery: " + searchQuery)

        Dim designAdapter = New SqlDataAdapter()
        designAdapter.SelectCommand = designQueryCommand
        Dim designDataSet = New DataSet
        designAdapter.Fill(designDataSet, "design")
        Return designDataSet.Tables(0)

    End Function

    Delegate Sub showDesignSearchResultDelegate(designTable As DataTable)

    Sub showDesignSearchResult(designTable As DataTable)
        dgReportDesignGrid.BindingContext = New BindingContext
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

    Sub searchBill(searchData As SearchData)
        Dim thread As Thread = New Thread(AddressOf fetchBillForReport)
        thread.IsBackground = True
        thread.Start(searchData)
    End Sub

    Sub fetchBillForReport(ByVal searchDataParam As Object)
        Dim searchData As SearchData = CType(searchDataParam, SearchData)
        Dim billTable As DataTable = executeQueryAndReturnTable(searchData.billQuery, "BillSearchResult")
        Dim showBillSearchResultInvoker As New showBillSearchResultDelegate(AddressOf Me.showBillSearchResult)
        Me.BeginInvoke(showBillSearchResultInvoker, billTable)

        Dim billsummarytable As DataTable = executeQueryAndReturnTable(searchData.billSummaryQuery, "TotalBillSearchResult")
        Dim showbillsummarysearchresultinvoker As New showBillSummarySearchResultDelegate(AddressOf Me.showBillSummarySearchResult)
        Me.BeginInvoke(showbillsummarysearchresultinvoker, billsummarytable)

        If gBillSearchResultDataSet IsNot Nothing Then
            gBillSearchResultDataSet.Dispose()
        End If
        gBillSearchResultDataSet = New DataSet
        gBillSearchResultDataSet.Tables.Add(billTable.Copy)
        gBillSearchResultDataSet.Tables.Add(billsummarytable.Copy)

    End Sub

    Delegate Sub showBillSearchResultDelegate(billTable As DataTable)

    Sub showBillSearchResult(billTable As DataTable)
        dgReportBillGrid.DataSource = billTable
    End Sub

    Delegate Sub showBillSummarySearchResultDelegate(billSummaryTable As DataTable)

    Sub showBillSummarySearchResult(billSummaryTable As DataTable)
        'log.Debug("showBillSummarySearchResult called")
        If billSummaryTable.Rows.Count = 0 Then
            Return
        End If

        Dim dataRow As DataRow = billSummaryTable.Rows(0)
        lblReportNoOfBills.Text = dataRow.Item("BillsCount")

        lblReportBillBilledAmount.Text = If(IsDBNull(dataRow.Item("TotalBillAmount")), "0", Format(dataRow.Item("TotalBillAmount"), "0.00"))
        lblReportBIllPaidAmount.Text = If(IsDBNull(dataRow.Item("TotalPaidAmount")), "0", Format(dataRow.Item("TotalPaidAmount"), "0.00"))
        lblReportBillNetBalance.Text = If(IsDBNull(dataRow.Item("TotalNetBalance")), "0", Format(dataRow.Item("TotalNetBalance"), "0.00"))
    End Sub

    Sub searchPayment(searchData As SearchData)
        Dim thread As Thread = New Thread(AddressOf fetchPaymentForReport)
        thread.IsBackground = True
        thread.Start(searchData)
    End Sub

    Sub fetchPaymentForReport(ByVal searchDataParam As Object)
        Dim searchData As SearchData = CType(searchDataParam, SearchData)

        Dim paymentTable As DataTable = executeQueryAndReturnTable(searchData.paymentQuery, "PaymentSearchResult")
        Dim showPaymentSearchResultInvoker As New showPaymentSearchResultDelegate(AddressOf Me.showPaymentSearchResult)
        Me.BeginInvoke(showPaymentSearchResultInvoker, paymentTable)

        Dim paymentSummaryTable As DataTable = executeQueryAndReturnTable(searchData.paymentSummaryQuery, "TotalPaymentSearchResult")
        Dim showPaymentSummarySearchResultInvoker As New showPaymentSummarySearchResultDelegate(AddressOf Me.showPaymentSummarySearchResult)
        Me.BeginInvoke(showPaymentSummarySearchResultInvoker, paymentSummaryTable)

        If gPaymentSearchResultDataSet IsNot Nothing Then
            gPaymentSearchResultDataSet.Dispose()
        End If
        gPaymentSearchResultDataSet = New DataSet
        gPaymentSearchResultDataSet.Tables.Add(paymentTable.Copy)
        gPaymentSearchResultDataSet.Tables.Add(paymentSummaryTable.Copy)
    End Sub

    Delegate Sub showPaymentSearchResultDelegate(paymentTable As DataTable)

    Sub showPaymentSearchResult(paymentTable As DataTable)
        dgReportPaymentGrid.DataSource = paymentTable
    End Sub

    Delegate Sub showPaymentSummarySearchResultDelegate(billSummaryTable As DataTable)

    Sub showPaymentSummarySearchResult(paymentSummaryTable As DataTable)
        If paymentSummaryTable.Rows.Count = 0 Then
            Return
        End If

        Dim dataRow As DataRow = paymentSummaryTable.Rows(0)
        lblReportNoOfPayment.Text = dataRow.Item("PaymentCount")
        lblReportPaidAmountActual.Text = If(IsDBNull(dataRow.Item("TotPaidAmountActual")), "0", Format(dataRow.Item("TotPaidAmountActual"), "0.00"))
        lblReportPaidAmountWithDeduction.Text = If(IsDBNull(dataRow.Item("TotalFInalPaidAmount")), "0", Format(dataRow.Item("TotalFInalPaidAmount"), "0.00"))
    End Sub

    Sub searchBillAndPaymentHistory(billAndPaymentHistoryQuery As String)
        Dim thread As Thread = New Thread(AddressOf fetchBillAndPaymentHistoryForReport)
        thread.IsBackground = True
        thread.Start(billAndPaymentHistoryQuery)
    End Sub

    Sub fetchBillAndPaymentHistoryForReport(ByVal billAndPaymentHistoryQuery As String)
        Dim resultTableName As String = "BillAndPaymentHistory"

        If (gSearchFilter And SEARCH_BY_CUSTOMER) <> 0 Then
            resultTableName = "BillAndPaymentHistoryPerCustomer"
        End If

        Dim billAndPaymentHistoryTable As DataTable = executeQueryAndReturnTable(billAndPaymentHistoryQuery, resultTableName)

        If gBillAndPaymentHistoryDataSet IsNot Nothing Then
            gBillAndPaymentHistoryDataSet.Dispose()
        End If
        gBillAndPaymentHistoryDataSet = New DataSet
        gBillAndPaymentHistoryDataSet.Tables.Add(billAndPaymentHistoryTable.Copy)
    End Sub

    Function executeQueryAndReturnTable(searchQuery As String, resultTableName As String) As DataTable
        log.Debug("executeQueryAndReturnTable: resultTableName:" + resultTableName + ", searchQuery:  " + searchQuery)
        Dim sqlQueryCommand As SqlCommand = New SqlCommand(searchQuery, dbConnection)
        Dim sqlTableAdapter = New SqlDataAdapter()
        sqlTableAdapter.SelectCommand = sqlQueryCommand
        Dim dataSet = New DataSet
        sqlTableAdapter.Fill(dataSet, resultTableName)
        Return dataSet.Tables(0)
    End Function

    Private Sub cmbDesDesignList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDesDesignList.SelectedIndexChanged

        'log.Debug("cmbDesDesignList_SelectedIndexChanged: " + cmbDesDesignList.SelectedIndex.ToString)

        If (cmbDesDesignList.SelectedIndex = -1 Or cmbDesDesignList.SelectedValue = -1) Then
            resetDesignScreen()
            gSelectedDesignNo = -1
            Return
        End If

        Dim designNo As Integer = cmbDesDesignList.SelectedValue
        gSelectedDesignNo = designNo

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
                pbDesDesignImage.Image = New Bitmap(designImageBuffer)
                'Image.FromStream(designImageBuffer)
            End If
            txtDesCalculatedPrice.Text = dataRow.Item("Price")
            dpDesDesignDate.Text = dataRow.Item("DesignDate")
        Else
            MessageBox.Show("No data found for design: " + cmbDesDesignList.Text)
        End If
    End Sub

    Private Sub dgDesDesignDetails_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgDesDesignDetails.RowEnter
        'Dim designNo As Integer = dgDesDesignDetails.Item("designNo", e.RowIndex).Value
        'cmbDesDesignList.SelectedValue = designNo

        'dgDesDesignDetails.Item("Image", e.RowIndex).ToolTipText = ToolTip1.ToString
    End Sub

    Dim imagePopup As ImagePopup = New ImagePopup

    Private Sub dgDesDesignDetails_CellMouseEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgDesDesignDetails.CellMouseEnter
        'Dim designNo As Integer = dgDesDesignDetails.Item("designNo", e.RowIndex).Value

        Dim rowIndex As Integer = e.RowIndex
        Dim columnIndex As Integer = e.ColumnIndex
        Dim columnName As String = dgDesDesignDetails.Columns(columnIndex).Name

        If dgDesDesignDetails.RowCount > 0 And rowIndex >= 0 And columnIndex < dgDesDesignDetails.ColumnCount And columnName.Equals("Image") Then

            Dim designImage() As Byte = CType(dgDesDesignDetails.Item("Image", rowIndex).Value, Byte())
            Dim designImageBuffer As New MemoryStream(designImage)
            Dim imageToShow As Image = New Bitmap(designImageBuffer)

            imagePopup.Show()
            imagePopup.Width = 900
            imagePopup.Height = 800

            imagePopup.loadImage(imageToShow)
        End If


    End Sub

    Private Sub dgDesDesignDetails_CellMouseLeave(sender As Object, e As DataGridViewCellEventArgs) Handles dgDesDesignDetails.CellMouseLeave
        imagePopup.Hide()
    End Sub

    Private Sub pbDesDesignImage_MouseEnter(sender As Object, e As EventArgs) Handles pbDesDesignImage.MouseEnter

        If pbDesDesignImage.Image IsNot Nothing Then

            Dim imageToShow As Image = pbDesDesignImage.Image

            imagePopup.Show()
            imagePopup.Width = 900
            imagePopup.Height = 800

            imagePopup.Left = 0

            imagePopup.loadImage(imageToShow)
        End If
    End Sub

    Private Sub pbDesDesignImage_MouseLeave(sender As Object, e As EventArgs) Handles pbDesDesignImage.MouseLeave
        imagePopup.Hide()
    End Sub


    Private Sub dgBIllingBillDetails_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgBIllingBillDetails.RowEnter

        'If btnBillingConfirmCreateBill.Visible = True Then
        '    MsgBox("You are in the middle of creating a Bill. You need to either complete the Bill creation or exit the Bill Creation to view any other Bills")
        '    Return
        'End If

        'Dim billNo As Integer = dgBIllingBillDetails.Item("InternalBillNo", e.RowIndex).Value
        'cmbBillingBillNoList.SelectedValue = billNo

    End Sub

    Private Sub dgPaymentDetails_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgPaymentDetails.RowEnter

        'If btnPaymentConfirmCreatePayment.Visible = True Then
        '    MsgBox("You are in the middle of creating a Payment. You need to either complete the payment creation or exit the create payment to view any other payment")
        '    Return
        'End If

        'Dim paymentNo As Integer = dgPaymentDetails.Item("PaymentNo", e.RowIndex).Value
        'cmbPaymentPaymentNoList.SelectedValue = paymentNo


    End Sub

    Private Sub dgReportDesignGrid_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgReportDesignGrid.RowEnter

        Dim rowIndex As Integer = e.RowIndex

        If dgReportDesignGrid.RowCount > 0 And rowIndex >= 0 And dgReportDesignGrid.Columns.Contains("ReportDesignImage") Then

            If (dgReportDesignGrid.Item("ReportDesignImage", rowIndex).Value Is DBNull.Value) Then
                pbReportDesignImage.Image = Nothing
                Return
            End If

            Dim designImage() As Byte = CType(dgReportDesignGrid.Item("ReportDesignImage", rowIndex).Value, Byte())
            Dim designImageBuffer As New MemoryStream(designImage)
            Dim imageToShow As Image = New Bitmap(designImageBuffer)

            pbReportDesignImage.Image = imageToShow

        End If
    End Sub

    Private Sub cmbReportCustomerList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbReportCustomerList.SelectedIndexChanged
        If (cmbReportCustomerList.SelectedIndex = -1 Or cmbReportCustomerList.SelectedValue = -1) Then
            resetIndexOfComboBox(cmbReportDesignNoList)
            gSelectedCustNo = -1
            Return
        End If

        Dim custNo As Integer = cmbReportCustomerList.SelectedValue
        gSelectedCustNo = custNo
        loadDesignList(custNo, cmbReportDesignNoList)

        If (cmbReportCustomerList.SelectedIndex = -1 Or cmbReportCustomerList.SelectedValue = -1) Then
            resetIndexOfComboBox(cmbReportBillNoList)
            Return
        End If

        loadBillList(custNo, cmbReportBillNoList)
    End Sub

    Private Sub cmbReportBillNoList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbReportBillNoList.SelectedIndexChanged
        If (cmbReportBillNoList.SelectedIndex = -1 Or cmbReportBillNoList.SelectedValue = -1) Then
            resetIndexOfComboBox(cmbReportDesignNoList)
            Return
        End If

        loadDesignList(Nothing, cmbReportDesignNoList, cmbReportBillNoList.SelectedValue)
    End Sub

    Private Sub dgDesDesignDetails_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgDesDesignDetails.CellClick
        If e.RowIndex < 0 Then
            Return
        End If

        Dim designNo As Integer = dgDesDesignDetails.Item("designNo", e.RowIndex).Value
        cmbDesDesignList.SelectedValue = designNo
    End Sub

    Private Sub dgBIllingBillDetails_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgBIllingBillDetails.CellClick
        If e.RowIndex < 0 Then
            Return
        End If

        If btnBillingConfirmCreateBill.Visible = True Then
            MsgBox("You are in the middle of creating a Bill. You need to either complete the Bill creation or exit the Bill Creation to view any other Bills")
            Return
        End If

        Dim billNo As Integer = dgBIllingBillDetails.Item("InternalBillNo", e.RowIndex).Value
        cmbBillingBillNoList.SelectedValue = billNo
    End Sub

    Private Sub dgPaymentDetails_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgPaymentDetails.CellClick
        If e.RowIndex < 0 Then
            Return
        End If

        If btnPaymentConfirmCreatePayment.Visible = True Then
            MsgBox("You are in the middle of creating a Payment. You need to either complete the payment creation or exit the create payment to view any other payment")
            Return
        End If

        Dim paymentNo As Integer = dgPaymentDetails.Item("PaymentNo", e.RowIndex).Value
        cmbPaymentPaymentNoList.SelectedValue = paymentNo
    End Sub

    Private Sub dgCustCustomerDetails_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgCustCustomerDetails.CellClick
        If e.RowIndex < 0 Then
            Return
        End If

        Dim CustNo As Integer = dgCustCustomerDetails.Item("CustomerCustNo", e.RowIndex).Value
        cmbCustCustomerList.SelectedValue = CustNo
    End Sub

    Private Sub btnPrintBillSearchDetails_Click(sender As Object, e As EventArgs) Handles btnPrintBillSearchDetails.ClickButtonArea
        If dgReportBillGrid.Rows.Count = 0 Then
            MsgBox("There are no bills to show the bill report. Please refine your search criteria")
            Return
        End If

        BillSearchCrystalReportHolder.ShowDialog()
    End Sub

    Private Sub btnPrintGSTDetails_Click(sender As Object, e As EventArgs) Handles btnPrintGSTDetails.ClickButtonArea
        If dgReportBillGrid.Rows.Count = 0 Then
            MsgBox("There are no bills to show the GST report. Please refine your search criteria")
            Return
        End If

        GSTCrystalReportHolder.ShowDialog()
    End Sub

    Private Sub btnSettingsBackupDatabase_Click(sender As Object, e As EventArgs) Handles btnSettingsBackupDatabase.ClickButtonArea
        Dim folderDialog As New FolderBrowserDialog
        If folderDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then

            Dim DBBackupPath As String = folderDialog.SelectedPath.ToString
            Dim todayDateTime As String = DateTime.Now.ToString("yyyyMMddHHmmss")
            Dim DBBackupFileName As String = "backup database agnidatabase to disk='" + DBBackupPath + "\Agni_DB_Backup_" + todayDateTime + ".bak'"

            Dim cmd As SqlCommand = New SqlCommand(DBBackupFileName, dbConnection)
            cmd.ExecuteNonQuery()

            MsgBox("The Database backup stored in file: " + DBBackupFileName)

        End If


    End Sub

    Private Sub btnSettingsResetBilNo_Click(sender As Object, e As EventArgs) Handles btnSettingsResetBilNo.ClickButtonArea
        If MessageBox.Show("This operatoin will reset the bill number to 1 and you cannot reverse this operation. Do you really want to reset the bill number to 1? ", "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Dim actionResult As DialogResult = ActionConfirmation.ShowDialog()
            ActionConfirmation.Dispose()

            If actionResult = System.Windows.Forms.DialogResult.Cancel Then
                'No need to anything as this result you get is when the user is canceled the Action Dialog
            ElseIf actionResult = System.Windows.Forms.DialogResult.Yes Then
                insertOrReplaceAttribute(ATTRIBUTE_LAST_BILL_NO, "0")
                MsgBox("The bill number has been reset. Next time you generate the bill the bill number will start with 1. If you have mistakenly did this operation then immediately contact the software maintenance team for help")
            ElseIf actionResult = System.Windows.Forms.DialogResult.No Then
                MsgBox("You do not have permission for this operation. Please try with Administrator user when prompted for confirmation")
            End If
        End If
    End Sub



    Private Sub btnReportSearchReset_Click(sender As Object, e As EventArgs) Handles btnReportSearchReset.ClickButtonArea
        resetIndexOfComboBox(cmbReportCustomerList)
        resetIndexOfComboBox(cmbReportBillNoList)
        resetIndexOfComboBox(cmbReportDesignNoList)
        txtReportDesignNumber.Text = ""
        dpReportFromDate.Text = DateTime.Today
        dpReportToDate.Text = DateTime.Today
        If dgReportDesignGrid.DataSource IsNot Nothing Then
            Call CType(dgReportDesignGrid.DataSource, DataTable).Rows.Clear()
        End If
        If dgReportBillGrid.DataSource IsNot Nothing Then
            Call CType(dgReportBillGrid.DataSource, DataTable).Rows.Clear()
        End If
        If dgReportPaymentGrid.DataSource IsNot Nothing Then
            Call CType(dgReportPaymentGrid.DataSource, DataTable).Rows.Clear()
        End If

        lblReportNoOfDesigns.Text = 0
        lblReportBilledDesignAmount.Text = 0
        lblReportUnBilledDesignAmount.Text = 0
        lblReportTotDesignAmount.Text = 0
        lblReportNoOfBills.Text = 0
        lblReportBillBilledAmount.Text = 0
        lblReportBIllPaidAmount.Text = 0
        lblReportBillNetBalance.Text = 0
        lblReportNoOfPayment.Text = 0
        lblReportPaidAmountActual.Text = 0
        lblReportPaidAmountWithDeduction.Text = 0
    End Sub

    Private Sub btnPrintPaymentDetails_Click(sender As Object, e As EventArgs) Handles btnPrintPaymentDetails.ClickButtonArea
        If dgReportPaymentGrid.Rows.Count = 0 Then
            MsgBox("There are no payment to show the Payment only report. Please refine your search criteria")
            Return
        End If

        PaymentSearchCrystalReportHolder.ShowDialog()
    End Sub

    Private Sub btnPrintBillAndPaymentDetails_Click(sender As Object, e As EventArgs) Handles btnPrintBillAndPaymentDetails.ClickButtonArea
        If dgReportPaymentGrid.Rows.Count = 0 Then
            MsgBox("There are no payment to show the Bill and Payment details report. Please refine your search criteria")
            Return
        End If

        BillAndPaymentHistoryCrystalReportHolder.ShowDialog()
    End Sub

    Private Sub btnBillingDeleteBill_Click(sender As Object, e As EventArgs) Handles btnBillingDeleteBill.ClickButtonArea
        If gSelectedBillNo = -1 Then
            MessageBox.Show("Please select a bill")
            cmbDesDesignList.Focus()
            Return
        End If

        Dim billTable As DataTable = dgBIllingBillDetails.DataSource

        billTable.DefaultView.Sort = "BillNo ASC"
        billTable = billTable.DefaultView.ToTable

        Dim lastBillRow As DataRow = billTable.Rows(billTable.Rows.Count - 1)
        Dim lastRowBillNo As Integer = lastBillRow.Item("BillNo")

        If (lastRowBillNo <> gSelectedBillNo) Then
            MessageBox.Show("Sorry, This bill is not last bill, so it cannot be deleted. You can delete only the last bill and also that last bill should not have any payments towards it.")
            cmbBillingBillNoList.Focus()
            Return
        End If

        Dim primaryKey(0) As DataColumn
        primaryKey(0) = billTable.Columns("BillNo")
        billTable.PrimaryKey = primaryKey

        Dim dataRow As DataRow = billTable.Rows.Find(gSelectedBillNo)
        Dim paidAmount As Decimal = dataRow.Item("PaidAmount")
        If (paidAmount > 0) Then
            MessageBox.Show("Sorry, This bill is having payments, so it cannot be deleted. You can delete only the last bill and also that last bill" +
                " should not have any payments towards it. First try to delete the respective payments and try this operation.")
            cmbBillingBillNoList.Focus()
            Return
        End If

        If MessageBox.Show("Are you sure you want to delete this Bill? Please note that this action cannot be undone.", "WARNING", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Dim actionResult As DialogResult = ActionConfirmation.ShowDialog()
            ActionConfirmation.Dispose()

            If actionResult = System.Windows.Forms.DialogResult.Cancel Then
                'No need to do anything as this result you get is when the user is canceled the Action Dialog
            ElseIf actionResult = System.Windows.Forms.DialogResult.Yes Then
                Dim custNo As Integer = cmbBillingCustomerList.SelectedValue

                Dim query As String = "DELETE FROM Bill where BillNo=@billNo"

                Using comm As New SqlCommand()
                    With comm
                        .Connection = dbConnection
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@billNo", gSelectedBillNo)
                    End With
                    comm.ExecuteNonQuery()
                End Using

                'log.Debug("btnBillingDeleteBill_Click: custNo.ToString : " + custNo.ToString + " gSelectedBillNo.ToString: " + gSelectedBillNo.ToString)
                Try
                    Dim designQuery As String = "update design set Billed=@Billed, BillNo=@BillNo where CustNo=@CustNo and BillNo=@deletedBillNo"

                    Using comm As New SqlCommand()
                        With comm
                            .Connection = dbConnection
                            .CommandType = CommandType.Text
                            .CommandText = designQuery
                            .Parameters.AddWithValue("@CustNo", custNo.ToString)
                            .Parameters.AddWithValue("@deletedBillNo", gSelectedBillNo)
                            .Parameters.AddWithValue("@Billed", False)
                            .Parameters.AddWithValue("@BillNo", DBNull.Value)
                        End With
                        comm.ExecuteNonQuery()
                    End Using
                Catch sqlEx As SqlException
                    MsgBox("Operation failed. DB error. Please try again or contact the software support if problem persists")
                Catch ex As Exception
                    MessageBox.Show("Message to Agni User:   " & ex.Message)
                End Try

                gSelectedBillNo = -1
                gSelectedDisplayBillNo = String.Empty

                loadBillList(custNo)
                loadBillGrid(custNo)

                loadDesignList(custNo)
                loadDesignGrid(custNo)

                MessageBox.Show("Bill successfully deleted")
            ElseIf actionResult = System.Windows.Forms.DialogResult.No Then
                MsgBox("You do not have permission for this operation. Please try with Administrator user when prompted for confirmation")
            End If

        End If
    End Sub

    Private Sub AgniMainForm_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If (e.Alt) Then
            Select Case (e.KeyCode)
                Case Keys.C
                    tabAllTabsHolder.SelectedTab = tabCustomer
                Case Keys.D
                    tabAllTabsHolder.SelectedTab = tabDesign
                Case Keys.B
                    tabAllTabsHolder.SelectedTab = tabBilling
                Case Keys.P
                    tabAllTabsHolder.SelectedTab = tabPayment
                Case Keys.S
                    tabAllTabsHolder.SelectedTab = tabSettings
                Case Keys.H
                    tabAllTabsHolder.SelectedTab = tabHelp
                Case Keys.R
                    tabAllTabsHolder.SelectedTab = tabReports
                Case Keys.D1
                    tabAllTabsHolder.SelectedIndex = 0
                Case Keys.D2
                    tabAllTabsHolder.SelectedIndex = 1
                Case Keys.D3
                    tabAllTabsHolder.SelectedIndex = 2
                Case Keys.D4
                    tabAllTabsHolder.SelectedIndex = 3
                Case Keys.D5
                    tabAllTabsHolder.SelectedIndex = 4
                Case Keys.D6
                    tabAllTabsHolder.SelectedIndex = 5
                Case Keys.D7
                    tabAllTabsHolder.SelectedIndex = 6
            End Select
        End If
    End Sub

    Private Sub btnChangeAddress_Click(sender As Object, e As EventArgs) Handles btnSettingsChangeAddress.ClickButtonArea
        ChangeAddress.ShowDialog()
    End Sub

    Private Sub btnLogOff_Click(sender As Object, e As EventArgs) Handles btnLogOff.ClickButtonArea
        If MessageBox.Show("Are you sure want to log off?", "Log off", System.Windows.Forms.MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            Me.Hide()
            Login.Show()
        End If
    End Sub

    Private Sub btnCustAdd_Click(Sender As Object, e As MouseEventArgs) Handles btnCustAdd.ClickButtonArea

    End Sub
End Class