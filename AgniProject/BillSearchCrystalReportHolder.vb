Imports System.Data.SqlClient

Imports System.IO
'Imports NLog
Imports VB = Microsoft.VisualBasic
Imports CrystalDecisions.Shared
Imports System.Threading

Public Class BillSearchCrystalReportHolder

    Dim billSearchCrystalReport As BillSearchCrystalReport

    Private Sub BillSearchCrystalReportHolder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        billSearchCrystalReport = New BillSearchCrystalReport
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
        Dim addressLine1 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_1)
        Dim addressLine2 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_2)
        Dim addressLine3 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_3)
        Dim addressLine4 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_4)
        Dim addressLine5 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_5)

        billSearchCrystalReport.SetParameterValue("AddressLine1", addressLine1)
        billSearchCrystalReport.SetParameterValue("AddressLine2", addressLine2)
        billSearchCrystalReport.SetParameterValue("AddressLine3", addressLine3)
        billSearchCrystalReport.SetParameterValue("AddressLine4", addressLine4)
        billSearchCrystalReport.SetParameterValue("AddressLine5", addressLine5)

        billSearchCrystalReport.SetParameterValue("TotalBillAmountInWords", getAmountInWords(totalAmount.ToString))
    End Sub

    Private Sub BillSearchCrystalReportHolder_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        billSearchCrystalReport.Dispose()
    End Sub

End Class

