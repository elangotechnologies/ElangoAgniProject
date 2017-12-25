Imports System.Data.SqlClient

Imports System.IO
'''Imports NLog
Imports VB = Microsoft.VisualBasic
Imports CrystalDecisions.Shared
Imports System.Threading

Public Class GSTCrystalReportHolder

    Dim dbConnection As SqlConnection
    Dim gstCrystalReport As New GSTCrystalReport

    Private Sub GSTReportHolder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        setGSTDataInReport(AgniMainForm.gBillSearchResultDataSet, AgniMainForm.gReportSearchFilterText)
    End Sub

    Sub setGSTDataInReport(searchBillDataSet As DataSet, billSearchFilterText As String)
        searchBillDataSet.Tables(0).TableName = "GSTData"
        searchBillDataSet.Tables(1).TableName = "TotalGSTData"

        gstCrystalReport.SetDataSource(searchBillDataSet)
        reportGSTReportViewer.ReportSource = gstCrystalReport

        gstCrystalReport.SetParameterValue("SearchFilterData", billSearchFilterText)

        Dim totalAmount As Decimal = 0
        If searchBillDataSet.Tables(1) IsNot Nothing AndAlso searchBillDataSet.Tables(1).Rows.Count > 0 Then
            totalAmount = searchBillDataSet.Tables(1).Rows(0).Item("TotalGSTAmount").ToString
        End If
        gstCrystalReport.SetParameterValue("TotalGSTAmountInWords", getAmountInWords(totalAmount.ToString))
    End Sub

    Private Sub GSTReportHolder_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        gstCrystalReport.Dispose()
    End Sub

End Class