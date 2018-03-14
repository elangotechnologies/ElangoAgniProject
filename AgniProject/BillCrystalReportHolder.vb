Imports System.Data.SqlClient
Imports System.IO
'Imports NLog
Imports VB = Microsoft.VisualBasic
Imports CrystalDecisions.Shared
Imports System.Threading
Public Class BillReportForm
    'Dim log = LogManager.GetCurrentClassLogger()

    Dim dbConnection As SqlConnection
    Dim billCrystalReport As BillCrystalReport
    Dim billPDFDestPath As String = "E:"

    Private ATTRIBUTE_BILL_PDF_PATH As String = "bill_pdf_dest_path"


    Private Sub BillReportForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        billCrystalReport = New BillCrystalReport

        dbConnection = getDBConnection()

        Dim selectedBillNo As Integer = AgniMainForm.gSelectedBillNo
        Dim selectedCustNo As Integer = AgniMainForm.gSelectedCustNo

        If (selectedBillNo = -1) Then
            Return
        End If

        loadBillDetails(selectedBillNo, selectedCustNo)
        'log.Debug("BillReportForm_Load complete")

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
        customerQuery = New SqlCommand("select CompName, GSTIN, AddressLine1, AddressLine2, AddressLine3, AddressLine4, AddressLine5 from customer where custNo=" + custNo.ToString, dbConnection)
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
        Dim addressLine1 = customerTable.Rows(0).Item("AddressLine1").ToString
        Dim addressLine2 = customerTable.Rows(0).Item("AddressLine2").ToString
        Dim addressLine3 = customerTable.Rows(0).Item("AddressLine3").ToString
        Dim addressLine4 = customerTable.Rows(0).Item("AddressLine4").ToString
        Dim addressLine5 = customerTable.Rows(0).Item("AddressLine5").ToString
        ''MsgBox(compAddress)

        Dim setCustDetailsInReportInvoker As New setCustDetailsInReportDelegate(AddressOf Me.setCustDetailsInReport)
        Me.BeginInvoke(setCustDetailsInReportInvoker, companyName, compGSTIN, addressLine1, addressLine2, addressLine3, addressLine4, addressLine5)
    End Sub

    Delegate Sub setCustDetailsInReportDelegate(companyName As String, compGSTIN As String, addressLine1 As String, addressLine2 As String, addressLine3 As String, addressLine4 As String, addressLine5 As String)

    Sub setCustDetailsInReport(companyName As String, compGSTIN As String, addressLine1 As String, addressLine2 As String, addressLine3 As String, addressLine4 As String, addressLine5 As String)
        billCrystalReport.SetParameterValue("CompName", companyName)
        billCrystalReport.SetParameterValue("CompGSTIN", compGSTIN)
        billCrystalReport.SetParameterValue("CustomerAddressLine1", addressLine1)
        billCrystalReport.SetParameterValue("CustomerAddressLine2", addressLine2)
        billCrystalReport.SetParameterValue("CustomerAddressLine3", addressLine3)
        billCrystalReport.SetParameterValue("CustomerAddressLine4", addressLine4)
        billCrystalReport.SetParameterValue("CustomerAddressLine5", addressLine5)
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

        Dim addressLine1 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_1)
        Dim addressLine2 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_2)
        Dim addressLine3 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_3)
        Dim addressLine4 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_4)
        Dim addressLine5 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_5)

        billCrystalReport.SetParameterValue("AddressLine1", addressLine1)
        billCrystalReport.SetParameterValue("AddressLine2", addressLine2)
        billCrystalReport.SetParameterValue("AddressLine3", addressLine3)
        billCrystalReport.SetParameterValue("AddressLine4", addressLine4)
        billCrystalReport.SetParameterValue("AddressLine5", addressLine5)

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


    Private Sub Agnireport_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ''billCrystalReport.Dispose()
    End Sub
End Class