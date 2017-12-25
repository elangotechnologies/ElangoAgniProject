
Imports System.Data.SqlClient
'Imports NLog

Public Class BillAndPaymentHistoryCrystalReportHolder

    Dim dbConnection As SqlConnection
    Dim billAndPaymentHistoryCrystalReport As New BillAndPaymentHistoryCrystalReport

    Private Sub BillAndPaymentHistoryReportViewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
            totalAmount = billAndPaymentHistoryDataSet.Tables(1).Rows(0).Item("TotalPaidAmount").ToString
        End If
        billAndPaymentHistoryCrystalReport.SetParameterValue("TotalFinalPaidAmountInWords", getAmountInWords(totalAmount.ToString))

    End Sub

    Private Sub GSTReportHolder_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        billAndPaymentHistoryCrystalReport.Dispose()
    End Sub

End Class