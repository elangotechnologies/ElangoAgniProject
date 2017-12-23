Imports System.Data.SqlClient
Imports System.IO
'Imports NLog
Imports VB = Microsoft.VisualBasic
Imports CrystalDecisions.Shared
Imports System.Threading
Public Class BillReportForm
    'Dim log = LogManager.GetCurrentClassLogger()

    Dim dbConnection As SqlConnection
    Dim billCrystalReport As New BillCrystalReport
    Dim billPDFDestPath As String = "E:"

    Private ATTRIBUTE_BILL_PDF_PATH As String = "bill_pdf_dest_path"

    Public valueToConvert As String
    Private n, intpart, realpart, numchar, intword, realword, spltval, spltword As String
    Private flag As Boolean


    Private Sub BillReportForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dbConnection = New SqlConnection("server=agni\SQLEXPRESS;Database=agnidatabase;Integrated Security=true; MultipleActiveResultSets=True;")
        dbConnection.Open()

        Dim selectedBillNo As Integer = AgniMainForm.gSelectedBillNo
        Dim selectedCustNo As Integer = AgniMainForm.gSelectedCustNo

        If (selectedBillNo = -1) Then
            Return
        End If

        loadBillDetails(selectedBillNo, selectedCustNo)
        'log.debug("BillReportForm_Load complete")

    End Sub
    Class SearchData
        Public custNo, billNo As Integer
        Sub New()
            custNo = -1
            billNo = -1
        End Sub
    End Class

    Sub loadBillDetails(billNo As Integer, custNo As Integer)
        Dim thread As Thread = New Thread(AddressOf fetchAndSetCompleteBillDetails)
        thread.IsBackground = True
        Dim searchData As SearchData = New SearchData
        searchData.custNo = custNo
        searchData.billNo = billNo
        thread.Start(searchData)
    End Sub

    Sub fetchAndSetCompleteBillDetails(ByVal searchDataParam As Object)
        Dim searchData As SearchData = CType(searchDataParam, SearchData)
        Dim billNo As Integer = searchData.billNo
        Dim custNo As Integer = searchData.custNo

        fetchAndSetDesignDetails(billNo)
        fetchAndSetCustomerDetails(custNo)
        fetchAndSetBillDetails(billNo, custNo)

    End Sub

    Sub fetchAndSetDesignDetails(billNo As Integer)
        Dim designQuery As SqlCommand
        designQuery = New SqlCommand("select * from design where BillNo=" + billNo.ToString, dbConnection)
        Dim designAdapter = New SqlDataAdapter()
        designAdapter.SelectCommand = designQuery
        Dim designDataSet = New DataSet
        designAdapter.Fill(designDataSet, "design")
        Dim designTable As DataTable = designDataSet.Tables(0)

        If designTable.Rows.Count = 0 Then
            Return
        End If

        Dim setDesignsInReportInvoker As New setDesignsInReportDelegate(AddressOf Me.setDesignsInReport)
        Me.Invoke(setDesignsInReportInvoker, designTable)
    End Sub

    Delegate Sub setDesignsInReportDelegate(designTable As DataTable)

    Sub setDesignsInReport(designTable As DataTable)
        billCrystalReport.SetDataSource(designTable)
        reportViewerBillReport.ReportSource = billCrystalReport
    End Sub

    Sub fetchAndSetCustomerDetails(custNo As Integer)

        Dim customerQuery As SqlCommand
        customerQuery = New SqlCommand("select CompName, GSTIN, Address from customer where custNo=" + custNo.ToString, dbConnection)
        Dim customerAdapter = New SqlDataAdapter()
        customerAdapter.SelectCommand = customerQuery
        Dim customerDataSet = New DataSet
        customerAdapter.Fill(customerDataSet, "customer")
        Dim customerTable As DataTable = customerDataSet.Tables(0)

        If customerTable.Rows.Count = 0 Then
            Return
        End If

        Dim companyName = customerTable.Rows(0).Item("CompName").ToString
        Dim compGSTIN = customerTable.Rows(0).Item("GSTIN").ToString
        Dim compAddress = customerTable.Rows(0).Item("Address").ToString

        Dim setCustDetailsInReportInvoker As New setCustDetailsInReportDelegate(AddressOf Me.setCustDetailsInReport)
        Me.BeginInvoke(setCustDetailsInReportInvoker, companyName, compGSTIN, compAddress)
    End Sub

    Delegate Sub setCustDetailsInReportDelegate(companyName As String, compGSTIN As String, compAddress As String)

    Sub setCustDetailsInReport(companyName As String, compGSTIN As String, compAddress As String)
        billCrystalReport.SetParameterValue("CompName", companyName)
        billCrystalReport.SetParameterValue("CompGSTIN", compGSTIN)
        billCrystalReport.SetParameterValue("CompAddress", compAddress)
    End Sub

    Sub fetchAndSetBillDetails(billNo As Integer, custNo As Integer)

        Dim billQuery As SqlCommand = New SqlCommand("SELECT TOP 2 * FROM bill where CustNo=" + custNo.ToString + "  and billNo <= " + billNo.ToString + " ORDER BY BillNo DESC", dbConnection)
        Dim billAdapter = New SqlDataAdapter()
        billAdapter.SelectCommand = billQuery
        Dim billDataSet = New DataSet
        billAdapter.Fill(billDataSet, "bill")
        Dim billTable As DataTable = billDataSet.Tables(0)

        If billTable.Rows.Count = 0 Then
            Return
        End If

        Dim setBillDetailsInReportInvoker As New setBillDetailsInReportDelegate(AddressOf Me.setBillDetailsInReport)
        Me.BeginInvoke(setBillDetailsInReportInvoker, billTable)
    End Sub

    Delegate Sub setBillDetailsInReportDelegate(billTable As DataTable)

    Sub setBillDetailsInReport(billTable As DataTable)

        billCrystalReport.SetParameterValue("BillNo", billTable.Rows(0).Item("DisplayBillNo").ToString)
        billCrystalReport.SetParameterValue("BillDate", billTable.Rows(0).Item("BillDate"))
        If (billTable.Rows.Count > 1) Then
            billCrystalReport.SetParameterValue("PrevBillNo", billTable.Rows(1).Item("DisplayBillNo").ToString)
        Else
            billCrystalReport.SetParameterValue("PrevBillNo", "None")
        End If

        Dim designAmount As Decimal = AgniMainForm.txtBillingDesignAmoutBeforeGST.Text
        Dim designAmountAfterGST As Decimal = AgniMainForm.txtBillingDesignAmoutAfterGST.Text
        Dim CGST As Decimal = AgniMainForm.txtBillingCGSTPercent.Text
        Dim SGST As Decimal = AgniMainForm.txtBillingSGSTPercent.Text
        Dim IGST As Decimal = AgniMainForm.txtBillingIGSTPercent.Text
        Dim CGSTAmount As Decimal = AgniMainForm.txtBillingCGSTAmount.Text
        Dim SGSTAmount As Decimal = AgniMainForm.txtBillingSGSTAmount.Text
        Dim IGSTAmount As Decimal = AgniMainForm.txtBillingIGSTAmount.Text
        Dim totalGSTAmount As Decimal = AgniMainForm.txtBillingTotalGSTAmount.Text
        ''MsgBox(totalGSTAmount)
        Dim prevBalance As Decimal = AgniMainForm.txtBillingPrevBalance.Text
        Dim paidAmountForThisBill As Decimal = AgniMainForm.txtBillingPaidAmount.Text
        Dim netBalance As Decimal = AgniMainForm.txtBillingRemainingBalance.Text
        Dim cancelledBill As String = If(billTable.Rows(0).Item("Cancelled") = 1, "This is a cancelled bill", "")

        billCrystalReport.SetParameterValue("DesignsAmountBeforeTax", Format(Math.Round(designAmount), "0.00"))
        billCrystalReport.SetParameterValue("CGST", CGST.ToString)
        billCrystalReport.SetParameterValue("SGST", SGST.ToString)
        billCrystalReport.SetParameterValue("IGST", IGST.ToString)
        billCrystalReport.SetParameterValue("CGSTAmount", CGSTAmount.ToString)
        billCrystalReport.SetParameterValue("SGSTAmount", SGSTAmount.ToString)
        billCrystalReport.SetParameterValue("IGSTAmount", IGSTAmount.ToString)
        billCrystalReport.SetParameterValue("TotalGSTTax", Format(Math.Round(totalGSTAmount), "0.00"))
        billCrystalReport.SetParameterValue("DesignsAmountAfterTax", Format(Math.Round(designAmountAfterGST), "0.00"))
        billCrystalReport.SetParameterValue("DesignsCostInWords", getAmountInWords(designAmountAfterGST.ToString))
        billCrystalReport.SetParameterValue("PaidAmountForThisBill", Format(Math.Round(paidAmountForThisBill), "0.00"))
        billCrystalReport.SetParameterValue("PrevBalance", Format(Math.Round(prevBalance), "0.00"))
        billCrystalReport.SetParameterValue("NetBalance", Format(Math.Round(netBalance), "0.00"))
        billCrystalReport.SetParameterValue("CancelledBill", cancelledBill)
    End Sub

    Function getAttribute(attributeName As String) As String
        Dim attributeQuery = New SqlCommand("select * from attributes where AttributeName='" + attributeName + "'", dbConnection)
        Dim attributeAdapter = New SqlDataAdapter()
        attributeAdapter.SelectCommand = attributeQuery
        Dim attributeDataSet = New DataSet
        attributeAdapter.Fill(attributeDataSet, "attributes")
        Dim attributeTable As DataTable = attributeDataSet.Tables(0)

        If attributeTable.Rows.Count > 0 Then
            Return attributeTable.Rows(0).Item("AttributeValue")
        End If
        Return Nothing
    End Function

    Public Sub insertOrReplaceAttribute(attributeName As String, attributeValue As String)

        Dim query As String = String.Empty
        query &= "begin tran
           update attributes with (serializable) set AttributeValue =  @attributeValue
           where AttributeName = @attributeName
           if @@rowcount = 0
           begin
              insert into attributes (AttributeName, AttributeValue) values (@attributeName, @attributeValue)
           end
        commit tran"

        Using comm As New SqlCommand()
            With comm
                .Connection = dbConnection
                .CommandType = CommandType.Text
                .CommandText = query
                .Parameters.AddWithValue("@attributeName", attributeName)
                .Parameters.AddWithValue("@attributeValue", attributeValue)
            End With
            comm.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub btnBillReportExportPdf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBillReportExportPdf.Click

        Dim CrExportOptions As ExportOptions
        Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions
        Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions
        Dim billnumber = AgniMainForm.gSelectedBillNo
        Dim displayBillnumber = AgniMainForm.gSelectedDisplayBillNo
        Dim custName = AgniMainForm.gSelectedCustName
        Dim pdfBillDestFolder As String = getAttribute(ATTRIBUTE_BILL_PDF_PATH)
        If pdfBillDestFolder Is Nothing Then
            pdfBillDestFolder = "E:"
        End If
        Dim fileName As String = pdfBillDestFolder + "\Bill_" + displayBillnumber + "_" + custName + ".pdf"
        CrDiskFileDestinationOptions.DiskFileName = fileName
        CrExportOptions = billCrystalReport.ExportOptions
        With CrExportOptions
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.PortableDocFormat
            .DestinationOptions = CrDiskFileDestinationOptions
            .FormatOptions = CrFormatTypeOptions
        End With
        billCrystalReport.Export()
        MsgBox("This bill is saved as '" + fileName + "'")

    End Sub



    Private Sub btnBillReportPDFPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBillReportPDFPath.Click
        Try
            Dim folderDialog As New FolderBrowserDialog
            If folderDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then

                Dim billPdfPath As String = folderDialog.SelectedPath.ToString
                insertOrReplaceAttribute(ATTRIBUTE_BILL_PDF_PATH, billPdfPath)
                MsgBox("Hereafter the exported bill pdf files will be saved in '" + billPdfPath + "\' directory. If you wish you can change this path again")
            End If

        Catch ex As Exception
            MessageBox.Show("message to agni user:   " & ex.Message)
        End Try
    End Sub


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
        billCrystalReport.Dispose()
    End Sub
End Class