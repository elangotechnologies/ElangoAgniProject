Imports System.Data.SqlClient

Imports System.IO
'Imports NLog
Imports VB = Microsoft.VisualBasic
Imports CrystalDecisions.Shared
Imports System.Threading

Public Class GSTCrystalReportHolder

    Dim gstCrystalReport As GSTCrystalReport

    Private Sub GSTReportHolder_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        gstCrystalReport = New GSTCrystalReport
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

        Dim addressLine1 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_1)
        Dim addressLine2 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_2)
        Dim addressLine3 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_3)
        Dim addressLine4 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_4)
        Dim addressLine5 As String = getAttribute(ATTRIBUTE_ADDRESS_LINE_5)

        gstCrystalReport.SetParameterValue("AddressLine1", addressLine1)
        gstCrystalReport.SetParameterValue("AddressLine2", addressLine2)
        gstCrystalReport.SetParameterValue("AddressLine3", addressLine3)
        gstCrystalReport.SetParameterValue("AddressLine4", addressLine4)
        gstCrystalReport.SetParameterValue("AddressLine5", addressLine5)

        gstCrystalReport.SetParameterValue("TotalGSTAmountInWords", getAmountInWords(totalAmount.ToString))
    End Sub

    Private Sub GSTReportHolder_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        gstCrystalReport.Dispose()
    End Sub

End Class