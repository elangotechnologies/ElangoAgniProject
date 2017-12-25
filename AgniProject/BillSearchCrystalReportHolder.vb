Imports System.Data.SqlClient

Imports System.IO
'''Imports NLog
Imports VB = Microsoft.VisualBasic
Imports CrystalDecisions.Shared
Imports System.Threading

Public Class BillSearchCrystalReportHolder

    Dim dbConnection As SqlConnection
    Dim billSearchCrystalReport As New BillSearchCrystalReport

    Private Sub BillSearchCrystalReportHolder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        setBillSearchDataInReport(AgniMainForm.gBillSearchResultDataSet, AgniMainForm.gReportSearchFilterText)
    End Sub

    Sub setBillSearchDataInReport(searchBillDataSet As DataSet, billSearchFilterText As String)
        billSearchCrystalReport.SetDataSource(searchBillDataSet)
        reportBillSearchReportViewer.ReportSource = billSearchCrystalReport

        billSearchCrystalReport.SetParameterValue("SearchFilterData", billSearchFilterText)

        Dim totalAmount As Decimal = 0
        If searchBillDataSet.Tables(1) IsNot Nothing AndAlso searchBillDataSet.Tables(1).Rows.Count > 0 Then
            totalAmount = searchBillDataSet.Tables(1).Rows(0).Item("TotalBillAmount").ToString
        End If
        billSearchCrystalReport.SetParameterValue("TotalBillAmountInWords", getAmountInWords(totalAmount.ToString))
    End Sub

    Private Sub BillSearchCrystalReportHolder_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        billSearchCrystalReport.Dispose()
    End Sub

End Class

