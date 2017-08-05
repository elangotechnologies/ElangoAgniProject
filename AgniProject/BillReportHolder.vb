Imports System.Data.SqlClient
Imports System.IO
'Imports NLog
Imports VB = Microsoft.VisualBasic
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Public Class BillReportForm
    'Dim log = LogManager.GetCurrentClassLogger()

    Dim dbConnection As SqlConnection
    Dim cmd1, cmd2 As SqlCommand
    Dim Sda1, Sda2 As SqlDataAdapter
    Dim Ds1, Ds2, ds3, ds4, ds5, ds6 As DataSet
    Dim Dt1, Dt2, dt3 As DataTable
    Dim Dr2, dr3, dr4, dr5, dr6 As DataRow
    Dim Dc2, dc3(3), dc4(10), dc5(3), dc6(6) As DataColumn
    Dim Scb2, scb3 As SqlCommandBuilder
    Dim desdate As Date
    Dim desdatestr As String
    Public valueToConvert As String
    Private n, intpart, realpart, numchar, intword, realword, spltval, spltword As String
    Private flag As Boolean
    Dim billReport As New BillReport

    Public Function getAmountInWords(valueToConvert As String) As String
        n = ""
        intpart = ""
        realpart = ""
        numchar = ""
        intword = ""
        realword = ""
        spltval = ""
        spltword = ""
        Dim amountInWords = ""
        If valueToConvert = "" Then
            Return "None"
        End If

        If valueToConvert = "." Then
            valueToConvert = "0.00"
        End If

        intpart = Format(Int(valueToConvert), "000000000")
        realpart = VB.Right(valueToConvert, 2)

        spltval = realpart
        Call ValFind()
        If spltword <> "" Then realword = spltword
        spltval = Mid(intpart, 1, 2)
        Call ValFind()
        If spltword <> "" Then intword = spltword + "Crore "
        spltval = Mid(intpart, 3, 2)
        Call ValFind()
        If spltword <> "" Then intword = intword + spltword + "Lakh "
        spltval = Mid(intpart, 5, 2)
        Call ValFind()
        If spltword <> "" Then intword = intword + spltword + "Thousand "
        n = Mid(intpart, 7, 1)
        Call ONES()
        If numchar <> "" Then intword = intword + numchar + "Hundred "
        spltval = Mid(intpart, 8, 2)
        If intword <> "" And Val(spltval) > 0 And realword = "" Then intword = intword + "and "
        Call ValFind()
        If spltword <> "" Then intword = intword + spltword
        If intword <> "" And realword <> "" Then amountInWords = intword + "and " + realword + " Paise Only"
        If intword <> "" And realword = "" Then amountInWords = intword + "Only"
        If intword = "" And realword <> "" Then amountInWords = "Paise: " + realword + "Only"

        Return amountInWords
    End Function

    Public Sub ValFind()
        n = ""
        spltword = ""
        If Val(spltval) = 0 Then Exit Sub
        n = VB.Left(spltval, 1)
        Call TENS()
        spltword = numchar
        If flag = False Then n = VB.Right(spltval, 1) : Call ONES() : spltword = spltword + numchar
    End Sub

    Public Sub ONES()
        numchar = ""
        If n = 0 Then numchar = ""
        If n = 1 Then numchar = "One "
        If n = 2 Then numchar = "Two "
        If n = 3 Then numchar = "Three "
        If n = 4 Then numchar = "Four "
        If n = 5 Then numchar = "Five "
        If n = 6 Then numchar = "Six "
        If n = 7 Then numchar = "Seven "
        If n = 8 Then numchar = "Eight "
        If n = 9 Then numchar = "Nine "
    End Sub

    Public Sub TENS()
        numchar = ""
        If n = 1 Then n = VB.Right(spltval, 1) : Call TEENS() : flag = True : Exit Sub Else flag = False
        If n = 0 Then numchar = ""
        If n = 2 Then numchar = "Twenty "
        If n = 3 Then numchar = "Thirty "
        If n = 4 Then numchar = "Fourty "
        If n = 5 Then numchar = "Fifty "
        If n = 6 Then numchar = "Sixty "
        If n = 7 Then numchar = "Seventy "
        If n = 8 Then numchar = "Eighty "
        If n = 9 Then numchar = "Ninety "
    End Sub

    Public Sub TEENS()
        numchar = ""
        If n = 0 Then numchar = "Ten "
        If n = 1 Then numchar = "Eleven "
        If n = 2 Then numchar = "Twelve "
        If n = 3 Then numchar = "Thirteen "
        If n = 4 Then numchar = "Fourteen "
        If n = 5 Then numchar = "Fifteen "
        If n = 6 Then numchar = "Sixteen "
        If n = 7 Then numchar = "Seventeen "
        If n = 8 Then numchar = "Eighteen "
        If n = 9 Then numchar = "Nineten "
    End Sub

    Private Sub Agnireport_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        billReport.Dispose()
    End Sub
    Private Sub BillReportForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Try

        dbConnection = New SqlConnection("server=agni\SQLEXPRESS;Database=agnidatabase;Integrated Security=true")
        dbConnection.Open()

        Dim selectedBillNo As Integer = AgnimainForm.gSelectedBillNo

        If (selectedBillNo = -1) Then
            Return
        End If

        Dim designQuery As SqlCommand
        designQuery = New SqlCommand("select * from design where BillNo=" + selectedBillNo.ToString, dbConnection)
        Dim designAdapter = New SqlDataAdapter()
        designAdapter.SelectCommand = designQuery
        Dim designDataSet = New DataSet
        designAdapter.Fill(designDataSet, "design")
        Dim designTable As DataTable = designDataSet.Tables(0)

        If designTable.Rows.Count = 0 Then
            Return
        End If

        billReport.SetDataSource(designTable)

        reportViewerBillReport.ReportSource = billReport

        Dim selectedCustNo As Integer = designTable.Rows(0).Item("CustNo")
        Dim customerQuery As SqlCommand
        customerQuery = New SqlCommand("select CompName, GSTIN, Address from customer where custNo=" + selectedCustNo.ToString, dbConnection)
        Dim customerAdapter = New SqlDataAdapter()
        customerAdapter.SelectCommand = customerQuery
        Dim customerDataSet = New DataSet
        customerAdapter.Fill(customerDataSet, "customer")
        Dim customerTable As DataTable = customerDataSet.Tables(0)

        If customerTable.Rows.Count = 0 Then
            Return
        End If

        Dim companyName = customerTable.Rows(0).Item("CompName")
        Dim CompGSTIN = customerTable.Rows(0).Item("GSTIN")
        Dim CompAddress = customerTable.Rows(0).Item("Address")

        'billReport.Subreports.Item("BillHeader").SetDataSource(customerTable)
        billReport.SetParameterValue("CompName", customerTable.Rows(0).Item("CompName").ToString)
        billReport.SetParameterValue("CompGSTIN", customerTable.Rows(0).Item("GSTIN").ToString)
        billReport.SetParameterValue("CompAddress", customerTable.Rows(0).Item("Address").ToString)

        Dim invoiceQuery As SqlCommand
        invoiceQuery = New SqlCommand("SELECT TOP 2 * FROM bill where CustNo=" + selectedCustNo.ToString + " ORDER BY BillNo DESC", dbConnection)
        Dim invoiceAdapter = New SqlDataAdapter()
        invoiceAdapter.SelectCommand = invoiceQuery
        Dim invoiceDataSet = New DataSet
        invoiceAdapter.Fill(invoiceDataSet, "bill")
        Dim invoiceTable As DataTable = invoiceDataSet.Tables(0)

        If invoiceTable.Rows.Count = 0 Then
            Return
        End If

        billReport.SetParameterValue("BillNo", invoiceTable.Rows(0).Item("BillNo").ToString)
        billReport.SetParameterValue("BillDate", invoiceTable.Rows(0).Item("BillDate"))
        If (invoiceTable.Rows.Count > 1) Then
            billReport.SetParameterValue("PrevBillNo", invoiceTable.Rows(1).Item("BillNo").ToString)
        Else
            billReport.SetParameterValue("PrevBillNo", "NIL")
        End If

        Dim designAmount As Decimal = AgnimainForm.txtBillingDesignAmoutBeforeGST.Text
        Dim designAmountAfterGST As Decimal = AgnimainForm.txtBillingDesignAmoutAfterGST.Text
        Dim CGST As Decimal = AgnimainForm.txtBillingCGSTPercent.Text
        Dim SGST As Decimal = AgnimainForm.txtBillingSGSTPercent.Text
        Dim IGST As Decimal = AgnimainForm.txtBillingIGSTPercent.Text
        Dim CGSTAmount As Decimal = AgnimainForm.txtBillingCGSTAmount.Text
        Dim SGSTAmount As Decimal = AgnimainForm.txtBillingSGSTAmount.Text
        Dim IGSTAmount As Decimal = AgnimainForm.txtBillingIGSTAmount.Text
        Dim totalGSTAmount As Decimal = AgnimainForm.txtBillingTotalGSTAmount.Text
        Dim prevBalance As Decimal = AgnimainForm.txtBillingPrevBalance.Text
        Dim paidAmountForThisBill As Decimal = AgnimainForm.txtBillingPaidAmount.Text
        Dim netBalance As Decimal = AgnimainForm.txtBillingRemainingBalance.Text

        billReport.SetParameterValue("DesignsAmountBeforeTax", designAmount.ToString)
        billReport.SetParameterValue("CGST", CGST.ToString)
        billReport.SetParameterValue("SGST", SGST.ToString)
        billReport.SetParameterValue("IGST", IGST.ToString)
        billReport.SetParameterValue("CGSTAmount", CGSTAmount.ToString)
        billReport.SetParameterValue("SGSTAmount", SGSTAmount.ToString)
        billReport.SetParameterValue("IGSTAmount", IGSTAmount.ToString)
        billReport.SetParameterValue("TotalGSTTax", totalGSTAmount.ToString)
        billReport.SetParameterValue("DesignsAmountAfterTax", designAmountAfterGST.ToString)

        billReport.SetParameterValue("DesignsAmountBeforeTax", designAmount.ToString)
        billReport.SetParameterValue("DesignsCostInWords", getAmountInWords(designAmountAfterGST.ToString))
        billReport.SetParameterValue("PaidAmountForThisBill", paidAmountForThisBill.ToString)
        billReport.SetParameterValue("PrevBalance", prevBalance.ToString)
        billReport.SetParameterValue("NetBalance", netBalance.ToString)

        '
        'billReport.Subreports.Item("BillHeader").SetDataSource(InvoiceIdentityTable)
        'objRpt.Subreports.Item("BillFooter").SetDataSource(ds6.Tables(0))
        'objRpt.Subreports.Item("Amt2words").SetDataSource(ds6.Tables(0))


        'Catch ex As Exception
        '    MessageBox.Show("message to agni user:    " & ex.Message)
        'End Try
    End Sub

    Private Sub CrystalReportViewer1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub CrystalReportViewer1_Load_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles reportViewerBillReport.Load

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Dim CrExportOptions As ExportOptions
            Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions
            Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions
            Dim fname As String = ""
            Dim billnumber = AgnimainForm.billkey
            'Dim billnumber1 As String = billnumber.Substring(billnumber.IndexOf("/") + 1, billnumber.Length - billnumber.IndexOf("/") - 1)
            'billnumber = billnumber.Substring(0, billnumber.IndexOf("/"))
            'fname = AgnimainForm.pdfdesfolder + "\Bill @" + billnumber + "#" + billnumber1 + " " + AgnimainForm.billcust + ".pdf"
            fname = AgnimainForm.pdfdesfolder + "\Bill_" + billnumber + "_" + AgnimainForm.billcust + ".pdf"
            CrDiskFileDestinationOptions.DiskFileName = fname
            CrExportOptions = billReport.ExportOptions
            With CrExportOptions
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
                .DestinationOptions = CrDiskFileDestinationOptions
                .FormatOptions = CrFormatTypeOptions
            End With
            billReport.Export()
            MsgBox("'" + fname + "' is successfully created")
        Catch ex As Exception
            MessageBox.Show("message to agni user:   " & ex.Message)
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            Dim folder As New FolderBrowserDialog
            If folder.ShowDialog() = Windows.Forms.DialogResult.OK Then
                MsgBox("The PDF file will be saved in '" + folder.SelectedPath.ToString + "\'")
                AgnimainForm.pdfdesfolder = folder.SelectedPath.ToString
            End If
        Catch ex As Exception
            MessageBox.Show("message to agni user:   " & ex.Message)
        End Try
    End Sub
End Class