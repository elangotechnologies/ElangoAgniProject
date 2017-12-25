
Imports System.Data.SqlClient
'Imports NLog
Imports VB = Microsoft.VisualBasic

Public Class PaymentSearchCrystalReportHolder

    Dim dbConnection As SqlConnection
    Dim paymentSearchCrystalReport As New PaymentSearchCrystalReport

    Private Sub PaymentSearchCrystalReportHolder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        setPaymentDataInReport(AgniMainForm.gPaymentSearchResultDataSet, AgniMainForm.gReportSearchFilterText)
    End Sub

    Private Sub paymentSearchReportViewer_Load(sender As Object, e As EventArgs) Handles paymentSearchReportViewer.Load

    End Sub

    Sub setPaymentDataInReport(paymentSearchDataSet As DataSet, reportSearchFilterText As String)

        paymentSearchCrystalReport.SetDataSource(paymentSearchDataSet)
        paymentSearchReportViewer.ReportSource = paymentSearchCrystalReport

        paymentSearchCrystalReport.SetParameterValue("SearchFilterData", reportSearchFilterText)

        Dim totalAmount As Decimal = 0
        If paymentSearchDataSet.Tables(1) IsNot Nothing AndAlso paymentSearchDataSet.Tables(1).Rows.Count > 0 Then
            totalAmount = paymentSearchDataSet.Tables(1).Rows(0).Item("TotalFinalPaidAmount").ToString
        End If
        paymentSearchCrystalReport.SetParameterValue("TotalFinalPaidAmountInWords", getAmountInWords(totalAmount.ToString))
    End Sub

    Private Sub GSTReportHolder_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        paymentSearchCrystalReport.Dispose()
    End Sub

End Class
