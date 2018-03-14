
Imports System.Data.SqlClient
'Imports NLog
Imports VB = Microsoft.VisualBasic

Public Class PaymentSearchCrystalReportHolder

    Dim paymentSearchCrystalReport As PaymentSearchCrystalReport

    Private Sub PaymentSearchCrystalReportHolder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        paymentSearchCrystalReport = New PaymentSearchCrystalReport

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

        Dim addressLine1 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_1)
        Dim addressLine2 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_2)
        Dim addressLine3 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_3)
        Dim addressLine4 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_4)
        Dim addressLine5 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_5)

        paymentSearchCrystalReport.SetParameterValue("AddressLine1", addressLine1)
        paymentSearchCrystalReport.SetParameterValue("AddressLine2", addressLine2)
        paymentSearchCrystalReport.SetParameterValue("AddressLine3", addressLine3)
        paymentSearchCrystalReport.SetParameterValue("AddressLine4", addressLine4)
        paymentSearchCrystalReport.SetParameterValue("AddressLine5", addressLine5)

        paymentSearchCrystalReport.SetParameterValue("TotalFinalPaidAmountInWords", getAmountInWords(totalAmount.ToString))
    End Sub

    Private Sub GSTReportHolder_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        paymentSearchCrystalReport.Dispose()
    End Sub

End Class
