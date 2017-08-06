Imports System.Data.SqlClient
Imports System.IO
Imports System.Math
Imports NLog

Public Class AgnimainForm
    Dim dbConnection As SqlConnection

    Dim log As Logger = LogManager.GetCurrentClassLogger()

    Dim BILL_TYPE_UNBILLED As Int16 = 0
    Dim BILL_TYPE_BILLED As Int16 = 1
    Dim BILL_TYPE_ALL As Int16 = 2

    Public gSelectedCustNoIndex As Integer = -1
    Public gSelectedBillNo As Integer = -1
    Public gDisplayBillNo As Integer = 0

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
        End If
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        log.Debug("Form is loading")

        tabAllTabsHolder.Width = Me.Width
        tabAllTabsHolder.Height = Me.Height
        dpBillingBillDate.Value = DateTime.Today
        cmbCustCompanyList.Focus()

        dbConnection = New SqlConnection("server=agni\SQLEXPRESS;Database=agnidatabase;Integrated Security=true; MultipleActiveResultSets=True;")
        dbConnection.Open()

        Dim lastBillRow As DataRow = getLastBillRow()
        If (lastBillRow IsNot Nothing) Then
            gDisplayBillNo = lastBillRow.Item("DisplayBillNo")
        End If

        bwtCustListLoadThread.RunWorkerAsync()

        resetAllScreens()

    End Sub

    Function getCustomerListTable() As DataTable
        log.Debug("getCustomerListTable: entry")
        Dim customerQuery = New SqlCommand("select CustNo,CompName from customer", dbConnection)
        Dim customerAdapter = New SqlDataAdapter()
        customerAdapter.SelectCommand = customerQuery
        Dim customerDataSet = New DataSet
        customerAdapter.Fill(customerDataSet, "customer")
        Return customerDataSet.Tables(0)
    End Function


    Sub setCustomerList(customerTable As DataTable, Optional cmbCompanyList As ComboBox = Nothing)
        log.Debug("setCustomerList: entry")

        Dim dummyFirstRow As DataRow = customerTable.NewRow()
        dummyFirstRow("CustNo") = -1
        dummyFirstRow("CompName") = "Please select a customer..."
        customerTable.Rows.InsertAt(dummyFirstRow, 0)

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

    Function getDesignListTable(Optional custNo As Integer = Nothing) As DataTable
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
        Return designDataSet.Tables(0)
    End Function

    Sub setDesignList(designTable As DataTable)
        cmbDesDesignList.BindingContext = New BindingContext()
        cmbDesDesignList.DataSource = designTable
    End Sub

    Function getDesignGridTable(Optional custNo As Integer = Nothing) As DataTable
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
    Sub setDesignGrid(designTable As DataTable)
        dgDesDesignDetails.DataSource = designTable
    End Sub

    Function getBillListTable(Optional custNo As Integer = Nothing) As DataTable
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
        Return billDataSet.Tables(0)

    End Function

    Sub setBillingList(billTable As DataTable, Optional cmbBillList As ComboBox = Nothing)

        If billTable.Rows.Count = 0 Then
            If cmbBillList IsNot Nothing Then
                cmbBillList.SelectedIndex = -1
            Else
                cmbBillingBillNoList.SelectedIndex = -1
            End If
        End If

        If cmbBillList IsNot Nothing Then
            cmbBillingBillNoList.BindingContext = New BindingContext()
            cmbBillList.DataSource = billTable
        Else
            cmbBillingBillNoList.BindingContext = New BindingContext()
            cmbBillingBillNoList.DataSource = billTable
        End If
    End Sub

    Function getBillGridTable(Optional custNo As Integer = Nothing) As DataTable
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

    Sub setBillingGrid(billTable As DataTable)
        dgBIllingBillDetails.DataSource = billTable
    End Sub

    Function getPaymentListTable(Optional custNo As Integer = Nothing) As DataTable
        log.Debug("getPaymentListTable: entry")
        Dim paymentQuery As SqlCommand
        If (custNo <> Nothing) Then
            paymentQuery = New SqlCommand("select PaymentNo from payment where custNo=" + custNo.ToString, dbConnection)
        Else
            paymentQuery = New SqlCommand("select PaymentNo from payment", dbConnection)
        End If

        Dim paymentAdapter = New SqlDataAdapter()
        paymentAdapter.SelectCommand = paymentQuery
        Dim paymentDataSet = New DataSet
        paymentAdapter.Fill(paymentDataSet, "payment")
        Return paymentDataSet.Tables(0)

    End Function
    Sub setPaymentList(paymentTable As DataTable, Optional cmbPaymentList As ComboBox = Nothing)
        If paymentTable.Rows.Count = 0 Then
            If cmbPaymentList IsNot Nothing Then
                cmbPaymentList.SelectedIndex = -1
            Else
                cmbPaymentPaymentNoList.SelectedIndex = -1
            End If
        End If

        If cmbPaymentList IsNot Nothing Then
            cmbPaymentList.BindingContext = New BindingContext()
            cmbPaymentList.DataSource = paymentTable
        Else
            cmbPaymentPaymentNoList.BindingContext = New BindingContext()
            cmbPaymentPaymentNoList.DataSource = paymentTable
        End If

    End Sub

    Sub loadPaymentGrid(Optional custNo As Integer = Nothing)
        'Try
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
        Dim paymentTable = paymentDataSet.Tables(0)
        dgPaymentDetails.DataSource = paymentTable
        dgPaymentDetails.Invalidate()
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Function getPaymentGridTable(Optional custNo As Integer = Nothing) As DataTable
        log.Debug("getPaymentGridTable: entry")
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
        Return paymentDataSet.Tables(0)
    End Function

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


    Private Sub cmbCustCompanyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCustCompanyList.SelectedIndexChanged
        'Try
        gSelectedCustNoIndex = cmbCustCompanyList.SelectedIndex

        If (cmbCustCompanyList.SelectedIndex = -1 Or cmbCustCompanyList.SelectedValue = -1) Then
            resetCustomerScreen()
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
            txtGstIn.Text = dataRow.Item("GSTIN")
            txtOwnerName.Text = dataRow.Item("OwnerName")
            txtAddress.Text = dataRow.Item("Address")
            txtMobile.Text = dataRow.Item("Mobile")
            txtLandline.Text = dataRow.Item("Landline")
            txtEmail.Text = dataRow.Item("Email")
            txtWebsite.Text = dataRow.Item("Website")
            txtCGST.Text = dataRow.Item("CGST").ToString
            txtSGST.Text = dataRow.Item("SGST").ToString
            txtIGST.Text = dataRow.Item("IGST").ToString
            txtWPCharge.Text = dataRow.Item("WorkingPrintSqrInch").ToString
            txtWorkingCharge.Text = dataRow.Item("WorkingColor").ToString
            txtPrintCharge.Text = dataRow.Item("PrintColor").ToString
        Else
            MessageBox.Show("No data found for customer: " + custNo + "-" + cmbCustCompanyList.Text)
        End If


        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try

    End Sub

    Sub resetAllScreens()
        cmbCustCompanyList.SelectedIndex = -1
        cmbDesCompanyList.SelectedIndex = -1
        cmbBillingCompanyList.SelectedIndex = -1
        cmbPaymentCompanyList.SelectedIndex = -1
        cmbReportCompanyList.SelectedIndex = -1
    End Sub

    Sub resetCustomerScreen()
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

    Sub resetBillingScreen(Optional resetBillListIndex As Boolean = True)
        cmbBillingBillNoList.Enabled = True
        btnBillingCreateBill.Visible = True
        btnBillingConfirmCreateBill.Visible = False
        btnBillingCancelCreateBill.Visible = False
        btnBillingCancelBill.Text = "Mark Cancelled"
        lblCancelledBillIndicator.Visible = False

        If resetBillListIndex Then
            cmbBillingBillNoList.SelectedIndex = -1
        End If

        dpBillingBillDate.Text = ""
        txtBillingPrevBalance.Text = ""
        txtBillingDesignAmoutBeforeGST.Text = ""
        txtBillingTotalGSTAmount.Text = ""
        txtBillingDesignAmoutAfterGST.Text = ""
        txtBillingTotalAmount.Text = ""
        txtBillingPaidAmount.Text = ""
        txtBillingRemainingBalance.Text = ""
        cmbBillingBillNoList.Focus()
    End Sub

    Sub resetPaymentScreen(Optional resetPaymentListIndex As Boolean = True)
        log.Debug("resetPaymentScreen: resetting payment screen")
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

        If resetPaymentListIndex Then
            cmbPaymentPaymentNoList.SelectedIndex = -1
        End If

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

    Public Sub btnCustAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustAdd.Click
        Try
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
                query &= "CGST, SGST, IGST, WorkingPrintSqrInch, WorkingColor, PrintColor) "
                query &= "VALUES ( @CompName, @GSTIN, @OwnerName, @Address, @Mobile, @Landline, @Email, "
                query &= "@Website, @CGST, @SGST, @IGST, @WorkingPrintSqrInch, @WorkingColor, @PrintColor)"

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
                        .Parameters.AddWithValue("@CGST", If(String.IsNullOrEmpty(txtCGST.Text), DBNull.Value, txtCGST.Text))
                        .Parameters.AddWithValue("@SGST", If(String.IsNullOrEmpty(txtSGST.Text), DBNull.Value, txtSGST.Text))
                        .Parameters.AddWithValue("@IGST", If(String.IsNullOrEmpty(txtIGST.Text), DBNull.Value, txtIGST.Text))
                        .Parameters.AddWithValue("@WorkingPrintSqrInch", If(String.IsNullOrEmpty(txtWPCharge.Text), DBNull.Value, txtWPCharge.Text))
                        .Parameters.AddWithValue("@WorkingColor", If(String.IsNullOrEmpty(txtWorkingCharge.Text), DBNull.Value, txtWorkingCharge.Text))
                        .Parameters.AddWithValue("@PrintColor", If(String.IsNullOrEmpty(txtPrintCharge.Text), DBNull.Value, txtPrintCharge.Text))
                    End With
                    comm.ExecuteNonQuery()
                End Using
                MessageBox.Show("Company successfully added")
                bwtCustListLoadThread.RunWorkerAsync()
            End If
        Catch ex As Exception
            MessageBox.Show("Message to Agni User:   " & ex.Message & " (Or) This Customer Record may already exist")
        End Try
    End Sub

    Private Sub btnCustDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustDelete.Click
        'Try

        If (cmbCustCompanyList.SelectedIndex = -1) Then
            MessageBox.Show("Please select a Company from Company List")
            cmbCustCompanyList.Focus()
            Return
        End If

        If MessageBox.Show("All Designs, Bills and Payments will be deleted belongs to this customer." + vbNewLine + vbNewLine + "  Do you want to delete this Customer - " & cmbCustCompanyList.Text & " ?", "WARNING", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
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
    Public Sub deleteSeletectedCustomer()

        If cmbCustCompanyList.SelectedIndex = -1 Then
            MessageBox.Show("Select a company from company list")
            cmbCustCompanyList.Focus()
            Return
        End If

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
        bwtCustListLoadThread.RunWorkerAsync()
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
            query &= "Mobile=@Mobile, Landline=@Landline, Email=@Email, Website=@Website, CGST=@CGST, SGST=@SGST, "
            query &= "IGST=@IGST, WorkingPrintSqrInch=@WorkingPrintSqrInch, WorkingColor=@WorkingColor, PrintColor=@PrintColor where CustNo=@CustNo"

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
                    .Parameters.AddWithValue("@CGST", If(String.IsNullOrEmpty(txtCGST.Text), DBNull.Value, txtCGST.Text))
                    .Parameters.AddWithValue("@SGST", If(String.IsNullOrEmpty(txtSGST.Text), DBNull.Value, txtSGST.Text))
                    .Parameters.AddWithValue("@IGST", If(String.IsNullOrEmpty(txtIGST.Text), DBNull.Value, txtIGST.Text))
                    .Parameters.AddWithValue("@WorkingPrintSqrInch", If(String.IsNullOrEmpty(txtWPCharge.Text), DBNull.Value, txtWPCharge.Text))
                    .Parameters.AddWithValue("@WorkingColor", If(String.IsNullOrEmpty(txtWorkingCharge.Text), DBNull.Value, txtWorkingCharge.Text))
                    .Parameters.AddWithValue("@PrintColor", If(String.IsNullOrEmpty(txtPrintCharge.Text), DBNull.Value, txtPrintCharge.Text))
                End With
                comm.ExecuteNonQuery()
            End Using
            MessageBox.Show("Company successfully updated")
            resetCustomerScreen()
            bwtCustListLoadThread.RunWorkerAsync()
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

            Dim custNo As Integer = cmbDesCompanyList.SelectedValue

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
            bwtDesListLoadThread.RunWorkerAsync(custNo)
            bwtDesGridLoadThread.RunWorkerAsync(custNo)

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

            Dim custNo As Integer = cmbDesCompanyList.SelectedValue

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
            bwtDesListLoadThread.RunWorkerAsync(custNo)
            bwtDesGridLoadThread.RunWorkerAsync(custNo)
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
        bwtDesListLoadThread.RunWorkerAsync(custNo)
        bwtDesGridLoadThread.RunWorkerAsync(custNo)
    End Sub

    Sub updateDesignsAsUnBilled(BillNo As Integer)

        If (BillNo = -1) Then
            Return
        End If

        Dim custNo As Integer = cmbDesCompanyList.SelectedValue

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
        bwtDesListLoadThread.RunWorkerAsync(custNo)
        bwtDesGridLoadThread.RunWorkerAsync(custNo)
    End Sub

    Sub addPaidAmountInBill(BillNo As Integer, paidAmount As Decimal)

        If (BillNo = -1) Then
            Return
        End If

        Dim custNo As Integer = cmbDesCompanyList.SelectedValue

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
        bwtCustListLoadThread.RunWorkerAsync(custNo)
        bwtDesGridLoadThread.RunWorkerAsync(custNo)
    End Sub

    Private Sub btnDesDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesDelete.Click
        'Try
        If cmbDesDesignList.SelectedIndex = -1 Then
            MessageBox.Show("Please select a design")
            cmbDesDesignList.Focus()
            Return
        End If

        If MessageBox.Show("Do you want to delete the design " & cmbDesDesignList.Text, "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Dim custNo As Integer = cmbDesCompanyList.SelectedValue

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
            bwtDesListLoadThread.RunWorkerAsync(custNo)
            bwtDesGridLoadThread.RunWorkerAsync(custNo)

        End If

        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub cmbBillingBillNoList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBillingBillNoList.SelectedIndexChanged
        'Try
        log.Debug("cmbBillingBillNoList_SelectedIndexChanged: entry")

        log.Debug("cmbBillingBillNoList_SelectedIndexChanged: cmbBillingBillNoList.SelectedIndex: " + cmbBillingBillNoList.SelectedIndex.ToString)

        If (cmbBillingBillNoList.SelectedIndex = -1) Then
            log.Debug("cmbBillingBillNoList_SelectedIndexChanged: resetBillingScreen is calling with false")
            resetBillingScreen(False)
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
            'log.Debug("cmbBillingBillNoList_SelectedIndexChanged: No data found for Bill: " + billNo.ToString)
        End If

        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub cmbBillingCompanyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBillingCompanyList.SelectedIndexChanged
        'Try
        gSelectedCustNoIndex = cmbBillingCompanyList.SelectedIndex

        If (cmbBillingCompanyList.SelectedIndex = -1 Or cmbBillingCompanyList.SelectedValue = -1) Then
            resetBillingScreen()
            Return
        End If

        bwtBillListLoadThread.RunWorkerAsync(cmbBillingCompanyList.SelectedValue)
        bwtBillGridLoadThread.RunWorkerAsync(cmbBillingCompanyList.SelectedValue)

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
        txtBillingDesignAmoutBeforeGST.Text = desamount
        txtBillingTotalAmount.Text = desamount + balamount
        txtBillingRemainingBalance.Text = txtBillingTotalAmount.Text
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub btnBillingPrintBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBillingPrintBill.Click
        'Try
        If cmbBillingBillNoList.SelectedIndex = -1 Then
            MsgBox("Please select a bill to print")
            cmbBillingBillNoList.Focus()
            Return
        End If

        gSelectedBillNo = cmbBillingBillNoList.SelectedValue

        BillReportForm.Show()
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub cmbDesCompanyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDesCompanyList.SelectedIndexChanged
        'Try
        gSelectedCustNoIndex = cmbDesCompanyList.SelectedIndex

        If (cmbDesCompanyList.SelectedIndex = -1 Or cmbDesCompanyList.SelectedValue = -1) Then
            resetDesignScreen()
            Return
        End If

        WorkingChargeTypeChanged()
        bwtDesListLoadThread.RunWorkerAsync(cmbDesCompanyList.SelectedValue)
        bwtDesGridLoadThread.RunWorkerAsync(cmbDesCompanyList.SelectedValue)
        'loadDesignGrid(cmbDesCompanyList.SelectedValue)
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub dgDesDesignDetails_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgDesDesignDetails.CurrentCellChanged
        cmbDesDesignList.SelectedIndex = dgDesDesignDetails.CurrentRowIndex
    End Sub

    Private Sub btnCustClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCustClear.Click
        cmbCustCompanyList.SelectedIndex = -1
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


    Private Sub WorkingChargeTypeChanged() Handles radioDesWP.CheckedChanged, radioDesWorking.CheckedChanged, radioDesPrint.CheckedChanged

        If (cmbDesCompanyList.SelectedIndex = -1) Then
            Return
        End If

        Dim custNo = cmbDesCompanyList.SelectedValue
        Dim customerQuery = New SqlCommand("select WorkingPrintSqrInch, WorkingColor, PrintColor from customer where CustNo=" + custNo.ToString, dbConnection)
        Dim customerAdapter = New SqlDataAdapter()
        customerAdapter.SelectCommand = customerQuery
        Dim customerDataSet = New DataSet
        customerAdapter.Fill(customerDataSet, "customer")
        Dim customerTable As DataTable = customerDataSet.Tables(0)

        If (customerTable.Rows.Count = 0) Then
            Return
        End If

        If radioDesWP.Checked Then
            lblDesCostPerUnit.Text = "Cost Per Inch"
            txtDesCostPerUnit.Text = customerTable.Rows(0).Item("WorkingPrintSqrInch").ToString
        ElseIf radioDesWorking.Checked Then
            lblDesCostPerUnit.Text = "Cost Per Color"
            txtDesCostPerUnit.Text = customerTable.Rows(0).Item("WorkingColor").ToString
        ElseIf radioDesPrint.Checked Then
            lblDesCostPerUnit.Text = "Cost Per Inch"
            txtDesCostPerUnit.Text = customerTable.Rows(0).Item("PrintColor").ToString
        End If
    End Sub

    Private Sub ComboBox6_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbReportCompanyList.SelectedIndexChanged
        'Try
        'gSelectedCustNoIndex = cmbReportCompanyList.SelectedIndex
        'If (cmbReportCompanyList.SelectedIndex = -1) Then
        '    Return
        'End If

        'loadOrReloadDesignList(cmbReportDesignList, cmbDesCompanyList.SelectedValue)

        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub btnDesEditPrice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesEditPrice.Click
        txtDesCalculatedPrice.ReadOnly = Not txtDesCalculatedPrice.ReadOnly
        txtDesCalculatedPrice.Focus()
    End Sub

    Private Sub btnDesClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesClear.Click
        resetDesignScreen()
    End Sub

    Private Sub bwtCustListThread_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwtCustListLoadThread.DoWork
        e.Result = getCustomerListTable()
    End Sub

    Private Sub bwtCustListThread_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwtCustListLoadThread.RunWorkerCompleted
        Dim customerTable As DataTable = e.Result
        setCustomerList(customerTable)
    End Sub

    Private Sub bwtDesListThread_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwtDesListLoadThread.DoWork
        Dim custNo As Integer = e.Argument
        e.Result = getDesignListTable(custNo)
    End Sub

    Private Sub bwtDesListThread_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwtDesListLoadThread.RunWorkerCompleted
        Dim designTable As DataTable = e.Result
        setDesignList(designTable)
    End Sub

    Private Sub bwtDesGridLoadThread_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwtDesGridLoadThread.DoWork
        Dim custNo As Integer = e.Argument
        e.Result = getDesignGridTable(custNo)
    End Sub

    Private Sub bwtDesGridLoadThread_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwtDesGridLoadThread.RunWorkerCompleted
        Dim designTable As DataTable = e.Result
        setDesignGrid(designTable)
    End Sub

    Private Sub bwtBillListLoadThread_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwtBillListLoadThread.DoWork
        Dim custNo As Integer = e.Argument
        e.Result = getBillListTable(custNo)
    End Sub

    Private Sub bwtBillListLoadThread_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwtBillListLoadThread.RunWorkerCompleted
        Dim billTable As DataTable = e.Result
        setBillingList(billTable)
    End Sub

    Private Sub bwtBillGridLoadThread_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwtBillGridLoadThread.DoWork
        Dim custNo As Integer = e.Argument
        e.Result = getBillGridTable(custNo)
    End Sub

    Private Sub bwtBillGridLoadThread_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwtBillGridLoadThread.RunWorkerCompleted
        Dim billingTable As DataTable = e.Result
        setBillingGrid(billingTable)
    End Sub

    Private Sub bwtPaymentListLoadThread_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwtPaymentListLoadThread.DoWork
        Dim custNo As Integer = e.Argument
        e.Result = getPaymentListTable(custNo)
    End Sub

    Private Sub bwtPaymentListLoadThread_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwtPaymentListLoadThread.RunWorkerCompleted
        Dim paymentTable As DataTable = e.Result
        setPaymentList(paymentTable)
    End Sub

    Private Sub bwtPaymentGridLoadThread_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bwtPaymentGridLoadThread.DoWork
        Dim custNo As Integer = e.Argument
        e.Result = getPaymentGridTable(custNo)
    End Sub

    Private Sub bwtPaymentGridLoadThread_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwtPaymentGridLoadThread.RunWorkerCompleted
        Dim paymentTable As DataTable = e.Result
        setPaymentGrid(paymentTable)
    End Sub

    Private Sub tabPayment_Click(sender As Object, e As EventArgs) Handles tabPayment.Click

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
            BillReportForm.Close()
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

    Private Sub TabPage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabCustomer.Click
        cmbCustCompanyList.Focus()
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
        BillReportForm.Show()
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
    End Sub

    Private Sub btnBilingOutstandingBalance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBilingOutstandingBalance.Click
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

    Private Sub radioPaymentByCash_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioPaymentByCash.CheckedChanged
        gbBankDetails.Visible = False
    End Sub

    Private Sub radioPaymentByCheque_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioPaymentByCheque.CheckedChanged
        gbBankDetails.Visible = True
    End Sub

    Private Sub cmbPaymentCompanyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPaymentCompanyList.SelectedIndexChanged
        log.debug("cmbPaymentCompanyList_SelectedIndexChanged: entry")
        gSelectedCustNoIndex = cmbPaymentCompanyList.SelectedIndex

        If (cmbPaymentCompanyList.SelectedIndex = -1 Or cmbPaymentCompanyList.SelectedValue = -1) Then
            resetPaymentScreen()
            Return
        End If

        bwtPaymentListLoadThread.RunWorkerAsync(cmbPaymentCompanyList.SelectedValue)
        bwtPaymentGridLoadThread.RunWorkerAsync(cmbPaymentCompanyList.SelectedValue)

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

        'log.Debug("btnBillingCreateBill_Click: entry")

        cmbBillingBillNoList.SelectedIndex = -1
        Dim custNo = cmbBillingCompanyList.SelectedValue

        loadGSTForCustomerInBilling(custNo)

        dpBillingBillDate.Text = ""
        Dim unBilledDesignAmount As Decimal = getDesignAmountWithoutGSTTax(BILL_TYPE_UNBILLED, custNo)
        Dim billedDesignAmount As Decimal = getDesignAmountWithGSTTax(BILL_TYPE_BILLED, custNo)
        Dim totalBillsPaidAmount As Decimal = getAllBillsPaidAmount(custNo)
        Dim unPaidBalance As Decimal = billedDesignAmount - totalBillsPaidAmount

        If (unBilledDesignAmount = 0) Then
            MessageBox.Show("There are no designs to bill or all the designs are billed already for this customer")
            Return
        End If

        txtBillingDesignAmoutBeforeGST.Text = Format(unBilledDesignAmount, "0.00")
        txtBillingPrevBalance.Text = Format(unPaidBalance, "0.00")

        calculateGSTAmountInBilling()
        Dim totalAmountToPay As Decimal = Format(unPaidBalance + Decimal.Parse(txtBillingDesignAmoutAfterGST.Text), "0.00")

        txtBillingTotalAmount.Text = Format(totalAmountToPay, "0.00")
        txtBillingPaidAmount.Text = "0.00"
        txtBillingRemainingBalance.Text = Format(totalAmountToPay, "0.00")

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
                    .Parameters.AddWithValue("@CustNo", cmbBillingCompanyList.SelectedValue)
                    .Parameters.AddWithValue("@BillDate", dpBillingBillDate.Text)
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

            updateRecentDesignsAsBilled(cmbBillingCompanyList.SelectedValue, newBillNo)

            MessageBox.Show("Bill successfully added")
            resetBillingScreen()
            bwtBillListLoadThread.RunWorkerAsync(cmbBillingCompanyList.SelectedValue)
            bwtBillGridLoadThread.RunWorkerAsync(cmbBillingCompanyList.SelectedValue)

            btnBillingCreateBill.Visible = True
            btnBillingConfirmCreateBill.Visible = False
            btnBillingCancelCreateBill.Visible = False
            cmbBillingBillNoList.Enabled = True

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

        If (lastBillRow Is Nothing) Then
            Return
        End If

        Dim lastBillNumber = lastBillRow.Item("BillNo")
        cmbBillingBillNoList.SelectedValue = lastBillNumber
    End Sub

    Private Sub btnBillingCancelBill_Click(sender As Object, e As EventArgs) Handles btnBillingCancelBill.Click

        If (cmbBillingBillNoList.SelectedIndex = -1) Then
            MessageBox.Show("Please select a bill")
            cmbBillingBillNoList.Focus()
            Return
        End If

        Dim billNo As Integer = cmbBillingBillNoList.SelectedValue
        Dim custNo As Integer = cmbBillingCompanyList.SelectedValue

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

        resetBillingScreen()

        bwtBillListLoadThread.RunWorkerAsync(custNo)
        bwtBillGridLoadThread.RunWorkerAsync(custNo)

    End Sub

    Private Sub dgBIllingBillDetails_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgBIllingBillDetails.CurrentCellChanged
        cmbBillingBillNoList.SelectedIndex = dgBIllingBillDetails.CurrentRowIndex
    End Sub

    Private Sub btnPaymentCreatePayment_Click(sender As Object, e As EventArgs) Handles btnPaymentCreatePayment.Click
        'log.Debug("btnBillingCreateBill_Click: entry")

        If (cmbPaymentCompanyList.SelectedIndex = -1) Then
            MessageBox.Show("Please select the company name to create a payment")
            cmbPaymentCompanyList.Focus()
            Return
        End If

        Dim custNo = cmbPaymentCompanyList.SelectedValue

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

    Private Sub btnPaymentConfirmCreatePayment_Click(sender As Object, e As EventArgs) Handles btnPaymentConfirmCreatePayment.Click

        If cmbPaymentCompanyList.Text.Trim.Equals("") Then
            MessageBox.Show("Choose a company from company list")
            cmbPaymentCompanyList.Focus()
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
        ElseIf Not cmbPaymentPaymentNoList.Text.Trim.Equals("") Then
            MessageBox.Show("You cannot make a payment from existing payment. You can only make payment for unpaid amount. Please reselect the customer from customer list to see the unpaid amount")
            cmbPaymentPaymentNoList.Focus()
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
                    .Parameters.AddWithValue("@CustNo", cmbPaymentCompanyList.SelectedValue)
                    .Parameters.AddWithValue("@BillNo", txtPaymentDisplayBillNo.Text)
                    .Parameters.AddWithValue("@UnPaidBilledAmount", txtPaymentUnPaidBilledAmount.Text)
                    .Parameters.AddWithValue("@PaymentDate", dpPaymentDate.Text)
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
                    comm.Parameters.AddWithValue("@ChequeDate", dpPaymentChequeDate.Text)
                Else
                    comm.Parameters.AddWithValue("@ChequeNo", DBNull.Value)
                    comm.Parameters.AddWithValue("@BankName", DBNull.Value)
                    comm.Parameters.AddWithValue("@ChequeDate", DBNull.Value)
                End If

                newPaymentNo = CInt(comm.ExecuteScalar())
            End Using

            cmbPaymentPaymentNoList.Text = newPaymentNo

            addPaidAmountInBill(txtPaymentDisplayBillNo.Text, txtPaymentFinalPaidAmount.Text)

            MessageBox.Show("Payment successfully added")

            bwtPaymentListLoadThread.RunWorkerAsync(cmbBillingCompanyList.SelectedValue)
            bwtPaymentGridLoadThread.RunWorkerAsync(cmbBillingCompanyList.SelectedValue)
        End If
    End Sub

    Private Sub btnPaymentCancelCreatePayment_Click(sender As Object, e As EventArgs) Handles btnPaymentCancelCreatePayment.Click
        resetPaymentScreen()

        Dim lastPaymentRow = getLastPaymentRow(cmbBillingCompanyList.SelectedValue)
        If (lastPaymentRow Is Nothing) Then
            Return
        End If

        Dim lastPaymentNumber = lastPaymentRow.Item("PaymentNo")
        cmbPaymentPaymentNoList.SelectedValue = lastPaymentNumber

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

        txtBillingDesignAmoutAfterGST.Text = Format((unBilledDesignAmount + totalGSTAmount), "0.00")

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

        txtDesCalculatedPrice.Text = Math.Floor(designCost)
    End Sub


    Private Sub dgPaymentDetails_CurrentCellChanged(sender As Object, e As EventArgs) Handles dgPaymentDetails.CurrentCellChanged

        If btnPaymentConfirmCreatePayment.Visible = True Then
            Return
        End If

        cmbPaymentPaymentNoList.SelectedIndex = dgPaymentDetails.CurrentRowIndex
    End Sub

    Private Sub cmbPaymentPaymentNoList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPaymentPaymentNoList.SelectedIndexChanged

        log.debug("cmbPaymentPaymentNoList_SelectedIndexChanged: cmbPaymentPaymentNoList.SelectedIndex: " + cmbPaymentPaymentNoList.SelectedIndex.ToString)

        If (cmbPaymentPaymentNoList.SelectedIndex = -1) Then
            resetPaymentScreen(False)
            Return
        End If

        Dim paymentNo As Integer = cmbPaymentPaymentNoList.SelectedValue
        'log.Debug("cmbPaymentPaymentNoList_SelectedIndexChanged: cmbPaymentPaymentNoList.SelectedValue  : " + paymentNo.ToString)
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
            'log.Debug("cmbPaymentPaymentNoList_SelectedIndexChanged: No data found for Bill: " + paymentNo.ToString)
        End If
    End Sub

    Private Sub DateTimePicker6_CloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles dpPaymentDate.CloseUp
        'Try
        'refreshgrid3()
        'refreshgrid2()
        Dim lastbildate, seldate As DateTime
        Dim lastbildatestr, seldatestr As String
        actpaid = 0
        dt11.Clear()
        key = cmbPaymentCompanyList.Text.Trim
        seldatestr = dpPaymentDate.Value.ToString("MM dd yyyy")
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
                    txtPaymentDisplayBillNo.Text = dr3.Item(8).ToString + "/" + dr3.Item(1).ToString
                    txtPaymentUnPaidBilledAmount.Text = dr3.Item(7)
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

        dgPaymentDetails.DataSource = ds11.Tables(0)
        Label83.Text = ds11.Tables(0).Rows.Count.ToString
        Label82.Text = actpaid
        'Catch ex As Exception
        'MessageBox.Show("Message to Agni User:   " & ex.Message)
        'End 'Try
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
        If cmbPaymentPaymentNoList.SelectedIndex = -1 Then
            MessageBox.Show("select the payment which you want to delete")
            cmbPaymentPaymentNoList.Focus()
        Else
            Dim custNo As Integer = cmbPaymentCompanyList.SelectedValue
            Dim selectedPaymentNo As Integer = cmbPaymentPaymentNoList.SelectedValue

            Dim lastPaymentRow As DataRow = getLastPaymentRow(custNo)

            'log.Debug("btnPaymentDelete_Click: selectedPaymentNo: " + selectedPaymentNo.ToString + " lastPaymentRow.Item(PaymentNo): " + lastPaymentRow.Item("PaymentNo").ToString)

            If (lastPaymentRow IsNot Nothing AndAlso selectedPaymentNo <> lastPaymentRow.Item("PaymentNo")) Then
                MessageBox.Show("This is not the last payment. You can only delete the last payment")
                Return
            End If

            If MessageBox.Show("Do you want to delete this payment transaction ", "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then

                Dim billNoForPayment As Integer = txtPaymentDisplayBillNo.Text
                Dim amountPaidForPayment As Decimal = txtPaymentFinalPaidAmount.Text

                If deleteSelectedPayment() = True Then
                    reduceBillPaidAmount(billNoForPayment, amountPaidForPayment)
                    MessageBox.Show("payment " + selectedPaymentNo.ToString + " is deleted successfully")
                    resetPaymentScreen()

                    bwtPaymentListLoadThread.RunWorkerAsync(cmbPaymentCompanyList.SelectedValue)
                    bwtPaymentGridLoadThread.RunWorkerAsync(cmbPaymentCompanyList.SelectedValue)
                    bwtBillGridLoadThread.RunWorkerAsync(cmbPaymentCompanyList.SelectedValue)
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
        resetPaymentScreen()
    End Sub

    Private Sub radioReportsBillNo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioReportsBillNo.CheckedChanged
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

    Private Sub radioReportsCompName_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioReportsCompName.CheckedChanged
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
    Private Sub ComboBox9_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbReportDesignList.SelectedIndexChanged
        PictureBox1.Image = Nothing
    End Sub

    Private Sub ComboBox9_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReportDesignList.VisibleChanged
        If cmbReportDesignList.Visible = False Then
            cmbReportDesignList.Items.Clear()
        End If
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
            'Dim item As MyComboitem
            Dim totdesamount As Decimal = 0
            Dim tottransamount As Decimal = 0
            Dim tottransamount1 As Decimal = 0
            Dim billnum As Integer = 0
            Dim countunbilled As Integer = 0
            Dim sumunbilled As Decimal = 0
            Dim countbilled As Integer = 0
            Dim sumbilled As Decimal = 0
            Dim countbill As Integer = 0
            'item = DirectCast(cmbReportDesignList.SelectedItem, MyComboitem)
            'key = item.ID
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
                'Dim item As MyComboitem
                'item = DirectCast(cmbReportDesignList.SelectedItem, MyComboitem)
                'key = item.ID
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


