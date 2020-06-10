
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
'Imports NLog

Public Class BillAndPaymentHistoryCrystalReportHolder

    Dim dbConnection As SqlConnection

    Dim billAndPaymentHistoryCrystalReport As ReportClass

    Private Sub BillAndPaymentHistoryReportViewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dbConnection = getDBConnection()

        If (AgniMainForm.gSearchFilter And SEARCH_BY_CUSTOMER) <> 0 Then
            billAndPaymentHistoryCrystalReport = New BillAndPaymentHistoryPerCustomerCrystalReport
        Else
            billAndPaymentHistoryCrystalReport = New BillAndPaymentHistoryCrystalReport
        End If
        setBillAndPaymentHistoryDataInReport(AgniMainForm.gReportSearchFilterText)

        If (AgniMainForm.gSearchFilter And SEARCH_BY_CUSTOMER) <> 0 Then
            fetchAndSetCustomerDetails(AgniMainForm.gSelectedCustNo)
        End If

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

        Dim setCustDetailsInReportInvoker As New setCustDetailsInReportDelegate(AddressOf Me.setCustDetailsInReport)
        Me.BeginInvoke(setCustDetailsInReportInvoker, companyName, compGSTIN, addressLine1, addressLine2, addressLine3, addressLine4, addressLine5)
    End Sub

    Delegate Sub setCustDetailsInReportDelegate(companyName As String, compGSTIN As String, addressLine1 As String, addressLine2 As String, addressLine3 As String, addressLine4 As String, addressLine5 As String)

    Sub setCustDetailsInReport(companyName As String, compGSTIN As String, addressLine1 As String, addressLine2 As String, addressLine3 As String, addressLine4 As String, addressLine5 As String)
        billAndPaymentHistoryCrystalReport.SetParameterValue("CompName", companyName)
        billAndPaymentHistoryCrystalReport.SetParameterValue("CompGSTIN", compGSTIN)
        billAndPaymentHistoryCrystalReport.SetParameterValue("CustomerAddressLine1", addressLine1)
        billAndPaymentHistoryCrystalReport.SetParameterValue("CustomerAddressLine2", addressLine2)
        billAndPaymentHistoryCrystalReport.SetParameterValue("CustomerAddressLine3", addressLine3)
        billAndPaymentHistoryCrystalReport.SetParameterValue("CustomerAddressLine4", addressLine4)
        billAndPaymentHistoryCrystalReport.SetParameterValue("CustomerAddressLine5", addressLine5)
    End Sub

    Sub setBillAndPaymentHistoryDataInReport(reportSearchFilterText As String)
        Dim billAndPaymentHistoryDataSet As DataSet = AgniMainForm.gBillAndPaymentHistoryDataSet

        If (billAndPaymentHistoryDataSet.Tables.Count > 1) Then
            billAndPaymentHistoryDataSet.Tables.Remove(billAndPaymentHistoryDataSet.Tables(1).TableName)
        End If

        billAndPaymentHistoryDataSet.Tables.Add(AgniMainForm.gBillSearchResultDataSet.Tables(1).Copy)
        billAndPaymentHistoryDataSet.Tables(1).TableName = "BillAndPaymentHistorySummary"

        billAndPaymentHistoryCrystalReport.SetDataSource(billAndPaymentHistoryDataSet)
        BillAndPaymentHistoryReportViewer.ReportSource = billAndPaymentHistoryCrystalReport

        Dim totalAmount As Decimal = 0
        If billAndPaymentHistoryDataSet.Tables(1) IsNot Nothing AndAlso billAndPaymentHistoryDataSet.Tables(1).Rows.Count > 0 Then
            totalAmount = billAndPaymentHistoryDataSet.Tables(1).Rows(0).Item("TotalNetBalance").ToString
        End If

        Dim addressLine1 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_1)
        Dim addressLine2 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_2)
        Dim addressLine3 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_3)
        Dim addressLine4 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_4)
        Dim addressLine5 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_5)

        billAndPaymentHistoryCrystalReport.SetParameterValue("AddressLine1", addressLine1)
        billAndPaymentHistoryCrystalReport.SetParameterValue("AddressLine2", addressLine2)
        billAndPaymentHistoryCrystalReport.SetParameterValue("AddressLine3", addressLine3)
        billAndPaymentHistoryCrystalReport.SetParameterValue("AddressLine4", addressLine4)
        billAndPaymentHistoryCrystalReport.SetParameterValue("AddressLine5", addressLine5)

        If (AgniMainForm.gSearchFilter And SEARCH_BY_CUSTOMER) <> 0 Then
            If (AgniMainForm.gSearchFilter And SEARCH_BY_DATE_RANGE) <> 0 Then
                billAndPaymentHistoryCrystalReport.SetParameterValue("FromDate", AgniMainForm.gSearchFromDate.ToString)
                billAndPaymentHistoryCrystalReport.SetParameterValue("ToDate", AgniMainForm.gSearchToDate.ToString)
            Else
                Dim billAndPaymentHistory As DataTable = billAndPaymentHistoryDataSet.Tables(0)
                Dim rowCount As Integer = billAndPaymentHistory.Rows.Count
                If rowCount > 0 Then
                    Dim firstRow As DataRow = billAndPaymentHistory.Rows(0)
                    Dim lastRow As DataRow = billAndPaymentHistory.Rows(rowCount - 1)

                    Dim fromDate As String = firstRow.Item("Date").ToString
                    Dim toDate As String = lastRow.Item("Date").ToString

                    billAndPaymentHistoryCrystalReport.SetParameterValue("FromDate", fromDate)
                    billAndPaymentHistoryCrystalReport.SetParameterValue("ToDate", toDate)

                End If
            End If
        Else
            billAndPaymentHistoryCrystalReport.SetParameterValue("SearchFilterData", reportSearchFilterText)
        End If

        billAndPaymentHistoryCrystalReport.SetParameterValue("TotalBalanceAmountInWords", getAmountInWords(totalAmount.ToString))

    End Sub

    Private Sub GSTReportHolder_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        billAndPaymentHistoryCrystalReport.Dispose()
    End Sub

End Class