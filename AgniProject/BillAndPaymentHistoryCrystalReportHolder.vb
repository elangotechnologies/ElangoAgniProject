
Imports System.Data.SqlClient
'Imports NLog

Public Class BillAndPaymentHistoryCrystalReportHolder

    Dim billAndPaymentHistoryCrystalReport As BillAndPaymentHistoryCrystalReport

    Private Sub BillAndPaymentHistoryReportViewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        billAndPaymentHistoryCrystalReport = New BillAndPaymentHistoryCrystalReport
        setBillAndPaymentHistoryDataInReport(AgniMainForm.gReportSearchFilterText)
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

        billAndPaymentHistoryCrystalReport.SetParameterValue("SearchFilterData", reportSearchFilterText)

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

        billAndPaymentHistoryCrystalReport.SetParameterValue("TotalBalanceAmountInWords", getAmountInWords(totalAmount.ToString))

    End Sub

    Private Sub GSTReportHolder_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        billAndPaymentHistoryCrystalReport.Dispose()
    End Sub

End Class